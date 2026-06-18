# Energy — İş Süreçleri ve Veri Modeli Kılavuzu

**Versiyon:** 1.0  
**Tarih:** Haziran 2026  
**Durum:** Kurumsal Referans Doküman

---

> Bu doküman Energy uygulamasının tüm iş süreçlerini, veri modelini, durum geçişlerini, karar noktalarını ve operasyon prosedürlerini kapsar. Bu dokümanı okuyan bir kişi sistemi hiç görmemiş olsa bile baştan sona anlayabilmelidir.

---

## İçindekiler

1. [Sisteme Genel Bakış](#1-sisteme-genel-bakış)
2. [Modül Haritası](#2-modül-haritası)
3. [Ortak Altyapı — Core Modülü](#3-ortak-altyapı--core-modülü)
4. [Kimlik ve Erişim Yönetimi — IAM Modülü](#4-kimlik-ve-erişim-yönetimi--iam-modülü)
5. [Organizasyon Yönetimi — Organization Modülü](#5-organizasyon-yönetimi--organization-modülü)
6. [İnsan Kaynakları — HR Modülü](#6-insan-kaynakları--hr-modülü)
7. [İş Ortakları — BusinessPartners Modülü](#7-iş-ortakları--businesspartners-modülü)
8. [Proje Yönetimi — Projects Modülü](#8-proje-yönetimi--projects-modülü)
9. [Malzeme Kataloğu — Catalog Modülü](#9-malzeme-kataloğu--catalog-modülü)
10. [Stok Yönetimi — Inventory Modülü](#10-stok-yönetimi--inventory-modülü)
11. [Talep Yönetimi — Requests Modülü](#11-talep-yönetimi--requests-modülü)
12. [Satın Alma — Procurement Modülü](#12-satın-alma--procurement-modülü)
13. [Operasyon ve İş Emirleri — Operations Modülü](#13-operasyon-ve-iş-emirleri--operations-modülü)
14. [Saha Operasyonları — FieldOperations Modülü](#14-saha-operasyonları--fieldoperations-modülü)
15. [Varlık ve Ekipman Yönetimi — Assets Modülü](#15-varlık-ve-ekipman-yönetimi--assets-modülü)
16. [Finans Yönetimi — Finance Modülü](#16-finans-yönetimi--finance-modülü)
17. [Bütçe Yönetimi — Budget Modülü](#17-bütçe-yönetimi--budget-modülü)
18. [Sözleşme Yönetimi — Contracts Modülü](#18-sözleşme-yönetimi--contracts-modülü)
19. [Hakediş Yönetimi — ProgressPayments Modülü](#19-hakediş-yönetimi--progresspayments-modülü)
20. [Belge Yönetimi — Documents Modülü](#20-belge-yönetimi--documents-modülü)
21. [Onay Akışı Motoru — Workflow Modülü](#21-onay-akışı-motoru--workflow-modülü)
22. [Bildirim Yönetimi — Notifications Modülü](#22-bildirim-yönetimi--notifications-modülü)
23. [Sohbet — Chat Modülü](#23-sohbet--chat-modülü)
24. [Raporlama — Reporting Modülü](#24-raporlama--reporting-modülü)
25. [Uçtan Uca Ana Süreç Akışları](#25-uçtan-uca-ana-süreç-akışları)
26. [Sistem Geneli İş Kuralları](#26-sistem-geneli-iş-kuralları)

---

# 1. Sisteme Genel Bakış

## 1.1 Sistemin Amacı

Energy, enerji ve inşaat sektöründe faaliyet gösteren şirketlerin proje bazlı operasyonlarını uçtan uca yönetmesini sağlayan kurumsal bir iş yönetim platformudur.

Sistem şu temel iş ihtiyaçlarını karşılar:

- Proje bazlı malzeme talep, satın alma ve stok yönetimi
- Saha operasyonları takibi (iş emirleri, günlük saha raporları, metraj)
- Tedarikçi ve müşteri sözleşmeleri ile hakediş yönetimi
- Ön muhasebe: borç, alacak, ödeme ve tahsilat
- Bütçe planlama ve maliyet kontrol
- Dinamik onay akışları (tek kişi, sıralı, paralel, quorum)
- Personel ve ekipman takibi

## 1.2 Sistemin Kapsamı

| Katman | İçerik |
|--------|--------|
| Modül sayısı | 22 |
| Tablo sayısı | 134 |
| İlişki sayısı | 539+ |
| Onay modeli | Sequential, ParallelAny, ParallelAll, Quorum |
| Çoklu dil desteği | Evet (LocalizationResources) |
| Çoklu şirket desteği | Evet (Companies + Branches) |
| Çoklu para birimi | Evet (Currencies + ExchangeRates) |

## 1.3 Sistem Geneli Ortak Tasarım İlkeleri

### Soft Delete
Her tablo `IsDeleted`, `DeletedAt`, `DeletedBy` alanlarına sahiptir. Hiçbir kayıt fiziksel olarak silinmez; pasif hale getirilir.

### Audit Trail
Her tablo `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` alanlarına sahiptir. Her değişikliğin kimin tarafından ne zaman yapıldığı izlenir.

### Belge Numarası
`SequenceDefinitions` tablosu üzerinden her belge türü için otomatik numara üretimi yapılır.

### Bağımsız Modül İlkesi
Her modül kendi tablolarını sahiplenir. Modüller arası bağlantı zorunlu değilse nullable foreign key veya `RelatedModule / RelatedEntityType / RelatedEntityId` yaklaşımıyla kurulur.

### Ana Veri Akışı (ER Overview)

```
[Talep] → [Teklif] → [Satın Alma Siparişi] → [Mal Kabul] → [Stok] → [Maliyet]
[Proje] → [Proje Fazları] → [İş Emirleri] → [Saha Raporları] → [Metraj] → [Hakediş]
[Personel] → [Proje Üyeleri] → [Puantaj] → [Finans Hareketi]
[Sözleşme] → [Sözleşme Kalemleri] → [Hakediş] → [Alacak/Borç]
[İş Ortağı] → [Borç/Alacak] → [Ödeme/Tahsilat] → [Kapatma]
```

---

# 2. Modül Haritası

| Modül | Tablo Sayısı | Bağımlılık | Açıklama |
|-------|-------------|-----------|----------|
| Core | 11 | Yok (temel) | Şirket, şube, döviz, ölçü birimi, sistem ayarları |
| IAM | 9 | Core | Kullanıcı, rol, izin, menü yönetimi |
| Organization | 7 | IAM | Personel, pozisyon, yetkinlik, izin, masraf |
| HR | 2 | Organization, Projects | Puantaj |
| BusinessPartners | 4 | Core | Müşteri, tedarikçi, taşeron |
| Projects | 7 | Core, BusinessPartners | Proje kartları, fazlar, üyeler |
| Catalog | 8 | Core | Malzeme kategorileri, markalar, öznitelikler |
| Inventory | 14 | Core, Catalog, Projects | Depolar, stok hareketleri, rezervasyonlar |
| Requests | 3 | Projects, Inventory | Malzeme talep süreçleri |
| Procurement | 8 | Requests, Inventory, BusinessPartners | Teklif, sipariş, mal kabul, fatura |
| Operations | 8 | Projects, Inventory | İş emirleri, atamalar, malzeme planlaması |
| FieldOperations | 7 | Projects, Operations | Saha raporları, metraj, ilerleme |
| Assets | 3 | Core, Projects | Ekipman, atama, bakım |
| Finance | 10 | BusinessPartners, Projects | Ön muhasebe, ödeme, tahsilat |
| Budget | 2 | Projects, Finance | Bütçe planlama |
| Contracts | 4 | BusinessPartners, Projects | Sözleşme ve protokol |
| ProgressPayments | 3 | Contracts, Projects | Hakediş |
| Documents | 5 | Tüm modüller | Belge arşivleme |
| Workflow | 10 | IAM | Onay akışı motoru |
| Notifications | 3 | IAM | Sistem bildirimleri |
| Chat | 4 | IAM | Anlık mesajlaşma |
| Reporting | 2 | Tüm modüller | Rapor ve dashboard tanımları |

---

# 3. Ortak Altyapı — Core Modülü

## 3.1 İş Amacı

Core modülü, sistemin çalışması için gerekli temel tanımlamaları barındırır. Diğer tüm modüller Core modülüne bağımlıdır. Buradaki veriler genellikle sistem kurulumunda bir kez girilir ve nadiren değişir.

## 3.2 Kapsam

| Paydaş | Rolü |
|--------|------|
| Sistem Yöneticisi | Tüm Core tanımları oluşturur ve yönetir |
| Muhasebe Ekibi | Para birimi ve kur tanımlarını günceller |
| Tüm Kullanıcılar | Core verileri okuma modunda kullanır |

## 3.3 Tablolar

### Companies — Şirket Tanımları

**İş Amacı:** Sistemin çalıştığı tüzel kişilikleri tanımlar. Çoklu şirket yapısını destekler.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar (UUID) |
| Name | nvarchar | Evet | Şirket ticari ünvanı |
| TaxNumber | nvarchar | Hayır | Vergi numarası |
| TaxOffice | nvarchar | Hayır | Vergi dairesi |
| Address | nvarchar | Hayır | Şirket merkez adresi |
| Phone | nvarchar | Hayır | İletişim telefonu |
| Email | nvarchar | Hayır | İletişim e-postası |
| LogoUrl | nvarchar | Hayır | Şirket logosu dosya yolu |
| IsActive | bit | Evet | Şirket aktif mi? |
| CreatedAt | datetime2 | Evet | Oluşturma zamanı |
| CreatedBy | uniqueidentifier | Hayır | Oluşturan kullanıcı (→ Users.Id) |
| UpdatedAt | datetime2 | Hayır | Son güncelleme zamanı |
| UpdatedBy | uniqueidentifier | Hayır | Güncelleyen kullanıcı (→ Users.Id) |
| IsDeleted | bit | Evet | Soft delete bayrağı (varsayılan: 0) |
| DeletedAt | datetime2 | Hayır | Silinme zamanı |
| DeletedBy | uniqueidentifier | Hayır | Silen kullanıcı (→ Users.Id) |

**Yaşam Döngüsü:**
- **Oluşturma:** Sistem kurulumunda Sistem Yöneticisi tarafından oluşturulur
- **Güncelleme:** Şirket bilgilerinde değişiklik olduğunda güncellenir
- **Pasifleştirme:** IsActive=false yapılarak devre dışı bırakılır (silinmez)

---

### Branches — Şube Tanımları

**İş Amacı:** Şirkete bağlı bölge, ofis veya şubeleri tanımlar.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| CompanyId | uniqueidentifier | Evet | Bağlı şirket (→ Companies.Id) |
| Name | nvarchar | Evet | Şube adı |
| Code | nvarchar | Evet | Kısa kod |
| Address | nvarchar | Hayır | Şube adresi |
| Phone | nvarchar | Hayır | İletişim telefonu |
| IsActive | bit | Evet | Aktif durumu |
| CreatedAt | datetime2 | Evet | Oluşturma zamanı |
| CreatedBy | uniqueidentifier | Hayır | Oluşturan kullanıcı |
| UpdatedAt | datetime2 | Hayır | Son güncelleme |
| UpdatedBy | uniqueidentifier | Hayır | Güncelleyen kullanıcı |
| IsDeleted | bit | Evet | Soft delete |
| DeletedAt | datetime2 | Hayır | Silinme zamanı |
| DeletedBy | uniqueidentifier | Hayır | Silen kullanıcı |

---

### Departments — Departman Hiyerarşisi

**İş Amacı:** Organizasyonun departman ağacını tanımlar. Kendi kendine referans vererek hiyerarşik yapıyı destekler.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| CompanyId | uniqueidentifier | Evet | Bağlı şirket (→ Companies.Id) |
| ParentDepartmentId | uniqueidentifier | Hayır | Üst departman (→ Departments.Id, self-ref) |
| Name | nvarchar | Evet | Departman adı |
| Code | nvarchar | Evet | Kısa kod |
| ManagerId | uniqueidentifier | Hayır | Departman yöneticisi (→ Employees.Id) |
| IsActive | bit | Evet | Aktif durumu |
| ... | ... | ... | (audit alanları) |

**İlişki Diyagramı:**
```
Companies (1) ──< Departments (N)
Departments (1) ──< Departments (N)   [ParentDepartmentId — self-referencing hiyerarşi]
```

---

### Currencies — Para Birimleri

**İş Amacı:** Sistemde kullanılan tüm para birimlerini tanımlar. Çoklu para birimi desteği için kullanılır.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Code | nvarchar(3) | Evet | ISO 4217 kodu (TRY, USD, EUR) |
| Name | nvarchar | Evet | Para birimi adı |
| Symbol | nvarchar(5) | Hayır | Sembol (₺, $, €) |
| IsBaseCurrency | bit | Evet | Temel para birimi mi? |
| IsActive | bit | Evet | Aktif durumu |
| ... | ... | ... | (audit alanları) |

**Başlangıç Verileri:** TRY, USD, EUR

---

### ExchangeRates — Kur Kayıtları

**İş Amacı:** Günlük ya da periyodik döviz kurlarını kayıt altında tutar. Çoklu para birimi hesaplamaları için kullanılır.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| FromCurrencyId | uniqueidentifier | Evet | Kaynak para birimi (→ Currencies.Id) |
| ToCurrencyId | uniqueidentifier | Evet | Hedef para birimi (→ Currencies.Id) |
| Rate | decimal(18,6) | Evet | Kur değeri |
| RateDate | date | Evet | Kurun geçerli olduğu tarih |
| Source | nvarchar | Hayır | Kaynak (TCMB, Manuel, vb.) |
| ... | ... | ... | (audit alanları) |

**İş Kuralı:** Aynı para birimi çifti için aynı tarihte sadece bir aktif kur kaydı bulunabilir.

---

### UnitsOfMeasure — Ölçü Birimleri

**İş Amacı:** Malzeme miktarlarında kullanılan ölçü birimlerini tanımlar.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Code | nvarchar | Evet | Kısa kod (Piece, Meter, Kg, Ton, vb.) |
| Name | nvarchar | Evet | Uzun ad |
| UnitType | nvarchar | Hayır | Birim tipi (Uzunluk, Ağırlık, Hacim vb.) |
| IsActive | bit | Evet | Aktif durumu |
| ... | ... | ... | (audit alanları) |

**Başlangıç Verileri:** Piece, Meter, Kilogram, Ton, Liter, Hour, Day, Roll, Package

---

### UnitConversions — Birim Dönüşümleri

**İş Amacı:** İki ölçü birimi arasındaki çevirimi tanımlar.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| FromUnitId | uniqueidentifier | Evet | Kaynak birim (→ UnitsOfMeasure.Id) |
| ToUnitId | uniqueidentifier | Evet | Hedef birim (→ UnitsOfMeasure.Id) |
| Factor | decimal(18,6) | Evet | Çevrim katsayısı |
| ... | ... | ... | (audit alanları) |

---

### SequenceDefinitions — Belge Numarası Üretim Tanımları

**İş Amacı:** Her belge türü için otomatik numara üretimini kontrol eder.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| EntityType | nvarchar | Evet | Hangi belge türü (Request, PurchaseOrder, vb.) |
| Prefix | nvarchar | Hayır | Önek (REQ, PO, WO, vb.) |
| Suffix | nvarchar | Hayır | Sonek |
| Pattern | nvarchar | Evet | Desen ({PREFIX}-{YEAR}-{SEQ:5}) |
| CurrentValue | int | Evet | Mevcut sayaç değeri |
| ResetPeriod | nvarchar | Hayır | Sıfırlama periyodu (Yearly, Monthly, Never) |
| LastResetDate | datetime2 | Hayır | Son sıfırlama tarihi |
| ... | ... | ... | (audit alanları) |

**İş Akışı:**
```
Yeni Belge Talebi
      ↓
SequenceDefinitions tablosunda EntityType = 'PurchaseOrder' kaydı bulunur
      ↓
CurrentValue +1 arttırılır (transaction içinde kilitlenir)
      ↓
Pattern uygulanarak belge numarası üretilir: PO-2026-00042
      ↓
Belge oluşturulur, üretilen numara atanır
```

---

### SystemSettings — Sistem Genel Ayarları

**İş Amacı:** Sistemin davranışını etkileyen global konfigürasyon değerlerini saklar.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Key | nvarchar | Evet | Ayar anahtarı |
| Value | nvarchar | Evet | Ayar değeri |
| ValueType | nvarchar | Hayır | Veri tipi (string, int, bool, json) |
| Description | nvarchar | Hayır | Ayarın açıklaması |
| IsPublic | bit | Evet | Kullanıcılara görünür mü? |
| ... | ... | ... | (audit alanları) |

---

### LocalizationResources — Çok Dilli Metin Kaynakları

**İş Amacı:** Arayüz metinlerinin birden fazla dilde saklanmasını sağlar.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Key | nvarchar | Evet | Metin anahtarı |
| LanguageCode | nvarchar(5) | Evet | Dil kodu (tr, en, de, vb.) |
| Value | nvarchar | Evet | Çeviri metni |
| Module | nvarchar | Hayır | İlgili modül |
| ... | ... | ... | (audit alanları) |

---

### AuditLogs — İstek ve Kritik İşlem Kayıtları

**İş Amacı:** Sistemde gerçekleştirilen kritik işlemlerin ve API isteklerinin izlenebilirliğini sağlar.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| UserId | uniqueidentifier | Hayır | İşlemi yapan kullanıcı |
| EntityType | nvarchar | Evet | Etkilenen nesne tipi |
| EntityId | uniqueidentifier | Hayır | Etkilenen nesne ID'si |
| Action | nvarchar | Evet | İşlem tipi (Create, Update, Delete, Approve, vb.) |
| OldValues | nvarchar(max) | Hayır | Önceki değerler (JSON) |
| NewValues | nvarchar(max) | Hayır | Yeni değerler (JSON) |
| IpAddress | nvarchar | Hayır | İstemci IP adresi |
| UserAgent | nvarchar | Hayır | Tarayıcı bilgisi |
| RequestPath | nvarchar | Hayır | API endpoint yolu |
| StatusCode | int | Hayır | HTTP durum kodu |
| Duration | int | Hayır | İşlem süresi (ms) |
| CreatedAt | datetime2 | Evet | Kayıt zamanı |
| CreatedBy | uniqueidentifier | Hayır | Oluşturan kullanıcı |
| IsDeleted | bit | Evet | Soft delete |
| ... | ... | ... | (diğer audit alanları) |

**Önemli Kural:** AuditLogs kayıtları hiçbir zaman güncellenmez veya silinmez; yalnızca ekleme yapılır.

## 3.4 Tablo İlişkileri

```
Companies (1) ──────< Branches (N)
Companies (1) ──────< Departments (N)
Departments (1) ────< Departments (N)   [self-referencing]
Currencies (1) ──────< ExchangeRates.FromCurrency (N)
Currencies (1) ──────< ExchangeRates.ToCurrency (N)
UnitsOfMeasure (1) ──< UnitConversions.FromUnit (N)
UnitsOfMeasure (1) ──< UnitConversions.ToUnit (N)
```

---

# 4. Kimlik ve Erişim Yönetimi — IAM Modülü

## 4.1 İş Amacı

IAM modülü, sistemin güvenliğini sağlamak için kullanıcı kimlik doğrulama, yetkilendirme ve erişim kontrolü işlemlerini yönetir. Hangi kullanıcının hangi işlemleri yapabileceğini, hangi menülere erişebileceğini ve hangi API endpoint'lerini çağırabileceğini belirler.

## 4.2 Kapsam

| Paydaş | Rolü |
|--------|------|
| Sistem Yöneticisi | Kullanıcı ve rol yönetimi |
| IT Ekibi | Teknik izin tanımlamaları |
| Tüm Kullanıcılar | Oturum açma ve kendi profilini görüntüleme |

## 4.3 Süreci Başlatan Olaylar

- Yeni çalışan sisteme dahil edildiğinde
- Kullanıcının rolü değiştiğinde
- İzin istisnası tanımlanması gerektiğinde
- Menü yapısında değişiklik gerektiğinde

## 4.4 Uçtan Uca Süreç Akışı

### Kullanıcı Oluşturma ve Yetkilendirme

```
1. Sistem Yöneticisi yeni kullanıcı bilgilerini girer
   → Users tablosuna kayıt oluşturulur
   → Şifre hash'lenerek saklanır

2. Kullanıcıya rol atanır
   → UserRoles tablosuna kayıt eklenir
   → Rol üzerinden tanımlı tüm izinler otomatik aktif olur

3. (Opsiyonel) Kullanıcıya bireysel izin istisnası tanımlanır
   → UserPermissions tablosuna kayıt eklenir
   → IsGranted=true ise ekstra izin verilir
   → IsGranted=false ise rol izni override edilir (kısıtlama)

4. Kullanıcı tercihleri ayarlanır
   → UserSettings tablosuna kayıt oluşturulur (dil, tema, vb.)
```

### Kullanıcı Oturum Açma ve Yetki Kontrolü

```
1. Kullanıcı kullanıcı adı + şifre girer
2. Users tablosunda eşleşme aranır
3. Şifre doğrulanır
4. JWT token veya session oluşturulur
5. Her API isteğinde:
   → Token doğrulanır
   → ApiEndpoints tablosunda endpoint'e atanmış izin kodu alınır
   → Kullanıcının bu izne sahip olup olmadığı kontrol edilir:
      a) UserPermissions'da doğrudan tanımlıysa → direkt karar
      b) UserRoles → RolePermissions üzerinden kontrol edilir
6. Erişim izinli → işlem devam eder
7. Erişim reddedildi → 403 Forbidden döner, AuditLogs'a yazılır
```

## 4.5 Durum Yönetimi

| Durum | Ne Zaman | Tetikleyen Olay |
|-------|----------|----------------|
| Active | Kullanıcı oluşturulduğunda | Sistem Yöneticisi oluşturma |
| Inactive | Kullanıcı devre dışı bırakıldığında | IsActive=false yapılması |
| Locked | Çok fazla başarısız giriş | Sistem otomatik kilitleme |
| Deleted | Soft delete uygulandığında | IsDeleted=true |

## 4.6 Tablolar

### Users — Kullanıcı Hesapları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Username | nvarchar | Evet | Kullanıcı adı (benzersiz) |
| Email | nvarchar | Evet | E-posta adresi (benzersiz) |
| PasswordHash | nvarchar | Evet | Hash'lenmiş şifre |
| FirstName | nvarchar | Evet | Ad |
| LastName | nvarchar | Evet | Soyad |
| PhoneNumber | nvarchar | Hayır | Telefon numarası |
| AvatarUrl | nvarchar | Hayır | Profil fotoğrafı |
| IsActive | bit | Evet | Aktif durumu |
| IsLocked | bit | Evet | Kilitli mi? |
| LastLoginAt | datetime2 | Hayır | Son giriş zamanı |
| FailedLoginCount | int | Evet | Başarısız giriş sayısı |
| EmployeeId | uniqueidentifier | Hayır | Bağlı personel kaydı (→ Employees.Id) |
| CreatedAt | datetime2 | Evet | Oluşturma zamanı |
| CreatedBy | uniqueidentifier | Hayır | Oluşturan kullanıcı |
| UpdatedAt | datetime2 | Hayır | Son güncelleme |
| UpdatedBy | uniqueidentifier | Hayır | Güncelleyen kullanıcı |
| IsDeleted | bit | Evet | Soft delete |
| DeletedAt | datetime2 | Hayır | Silinme zamanı |
| DeletedBy | uniqueidentifier | Hayır | Silen kullanıcı |

---

### Roles — Roller

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Name | nvarchar | Evet | Rol adı (benzersiz) |
| Description | nvarchar | Hayır | Açıklama |
| IsSystemRole | bit | Evet | Sistem tarafından tanımlı mı? |
| IsActive | bit | Evet | Aktif durumu |
| ... | ... | ... | (audit alanları) |

**Başlangıç Rolleri:** Admin, ProjectManager, WarehouseManager, PurchaseManager, FinanceManager, HRManager, SiteSupervisor

---

### Permissions — İzin Kataloğu

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Code | nvarchar | Evet | İzin kodu (benzersiz, örn: Inventory.StockDocument.Create) |
| Name | nvarchar | Evet | İzin adı |
| Module | nvarchar | Hayır | İlgili modül |
| Description | nvarchar | Hayır | Açıklama |
| ... | ... | ... | (audit alanları) |

**İzin Kodu Standartları:**
- `Default.Read` — Temel okuma
- `Default.Create` — Oluşturma
- `Default.Update` — Güncelleme
- `Default.Delete` — Silme
- `Default.ReadAll` — Tüm kayıtları okuma

---

### UserRoles — Kullanıcı Rol Bağlantıları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| UserId | uniqueidentifier | Evet | Kullanıcı (→ Users.Id, CASCADE DELETE) |
| RoleId | uniqueidentifier | Evet | Rol (→ Roles.Id) |
| ValidFrom | datetime2 | Hayır | Geçerlilik başlangıcı |
| ValidTo | datetime2 | Hayır | Geçerlilik bitişi |
| ... | ... | ... | (audit alanları) |

---

### RolePermissions — Rol İzin Bağlantıları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| RoleId | uniqueidentifier | Evet | Rol (→ Roles.Id, CASCADE DELETE) |
| PermissionCode | nvarchar | Evet | İzin kodu (→ Permissions.Code) |
| ... | ... | ... | (audit alanları) |

---

### UserPermissions — Kullanıcı Bazlı İzin İstisnaları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| UserId | uniqueidentifier | Evet | Kullanıcı (→ Users.Id, CASCADE DELETE) |
| PermissionCode | nvarchar | Evet | İzin kodu (→ Permissions.Code) |
| IsGranted | bit | Evet | true=ek izin, false=kısıtlama |
| Reason | nvarchar | Hayır | Neden tanımlandığı |
| ValidFrom | datetime2 | Hayır | Geçerlilik başlangıcı |
| ValidTo | datetime2 | Hayır | Geçerlilik bitişi |
| ... | ... | ... | (audit alanları) |

---

### Menus — Menü Ağacı

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ParentId | uniqueidentifier | Hayır | Üst menü (→ Menus.Id) |
| Name | nvarchar | Evet | Menü adı |
| Route | nvarchar | Hayır | URL yolu |
| Icon | nvarchar | Hayır | İkon kodu |
| SortOrder | int | Evet | Sıralama |
| RequiredPermissionCode | nvarchar | Hayır | Görüntüleme için gerekli izin |
| IsActive | bit | Evet | Aktif durumu |
| ... | ... | ... | (audit alanları) |

---

### ApiEndpoints — API Endpoint İzin Eşleştirmeleri

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| HttpMethod | nvarchar | Evet | GET, POST, PUT, DELETE |
| Path | nvarchar | Evet | API yolu (/api/v1/purchase-orders) |
| RequiredPermissionCode | nvarchar | Hayır | Gerekli izin kodu |
| IsPublic | bit | Evet | Kimlik doğrulama gerektirmez mi? |
| Description | nvarchar | Hayır | Açıklama |
| ... | ... | ... | (audit alanları) |

---

### UserSettings — Kullanıcı Tercihleri

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| UserId | uniqueidentifier | Evet | Kullanıcı (→ Users.Id) |
| Key | nvarchar | Evet | Tercih anahtarı |
| Value | nvarchar | Evet | Tercih değeri |
| ... | ... | ... | (audit alanları) |

**Örnek Tercihler:** language=tr, theme=dark, itemsPerPage=25, defaultCurrency=TRY

## 4.7 Yetki Hesaplama Mantığı

```
Kullanıcının İzin Seti =
    (Kullanıcının tüm rollerinden gelen izinler)
    UNION
    (UserPermissions.IsGranted=true olanlar)
    MINUS
    (UserPermissions.IsGranted=false olanlar)
```

## 4.8 Tablo İlişkileri

```
Users (1) ──< UserRoles (N) >── Roles (1)
Roles (1) ──< RolePermissions (N) >── Permissions (1)
Users (1) ──< UserPermissions (N) >── Permissions (1)
Menus (1) ──< Menus (N)   [self-referencing]
Menus (N) >── Permissions (1)   [RequiredPermissionCode]
ApiEndpoints (N) >── Permissions (1)   [RequiredPermissionCode]
Users (1) ──< UserSettings (N)
```

## 4.9 İş Kuralları

- Kullanıcı en az bir role sahip olmalıdır
- Silinen rol, UserRoles kayıtlarını cascade siler
- Admin rolü silinemez ve devre dışı bırakılamaz
- 5 başarısız giriş denemesinde kullanıcı hesabı otomatik kilitlenir
- Şifre değiştirme işlemi AuditLogs'a yazılır, eski şifre hash'i saklanmaz
- UserPermissions'daki kısıtlama (IsGranted=false), rol izinlerini override eder

## 4.10 Hata Senaryoları

| Hata | Açıklama | Çözüm |
|------|----------|-------|
| Yanlış şifre | FailedLoginCount artırılır | 5. denemede hesap kilitlenir |
| Hesap kilitli | Login engellenir | Sistem Yöneticisi IsLocked=false yapar |
| İzin yok | 403 döner, audit log yazılır | Yönetici uygun rol/izin ekler |
| Silinen kullanıcı | Token geçersiz | Yeni hesap oluşturulur |

---

# 5. Organizasyon Yönetimi — Organization Modülü

## 5.1 İş Amacı

Şirketin insan kaynağını yönetir: personel kartları, pozisyon tanımları, yetkinlikler, izin talepleri ve masraf talepleri. Diğer modüllerde (Projeler, İş Emirleri, Puantaj) personel atamalarının kaynağı bu modüldür.

## 5.2 Kapsam

| Paydaş | Rolü |
|--------|------|
| İK Yöneticisi | Personel kayıtları ve izin/masraf onayları |
| Departman Yöneticisi | Kendi ekibinin izin ve masraf taleplerini onaylar |
| Personel | İzin talebi oluşturur, masraf bildirir |
| Proje Yöneticisi | Masraf onayları (proje bazlı) |

## 5.3 Tablolar

### Employees — Personel Kartları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| EmployeeNumber | nvarchar | Evet | Sicil numarası (benzersiz) |
| FirstName | nvarchar | Evet | Ad |
| LastName | nvarchar | Evet | Soyad |
| NationalId | nvarchar | Hayır | TC kimlik no |
| BirthDate | date | Hayır | Doğum tarihi |
| HireDate | date | Evet | İşe başlama tarihi |
| TerminationDate | date | Hayır | İşten ayrılma tarihi |
| DepartmentId | uniqueidentifier | Hayır | Bağlı departman (→ Departments.Id) |
| PositionId | uniqueidentifier | Hayır | Pozisyon (→ EmployeePositions.Id) |
| UserId | uniqueidentifier | Hayır | Bağlı kullanıcı hesabı (→ Users.Id) |
| ManagerId | uniqueidentifier | Hayır | Yöneticisi (→ Employees.Id, self-ref) |
| Email | nvarchar | Hayır | Kurumsal e-posta |
| Phone | nvarchar | Hayır | Telefon |
| EmploymentType | nvarchar | Hayır | İstihdam türü (FullTime, PartTime, Contractor) |
| IsActive | bit | Evet | Aktif mi? |
| ... | ... | ... | (audit alanları) |

---

### EmployeePositions — Pozisyon Tanımları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Name | nvarchar | Evet | Pozisyon adı |
| Code | nvarchar | Evet | Kısa kod |
| DepartmentId | uniqueidentifier | Hayır | Bağlı departman |
| IsActive | bit | Evet | Aktif durumu |
| ... | ... | ... | (audit alanları) |

---

### EmployeeSkills ve EmployeeSkillAssignments — Yetkinlik Sistemi

**EmployeeSkills:** Şirkette tanımlı yetkinlik kataloğu (Elektrik Montajı, Kaynak, Vinç Operatörlüğü vb.)

**EmployeeSkillAssignments:** Hangi personelin hangi yetkinliğe sahip olduğu

| Alan (EmployeeSkillAssignments) | Tip | Açıklama |
|-------------------------------|-----|----------|
| EmployeeId | uniqueidentifier | Personel (→ Employees.Id) |
| SkillId | uniqueidentifier | Yetkinlik (→ EmployeeSkills.Id) |
| ProficiencyLevel | nvarchar | Seviye (Beginner, Intermediate, Expert) |
| CertificateNo | nvarchar | Sertifika numarası |
| CertificateExpiry | date | Sertifika bitiş tarihi |

---

### LeaveRequests — İzin Talepleri

**İş Amacı:** Personelin yıllık izin, mazeret izni, hastalık izni gibi talepleri ve onay süreçleri.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| EmployeeId | uniqueidentifier | Evet | İzin talep eden personel |
| LeaveType | nvarchar | Evet | İzin türü (Annual, Sick, Personal, vb.) |
| StartDate | date | Evet | İzin başlangıç tarihi |
| EndDate | date | Evet | İzin bitiş tarihi |
| TotalDays | decimal | Evet | Toplam gün sayısı |
| Reason | nvarchar | Hayır | İzin gerekçesi |
| Status | nvarchar | Evet | Draft → PendingApproval → Approved / Rejected |
| ApprovedBy | uniqueidentifier | Hayır | Onaylayan yönetici |
| ApprovedAt | datetime2 | Hayır | Onay zamanı |
| RejectionReason | nvarchar | Hayır | Ret gerekçesi |
| ... | ... | ... | (audit alanları) |

**Durum Geçiş Diyagramı:**
```
[Draft] ──(Talebi Gönder)──> [PendingApproval]
[PendingApproval] ──(Onayla)──> [Approved]
[PendingApproval] ──(Reddet)──> [Rejected]
[PendingApproval] ──(İptal Et)──> [Cancelled]
[Draft] ──(İptal Et)──> [Cancelled]
```

---

### ExpenseClaims — Personel Masraf Talepleri

**İş Amacı:** Personelin iş seyahati, yakıt, konaklama gibi masraflarını sisteme bildirmesi ve onaylatması.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ClaimNumber | nvarchar | Evet | Talep numarası (otomatik) |
| EmployeeId | uniqueidentifier | Evet | Masrafı bildiren personel |
| ProjectId | uniqueidentifier | Hayır | İlgili proje |
| ClaimDate | date | Evet | Talep tarihi |
| TotalAmount | decimal(18,2) | Evet | Toplam tutar |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| Status | nvarchar | Evet | Draft → PendingApproval → Approved → Paid |
| Description | nvarchar | Hayır | Genel açıklama |
| ApprovalRequestId | uniqueidentifier | Hayır | Onay talebi (→ ApprovalRequests.Id) |
| ... | ... | ... | (audit alanları) |

---

### ExpenseClaimLines — Masraf Satırları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ExpenseClaimId | uniqueidentifier | Evet | Masraf başlığı |
| ExpenseType | nvarchar | Evet | Masraf tipi (Fuel, Accommodation, Meal, Travel, vb.) |
| ExpenseDate | date | Evet | Masraf tarihi |
| Amount | decimal(18,2) | Evet | Tutar |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| Description | nvarchar | Hayır | Açıklama |
| ReceiptUrl | nvarchar | Hayır | Fiş/fatura dosyası |
| ... | ... | ... | (audit alanları) |

## 5.4 Masraf Talebi Süreci Akışı

```
1. Personel yeni masraf talebi oluşturur
   → ExpenseClaims: Status=Draft
   → ExpenseClaimLines satırları eklenir

2. Personel talebi onaya gönderir
   → ExpenseClaims: Status=PendingApproval
   → Workflow motoru APR-EXPENSE akışını başlatır
   → ApprovalRequests kaydı oluşturulur

3a. Tutar < 5000 ise:
    → Proje Yöneticisi VEYA Departman Yöneticisi onaylaması yeterlidir (ParallelAny)
    → ExpenseClaims: Status=Approved

3b. Tutar >= 5000 ise:
    → Adım 1: Proje Yöneticisi onayı (Sequential)
    → Adım 2: Finans Yöneticisi onayı (Sequential)
    → ExpenseClaims: Status=Approved

4. Onaylanan masraf ödeme sürecine girer
   → Finance modülünde Payables kaydı oluşturulur
   → ExpenseClaims: Status=Paid
```

## 5.5 İş Kuralları

- Personel aynı tarih aralığı için birden fazla onaylı izin talebine sahip olamaz
- Masraf satırı en az bir fiş/fatura belgesiyle desteklenmelidir (iş kuralı — opsiyonel zorunluluk)
- İşten ayrılan personelin (TerminationDate dolu) açık talepleri sistem tarafından uyarı üretir
- Masraf onayı için güncel imza sirküleri geçerlidir; imza yetkisi aşıldığında bir üst makama eskalasyon yapılır

## 5.6 Tablo İlişkileri

```
Employees (1) ──< LeaveRequests (N)
Employees (1) ──< ExpenseClaims (N)
ExpenseClaims (1) ──< ExpenseClaimLines (N)
Employees (1) ──< EmployeeSkillAssignments (N) >── EmployeeSkills (1)
Employees (1) ──< Employees (N)   [ManagerId, self-ref]
Departments (1) ──< Employees (N)
EmployeePositions (1) ──< Employees (N)
```

---

# 6. İnsan Kaynakları — HR Modülü

## 6.1 İş Amacı

Personelin projelerdeki ve genel çalışmadaki zaman kayıtlarını tutar. Puantaj verileri maliyet hesaplamaları için Finance modülüne beslenir.

## 6.2 Tablolar

### Timesheets — Puantaj Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| EmployeeId | uniqueidentifier | Evet | Personel (→ Employees.Id) |
| PeriodStart | date | Evet | Dönem başlangıcı |
| PeriodEnd | date | Evet | Dönem bitişi |
| TotalHours | decimal | Evet | Toplam çalışma saati |
| Status | nvarchar | Evet | Draft → Submitted → Approved |
| ApprovedBy | uniqueidentifier | Hayır | Onaylayan |
| ApprovedAt | datetime2 | Hayır | Onay zamanı |
| ... | ... | ... | (audit alanları) |

### TimesheetLines — Puantaj Satırları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| TimesheetId | uniqueidentifier | Evet | Başlık (→ Timesheets.Id) |
| WorkDate | date | Evet | Çalışma tarihi |
| ProjectId | uniqueidentifier | Hayır | İlgili proje |
| WorkOrderId | uniqueidentifier | Hayır | İlgili iş emri |
| Hours | decimal | Evet | Çalışma saati |
| WorkType | nvarchar | Hayır | Normal, Overtime, Weekend vb. |
| Description | nvarchar | Hayır | İş açıklaması |
| ... | ... | ... | (audit alanları) |

## 6.3 Veri Akışı

```
Personel günlük çalışma saatlerini girer
         ↓
TimesheetLines → Timesheets (toplanır)
         ↓
Yönetici puantajı onaylar
         ↓
Finance modülü: Personel maliyeti FinancialTransactions olarak kaydedilir
         ↓
Budget modülü: Gerçekleşen maliyet bütçe ile karşılaştırılır
```

---

# 7. İş Ortakları — BusinessPartners Modülü

## 7.1 İş Amacı

Müşteriler, tedarikçiler, taşeronlar ve diğer cari taraflara ait temel bilgileri, iletişim kişilerini, adresleri ve banka hesaplarını tek bir yapıda yönetir.

## 7.2 Kapsam

| Paydaş | Rolü |
|--------|------|
| Satın Alma Ekibi | Tedarikçi ve taşeron kayıtları |
| Finans Ekibi | Banka hesabı doğrulama, ödeme |
| Proje Yöneticisi | Müşteri ve sözleşme bilgileri |

## 7.3 İş Ortağı Türleri (PartnerType)

| Değer | Açıklama |
|-------|----------|
| Customer | Müşteri — alacak yaratır, hakediş alıcısı |
| Supplier | Tedarikçi — borç yaratır, ödeme alıcısı |
| Subcontractor | Taşeron — iş emri ve hakediş ile çalışır |
| Other | Diğer — sınıflandırılmamış |

## 7.4 Tablolar

### BusinessPartners — İş Ortakları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Code | nvarchar | Evet | Cari kodu (benzersiz, otomatik) |
| Name | nvarchar | Evet | Ticaret ünvanı |
| ShortName | nvarchar | Hayır | Kısa ad |
| PartnerType | nvarchar | Evet | Customer / Supplier / Subcontractor / Other |
| TaxNumber | nvarchar | Hayır | Vergi numarası |
| TaxOffice | nvarchar | Hayır | Vergi dairesi |
| IsVatExempt | bit | Evet | KDV muaf mı? |
| CurrencyId | uniqueidentifier | Hayır | Varsayılan para birimi |
| CreditLimit | decimal(18,2) | Hayır | Kredi limiti |
| PaymentTermDays | int | Hayır | Ödeme vadesi (gün) |
| IsActive | bit | Evet | Aktif mi? |
| ... | ... | ... | (audit alanları) |

---

### BusinessPartnerContacts — Cari İletişim Kişileri

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| BusinessPartnerId | uniqueidentifier | Evet | Cari (→ BusinessPartners.Id) |
| FirstName | nvarchar | Evet | Ad |
| LastName | nvarchar | Evet | Soyad |
| Title | nvarchar | Hayır | Ünvan |
| Email | nvarchar | Hayır | E-posta |
| Phone | nvarchar | Hayır | Telefon |
| IsPrimary | bit | Evet | Ana iletişim kişisi mi? |
| ... | ... | ... | (audit alanları) |

---

### BusinessPartnerAddresses — Cari Adresleri

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| BusinessPartnerId | uniqueidentifier | Evet | Cari (→ BusinessPartners.Id) |
| AddressType | nvarchar | Evet | Billing / Shipping / Legal |
| AddressLine1 | nvarchar | Evet | Adres satırı 1 |
| AddressLine2 | nvarchar | Hayır | Adres satırı 2 |
| City | nvarchar | Evet | Şehir |
| District | nvarchar | Hayır | İlçe |
| PostalCode | nvarchar | Hayır | Posta kodu |
| Country | nvarchar | Evet | Ülke |
| IsPrimary | bit | Evet | Birincil adres mi? |
| ... | ... | ... | (audit alanları) |

---

### BusinessPartnerBankAccounts — Cari Banka Hesapları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| BusinessPartnerId | uniqueidentifier | Evet | Cari (→ BusinessPartners.Id) |
| BankName | nvarchar | Evet | Banka adı |
| BranchName | nvarchar | Hayır | Şube adı |
| AccountNumber | nvarchar | Evet | Hesap numarası |
| IBAN | nvarchar | Hayır | IBAN numarası |
| CurrencyId | uniqueidentifier | Hayır | Hesap para birimi |
| IsActive | bit | Evet | Aktif mi? |
| IsPrimary | bit | Evet | Birincil hesap mı? |
| ... | ... | ... | (audit alanları) |

## 7.5 Tablo İlişkileri

```
BusinessPartners (1) ──< BusinessPartnerContacts (N)
BusinessPartners (1) ──< BusinessPartnerAddresses (N)
BusinessPartners (1) ──< BusinessPartnerBankAccounts (N)
BusinessPartners (1) ──< PurchaseOrders (N)       [Procurement]
BusinessPartners (1) ──< SupplierInvoices (N)     [Procurement]
BusinessPartners (1) ──< Contracts (N)             [Contracts]
BusinessPartners (1) ──< Payables (N)              [Finance]
BusinessPartners (1) ──< Receivables (N)           [Finance]
```

## 7.6 İş Kuralları

- Vergi numarası benzersiz olmalıdır (aynı vergi numarasıyla iki aktif cari olamaz)
- Silinmiş cari, aktif sipariş veya fatura varsa silinemez
- Ödeme için mutlaka aktif bir banka hesabı tanımlı olmalıdır
- Kredi limiti aşıldığında sistem uyarı verir (engelleme iş kuralına göre konfigüre edilir)

---

# 8. Proje Yönetimi — Projects Modülü

## 8.1 İş Amacı

Şirketin yürüttüğü projeleri tanımlar, faz/WBS kırılımını oluşturur, proje ekibini atar ve proje süresince oluşan notları saklar. Stok, satın alma, iş emirleri, saha raporları ve hakediş süreçlerinin bağlandığı merkezi referans noktasıdır.

## 8.2 Proje Yaşam Döngüsü

```
[Draft] ──(Aktifleştir)──> [Active]
[Active] ──(Askıya Al)──> [OnHold]
[OnHold] ──(Devam Et)──> [Active]
[Active] ──(Tamamla)──> [Completed]
[Completed] ──(Kapat)──> [Closed]
[Draft/Active] ──(İptal Et)──> [Cancelled]
```

## 8.3 Tablolar

### Projects — Proje Ana Kartları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ProjectNumber | nvarchar | Evet | Proje numarası (otomatik) |
| Name | nvarchar | Evet | Proje adı |
| TypeId | uniqueidentifier | Evet | Proje türü (→ ProjectTypes.Id) |
| StatusId | uniqueidentifier | Evet | Proje durumu (→ ProjectStatuses.Id) |
| CustomerId | uniqueidentifier | Hayır | Müşteri (→ BusinessPartners.Id) |
| ContractId | uniqueidentifier | Hayır | Ana sözleşme (→ Contracts.Id) |
| StartDate | date | Hayır | Planlanan başlangıç |
| EndDate | date | Hayır | Planlanan bitiş |
| ActualStartDate | date | Hayır | Gerçekleşen başlangıç |
| ActualEndDate | date | Hayır | Gerçekleşen bitiş |
| BudgetAmount | decimal(18,2) | Hayır | Onaylı bütçe tutarı |
| CurrencyId | uniqueidentifier | Hayır | Bütçe para birimi |
| Description | nvarchar | Hayır | Açıklama |
| BranchId | uniqueidentifier | Hayır | Bağlı şube |
| ... | ... | ... | (audit alanları) |

---

### ProjectTypes — Proje Türleri

Örnek: EPC (Engineering, Procurement, Construction), Bakım, Yatırım, Danışmanlık

---

### ProjectStatuses — Proje Durumları

Özelleştirilebilir durum tanımları. Varsayılanlar: Draft, Active, OnHold, Completed, Closed, Cancelled

---

### ProjectLocations — Proje Lokasyon Hiyerarşisi

**İş Amacı:** Proje alanının coğrafi veya operasyonel kırılımını tanımlar (bölge, saha, alan, koordinat).

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ProjectId | uniqueidentifier | Evet | Proje (→ Projects.Id) |
| ParentId | uniqueidentifier | Hayır | Üst lokasyon (self-ref hiyerarşi) |
| Name | nvarchar | Evet | Lokasyon adı |
| Code | nvarchar | Hayır | Kısa kod |
| Latitude | decimal | Hayır | Enlem |
| Longitude | decimal | Hayır | Boylam |
| ... | ... | ... | (audit alanları) |

---

### ProjectPhases — Proje Fazları (WBS)

**İş Amacı:** Projenin iş kırılım yapısı (WBS). Her faz alt fazlara ve iş emirlerine bağlanabilir.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ProjectId | uniqueidentifier | Evet | Proje (→ Projects.Id) |
| ParentPhaseId | uniqueidentifier | Hayır | Üst faz (self-ref) |
| Name | nvarchar | Evet | Faz adı |
| Code | nvarchar | Hayır | Faz kodu |
| PlannedStartDate | date | Hayır | Planlanan başlangıç |
| PlannedEndDate | date | Hayır | Planlanan bitiş |
| PlannedQuantity | decimal | Hayır | Planlanan metraj/miktar |
| UnitId | uniqueidentifier | Hayır | Ölçü birimi |
| UnitPrice | decimal(18,2) | Hayır | Birim fiyat |
| SortOrder | int | Evet | Sıralama |
| ... | ... | ... | (audit alanları) |

---

### ProjectMembers — Proje Ekibi

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ProjectId | uniqueidentifier | Evet | Proje (→ Projects.Id) |
| EmployeeId | uniqueidentifier | Hayır | Personel (→ Employees.Id) |
| UserId | uniqueidentifier | Hayır | Kullanıcı (→ Users.Id) |
| ProjectRole | nvarchar | Evet | Proje rolü (ProjectManager, SiteSupervisor, vb.) |
| StartDate | date | Hayır | Başlangıç tarihi |
| EndDate | date | Hayır | Bitiş tarihi |
| AllocationPercentage | decimal | Hayır | Atama oranı (%) |
| ... | ... | ... | (audit alanları) |

---

### ProjectNotes — Proje Notları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ProjectId | uniqueidentifier | Evet | Proje (→ Projects.Id) |
| NoteDate | datetime2 | Evet | Not tarihi |
| Content | nvarchar(max) | Evet | Not içeriği |
| IsPrivate | bit | Evet | Sadece ekibe mi görünür? |
| ... | ... | ... | (audit alanları) |

## 8.4 Tablo İlişkileri

```
Projects (1) ──< ProjectPhases (N)
ProjectPhases (1) ──< ProjectPhases (N)   [self-ref]
Projects (1) ──< ProjectLocations (N)
ProjectLocations (1) ──< ProjectLocations (N)   [self-ref]
Projects (1) ──< ProjectMembers (N)
Projects (1) ──< ProjectNotes (N)
Projects (N) >── BusinessPartners (1)   [CustomerId]
Projects (N) >── Contracts (1)           [ContractId]
Projects (1) ──< WorkOrders (N)          [Operations]
Projects (1) ──< StockDocuments (N)      [Inventory]
Projects (1) ──< DailySiteReports (N)    [FieldOperations]
```

---

# 9. Malzeme Kataloğu — Catalog Modülü

## 9.1 İş Amacı

Sistemdeki tüm malzemelerin tanımlı olduğu ana katalogdur. Malzeme kategorileri, markaları, dinamik öznitelikleri ve birim dönüşümleri bu modülde yönetilir. Stok, satın alma ve talep modülleri bu kataloga referans verir.

## 9.2 Katalog Hiyerarşisi

```
Brands (Markalar)
     │
MaterialCategories (Kategori Ağacı — self-ref hiyerarşi)
     │
MaterialAttributeDefinitions (Kategori bazlı öznitelik tanımları)
     │
Materials (Malzeme Kartları)
     ├── MaterialAttributeValues (Dinamik öznitelik değerleri)
     └── MaterialUnitConversions (Malzemeye özel birim çevirimi)
```

## 9.3 Tablolar

### Materials — Malzeme Kartları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Code | nvarchar | Evet | Malzeme kodu (benzersiz) |
| Name | nvarchar | Evet | Malzeme adı |
| Description | nvarchar | Hayır | Açıklama |
| CategoryId | uniqueidentifier | Evet | Kategori (→ MaterialCategories.Id) |
| BrandId | uniqueidentifier | Hayır | Marka (→ Brands.Id) |
| BaseUnitId | uniqueidentifier | Evet | Temel ölçü birimi |
| StockUnitId | uniqueidentifier | Hayır | Stok birimi (farklıysa) |
| PurchaseUnitId | uniqueidentifier | Hayır | Satın alma birimi |
| MinStockLevel | decimal | Hayır | Minimum stok seviyesi |
| MaxStockLevel | decimal | Hayır | Maksimum stok seviyesi |
| ReorderPoint | decimal | Hayır | Yeniden sipariş noktası |
| IsActive | bit | Evet | Aktif mi? |
| IsPurchasable | bit | Evet | Satın alınabilir mi? |
| IsStockable | bit | Evet | Stoklanabilir mi? |
| ... | ... | ... | (audit alanları) |

---

### MaterialAttributeDefinitions — Dinamik Öznitelik Tanımları

**İş Amacı:** Kategori bazında özelleştirilebilir öznitelikler tanımlar (renk, voltaj, kesit, akım vb.).

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Name | nvarchar | Evet | Öznitelik adı (örn: Kesit, Renk, Voltaj) |
| DataType | nvarchar | Evet | Text / Number / Boolean / Select |
| IsRequired | bit | Evet | Zorunlu mu? |
| ... | ... | ... | (audit alanları) |

---

### MaterialAttributeOptions — Seçimli Öznitelik Değerleri

Dropdown tipi öznitelikler için geçerli değer listesi.

---

### MaterialCategoryAttributes — Kategori Öznitelik Bağlantıları

Hangi kategoride hangi özniteliklerin tanımlı olduğunu belirler.

---

### MaterialAttributeValues — Malzeme Öznitelik Değerleri

Her malzemenin dinamik öznitelik değerlerini saklar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| MaterialId | uniqueidentifier | Malzeme (→ Materials.Id) |
| AttributeDefinitionId | uniqueidentifier | Öznitelik tanımı |
| Value | nvarchar | Değer |

---

### MaterialUnitConversions — Malzemeye Özel Birim Dönüşümleri

Genel UnitConversions'a ek olarak malzeme bazında özel çevrim faktörleri tanımlamayı sağlar.

## 9.4 İş Kuralları

- Malzeme kodu değiştirilemez; sadece pasif hale getirilebilir
- Stok hareketi olan malzeme silinemez
- Kategori silinmeden önce bağlı malzeme olmamalıdır
- Zorunlu öznitelikler olmadan malzeme kaydedilemez

---

# 10. Stok Yönetimi — Inventory Modülü

## 10.1 İş Amacı

Malzemelerin depolar arasındaki hareketlerini, stok miktarlarını, lot bazlı maliyet hesaplamasını ve fiziksel sayım süreçlerini yönetir. Stok giriş/çıkış belgeleri bu modülün temel çıktısıdır.

## 10.2 Kapsam

| Paydaş | Rolü |
|--------|------|
| Depo Sorumlusu | Stok belgeleri oluşturur, sayım yapar |
| Satın Alma Ekibi | Mal kabulü tetikler (Procurement bağlantısı) |
| Saha Sorumlusu | Proje çıkışı talebi oluşturur |
| Finans Ekibi | Maliyet raporları görüntüler |

## 10.3 Depo Türleri (WarehouseType)

| Değer | Açıklama |
|-------|----------|
| Central | Merkez depo |
| ProjectSite | Proje sahası deposu |
| Temporary | Geçici depo |
| Vehicle | Araç üzerindeki ekipman |
| Consignment | Konsinye (tedarikçi mülkiyetinde) |

## 10.4 Stok Belgesi Akışı

```
Giriş Kaynakları:
  Mal Kabul (PurchaseReceipts) → StockDocument [GirisType=PurchaseReceipt]
  Sayım Fazlası              → StockDocument [GirisType=CountSurplus]
  Transfer Girişi            → StockDocument [GirisType=TransferIn]

Çıkış Kaynakları:
  Proje/İş Emri Çıkışı       → StockDocument [CikisType=ProjectIssue]
  Sayım Eksiği               → StockDocument [CikisType=CountDeficit]
  Transfer Çıkışı            → StockDocument [CikisType=TransferOut]
  Fire/Zayi                  → StockDocument [CikisType=Loss]
```

## 10.5 Tablolar

### Warehouses — Depolar

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Code | nvarchar | Evet | Depo kodu (benzersiz) |
| Name | nvarchar | Evet | Depo adı |
| WarehouseType | nvarchar | Evet | Central / ProjectSite / Temporary / Vehicle / Consignment |
| ProjectId | uniqueidentifier | Hayır | Bağlı proje (saha deposu için) |
| BranchId | uniqueidentifier | Hayır | Bağlı şube |
| Address | nvarchar | Hayır | Adres |
| IsActive | bit | Evet | Aktif mi? |
| ... | ... | ... | (audit alanları) |

---

### WarehouseLocations — Depo Lokasyonları (Raf/Alan Hiyerarşisi)

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| WarehouseId | uniqueidentifier | Evet | Depo (→ Warehouses.Id) |
| ParentId | uniqueidentifier | Hayır | Üst lokasyon (self-ref) |
| Name | nvarchar | Evet | Lokasyon adı (A-01-03 vb.) |
| Code | nvarchar | Evet | Kısa kod |
| ... | ... | ... | (audit alanları) |

---

### StockDocumentTypes — Stok Belge Türleri

Sistemde kullanılacak stok belgesi türlerini tanımlar (Mal Kabul, Proje Çıkışı, Sayım, Transfer vb.).

| Alan | Tip | Açıklama |
|------|-----|----------|
| Code | nvarchar | Belge tipi kodu |
| Name | nvarchar | Ad |
| Direction | nvarchar | In / Out / Transfer |
| RequiresApproval | bit | Onay gerektirir mi? |
| AffectsStock | bit | Stok bakiyesini etkiler mi? |

---

### StockDocuments — Stok Hareket Belgeleri

**İş Amacı:** Her stok hareketinin başlık belgesidir.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| DocumentNumber | nvarchar | Evet | Belge numarası (SequenceDefinitions) |
| DocumentTypeId | uniqueidentifier | Evet | Belge türü (→ StockDocumentTypes.Id) |
| DocumentDate | date | Evet | Belge tarihi |
| WarehouseId | uniqueidentifier | Evet | İlgili depo |
| ProjectId | uniqueidentifier | Hayır | İlgili proje |
| WorkOrderId | uniqueidentifier | Hayır | İlgili iş emri |
| Status | nvarchar | Evet | Draft → PendingApproval → Approved → Posted |
| RelatedDocumentId | uniqueidentifier | Hayır | Kaynak belge (mal kabul, talep vb.) |
| ApprovalRequestId | uniqueidentifier | Hayır | Onay talebi |
| Description | nvarchar | Hayır | Açıklama |
| ... | ... | ... | (audit alanları) |

---

### StockDocumentLines — Stok Belge Satırları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| StockDocumentId | uniqueidentifier | Evet | Başlık (→ StockDocuments.Id) |
| MaterialId | uniqueidentifier | Evet | Malzeme (→ Materials.Id) |
| LocationId | uniqueidentifier | Hayır | Depo lokasyonu |
| Quantity | decimal | Evet | Miktar |
| UnitId | uniqueidentifier | Evet | Ölçü birimi |
| UnitCost | decimal(18,4) | Hayır | Birim maliyet (giriş için) |
| TotalCost | decimal(18,2) | Hayır | Toplam maliyet |
| LotId | uniqueidentifier | Hayır | Lot (→ StockLots.Id) |
| ... | ... | ... | (audit alanları) |

---

### StockLots — Lot ve Maliyet Katmanları

**İş Amacı:** Her giriş belgesi bir lot oluşturur. Çıkış maliyeti FIFO veya AVCO yöntemiyle lotlardan hesaplanır.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| MaterialId | uniqueidentifier | Evet | Malzeme |
| WarehouseId | uniqueidentifier | Evet | Depo |
| ReceiptDocumentLineId | uniqueidentifier | Evet | Kaynak giriş satırı |
| LotNumber | nvarchar | Hayır | Lot/seri no |
| ReceivedQuantity | decimal | Evet | Girişteki toplam miktar |
| RemainingQuantity | decimal | Evet | Kalan miktar |
| UnitCost | decimal(18,4) | Evet | Birim maliyet |
| ReceiptDate | date | Evet | Giriş tarihi |
| ExpiryDate | date | Hayır | Son kullanma tarihi |
| ... | ... | ... | (audit alanları) |

---

### StockIssueAllocations — Çıkış Lot Maliyet Dağılımı

**İş Amacı:** Bir çıkış satırının hangi lot(lar)dan kaçar miktar alındığını ve maliyetinin nasıl dağıldığını kaydeder (FIFO uygulaması).

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| IssueDocumentLineId | uniqueidentifier | Evet | Çıkış satırı |
| StockLotId | uniqueidentifier | Evet | Kullanılan lot |
| AllocatedQuantity | decimal | Evet | Bu lotta kullanılan miktar |
| UnitCost | decimal(18,4) | Evet | Bu lotun birim maliyeti |
| TotalCost | decimal(18,2) | Evet | Toplam maliyet |
| ... | ... | ... | (audit alanları) |

---

### StockTransactions — Değiştirilemez Stok Hareketleri

**İş Amacı:** Onaylanmış stok belgelerinden üretilen, hiç değiştirilemeyen hareket kayıtları. Muhasebe defteri gibidir.

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| DocumentLineId | uniqueidentifier | Evet | Kaynak belge satırı |
| MaterialId | uniqueidentifier | Evet | Malzeme |
| WarehouseId | uniqueidentifier | Evet | Depo |
| TransactionDate | datetime2 | Evet | İşlem zamanı |
| Direction | nvarchar | Evet | In / Out |
| Quantity | decimal | Evet | Hareket miktarı |
| UnitCost | decimal(18,4) | Evet | Birim maliyet |
| TotalCost | decimal(18,2) | Evet | Toplam maliyet |
| ... | ... | ... | (audit alanları — sadece oluşturma) |

**Kritik Kural:** StockTransactions tablosuna ekleme yapılır, güncelleme veya silme yapılmaz. Hata durumunda ters kayıt (reverse transaction) oluşturulur.

---

### StockBalances — Özet Stok Bakiyeleri

**İş Amacı:** Malzeme + depo bazında anlık stok miktarlarını ve ortalama maliyeti özetler. Performans amaçlı denormalize özet tablodur.

| Alan | Tip | Açıklama |
|------|-----|----------|
| MaterialId | uniqueidentifier | Malzeme |
| WarehouseId | uniqueidentifier | Depo |
| OnHandQuantity | decimal | Fiziksel stok miktarı |
| ReservedQuantity | decimal | Rezerve edilmiş miktar |
| AvailableQuantity | decimal | Kullanılabilir miktar (OnHand - Reserved) |
| AverageCost | decimal(18,4) | Ağırlıklı ortalama maliyet |
| LastUpdatedAt | datetime2 | Son güncelleme zamanı |

---

### StockReservations — Stok Rezervasyonları

**İş Amacı:** Henüz çıkışı yapılmamış ama tahsis edilmiş malzeme miktarlarını takip eder.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | uniqueidentifier | Birincil anahtar |
| MaterialId | uniqueidentifier | Malzeme |
| WarehouseId | uniqueidentifier | Depo |
| ReservedQuantity | decimal | Rezerve miktar |
| RelatedEntityType | nvarchar | Neyin için reserve edildi (WorkOrder, Request vb.) |
| RelatedEntityId | uniqueidentifier | İlgili kaydın ID'si |
| ExpiryDate | datetime2 | Rezervasyonun geçerlilik süresi |
| Status | nvarchar | Active / Consumed / Expired / Cancelled |

---

### StockCounts ve StockCountLines — Sayım Süreçleri

**StockCounts:** Sayım başlığı (tarih, depo, sorumlu, durum)
**StockCountLines:** Her malzeme için beklenen ve sayılan miktar

| Alan (StockCountLines) | Tip | Açıklama |
|-----------------------|-----|----------|
| MaterialId | uniqueidentifier | Malzeme |
| ExpectedQuantity | decimal | Sistem kaydındaki miktar |
| CountedQuantity | decimal | Fiziksel sayım sonucu |
| Difference | decimal | Fark (hesaplanan) |
| AdjustmentDocumentId | uniqueidentifier | Fark belgesi (StockDocument) |

---

### WarehouseTransfers ve WarehouseTransferLines — Depolar Arası Transfer

**İş Amacı:** Bir depodan diğerine malzeme transferini yönetir. Transfer, kaynak depoda çıkış + hedef depoda giriş belgeleri oluşturur.

## 10.6 Stok Belgesi Durum Geçiş Diyagramı

```
[Draft] 
    │─(Onay Gerekmiyor)──────> [Approved] ──(Kaydet)──> [Posted]
    │─(Onay Gerekiyor)──────> [PendingApproval]
                                    │─(Onayla)──> [Approved] ──(Kaydet)──> [Posted]
                                    │─(Reddet)──> [Rejected]
                                    └─(İptal)───> [Cancelled]
```

## 10.7 Stok Maliyet Hesaplama Akışı

```
Mal Kabulü gelir
    ↓
StockDocumentLines → Birim maliyet belirlenir
    ↓
StockLots kaydı oluşturulur (RemainingQuantity = ReceivedQuantity)
    ↓
Stok çıkışı yapılır
    ↓
FIFO: En eski lot önce kullanılır
    ↓
StockIssueAllocations → Her lotta kullanılan miktar ve maliyet kaydedilir
    ↓
StockLots.RemainingQuantity azaltılır
    ↓
StockTransactions'a değiştirilemez kayıt eklenir
    ↓
StockBalances güncellenir
```

## 10.8 Sayım Süreci Akışı

```
1. Depo Sorumlusu yeni sayım başlatır
   → StockCounts: Status=Draft
   → StockCountLines: Sistem bakiyeleri ExpectedQuantity'ye kopyalanır

2. Sayım ekibi fiziksel sayımı yapar
   → StockCountLines.CountedQuantity güncellenir

3. Farklar hesaplanır (CountedQuantity - ExpectedQuantity)

4. Sayım kapatılır
   → Fazla için: StockDocument [CountSurplus] oluşturulur
   → Eksik için: StockDocument [CountDeficit] oluşturulur
   → StockCounts: Status=Closed
```

## 10.9 İş Kuralları

- Stok miktarı eksi olamaz
- Onaylanmamış stok belgesi posted (kesinleşmiş) edilemez
- Kesinleşmiş belge güncellenemez; yalnızca ters kayıt oluşturulabilir
- Rezervasyon süresi dolmuş aktif rezervasyonlar sistem tarafından temizlenir
- Minimum stok seviyesinin altına düşülünce sistem otomatik uyarı üretir

## 10.10 Gerçek Hayat Senaryosu — Başarılı Proje Malzeme Çıkışı

```
Adım 1: Saha Sorumlusu kablo siparişi için stok çıkışı talebi oluşturur
         → StockDocument [Type=ProjectIssue, Status=Draft]
         → StockDocumentLines: MaterialId=Kablo, Qty=500m

Adım 2: Otomatik APR-STOCK-ISSUE akışı tetiklenir
         → ApprovalRequests kaydı oluşturulur
         → Adım 1: Depo Sorumlusu onayı

Adım 3: Depo Sorumlusu onaylar
         → ApprovalActions: ActionType=Approve
         → Proje çıkışı olduğu için Adım 2: Saha Sorumlusu onayı gerekir

Adım 4: Saha Sorumlusu onaylar
         → StockDocument: Status=Approved

Adım 5: Belge kaydedilir (posted)
         → FIFO: En eski Kablo lotu bulunur (StockLots)
         → StockIssueAllocations: Lot 1'den 300m, Lot 2'den 200m
         → StockTransactions: 500m Out kaydı oluşturulur (değiştirilemez)
         → StockBalances: Kablo bakiyesi 500m azalır
         → StockReservations: Varsa ilgili rezervasyon Consumed yapılır
```

---

# 11. Talep Yönetimi — Requests Modülü

## 11.1 İş Amacı

Projeler veya departmanlar için malzeme/hizmet talep süreçlerini yönetir. Talep, onaylandıktan sonra satın alma sürecini (Procurement) tetikler. Talep, stok ihtiyacının ilk formalize edildiği noktadır.

## 11.2 Talep Yaşam Döngüsü

```
[Draft] ──(Gönder)──> [PendingApproval]
[PendingApproval] ──(Onayla)──> [Approved]
[PendingApproval] ──(Reddet)──> [Rejected]
[Approved] ──(Satın Alma Oluştur)──> [Ordered]
[Ordered] ──(Tamamla)──> [Closed]
[Draft/PendingApproval] ──(İptal)──> [Cancelled]
```

## 11.3 Tablolar

### RequestTypes — Talep Türleri

Malzeme Talebi, Hizmet Talebi, Ekipman Talebi gibi türleri tanımlar.

### Requests — Talep Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| RequestNumber | nvarchar | Evet | Talep numarası (otomatik) |
| TypeId | uniqueidentifier | Evet | Talep türü (→ RequestTypes.Id) |
| ProjectId | uniqueidentifier | Hayır | İlgili proje |
| RequestedBy | uniqueidentifier | Evet | Talep eden kullanıcı |
| RequestDate | date | Evet | Talep tarihi |
| RequiredDate | date | Hayır | İstenen temin tarihi |
| Status | nvarchar | Evet | Draft → PendingApproval → Approved → Ordered → Closed |
| Priority | nvarchar | Hayır | Low / Normal / High / Urgent |
| Description | nvarchar | Hayır | Genel açıklama |
| ApprovalRequestId | uniqueidentifier | Hayır | Onay talebi (→ ApprovalRequests.Id) |
| ... | ... | ... | (audit alanları) |

### RequestLines — Talep Satırları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| RequestId | uniqueidentifier | Evet | Başlık (→ Requests.Id) |
| MaterialId | uniqueidentifier | Evet | Malzeme (→ Materials.Id) |
| RequestedQuantity | decimal | Evet | Talep edilen miktar |
| UnitId | uniqueidentifier | Evet | Ölçü birimi |
| EstimatedUnitCost | decimal(18,2) | Hayır | Tahmini birim maliyet |
| RequiredDate | date | Hayır | Satır bazında istenen tarih |
| Description | nvarchar | Hayır | Satır açıklaması |
| OrderedQuantity | decimal | Hayır | Siparişe alınan miktar |
| ... | ... | ... | (audit alanları) |

## 11.4 Veri Akışı

```
Proje Yöneticisi / Saha Sorumlusu
         ↓
Requests [Draft] + RequestLines oluşturulur
         ↓
Onaya gönderilir → Workflow motoru devreye girer
         ↓
Onaylı → Status=Approved
         ↓
Satın Alma Ekibi teklif süreci başlatır
         ↓
Requests: Status=Ordered + SupplierQuotes oluşturulur
         ↓
Satın Alma Siparişi oluşturulur → PurchaseOrders
```

## 11.5 İş Kuralları

- Reddedilmiş talep tekrar onaya gönderilebilir (revizyonlu)
- İptal edilmiş talep tekrar aktifleştirilemez
- Sipariş aşamasındaki talep satırı silinemez
- Stokta yeterli malzeme varsa talep yerine direkt stok çıkışı yapılabilir

---

# 12. Satın Alma — Procurement Modülü

## 12.1 İş Amacı

Malzeme ve hizmet tedarik süreçlerinin tamamını yönetir: teklif toplama, sipariş oluşturma, mal kabulü ve tedarikçi faturası. Stok girişlerinin ve borç kayıtlarının kaynağıdır.

## 12.2 Kapsam

| Paydaş | Rolü |
|--------|------|
| Satın Alma Uzmanı | Teklif ve sipariş süreçleri |
| Satın Alma Müdürü | Onay yetkisi |
| Finans Müdürü | Yüksek tutarlı onaylar |
| Depo Sorumlusu | Mal kabulü |
| Finans Ekibi | Fatura eşleştirme ve ödeme |

## 12.3 Uçtan Uca Satın Alma Süreci

```
1. Talep (Requests) onaylanır
         ↓
2. Satın Alma Uzmanı tedarikçilere teklif gönderir
   → SupplierQuotes başlığı + SupplierQuoteLines satırları
         ↓
3. Teklifler karşılaştırılır, en uygun tedarikçi seçilir
         ↓
4. Satın Alma Siparişi oluşturulur
   → PurchaseOrders [Status=Draft]
   → PurchaseOrderLines eklenir
         ↓
5. Onay akışı tetiklenir (APR-PURCHASE)
   → Tutar 0-50.000 TL: Proje Yöneticisi
   → Tutar 50.000-250.000: Satın Alma Yöneticisi + Finans Yöneticisi
   → Tutar 250.000+: Satın Alma + Finans + Genel Yönetim
         ↓
6. Sipariş tedarikçiye iletilir → PurchaseOrders [Status=Approved]
         ↓
7. Malzeme gelir → Mal Kabul yapılır
   → PurchaseReceipts [Status=Draft → Completed]
   → PurchaseReceiptLines
   → Stok girişi tetiklenir: StockDocuments + StockDocumentLines
   → StockLots oluşturulur
   → PurchaseOrders: Status=PartiallyReceived veya Received
         ↓
8. Tedarikçi faturası gelir
   → SupplierInvoices kaydedilir
   → Sipariş ile 3-way matching: Sipariş ↔ Mal Kabul ↔ Fatura
         ↓
9. Fatura onaylanır → Finance modülünde Payables kaydı oluşturulur
```

## 12.4 Tablolar

### SupplierQuotes — Tedarikçi Teklif Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| QuoteNumber | nvarchar | Evet | Teklif numarası |
| RequestId | uniqueidentifier | Hayır | İlgili talep (→ Requests.Id) |
| SupplierId | uniqueidentifier | Evet | Tedarikçi (→ BusinessPartners.Id) |
| QuoteDate | date | Evet | Teklif tarihi |
| ValidUntil | date | Hayır | Geçerlilik tarihi |
| Status | nvarchar | Evet | Draft / Sent / Received / Selected / Rejected |
| TotalAmount | decimal(18,2) | Evet | Toplam tutar |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| DeliveryDays | int | Hayır | Teslimat süresi |
| ... | ... | ... | (audit alanları) |

---

### SupplierQuoteLines — Tedarikçi Teklif Satırları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| SupplierQuoteId | uniqueidentifier | Evet | Başlık (→ SupplierQuotes.Id) |
| MaterialId | uniqueidentifier | Evet | Malzeme |
| Quantity | decimal | Evet | Teklif miktarı |
| UnitId | uniqueidentifier | Evet | Birim |
| UnitPrice | decimal(18,4) | Evet | Birim fiyat |
| VatRate | decimal | Evet | KDV oranı (%) |
| TotalPrice | decimal(18,2) | Evet | Toplam fiyat |
| DeliveryDate | date | Hayır | Bu kalem için teslimat tarihi |
| ... | ... | ... | (audit alanları) |

---

### PurchaseOrders — Satın Alma Sipariş Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| OrderNumber | nvarchar | Evet | Sipariş numarası (otomatik) |
| SupplierId | uniqueidentifier | Evet | Tedarikçi (→ BusinessPartners.Id) |
| RequestId | uniqueidentifier | Hayır | Kaynak talep |
| QuoteId | uniqueidentifier | Hayır | Kaynak teklif |
| OrderDate | date | Evet | Sipariş tarihi |
| DeliveryDate | date | Hayır | İstenen teslimat tarihi |
| Status | nvarchar | Evet | Draft → Approved → PartiallyReceived → Received → Cancelled |
| TotalAmount | decimal(18,2) | Evet | Toplam tutar |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| ProjectId | uniqueidentifier | Hayır | İlgili proje |
| WarehouseId | uniqueidentifier | Hayır | Teslim alınacak depo |
| ApprovalRequestId | uniqueidentifier | Hayır | Onay talebi |
| ... | ... | ... | (audit alanları) |

---

### PurchaseOrderLines — Satın Alma Sipariş Satırları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| PurchaseOrderId | uniqueidentifier | Evet | Başlık |
| MaterialId | uniqueidentifier | Evet | Malzeme |
| OrderedQuantity | decimal | Evet | Sipariş miktarı |
| ReceivedQuantity | decimal | Evet | Teslim alınan miktar (başta 0) |
| UnitId | uniqueidentifier | Evet | Birim |
| UnitPrice | decimal(18,4) | Evet | Birim fiyat |
| VatRate | decimal | Evet | KDV oranı |
| TotalPrice | decimal(18,2) | Evet | Toplam |
| ... | ... | ... | (audit alanları) |

---

### PurchaseReceipts — Mal Kabul Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ReceiptNumber | nvarchar | Evet | İrsaliye numarası |
| PurchaseOrderId | uniqueidentifier | Evet | Satın alma siparişi (→ PurchaseOrders.Id) |
| SupplierId | uniqueidentifier | Evet | Tedarikçi |
| ReceiptDate | date | Evet | Teslim alma tarihi |
| WarehouseId | uniqueidentifier | Evet | Teslim alınan depo |
| Status | nvarchar | Evet | Draft → Completed |
| SupplierDeliveryNote | nvarchar | Hayır | Tedarikçi irsaliye numarası |
| ... | ... | ... | (audit alanları) |

---

### PurchaseReceiptLines — Mal Kabul Satırları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| PurchaseReceiptId | uniqueidentifier | Evet | Başlık |
| PurchaseOrderLineId | uniqueidentifier | Evet | Bağlı sipariş satırı |
| MaterialId | uniqueidentifier | Evet | Malzeme |
| ReceivedQuantity | decimal | Evet | Alınan miktar |
| UnitId | uniqueidentifier | Evet | Birim |
| UnitCost | decimal(18,4) | Evet | Birim maliyet |
| StockDocumentLineId | uniqueidentifier | Hayır | Oluşturulan stok satırı |
| ... | ... | ... | (audit alanları) |

---

### SupplierInvoices — Tedarikçi Faturaları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| InvoiceNumber | nvarchar | Evet | Fatura numarası |
| SupplierId | uniqueidentifier | Evet | Tedarikçi |
| PurchaseOrderId | uniqueidentifier | Hayır | İlgili sipariş |
| InvoiceDate | date | Evet | Fatura tarihi |
| DueDate | date | Hayır | Vade tarihi |
| SubTotal | decimal(18,2) | Evet | KDV hariç tutar |
| VatAmount | decimal(18,2) | Evet | KDV tutarı |
| TotalAmount | decimal(18,2) | Evet | Genel toplam |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| Status | nvarchar | Evet | Draft → Matched → Approved → Payable |
| PayableId | uniqueidentifier | Hayır | Oluşturulan borç kaydı (→ Payables.Id) |
| ... | ... | ... | (audit alanları) |

## 12.5 Satın Alma Sipariş Durumu Geçiş Diyagramı

```
[Draft]
  │──(Onay Akışı Başlat)──> [PendingApproval]
  │                              │──(Onayla)──> [Approved]
  │                              └──(Reddet)──> [Rejected]
  └──(İptal)──> [Cancelled]
[Approved]
  │──(Kısmi Teslim)──> [PartiallyReceived]
  │──(Tam Teslim)──> [Received]
  └──(İptal)──> [Cancelled]
```

## 12.6 3-Way Matching (Fatura-Sipariş-Mal Kabul Eşleştirme)

```
SupplierInvoice
       ↓ eşleştirilir
PurchaseOrder ←──── PurchaseReceipt
       │
Miktar: Invoice.Qty ≤ Receipt.Qty ≤ Order.Qty?
Fiyat: Invoice.UnitPrice ≈ Order.UnitPrice?
       ↓ Tüm koşullar sağlanırsa
Status=Matched → Approved → Payables kaydı oluşturulur
```

## 12.7 İş Kuralları

- Onaylanmamış sipariş tedarikçiye iletilemez
- Mal kabul miktarı sipariş miktarını geçemez
- Fatura tutarı, mal kabul tutarının %5'inden fazla sapıyorsa manuel inceleme gerekir
- Sipariş iptal edilmişse mal kabul yapılamaz
- Fatura, bağlı stok girişi olmadan Approved yapılamaz (3-way matching zorunluluğu)

## 12.8 Hata Senaryosu — Fiyat Uyuşmazlığı

```
Tedarikçi faturası gelir: Birim fiyat = 120 TL
Siparişte birim fiyat = 100 TL (%20 sapma)
        ↓
Sistem uyarı üretir: "Fiyat uyuşmazlığı — Manuel inceleme gerekli"
        ↓
SupplierInvoice: Status=ManualReview
        ↓
Satın Alma Müdürü farkı inceler:
  a) Fiyat farkını kabul ederse → Onaylar, Status=Approved
  b) Kabul etmezse → Tedarikçiden düzeltilmiş fatura ister, Status=Rejected
```

---

# 13. Operasyon ve İş Emirleri — Operations Modülü

## 13.1 İş Amacı

Proje sahası veya bakım operasyonlarında yapılacak işleri tanımlar, personele atar, malzeme planlar ve gerçekleşen işleri kayıt altına alır. İş emri, saha çalışmasının temel birimidir.

## 13.2 İş Emri Türleri (WorkOrderTypes)

Kurulum, Bakım, Onarım, Test, Muayene, Söküm gibi türleri tanımlar.

## 13.3 İş Emri Durum Geçiş Diyagramı

```
[Draft]
  │──(Ata)──> [Assigned]
  │──(İptal)──> [Cancelled]
[Assigned]
  │──(Başla)──> [InProgress]
  │──(İptal)──> [Cancelled]
[InProgress]
  │──(Beklet)──> [OnHold]
  │──(Tamamla)──> [Completed]
[OnHold]
  │──(Devam Et)──> [InProgress]
  │──(İptal)──> [Cancelled]
[Completed]
  │──(Kapat)──> [Closed]
```

## 13.4 Tablolar

### WorkOrders — İş Emirleri

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| OrderNumber | nvarchar | Evet | İş emri numarası (otomatik) |
| TypeId | uniqueidentifier | Evet | İş emri türü (→ WorkOrderTypes.Id) |
| ProjectId | uniqueidentifier | Evet | İlgili proje (→ Projects.Id) |
| PhaseId | uniqueidentifier | Hayır | Proje fazı (→ ProjectPhases.Id) |
| Status | nvarchar | Evet | Draft → Assigned → InProgress → OnHold → Completed → Closed |
| Priority | nvarchar | Evet | Low / Normal / High / Critical |
| PlannedStartDate | datetime2 | Hayır | Planlanan başlangıç |
| PlannedEndDate | datetime2 | Hayır | Planlanan bitiş |
| ActualStartDate | datetime2 | Hayır | Gerçekleşen başlangıç |
| ActualEndDate | datetime2 | Hayır | Gerçekleşen bitiş |
| Description | nvarchar | Hayır | İş emri açıklaması |
| LocationId | uniqueidentifier | Hayır | İş yeri lokasyonu |
| ... | ... | ... | (audit alanları) |

---

### WorkOrderAssignments — İş Emri Görev Atamaları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| WorkOrderId | uniqueidentifier | Evet | İş emri |
| EmployeeId | uniqueidentifier | Evet | Atanan personel |
| Role | nvarchar | Hayır | Roldeki görevi (Lead, Support vb.) |
| PlannedHours | decimal | Hayır | Planlanan çalışma saati |
| ActualHours | decimal | Hayır | Gerçekleşen saat |
| ... | ... | ... | (audit alanları) |

---

### WorkOrderMaterialPlans — Planlanan Malzemeler

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| WorkOrderId | uniqueidentifier | Evet | İş emri |
| MaterialId | uniqueidentifier | Evet | Malzeme |
| PlannedQuantity | decimal | Evet | Planlanan miktar |
| UnitId | uniqueidentifier | Evet | Birim |
| ... | ... | ... | (audit alanları) |

---

### WorkOrderMaterialUsages — Gerçekleşen Malzeme Kullanımları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| WorkOrderId | uniqueidentifier | Evet | İş emri |
| MaterialId | uniqueidentifier | Evet | Malzeme |
| UsedQuantity | decimal | Evet | Kullanılan miktar |
| UnitId | uniqueidentifier | Evet | Birim |
| StockDocumentLineId | uniqueidentifier | Hayır | İlgili stok çıkış satırı |
| UsageDate | date | Evet | Kullanım tarihi |
| ... | ... | ... | (audit alanları) |

---

### WorkOrderChecklists ve WorkOrderChecklistItems — Kontrol Listesi

İş emrinde tamamlanması gereken kontrol adımları. Her adım işaretlenebilir, not eklenebilir.

---

### WorkOrderStatusHistories — Durum Geçmişi

Her durum değişikliği bu tabloya kayıt düşer. Kim, ne zaman, neden değiştirdi?

| Alan | Tip | Açıklama |
|------|-----|----------|
| WorkOrderId | uniqueidentifier | İş emri |
| FromStatus | nvarchar | Önceki durum |
| ToStatus | nvarchar | Yeni durum |
| ChangedBy | uniqueidentifier | Değiştiren kullanıcı |
| ChangedAt | datetime2 | Değişim zamanı |
| Reason | nvarchar | Açıklama |

## 13.5 Veri Akışı

```
Proje fazı tanımlanır (ProjectPhases)
        ↓
İş Emri oluşturulur (WorkOrders)
        ↓
Personel atanır (WorkOrderAssignments)
        ↓
Malzeme planlanır (WorkOrderMaterialPlans)
        ↓
Stok rezervasyonu yapılır (StockReservations)
        ↓
İş başlar → Status=InProgress
        ↓
Malzeme kullanılır (WorkOrderMaterialUsages)
        ↓
Stok çıkışı yapılır (StockDocuments)
        ↓
İş tamamlanır → Checklist onaylanır
        ↓
İlerleme kaydedilir (ProgressEntries)
        ↓
Puantaj güncellenir (TimesheetLines)
```

---

# 14. Saha Operasyonları — FieldOperations Modülü

## 14.1 İş Amacı

Proje sahasındaki günlük aktiviteleri, personel ve ekipman kullanımını, fiziksel ilerlemeyi ve metraj ölçümlerini kayıt altına alır. Hakediş hesaplamalarının temel kaynağıdır.

## 14.2 Tablolar

### DailySiteReports — Günlük Saha Raporları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ProjectId | uniqueidentifier | Evet | Proje |
| ReportDate | date | Evet | Rapor tarihi |
| WeatherCondition | nvarchar | Hayır | Hava durumu |
| Status | nvarchar | Evet | Draft → Submitted → Approved |
| Summary | nvarchar | Hayır | Gün özeti |
| ... | ... | ... | (audit alanları) |

---

### DailySiteReportWorkers — Günlük Saha Personelleri

Her günlük raporda çalışan personel listesi.

| Alan | Açıklama |
|------|----------|
| EmployeeId | Personel |
| WorkHours | Çalışılan saat |
| WorkType | Normal, Mesai, vb. |

---

### DailySiteReportEquipments — Günlük Saha Ekipmanları

Her günlük raporda kullanılan ekipman listesi.

---

### DailySiteReportMaterials — Günlük Saha Malzemeleri

Günde sahada kullanılan malzemeler (stok çıkışıyla ilişkilendirilir).

---

### ProgressEntries — Proje İlerleme Kayıtları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ProjectId | uniqueidentifier | Evet | Proje |
| PhaseId | uniqueidentifier | Hayır | Proje fazı |
| WorkOrderId | uniqueidentifier | Hayır | İş emri |
| EntryDate | date | Evet | Kayıt tarihi |
| CompletedQuantity | decimal | Evet | Tamamlanan miktar |
| UnitId | uniqueidentifier | Evet | Birim |
| CumulativeQuantity | decimal | Hayır | Kümülatif tamamlanan |
| Description | nvarchar | Hayır | Açıklama |
| ... | ... | ... | (audit alanları) |

---

### MeasurementSheets — Metraj Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| SheetNumber | nvarchar | Evet | Metraj numarası |
| ProjectId | uniqueidentifier | Evet | Proje |
| SheetDate | date | Evet | Metraj tarihi |
| Status | nvarchar | Evet | Draft → Submitted → Approved |
| ApprovedBy | uniqueidentifier | Hayır | Onaylayan |
| ... | ... | ... | (audit alanları) |

---

### MeasurementSheetLines — Metraj Satırları

| Alan | Tip | Açıklama |
|------|-----|----------|
| PhaseId | uniqueidentifier | Proje fazı |
| WorkOrderId | uniqueidentifier | İş emri |
| MeasuredQuantity | decimal | Ölçülen miktar |
| PreviousQuantity | decimal | Önceki dönem miktarı |
| CurrentQuantity | decimal | Bu dönem miktarı |
| UnitId | uniqueidentifier | Birim |

## 14.3 Metrajdan Hakedişe Veri Akışı

```
MeasurementSheetLines (ölçülen metraj)
        ↓
MeasurementSheet onaylanır
        ↓
ProgressPayments modülüne beslenir
        ↓
ProgressPaymentLines: Metraj × Birim Fiyat = Tutar
        ↓
Müşteriye/Tedarikçiye fatura veya alacak/borç oluşturulur
```

---

# 15. Varlık ve Ekipman Yönetimi — Assets Modülü

## 15.1 İş Amacı

Şirkete ait ekipman, araç, alet ve demirbaşların takibini yapar. Ekipmanların hangi projede veya kullanıcıda olduğunu, bakım geçmişini ve servis tarihlerini yönetir.

## 15.2 Tablolar

### EquipmentAssets — Ekipman Kartları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| AssetCode | nvarchar | Evet | Sabit kıymet kodu |
| Name | nvarchar | Evet | Ekipman adı |
| SerialNumber | nvarchar | Hayır | Seri numarası |
| CategoryId | uniqueidentifier | Hayır | Kategori |
| BrandId | uniqueidentifier | Hayır | Marka |
| PurchaseDate | date | Hayır | Satın alma tarihi |
| PurchaseCost | decimal(18,2) | Hayır | Satın alma maliyeti |
| Status | nvarchar | Evet | Available / InUse / UnderMaintenance / Retired |
| CurrentProjectId | uniqueidentifier | Hayır | Şu an hangi projede |
| ... | ... | ... | (audit alanları) |

---

### EquipmentAssignments — Ekipman Atamaları

| Alan | Tip | Açıklama |
|------|-----|----------|
| EquipmentAssetId | uniqueidentifier | Ekipman |
| ProjectId | uniqueidentifier | Atanan proje |
| EmployeeId | uniqueidentifier | Sorumlu personel |
| AssignedDate | date | Atama tarihi |
| ReturnDate | date | İade tarihi |
| Status | nvarchar | Active / Returned |

---

### EquipmentMaintenances — Bakım Kayıtları

| Alan | Tip | Açıklama |
|------|-----|----------|
| EquipmentAssetId | uniqueidentifier | Ekipman |
| MaintenanceType | nvarchar | Preventive / Corrective / Emergency |
| MaintenanceDate | date | Bakım tarihi |
| Description | nvarchar | Yapılan işlemler |
| Cost | decimal(18,2) | Bakım maliyeti |
| NextMaintenanceDate | date | Sonraki bakım tarihi |

---

# 16. Finans Yönetimi — Finance Modülü

## 16.1 İş Amacı

Ön muhasebe işlemlerini yönetir: borç ve alacak takibi, ödeme ve tahsilat, mutabakat. Kesinleşmiş finans hareketlerinin saklandığı yerdir.

## 16.2 Kapsam

| Paydaş | Rolü |
|--------|------|
| Finans Uzmanı | Ödeme emirleri, tahsilat kaydı |
| Finans Müdürü | Onaylar, raporlar |
| Muhasebeci | Muhasebe entegrasyonu için veri |

## 16.3 Ana Veri Akışı

```
Tedarikçi Faturası Onaylanır
        ↓
Payables kaydı oluşturulur (Borç)
        ↓
Vade geldiğinde ödeme yapılır
        ↓
Payments kaydı oluşturulur
        ↓
PaymentAllocations: Hangi borç için ödendi?
        ↓
Payable.RemainingAmount güncellenir
        ↓
Payable.Status = Paid

─────────────────────────────────────

Müşteriye Hakediş/Fatura Kesilir
        ↓
Receivables kaydı oluşturulur (Alacak)
        ↓
Müşteri ödeme yapar
        ↓
Collections kaydı oluşturulur
        ↓
CollectionAllocations: Hangi alacak için tahsil edildi?
        ↓
Receivable.RemainingAmount güncellenir
        ↓
Receivable.Status = Collected
```

## 16.4 Tablolar

### FinancialAccounts — Finans Hesapları

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | uniqueidentifier | Birincil anahtar |
| Code | nvarchar | Hesap kodu |
| Name | nvarchar | Hesap adı |
| AccountType | nvarchar | Asset / Liability / Income / Expense |
| CurrencyId | uniqueidentifier | Para birimi |
| IsActive | bit | Aktif mi? |

---

### CostCenters — Maliyet Merkezleri

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | uniqueidentifier | Birincil anahtar |
| Code | nvarchar | Maliyet merkezi kodu |
| Name | nvarchar | Ad |
| ProjectId | uniqueidentifier | Bağlı proje (opsiyonel) |
| DepartmentId | uniqueidentifier | Bağlı departman (opsiyonel) |

---

### FinancialTransactions — Ön Muhasebe Hareket Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| TransactionNumber | nvarchar | Evet | İşlem numarası |
| TransactionType | nvarchar | Evet | Expense / Income / Payable / Receivable / Payment / Collection |
| TransactionDate | date | Evet | İşlem tarihi |
| TotalAmount | decimal(18,2) | Evet | Tutar |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| ExchangeRate | decimal(18,6) | Hayır | Kur (yerel para değilse) |
| RelatedEntityType | nvarchar | Hayır | Kaynak nesne tipi |
| RelatedEntityId | uniqueidentifier | Hayır | Kaynak nesne ID'si |
| Description | nvarchar | Hayır | Açıklama |
| Status | nvarchar | Evet | Draft → Posted |
| ... | ... | ... | (audit alanları) |

---

### FinancialTransactionLines — Hareket Satırları

| Alan | Tip | Açıklama |
|------|-----|----------|
| FinancialTransactionId | uniqueidentifier | Başlık |
| AccountId | uniqueidentifier | Finans hesabı |
| CostCenterId | uniqueidentifier | Maliyet merkezi |
| Debit | decimal(18,2) | Borç tutarı |
| Credit | decimal(18,2) | Alacak tutarı |
| Description | nvarchar | Satır açıklaması |

---

### Payables — Borç Kayıtları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| BusinessPartnerId | uniqueidentifier | Evet | Alacaklı cari |
| InvoiceId | uniqueidentifier | Hayır | Kaynak fatura |
| OriginalAmount | decimal(18,2) | Evet | Başlangıç borç tutarı |
| RemainingAmount | decimal(18,2) | Evet | Kalan borç tutarı |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| DueDate | date | Hayır | Vade tarihi |
| Status | nvarchar | Evet | Open → PartiallyPaid → Paid → Overdue |
| ... | ... | ... | (audit alanları) |

---

### Receivables — Alacak Kayıtları

| Alan | Tip | Açıklama |
|------|-----|----------|
| BusinessPartnerId | uniqueidentifier | Borçlu cari |
| OriginalAmount | decimal(18,2) | Başlangıç alacak tutarı |
| RemainingAmount | decimal(18,2) | Kalan alacak tutarı |
| DueDate | date | Vade tarihi |
| Status | nvarchar | Open → PartiallyCollected → Collected → Overdue |

---

### Payments — Ödeme Başlıkları

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | uniqueidentifier | Birincil anahtar |
| BusinessPartnerId | uniqueidentifier | Ödeme yapılan cari |
| PaymentDate | date | Ödeme tarihi |
| Amount | decimal(18,2) | Ödeme tutarı |
| CurrencyId | uniqueidentifier | Para birimi |
| PaymentMethod | nvarchar | Havale / EFT / Çek / Kasa |
| BankAccountId | uniqueidentifier | Ödenen banka hesabı |
| Status | nvarchar | Draft → Approved → Completed |

---

### PaymentAllocations — Ödeme-Borç Dağılımı

Bir ödemenin hangi borç(lar)a kapatıldığını gösterir.

| Alan | Tip | Açıklama |
|------|-----|----------|
| PaymentId | uniqueidentifier | Ödeme |
| PayableId | uniqueidentifier | Kapatılan borç |
| AllocatedAmount | decimal(18,2) | Bu borç için kullanılan tutar |

---

### Collections — Tahsilat Başlıkları

Müşteriden gelen ödemeler.

---

### CollectionAllocations — Tahsilat-Alacak Dağılımı

Bir tahsilatın hangi alacak(lar)a kapatıldığını gösterir.

## 16.5 İş Kuralları

- Borç kaydı oluşturulmadan ödeme yapılamaz
- Ödeme tutarı, toplam açık borç tutarını geçemez (kural konfigüre edilebilir)
- Vadesi geçmiş borçlar için otomatik Overdue durumuna geçiş ve bildirim üretilir
- Onaylanmamış ödeme gerçekleştirilemez
- Kapatılmış (Paid/Collected) hareket güncellenemez

---

# 17. Bütçe Yönetimi — Budget Modülü

## 17.1 İş Amacı

Proje veya dönem bazlı bütçe planlamasını ve gerçekleşen maliyet karşılaştırmasını sağlar.

## 17.2 Tablolar

### Budgets — Bütçe Başlıkları

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | uniqueidentifier | Birincil anahtar |
| ProjectId | uniqueidentifier | Bağlı proje |
| PeriodStart | date | Bütçe dönemi başlangıcı |
| PeriodEnd | date | Bütçe dönemi bitişi |
| TotalAmount | decimal(18,2) | Toplam bütçe |
| CurrencyId | uniqueidentifier | Para birimi |
| Status | nvarchar | Draft → Approved → Closed |

### BudgetLines — Bütçe Satırları

| Alan | Tip | Açıklama |
|------|-----|----------|
| BudgetId | uniqueidentifier | Bütçe başlığı |
| AccountId | uniqueidentifier | Maliyet hesabı |
| CostCenterId | uniqueidentifier | Maliyet merkezi |
| PlannedAmount | decimal(18,2) | Planlanan tutar |
| ActualAmount | decimal(18,2) | Gerçekleşen tutar |
| VarianceAmount | decimal(18,2) | Sapma tutarı |

## 17.3 Sapma İzleme

```
BudgetLines.ActualAmount = FinancialTransactionLines toplamı (dönem + maliyet merkezi)
Variance = PlannedAmount - ActualAmount
%10 üzeri sapma → Otomatik uyarı + Bildirim
```

---

# 18. Sözleşme Yönetimi — Contracts Modülü

## 18.1 İş Amacı

Müşteri, tedarikçi ve taşeron sözleşmelerini, sözleşme kalemlerini ve ek protokolleri yönetir. Hakediş ve ödeme planlarının dayanağıdır.

## 18.2 Sözleşme Türleri (ContractType)

| Değer | Açıklama |
|-------|----------|
| Customer | Müşteri sözleşmesi (gelir yaratır) |
| Supplier | Tedarikçi sözleşmesi |
| Subcontractor | Taşeron sözleşmesi |
| Rental | Kiralama sözleşmesi |
| Service | Hizmet sözleşmesi |

## 18.3 Tablolar

### Contracts — Sözleşmeler

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ContractNumber | nvarchar | Evet | Sözleşme numarası |
| ContractType | nvarchar | Evet | Customer / Supplier / Subcontractor / Rental / Service |
| ProjectId | uniqueidentifier | Hayır | İlgili proje |
| StartDate | date | Evet | Başlangıç tarihi |
| EndDate | date | Hayır | Bitiş tarihi |
| TotalAmount | decimal(18,2) | Evet | Toplam sözleşme bedeli |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| Status | nvarchar | Evet | Draft → Active → Completed → Terminated |
| Description | nvarchar | Hayır | Açıklama |
| ApprovalRequestId | uniqueidentifier | Hayır | Onay talebi |
| ... | ... | ... | (audit alanları) |

---

### ContractParties — Sözleşme Tarafları

| Alan | Tip | Açıklama |
|------|-----|----------|
| ContractId | uniqueidentifier | Sözleşme |
| BusinessPartnerId | uniqueidentifier | Taraf (cari) |
| PartyRole | nvarchar | Employer / Contractor / Subcontractor vb. |

---

### ContractLines — Sözleşme Kalemleri

| Alan | Tip | Açıklama |
|------|-----|----------|
| ContractId | uniqueidentifier | Sözleşme |
| PhaseId | uniqueidentifier | Proje fazı (opsiyonel) |
| Description | nvarchar | Kalem açıklaması |
| Quantity | decimal | Miktar |
| UnitId | uniqueidentifier | Birim |
| UnitPrice | decimal(18,4) | Birim fiyat |
| TotalPrice | decimal(18,2) | Toplam |

---

### ContractAmendments — Ek Protokoller

| Alan | Tip | Açıklama |
|------|-----|----------|
| ContractId | uniqueidentifier | Ana sözleşme |
| AmendmentNumber | nvarchar | Ek protokol numarası |
| AmendmentDate | date | Tarih |
| ChangeDescription | nvarchar | Değişiklik açıklaması |
| AmountChange | decimal(18,2) | Tutar değişimi (+/-) |
| NewTotalAmount | decimal(18,2) | Yeni toplam |

---

# 19. Hakediş Yönetimi — ProgressPayments Modülü

## 19.1 İş Amacı

Proje sözleşmesine dayalı olarak müşteriye veya taşerona yapılan periyodik hakediş ödemelerini yönetir. Metraj ölçümleri hakedişin temel verisidir.

## 19.2 Hakediş Süreci Akışı

```
1. Saha ekibi metrajları ölçer
   → MeasurementSheets + MeasurementSheetLines

2. Metraj onaylanır
   → MeasurementSheet: Status=Approved

3. Hakediş oluşturulur
   → ProgressPayments [Status=Draft]
   → ProgressPaymentLines: Metraj × Sözleşme birim fiyatı

4. Kesintiler hesaplanır
   → ProgressPaymentDeductions (avans kesintisi, garanti kesintisi, vb.)

5. Hakediş onay akışı başlatılır (APR-PROGRESS)
   → Adım 1: Saha Sorumlusu
   → Adım 2: Proje Yöneticisi
   → Adım 3: Finans + Genel Müdür (ParallelAll)

6. Onaylanan hakediş faturaya dönüşür
   → Müşteri hakedişi → Receivables kaydı (Alacak)
   → Taşeron hakedişi → Payables kaydı (Borç)
```

## 19.3 Durum Geçiş Diyagramı

```
[Draft] → [Submitted] → [UnderApproval] → [Approved] → [Invoiced]
                ↓               ↓
           [Returned]      [Rejected]
```

## 19.4 Tablolar

### ProgressPayments — Hakediş Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| PaymentNumber | nvarchar | Evet | Hakediş numarası |
| ContractId | uniqueidentifier | Evet | Sözleşme (→ Contracts.Id) |
| ProjectId | uniqueidentifier | Evet | Proje |
| PeriodStart | date | Evet | Dönem başlangıcı |
| PeriodEnd | date | Evet | Dönem bitişi |
| GrossAmount | decimal(18,2) | Evet | Brüt hakediş tutarı |
| DeductionAmount | decimal(18,2) | Evet | Toplam kesinti |
| NetAmount | decimal(18,2) | Evet | Net hakediş tutarı |
| CurrencyId | uniqueidentifier | Evet | Para birimi |
| Status | nvarchar | Evet | Draft → Submitted → UnderApproval → Approved → Invoiced → Paid |
| ApprovalRequestId | uniqueidentifier | Hayır | Onay talebi |
| ... | ... | ... | (audit alanları) |

---

### ProgressPaymentLines — Hakediş Satırları

| Alan | Tip | Açıklama |
|------|-----|----------|
| ProgressPaymentId | uniqueidentifier | Başlık |
| PhaseId | uniqueidentifier | Proje fazı |
| ContractLineId | uniqueidentifier | Sözleşme kalemi |
| MeasuredQuantity | decimal | Ölçülen miktar |
| UnitId | uniqueidentifier | Birim |
| UnitPrice | decimal(18,4) | Birim fiyat (sözleşmeden) |
| CurrentPeriodAmount | decimal(18,2) | Bu dönem tutarı |
| CumulativeAmount | decimal(18,2) | Kümülatif tutar |

---

### ProgressPaymentDeductions — Hakediş Kesintileri

| Alan | Tip | Açıklama |
|------|-----|----------|
| ProgressPaymentId | uniqueidentifier | Hakediş |
| DeductionType | nvarchar | AdvanceRecovery / RetentionMoney / Penalty vb. |
| Amount | decimal(18,2) | Kesinti tutarı |
| Description | nvarchar | Kesinti açıklaması |

---

# 20. Belge Yönetimi — Documents Modülü

## 20.1 İş Amacı

Sistem içindeki tüm belgelerin (sözleşme, teknik çizim, fatura, onay belgesi vb.) versiyonlu olarak arşivlenmesini ve herhangi bir iş nesnesine bağlanabilmesini sağlar.

## 20.2 Tablolar

### DocumentFolders — Belge Klasörleri

Hiyerarşik klasör yapısı. Her proje, departman veya sözleşme için ayrı klasör oluşturulabilir.

### Documents — Belge Kayıtları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Title | nvarchar | Evet | Belge başlığı |
| DocumentType | nvarchar | Hayır | Contract / Drawing / Invoice / Report vb. |
| FolderId | uniqueidentifier | Hayır | Klasör |
| Status | nvarchar | Evet | Draft → PendingApproval → Approved → Archived |
| LatestVersionId | uniqueidentifier | Hayır | En güncel versiyon |
| ... | ... | ... | (audit alanları) |

---

### DocumentVersions — Belge Versiyonları

| Alan | Tip | Açıklama |
|------|-----|----------|
| DocumentId | uniqueidentifier | Belge |
| VersionNumber | nvarchar | Versiyon (1.0, 1.1, 2.0 vb.) |
| FileUrl | nvarchar | Dosya depolama URL'i |
| FileSize | bigint | Dosya boyutu (byte) |
| MimeType | nvarchar | Dosya türü |
| UploadedAt | datetime2 | Yükleme zamanı |

---

### DocumentRelations — Belge İş Nesnesi Bağlantıları

Herhangi bir belgeyi herhangi bir iş nesnesine bağlar (generic ilişki).

| Alan | Tip | Açıklama |
|------|-----|----------|
| DocumentId | uniqueidentifier | Belge |
| RelatedEntityType | nvarchar | Hangi modül/tablo (Project, Contract, WorkOrder vb.) |
| RelatedEntityId | uniqueidentifier | İlgili kaydın ID'si |

---

### DocumentPermissions — Belge Erişim Yetkileri

| Alan | Tip | Açıklama |
|------|-----|----------|
| DocumentId | uniqueidentifier | Belge |
| UserId/RoleId | uniqueidentifier | Yetkili kullanıcı veya rol |
| Permission | nvarchar | Read / Write / Delete |

---

# 21. Onay Akışı Motoru — Workflow Modülü

## 21.1 İş Amacı

Sistemdeki tüm onay süreçlerini yöeten dinamik, versiyonlu, çok adımlı onay altyapısıdır. Tek kişi onayından paralel çok kişi onayına, quorum modeline kadar her türlü onay senaryosunu destekler. Herhangi bir belge türü için onay akışı tanımlanabilir.

## 21.2 Kapsam

| Paydaş | Rolü |
|--------|------|
| Sistem Yöneticisi | Onay akışı tanımları ve versiyonları |
| Her Onaycı | Kendi üzerine düşen onay kararı |
| Her Talep Sahibi | Onay talebini izler |

## 21.3 Onay Modelleri

| Model | Açıklama | Kullanım |
|-------|----------|----------|
| Sequential | Sıralı — her adım bir önceki tamamlanmadan başlayamaz | Satın Alma, Hakediş |
| ParallelAny | Paralel — aday onaycılardan birinin onayı yeterli | Küçük masraf talepleri |
| ParallelAll | Paralel — tüm onaycıların onayı gerekli | Yüksek tutarlı satın alma |
| Quorum | Paralel — RequiredApprovalCount kadar onay yeterli | Komite kararları |

## 21.4 Onay Akışı Tasarımı (Tanım Tarafı)

```
ApprovalDefinitions (Akış tanımı)
    └──< ApprovalDefinitionVersions (Versiyonlanmış akış)
              └──< ApprovalStepDefinitions (Adım tanımları)
                        ├──< ApprovalStepApprovers (Kim onaylayacak?)
                        └──< ApprovalConditions (Hangi koşulda bu akış seçilir?)
```

## 21.5 Onay Talebi Yaşam Döngüsü (Çalışma Tarafı)

```
ApprovalRequests (Talep örneği)
    └──< ApprovalRequestSteps (Her adımın örneği)
              └──< ApprovalRequestApprovers (Gerçek onaycılar — talep anında kopyalanır)
                        └──< ApprovalActions (Onay, ret, iade, iptal kararları)
```

## 21.6 Tablolar

### ApprovalDefinitions — Onay Akışı Tanımları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Code | nvarchar | Evet | Akış kodu (APR-PURCHASE, APR-EXPENSE vb.) |
| Name | nvarchar | Evet | Akış adı |
| RelatedModule | nvarchar | Evet | İlgili modül (Procurement, Finance vb.) |
| RelatedEntityType | nvarchar | Evet | İlgili nesne tipi (PurchaseOrders, ExpenseClaims) |
| IsActive | bit | Evet | Aktif mi? |
| ... | ... | ... | (audit alanları) |

---

### ApprovalDefinitionVersions — Akış Versiyonları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| DefinitionId | uniqueidentifier | Evet | Tanım (→ ApprovalDefinitions.Id) |
| VersionNumber | int | Evet | Versiyon numarası |
| IsCurrentVersion | bit | Evet | Yürürlükteki versiyon mu? |
| EffectiveDate | date | Evet | Yürürlük tarihi |
| ... | ... | ... | (audit alanları) |

**Kural:** Herhangi bir anda bir tanım için yalnızca tek bir aktif (IsCurrentVersion=true) versiyon olabilir.

---

### ApprovalStepDefinitions — Onay Adımı Tanımları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| VersionId | uniqueidentifier | Evet | Versiyon (→ ApprovalDefinitionVersions.Id) |
| StepNumber | int | Evet | Adım sırası |
| Name | nvarchar | Evet | Adım adı |
| ApprovalMode | nvarchar | Evet | Sequential / ParallelAny / ParallelAll / Quorum |
| RequiredApprovalCount | int | Hayır | Quorum için gerekli onay sayısı |
| IsRequired | bit | Evet | Zorunlu adım mı? |
| TimeoutHours | int | Hayır | Zaman aşımı (saat) |
| ... | ... | ... | (audit alanları) |

---

### ApprovalStepApprovers — Adım Onaycıları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| StepDefinitionId | uniqueidentifier | Evet | Adım tanımı |
| ApproverType | nvarchar | Evet | User / Role / ProjectRole / DepartmentManager |
| ApproverId | uniqueidentifier | Hayır | Kişi ID'si (ApproverType=User ise) |
| RoleId | uniqueidentifier | Hayır | Rol ID'si (ApproverType=Role ise) |
| ProjectRole | nvarchar | Hayır | Proje rolü (ProjectManager, SiteSupervisor vb.) |
| ... | ... | ... | (audit alanları) |

---

### ApprovalConditions — Koşul Tanımları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| VersionId | uniqueidentifier | Evet | Hangi versiyona ait |
| FieldName | nvarchar | Evet | Koşul alanı (TotalAmount, ProjectId, DepartmentId vb.) |
| Operator | nvarchar | Evet | Equals / GreaterThan / LessThan / In vb. |
| Value | nvarchar | Evet | Karşılaştırma değeri |
| GroupId | int | Hayır | Koşul grubu (aynı gruptaki koşullar AND ile birleşir) |
| ... | ... | ... | (audit alanları) |

---

### ApprovalRequests — Çalışan Onay Talepleri

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| DefinitionVersionId | uniqueidentifier | Evet | Hangi akış versiyonu kullanıldı |
| RelatedEntityType | nvarchar | Evet | Hangi nesne tipi için |
| RelatedEntityId | uniqueidentifier | Evet | Hangi kaydın onayı |
| RequestedBy | uniqueidentifier | Evet | Onaya gönderen kullanıcı |
| RequestedAt | datetime2 | Evet | Gönderim zamanı |
| Status | nvarchar | Evet | Draft → Pending → Approved → Rejected → Returned → Cancelled |
| CurrentStepNumber | int | Hayır | Şu an hangi adımda |
| CompletedAt | datetime2 | Hayır | Tamamlanma zamanı |
| ... | ... | ... | (audit alanları) |

---

### ApprovalRequestSteps — Talep Adım Örnekleri

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| ApprovalRequestId | uniqueidentifier | Evet | Talep |
| StepDefinitionId | uniqueidentifier | Evet | Hangi adım tanımından |
| StepNumber | int | Evet | Sıra |
| Status | nvarchar | Evet | Waiting → Active → Approved → Rejected → Returned → Skipped |
| ActivatedAt | datetime2 | Hayır | Aktif hale geliş zamanı |
| CompletedAt | datetime2 | Hayır | Tamamlanma zamanı |
| ... | ... | ... | (audit alanları) |

---

### ApprovalRequestApprovers — Gerçek Onaycılar

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| RequestStepId | uniqueidentifier | Evet | Adım örneği |
| UserId | uniqueidentifier | Evet | Onaycı kullanıcı (talep anında kopyalanır) |
| Status | nvarchar | Evet | Waiting → Approved → Rejected → Delegated |
| ... | ... | ... | (audit alanları) |

**Kritik Kural:** Onaycılar talep anında bu tabloya kopyalanır. Sonradan rol değişse bile geçmiş bozulmaz.

---

### ApprovalActions — Onay Kararları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| RequestStepId | uniqueidentifier | Evet | Adım örneği |
| ApproverId | uniqueidentifier | Evet | Kararı veren kullanıcı |
| ActionType | nvarchar | Evet | Approve / Reject / Return / Cancel |
| ActionDate | datetime2 | Evet | Karar zamanı |
| Comment | nvarchar | Hayır | Onaycı notu |
| ... | ... | ... | (audit alanları) |

---

### ApprovalDelegations — Geçici Onay Yetkisi Devri

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| DelegatorId | uniqueidentifier | Evet | Yetkiyi devreden |
| DelegateId | uniqueidentifier | Evet | Yetkiyi alan |
| ValidFrom | datetime2 | Evet | Geçerlilik başlangıcı |
| ValidTo | datetime2 | Evet | Geçerlilik bitişi |
| Reason | nvarchar | Hayır | Neden devredildi |
| IsActive | bit | Evet | Aktif mi? |
| ... | ... | ... | (audit alanları) |

## 21.7 Satın Alma Onay Akışı Örneği (APR-PURCHASE)

```
Tutar 0-50.000 TL:
  Adım 1: ProjectManager (Sequential) ──> Onaylı
  
Tutar 50.000-250.000 TL:
  Adım 1: PurchaseManager (Sequential)
  Adım 2: FinanceManager (Sequential) ──> Onaylı
  
Tutar 250.000+ TL:
  Adım 1: PurchaseManager (Sequential)
  Adım 2: FinanceManager + ProjectManager (ParallelAll) — İKİSİ DE ONAYLAMALI
  Adım 3: Admin/GenelMüdür (Sequential) ──> Onaylı
```

## 21.8 Onay Akışı Tam Süreç Diyagramı

```
PurchaseOrder oluşturulur (Status=Draft)
       ↓
Onaya Gönderilir
       ↓
Sistem ApprovalDefinitions'da RelatedEntityType=PurchaseOrders akışını bulur
       ↓
ApprovalConditions değerlendirilir (TotalAmount koşulu)
       ↓
Uygun versiyon seçilir (IsCurrentVersion=true)
       ↓
ApprovalRequests kaydı oluşturulur
       ↓
ApprovalRequestSteps adım örnekleri oluşturulur (tümü Waiting)
       ↓
Adım 1 Active yapılır
       ↓
ApprovalRequestApprovers: Gerçek onaycılar kopyalanır
       ↓
Onaycıya bildirim gider (Notifications)
       ↓
Onaycı karar verir → ApprovalActions kaydedilir
       ↓
[Approve]: Sonraki adım Active → (döngü)
[Reject]:  ApprovalRequest.Status=Rejected, PurchaseOrder.Status=Rejected
[Return]:  ApprovalRequest.Status=Returned, talep sahibi revizyon yapar
[Cancel]:  ApprovalRequest.Status=Cancelled
       ↓
Tüm adımlar tamamlandığında → ApprovalRequest.Status=Approved
       ↓
PurchaseOrder.Status=Approved
```

## 21.9 Onay Akışı Kuralları (APR-001 ile APR-010)

| Kural | Açıklama |
|-------|----------|
| APR-001 | Bir belge türü için birden fazla versiyon olabilir; yalnızca yürürlükteki versiyon kullanılır |
| APR-002 | Sequential modda bir sonraki adım, önceki zorunlu adım tamamlanmadan aktif olamaz |
| APR-003 | ParallelAny modda aday onaycılardan birinin onayı yeterlidir |
| APR-004 | ParallelAll modda listedeki tüm onaycılar onay vermelidir |
| APR-005 | Quorum modda RequiredApprovalCount kadar olumlu işlem gereklidir |
| APR-006 | ApprovalConditions ile tutar, proje, departman bazlı farklı akışlar seçilebilir |
| APR-007 | Ret işlemi kaynak belgeyi doğrudan Approved yapamaz |
| APR-008 | Onaylanmış hareketler direkt silinmemeli; iptal veya ters kayıt oluşturulmalıdır |
| APR-009 | ApprovalDelegations tarih aralığında geçici onay devrini destekler |
| APR-010 | Gerçek onaycılar talep anında ApprovalRequestApprovers'a kopyalanmalıdır |

## 21.10 Hakediş Onay Akışı Örneği (APR-PROGRESS)

```
Tüm tutarlar için 3 adımlı süreç:
  Adım 1: SiteSupervisor (Sequential) — Saha kontrolü
  Adım 2: ProjectManager (Sequential) — Proje onayı
  Adım 3: FinanceManager + Admin (ParallelAll) — Finans ve yönetim birlikte
```

## 21.11 Delegasyon Senaryosu

```
Proje Yöneticisi izne çıkıyor:
  ApprovalDelegations oluşturulur:
    DelegatorId = PM, DelegateId = Vekil, ValidFrom=TarihX, ValidTo=TarihY
  
  Onay talebi geldiğinde:
    Sistem ApprovalDelegations tablosunu kontrol eder
    Aktif delegasyon varsa → DelegateId'ye bildirim gider
    Vekil onaycı normal onaycı gibi karar verir
    ApprovalRequestApprovers.Status=Delegated olarak işaretlenir
```

---

# 22. Bildirim Yönetimi — Notifications Modülü

## 22.1 İş Amacı

Sistem içindeki olayları (onay talebi, vade yaklaşması, stok uyarısı vb.) ilgili kişilere iletmek için bildirim mekanizmasını yönetir.

## 22.2 Tablolar

### Notifications — Bildirim Başlıkları

| Alan | Tip | Zorunlu | Açıklama |
|------|-----|---------|----------|
| Id | uniqueidentifier | Evet | Birincil anahtar |
| Title | nvarchar | Evet | Bildirim başlığı |
| Body | nvarchar | Evet | Bildirim içeriği |
| NotificationType | nvarchar | Evet | ApprovalRequest / StockAlert / PaymentDue / SystemInfo vb. |
| RelatedEntityType | nvarchar | Hayır | İlgili nesne tipi |
| RelatedEntityId | uniqueidentifier | Hayır | İlgili nesne ID'si |
| Priority | nvarchar | Evet | Low / Normal / High / Critical |
| CreatedAt | datetime2 | Evet | Oluşturma zamanı |
| ... | ... | ... | (audit alanları) |

---

### NotificationRecipients — Bildirim Alıcıları

| Alan | Tip | Açıklama |
|------|-----|----------|
| NotificationId | uniqueidentifier | Bildirim |
| UserId | uniqueidentifier | Alıcı kullanıcı |
| IsRead | bit | Okundu mu? |
| ReadAt | datetime2 | Okunma zamanı |
| Channel | nvarchar | InApp / Email / SMS |
| SentAt | datetime2 | Gönderilme zamanı |

---

### NotificationPreferences — Bildirim Tercihleri

Her kullanıcının hangi bildirim türünü hangi kanaldan almak istediğini tanımlar.

| Alan | Typ | Açıklama |
|------|-----|----------|
| UserId | uniqueidentifier | Kullanıcı |
| NotificationType | nvarchar | Bildirim türü |
| Channel | nvarchar | Kanal (InApp, Email, SMS) |
| IsEnabled | bit | Aktif mi? |

---

# 23. Sohbet — Chat Modülü

## 23.1 İş Amacı

Kullanıcılar arasında anlık mesajlaşmayı sağlar. Hem bire bir hem de grup sohbetlerini destekler. Mevcut altyapıyla uyumlu olarak tasarlanmıştır.

## 23.2 Tablolar

### ChatGroups — Sohbet Grupları

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | uniqueidentifier | Birincil anahtar |
| Name | nvarchar | Grup adı |
| OwnerId | uniqueidentifier | Grubu oluşturan kullanıcı |
| IsPrivate | bit | Özel (bire bir) mi? |
| RelatedEntityType | nvarchar | Proje, sözleşme vb. bağlantı |
| RelatedEntityId | uniqueidentifier | Bağlı nesne ID'si |

### ChatGroupMembers — Grup Üyeleri

### ChatMessages — Sohbet Mesajları

| Alan | Tip | Açıklama |
|------|-----|----------|
| GroupId | uniqueidentifier | Grup (grup mesajı için) |
| RecipientId | uniqueidentifier | Alıcı (bire bir için) |
| SenderId | uniqueidentifier | Gönderen |
| ReplyToMessageId | uniqueidentifier | Yanıtlanan mesaj |
| Content | nvarchar(max) | Mesaj içeriği |
| MessageType | nvarchar | Text / File / Image |
| IsRead | bit | Okundu mu? |

### ChatMessageReactions — Mesaj Tepkileri

---

# 24. Raporlama — Reporting Modülü

## 24.1 İş Amacı

Sistemdeki tüm verilerin anlamlı raporlar ve dashboardlar olarak sunulmasını sağlar.

## 24.2 Tablolar

### ReportDefinitions — Rapor Tanımları

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | uniqueidentifier | Birincil anahtar |
| Name | nvarchar | Rapor adı |
| Module | nvarchar | İlgili modül |
| QueryDefinition | nvarchar(max) | Rapor sorgusu/konfigürasyonu (JSON) |
| RequiredPermissionCode | nvarchar | Görüntüleme için gerekli izin |
| IsPublic | bit | Herkese açık mı? |

### DashboardWidgets — Dashboard Widget Tanımları

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | uniqueidentifier | Birincil anahtar |
| Name | nvarchar | Widget adı |
| WidgetType | nvarchar | Chart / Table / KPI / Map |
| DataSource | nvarchar(max) | Veri kaynağı konfigürasyonu |
| DefaultPosition | nvarchar | Varsayılan konum |

---

# 25. Uçtan Uca Ana Süreç Akışları

## 25.1 Proje Malzeme Tedarik Süreci (Tam Akış)

```
Proje oluşturulur
       ↓
Proje fazları ve WBS tanımlanır
       ↓
İş emirleri oluşturulur + malzeme planlanır
       ↓
Stok kontrolü yapılır:
  - Yeterli stok varsa → Stok çıkışı (StockDocuments)
  - Yetersiz/yok ise → Talep oluşturulur (Requests)
       ↓
Talep onaylanır (Workflow APR-REQUEST)
       ↓
Satın Alma ekibi devreye girer
       ↓
Tedarikçilere teklif gönderilir (SupplierQuotes)
       ↓
Teklif karşılaştırma → En uygun seçilir
       ↓
Satın Alma Siparişi oluşturulur (PurchaseOrders)
       ↓
Onay akışı (APR-PURCHASE — tutar bazlı)
       ↓
Sipariş tedarikçiye iletilir
       ↓
Mal Kabul (PurchaseReceipts)
       ↓
Stok Girişi otomatik oluşturulur (StockDocuments + StockLots)
       ↓
Tedarikçi Faturası kaydedilir (SupplierInvoices)
       ↓
3-Way Matching: Sipariş ↔ Mal Kabul ↔ Fatura
       ↓
Fatura onaylanır → Borç kaydı oluşturulur (Payables)
       ↓
Vade geldiğinde ödeme yapılır (Payments + PaymentAllocations)
```

## 25.2 Proje Saha Operasyonu ve Hakediş Süreci (Tam Akış)

```
Proje aktif, sahada çalışmalar başlıyor
       ↓
Günlük Saha Raporları doldurulur
  → Çalışan personel, kullanılan ekipman, harcanan malzeme
       ↓
İş emirleri ilerledikçe ilerleme kaydedilir (ProgressEntries)
       ↓
Dönem sonunda metraj alınır (MeasurementSheets)
  → Her proje fazı için ölçülen miktar girilir
       ↓
Metraj onaylanır (Saha Sorumlusu)
       ↓
Hakediş oluşturulur (ProgressPayments)
  → Metraj × Sözleşme birim fiyatı = Brüt hakediş
  → Kesintiler hesaplanır (ProgressPaymentDeductions)
  → Net hakediş = Brüt - Kesintiler
       ↓
Hakediş onay akışı (APR-PROGRESS):
  Saha Sorumlusu → Proje Yöneticisi → Finans + Genel Müdür
       ↓
Hakediş onaylanır
       ↓
Müşteri hakedişi ise → Receivables kaydı (Alacak)
Taşeron hakedişi ise → Payables kaydı (Borç)
       ↓
Müşteri ödeme yapınca → Collections + CollectionAllocations
```

## 25.3 ERD — Üst Düzey İlişki Diyagramı

```
Core/IAM (Temel Altyapı)
├── Companies ──< Branches
├── Departments (hiyerarşik)
├── Users ──< UserRoles >── Roles ──< RolePermissions >── Permissions
└── AuditLogs

Organization
├── Employees ──< LeaveRequests
├── Employees ──< ExpenseClaims ──< ExpenseClaimLines
└── Employees ──< EmployeeSkillAssignments >── EmployeeSkills

BusinessPartners
└── BusinessPartners ──< Contacts / Addresses / BankAccounts

Projects
├── Projects ──< ProjectPhases (WBS)
├── Projects ──< ProjectLocations
├── Projects ──< ProjectMembers
└── Projects ──< ProjectNotes

Catalog
├── MaterialCategories (hiyerarşik)
├── Materials ──< MaterialAttributeValues
└── Materials ──< MaterialUnitConversions

Inventory
├── Warehouses ──< WarehouseLocations
├── StockDocuments ──< StockDocumentLines ──> StockLots
├── StockLots ──< StockIssueAllocations
├── StockTransactions (immutable)
├── StockBalances (materialized view benzeri)
└── StockReservations

Requests → Procurement
├── Requests ──< RequestLines
├── SupplierQuotes ──< SupplierQuoteLines
├── PurchaseOrders ──< PurchaseOrderLines
├── PurchaseReceipts ──< PurchaseReceiptLines
└── SupplierInvoices ──< SupplierInvoiceLines

Operations → FieldOperations
├── WorkOrders ──< WorkOrderAssignments (Employees)
├── WorkOrders ──< WorkOrderMaterialPlans (Materials)
├── WorkOrders ──< WorkOrderMaterialUsages
├── DailySiteReports ──< Workers/Equipments/Materials
└── MeasurementSheets ──< MeasurementSheetLines

Finance
├── Payables ──< PaymentAllocations >── Payments
└── Receivables ──< CollectionAllocations >── Collections

Contracts → ProgressPayments
├── Contracts ──< ContractLines
├── Contracts ──< ContractAmendments
└── ProgressPayments ──< ProgressPaymentLines ──< ProgressPaymentDeductions

Workflow (Onay Motoru)
├── ApprovalDefinitions ──< Versions ──< StepDefinitions ──< StepApprovers/Conditions
└── ApprovalRequests ──< RequestSteps ──< RequestApprovers ──< Actions

Documents
└── Documents ──< DocumentVersions / DocumentRelations / DocumentPermissions

Notifications
└── Notifications ──< NotificationRecipients

Chat
└── ChatGroups ──< ChatGroupMembers / ChatMessages ──< ChatMessageReactions
```

---

# 26. Sistem Geneli İş Kuralları

## 26.1 Veri Bütünlüğü Kuralları

| Kural | Açıklama |
|-------|----------|
| GEN-001 | Hiçbir kayıt fiziksel olarak silinmez (soft delete) |
| GEN-002 | Her işlem AuditLogs'a yazılır |
| GEN-003 | Tüm belge numaraları SequenceDefinitions üzerinden üretilir |
| GEN-004 | Para birimi dönüşümlerinde günlük kur kullanılır |
| GEN-005 | Onaylanmış ve kapatılmış kayıtlar güncellenemez |
| GEN-006 | Stok miktarı asla negatife düşemez |
| GEN-007 | StockTransactions immutable'dır; hata durumunda ters kayıt açılır |

## 26.2 Onay Kuralları

Bkz. Bölüm 21.9 — APR-001 ile APR-010

## 26.3 Durum Geçiş Kuralları

| Nesne | Geriye Dönüş |
|-------|-------------|
| Tüm belgeler | Draft → PendingApproval gidebilir, geri dönemez (Return aksiyon hariç) |
| PurchaseOrder | Approved → Cancelled olabilir, Received → Cancelled olamaz |
| ProgressPayment | Paid/Collected durumundan geri dönülemez |
| StockTransaction | Hiçbir şekilde güncellenemez |

## 26.4 Entegrasyon Noktaları

| Sistem | Veri Akışı |
|--------|-----------|
| Dış Muhasebe | FinancialTransactions dışa aktarılır |
| TCMB/Döviz API | ExchangeRates otomatik güncelleme |
| E-Fatura | SupplierInvoices / ProgressPayments entegrasyonu |
| ERP | Malzeme kataloğu senkronizasyonu |

---

## Ekler

### A. Tüm Durum (Status) Değerleri Referansı

| Alan | Değerler |
|------|---------|
| ApprovalRequestStatus | Draft, Pending, Approved, Rejected, Returned, Cancelled |
| ApprovalStepStatus | Waiting, Active, Approved, Rejected, Returned, Skipped |
| ApprovalApproverStatus | Waiting, Approved, Rejected, Delegated |
| DocumentStatus | Draft, PendingApproval, Approved, Rejected, Cancelled, Closed |
| RequestStatus | Draft, PendingApproval, Approved, Rejected, Ordered, Closed |
| PurchaseOrderStatus | Draft, Approved, PartiallyReceived, Received, Cancelled |
| WorkOrderStatus | Draft, Assigned, InProgress, OnHold, Completed, Closed |
| FinancialTransactionType | Expense, Income, Payable, Receivable, Payment, Collection |
| WarehouseType | Central, ProjectSite, Temporary, Vehicle, Consignment |
| PartnerType | Customer, Supplier, Subcontractor, Other |
| ContractType | Customer, Supplier, Subcontractor, Rental, Service |
| ApprovalMode | Sequential, ParallelAny, ParallelAll, Quorum |
| ApproverType | User, Role, ProjectRole, DepartmentManager |

### B. Sistem Başlangıç Verileri

| Tablo | Veriler |
|-------|---------|
| Currencies | TRY, USD, EUR |
| UnitsOfMeasure | Piece, Meter, Kilogram, Ton, Liter, Hour, Day, Roll, Package |
| Roles | Admin, ProjectManager, WarehouseManager, PurchaseManager, FinanceManager, HRManager, SiteSupervisor |
| Permissions | Default.Read, Default.ReadAll, Default.Create, Default.Update, Default.Delete |

### C. Modül Tablo Sayıları

| Modül | Tablo Sayısı |
|-------|-------------|
| Core | 11 |
| IAM | 9 |
| Chat | 4 |
| Organization | 7 |
| BusinessPartners | 4 |
| Projects | 7 |
| Catalog | 8 |
| Inventory | 14 |
| Requests | 3 |
| Procurement | 8 |
| Operations | 8 |
| FieldOperations | 7 |
| HR | 2 |
| Assets | 3 |
| Finance | 10 |
| Budget | 2 |
| Contracts | 4 |
| ProgressPayments | 3 |
| Documents | 5 |
| Workflow | 10 |
| Notifications | 3 |
| Reporting | 2 |
| **TOPLAM** | **134** |

---

*Bu doküman Energy uygulamasının iş süreçleri ve veri modeli referans kılavuzudur. Güncellemeler için sürüm numarasını artırın ve değişiklik kaydına ekleyin.*
