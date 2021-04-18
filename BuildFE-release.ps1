cd sk.bladeslice.ui
# npm install

(Get-Content ".\packages\local\Dcore\src\common\Config.js").replace("RESOLVE_SVC: false", "RESOLVE_SVC: true") | Set-Content ".\packages\local\Dcore\src\common\Config.js";

# git pull
$version = Get-Random -Maximum 9999
$buildDate = Get-Date -Format "dd.MM.yyyy HH:mm"
.\node_modules\.bin\sencha config -prop buildNumber=$version then app build
(Get-Content ".\build\production\Esam\generatedFiles\desktop\app.js").replace("BASE_SVC_URL:'https://esam-dev.datalan.sk'", "BASE_SVC_URL:'http://localhost:85'") | Set-Content ".\build\production\Esam\generatedFiles\desktop\app.js";
(Get-Content ".\build\production\Esam\generatedFiles\desktop\app.js").replace("BUILD_NUMBER:''", "BUILD_NUMBER:'$version'") | Set-Content ".\build\production\Esam\generatedFiles\desktop\app.js";
(Get-Content ".\build\production\Esam\generatedFiles\desktop\app.js").replace("BUILD_DATE:''", "BUILD_DATE:'$buildDate'") | Set-Content ".\build\production\Esam\generatedFiles\desktop\app.js";

Write-Output ""
Write-Output "Skopirovanie OK. Press pause to close."

cmd /c pause | out-null  