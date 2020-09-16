#include <windows.h>
#include <conio.h>
#include <stdio.h>

int main(int argc, char *argv[])
{
    if (argc < 2)
    {
        printf("Please pass a library or libraries to load!\n");
        return -1;
    }

    size_t count = argc - 1;
    HMODULE *hMods = new HMODULE[count];
    char **dll_names = new char *[count];

    for (size_t i=0;i<count;++i)
    {
        const char *dll_name = argv[i + 1];

        __try
        {
            HMODULE hMod = LoadLibraryA(dll_name);
            if (hMod == nullptr)
            {
                printf("#%zu failed to load '%s'", i, dll_name);
                return -2;
            }

            printf("#%zu %p: '%s' loaded...\n", i, hMod, dll_name);

            hMods[i] = hMod;
            dll_names[i] = _strdup(dll_name);
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            printf("#%zu exception occurred!", i);
            _getch();
            break;
        }
    }

    // Pause and unload
    printf("\npress any key to terminate and unload...");
    _getch();
    printf("\n");

    for (size_t i=0;i<count;++i)
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