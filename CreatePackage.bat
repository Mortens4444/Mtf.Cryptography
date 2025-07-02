@echo off
setlocal enabledelayedexpansion

set "ProjectName=Mtf.Cryptography"
set "TargetDir=C:\NuGetTest"

REM for %%F in ("%TargetDir%\%ProjectName%.*.nupkg") do (
    REM set "FileName=%%~nxF"
    REM if not "!FileName!" == "!FileName:.symbols=!" (
        REM echo Deleting: %%F
        REM del "%%F"
    REM )
REM )

REM for %%F in ("%TargetDir%\%ProjectName%.*.nupkg") do call :ProcessFile "%%F"
REM goto :UpdatePackages

REM :ProcessFile
REM set "FilePath=%~1"
REM set "FileName=%~nx1"

REM call set "RestPart=%%FileName:%ProjectName%.=%%"
REM echo !RestPart! | findstr /R "^[0-9][0-9]*\.[0-9][0-9]*\.[0-9][0-9]*" >nul

REM if !errorlevel! == 0 (
    REM echo Deleting: !FilePath!
    REM del "!FilePath!"
REM )

REM goto :eof

REM :UpdatePackages

REM for /R %%P in (*.csproj) do (
    REM echo Checking: %%~nxP
    REM pushd %%~dpP
    REM FOR /F "tokens=1,2,*" %%A IN ('dotnet list package --outdated --source "%TargetDir%"') DO (
        REM IF "%%A"==">" (
            REM set "PackageName=%%B"
            REM echo   Updating: !PackageName! in %%~nxP
            REM dotnet add package !PackageName! -s "%TargetDir%"
            REM IF ERRORLEVEL 1 (
                REM echo     Error: !PackageName! update failed.
            REM ) ELSE (
                REM echo     OK: !PackageName! update success.
            REM )
            REM echo.
        REM )
    REM )
    REM popd
REM )

for /f %%V in ('powershell -ExecutionPolicy Bypass -File ".\IncrementPackageVersion.ps1" -CsprojFile "%ProjectName%\%ProjectName%.csproj"') do set "PackageVersion=%%V"
REM git add -A
REM git commit -m "%ProjectName% NuGet package release %PackageVersion%"
REM git push

dotnet pack --include-symbols --include-source %ProjectName%\%ProjectName%.csproj -c Release /p:IncludeSymbols=true /p:IncludeSource=true /p:DebugType=full /p:EmbedAllSources=true /p:Deterministic=true
move .\%ProjectName%\bin\Release\*.nupkg %TargetDir%
REM pause