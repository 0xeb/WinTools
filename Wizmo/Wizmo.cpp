#include "Wizmo.h"
#include <stdio.h>
#pragma comment(lib, "winmm")

//----------------------------------------------------------------------------------
void CWizmo::MonitorOff()
{
    ::SendMessage(HWND_TOPMOST, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
}

//----------------------------------------------------------------------------------
// Reboots windows
void CWizmo::Reboot()
{
    MyExitWindowsEx(EWX_REBOOT);
}

//----------------------------------------------------------------------------------
// taken from http://www.codeguru.com/system/cdr.html
void CWizmo::OpenCloseDrive(char cDrive, bool bOpenDrive)
{
    MCI_OPEN_PARMS op = { 0 };
    MCI_STATUS_PARMS st;
    DWORD flags;
    TCHAR szDriveName[4];

    _tcscpy(szDriveName, _T("X:"));
    op.lpstrDeviceType = (LPCSTR)MCI_DEVTYPE_CD_AUDIO;
    if (cDrive > 1)
    {
        szDriveName[0] = cDrive;
        op.lpstrElementName = szDriveName;
        flags = MCI_OPEN_TYPE
            | MCI_OPEN_TYPE_ID
            | MCI_OPEN_ELEMENT
            | MCI_OPEN_SHAREABLE;
    }
    else flags = MCI_OPEN_TYPE
        | MCI_OPEN_TYPE_ID
        | MCI_OPEN_SHAREABLE;

    if (!mciSendCommand(0, MCI_OPEN, flags, (DWORD_PTR)&op))
    {
        st.dwItem = MCI_STATUS_READY;
        if (bOpenDrive)
            mciSendCommand(op.wDeviceID, MCI_SET, MCI_SET_DOOR_OPEN, 0);
        else
            mciSendCommand(op.wDeviceID, MCI_SET, MCI_SET_DOOR_CLOSED, 0);
        mciSendCommand(op.wDeviceID, MCI_CLOSE, MCI_WAIT, 0);
    }
}


//----------------------------------------------------------------------------------
// Play wave file(s).
// multiple files can be passed if seperated by a comma
void CWizmo::PlayFile(LPCTSTR lpFile)
{
    LPTSTR szFiles = new TCHAR[_tcslen(lpFile) + 1];
    LPTSTR token, seps = _T(",");
    _tcscpy(szFiles, lpFile);

    token = _tcstok(szFiles, seps);
    while (token != NULL)
    {
        ::sndPlaySound(token, SND_SYNC);
        token = _tcstok(NULL, seps);
    }
    delete[] szFiles;
}

//----------------------------------------------------------------------------------
// Adjusts wave volume. It makes both left and right channel of same value
void CWizmo::SetWaveVolume(UINT nVolume)
{
    // check how many wave devices exist
    if (!::waveOutGetNumDevs())
        return;

    WAVEOUTCAPS woc;
    ::waveOutGetDevCaps(WAVE_MAPPER, &woc, sizeof(WAVEOUTCAPS));
    if ((woc.dwSupport & WAVECAPS_VOLUME) == 0)
        return;
    nVolume &= 0xFFFF;
    waveOutSetVolume((HWAVEOUT)UINT_PTR(WAVE_MAPPER), nVolume | (nVolume << 16));
}

//----------------------------------------------------------------------------------
// Mutes or Unmutes speakers
void CWizmo::MuteMasterVolume(bool bMute)
{
    SpeakersAdjust(bMute ? 1 : 0, MIXERCONTROL_CONTROLTYPE_MUTE);
}

//----------------------------------------------------------------------------------
// Adjust the master volume
// Maximum value is 65535
// or what is specified by MIXERCONTROL.Bounds
void CWizmo::SetMasterVolume(UINT nVolume)
{
    SpeakersAdjust(nVolume, MIXERCONTROL_CONTROLTYPE_VOLUME);
}

//----------------------------------------------------------------------------------
// Adjusts a value in the speaker line / specific control
void CWizmo::SpeakersAdjust(int nValue, int aControlType)
{
    HMIXER hmixer;
    MIXERCAPS mxc = { 0 };
    MIXERLINE mxl = { 0 };
    MIXERLINECONTROLS mxlc = { 0 };
    PMIXERCONTROL pmxctrl = NULL;
    MIXERCONTROLDETAILS mxcd = { 0 };

    // Open mixer device
    if (mixerOpen(&hmixer, 0, 0, 0, MIXER_OBJECTF_MIXER) != MMSYSERR_NOERROR)
        return;

    do
    {
        // get number of lines
        if (mixerGetDevCaps((UINT_PTR)hmixer, &mxc, sizeof(mxc)) != MMSYSERR_NOERROR)
            break;

        // search for speaker
        do
        {
            mxl.cbStruct = sizeof(MIXERLINE);
            mxl.dwDestination = --mxc.cDestinations;
            mixerGetLineInfo((HMIXEROBJ)hmixer, &mxl, MIXER_GETLINEINFOF_DESTINATION);
            if (mxl.dwComponentType == MIXERLINE_COMPONENTTYPE_DST_SPEAKERS)
                break;
        } while (mxc.cDestinations);

        // doesn't have any controls ?
        mxlc.cControls = mxl.cControls;
        if (mxl.cControls == 0)
            break;

        // get all control lines
        pmxctrl = new MIXERCONTROL[mxl.cControls];
        mxlc.cbStruct = sizeof(MIXERLINECONTROLS);
        mxlc.dwLineID = mxl.dwLineID;
        mxlc.pamxctrl = pmxctrl;
        mxlc.cbmxctrl = sizeof(MIXERCONTROL);
        if (mixerGetLineControls((HMIXEROBJ)hmixer, &mxlc, MIXER_GETLINECONTROLSF_ALL) != MMSYSERR_NOERROR)
            break;

        // set the control details
        mxcd.cbStruct = sizeof(MIXERCONTROLDETAILS);
        mxcd.cChannels = 1;
        mxcd.cMultipleItems = 0;
        mxcd.cbDetails = sizeof(MIXERCONTROLDETAILS_UNSIGNED);
        mxcd.paDetails = &nValue;
        for (UINT i = 0; i < mxl.cControls; i++)
        {
            // is this the control we are accessing ?
            if (pmxctrl[i].dwControlType == aControlType)
            {
                mxcd.dwControlID = pmxctrl[i].dwControlID;
                mixerSetControlDetails((HMIXEROBJ)hmixer, &mxcd, MIXER_OBJECTF_MIXER);
                break;
            }
        }
    } while (false);

    if (pmxctrl != NULL)
        delete[] pmxctrl;
    mixerClose(hmixer);
}


//----------------------------------------------------------------------------------
// Turns off the PC
void CWizmo::ShutDown()
{
    MyExitWindowsEx(EWX_POWEROFF);
}

//----------------------------------------------------------------------------------
// Shutdowns the system and doesn't necessarily turn PC off or reboot
void CWizmo::Exit()
{
    MyExitWindowsEx(EWX_SHUTDOWN);
}

bool CWizmo::MyExitWindowsEx(UINT uFlags)
{
    AdjustShutdownPrivilege();
    if (m_bForce)
        uFlags |= EWX_FORCE;
    return ::ExitWindowsEx(uFlags, 0) ? true : false;
}

//----------------------------------------------------------------------------------
// Initiates a system shutdown sequence w/ a given timeout and error message
void CWizmo::InitShutdown(LPTSTR Message, DWORD timeOut, BOOL bReboot)
{
    AdjustShutdownPrivilege();
    ::InitiateSystemShutdown(m_szMachineName, Message, timeOut, m_bForce, bReboot);
}

//----------------------------------------------------------------------------------
// Aborts a previously initiated shutdown sequence
void CWizmo::AbortShutdown()
{
    AdjustShutdownPrivilege();
    ::AbortSystemShutdown(m_szMachineName);
}


//----------------------------------------------------------------------------------
// Constructor
CWizmo::CWizmo()
{
    m_hInstance = (HINSTANCE) ::GetModuleHandle(NULL);
    m_bForce = FALSE;
    m_szMachineName = NULL;
}

//----------------------------------------------------------------------------------
void CWizmo::ShowHelp()
{
    printf("usage:\n"
        "\t-monoff: turns the monitor off\n"
        "\t-abortshutdown: aborts an initiated system shutdown\n"
        "\t-play: plays multimedia file (or files separated with a comma\n"
        "\t-lock: locks workstation\n"
        "\t-hibernate: hibernates the system\n"
        "\t-logoff: logs off current user\n"
        "\t-mute [0|1]: mutes (default) / unmutes master volume\n"
        "\t-reboot: reboots the system\n"
        "\t-blank\n"
        "\t-eject/-close DRIVE_LETTER: opens or closes tray\n"
        "\n"
        "Example\n"
        "-------\n"
        "\twizmo -monoff\n"
        "\n");
}

//----------------------------------------------------------------------------------
// Determines whether this is a WinNT system or not
bool CWizmo::IsWinNT()
{
    return (GetVersion() < 0x80000000);
}

//----------------------------------------------------------------------------------
// Gives the current process the shutdown privileges which will enable it to:
// change power state, restart system, ...
void CWizmo::AdjustShutdownPrivilege()
{
    if (!IsWinNT())
        return;

    HANDLE hToken;
    if (!OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY | TOKEN_ADJUST_PRIVILEGES, &hToken))
        return;

    TOKEN_PRIVILEGES tkp = { 0 };
    if (!LookupPrivilegeValue(0, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid))
        return;

    tkp.PrivilegeCount = 1;
    tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
    AdjustTokenPrivileges(hToken, FALSE, &tkp, 0, NULL, NULL);
}

