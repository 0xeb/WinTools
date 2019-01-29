// Windows Events Providers Explorer / Command line
// By Elias Bachaalany <lallousz-x86@yahoo.com>
// Based on the sample code from: https://msdn.microsoft.com/en-us/library/windows/desktop/dd996925(v=vs.85).aspx
//

/* -----------------------------------------------------------------------------
* Copyright (c) Elias Bachaalany <lallousz-x86@yahoo.com>
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions
* are met:
* 1. Redistributions of source code must retain the above copyright
*    notice, this list of conditions and the following disclaimer.
* 2. Redistributions in binary form must reproduce the above copyright
*    notice, this list of conditions and the following disclaimer in the
*    documentation and/or other materials provided with the distribution.
*
* THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
* ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
* FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
* OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
* HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
* SUCH DAMAGE.
* -----------------------------------------------------------------------------
*/
#include "stdafx.h"

//--------------------------------------------------------------------------
namespace std_utils
{
    // http://stackoverflow.com/questions/1494399/how-do-i-search-find-and-replace-in-a-standard-string
    static void str_replace(
        std::wstring &str,
        const std::wstring &oldStr,
        const std::wstring &newStr)
    {
        size_t pos = 0;
        while ((pos = str.find(oldStr, pos)) != std::wstring::npos)
        {
            str.replace(pos, oldStr.length(), newStr);
            pos += newStr.length();
        }
    }

    static void html_entities(std::wstring &str)
    {
        static std::wstring tbl[] =
        {
            L"&", L"&amp;",
            L"<", L"&lt;",
            L">", L"&gt;"
        };

        for (size_t i = 0; i < _countof(tbl); i += 2)
            str_replace(str, tbl[i], tbl[i + 1]);
    }
}

//--------------------------------------------------------------------------
// XML constants begin
static LPCWSTR XML_PROVIDERS = L"Providers";
static LPCWSTR XML_PROVIDER = L"Provider";
static LPCWSTR XML_NAME = L"Name";
static LPCWSTR XML_GUID = L"Guid";
static LPCWSTR XML_METADATA = L"Metadata";
static LPCWSTR XML_EVENT_METADATA = L"EventMetadata";
static LPCWSTR XML_RESOURCEFILEPATH = L"ResourceFilePath";
static LPCWSTR XML_PARAMETERFILEPATH = L"ParameterFilePath";
static LPCWSTR XML_PUBLISHER_MESSAGE = L"PublisherMessage";
static LPCWSTR XML_MESSAGEFILEPATH = L"MessageFilePath";
static LPCWSTR XML_HELPLINK = L"HelpLink";
static LPCWSTR XML_CHANNEL = L"Channel";
static LPCWSTR XML_CHANNELS = L"Channels";
static LPCWSTR XML_MESSAGE = L"Message";
static LPCWSTR XML_INDEX = L"Index";
static LPCWSTR XML_ID = L"Id";
static LPCWSTR XML_PATH = L"Path";
static LPCWSTR XML_IMPORTED = L"Imported";
static LPCWSTR XML_TASKS = L"Tasks";
static LPCWSTR XML_LEVELS = L"Levels";
static LPCWSTR XML_LEVEL = L"Level";
static LPCWSTR XML_OPCODES = L"Opcodes";
static LPCWSTR XML_OPCODE = L"Opcode";
static LPCWSTR XML_VALUE = L"Value";
static LPCWSTR XML_VERSION = L"Version";
static LPCWSTR XML_KEYWORDS = L"Keywords";
static LPCWSTR XML_KEYWORD = L"Keyword";
static LPCWSTR XML_EVENT = L"Event";
static LPCWSTR XML_TEMPLATE = L"Template";
static LPCWSTR XML_TASK = L"Task";
// XML constants end

static LPCWSTR OPT_NAME = L"/name";
static LPCWSTR STR_NONE = L"None";
static LPCWSTR OPT_OUT = L"/out";
static LPCWSTR OPT_META = L"/meta";
static LPCWSTR OPT_EVENTMETA = L"/eventmeta";
static LPCWSTR OPT_FORCE = L"/force";

//--------------------------------------------------------------------------
struct COptions
{
    LPWSTR ProviderFilter;
    bool bGetMeta;
    bool bGetEventMeta;
    bool bForce;
    LPWSTR OutFileName;

    COptions(): bGetMeta(false), OutFileName(nullptr), bForce(false),
                ProviderFilter(nullptr), bGetEventMeta(false)
    {
    }
};

//--------------------------------------------------------------------------
struct CXmlOutput
{
    size_t m_nLevel;
    std::wostream &m_osOut;

    CXmlOutput(std::wostream &Out) : m_nLevel(0), m_osOut(Out)
    {
    }

    void Tabs()
    {
        for (size_t i = 0; i < m_nLevel; ++i)
            m_osOut << L"    ";
    }

    void OpenTag(LPCWSTR TagName)
    {
        Tabs();
        m_osOut << L"<" << TagName << L">" << std::endl;
        ++m_nLevel;
    }

    void CloseTag(LPCWSTR TagName)
    {
        --m_nLevel;
        Tabs();
        m_osOut << L"</" << TagName << L">" << std::endl;
    }

    void TabText(LPCWSTR Txt)
    {
        Tabs();
        m_osOut << Txt << std::endl;
    }

    void TagText(
        LPCWSTR TagName,
        LPCWSTR Text)
    {
        Tabs();
        std::wstring s(Text);
        std_utils::html_entities(s);
        m_osOut << L"<" << TagName << L">" 
                << s.c_str() 
                << L"</" << TagName << L">" 
                << std::endl;
    }

    void TagCData(
        LPCWSTR TagName,
        LPCWSTR Text)
    {
        Tabs();
        m_osOut << L"<" << TagName << L"><![CDATA[" << std::endl << Text << L"]]></" << TagName << L">" << std::endl;
    }
};

//--------------------------------------------------------------------------
class CProvidersExplorer
{
private:
    // Contains the value and message string for a type, such as
    // an opcode or task that the provider defines or uses. If the
    // type does not specify a message string, the message member
    // contains the value of the type's name attribute.
    typedef struct _msgstring
    {
        union
        {
            DWORD dwValue;  // Value attribute for opcode, task, and level
            UINT64 ullMask; // Mask attribute for keyword
        };
        LPWSTR pwcsMessage; // Message string or name attribute
    } MSG_STRING, *PMSG_STRING;

    // Header for the block of value/message string pairs. The dwSize member
    // is the size, in bytes, of the block of MSG_STRING structures to which 
    // the pMessage member points.
    typedef struct _messages
    {
        DWORD dwSize;
        PMSG_STRING pMessage;
    } MESSAGES, *PMESSAGES;

    // Global variables.
    EVT_HANDLE m_hMetadata;
    MESSAGES m_ChannelMessages;
    MESSAGES m_LevelMessages;
    MESSAGES m_TaskMessages;
    MESSAGES m_OpcodeMessages;
    MESSAGES m_KeywordMessages;

    CXmlOutput *m_Out;
    COptions *m_Opt;

    void DumpPublisherMetadata(LPWSTR pwszPublisherName);
    DWORD DumpProviderEvents(EVT_HANDLE hMetadata);
    DWORD DumpEventProperties(EVT_HANDLE hEvent);
    DWORD DumpEventProperty(
        EVT_HANDLE hMetadata,
        int Id,
        PEVT_VARIANT pProperty);
    DWORD DumpProviderProperties(
        EVT_HANDLE hMetadata,
        DWORD PropIdFilter = -1);
    static DWORD GetMetadataProperty(
        EVT_HANDLE hMetadata,
        EVT_PUBLISHER_METADATA_PROPERTY_ID Id,
        PEVT_VARIANT *ppProperty);

