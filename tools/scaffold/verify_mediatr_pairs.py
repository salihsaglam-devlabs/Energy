import os
import re

root = os.path.join(os.path.dirname(__file__), "..", "..", "Energy.Application")
root = os.path.abspath(root)

reqs = []          # (request_type_name, file)
handler_targets = set()  # request type names that have a handler

req_re1 = re.compile(r'record\s+(\w+)\s*\([^)]*\)\s*:?\s*\n?\s*:?\s*IRequest<')
req_re_simple = re.compile(r'record\s+(\w+)\b[^\n]*IRequest<')
handler_re = re.compile(r'class\s+(\w+)\s*\n?\s*:\s*IRequestHandler<\s*([\w\.]+)\s*,')

for dp, _, fs in os.walk(root):
    if os.sep + "bin" + os.sep in dp or os.sep + "obj" + os.sep in dp:
        continue
    for fn in fs:
        if not fn.endswith(".cs"):
            continue
        text = open(os.path.join(dp, fn), encoding="utf-8", errors="ignore").read()
        for m in req_re_simple.finditer(text):
            reqs.append((m.group(1), os.path.join(dp, fn)))
        for m in handler_re.finditer(text):
            handler_targets.add(m.group(2).split(".")[-1])

req_names = {r[0] for r in reqs}
missing = sorted({r[0] for r in reqs if r[0] not in handler_targets})
orphan = sorted({h for h in handler_targets if h not in req_names})

print("total request types:", len(req_names))
print("request types WITH handler:", len(req_names & handler_targets))
print("requests WITHOUT a handler:", len(missing))
for n in missing[:60]:
    print("   MISSING HANDLER ->", n)
print("handlers for unknown request:", len(orphan))
for h in orphan[:60]:
    print("   ORPHAN HANDLER ->", h)