//----------------------------------------------------------------------------------
// Logs off the system
void CWizmo::LogOff()
{
    MyExitWindowsEx(EWX_LOGOFF);
}

//----------------------------------------------------------------------------------
// Puts the system on stand by
void CWizmo::StandBy()
{
    AdjustShutdownPrivilege();
    SetSystemPowerState(TRUE, m_bForce);
}

//----------------------------------------------------------------------------------
// Hibernates the system
void CWizmo::Hibernate()
{
    AdjustShutdownPrivilege();
    SetSystemPowerState(FALSE, m_bForce);
}

//----------------------------------------------------------------------------------
// Simply invokes the screen saver
void CWizmo::Blank()
{
    SendMessage(HWND_TOPMOST, WM_SYSCOMMAND, SC_SCREENSAVE, 0);
}

//----------------------------------------------------------------------------------
//
bool CWizmo::MakeCoverageWindow()
{
    LPTSTR szCoverageWindow = _T("WizmoCoverageWindow");

    WNDCLASSEX cls = { 0 };

    cls.cbSize = sizeof(WNDCLASSEX);
    cls.hInstance = m_hInstance;
    cls.lpszClassName = szCoverageWindow;
    cls.lpfnWndProc = ::DefWindowProc;

    if (!::RegisterClassEx(&cls))
        return false;

    // values for the window coordinates
    int mx, my, x, y;

    if (mx = GetSystemMetrics(SM_CXVIRTUALSCREEN))
    {
        my = GetSystemMetrics(SM_CYVIRTUALSCREEN);
        x = GetSystemMetrics(SM_XVIRTUALSCREEN);
        y = GetSystemMetrics(SM_YVIRTUALSCREEN);
    }
    else
    {
        x = y = 0;
        mx = GetSystemMetrics(SM_CXSCREEN);
        my = GetSystemMetrics(SM_CYSCREEN);
    }
#ifdef __MYDEBUG
    mx = 100;
    my = 100;
    x = 10;
    y = 10;
#endif
    m_hwndCoverage =
        ::CreateWindowEx(WS_EX_TOPMOST, szCoverageWindow, "szCoverageWindow",
            WS_POPUP | WS_VISIBLE,
            x, y, mx, my, NULL, NULL, m_hInstance, NULL);
#ifndef __MYDEBUG
    SetFocus(m_hwndCoverage);
    SetCapture(m_hwndCoverage);
    SetForegroundWindow(m_hwndCoverage);
#endif

    MSG msg;
    BOOL bRet;
    while ((bRet = GetMessage(&msg, m_hwndCoverage, 0, 0)) != 0)
    {
        if (bRet == -1)
            break;
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
    UnregisterClass(szCoverageWindow, m_hInstance);
    return true;
}

//----------------------------------------------------------------------------------
// Locks the current workstation
void CWizmo::Lock()
{
    LockWorkStation();
}
