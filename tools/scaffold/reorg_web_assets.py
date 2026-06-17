#!/usr/bin/env python3
"""
wwwroot/js ve css yapısını controller yapısına hizalar:
  * Ortak (shared) JS yardımcıları  js/app/*.js  -> js/common/*.js
  * Ortak CSS (app.css, dx.fluent..., icons/) -> css/common/
  * Cross-cutting sayfa scriptleri js/app/pages/*.js -> js/{iam|chat|core}/*.js
    (ilgili controller modülüne göre)
Tüm view/layout referanslarını günceller.
"""
import os, re, shutil

WEB = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", "Energy.Web"))
WWW = os.path.join(WEB, "wwwroot")

# Sayfa scripti -> modül klasörü
PAGE_MODULE = {
    "users": "iam", "roles": "iam", "permissions": "iam", "menus": "iam",
    "api-endpoints": "iam", "login": "iam", "profile": "iam", "user-access": "iam",
    "chat": "chat",
    "localization": "core", "logs": "core", "settings": "core",
}


def move(src, dst):
    if not os.path.exists(src):
        print("  (yok, atlandı):", src)
        return
    os.makedirs(os.path.dirname(dst), exist_ok=True)
    shutil.move(src, dst)


def main():
    js_app = os.path.join(WWW, "js", "app")

    # 1) Ortak JS yardımcıları -> js/common/
    common_js = os.path.join(WWW, "js", "common")
    os.makedirs(common_js, exist_ok=True)
    for fn in os.listdir(js_app):
        p = os.path.join(js_app, fn)
        if os.path.isfile(p) and fn.endswith(".js"):
            move(p, os.path.join(common_js, fn))

    # 2) Sayfa scriptleri -> js/{modül}/
    pages_dir = os.path.join(js_app, "pages")
    if os.path.isdir(pages_dir):
        for fn in os.listdir(pages_dir):
            if not fn.endswith(".js"):
                continue
            page = fn[:-3]
            module = PAGE_MODULE.get(page, "core")
            move(os.path.join(pages_dir, fn),
                 os.path.join(WWW, "js", module, fn))

    # 3) js/app kalıntısını temizle
    shutil.rmtree(js_app, ignore_errors=True)

    # 4) Ortak CSS -> css/common/
    common_css = os.path.join(WWW, "css", "common")
    os.makedirs(common_css, exist_ok=True)
    for item in ("app.css", "dx.fluent.energy-custom-scheme.css", "icons"):
        src = os.path.join(WWW, "css", item)
        if os.path.exists(src):
            move(src, os.path.join(common_css, item))

    # 5) Referansları güncelle (tüm .cshtml)
    helper_re = re.compile(r'~/js/app/([\w-]+\.js)')       # js/app/X.js (pages/ HARİÇ)
    page_re = re.compile(r'~/js/app/pages/([\w-]+)\.js')   # js/app/pages/X.js
    changed = 0
    for dp, _, fs in os.walk(WEB):
        if os.sep + "bin" + os.sep in dp or os.sep + "obj" + os.sep in dp:
            continue
        for fn in fs:
            if not fn.endswith(".cshtml"):
                continue
            p = os.path.join(dp, fn)
            t = open(p, encoding="utf-8").read()
            o = t
            t = page_re.sub(lambda m: f"~/js/{PAGE_MODULE.get(m.group(1), 'core')}/{m.group(1)}.js", t)
            t = helper_re.sub(r'~/js/common/\1', t)
            t = t.replace("~/css/app.css", "~/css/common/app.css")
            t = t.replace("~/css/dx.fluent.energy-custom-scheme.css",
                          "~/css/common/dx.fluent.energy-custom-scheme.css")
            if t != o:
                open(p, "w", encoding="utf-8").write(t)
                changed += 1
    print(f"updated {changed} cshtml files")


if __name__ == "__main__":
    main()

