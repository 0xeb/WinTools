#include <windows.h>
#include <stdio.h>

// These constructs below seem to force the VS compiler to generate individual code sections.
// The reason I do that, is because I don't have much code to generate code pages.
// Instead, we use section alignment as a trick to generate pages.

template <int> void dummy_func()
{
    static const int ABC[100] = { 1, 2, 3, 0 };
    printf("Something else %d!\n", ABC[4]);
}

#define STRINGIFY(a) #a 
#define CONCATS(a, b) STRINGIFY(a ## b)

#define make_dummy_func(n) \
    __pragma(code_seg(push, CONCATS(.text, n))) \
    template<> void dummy_func<n>() \
    { \
        printf("Func " #n "\n"); \
    } \
    __pragma(code_seg(pop) )

make_dummy_func(1)
make_dummy_func(2)
make_dummy_func(3)
make_dummy_func(4)
make_dummy_func(5)
make_dummy_func(6)
make_dummy_func(7)
make_dummy_func(8)
make_dummy_func(9)


BOOL WINAPI DllMain(
    HINSTANCE hinstDLL,  // handle to DLL module
    DWORD fdwReason,     // reason for calling function
    LPVOID lpReserved)   // reserved
{
    // Perform actions based on the reason for calling.
    switch( fdwReason ) 
    { 
        case DLL_PROCESS_ATTACH:
         // Initialize once for each new process.
         // Return FALSE to fail DLL load.
            break;

        case DLL_THREAD_ATTACH:
         // Do thread-specific initialization.
            break;

        case DLL_THREAD_DETACH:
         // Do thread-specific cleanup.
            break;

        case DLL_PROCESS_DETACH:
         // Perform any necessary cleanup.
            break;
    }
    return TRUE;  // Successful DLL_PROCESS_ATTACH.
}