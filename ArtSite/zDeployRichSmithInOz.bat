
@echo off

echo zipping...

del Art1stLeeds.zip

set path="C:\Program Files\WinRAR\";%path%

winrar a -afzip -r -xweb.config -x/myFiles -x*.bat -x*.cs -x*.csproj -x*.user -x*svn*\ -x*\svn\ Art1stLeeds

echo zipped!


echo uploading...

upload Art1stleeds.zip richsmithinoz
rem upload "C:\Users\rich\Documents\Visual Studio 2010\Projects\ArtSite\ArtSite\art1stleeds.zip"


pause