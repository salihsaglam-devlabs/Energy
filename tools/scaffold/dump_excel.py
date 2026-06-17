import openpyxl, json, os

src = 'Energy.Web/wwwroot/docs/Energy_Teknik_Tasarim_Dokumani-v1.xlsx'
wb = openpyxl.load_workbook(src, data_only=True)
outdir = 'tools/scaffold/excel_dump'
os.makedirs(outdir, exist_ok=True)

summary = {}
for ws in wb.worksheets:
    rows = []
    for r in ws.iter_rows(values_only=True):
        if any(c is not None and str(c).strip() != '' for c in r):
            rows.append([('' if c is None else str(c)) for c in r])
    summary[ws.title] = len(rows)
    safe = ws.title.replace(' ', '_')
    with open(os.path.join(outdir, safe + '.json'), 'w', encoding='utf-8') as f:
        json.dump(rows, f, ensure_ascii=False, indent=1)
    # also tsv for quick view
    with open(os.path.join(outdir, safe + '.tsv'), 'w', encoding='utf-8') as f:
        for row in rows:
            f.write('\t'.join(row) + '\n')

print(json.dumps(summary, ensure_ascii=False, indent=2))