    DWORD DumpProviderProperty(
        EVT_HANDLE hMetadata, 
        int Id, 
        PEVT_VARIANT pProperty);
    DWORD DumpChannelProperties(
        EVT_HANDLE hChannels, 
        DWORD dwIndex, 
        PMESSAGES pMessages);
    DWORD DumpLevelProperties(
        EVT_HANDLE hLevels, 
        DWORD dwIndex, 
        PMESSAGES pMessages);
    DWORD DumpTaskProperties(
        EVT_HANDLE hTasks, 
        DWORD dwIndex, 
        PMESSAGES pMessages);
    DWORD DumpOpcodeProperties(
        EVT_HANDLE hOpcodes, 
        DWORD dwIndex, 
        PMESSAGES pMessages);
    DWORD DumpKeywordProperties(
        EVT_HANDLE hKeywords, 
        DWORD dwIndex, 
        PMESSAGES pMessages);
    LPWSTR GetMessageString(
        EVT_HANDLE hMetadata,
        DWORD dwMessageId);
    PEVT_VARIANT GetProperty(
        EVT_HANDLE handle, 
        DWORD dwIndex, 
        EVT_PUBLISHER_METADATA_PROPERTY_ID PropertyId);
    LPWSTR GetPropertyName(
        EVT_HANDLE hMetadata,
        PMESSAGES pMessages,
        DWORD dwValue);
    LPWSTR GetOpcodeName(
        EVT_HANDLE hMetadata,
        PMESSAGES pMessages,
        DWORD dwOpcodeValue,
        DWORD dwTaskValue);
    LPWSTR GetKeywordName(
        EVT_HANDLE hMetadata,
        PMESSAGES pMessages,
        UINT64 ullKeyword);

    void FreeMessages(PMESSAGES pMessages);
    void CleanMetadataWorkVars();
public:
    int Dump();
    CProvidersExplorer(
        COptions *opt,
        CXmlOutput *out) : m_Opt(opt), m_Out(out)
    {
    }
};

//--------------------------------------------------------------------------
void CProvidersExplorer::CleanMetadataWorkVars()
{
    m_hMetadata = NULL;
    memset(&m_ChannelMessages, 0, sizeof(m_ChannelMessages));
    memset(&m_LevelMessages, 0, sizeof(m_LevelMessages));
    memset(&m_TaskMessages, 0, sizeof(m_TaskMessages));
    memset(&m_OpcodeMessages, 0, sizeof(m_OpcodeMessages));
    memset(&m_KeywordMessages, 0, sizeof(m_KeywordMessages));
}

//--------------------------------------------------------------------------
void CProvidersExplorer::DumpPublisherMetadata(LPWSTR pwszPublisherName)
{
    CleanMetadataWorkVars();

    DWORD status = ERROR_SUCCESS;

    // Get a handle to the provider's metadata. You can specify the provider's name
    // if the provider is registered on the computer or the full path to an archived 
    // log file (archived log files contain the provider's metadata). Specify the locale 
    // identifier if you want the localized strings returned in a locale other than
    // the locale of the current thread.
    m_hMetadata = EvtOpenPublisherMetadata(
        NULL,
        pwszPublisherName,
        NULL,
        0,
        0);

    if (m_hMetadata == NULL)
    {
        wprintf(L"ERROR: EvtOpenPublisherMetadata failed with %d\n", GetLastError());
        goto cleanup;
    }

    m_Out->OpenTag(XML_METADATA);
    status = DumpProviderProperties(
        m_hMetadata,
        m_Opt->bGetMeta ? -1 : EvtPublisherMetadataPublisherGuid);

    m_Out->CloseTag(XML_METADATA);

    if (m_Opt->bGetEventMeta)
    {
        m_Out->OpenTag(XML_EVENT_METADATA);
        status = DumpProviderEvents(m_hMetadata);
        m_Out->CloseTag(XML_EVENT_METADATA);
    }

cleanup:
    if (m_hMetadata != nullptr)
        EvtClose(m_hMetadata);

    if (m_ChannelMessages.pMessage != nullptr)
        FreeMessages(&m_ChannelMessages);

    if (m_LevelMessages.pMessage != nullptr)
        FreeMessages(&m_LevelMessages);

    if (m_TaskMessages.pMessage != nullptr)
        FreeMessages(&m_TaskMessages);

    if (m_OpcodeMessages.pMessage != nullptr)
        FreeMessages(&m_OpcodeMessages);

    if (m_KeywordMessages.pMessage != nullptr)
        FreeMessages(&m_KeywordMessages);
}

//--------------------------------------------------------------------------
// Free the memory for each message string in the messages block
// and then free the messages block.
void CProvidersExplorer::FreeMessages(PMESSAGES pMessages)
{
    DWORD dwCount = pMessages->dwSize / sizeof(MSG_STRING);

    for (DWORD i = 0; i < dwCount; i++)
    {
        free(((pMessages->pMessage) + i)->pwcsMessage);
        ((pMessages->pMessage) + i)->pwcsMessage = nullptr;
    }

    free(pMessages->pMessage);
    pMessages->pMessage = nullptr;
}

//--------------------------------------------------------------------------
// Get an enumerator to the provider's events and enumerate them.
// Call this function after first calling the PrintProviderProperties
// function to get the message strings that this section uses.
DWORD CProvidersExplorer::DumpProviderEvents(EVT_HANDLE hMetadata)
{
    EVT_HANDLE hEvents = NULL;
    EVT_HANDLE hEvent = NULL;
    DWORD status = ERROR_SUCCESS;

    // Get a handle to the provider's events.
    hEvents = EvtOpenEventMetadataEnum(hMetadata, 0);
    if (NULL == hEvents)
    {
        wprintf(L"ERROR: EvtOpenEventMetadataEnum failed with %lu\n", GetLastError());
        goto cleanup;
    }

    // Enumerate the events and print each event's metadata.
    while (true)
    {
        hEvent = EvtNextEventMetadata(hEvents, 0);
        if (NULL == hEvent)
        {
            if (ERROR_NO_MORE_ITEMS != (status = GetLastError()))
            {
                wprintf(L"ERROR: EvtNextEventMetadata failed with %lu\n", status);
            }

            break;
        }

        m_Out->OpenTag(XML_EVENT);
        status = DumpEventProperties(hEvent);
        if (status != ERROR_SUCCESS)
            break;
        m_Out->CloseTag(XML_EVENT);

        EvtClose(hEvent);
        hEvent = NULL;
    }

cleanup:
    if (hEvents != nullptr)
        EvtClose(hEvents);

    if (hEvent != nullptr)
        EvtClose(hEvent);

    return status;
}

//--------------------------------------------------------------------------
// Print the metadata for the event.
DWORD CProvidersExplorer::DumpEventProperties(EVT_HANDLE hEvent)
{
    PEVT_VARIANT pProperty = NULL;  // Contains a metadata value
    PEVT_VARIANT pTemp = NULL;
    DWORD dwBufferSize = 0;
    DWORD dwBufferUsed = 0;
    DWORD status = ERROR_SUCCESS;

    // Use the EVT_EVENT_METADATA_PROPERTY_ID's enumeration values to loop
    // through all the metadata for the event.
    for (int Id = 0; Id < EvtEventMetadataPropertyIdEND; Id++)
    {
        // Get the specified metadata property. If the pProperty buffer is not big enough, reallocate the buffer.
        if (!EvtGetEventMetadataProperty(
                hEvent, 
                EVT_EVENT_METADATA_PROPERTY_ID(Id), 
                0, 
                dwBufferSize, 
                pProperty, 
                &dwBufferUsed))
        {
            status = GetLastError();
            if (ERROR_INSUFFICIENT_BUFFER == status)
            {
                dwBufferSize = dwBufferUsed;
                pTemp = (PEVT_VARIANT)realloc(pProperty, dwBufferSize);
                if (pTemp != nullptr)
                {
                    pProperty = pTemp;
                    pTemp = NULL;
                    EvtGetEventMetadataProperty(
                        hEvent, 
                        (EVT_EVENT_METADATA_PROPERTY_ID)Id, 
                        0, 
                        dwBufferSize, 
                        pProperty, 
                        &dwBufferUsed);
                }
                else
                {
                    wprintf(L"ERROR: realloc failed\n");
                    status = ERROR_OUTOFMEMORY;
                    goto cleanup;
                }
            }

            if (ERROR_SUCCESS != (status = GetLastError()))
            {
                wprintf(L"ERROR: EvtGetEventMetadataProperty failed with %d\n", GetLastError());
                goto cleanup;
            }
        }

        status = DumpEventProperty(
            m_hMetadata, 
            Id,
            pProperty);

        if (status != ERROR_SUCCESS)
            break;

        RtlZeroMemory(pProperty, dwBufferUsed);
    }

cleanup:

    if (pProperty != nullptr)
        free(pProperty);

    return status;
}


