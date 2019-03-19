#include <stdio.h>
#include <windows.h>
#pragma comment(lib, "user32")

//-------------------------------------------------------------------------
int main(int argc, char *argv[])
{
    if (argc < 3)
    {
        printf("send_console_keys WindowTitle KeysToSend\n");
        return -1;
    }

    HWND hwndTarget = FindWindowA("ConsoleWindowClass", argv[1]);
    if (hwndTarget == nullptr)
    {
        printf("Could not find window!\n");
        return -2;
    }
    else
    {
        char *cmd = argv[2];
        if (*cmd != '\0')
        {
            for (; *cmd != '\0'; ++cmd)
                SendMessageA(hwndTarget, WM_CHAR, *cmd, 0);

            SendMessageA(hwndTarget, WM_CHAR, '\r', 0);
        }
    }

    return 0;
}
