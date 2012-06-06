

@echo off

del Art1stLeeds.zip

set path="C:\Program Files\WinRAR\";%path%

winrar a -afzip -r -xweb.config -x/myFiles -x*.bat -x*.cs -x*.csproj -x*.user -x*svn*\ -x*\svn\ Art1stLeeds


--rem winrar a -afzip -r  -x*.config  -x/myFiles -x*.bat Art1stLeeds

--rem winrar a -afzip -r   -x*.config  -x/myFiles -x*.bat Art1stLeeds

--rem -x*.cs -x*.csproj -x*.user