//--------------------------------------------------------------------------
// Print the event property.
DWORD CProvidersExplorer::DumpEventProperty(
    EVT_HANDLE hMetadata, 
    int Id, 
    PEVT_VARIANT pProperty)
{
    DWORD status = ERROR_SUCCESS;
    static DWORD dwOpcode = 0;
    LPWSTR pName = NULL;       // The event's name property
    LPWSTR pMessage = NULL;    // The event's message string

    switch (Id)
    {
        case EventMetadataEventID:
        {
            wchar_t FormatBuffer[32];
            swprintf_s(
                FormatBuffer, 
                _countof(FormatBuffer), 
                L"%lu", 
                pProperty->UInt32Val);
            m_Out->TagText(
                XML_ID, 
                FormatBuffer);

            break;
        }

        case EventMetadataEventVersion:
        {
            wchar_t FormatBuffer[32];
            swprintf(
                FormatBuffer,
                _countof(FormatBuffer),
                L"%lu",
                pProperty->UInt32Val);

            m_Out->TagText(XML_VERSION, FormatBuffer);
            break;
        }

        // The channel property contains the value of the channel's value attribute.
        // Instead of printing the value attribute, use it to find the channel's
        // message string or name and print it.
        case EventMetadataEventChannel:
            if (pProperty->UInt32Val > 0)
            {
                pName = GetPropertyName(
                    hMetadata, 
                    &m_ChannelMessages, 
                    pProperty->UInt32Val);
                if (pName != nullptr)
                    m_Out->TagText(XML_CHANNEL, pName);
            }
            break;

            // The level property contains the value of the level's value attribute.
            // Instead of printing the value attribute, use it to find the level's
            // message string or name and print it.
        case EventMetadataEventLevel:
            if (pProperty->UInt32Val > 0)
            {
                pName = GetPropertyName(
                    hMetadata, 
                    &m_LevelMessages, 
                    pProperty->UInt32Val);
                if (pName != nullptr)
                    m_Out->TagText(XML_LEVEL, pName);
            }
            break;

            // The opcode property contains the value of the opcode's value attribute.
            // Instead of printing the value attribute, use it to find the opcode's
            // message string or name and print it.
            // The opcode value contains the opcode in the high word. If the opcode is 
            // task-specific, the opcode value will contain the task value in the low word.
            // Save the opcode in a static variable and print it when we get the task
            // value, so we can decide if the opcode is task-specific.
        case EventMetadataEventOpcode:
            dwOpcode = pProperty->UInt32Val;
            break;

            // The task property contains the value of the task's value attribute.
            // Instead of printing the value attribute, use it to find the task's
            // message string or name and print it.
        case EventMetadataEventTask:
            if (pProperty->UInt32Val > 0)
            {
                pName = GetPropertyName(
                    hMetadata, 
                    &m_TaskMessages, 
                    pProperty->UInt32Val);
                if (pName != nullptr)
                    m_Out->TagText(XML_TASK, pName);
            }

            // Now that we know the task, get the opcode string and print it.
            if (dwOpcode > 0)
            {
                pName = GetOpcodeName(
                    hMetadata, 
                    &m_OpcodeMessages, 
                    dwOpcode, 
                    pProperty->UInt32Val);
                if (pName != nullptr)
                    m_Out->TagText(XML_OPCODE, pName);
            }
            break;

            // The keyword property contains a bit mask of all the keywords.
            // Instead of printing the value attribute, use it to find the 
            // message string or name associated with each keyword and print them (space delimited).
        case EventMetadataEventKeyword:
            // The upper 8 bits can contain reserved bit values, so do not include them
            // when checking to see if any keywords are set.
            if ((pProperty->UInt32Val & 0x00FFFFFFFFFFFFFF) > 0)
            {
                pName = GetKeywordName(
                    hMetadata, 
                    &m_KeywordMessages, 
                    pProperty->UInt32Val);
                if (pName != nullptr)
                {
                    m_Out->TagText(XML_KEYWORD, pName);
                    free(pName);
                }
            }
            break;

            // If the message string is not specified, the value is -1.
        case EventMetadataEventMessageID:
            if (pProperty->UInt32Val != -1)
            {
                pMessage = GetMessageString(
                    hMetadata, 
                    pProperty->UInt32Val);
                if (pMessage != nullptr)
                {
                    m_Out->TagCData(XML_MESSAGE, pMessage);
                    free(pMessage);
                }
            }
            break;

            // When you define the event, the template attribute contains the template
            // identifier; however, the template metadata contains an XML string of the 
            // template (includes the data items, not the UserData section).
        case EventMetadataEventTemplate:
            m_Out->TagCData(XML_TEMPLATE, pProperty->StringVal);
            break;

        default:
            break;
    }
    return status;
}

//--------------------------------------------------------------------------
// Used to get the message string or name for levels, tasks, and channels.
// Search the messages block sequentially for an item that has the same value
// and return a pointer to the message string.
LPWSTR CProvidersExplorer::GetPropertyName(
    EVT_HANDLE hMetadata, 
    PMESSAGES pMessages, 
    DWORD dwValue)
{
    UNREFERENCED_PARAMETER(hMetadata);

    LPWSTR pMessage = NULL;
    DWORD dwCount = pMessages->dwSize / sizeof(MSG_STRING);

    for (DWORD i = 0; i < dwCount; i++)
    {
        if (     pMessages->pMessage != nullptr 
              && dwValue == ((pMessages->pMessage) + i)->dwValue )
        {
            pMessage = ((pMessages->pMessage) + i)->pwcsMessage;
            break;
        }
    }

    return pMessage;
}

//--------------------------------------------------------------------------
// Used to get the message string or name for an opcode. Search the messages block sequentially 
// for an item that has the same opcode value (high word). Opcodes can be defined globally or 
// locally (task-specific). All global opcodes must be unique, but multiple tasks can specify the
// same opcode value, so we need to check the low word to see if the task on the event matches
// the task on the opcode.
LPWSTR CProvidersExplorer::GetOpcodeName(
    EVT_HANDLE hMetadata, 
    PMESSAGES pMessages, 
    DWORD dwOpcodeValue, 
    DWORD dwTaskValue)
{
    UNREFERENCED_PARAMETER(hMetadata);

    LPWSTR pMessage = NULL;
    DWORD dwCount = pMessages->dwSize / sizeof(MSG_STRING);
    DWORD dwOpcodeIndex = 0;  // Points to the global opcode (low word is zero)
    BOOL fFound = FALSE;

    for (DWORD i = 0; i < dwCount; i++)
    {
        if (pMessages->pMessage != nullptr && dwOpcodeValue == HIWORD(((pMessages->pMessage) + i)->dwValue))
        {
            if (0 == LOWORD(((pMessages->pMessage) + i)->dwValue))
            {
                dwOpcodeIndex = i;
            }
            else if (dwTaskValue == LOWORD(((pMessages->pMessage) + i)->dwValue))
            {
                pMessage = ((pMessages->pMessage) + i)->pwcsMessage;
                fFound = TRUE;
                break;
            }
        }
    }

    if (FALSE == fFound)
    {
        if (pMessages->pMessage != nullptr)
            pMessage = ((pMessages->pMessage) + dwOpcodeIndex)->pwcsMessage;
    }

    return pMessage;
}

