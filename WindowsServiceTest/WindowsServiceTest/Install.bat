@echo.start......
@echo off
@%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe WindowsServiceTest.exe
@net start ServiceTestAR 
@sc config ServiceTestAR start = auto
@echo off
@echo.over......
@pause