import os, re
root = os.path.abspath("Energy.Application")
reqs=set(); targets=set()
req_re=re.compile(r'record\s+(\w+)[^;{]*?:\s*IRequest<')
h_re=re.compile(r'class\s+(\w+)\s*:\s*IRequestHandler<\s*([\w\.]+)\s*,')
for dp,_,fs in os.walk(root):
    if '/bin/' in dp or '/obj/' in dp: continue
    for fn in fs:
        if not fn.endswith('.cs'): continue
        flat=re.sub(r'\s+',' ',open(os.path.join(dp,fn),encoding='utf-8',errors='ignore').read())
        for m in req_re.finditer(flat): reqs.add(m.group(1))
        for m in h_re.finditer(flat): targets.add(m.group(2).split('.')[-1])
missing=sorted(reqs-targets); orphan=sorted(targets-reqs)
print("requests:",len(reqs)," handlers:",len(targets))
print("WITHOUT handler:",len(missing));  [print("  MISSING",n) for n in missing]
print("ORPHAN handlers:",len(orphan));    [print("  ORPHAN",h) for h in orphan]
