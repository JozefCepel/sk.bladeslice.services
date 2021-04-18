cd sk.bladeslice.ui

(Get-Content ".\packages\local\Dcore\src\common\Config.js").replace("RESOLVE_SVC: true", "RESOLVE_SVC: false") | Set-Content ".\packages\local\Dcore\src\common\Config.js";

npm start