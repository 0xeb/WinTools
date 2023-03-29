@echo off

:: check the Batchography book

setlocal

if not exist build (
    mkdir build
    pushd build
    cmake -A Win32 -G "Visual Studio 16 2019" ..
    popd
)

if not exist build64 (
    mkdir build64
    pushd build64
    cmake -A x64 -G "Visual Studio 16 2019" ..
    popd
)

if "%1"=="build" (
    pushd build
    cmake --build . --config Release
    pushd ..\build64
    cmake --build . --config Release
    popd
)

echo.
echo All done!
echo.