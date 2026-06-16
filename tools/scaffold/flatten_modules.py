#!/usr/bin/env python3
"""
Tüm solution'dan "Modules" sarmal klasörünü kaldırır: Domain/Application/Infrastructure/
Tests ve EF Configurations altındaki Modules/{M} içeriğini bir üst seviyeye taşır
(legacy {M} klasörleriyle birleşir). Ayrıca wwwroot/js|css/modules → js|css.

Çalışma modu:
  python3 flatten_modules.py            -> sadece ÇAKIŞMA tespiti (dry-run)
  python3 flatten_modules.py --apply    -> taşımayı uygular
Namespace/asset sed'leri AYRI yapılır (apply sonrası).
"""
from __future__ import annotations
import os, sys, shutil

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))

# (parent_dir, modules_dir_name)
TARGETS = [
    ("Energy.Domain", "Modules"),
    ("Energy.Application", "Modules"),
    ("Energy.Infrastructure", "Modules"),
    ("Energy.Infrastructure/Persistence/Configurations", "Modules"),
    ("Energy.Tests", "Modules"),
]
ASSET_MOVES = [
    ("Energy.Web/wwwroot/js/modules", "Energy.Web/wwwroot/js"),
    ("Energy.Web/wwwroot/css/modules", "Energy.Web/wwwroot/css"),
]


def plan_moves():
    moves = []          # (src_file, dst_file)
    collisions = []
    for parent_rel, mod in TARGETS:
        parent = os.path.join(ROOT, parent_rel)
        mod_dir = os.path.join(parent, mod)
        if not os.path.isdir(mod_dir):
            continue
        for dp, _, fs in os.walk(mod_dir):
            for fn in fs:
                src = os.path.join(dp, fn)
                rel = os.path.relpath(src, mod_dir)      # {M}/.../file
                dst = os.path.join(parent, rel)
                if os.path.exists(dst):
                    collisions.append(dst)
                moves.append((src, dst))
    # asset dirs (move subfolders of js/modules into js/)
    for src_rel, dst_rel in ASSET_MOVES:
        src_dir = os.path.join(ROOT, src_rel)
        dst_dir = os.path.join(ROOT, dst_rel)
        if not os.path.isdir(src_dir):
            continue
        for dp, _, fs in os.walk(src_dir):
            for fn in fs:
                src = os.path.join(dp, fn)
                rel = os.path.relpath(src, src_dir)
                dst = os.path.join(dst_dir, rel)
                if os.path.exists(dst):
                    collisions.append(dst)
                moves.append((src, dst))
    return moves, collisions


def main():
    apply = "--apply" in sys.argv
    moves, collisions = plan_moves()
    print(f"planned file moves: {len(moves)}")
    print(f"collisions: {len(collisions)}")
    for c in collisions[:50]:
        print("  COLLISION:", os.path.relpath(c, ROOT))
    if collisions:
        print("ABORT: çakışmalar var, taşıma yapılmadı.")
        return
    if not apply:
        print("DRY-RUN: çakışma yok. Uygulamak için --apply.")
        return
    for src, dst in moves:
        os.makedirs(os.path.dirname(dst), exist_ok=True)
        shutil.move(src, dst)
    # boş kalan Modules ve js/modules, css/modules dizinlerini temizle
    for parent_rel, mod in TARGETS:
        d = os.path.join(ROOT, parent_rel, mod)
        if os.path.isdir(d):
            shutil.rmtree(d, ignore_errors=True)
    for src_rel, _ in ASSET_MOVES:
        d = os.path.join(ROOT, src_rel)
        if os.path.isdir(d):
            shutil.rmtree(d, ignore_errors=True)
    print(f"APPLIED: {len(moves)} dosya taşındı, Modules klasörleri kaldırıldı.")


if __name__ == "__main__":
    main()