//--------------------------------------------------------------------------
// Used to get the message strings or names for the keywords specified on the event. The event
// contains a bit mask that has bits set for each keyword specified on the event. Search the 
// messages block sequentially for items that have the same keyword bit set. Concatenate all the
// message strings.
LPWSTR CProvidersExplorer::GetKeywordName(
    EVT_HANDLE hMetadata, 
    PMESSAGES pMessages, 
    UINT64 ullKeywords)
{
    UNREFERENCED_PARAMETER(hMetadata);

    LPWSTR pMessage = NULL;
    LPWSTR pTemp = NULL;
    BOOL fFirstMessage = TRUE;
    size_t dwStringLen = 0;
    DWORD dwCount = pMessages->dwSize / sizeof(MSG_STRING);

    for (DWORD i = 0; i < dwCount; i++)
    {
        if (ullKeywords & ((pMessages->pMessage) + i)->ullMask)
        {
            // + space delimiter + null-terminator
            dwStringLen += wcslen(((pMessages->pMessage) + i)->pwcsMessage) + 1 + 1;
            pTemp = (LPWSTR)realloc(pMessage, dwStringLen * sizeof(WCHAR));
            if (pTemp)
            {
                pMessage = pTemp;
                pTemp = NULL;

                if (fFirstMessage)
                {
                    *pMessage = L'\0';  // Need so first wcscat_s call works
                    fFirstMessage = FALSE;
                }
                else
                {
                    wcscat_s(pMessage, dwStringLen, L" ");  // Space delimiter
                }

                wcscat_s(pMessage, dwStringLen, ((pMessages->pMessage) + i)->pwcsMessage);
            }
            else
            {
                wprintf(L"ERROR: realloc failed for GetKeywordName\n");
                if (pMessage)
                {
                    free(pMessage);
                    pMessage = NULL;
                }
                break;
            }
        }
    }

    return pMessage;
}


//--------------------------------------------------------------------------
// Use the EVT_PUBLISHER_METADATA_PROPERTY_ID enumeration values to enumerate all the
// provider's metadata excluding event metadata. Enumerates the metadata for channels,
// tasks, opcodes, levels, keywords, and the provider.
DWORD CProvidersExplorer::DumpProviderProperties(
    EVT_HANDLE hMetadata,
    DWORD PropIdFilter)
{
    DWORD status = ERROR_SUCCESS;

    for (int Id = 0; Id < EvtPublisherMetadataPropertyIdEND; Id++)
    {
        if (PropIdFilter != -1 && Id != PropIdFilter)
            continue;

        PEVT_VARIANT pProperty = NULL;
        status = GetMetadataProperty(
            hMetadata, 
            EVT_PUBLISHER_METADATA_PROPERTY_ID(Id), 
            &pProperty);

        if (status != ERROR_SUCCESS)
        {
            wprintf(L"ERROR: EvtGetPublisherMetadataProperty failed with %d\n", GetLastError());
            return status;
        }

        status = DumpProviderProperty(
            hMetadata, 
            Id, 
            pProperty);

        if (pProperty != nullptr)
            free(pProperty);

        if (status != ERROR_SUCCESS)
            break;

        // Skip the type-specific IDs, so the loop doesn't fail. For channels, levels,
        // opcodes, tasks, and keywords, you use EvtGetPublisherMetadataProperty 
        // to get a handle to an array of those objects. You would then use the type
        // specific ID (for example, EvtPublisherMetadataLevelValue) to access the metadata from
        // the array. Do not call EvtGetPublisherMetadataProperty with a type specific ID or it 
        // will fail. The switch statement increments to the end of the type specific IDs for 
        // each type.
        switch (Id)
        {
            case EvtPublisherMetadataChannelReferences:
                Id = EvtPublisherMetadataChannelReferenceMessageID;
                break;

            case EvtPublisherMetadataLevels:
                Id = EvtPublisherMetadataLevelMessageID;
                break;

            case EvtPublisherMetadataOpcodes:
                Id = EvtPublisherMetadataOpcodeMessageID;
                break;

            case EvtPublisherMetadataTasks:
                Id = EvtPublisherMetadataTaskMessageID;
                break;

            case EvtPublisherMetadataKeywords:
                Id = EvtPublisherMetadataKeywordMessageID;
                break;
        }
    }

    return status;
}

//--------------------------------------------------------------------------
DWORD CProvidersExplorer::GetMetadataProperty(
    EVT_HANDLE hMetadata, 
    EVT_PUBLISHER_METADATA_PROPERTY_ID Id, 
    PEVT_VARIANT *ppProperty)
{
    PEVT_VARIANT pProperty = NULL;
    DWORD dwBufferSize = 0;
    DWORD dwBufferUsed = 0;
    DWORD status = ERROR_SUCCESS;

    *ppProperty = nullptr;

    // Get the metadata property. If the buffer is not big enough, reallocate the buffer.
    if (!EvtGetPublisherMetadataProperty(
        hMetadata,
        Id,
        0,
        dwBufferSize,
        pProperty,
        &dwBufferUsed))
    {
        status = GetLastError();
        if (status == ERROR_INSUFFICIENT_BUFFER)
        {
            dwBufferSize = dwBufferUsed;
            PEVT_VARIANT pTemp = (PEVT_VARIANT)malloc(dwBufferSize);
            if (pTemp != nullptr)
            {
                pProperty = pTemp;
                EvtGetPublisherMetadataProperty(
                    hMetadata,
                    EVT_PUBLISHER_METADATA_PROPERTY_ID(Id),
                    0,
                    dwBufferSize,
                    pProperty,
                    &dwBufferUsed);
            }
            else
            {
                return ERROR_OUTOFMEMORY;
            }
        }

        status = GetLastError();
        if (status != ERROR_SUCCESS)
        {
            if (pProperty != nullptr)
                free(pProperty);

            return status;
        }
    }

    *ppProperty = pProperty;
    return ERROR_SUCCESS;
}

