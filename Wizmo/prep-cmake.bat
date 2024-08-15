@echo off

:: check the Batchography book

setlocal

if not exist build cmake .. -A Win32 -B build

if not exist build64 cmake -A x64 .. -B build64

if "%1"=="build" (
    cmake --build build --config Release
    cmake --build build64 --config Release
)

echo.
echo All done!
echo.