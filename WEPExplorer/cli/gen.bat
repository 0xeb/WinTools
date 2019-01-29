@echo off

:: 
:: The Batchography book by Elias Bachaalany
::

setlocal enabledelayedexpansion

if "%1"=="grep" (
for /F "usebackq tokens=3*" %%a in (`findstr /B /C:"static LPCWSTR XML_"  WEPExplorer.cpp`) DO (
  set Z=%%b
  set Z=!Z: L"= "!
  echo public const string %%a !Z!
)
  goto :eof
)

call %0 grep | sort | clip