//--------------------------------------------------------------------------
// Print the metadata properties for the provider and the types that
// it defines or references.
DWORD CProvidersExplorer::DumpProviderProperty(
    EVT_HANDLE hMetadata, 
    int Id, 
    PEVT_VARIANT pProperty)
{
    UNREFERENCED_PARAMETER(hMetadata);

    DWORD status = ERROR_SUCCESS;
    DWORD dwArraySize = 0;
    DWORD dwBlockSize = 0;
    LPWSTR pMessage = NULL;

    switch (Id)
    {
        case EvtPublisherMetadataPublisherGuid:
        {
            WCHAR wszProviderGuid[(sizeof(GUID)+8) * 2 *sizeof(WCHAR)];
            StringFromGUID2(
                *(pProperty->GuidVal),
                wszProviderGuid,
                sizeof(wszProviderGuid) / sizeof(WCHAR));

            m_Out->TagText(
                XML_GUID,
                wszProviderGuid);
            break;
        }

        case EvtPublisherMetadataResourceFilePath:
            if (pProperty->Type != EvtVarTypeNull)
            {
                m_Out->TagText(
                    XML_RESOURCEFILEPATH,
                    pProperty->StringVal == nullptr ? L"" : pProperty->StringVal);
            }
            break;

        case EvtPublisherMetadataParameterFilePath:
            m_Out->TagText(
                XML_PARAMETERFILEPATH, 
                (pProperty->Type == EvtVarTypeNull) ? L"" : pProperty->StringVal);
            break;

        case EvtPublisherMetadataMessageFilePath:
            m_Out->TagText(
                XML_MESSAGEFILEPATH,
                (pProperty->Type == EvtVarTypeNull) || (pProperty->StringVal == nullptr) ? L"" : pProperty->StringVal);
            break;

        case EvtPublisherMetadataHelpLink:
            m_Out->TagText(
                XML_HELPLINK,
                (pProperty->Type == EvtVarTypeNull) || (pProperty->StringVal == nullptr) ? L"" : pProperty->StringVal);
            break;

        // The message string ID is -1 if the provider element does not specify the message attribute.
        case EvtPublisherMetadataPublisherMessageID:
            if (pProperty->UInt32Val == -1)
            {
                pMessage = nullptr;
            }
            else
            {
                pMessage = GetMessageString(
                    m_hMetadata, 
                    pProperty->UInt32Val);
            }
            m_Out->TagText(
                XML_PUBLISHER_MESSAGE,
                pMessage == nullptr ? L"" : pMessage);

            if (pMessage != nullptr)
                free(pMessage);

            break;

        // We got the handle to all the channels defined in the channels section
        // of the manifest. Get the size of the array of channel objects and 
        // allocate the messages block that will contain the value and
        // message string for each channel. The strings are used to retrieve
        // display names for the channel referenced in an event definition.
        case EvtPublisherMetadataChannelReferences:
            m_Out->OpenTag(XML_CHANNELS);

            if (EvtGetObjectArraySize(
                    pProperty->EvtHandleVal, 
                    &dwArraySize) && (dwArraySize > 0))
            {
                // You always get a handle to the array but the array can be empty.
                dwBlockSize = sizeof(MSG_STRING) * dwArraySize;
                m_ChannelMessages.pMessage = (PMSG_STRING)malloc(dwBlockSize);
                if (m_ChannelMessages.pMessage != nullptr)
                {
                    RtlZeroMemory(m_ChannelMessages.pMessage, dwBlockSize);
                    m_ChannelMessages.dwSize = dwBlockSize;

                    // For each channel, print its metadata.
                    for (DWORD i = 0; i < dwArraySize; i++)
                    {
                        status = DumpChannelProperties(
                            pProperty->EvtHandleVal, 
                            i, 
                            &m_ChannelMessages);
                        if (status != ERROR_SUCCESS)
                            break;
                    }
                }
                else
                {
                    status = ERROR_OUTOFMEMORY;
                    wprintf(L"ERROR: g_pChannelMessages allocation error\n");
                }
            }
            else
            {
                status = GetLastError();
            }

            EvtClose(pProperty->EvtHandleVal);
            
            m_Out->CloseTag(XML_CHANNELS);
            break;

        // These are handled by the EvtPublisherMetadataChannelReferences case;
        // they are here for completeness but will never be exercised.
        case EvtPublisherMetadataChannelReferencePath:
        case EvtPublisherMetadataChannelReferenceIndex:
        case EvtPublisherMetadataChannelReferenceID:
        case EvtPublisherMetadataChannelReferenceFlags:
        case EvtPublisherMetadataChannelReferenceMessageID:
            break;

            // We got the handle to all the levels defined in the channels section
            // of the manifest. Get the size of the array of level objects and 
            // allocate the messages block that will contain the value and
            // message string for each level. The strings are used to retrieve
            // display names for the level referenced in an event definition.
            // References to the levels defined in Winmeta.xml are included in 
            // the list.
        case EvtPublisherMetadataLevels:
            m_Out->OpenTag(XML_LEVELS);
            // You always get a handle to the array but the array can be empty.
            if (EvtGetObjectArraySize(pProperty->EvtHandleVal, &dwArraySize) && dwArraySize > 0)
            {
                dwBlockSize = sizeof(MSG_STRING) * dwArraySize;
                m_LevelMessages.pMessage = (PMSG_STRING)malloc(dwBlockSize);
                if (m_LevelMessages.pMessage != nullptr)
                {
                    RtlZeroMemory(m_LevelMessages.pMessage, dwBlockSize);
                    m_LevelMessages.dwSize = dwBlockSize;

                    // For each level, print its metadata.
                    for (DWORD i = 0; i < dwArraySize; i++)
                    {
                        m_Out->OpenTag(XML_LEVEL);
                        status = DumpLevelProperties(
                            pProperty->EvtHandleVal, 
                            i, 
                            &m_LevelMessages);
                        m_Out->CloseTag(XML_LEVEL);
                        if (status != ERROR_SUCCESS)
                            break;
                    }
                }
                else
                {
                    status = ERROR_OUTOFMEMORY;
                    wprintf(L"ERROR: g_pLevelMessages allocation error\n");
                }
            }
            else
            {
                status = GetLastError();
            }

            EvtClose(pProperty->EvtHandleVal);

            m_Out->CloseTag(XML_LEVELS);
            break;

            // These are handled by the EvtPublisherMetadataLevels case;
            // they are here for completeness but will never be exercised.
        case EvtPublisherMetadataLevelName:
        case EvtPublisherMetadataLevelValue:
        case EvtPublisherMetadataLevelMessageID:
            break;

            // We got the handle to all the tasks defined in the channels section
            // of the manifest. Get the size of the array of task objects and 
            // allocate the messages block that will contain the value and
            // message string for each task. The strings are used to retrieve
            // display names for the task referenced in an event definition.
        case EvtPublisherMetadataTasks:
            m_Out->OpenTag(XML_TASKS);

            if (EvtGetObjectArraySize(pProperty->EvtHandleVal, &dwArraySize))
            {
                // You always get a handle to the array but the array can be empty.
                if (dwArraySize > 0)
                {
                    dwBlockSize = sizeof(MSG_STRING) * dwArraySize;
                    m_TaskMessages.pMessage = (PMSG_STRING)malloc(dwBlockSize);
                    if (m_TaskMessages.pMessage)
                    {
                        RtlZeroMemory(m_TaskMessages.pMessage, dwBlockSize);
                        m_TaskMessages.dwSize = dwBlockSize;

                        // For each task, print its metadata.
                        for (DWORD i = 0; i < dwArraySize; i++)
                        {
                            m_Out->OpenTag(XML_TASK);
                            status = DumpTaskProperties(
                                pProperty->EvtHandleVal,
                                i,
                                &m_TaskMessages);
                            m_Out->CloseTag(XML_TASK);
                            if (status != ERROR_SUCCESS)
                                break;
                        }
                    }
                    else
                    {
                        status = ERROR_OUTOFMEMORY;
                        wprintf(L"ERROR: g_pTaskMessages allocation error\n");
                    }
                }
            }
            else
            {
                status = GetLastError();
            }

            EvtClose(pProperty->EvtHandleVal);
            
            m_Out->CloseTag(XML_TASKS);
            break;

            // These are handled by the EvtPublisherMetadataTasks case;
            // they are here for completeness but will never be exercised.
        case EvtPublisherMetadataTaskName:
        case EvtPublisherMetadataTaskEventGuid:
        case EvtPublisherMetadataTaskValue:
        case EvtPublisherMetadataTaskMessageID:
            break;

            // We got the handle to all the opcodes defined in the channels section
            // of the manifest. Get the size of the array of opcode objects and 
            // allocate the messages block that will contain the value and
            // message string for each opcode. The strings are used to retrieve
            // display names for the opcode referenced in an event definition.
        case EvtPublisherMetadataOpcodes:
            m_Out->OpenTag(XML_OPCODES);
            
            if (EvtGetObjectArraySize(pProperty->EvtHandleVal, &dwArraySize))
            {
                // You always get a handle to the array but the array can be empty.
                if (dwArraySize > 0)
                {
                    dwBlockSize = sizeof(MSG_STRING) * dwArraySize;
                    m_OpcodeMessages.pMessage = (PMSG_STRING)malloc(dwBlockSize);
                    if (m_OpcodeMessages.pMessage != nullptr)
                    {
                        RtlZeroMemory(
                            m_OpcodeMessages.pMessage, 
                            dwBlockSize);

                        m_OpcodeMessages.dwSize = dwBlockSize;

                        // For each opcode, print its metadata.
                        for (DWORD i = 0; i < dwArraySize; i++)
                        {
                            m_Out->OpenTag(XML_OPCODE);
                            status = DumpOpcodeProperties(
                                pProperty->EvtHandleVal, 
                                i, 
                                &m_OpcodeMessages);
                            m_Out->CloseTag(XML_OPCODE);

                            if (status != ERROR_SUCCESS)
                                break;
                        }
                    }
                    else
                    {
                        status = ERROR_OUTOFMEMORY;
                        wprintf(L"ERROR: g_pOpcodeMessages allocation error\n");
                    }
                }
            }
            else
            {
                status = GetLastError();
            }

            EvtClose(pProperty->EvtHandleVal);

            m_Out->CloseTag(XML_OPCODES);
            break;

            // These are handled by the EvtPublisherMetadataOpcodes case;
            // they are here for completeness but will never be exercised.
        case EvtPublisherMetadataOpcodeName:
        case EvtPublisherMetadataOpcodeValue:
        case EvtPublisherMetadataOpcodeMessageID:
            break;

            // We got the handle to all the keywords defined in the channels section
            // of the manifest. Get the size of the array of keyword objects and 
            // allocate the messages block that will contain the value and
            // message string for each keyword. The strings are used to retrieve
            // display names for the keyword referenced in an event definition.
        case EvtPublisherMetadataKeywords:
            m_Out->OpenTag(XML_KEYWORDS);
            if (
                     EvtGetObjectArraySize(pProperty->EvtHandleVal, &dwArraySize)
                  // You always get a handle to the array but the array can be empty.
                  && (dwArraySize > 0))
            {
                dwBlockSize = sizeof(MSG_STRING) * dwArraySize;
                m_KeywordMessages.pMessage = (PMSG_STRING)malloc(dwBlockSize);
                if (m_KeywordMessages.pMessage)
                {
                    RtlZeroMemory(m_KeywordMessages.pMessage, dwBlockSize);
                    m_KeywordMessages.dwSize = dwBlockSize;

                    // For each keyword, print its metadata.
                    for (DWORD i = 0; i < dwArraySize; i++)
                    {
                        m_Out->OpenTag(XML_KEYWORD);
                        status = DumpKeywordProperties(
                            pProperty->EvtHandleVal, 
                            i, 
                            &m_KeywordMessages);
                        m_Out->CloseTag(XML_KEYWORD);
                        if (status != ERROR_SUCCESS)
                            break;
                    }
                }
                else
                {
                    status = ERROR_OUTOFMEMORY;
                    wprintf(L"ERROR: g_pKeywordMessages allocation error\n");
                }
            }
            else
            {
                status = GetLastError();
            }

            EvtClose(pProperty->EvtHandleVal);

            m_Out->CloseTag(XML_KEYWORDS);
            break;

            // These are handled by the EvtPublisherMetadataKeywords case;
            // they are here for completeness but will never be exercised.
        case EvtPublisherMetadataKeywordName:
        case EvtPublisherMetadataKeywordValue:
        case EvtPublisherMetadataKeywordMessageID:
            break;

        default:
            wprintf(L"ERROR: Unknown property Id: %d\n", Id);
            break;
    }
    return status;
}

