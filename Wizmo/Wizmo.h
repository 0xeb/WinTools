#pragma once

#define WIN32_LEAN_AND_MEAN     // Exclude rarely-used stuff from Windows headers
#define _WIN32_WINNT 0x0500
#define _CRT_SECURE_NO_WARNINGS

#include <windows.h>
#include <mmsystem.h>
#include <tchar.h>

#define __MYDEBUG

class CWizmo
{
private:
    HINSTANCE m_hInstance;
    HWND m_hwndCoverage;
    BOOL m_bForce;
    LPTSTR m_szMachineName;

    bool IsWinNT();
    void AdjustShutdownPrivilege();
    bool MakeCoverageWindow();
    bool MyExitWindowsEx(UINT Flags);
    void SpeakersAdjust(int nValue, int aControlType);
public:
    CWizmo();
    static void ShowHelp();
    void Blank();
    void StandBy();
    void Hibernate();
    void Lock();
    void LogOff();
    void ShutDown();
    void Exit();
    void InitShutdown(LPTSTR Message, DWORD timeOut, BOOL bReboot);
    void AbortShutdown();
    void SetMasterVolume(UINT nVolume);
    void MuteMasterVolume(bool bMute);
    void SetWaveVolume(UINT nVolume);
    void PlayFile(LPCTSTR lpFile);
    void OpenCloseDrive(char cDrive, bool bOpenDrive);
    void Reboot();
    void MonitorOff();
};
