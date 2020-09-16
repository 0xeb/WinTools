@echo off

:: checkout the Batchography book

setlocal

if not exist build (
    mkdir build
    pushd build
    cmake -A x64 -G "Visual Studio 15" ..
    popd
)

if not exist build64 (
    mkdir build64
    pushd build64
    cmake -A x64 -G "Visual Studio 15" ..
    popd
)

if "%1"=="build" (
    pushd build
    cmake --build . --config Release
    popd
    pushd build64
    cmake --build . --config Release
)

echo.
echo All done!
echo.