//--------------------------------------------------------------------------
// Print the metadata for a channel. Capture the message string and value for use later.
DWORD CProvidersExplorer::DumpChannelProperties(
    EVT_HANDLE hChannels, 
    DWORD dwIndex, 
    PMESSAGES pMessages)
{
    LPWSTR pMessage = NULL;
    DWORD status = ERROR_SUCCESS;
    size_t dwStringLen = 0;
    PEVT_VARIANT pvBuffer = NULL;

    m_Out->OpenTag(XML_CHANNEL);

    pvBuffer = GetProperty(
        hChannels, 
        dwIndex, 
        EvtPublisherMetadataChannelReferenceMessageID);

    if (pvBuffer != nullptr)
    {
        // The value is -1 if the channel did not specify a message attribute.
        if (pvBuffer->UInt32Val  != -1)
        {
            pMessage = GetMessageString(
                m_hMetadata, 
                pvBuffer->UInt32Val);
        }
        m_Out->TagText(
            XML_MESSAGE, 
            pMessage == nullptr ? L"" : pMessage);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    // This is the channel name. You can use it to call the EvtOpenChannelConfig function
    // to get the channel's configuration information.
    pvBuffer = GetProperty(
        hChannels, 
        dwIndex, 
        EvtPublisherMetadataChannelReferencePath);

    if (pvBuffer != nullptr)
    {
        m_Out->TagText(
            XML_PATH, 
            pvBuffer->StringVal);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    // Capture the message string if the channel specified a message string; otherwise,
    // capture the channel's name.
    if (pMessage != nullptr)
    {
        dwStringLen = wcslen(pMessage) + 1;
        LPWSTR pNextMsg = ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(pNextMsg, dwStringLen, pMessage);
    }
    else
    {
        dwStringLen = wcslen(pvBuffer->StringVal) + 1;
        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pvBuffer->StringVal);
    }

    pvBuffer = GetProperty(
        hChannels, 
        dwIndex, 
        EvtPublisherMetadataChannelReferenceIndex);
    if (pvBuffer != nullptr)
    {
        wchar_t FormatBuffer[40];
        swprintf_s(
            FormatBuffer, 
            _countof(FormatBuffer), 
            L"%lu", 
            pvBuffer->UInt32Val);

        m_Out->TagText(XML_INDEX, FormatBuffer);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    // Capture the channel's value attribute, which is used to look up the channel's
    // message string.
    pvBuffer = GetProperty(
        hChannels, 
        dwIndex, 
        EvtPublisherMetadataChannelReferenceID);
    if (pvBuffer)
    {
        wchar_t FormatBuffer[40];
        swprintf_s(FormatBuffer, _countof(FormatBuffer), L"%lu", pvBuffer->UInt32Val);
        ((pMessages->pMessage) + dwIndex)->dwValue = pvBuffer->UInt32Val;

        m_Out->TagText(XML_ID, FormatBuffer);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    pvBuffer = GetProperty(hChannels, dwIndex, EvtPublisherMetadataChannelReferenceFlags);
    if (pvBuffer)
    {
        if (EvtChannelReferenceImported == (EvtChannelReferenceImported & pvBuffer->UInt32Val))
            m_Out->TagText(XML_IMPORTED, L"true");
        else
            m_Out->TagText(XML_IMPORTED, L"false");
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

cleanup:
    if (pvBuffer != nullptr)
        free(pvBuffer);

    if (pMessage != nullptr)
        free(pMessage);

    m_Out->CloseTag(XML_CHANNEL);
    return status;
}

//--------------------------------------------------------------------------
// Print the metadata for a level. Capture the message string and value for use later.
DWORD CProvidersExplorer::DumpLevelProperties(
    EVT_HANDLE hLevels, 
    DWORD dwIndex, 
    PMESSAGES pMessages)
{
    LPWSTR pMessage = NULL;
    DWORD status = ERROR_SUCCESS;
    size_t dwStringLen = 0;
    PEVT_VARIANT pvBuffer = NULL;

    pvBuffer = GetProperty(
        hLevels, 
        dwIndex, 
        EvtPublisherMetadataLevelMessageID);

    if (pvBuffer != nullptr)
    {
        // The value is -1 if the level did not specify a message attribute.
        if (pvBuffer->UInt32Val  != -1)
        {
            pMessage = GetMessageString(
                m_hMetadata, 
                pvBuffer->UInt32Val);
        }
        m_Out->TagText(
            XML_MESSAGE, 
            (pMessage != nullptr) ? pMessage : L"");
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    pvBuffer = GetProperty(
        hLevels, 
        dwIndex, 
        EvtPublisherMetadataLevelName);
    if (pvBuffer != nullptr)
    {
        m_Out->TagText(
            XML_NAME, 
            pvBuffer->StringVal);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    // Capture the message string if the level specified a message string; otherwise,
    // capture the level's name.
    if (pMessage != nullptr)
    {
        dwStringLen = wcslen(pMessage) + 1;
        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pMessage);
    }
    else
    {
        dwStringLen = wcslen(pvBuffer->StringVal) + 1;
        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pvBuffer->StringVal);
    }

    // Capture the level's value attribute, which is used to look up the level's
    // message string.
    pvBuffer = GetProperty(
        hLevels, 
        dwIndex, 
        EvtPublisherMetadataLevelValue);
    if (pvBuffer != nullptr)
    {
        ((pMessages->pMessage) + dwIndex)->dwValue = pvBuffer->UInt32Val;

        wchar_t FormBuffer[64];
        swprintf_s(FormBuffer, _countof(FormBuffer), L"%lu", pvBuffer->UInt32Val);
        m_Out->TagText(XML_VALUE, FormBuffer);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

cleanup:

    if (pvBuffer)
        free(pvBuffer);

    if (pMessage)
        free(pMessage);

    return status;
}


//--------------------------------------------------------------------------
// Print the metadata for a task. Capture the message string and value for use later.
DWORD CProvidersExplorer::DumpTaskProperties(
    EVT_HANDLE hTasks, 
    DWORD dwIndex, 
    PMESSAGES pMessages)
{
    LPWSTR pMessage = NULL;
    DWORD status = ERROR_SUCCESS;
    size_t dwStringLen = 0;
    PEVT_VARIANT pvBuffer = NULL;
    WCHAR wszEventGuid[50];

    pvBuffer = GetProperty(hTasks, dwIndex, EvtPublisherMetadataTaskMessageID);
    if (pvBuffer)
    {
        // The value is -1 if the task did not specify a message attribute.
        if (-1 != pvBuffer->UInt32Val)
        {
            pMessage = GetMessageString(m_hMetadata, pvBuffer->UInt32Val);
        }
        m_Out->TagText(
            XML_MESSAGE, 
            pMessage != nullptr ? pMessage : L"");
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    pvBuffer = GetProperty(
        hTasks, 
        dwIndex, 
        EvtPublisherMetadataTaskName);
    if (pvBuffer)
    {
        m_Out->TagText(XML_NAME, pvBuffer->StringVal);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    // Capture the message string if the task specified a message string; otherwise,
    // capture the task's name.
    if (pMessage)
    {
        dwStringLen = wcslen(pMessage) + 1;
        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pMessage);
    }
    else
    {
        dwStringLen = wcslen(pvBuffer->StringVal) + 1;
        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pvBuffer->StringVal);
    }

    pvBuffer = GetProperty(
        hTasks, 
        dwIndex, 
        EvtPublisherMetadataTaskEventGuid);
    if (pvBuffer != nullptr)
    {
        if (!IsEqualGUID(GUID_NULL, *(pvBuffer->GuidVal)))
        {
            StringFromGUID2(*(pvBuffer->GuidVal), wszEventGuid, sizeof(wszEventGuid) / sizeof(WCHAR));
            m_Out->TagText(XML_GUID, wszEventGuid);
        }
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    // Capture the task's value attribute, which is used to look up the task's
    // message string.
    pvBuffer = GetProperty(hTasks, dwIndex, EvtPublisherMetadataTaskValue);
    if (pvBuffer)
    {
        ((pMessages->pMessage) + dwIndex)->dwValue = pvBuffer->UInt32Val;

        wchar_t FormBuf[32];
        swprintf_s(FormBuf, _countof(FormBuf), L"%lu", pvBuffer->UInt32Val);
        m_Out->TagText(XML_VALUE, FormBuf);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

cleanup:

    if (pvBuffer)
        free(pvBuffer);

    if (pMessage)
        free(pMessage);

    return status;
}

//--------------------------------------------------------------------------
// Print the metadata for a opcode. Capture the message string and value for use later.
DWORD CProvidersExplorer::DumpOpcodeProperties(
    EVT_HANDLE hOpcodes, 
    DWORD dwIndex, 
    PMESSAGES pMessages)
{
    LPWSTR pMessage = NULL;
    DWORD status = ERROR_SUCCESS;
    size_t dwStringLen = 0;
    PEVT_VARIANT pvBuffer = NULL;

    pvBuffer = GetProperty(
        hOpcodes, 
        dwIndex, 
        EvtPublisherMetadataOpcodeMessageID);

    if (pvBuffer != nullptr)
    {
        // The value is -1 if the opcode did not specify a message attribute.
        if (-1 != pvBuffer->UInt32Val)
        {
            pMessage = GetMessageString(
                m_hMetadata, 
                pvBuffer->UInt32Val);
        }

        m_Out->TagText(
            XML_MESSAGE, 
            pMessage != nullptr ? pMessage : L"");
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    pvBuffer = GetProperty(
        hOpcodes, 
        dwIndex, 
        EvtPublisherMetadataOpcodeName);
    if (pvBuffer != nullptr)
    {
        m_Out->TagText(XML_NAME, pvBuffer->StringVal);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    // Capture the message string if the opcode specified a message string; otherwise,
    // capture the opcode's name.
    if (pMessage != nullptr)
    {
        dwStringLen = wcslen(pMessage) + 1;

        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pMessage);
    }
    else
    {
        dwStringLen = wcslen(pvBuffer->StringVal) + 1;
        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pvBuffer->StringVal);
    }

    // Capture the opcode's value attribute, which is used to look up the opcode's
    // message string.
    pvBuffer = GetProperty(
        hOpcodes, 
        dwIndex, 
        EvtPublisherMetadataOpcodeValue);
    if (pvBuffer)
    {
        wchar_t FormBuf[64];
        swprintf_s(FormBuf, _countof(FormBuf), L"%hu", HIWORD(pvBuffer->UInt32Val));
        m_Out->TagText(XML_VALUE, FormBuf);

        swprintf_s(FormBuf, _countof(FormBuf), L"%hu", LOWORD(pvBuffer->UInt32Val));
        m_Out->TagText(XML_TASK, FormBuf);

        ((pMessages->pMessage) + dwIndex)->dwValue = pvBuffer->UInt32Val;
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

cleanup:

    if (pvBuffer)
        free(pvBuffer);

    if (pMessage)
        free(pMessage);

    return status;
}

//--------------------------------------------------------------------------
// Print the metadata for a keyword. Capture the message string and mask for use later.
DWORD CProvidersExplorer::DumpKeywordProperties(
    EVT_HANDLE hKeywords, 
    DWORD dwIndex, 
    PMESSAGES pMessages)
{
    LPWSTR pMessage = NULL;
    DWORD status = ERROR_SUCCESS;
    size_t dwStringLen = 0;
    PEVT_VARIANT pvBuffer = NULL;

    pvBuffer = GetProperty(
        hKeywords, 
        dwIndex, 
        EvtPublisherMetadataKeywordMessageID);
    if (pvBuffer)
    {
        // The value is -1 if the keyword did not specify a message attribute.
        if (-1 != pvBuffer->UInt32Val)
        {
            pMessage = GetMessageString(m_hMetadata, pvBuffer->UInt32Val);
        }
        m_Out->TagText(
            XML_MESSAGE,
            pMessage != nullptr ? pMessage : L"");
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    pvBuffer = GetProperty(
        hKeywords, 
        dwIndex, 
        EvtPublisherMetadataKeywordName);
    if (pvBuffer != nullptr)
    {
        m_Out->TagText(
            XML_NAME, 
            pvBuffer->StringVal);
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

    // Capture the message string if the keyword specified a message string; otherwise,
    // capture the keyword's name.
    if (pMessage)
    {
        dwStringLen = wcslen(pMessage) + 1;
        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pMessage);
    }
    else
    {
        dwStringLen = wcslen(pvBuffer->StringVal) + 1;
        ((pMessages->pMessage) + dwIndex)->pwcsMessage = (LPWSTR)malloc(dwStringLen * sizeof(WCHAR));
        wcscpy_s(((pMessages->pMessage) + dwIndex)->pwcsMessage, dwStringLen, pvBuffer->StringVal);
    }

    // Capture the keyword's mask attribute, which is used to look up the keyword's
    // message string.
    pvBuffer = GetProperty(
        hKeywords, 
        dwIndex, 
        EvtPublisherMetadataKeywordValue);
    if (pvBuffer != nullptr)
    {
        wchar_t FormatBuffer[64];
        swprintf_s(
            FormatBuffer, 
            _countof(FormatBuffer), 
            L"%I64u", 
            pvBuffer->UInt64Val);

        m_Out->TagText(XML_VALUE, FormatBuffer);

        ((pMessages->pMessage) + dwIndex)->ullMask = pvBuffer->UInt32Val;
    }
    else
    {
        status = GetLastError();
        goto cleanup;
    }

cleanup:
    if (pvBuffer != nullptr)
        free(pvBuffer);

    if (pMessage != nullptr)
        free(pMessage);

    return status;
}

//--------------------------------------------------------------------------
// Get the metadata property for an object in the array.
PEVT_VARIANT CProvidersExplorer::GetProperty(
    EVT_HANDLE handle, 
    DWORD dwIndex, 
    EVT_PUBLISHER_METADATA_PROPERTY_ID PropertyId)
{
    DWORD status = ERROR_SUCCESS;
    PEVT_VARIANT pvBuffer = NULL;
    DWORD dwBufferSize = 0;
    DWORD dwBufferUsed = 0;

    if (!EvtGetObjectArrayProperty(handle, PropertyId, dwIndex, 0, dwBufferSize, pvBuffer, &dwBufferUsed))
    {
        status = GetLastError();
        if (ERROR_INSUFFICIENT_BUFFER == status)
        {
            dwBufferSize = dwBufferUsed;
            pvBuffer = (PEVT_VARIANT)malloc(dwBufferSize);
            if (pvBuffer != nullptr)
            {
                EvtGetObjectArrayProperty(
                    handle, 
                    PropertyId, 
                    dwIndex, 
                    0, 
                    dwBufferSize, 
                    pvBuffer, 
                    &dwBufferUsed);
            }
            else
            {
                wprintf(L"malloc failed\n");
                status = ERROR_OUTOFMEMORY;
                goto cleanup;
            }
        }

        if (ERROR_SUCCESS != (status = GetLastError()))
        {
            wprintf(L"ERROR: EvtGetObjectArrayProperty failed with %d\n", status);
        }
    }

cleanup:
    return pvBuffer;
}

//--------------------------------------------------------------------------
// Get the formatted message string.
LPWSTR CProvidersExplorer::GetMessageString(
    EVT_HANDLE hMetadata, 
    DWORD dwMessageId)
{
    LPWSTR pBuffer = NULL;
    DWORD dwBufferSize = 0;
    DWORD dwBufferUsed = 0;
    DWORD status = 0;

    if (!EvtFormatMessage(
            hMetadata, 
            NULL, 
            dwMessageId, 
            0, 
            NULL, 
            EvtFormatMessageId, 
            dwBufferSize, 
            pBuffer, 
            &dwBufferUsed))
    {
        status = GetLastError();
        if (ERROR_INSUFFICIENT_BUFFER == status)
        {
            dwBufferSize = dwBufferUsed;
            pBuffer = (LPWSTR)malloc(dwBufferSize * sizeof(WCHAR));

            if (pBuffer != nullptr)
            {
                EvtFormatMessage(
                    hMetadata, 
                    NULL, 
                    dwMessageId, 
                    0, 
                    NULL, 
                    EvtFormatMessageId, 
                    dwBufferSize, 
                    pBuffer, 
                    &dwBufferUsed);
            }
        }
        else
        {
            wprintf(L"ERROR: EvtFormatMessage failed with %u\n", status);
        }
    }

    return pBuffer;
}

//--------------------------------------------------------------------------
static int ParseOptions(
    int argc, 
    wchar_t *argv[],
    COptions *opt,
    wchar_t **errmsg)
{
    opt->ProviderFilter = nullptr;
    opt->bGetMeta = false;
    *errmsg = nullptr;

    for (int i = 1; i < argc; i++)
    {
        if (_wcsicmp(argv[i], OPT_NAME) == 0)
        {
            if (++i >= argc)
            {
                *errmsg = L"No provider name specified!\n";
                return -1;
            }

            opt->ProviderFilter = argv[i];
        }
        else if (_wcsicmp(argv[i], OPT_OUT) == 0)
        {
            if (++i >= argc)
            {
                *errmsg = L"No output file name specified!\n";
                return -1;
            }
            opt->OutFileName = argv[i];
        }
        else if (_wcsicmp(argv[i], L"/?") == 0)
        {
            return 0;
        }
        else if (_wcsicmp(argv[i], OPT_META) == 0)
        {
            opt->bGetMeta = true;
        }
        else if (_wcsicmp(argv[i], OPT_FORCE) == 0)
        {
            opt->bForce = true;
        }
        else if (_wcsicmp(argv[i], OPT_EVENTMETA) == 0)
        {
            opt->bGetEventMeta = true;
        }
    }
    return 1;
}

//--------------------------------------------------------------------------
int CProvidersExplorer::Dump()
{
    std::set<std::wstring> seen_provider;
    EVT_HANDLE hProviders = NULL;
    // Get a handle to the list of providers.
    hProviders = EvtOpenPublisherEnum(
        NULL, 
        0);

    LPWSTR pwcsProviderName = nullptr;
    LPWSTR pTemp = nullptr;

    if (NULL == hProviders)
    {
        wprintf(L"ERROR: EvtOpenPublisherEnum failed with %lu\n", GetLastError());
        goto cleanup;
    }

    DWORD dwBufferSize = 0;
    DWORD dwBufferUsed = 0;
    DWORD status = ERROR_SUCCESS;

    int err = -1;

    m_Out->OpenTag(XML_PROVIDERS);

    // Enumerate the providers in the list.
    while (true)
    {
        // Get a provider from the list. If the buffer is not big enough
        // to contain the provider's name, reallocate the buffer to the required size.
        if (!EvtNextPublisherId(hProviders, dwBufferSize, pwcsProviderName, &dwBufferUsed))
        {
            status = GetLastError();
            if (ERROR_NO_MORE_ITEMS == status)
            {
                break;
            }
            else if (ERROR_INSUFFICIENT_BUFFER == status)
            {
                dwBufferSize = dwBufferUsed;
                pTemp = (LPWSTR)realloc(pwcsProviderName, dwBufferSize * sizeof(WCHAR));
                if (pTemp != nullptr)
                {
                    pwcsProviderName = pTemp;
                    pTemp = NULL;
                    EvtNextPublisherId(
                        hProviders, 
                        dwBufferSize, 
                        pwcsProviderName, 
                        &dwBufferUsed);
                }
                else
                {
                    wprintf(L"ERROR: realloc failed\n");
                    goto cleanup;
                }
            }

            if ((status = GetLastError()) != ERROR_SUCCESS)
            {
                wprintf(L"ERROR: EvtNextPublisherId failed with %d\n", status);
                goto cleanup;
            }
        }

        auto p = seen_provider.find(pwcsProviderName);
        if (        p == seen_provider.end()
              &&
                  (    m_Opt->ProviderFilter == nullptr 
                    || wcscmp(m_Opt->ProviderFilter, pwcsProviderName) == 0 )
            )
        {
            m_Out->OpenTag(XML_PROVIDER);
            m_Out->TagText(XML_NAME, pwcsProviderName);

            DumpPublisherMetadata(pwcsProviderName);

            m_Out->CloseTag(XML_PROVIDER);
            seen_provider.insert(pwcsProviderName);
        }

        RtlZeroMemory(
            pwcsProviderName, 
            dwBufferUsed * sizeof(WCHAR));
    }
    err = 0;
    m_Out->CloseTag(XML_PROVIDERS);

cleanup:

    if (pwcsProviderName != nullptr)
        free(pwcsProviderName);

    if (hProviders)
        EvtClose(hProviders);

    return err;
}

//--------------------------------------------------------------------------
int _tmain(int argc, TCHAR *argv[])
{
    COptions opt;
    wchar_t *errmsg;

    _setmode(_fileno(stdout), _O_U16TEXT);

    int err = ParseOptions(
        argc, 
        argv, 
        &opt, 
        &errmsg);
    if (err == -1)
    {
        wprintf(L"ERROR: Error while parsing arguments: %s\n", errmsg);
        return -1;
    }
    else if (err == 0)
    {
        wprintf(
            L"Usage:\n"
            L"------\n"
            L"%s [name]\t - Specify provider name\n"
            L"%s [filename]\t - Specify output file name\n"
            L"%s\t - Show metadata\n"
            L"%s\t - Show event metadata\n",
            OPT_NAME, OPT_OUT, OPT_META, OPT_EVENTMETA);

        return 0;
    }

    if (!opt.bForce && opt.bGetMeta && opt.ProviderFilter == nullptr)
    {
        wprintf(L"ERROR: Please provider a provider name when specifying the meta argument\n");
        return -2;
    }

    std::wofstream ofs;
    if (opt.OutFileName != nullptr)
    {
        const std::locale utf8_locale = std::locale(std::locale(), new std::codecvt_utf8<wchar_t>());
        ofs.open(opt.OutFileName, std::ofstream::out);
        if (!ofs.is_open())
        {
            wprintf(L"ERROR: could not create output file: %s\n", opt.OutFileName);
            return -1;
        }
        ofs.imbue(utf8_locale);
    }

    CXmlOutput o(opt.OutFileName != nullptr ? ofs : std::wcout);
    CProvidersExplorer p(&opt, &o);
    return p.Dump();
}