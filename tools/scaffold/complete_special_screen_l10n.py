#!/usr/bin/env python3
"""
Per-screen localization completion for the bespoke IAM / Core screens.

Each special screen (Users, Roles, Menus, Permissions, UserAccess, ApiEndpoints,
Logs, Settings, Localization) gets its OWN copy of the grid-button / notification
/ borrowed texts under its own "{Prefix}.*" section, so it no longer reads any
SHARED Grid.* / Notifications.* / cross-screen keys. Values are COPIED verbatim
from the existing shared entries (correct TR/EN), idempotently.
"""
import os, re

BASE = os.path.join(os.path.dirname(__file__), "..", "..", "Energy.Localization", "Resources")
FILES = ["SharedResource.resx", "SharedResource.tr-TR.resx", "SharedResource.en-US.resx"]

GRID = {
    "GridAdd": "Grid.Add", "GridEdit": "Grid.Edit", "GridDelete": "Grid.Delete",
    "GridSave": "Grid.Save", "GridCancel": "Grid.Cancel", "GridRefresh": "Grid.Refresh",
    "GridSearch": "Grid.Search", "GridLoading": "Grid.Loading", "GridNoData": "Grid.NoData",
    "GridActions": "Grid.Actions", "GridConfirmDelete": "Grid.ConfirmDelete",
}
NOTIF = {
    "NotifSaved": "Notifications.Saved", "NotifDeleted": "Notifications.Deleted",
    "NotifFailed": "Notifications.Failed", "NotifNetworkError": "Notifications.NetworkError",
}

SCREENS = {
    "UsersScreen":        {"grid": True,  "notif": True, "extra": {"FieldRequired": "Auth.FieldRequired"}},
    "RolesScreen":        {"grid": True,  "notif": True, "extra": {"PermissionLabel": "PermissionsScreen.Title"}},
    "MenusScreen":        {"grid": True,  "notif": True, "extra": {
        "ManagePermissions": "RolesScreen.ManagePermissions",
        "PermissionsTitle":  "RolesScreen.PermissionsTitle",
        "PermissionLabel":   "PermissionsScreen.Title"}},
    "PermissionsScreen":  {"grid": True,  "notif": True, "extra": {}},
    "UserAccessScreen":   {"grid": True,  "notif": True, "extra": {
        "FirstName": "UsersScreen.FirstName", "LastName": "UsersScreen.LastName",
        "UserName":  "UsersScreen.UserName",  "IsActive": "UsersScreen.IsActive"}},
    "ApiEndpointsScreen": {"grid": True,  "notif": True, "extra": {}},
    "LogsScreen":         {"grid": True,  "notif": True, "extra": {}},
    "SettingsScreen":     {"grid": False, "notif": True, "extra": {}},
    "LocalizationScreen": {"grid": True,  "notif": True, "extra": {}},
}


def parse(txt):
    d = {}
    for m in re.finditer(r'<data name="([^"]+)"[^>]*>\s*<value>(.*?)</value>', txt, re.S):
        d[m.group(1)] = m.group(2)
    return d


def build_targets(src):
    """Return {newKey: rawValue} for one resx file."""
    out = {}
    for prefix, cfg in SCREENS.items():
        pairs = {}
        if cfg["grid"]:
            pairs.update(GRID)
        if cfg["notif"]:
            pairs.update(NOTIF)
        pairs.update(cfg["extra"])
        for suffix, srckey in pairs.items():
            if srckey not in src:
                raise SystemExit(f"MISSING source key: {srckey}")
            out[f"{prefix}.{suffix}"] = src[srckey]
    return out


total = 0
for fn in FILES:
    path = os.path.join(BASE, fn)
    txt = open(path, encoding="utf-8").read()
    existing = parse(txt)
    targets = build_targets(existing)
    blocks = []
    for name, value in targets.items():
        if name in existing:
            continue
        blocks.append(
            f'  <data name="{name}" xml:space="preserve">\n    <value>{value}</value>\n  </data>\n'
        )
    if blocks:
        txt = txt.replace("</root>", "".join(blocks) + "</root>")
        open(path, "w", encoding="utf-8").write(txt)
    print(f"{fn}: +{len(blocks)} keys")
    total += len(blocks)

print(f"Total inserted: {total}")

