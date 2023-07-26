#include <stdio.h>
#include <tchar.h>
#include <conio.h>
#include <cstdlib>
#include "Wizmo.h"

//----------------------------------------------------------------------------------
int main(int argc, char *argv[])
{
    // Should pause at exit?
    for (int iarg = 1; iarg < argc; ++iarg)
    {
        if (_stricmp(argv[iarg], "--pause") == 0)
        {
            atexit([] { printf("press any key to exit...\n"); _getch(); });
            break;
        }
    }

    if (argc < 2)
    {
        CWizmo::ShowHelp();
        return -1;
    }
    CWizmo w;


    const char *cmd = argv[1];
    const char *param = argv[2];

    if (_stricmp(cmd, "-monoff") == 0)
        w.MonitorOff();
    else if (_stricmp(cmd, "-abortshutdown") == 0)
        w.AbortShutdown();
    else if (param != NULL && _stricmp(cmd, "-play") == 0)
        w.PlayFile(param);
    else if (_stricmp(cmd, "-lock") == 0)
        w.Lock();
    else if (_stricmp(cmd, "-hibernate") == 0)
        w.Hibernate();
    else if (_stricmp(cmd, "-logoff") == 0)
        w.LogOff();
    else if (_stricmp(cmd, "-mute") == 0)
        w.MuteMasterVolume(param == NULL ? true : (param[0] - '0' == 1));
    else if (_stricmp(cmd, "-reboot") == 0)
        w.Reboot();
    else if (_stricmp(cmd, "-blank") == 0)
        w.Blank();
    else if (param != NULL && _stricmp(cmd, "-eject") == 0)
        w.OpenCloseDrive(param[0], true);
    else if (param != NULL && _stricmp(cmd, "-close") == 0)
        w.OpenCloseDrive(param[0], false);
    else
    {
        printf("Invalid usage!\n");
        return -2;
    }
    return 0;
}