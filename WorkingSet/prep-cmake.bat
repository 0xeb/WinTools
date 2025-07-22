@echo off

:: check the Batchography book

setlocal

if not exist build cmake -B build


if "%1"=="build" cmake --build build --config Release

echo.
echo All done!
echo.
