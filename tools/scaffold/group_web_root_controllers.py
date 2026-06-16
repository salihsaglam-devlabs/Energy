import os, re

root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
ctrl = os.path.join(root, "Energy.Web", "Controllers")

# Kök (app-shell) Web controller'larını Excel modül klasörlerine grupla (API ile simetri).
mapping = {
    "IAM": ["Users", "Roles", "Permissions", "Menus", "ApiEndpoints", "Account", "UserAccess", "Profile"],
    "Chat": ["Chat"],
    "Core": ["Localization", "Settings", "Logs", "Culture"],
    "Home": ["Home", "Dashboard"],
}

moved = 0
for folder, names in mapping.items():
    dest = os.path.join(ctrl, folder)
    os.makedirs(dest, exist_ok=True)
    for name in names:
        src = os.path.join(ctrl, f"{name}Controller.cs")
        if not os.path.exists(src):
            print("WARN missing:", src)
            continue
        with open(src, "r", encoding="utf-8") as f:
            text = f.read()
        # Tek tip kök namespace'i modül alt-namespace'ine çevir.
        text = re.sub(r"namespace Energy\.Web\.Controllers;",
                      f"namespace Energy.Web.Controllers.{folder};", text)
        with open(os.path.join(dest, f"{name}Controller.cs"), "w", encoding="utf-8") as f:
            f.write(text)
        os.remove(src)
        moved += 1

print(f"moved {moved} root web controllers into module folders")

