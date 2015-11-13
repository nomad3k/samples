@echo off

SET DIR=%~d0%~p0%

SET project.name=CheeseShop
 SET sql.files.directory="%DIR%..\db\SQLServer\%project.name%"
SET server.database="(local)\SQLEXPRESS"
SET repository.path="git@sourceserver:CheeseShop.git"
SET version.file="_BuildInfo.xml"
SET version.xpath="//buildInfo/version"

"%DIR%Console\rh.exe" /d=%database.name% /f=%sql.files.directory% /s=%server.database% /vf=%version.file% /vx=%version.xpath% /r=%repository.path% /env=%environment% /simple

pause