#include <windows.h>
#include <conio.h>
#include <stdio.h>

LPSTR GetErrorAsString(DWORD error_code)
{
    LPSTR message_buffer = nullptr;
    size_t size = FormatMessageA(
        FORMAT_MESSAGE_ALLOCATE_BUFFER |
        FORMAT_MESSAGE_FROM_SYSTEM |
        FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL,
        error_code,
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        reinterpret_cast<LPSTR>(&message_buffer),
        0,
        NULL
    );

    return message_buffer;
}

//-------------------------------------------------------------------------
int main(int argc, char *argv[])
{
    if (argc < 2)
    {
        printf("Please pass a library or libraries to load!\n");
        return -1;
    }

    size_t nMaxDlls  = size_t(argc) - 1;
    size_t nLoadDlls = 0;
    HMODULE *hMods   = new HMODULE[nMaxDlls];
    char **dll_names = new char *[nMaxDlls];

    // Loading stage
    for (int iArg=1, iMod=0; iArg < argc; ++iArg)
    {
        const char *dll_name = argv[iArg];

        __try
        {
            HMODULE hMod = LoadLibraryA(dll_name);
            if (hMod == nullptr)
            {
                auto gle = GetLastError();
                auto message_buffer = GetErrorAsString(gle);
                printf("#%d failed to load '%s'...skipped with error %d: %s", 
                    iArg, 
                    dll_name, 
                    gle,
                    message_buffer);
                LocalFree(message_buffer);

                continue;
            }

            printf("#%d %p: '%s' loaded...\n", iArg, hMod, dll_name);

            hMods[iMod] = hMod;
            dll_names[iMod] = _strdup(dll_name);
            ++iMod;
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            printf("#%d exception occurred while loading '%s'...skipped", iArg, dll_name);
            (void)_getch();
            continue;
        }
    }

    // Pause and unload
    printf("\nPress any key to terminate and unload...");
    (void)_getch();
    printf("\n");

    for (size_t i=0; i < nLoadDlls; ++i)
    {
        HMODULE hMod = hMods[i];
        FreeLibrary(hMod);
        printf("%p '%s', library unloaded...\n", hMod, dll_names[i]);
        free(dll_names[i]);
    }

    delete[] hMods;
    delete[] dll_names;

    return 0;
}