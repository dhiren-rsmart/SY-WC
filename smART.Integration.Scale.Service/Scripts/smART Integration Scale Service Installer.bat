@ECHO OFF
CLS
ECHO *********************** Notification Scheduler Service Intaller ************************
SET currentDirPath=%~dp0
SET serviceName=WeightWindowsServiceWCF
SET installUtilPath=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil
ECHO.
ECHO %currentDirPath%
ECHO.
ECHO Enter any option to continue.
ECHO.
ECHO To install service [Press 1].
ECHO To uninstall service [Press 2].
ECHO To start service [Press 3].
ECHO To stop service [Press 4].
:StartPoint
ECHO.
SET /p userInput="Enter your choice: "%=%

IF %userInput% GTR 4 (
ECHO.
goto InValidChoice
)

IF %userInput% LSS 1 (
ECHO.
goto InValidChoice
)

IF %userInput% EQU 1 (
ECHO NOTE:- Installer will ask for user account previledges to run windows service.
ECHO ********* Installing Notification Scheduler Service *********
ECHO.
"%installUtilPath%" "%currentDirPath%\"WCFOverWindowsService.exe
REM C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil /i %installUtilPath%
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
ECHO **** Notification Scheduler service installed sucessfully. ****
ECHO.
ECHO ********* Starting Notification Scheduler Service *********
ECHO.
NET START %serviceName%"
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
goto SUCCEDED
)

IF %userInput% EQU 2 (
ECHO ********* Un-Installing Notification Scheduler Service *********
ECHO.
"%installUtilPath%" /u "%currentDirPath%\"WCFOverWindowsService.exe
REM C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil /u %installUtilPath%
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
ECHO **** Notification Scheduler service un-installed sucessfully. ****
ECHO.
goto SUCCEDED
)

IF %userInput% EQU 3 (
ECHO ********* Starting Notification Scheduler Service *********
ECHO.
REM NET START %serviceName%
NET START "%serviceName%"
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
goto SUCCEDED
)

IF %userInput% EQU 4 (
ECHO ********* Stopping Notification Scheduler Service *********
ECHO.
REM NET STOP %serviceName%
NET STOP "%serviceName%"
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
goto SUCCEDED
)

:InValidChoice (
ECHO InValid Choice.
goto StartPoint
)

:Failed (
ECHO.
ECHO An error is occurred.
pause
)

:SUCCEDED (
pause
}