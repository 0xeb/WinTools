@echo off

:: check the Batchography book

setlocal

if not exist build (
    mkdir build
    pushd build
    cmake -A Win32 ..
    popd
)

if not exist build64 (
    mkdir build64
    pushd build64
    cmake -A x64 ..
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