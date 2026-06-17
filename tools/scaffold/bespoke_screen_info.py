#!/usr/bin/env python3
"""Per-screen business-purpose intro for the bespoke IAM/Core/Profile/Dashboard/Chat
screens. Adds {Prefix}.HelpIntro (TR + EN + neutral). Idempotent."""
import os

BASE = os.path.join(os.path.dirname(__file__), "..", "..", "Energy.Localization", "Resources")

B = {
    "UsersScreen": (
        "Sistem kullanıcılarını yönetirsiniz: kullanıcı oluşturma, düzenleme ve silme; rol atama ile parola değiştirme işlemleri bu ekrandan yapılır.",
        "Manage system users: create, edit and delete users; assign roles and change passwords here."),
    "RolesScreen": (
        "Rolleri ve her role bağlı yetkileri yönetirsiniz; kullanıcıların erişim seviyeleri rollerle belirlenir.",
        "Manage roles and the permissions tied to each role; users' access levels are determined by roles."),
    "MenusScreen": (
        "Uygulama gezinme menüsünü (menü öğeleri, hiyerarşi, ikon, erişim yetkisi) yönetirsiniz.",
        "Manage the application navigation menu (menu items, hierarchy, icon, access permission)."),
    "PermissionsScreen": (
        "Sistemdeki yetki kataloğunu (modül/eylem bazında) görüntülersiniz; her yetki, bağlı rol, menü ve uç nokta sayılarıyla salt-okunur listelenir.",
        "View the system permission catalog (by module/action); each permission is listed read-only with its role, menu and endpoint counts."),
    "UserAccessScreen": (
        "Kullanıcı bazında erişimi yönetirsiniz: bir kullanıcıya rol atar ve rollerden gelen yetkilere ek olarak doğrudan yetkiler verirsiniz.",
        "Manage access per user: assign roles and grant direct permissions on top of role-inherited ones."),
    "ApiEndpointsScreen": (
        "API uç noktalarının erişim ayarlarını (gerekli yetki, etkin/pasif durum) yönetirsiniz.",
        "Manage access settings of API endpoints (required permission, active/inactive)."),
    "LogsScreen": (
        "Sistem denetim günlüklerini (API istekleri, kullanıcı, durum, hata) salt-okunur inceler ve kayıt detaylarını görüntülersiniz.",
        "Review system audit logs (API requests, user, status, error) read-only and view record details."),
    "SettingsScreen": (
        "Kişisel tercihlerinizi (bildirim/arama sesi, masaüstü bildirimleri, okundu bilgileri ve tema) yönetirsiniz.",
        "Manage your personal preferences (notification/call sound, desktop notifications, read receipts and theme)."),
    "LocalizationScreen": (
        "Arayüz çevirilerini (anahtar, Türkçe/İngilizce değer) ekler, düzenler ve resx kaynaklarından içe aktarırsınız.",
        "Add, edit and import UI translations (key, Turkish/English value) from resx resources."),
    "ProfileScreen": (
        "Kendi profil bilgilerinizi (kişisel bilgiler, profil resmi) görüntüler ve güncellersiniz; rolleriniz ve yetkileriniz de listelenir.",
        "View and update your own profile (personal info, profile picture); your roles and permissions are also listed."),
    "Dashboard": (
        "Sistemin genel durumunu özetleyen gösterge panelidir: kurulum hazırlık durumu, modül özetleri ve hızlı işlemler buradadır.",
        "A dashboard summarizing the overall system state: setup readiness, module summaries and quick actions."),
    "ChatScreen": (
        "Kullanıcılar arası anlık mesajlaşma ekranıdır; kişi seçer, mesaj ve dosya gönderir, gelen bildirimleri görürsünüz.",
        "An instant messaging screen between users; pick a contact, send messages and files, and see incoming notifications."),
}

FILES = {"SharedResource.tr-TR.resx": 0, "SharedResource.en-US.resx": 1, "SharedResource.resx": 1}

for fn, idx in FILES.items():
    p = os.path.join(BASE, fn)
    txt = open(p, encoding="utf-8").read()
    n = 0
    for pre, vals in B.items():
        name = f"{pre}.HelpIntro"
        if f'name="{name}"' in txt:
            continue
        block = f'  <data name="{name}" xml:space="preserve">\n    <value>{vals[idx]}</value>\n  </data>\n'
        txt = txt.replace("</root>", block + "</root>")
        n += 1
    open(p, "w", encoding="utf-8").write(txt)
    print(f"{fn}: +{n}")
print("Done.")

