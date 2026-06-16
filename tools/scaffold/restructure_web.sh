#!/bin/zsh
# Energy.Web — Controllers/Views/Clients altındaki gereksiz "Modules" sarmalını kaldırır.
# Kurala göre: Controllers/{Module}, Views/{Module}, Clients/{Module} (Modules yok).
# wwwroot/js/modules ve css/modules KURALA GÖRE "modules" korur — dokunulmaz.
set -e
cd /Users/base/Codes/Energy/Energy.Web

for layer in Views Clients Controllers; do
  if [ -d "$layer/Modules" ]; then
    for d in "$layer"/Modules/*/; do
      n="$(basename "$d")"
      mv "$layer/Modules/$n" "$layer/$n"
    done
    rmdir "$layer/Modules"
  fi
done

# Namespace ve açık view yollarını düzelt (.cs + .cshtml)
fix() {
  local pat="$1" rep="$2"
  grep -rl "$pat" . --include='*.cs' --include='*.cshtml' 2>/dev/null | grep -vE '/bin/|/obj/' | while read -r f; do
    sed -i '' "s#$pat#$rep#g" "$f"
  done
}
fix 'Energy\.Web\.Controllers\.Modules\.' 'Energy.Web.Controllers.'
fix 'Energy\.Web\.Clients\.Modules\.' 'Energy.Web.Clients.'
fix '~/Views/Modules/' '~/Views/'

echo "remaining .Modules refs: $(grep -rl 'Web.Controllers.Modules\|Web.Clients.Modules\|~/Views/Modules/' . --include='*.cs' --include='*.cshtml' 2>/dev/null | grep -vE '/bin/|/obj/' | wc -l | tr -d ' ')"
echo "done"

