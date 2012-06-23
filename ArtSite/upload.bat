rem "C:\Program Files (x86)\FileZilla FTP Client\filezilla.exe" ftp:richinoz:bollocks@richinoz.com put art1stleeds.zip



@echo off
echo ftp
echo user %2> ftpcmd.dat
echo B0llocks>> ftpcmd.dat
echo bin>> ftpcmd.dat
echo put %1>> ftpcmd.dat
echo quit>> ftpcmd.dat
ftp -n -s:ftpcmd.dat richinoz.com
del ftpcmd.dat

pause