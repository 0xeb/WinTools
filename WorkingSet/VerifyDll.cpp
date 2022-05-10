/*
QueryWorkingSetEx example.

The companion dummy DLL project is used for testing the API.
*/

#include <windows.h>
#include <Psapi.h>
#include <inttypes.h>
#include <tchar.h>
#include <conio.h>
#include <stdio.h>
#include <time.h>

constexpr size_t PAGE_SIZE = 0x1000;

//-------------------------------------------------------------------------
static void setup_wksinfo(
    HMODULE hDll, 
    PPSAPI_WORKING_SET_EX_INFORMATION *pv, 
    DWORD *count)
{
    auto pinh = (PIMAGE_NT_HEADERS)((char*)hDll + PIMAGE_DOS_HEADER(hDll)->e_lfanew);

    DWORD nb_pages = *count = pinh->OptionalHeader.SizeOfCode / PAGE_SIZE;
    
    PPSAPI_WORKING_SET_EX_INFORMATION pwks = *pv = new _PSAPI_WORKING_SET_EX_INFORMATION[nb_pages];
    if (pwks == nullptr)
    {
        printf("Fatal error: could not allocate for %u pages (%" PRIuPTR " bytes)\n",
            nb_pages,
            nb_pages * sizeof(*pwks));
        exit(ENOMEM);
    }

    uintptr_t p_code = uintptr_t(hDll) + pinh->OptionalHeader.BaseOfCode;
    for (DWORD i = 0; 
         i < nb_pages; 
         ++i, p_code += PAGE_SIZE, ++pwks)
    {
        pwks->VirtualAddress = PVOID(p_code);
        pwks->VirtualAttributes.Flags = 0;
    }
}

//-------------------------------------------------------------------------
static int get_key(const char* prompt = nullptr)
{
    if (prompt != nullptr)
        printf("%s", prompt);

    auto ch = _getch();

    if (prompt != nullptr)
        printf("%c\n", ch);

    return ch;
}

//-------------------------------------------------------------------------
int main(int argc, char *argv[])
{
    HMODULE hDll = LoadLibrary(_TEXT("DummyDll"));
    if (hDll == nullptr)
    {
        printf("DummyDll not found!\n");
        return -1;
    }

    srand((unsigned int)time(NULL));

    printf("Loaded test DLL at %p\n", hDll);

    PPSAPI_WORKING_SET_EX_INFORMATION pv;
    DWORD pv_count;
    setup_wksinfo(hDll, &pv, &pv_count);

    DWORD pv_size = sizeof(*pv) * pv_count;

    for (;;)
    {
        printf(
            "---------------------------------------------------------------\n"
            "i) information\n"
            "s) scan pages\n"
            "p) patch page\n"
            "q) quit\n");

        auto ch = get_key();

        // Internal information
        if (ch == 'i')
        {
            printf(
                "----------- Information ---------------------------------------\n"
                "Test DLL image base: %p\n"
                "Number of pages %u\n",
                hDll, pv_count);
        }
        // Scan memory pages
        else if (ch == 's')
        {
            printf("---- Querying all pages ---------------------------------------\n");
            // Query all pages
            if (!QueryWorkingSetEx(GetCurrentProcess(), pv, pv_size))
            {
                printf("QWS failed!\n");
                break;
            }

            // Inspect changes
            for (DWORD i = 0; i < pv_count; ++i)
            {
                auto pvi = pv + i;
                auto valid = DWORD(pvi->VirtualAttributes.Valid);
                auto rsvd_ = DWORD(pvi->VirtualAttributes.Reserved & 0x40) != 0;
                DWORD shared = valid ? DWORD(pvi->VirtualAttributes.Shared) : DWORD(pvi->VirtualAttributes.Invalid.Shared);
                printf("Page #%2u: %p; Valid: %u; Shared: %u Rsvd: %u\n",
                    i + 1,
                    pvi->VirtualAddress,
                    valid,
                    shared,
                    rsvd_);
            }
        }
        // Patch
        else if (ch == 'p') do
        {
            printf("------ Patching a page ----------------------------------------\n");
            DWORD page_no;
            char input[10];
            printf("enter page number: ");
            gets_s(input);
            input[sizeof(input) - 1] = '\0';
            sscanf_s(input, "%u", &page_no);

            if (page_no <= 1)
            {
                printf("Reserved paged number %u cannot be used!\n", page_no);
                continue;
            }

            --page_no;

            if (page_no >= pv_count)
            {
                printf("Invalid page number\n");
                continue;
            }

            DWORD old_protect;
            auto addr = pv[page_no].VirtualAddress;
            if (!VirtualProtect(addr, PAGE_SIZE, PAGE_EXECUTE_READWRITE, &old_protect))
            {
                printf("Failed to change page #%u @ %" PRIxPTR " protection\n", page_no, uintptr_t(addr));
                exit(ENXIO);
            }

            volatile auto rnd_addr = (uint8_t * )(uintptr_t(addr) + (rand() % PAGE_SIZE));

            // Touch the shared page
            *rnd_addr = *rnd_addr;

            VirtualProtect(addr, PAGE_SIZE, old_protect, &old_protect);
        } while (false);
        // Quit
        else if (ch == 'q')
        {
            break;
        }
    }

    FreeLibrary(hDll);
    return 0;
}