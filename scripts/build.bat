echo off

set outputFile=Initialization.sql

echo /***** File generated: %date% *****/>%outputFile%

for %%i in (*.sql) do CALL :Subroutine %%i

:Subroutine

	if %1==%outputFile% goto :eof
	echo %1
	echo.>> %outputFile%
	echo GO >> %outputFile%
	echo.>> %outputFile%
	echo /***************** BEGINNING SCRIPT FILE: %1 *****************/ >> %outputFile%
	echo.>> %outputFile%
	type %1 >> %outputFile%
	
goto :eof

