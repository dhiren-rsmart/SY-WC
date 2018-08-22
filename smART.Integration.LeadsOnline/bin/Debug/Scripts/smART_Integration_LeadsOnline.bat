@ECHO OFF
CLS
ECHO *********************** Leads Online Scheduler Intaller ************************
SET currentDirPath=%~dp0
SET serviceName=RsmartLeadsOnlineSchedulerService
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
ECHO ********* Installing Rsmart Leads Online Scheduler Service *********
ECHO.
"%installUtilPath%" "%currentDirPath%\"smART.Integration.LeadsOnline.exe
REM C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil /i %installUtilPath%
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
ECHO **** Rsmart Leads Online Scheduler  installed sucessfully. ****
ECHO.
ECHO ********* Starting Rsmart Leads Online Scheduler  *********
ECHO.
NET START %serviceName%"
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
goto SUCCEDED
)

IF %userInput% EQU 2 (
ECHO ********* Un-Installing Rsmart Leads Online Scheduler  *********
ECHO.
"%installUtilPath%" /u "%currentDirPath%\"smART.Integration.LeadsOnline.exe
REM C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil /u %installUtilPath%
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
ECHO **** Rsmart Leads Online Scheduler un-installed sucessfully. ****
ECHO.
goto SUCCEDED
)

IF %userInput% EQU 3 (
ECHO ********* Starting Rsmart Leads Online Scheduler  *********
ECHO.
REM NET START %serviceName%
NET START "%serviceName%"
IF %ERRORLEVEL% GTR 1 goto Failed
ECHO.
goto SUCCEDED
)

IF %userInput% EQU 4 (
ECHO ********* Stopping Rsmart Leads Online Scheduler  *********
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