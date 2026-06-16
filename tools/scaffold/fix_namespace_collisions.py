#!/usr/bin/env python3
"""
Düzleştirme sonrası oluşan 'entity adı == kardeş namespace' (CS0118) çakışmalarını
sadece etkilenen legacy aggregate servis/DbContext dosyalarında giderir: çakışan
domain entity tiplerine, namespace ile çakışmayan `{Entity}Entity` alias'ı ekler ve
o dosyadaki düz (qualified olmayan) kullanımları alias'a çevirir.
"""
import os, re

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))


def domain_entities(module):
    d = os.path.join(ROOT, "Energy.Domain", module, "Entities")
    if not os.path.isdir(d):
        return []
    return sorted(fn[:-3] for fn in os.listdir(d) if fn.endswith(".cs"))


def fix_file(rel, module, domain_ns, only=None):
    path = os.path.join(ROOT, rel)
    text = open(path, encoding="utf-8").read()
    entities = domain_entities(module)
    if only is not None:
        entities = [e for e in entities if e in only]
    # uzun adları önce işle (ChatMessageReaction, ChatMessage'tan önce)
    entities.sort(key=len, reverse=True)
    used = []
    for e in entities:
        # nokta veya kelime karakteriyle çevrili olmayan, tam kelime kullanımları
        pat = re.compile(r'(?<![\w.])' + re.escape(e) + r'(?![\w])')
        if pat.search(text):
            new_text = pat.sub(e + "Entity", text)
            if new_text != text:
                text = new_text
                used.append(e)
    if not used:
        return 0
    # alias satırlarını ilk using'den sonra ekle
    aliases = "".join(
        f"using {e}Entity = {domain_ns}.{e};\n" for e in sorted(used))
    # ilk 'using' satırının başına ekle (en üstte using bloğu varsayımı)
    m = re.search(r'^using [^\n]*\n', text, re.M)
    if m:
        text = text[:m.start()] + aliases + text[m.start():]
    else:
        text = aliases + text
    open(path, "w", encoding="utf-8").write(text)
    print(f"  {rel}: aliased {', '.join(sorted(used))}")
    return len(used)


def main():
    infra = os.path.join(ROOT, "Energy.Infrastructure")
    infra_top = {d for d in os.listdir(infra)
                 if os.path.isdir(os.path.join(infra, d))}

    # 1) Tüm aggregate servis dosyaları: Energy.Infrastructure/{Module}/Services/*.cs
    #    (entity servisleri {Module}/{Entity}/Services altında; onlar DbSet kullanır, çakışmaz)
    for module in sorted(infra_top):
        svc_dir = os.path.join(infra, module, "Services")
        if not os.path.isdir(svc_dir):
            continue
        for fn in os.listdir(svc_dir):
            if fn.endswith(".cs"):
                fix_file(f"Energy.Infrastructure/{module}/Services/{fn}",
                         module, f"Energy.Domain.{module}")

    # 2) Entity adı == bir üst-seviye Infrastructure klasörü olan domain tipleri,
    #    Persistence (AppDbContext) ve Seeding dosyalarında çakışır. Bunları alias'la.
    #    (entity -> module) eşlemesini Domain'den çıkar.
    ent_to_module = {}
    domain = os.path.join(ROOT, "Energy.Domain")
    for module in os.listdir(domain):
        ent_dir = os.path.join(domain, module, "Entities")
        if not os.path.isdir(ent_dir):
            continue
        for fn in os.listdir(ent_dir):
            if fn.endswith(".cs"):
                ent_to_module[fn[:-3]] = module
    colliding = {e: m for e, m in ent_to_module.items() if e in infra_top}

    for sub in ("Persistence", "Seeding"):
        base = os.path.join(infra, sub)
        for dp, _, fs in os.walk(base):
            for fn in fs:
                if not fn.endswith(".cs"):
                    continue
                rel = os.path.relpath(os.path.join(dp, fn), ROOT)
                # her çakışan entity için kendi modülünden alias
                for e, m in sorted(colliding.items()):
                    fix_file(rel, m, f"Energy.Domain.{m}", only={e})

    # 3) Tests projesi de düzleştirildi (Energy.Tests.{Module}); aynı çakışmalar.
    tests = os.path.join(ROOT, "Energy.Tests")
    for dp, _, fs in os.walk(tests):
        if os.sep + "bin" + os.sep in dp or os.sep + "obj" + os.sep in dp:
            continue
        for fn in fs:
            if not fn.endswith(".cs"):
                continue
            rel = os.path.relpath(os.path.join(dp, fn), ROOT)
            for e, m in sorted(colliding.items()):
                fix_file(rel, m, f"Energy.Domain.{m}", only={e})
    print("done")


if __name__ == "__main__":
    main()

