# Energy — Üretime Hazır Sistem Tasarımı
## Eksiksiz Teknik Mimari, Veritabanı Şeması, API Spesifikasyonu ve İş Akışları

**Versiyon:** 2.0  
**Tarih:** Haziran 2026  
**Durum:** Üretime Hazır Mühendislik Referansı  
**Hedef Kitle:** Mühendislik Ekipleri, Mimarlar, DevOps, QA

---

> Bu doküman, Energy platformunun yetkili mühendislik referansıdır. Bir ekip herhangi bir bölümden bilet açarak uygulamaya başlayabilir. Hiçbir konu belirsiz ya da ertelenmiş değildir.

---

## İçindekiler

1. [Sisteme Genel Bakış](#1-sisteme-genel-bakış)
2. [Mimari Tasarım](#2-mimari-tasarım)
3. [Modül Kırılımı](#3-modül-kırılımı)
4. [Veritabanı Tasarımı — Tam Şema](#4-veritabanı-tasarımı--tam-şema)
5. [API Spesifikasyonu — Eksiksiz](#5-api-spesifikasyonu--eksiksiz)
6. [İş Akışları — Uçtan Uca](#6-iş-akışları--uçtan-uca)
7. [Olay Güdümlü Tasarım](#7-olay-güdümlü-tasarım)
8. [Güvenlik Mimarisi](#8-güvenlik-mimarisi)
9. [Gözlemlenebilirlik ve İzleme](#9-gözlemlenebilirlik-ve-i̇zleme)
10. [Altyapı ve Deployment](#10-altyapı-ve-deployment)
11. [Üretime Hazırlık Kontrol Listesi](#11-üretime-hazırlık-kontrol-listesi)

---

# 1. Sisteme Genel Bakış

## 1.1 Amaç

**Energy**, enerji ve inşaat şirketlerine yönelik kurumsal proje operasyonları platformudur. Malzeme tedarikinden saha operasyonlarına, sözleşme yönetiminden faturalama ve finansa kadar proje bazlı çalışmanın tüm yaşam döngüsünü yönetir.

### Temel Yetenekler

| Alan | Yetenek |
|------|---------|
| Projeler | WBS, fazlar, ekip atamaları, lokasyon hiyerarşisi |
| Satın Alma | Teklif → Sipariş → Mal Kabul → 3-yönlü eşleştirme → Fatura |
| Stok | Çok depolu, FIFO maliyetleme, lot takibi, rezervasyonlar |
| Saha Operasyonları | İş emirleri, günlük saha raporları, ilerleme ölçümleri |
| Finans | Borç, alacak, ödeme, tahsilat, çok para birimi |
| Sözleşme ve Hakediş | Sözleşme kalemleri, hakediş, kesintiler |
| İş Akışı Motoru | Sıralı, Paralel, Quorum onay akışları |
| İK ve Organizasyon | Personel, pozisyon, puantaj, izin, masraf |
| Varlıklar | Ekipman yaşam döngüsü, atamalar, bakım |
| Dokümanlar | Nesneye bağlanabilir, versiyonlu doküman arşivi |
| Bildirimler | Uygulama içi, e-posta, SMS — kullanıcı bazlı tercih |
| Sohbet | 1:1 ve grup mesajlaşması, iş nesnelerine bağlı |
| Raporlama | Yapılandırılabilir dashboard ve rapor tanımları |

## 1.2 Ölçek Hedefleri

| Metrik | Hedef |
|--------|-------|
| Eş zamanlı kullanıcı | 500 |
| Günlük API isteği | 2.000.000 |
| Veritabanı tablosu | 134 |
| Nesne ilişkisi | 539+ |
| Çalışma süresi SLA | %99,9 |
| API P99 gecikme | < 500ms |
| Arka plan iş kapasitesi | 10.000 iş/saat |
| Veri saklama süresi | 10 yıl (denetim logları: kalıcı) |

## 1.3 Genel Tasarım İlkeleri

### Yumuşak Silme (Soft Delete)
Her tabloda `is_deleted BOOLEAN DEFAULT false`, `deleted_at TIMESTAMPTZ`, `deleted_by UUID` alanları bulunur. Hiçbir kayıt fiziksel olarak silinmez.

### Denetim İzi (Audit Trail)
Her tabloda `created_at TIMESTAMPTZ NOT NULL DEFAULT now()`, `created_by UUID`, `updated_at TIMESTAMPTZ`, `updated_by UUID` alanları bulunur. Tüm yazma işlemleri denetim kesicisindan (interceptor) geçer.

### Değiştirilemez Defter Tabloları
`stock_transactions` ve `audit_logs` yalnızca ekleme (append-only) yapılan tablolardır. Düzeltmeler için ters kayıt oluşturulur; asla güncelleme veya silme yapılmaz.

### Belge Numaralandırma
`sequence_definitions` tablosu her belge türü için otomatik numaralandırmayı yönetir (PO-2026-00042 formatı). Sayaç artışı işlem (transaction) içinde atomik olarak yapılır.

### Modül Bağımsızlığı
Modüller arası referanslar; zorunlu bağlantılarda doğrudan FK, isteğe bağlı/çapraz bağlantılarda `(related_entity_type, related_entity_id)` polimorfik pattern ile yapılır.

### Çok Kiracılık (Multi-Tenancy)
Sistem birden fazla şirketi (`company_id`) ve şubeyi (`branch_id`) destekler. Satır düzeyinde filtreleme middleware katmanında zorunlu kılınır.

---

# 2. Mimari Tasarım

## 2.1 Üst Düzey Mimari

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         İSTEMCİ KATMANI                                 │
│   Web SPA (React)          Mobil Uygulama (React Native)   API İstemcileri│
└──────────────────────────────┬──────────────────────────────────────────┘
                               │ HTTPS / WSS
┌──────────────────────────────▼──────────────────────────────────────────┐
│                     API AĞ GEÇİDİ / TERS PROXY                         │
│         (nginx / Kong) — TLS sonlandırma, hız sınırlama, yönlendirme   │
└──────────┬─────────────────────────────────────────────┬────────────────┘
           │ REST + WebSocket                             │ Auth
┌──────────▼────────────────┐              ┌─────────────▼──────────────┐
│    Uygulama Sunucusu       │              │     Auth Servisi            │
│    (Node.js / Express 5)   │              │     (JWT + Yenileme Token)  │
│    Modüler Monolit         │              └────────────────────────────┘
│                            │
│  ┌──────────────────────┐  │   ┌─────────────────────────────────────┐
│  │  REST API İşleyiciler│  │   │         Mesaj Kuyruğu               │
│  │  WebSocket İşleyici  │──┼──►│    (Redis Streams / BullMQ)         │
│  │  İş Akışı Motoru     │  │   │                                     │
│  │  Bildirim Motoru     │  │   │  Kuyruklar:                         │
│  └──────────────────────┘  │   │  • approval-engine (onay motoru)    │
│                            │   │  • notifications (bildirimler)      │
└─────────────┬──────────────┘   │  • stock-recalc (stok hesaplama)    │
              │                  │  • sequence-generation (sıra no.)   │
    ┌─────────▼──────────┐       │  • email-dispatch (e-posta)         │
    │   PostgreSQL 16     │       │  • report-generation (rapor)        │
    │   Ana Veritabanı    │       └──────────────┬──────────────────────┘
    │   (RDS / Supabase)  │                      │
    └─────────┬──────────┘              ┌────────▼──────────────────────┐
              │ Replikasyon             │      İşçi Süreçleri            │
    ┌─────────▼──────────┐             │      (BullMQ Workers)          │
    │   PostgreSQL        │             └───────────────────────────────┘
    │   Okuma Replikası   │
    └────────────────────┘       ┌──────────────────────────────────────┐
                                 │          Redis Önbellek               │
    ┌────────────────────┐       │  • Oturum deposu                     │
    │   Nesne Depolama    │       │  • İzin önbelleği (TTL 5dk)          │
    │   (S3 / R2)         │       │  • Stok bakiye önbelleği             │
    │   Doküman dosyaları │       │  • Döviz kuru önbelleği (TTL 1sa)    │
    └────────────────────┘       └──────────────────────────────────────┘
```

## 2.2 Teknoloji Yığını

| Katman | Teknoloji | Gerekçe |
|--------|-----------|---------|
| Çalışma Ortamı | Node.js 24 LTS | Asenkron G/Ç, TypeScript-first |
| Framework | Express 5 | Kararlı, async middleware desteği |
| Dil | TypeScript 5.9 | Tam yığın tip güvenliği |
| Veritabanı | PostgreSQL 16 | ACID, JSONB, gelişmiş indeks, partisyon |
| ORM | Drizzle ORM | Tip güvenli sorgular, migrasyon-öncelikli |
| Doğrulama | Zod v4 | Tüm G/Ç üzerinde çalışma zamanı şema doğrulaması |
| Önbellek / Kuyruk | Redis 7 + BullMQ | Düşük gecikme + güvenilir iş kuyruğu |
| Nesne Depolama | S3 uyumlu (R2 veya AWS S3) | Versiyonlu doküman depolama |
| Kimlik Doğrulama | JWT (erişim 15dk) + Yenileme Token (7 gün, HttpOnly çerez) | Durumsuz API, güvenli yenileme |
| API Şeması | OpenAPI 3.1 | Sözleşme-öncelikli, istemciye kod üretimi |
| WebSockets | Socket.IO | Gerçek zamanlı bildirimler ve sohbet |
| E-posta | Resend / SendGrid | İşlem e-postaları |
| SMS | Twilio / Netgsm | SMS bildirimleri |
| Gözlemlenebilirlik | OpenTelemetry → Grafana/Loki/Tempo | İzler, metrikler, loglar |
| CI/CD | GitHub Actions | Derleme, test, lint, deploy |
| Konteyner | Docker + Docker Compose (geliştirme) | Tekrarlanabilir ortamlar |
| Orkestrasyon | Kubernetes (üretim) / Railway/Render (staging) | Ölçeklenebilir deployment |

## 2.3 Modüler Monolit Yapısı

Sistem **modüler monolit** olarak tasarlanmıştır — tek bir dağıtılabilir birim, ancak güçlü modül sınırlarıyla. Bu yapı deployment'ı basit tutarken modülleri bağımsız test edilebilir ve ilerleyen süreçte mikro servis olarak çıkarılabilir kılar.

```
src/
├── core/          # Paylaşılan altyapı (DB, önbellek, kuyruk, logger, hatalar)
├── modules/
│   ├── core/          # Şirketler, Şubeler, Para Birimleri, Birimler, Sıra, Ayarlar
│   ├── iam/           # Kullanıcılar, Roller, İzinler, Menüler
│   ├── organization/  # Personel, Departmanlar, Pozisyonlar, İzin, Masraf
│   ├── hr/            # Puantaj
│   ├── business-partners/  # Cari Kart (Müşteri/Tedarikçi/Taşeron)
│   ├── projects/      # Projeler, Fazlar, Ekip, Lokasyonlar
│   ├── catalog/       # Malzeme, Kategori, Öznitelik, Marka
│   ├── inventory/     # Depolar, Stok Belgeleri, Lot, Bakiye
│   ├── requests/      # Malzeme/Hizmet Talep
│   ├── procurement/   # Teklif, Sipariş, Mal Kabul, Fatura
│   ├── operations/    # İş Emirleri, Atamalar, Kontrol Listesi
│   ├── field-operations/  # Saha Raporları, İlerleme, Metraj
│   ├── assets/        # Ekipman Yaşam Döngüsü
│   ├── finance/       # Borç, Alacak, Ödeme, Tahsilat
│   ├── budget/        # Bütçe Planlama ve Sapma
│   ├── contracts/     # Sözleşme Yaşam Döngüsü
│   ├── progress-payments/ # Hakediş
│   ├── documents/     # Versiyonlu Doküman Arşivi
│   ├── workflow/      # Onay Motoru
│   ├── notifications/ # Bildirim Gönderimi
│   ├── chat/          # Mesajlaşma
│   └── reporting/     # Raporlar ve Dashboard
├── shared/        # Çapraz modül DTO, enum, yardımcılar
└── app.ts         # Express kurulumu + middleware
```

### Modül İç Yapısı (her modül)

```
modules/<isim>/
├── <isim>.router.ts      # Express rotaları
├── <isim>.controller.ts  # İstek/yanıt işleme
├── <isim>.service.ts     # İş mantığı
├── <isim>.repository.ts  # Veritabanı erişimi (Drizzle)
├── <isim>.schema.ts      # Drizzle tablo tanımı
├── <isim>.zod.ts         # Zod doğrulama şemaları
├── <isim>.events.ts      # Bu modülün ürettiği olaylar
└── <isim>.types.ts       # TypeScript tipleri/arayüzleri
```

---

# 3. Modül Kırılımı

| Modül | Tablo | Bağımlı Olduğu | Temel Sorumluluk |
|-------|-------|----------------|-----------------|
| Core | 11 | — | Şirketler, para birimleri, birimler, sıra tanımları, ayarlar, denetim |
| IAM | 9 | Core | Kimlik doğrulama, kullanıcı, rol, izin, menü |
| Organizasyon | 7 | IAM | Personel, pozisyon, yetkinlik, izin, masraf |
| İK | 2 | Organizasyon, Projeler | Puantaj |
| Cari Kartlar | 4 | Core | Müşteri/tedarikçi/taşeron ana verisi |
| Projeler | 7 | Core, Cari | Proje yaşam döngüsü, WBS, ekip |
| Katalog | 8 | Core | Malzeme kartı, dinamik öznitelikler |
| Stok | 14 | Core, Katalog, Projeler | Stok hareketleri, lotlar, FIFO, rezervasyon |
| Talepler | 3 | Projeler, Stok | Malzeme/hizmet talep akışı |
| Satın Alma | 8 | Talepler, Stok, Cari | Teklif → Sipariş → Mal Kabul → 3-yönlü eşleştirme |
| Operasyonlar | 8 | Projeler, Stok | İş emirleri, atamalar, malzemeler |
| Saha Operasyonları | 7 | Projeler, Operasyonlar | Saha raporları, ilerleme, metraj |
| Varlıklar | 3 | Core, Projeler | Ekipman yaşam döngüsü |
| Finans | 10 | Cari, Projeler | Borç, alacak, ödeme |
| Bütçe | 2 | Projeler, Finans | Bütçe planlama ve sapma takibi |
| Sözleşmeler | 4 | Cari, Projeler | Sözleşme yaşam döngüsü |
| Hakediş | 3 | Sözleşmeler, Projeler | Hakediş oluşturma ve faturalama |
| Dokümanlar | 5 | Tümü | Versiyonlu dosya arşivi |
| İş Akışı | 10 | IAM | Dinamik çok adımlı onay motoru |
| Bildirimler | 3 | IAM | Uygulama içi, e-posta, SMS gönderimi |
| Sohbet | 4 | IAM | 1:1 ve grup mesajlaşması |
| Raporlama | 2 | Tümü | Rapor ve dashboard tanımları |

---

# 4. Veritabanı Tasarımı — Tam Şema

## 4.1 Şema Kuralları

- **Birincil anahtarlar:** `UUID` (gen_random_uuid()), ismi `id`
- **Zaman damgaları:** `TIMESTAMPTZ NOT NULL DEFAULT now()`
- **Yumuşak silme:** `is_deleted BOOLEAN NOT NULL DEFAULT false`, `deleted_at TIMESTAMPTZ`, `deleted_by UUID`
- **Denetim alanları:** Her tabloda `created_at`, `created_by`, `updated_at`, `updated_by`
- **İsimlendirme:** Tablo/sütunlarda snake_case, kod tarafında PascalCase
- **İndeksler:** Tüm FK'lar indekslenir, `(status, is_deleted)` kombinasyonları indekslenir
- **Kısıtlamalar:** Benzersiz kısıtlamalar açıkça isimlendirilir

---

## 4.2 Core Modülü

### `companies` — Şirketler
```sql
CREATE TABLE companies (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name            VARCHAR(255) NOT NULL,
  tax_number      VARCHAR(50),                   -- Vergi numarası
  tax_office      VARCHAR(100),                  -- Vergi dairesi
  address         TEXT,
  phone           VARCHAR(30),
  email           VARCHAR(255),
  logo_url        TEXT,
  is_active       BOOLEAN NOT NULL DEFAULT true,
  created_at      TIMESTAMPTZ NOT NULL DEFAULT now(),
  created_by      UUID REFERENCES users(id),
  updated_at      TIMESTAMPTZ,
  updated_by      UUID REFERENCES users(id),
  is_deleted      BOOLEAN NOT NULL DEFAULT false,
  deleted_at      TIMESTAMPTZ,
  deleted_by      UUID REFERENCES users(id)
);
CREATE INDEX idx_companies_is_deleted ON companies(is_deleted) WHERE is_deleted = false;
```

### `branches` — Şubeler
```sql
CREATE TABLE branches (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id  UUID NOT NULL REFERENCES companies(id),
  name        VARCHAR(255) NOT NULL,
  code        VARCHAR(50) NOT NULL UNIQUE,
  address     TEXT,
  phone       VARCHAR(30),
  is_active   BOOLEAN NOT NULL DEFAULT true,
  -- (denetim + yumuşak silme alanları)
  CONSTRAINT uq_branches_code UNIQUE (code)
);
CREATE INDEX idx_branches_company_id ON branches(company_id);
```

### `departments` — Departmanlar
```sql
CREATE TABLE departments (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id            UUID NOT NULL REFERENCES companies(id),
  parent_department_id  UUID REFERENCES departments(id),  -- Üst departman (self-ref)
  name                  VARCHAR(255) NOT NULL,
  code                  VARCHAR(50) NOT NULL,
  manager_id            UUID,    -- FK → employees(id), employees tablosundan sonra eklenir
  is_active             BOOLEAN NOT NULL DEFAULT true
  -- (denetim + yumuşak silme alanları)
);
CREATE INDEX idx_departments_company ON departments(company_id);
CREATE INDEX idx_departments_parent ON departments(parent_department_id);
```

### `currencies` — Para Birimleri
```sql
CREATE TABLE currencies (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code             VARCHAR(3) NOT NULL UNIQUE,    -- ISO 4217 (TRY, USD, EUR)
  name             VARCHAR(100) NOT NULL,
  symbol           VARCHAR(5),
  is_base_currency BOOLEAN NOT NULL DEFAULT false,  -- Yalnızca bir satır true olabilir
  is_active        BOOLEAN NOT NULL DEFAULT true
  -- (denetim + yumuşak silme alanları)
);
```

### `exchange_rates` — Döviz Kurları
```sql
CREATE TABLE exchange_rates (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  from_currency_id UUID NOT NULL REFERENCES currencies(id),
  to_currency_id   UUID NOT NULL REFERENCES currencies(id),
  rate             NUMERIC(18,6) NOT NULL CHECK (rate > 0),
  rate_date        DATE NOT NULL,
  source           VARCHAR(100),   -- 'TCMB', 'Manuel', vb.
  -- (denetim + yumuşak silme alanları)
  CONSTRAINT uq_exchange_rates_pair_date UNIQUE (from_currency_id, to_currency_id, rate_date)
);
CREATE INDEX idx_exchange_rates_date ON exchange_rates(rate_date DESC);
```

### `units_of_measure` — Ölçü Birimleri
```sql
CREATE TABLE units_of_measure (
  id         UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code       VARCHAR(50) NOT NULL UNIQUE,
  name       VARCHAR(100) NOT NULL,
  unit_type  VARCHAR(50),   -- 'Uzunluk', 'Ağırlık', 'Hacim', 'Adet', vb.
  is_active  BOOLEAN NOT NULL DEFAULT true
  -- (denetim + yumuşak silme alanları)
);
```

### `unit_conversions` — Birim Dönüşümleri
```sql
CREATE TABLE unit_conversions (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  from_unit_id  UUID NOT NULL REFERENCES units_of_measure(id),
  to_unit_id    UUID NOT NULL REFERENCES units_of_measure(id),
  factor        NUMERIC(18,6) NOT NULL CHECK (factor > 0),
  CONSTRAINT uq_unit_conversions UNIQUE (from_unit_id, to_unit_id)
);
```

### `sequence_definitions` — Belge Sıra Numarası Tanımları
```sql
CREATE TABLE sequence_definitions (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  entity_type      VARCHAR(100) NOT NULL UNIQUE,  -- 'PurchaseOrder', 'Request', vb.
  prefix           VARCHAR(20),
  suffix           VARCHAR(20),
  pattern          VARCHAR(100) NOT NULL,   -- '{PREFIX}-{YIL}-{SIRA:5}'
  current_value    INTEGER NOT NULL DEFAULT 0,
  reset_period     VARCHAR(20),             -- 'Yearly', 'Monthly', 'Never'
  last_reset_date  TIMESTAMPTZ
  -- (denetim + yumuşak silme alanları)
);
```

### `system_settings` — Sistem Ayarları
```sql
CREATE TABLE system_settings (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  key         VARCHAR(200) NOT NULL UNIQUE,
  value       TEXT NOT NULL,
  value_type  VARCHAR(20) DEFAULT 'string',   -- 'string','int','bool','json'
  description TEXT,
  is_public   BOOLEAN NOT NULL DEFAULT false  -- Kullanıcı arayüzüne açılabilir mi?
);
```

### `audit_logs` — Denetim Kayıtları
```sql
CREATE TABLE audit_logs (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id         UUID,             -- Sistem işlemlerinde NULL olabilir
  entity_type     VARCHAR(100) NOT NULL,
  entity_id       UUID,
  action          VARCHAR(50) NOT NULL,  -- 'Create','Update','Delete','Approve','Reject'
  old_values      JSONB,
  new_values      JSONB,
  ip_address      VARCHAR(45),
  user_agent      TEXT,
  request_path    VARCHAR(500),
  status_code     INTEGER,
  duration_ms     INTEGER,
  created_at      TIMESTAMPTZ NOT NULL DEFAULT now()
  -- updated_at YOK, is_deleted YOK — değiştirilemez ekleme-only tablo
);
CREATE INDEX idx_audit_logs_entity ON audit_logs(entity_type, entity_id);
CREATE INDEX idx_audit_logs_user ON audit_logs(user_id);
CREATE INDEX idx_audit_logs_created ON audit_logs(created_at DESC);
-- Performans için aylık partition önerilir:
-- PARTITION BY RANGE (created_at)
```

---

## 4.3 IAM Modülü

### `users` — Kullanıcılar
```sql
CREATE TABLE users (
  id                   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  username             VARCHAR(100) NOT NULL UNIQUE,
  email                VARCHAR(255) NOT NULL UNIQUE,
  password_hash        VARCHAR(255) NOT NULL,
  first_name           VARCHAR(100) NOT NULL,
  last_name            VARCHAR(100) NOT NULL,
  phone_number         VARCHAR(30),
  avatar_url           TEXT,
  is_active            BOOLEAN NOT NULL DEFAULT true,
  is_locked            BOOLEAN NOT NULL DEFAULT false,
  last_login_at        TIMESTAMPTZ,
  failed_login_count   INTEGER NOT NULL DEFAULT 0,
  employee_id          UUID    -- FK → employees(id), personel bağlantısı (opsiyonel)
  -- (denetim + yumuşak silme alanları)
);
```

### `refresh_tokens` — Yenileme Token'ları
```sql
CREATE TABLE refresh_tokens (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  token_hash  VARCHAR(255) NOT NULL UNIQUE,   -- SHA-256 hash — asla düz metin saklanmaz
  expires_at  TIMESTAMPTZ NOT NULL,
  revoked_at  TIMESTAMPTZ,
  ip_address  VARCHAR(45),
  user_agent  TEXT,
  created_at  TIMESTAMPTZ NOT NULL DEFAULT now()
);
```

### `roles` — Roller
```sql
CREATE TABLE roles (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name            VARCHAR(100) NOT NULL UNIQUE,
  description     TEXT,
  is_system_role  BOOLEAN NOT NULL DEFAULT false,
  is_active       BOOLEAN NOT NULL DEFAULT true
);
-- Seed: Admin, ProjeYoneticisi, DepoSorumlusu, SatinAlmaYoneticisi,
--       FinansYoneticisi, IKYoneticisi, SahaSorumlusu
```

### `permissions` — İzinler
```sql
CREATE TABLE permissions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code        VARCHAR(200) NOT NULL UNIQUE,  -- 'Stok.StokBelgesi.Olustur'
  name        VARCHAR(200) NOT NULL,
  module      VARCHAR(100),
  description TEXT
);
```

### `user_roles` — Kullanıcı Rol Atamaları
```sql
CREATE TABLE user_roles (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  role_id     UUID NOT NULL REFERENCES roles(id),
  valid_from  TIMESTAMPTZ,
  valid_to    TIMESTAMPTZ,   -- NULL = süresiz
  CONSTRAINT uq_user_roles UNIQUE (user_id, role_id)
);
```

### `role_permissions` — Rol İzin Atamaları
```sql
CREATE TABLE role_permissions (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  role_id         UUID NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
  permission_code VARCHAR(200) NOT NULL REFERENCES permissions(code),
  CONSTRAINT uq_role_permissions UNIQUE (role_id, permission_code)
);
```

### `user_permissions` — Kullanıcı Bazlı İzin Geçersiz Kılma
```sql
CREATE TABLE user_permissions (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  permission_code VARCHAR(200) NOT NULL REFERENCES permissions(code),
  is_granted      BOOLEAN NOT NULL,   -- true=ver, false=reddet (rol iznini geçersiz kılar)
  reason          TEXT,
  valid_from      TIMESTAMPTZ,
  valid_to        TIMESTAMPTZ
);
```

---

## 4.4 Organizasyon Modülü

### `positions` — Pozisyonlar
```sql
CREATE TABLE positions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id  UUID NOT NULL REFERENCES companies(id),
  name        VARCHAR(200) NOT NULL,
  code        VARCHAR(50),
  level       INTEGER,
  is_active   BOOLEAN NOT NULL DEFAULT true
);
```

### `employees` — Personel
```sql
CREATE TABLE employees (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id        UUID NOT NULL REFERENCES companies(id),
  branch_id         UUID REFERENCES branches(id),
  department_id     UUID REFERENCES departments(id),
  position_id       UUID REFERENCES positions(id),
  employee_number   VARCHAR(50) NOT NULL,     -- Sicil numarası
  first_name        VARCHAR(100) NOT NULL,
  last_name         VARCHAR(100) NOT NULL,
  email             VARCHAR(255),
  phone             VARCHAR(30),
  hire_date         DATE NOT NULL,
  termination_date  DATE,
  employment_type   VARCHAR(50) NOT NULL,
  -- 'TamZamanli','YariZamanli','Sozlesmeli','Stajyer'
  is_active         BOOLEAN NOT NULL DEFAULT true,
  CONSTRAINT uq_employees_number UNIQUE (company_id, employee_number)
);
```

### `leave_requests` — İzin Talepleri
```sql
CREATE TABLE leave_requests (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  employee_id         UUID NOT NULL REFERENCES employees(id),
  leave_type          VARCHAR(50) NOT NULL,
  -- 'YillikIzin','HastalikIzni','UcretsizIzin','OlumIzni'
  start_date          DATE NOT NULL,
  end_date            DATE NOT NULL,
  total_days          NUMERIC(5,1) NOT NULL,
  reason              TEXT,
  status              VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→OnayBekliyor→Onaylandi→Reddedildi→Iptal
  approval_request_id UUID   -- FK → approval_requests
);
```

### `expense_claims` — Masraf Talepleri
```sql
CREATE TABLE expense_claims (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  employee_id         UUID NOT NULL REFERENCES employees(id),
  project_id          UUID,
  claim_date          DATE NOT NULL,
  claim_number        VARCHAR(100) NOT NULL,
  total_amount        NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  status              VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  approval_request_id UUID
);
```

### `expense_claim_lines` — Masraf Talep Satırları
```sql
CREATE TABLE expense_claim_lines (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  expense_claim_id UUID NOT NULL REFERENCES expense_claims(id),
  expense_date     DATE NOT NULL,
  category         VARCHAR(100) NOT NULL,   -- 'Seyahat','Konaklama','Yemek','Diger'
  description      TEXT,
  amount           NUMERIC(18,2) NOT NULL,
  currency_id      UUID NOT NULL REFERENCES currencies(id),
  receipt_url      TEXT
);
```

---

## 4.5 İK Modülü

### `timesheet_headers` — Puantaj Başlıkları
```sql
CREATE TABLE timesheet_headers (
  id           UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  employee_id  UUID NOT NULL REFERENCES employees(id),
  project_id   UUID REFERENCES projects(id),
  period_start DATE NOT NULL,
  period_end   DATE NOT NULL,
  status       VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Gonderildi→Onaylandi→Reddedildi
  total_hours  NUMERIC(8,2),
  CONSTRAINT uq_timesheet_period UNIQUE (employee_id, period_start, period_end)
);
```

### `timesheet_lines` — Puantaj Satırları
```sql
CREATE TABLE timesheet_lines (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  timesheet_header_id UUID NOT NULL REFERENCES timesheet_headers(id),
  work_date           DATE NOT NULL,
  work_order_id       UUID,
  phase_id            UUID,
  regular_hours       NUMERIC(5,2) NOT NULL DEFAULT 0,    -- Normal mesai
  overtime_hours      NUMERIC(5,2) NOT NULL DEFAULT 0,    -- Fazla mesai
  description         TEXT
);
```

---

## 4.6 Cari Kartlar Modülü

### `business_partners` — Cari Kartlar
```sql
CREATE TABLE business_partners (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  partner_type        VARCHAR(50) NOT NULL,
  -- 'Musteri','Tedarikci','Taseron','HepsiOlabilir'
  name                VARCHAR(255) NOT NULL,
  short_name          VARCHAR(100),
  tax_number          VARCHAR(50),
  tax_office          VARCHAR(100),
  website             VARCHAR(255),
  is_active           BOOLEAN NOT NULL DEFAULT true,
  default_currency_id UUID REFERENCES currencies(id),
  payment_terms_days  INTEGER DEFAULT 30,
  credit_limit        NUMERIC(18,2)
);
CREATE INDEX idx_bp_company ON business_partners(company_id);
CREATE INDEX idx_bp_type ON business_partners(partner_type);
```

### `business_partner_contacts` — Cari Kişiler
```sql
CREATE TABLE business_partner_contacts (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  first_name          VARCHAR(100) NOT NULL,
  last_name           VARCHAR(100) NOT NULL,
  title               VARCHAR(100),
  email               VARCHAR(255),
  phone               VARCHAR(30),
  is_primary          BOOLEAN NOT NULL DEFAULT false
);
```

### `business_partner_addresses` — Cari Adresler
```sql
CREATE TABLE business_partner_addresses (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  address_type        VARCHAR(50),   -- 'Fatura','Teslimat','Yasal'
  street              TEXT,
  city                VARCHAR(100),
  state               VARCHAR(100),
  postal_code         VARCHAR(20),
  country             VARCHAR(100),
  is_default          BOOLEAN NOT NULL DEFAULT false
);
```

### `business_partner_bank_accounts` — Cari Banka Hesapları
```sql
CREATE TABLE business_partner_bank_accounts (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  bank_name           VARCHAR(200) NOT NULL,
  iban                VARCHAR(50) NOT NULL,
  swift_code          VARCHAR(20),
  currency_id         UUID REFERENCES currencies(id),
  account_name        VARCHAR(200),
  is_default          BOOLEAN NOT NULL DEFAULT false
);
```

---

## 4.7 Projeler Modülü

### `project_types` — Proje Türleri
```sql
CREATE TABLE project_types (
  id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name      VARCHAR(100) NOT NULL,
  code      VARCHAR(50) NOT NULL UNIQUE,
  is_active BOOLEAN NOT NULL DEFAULT true
);
-- Seed: EPC, Bakim, Yatirim, Danismanlik, AnahtarTeslim
```

### `projects` — Projeler
```sql
CREATE TABLE projects (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  project_number      VARCHAR(100) NOT NULL UNIQUE,   -- Otomatik sıra numarası
  name                VARCHAR(255) NOT NULL,
  type_id             UUID NOT NULL REFERENCES project_types(id),
  status              VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Aktif→AskıdA→Tamamlandi→Kapatildi→Iptal
  customer_id         UUID REFERENCES business_partners(id),
  contract_id         UUID,    -- FK → contracts(id)
  start_date          DATE,
  end_date            DATE,
  actual_start_date   DATE,
  actual_end_date     DATE,
  budget_amount       NUMERIC(18,2),
  currency_id         UUID REFERENCES currencies(id),
  description         TEXT,
  branch_id           UUID REFERENCES branches(id)
);
CREATE INDEX idx_projects_company ON projects(company_id);
CREATE INDEX idx_projects_status ON projects(status) WHERE is_deleted = false;
```

### `project_phases` — Proje Fazları (WBS)
```sql
CREATE TABLE project_phases (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id        UUID NOT NULL REFERENCES projects(id),
  parent_phase_id   UUID REFERENCES project_phases(id),  -- Üst faz (self-ref)
  name              VARCHAR(255) NOT NULL,
  code              VARCHAR(50),
  planned_start     DATE,
  planned_end       DATE,
  planned_quantity  NUMERIC(18,3),
  unit_id           UUID REFERENCES units_of_measure(id),
  unit_price        NUMERIC(18,4),
  sort_order        INTEGER NOT NULL DEFAULT 0
);
```

### `project_locations` — Proje Lokasyonları
```sql
CREATE TABLE project_locations (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id  UUID NOT NULL REFERENCES projects(id),
  parent_id   UUID REFERENCES project_locations(id),  -- Self-ref hiyerarşi
  name        VARCHAR(255) NOT NULL,
  code        VARCHAR(50),
  latitude    NUMERIC(10,7),
  longitude   NUMERIC(10,7)
);
```

### `project_members` — Proje Ekibi
```sql
CREATE TABLE project_members (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id            UUID NOT NULL REFERENCES projects(id),
  employee_id           UUID REFERENCES employees(id),
  user_id               UUID REFERENCES users(id),
  project_role          VARCHAR(100) NOT NULL,
  -- 'ProjeYoneticisi','SahaSorumlusu','Muhendis','Usta','GuvenlikSorumlusu'
  start_date            DATE,
  end_date              DATE,
  allocation_percentage NUMERIC(5,2) CHECK (allocation_percentage BETWEEN 0 AND 100)
);
```

---

## 4.8 Katalog Modülü

### `brands` — Markalar
```sql
CREATE TABLE brands (
  id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name      VARCHAR(200) NOT NULL UNIQUE,
  country   VARCHAR(100),
  is_active BOOLEAN NOT NULL DEFAULT true
);
```

### `material_categories` — Malzeme Kategorileri
```sql
CREATE TABLE material_categories (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  parent_id   UUID REFERENCES material_categories(id),   -- Self-ref ağaç yapısı
  name        VARCHAR(200) NOT NULL,
  code        VARCHAR(50) NOT NULL UNIQUE,
  description TEXT,
  is_active   BOOLEAN NOT NULL DEFAULT true
);
```

### `material_attribute_definitions` — Dinamik Öznitelik Tanımları
```sql
CREATE TABLE material_attribute_definitions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name        VARCHAR(200) NOT NULL,    -- 'Kesit', 'Renk', 'Voltaj', 'Akım'
  data_type   VARCHAR(50) NOT NULL,     -- 'Metin','Sayi','Boolean','Secim'
  is_required BOOLEAN NOT NULL DEFAULT false,
  unit        VARCHAR(50)
);
```

### `materials` — Malzeme Kartları
```sql
CREATE TABLE materials (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code              VARCHAR(100) NOT NULL UNIQUE,   -- Değiştirilemez
  name              VARCHAR(255) NOT NULL,
  description       TEXT,
  category_id       UUID NOT NULL REFERENCES material_categories(id),
  brand_id          UUID REFERENCES brands(id),
  base_unit_id      UUID NOT NULL REFERENCES units_of_measure(id),
  stock_unit_id     UUID REFERENCES units_of_measure(id),
  purchase_unit_id  UUID REFERENCES units_of_measure(id),
  min_stock_level   NUMERIC(18,3),
  max_stock_level   NUMERIC(18,3),
  reorder_point     NUMERIC(18,3),
  is_active         BOOLEAN NOT NULL DEFAULT true,
  is_purchasable    BOOLEAN NOT NULL DEFAULT true,
  is_stockable      BOOLEAN NOT NULL DEFAULT true
);
CREATE INDEX idx_materials_category ON materials(category_id);
CREATE INDEX idx_materials_code ON materials(code);
```

### `material_attribute_values` — Malzeme Öznitelik Değerleri
```sql
CREATE TABLE material_attribute_values (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  material_id     UUID NOT NULL REFERENCES materials(id) ON DELETE CASCADE,
  definition_id   UUID NOT NULL REFERENCES material_attribute_definitions(id),
  value           TEXT NOT NULL,
  CONSTRAINT uq_mav UNIQUE (material_id, definition_id)
);
```

---

## 4.9 Stok Yönetimi Modülü

### `warehouses` — Depolar
```sql
CREATE TABLE warehouses (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id      UUID NOT NULL REFERENCES companies(id),
  code            VARCHAR(50) NOT NULL UNIQUE,
  name            VARCHAR(200) NOT NULL,
  warehouse_type  VARCHAR(50) NOT NULL,
  -- 'Merkez','ProjeSahasi','Gecici','Arac','Konsinye'
  project_id      UUID REFERENCES projects(id),
  branch_id       UUID REFERENCES branches(id),
  address         TEXT,
  is_active       BOOLEAN NOT NULL DEFAULT true
);
```

### `stock_document_types` — Stok Belge Türleri
```sql
CREATE TABLE stock_document_types (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code              VARCHAR(50) NOT NULL UNIQUE,
  name              VARCHAR(200) NOT NULL,
  direction         VARCHAR(20) NOT NULL,    -- 'Giris','Cikis','Transfer'
  requires_approval BOOLEAN NOT NULL DEFAULT false,
  affects_stock     BOOLEAN NOT NULL DEFAULT true
);
-- Seed: MalKabul/Giris, ProjeÇıkışı/Cikis, SayımFazlası/Giris,
--       SayımEksiği/Cikis, TransferGiriş/Giris, TransferÇıkış/Cikis, Fire/Cikis
```

### `stock_documents` — Stok Hareket Belgeleri
```sql
CREATE TABLE stock_documents (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id            UUID NOT NULL REFERENCES companies(id),
  document_number       VARCHAR(100) NOT NULL UNIQUE,
  document_type_id      UUID NOT NULL REFERENCES stock_document_types(id),
  document_date         DATE NOT NULL,
  warehouse_id          UUID NOT NULL REFERENCES warehouses(id),
  project_id            UUID REFERENCES projects(id),
  work_order_id         UUID,
  status                VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→OnayBekliyor→Onaylandi→Kesinlesti→Iptal→Reddedildi
  related_document_id   UUID,
  approval_request_id   UUID,
  description           TEXT
);
CREATE INDEX idx_stock_docs_warehouse ON stock_documents(warehouse_id);
CREATE INDEX idx_stock_docs_project ON stock_documents(project_id);
```

### `stock_document_lines` — Stok Belge Satırları
```sql
CREATE TABLE stock_document_lines (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  stock_document_id   UUID NOT NULL REFERENCES stock_documents(id),
  material_id         UUID NOT NULL REFERENCES materials(id),
  location_id         UUID REFERENCES warehouse_locations(id),
  quantity            NUMERIC(18,4) NOT NULL CHECK (quantity > 0),
  unit_id             UUID NOT NULL REFERENCES units_of_measure(id),
  unit_cost           NUMERIC(18,4),
  total_cost          NUMERIC(18,2),
  lot_id              UUID    -- FK → stock_lots(id)
);
```

### `stock_lots` — Stok Lotları (FIFO Maliyet Katmanları)
```sql
CREATE TABLE stock_lots (
  id                        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  material_id               UUID NOT NULL REFERENCES materials(id),
  warehouse_id              UUID NOT NULL REFERENCES warehouses(id),
  receipt_document_line_id  UUID NOT NULL REFERENCES stock_document_lines(id),
  lot_number                VARCHAR(100),
  received_quantity         NUMERIC(18,4) NOT NULL,
  remaining_quantity        NUMERIC(18,4) NOT NULL,    -- FIFO: azalır
  unit_cost                 NUMERIC(18,4) NOT NULL,
  receipt_date              DATE NOT NULL,
  expiry_date               DATE
);
CREATE INDEX idx_stock_lots_material_wh ON stock_lots(material_id, warehouse_id);
CREATE INDEX idx_stock_lots_fifo ON stock_lots(material_id, warehouse_id, receipt_date)
  WHERE remaining_quantity > 0;
```

### `stock_issue_allocations` — Çıkış Lot Maliyet Dağılımı
```sql
CREATE TABLE stock_issue_allocations (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  issue_document_line_id UUID NOT NULL REFERENCES stock_document_lines(id),
  stock_lot_id           UUID NOT NULL REFERENCES stock_lots(id),
  allocated_quantity     NUMERIC(18,4) NOT NULL,
  unit_cost              NUMERIC(18,4) NOT NULL,
  total_cost             NUMERIC(18,2) NOT NULL
);
```

### `stock_transactions` — Değiştirilemez Stok Hareketleri
```sql
CREATE TABLE stock_transactions (
  id                   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  document_line_id     UUID NOT NULL REFERENCES stock_document_lines(id),
  material_id          UUID NOT NULL REFERENCES materials(id),
  warehouse_id         UUID NOT NULL REFERENCES warehouses(id),
  transaction_date     TIMESTAMPTZ NOT NULL,
  direction            VARCHAR(5) NOT NULL CHECK (direction IN ('Giris','Cikis')),
  quantity             NUMERIC(18,4) NOT NULL,
  unit_cost            NUMERIC(18,4),
  total_cost           NUMERIC(18,2),
  created_at           TIMESTAMPTZ NOT NULL DEFAULT now(),
  created_by           UUID
  -- updated_at YOK, is_deleted YOK — muhasebe defteri gibi, asla değiştirilemez
);
```

### `stock_balances` — Özet Stok Bakiyeleri
```sql
CREATE TABLE stock_balances (
  material_id         UUID NOT NULL REFERENCES materials(id),
  warehouse_id        UUID NOT NULL REFERENCES warehouses(id),
  on_hand_quantity    NUMERIC(18,4) NOT NULL DEFAULT 0,    -- Fiziksel stok
  reserved_quantity   NUMERIC(18,4) NOT NULL DEFAULT 0,    -- Rezerve
  available_quantity  NUMERIC(18,4) GENERATED ALWAYS AS   -- Kullanılabilir
    (on_hand_quantity - reserved_quantity) STORED,
  average_cost        NUMERIC(18,4),
  last_updated_at     TIMESTAMPTZ NOT NULL DEFAULT now(),
  PRIMARY KEY (material_id, warehouse_id)
);
```

### `stock_reservations` — Stok Rezervasyonları
```sql
CREATE TABLE stock_reservations (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  material_id         UUID NOT NULL REFERENCES materials(id),
  warehouse_id        UUID NOT NULL REFERENCES warehouses(id),
  reserved_quantity   NUMERIC(18,4) NOT NULL,
  related_entity_type VARCHAR(100) NOT NULL,    -- 'IsEmri', 'Talep', vb.
  related_entity_id   UUID NOT NULL,
  expiry_date         TIMESTAMPTZ,
  status              VARCHAR(50) NOT NULL DEFAULT 'Aktif'
  -- Aktif→Kullanildi→SuresiDoldu→Iptal
);
CREATE INDEX idx_stock_res_active ON stock_reservations(material_id, warehouse_id)
  WHERE status = 'Aktif';
```

### `stock_counts` ve `stock_count_lines` — Sayım Süreçleri
```sql
CREATE TABLE stock_counts (
  id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  warehouse_id   UUID NOT NULL REFERENCES warehouses(id),
  count_date     DATE NOT NULL,
  status         VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→DevamEdiyor→Tamamlandi→Kapatildi
  responsible_id UUID REFERENCES employees(id),
  notes          TEXT
);

CREATE TABLE stock_count_lines (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  stock_count_id         UUID NOT NULL REFERENCES stock_counts(id),
  material_id            UUID NOT NULL REFERENCES materials(id),
  location_id            UUID REFERENCES warehouse_locations(id),
  expected_quantity      NUMERIC(18,4) NOT NULL,    -- Sistemdeki bakiye
  counted_quantity       NUMERIC(18,4),             -- Fiziksel sayım
  difference             NUMERIC(18,4) GENERATED ALWAYS AS
    (counted_quantity - expected_quantity) STORED,
  adjustment_document_id UUID REFERENCES stock_documents(id)
);
```

---

## 4.10 Talepler Modülü

### `requests` — Talep Başlıkları
```sql
CREATE TABLE requests (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  request_number      VARCHAR(100) NOT NULL UNIQUE,
  type_id             UUID NOT NULL REFERENCES request_types(id),
  project_id          UUID REFERENCES projects(id),
  requested_by        UUID NOT NULL REFERENCES users(id),
  request_date        DATE NOT NULL,
  required_date       DATE,
  status              VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→OnayBekliyor→Onaylandi→Siparise Alindi→Kapatildi→Reddedildi→Iptal
  priority            VARCHAR(20) DEFAULT 'Normal',
  -- Dusuk, Normal, Yuksek, Acil
  description         TEXT,
  approval_request_id UUID
);
```

### `request_lines` — Talep Satırları
```sql
CREATE TABLE request_lines (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  request_id          UUID NOT NULL REFERENCES requests(id),
  material_id         UUID NOT NULL REFERENCES materials(id),
  requested_quantity  NUMERIC(18,4) NOT NULL,
  unit_id             UUID NOT NULL REFERENCES units_of_measure(id),
  estimated_unit_cost NUMERIC(18,2),
  required_date       DATE,
  description         TEXT,
  ordered_quantity    NUMERIC(18,4) DEFAULT 0
);
```

---

## 4.11 Satın Alma Modülü

### `supplier_quotes` — Tedarikçi Teklifleri
```sql
CREATE TABLE supplier_quotes (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  quote_number  VARCHAR(100) NOT NULL UNIQUE,
  request_id    UUID REFERENCES requests(id),
  supplier_id   UUID NOT NULL REFERENCES business_partners(id),
  quote_date    DATE NOT NULL,
  valid_until   DATE,
  currency_id   UUID NOT NULL REFERENCES currencies(id),
  total_amount  NUMERIC(18,2),
  status        VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Gonderildi→Alindi→Degerlendirildi→Kabul→Reddedildi
  notes         TEXT
);
```

### `purchase_orders` — Satın Alma Siparişleri
```sql
CREATE TABLE purchase_orders (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  order_number        VARCHAR(100) NOT NULL UNIQUE,
  supplier_id         UUID NOT NULL REFERENCES business_partners(id),
  request_id          UUID REFERENCES requests(id),
  quote_id            UUID REFERENCES supplier_quotes(id),
  project_id          UUID REFERENCES projects(id),
  order_date          DATE NOT NULL,
  expected_delivery   DATE,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  subtotal            NUMERIC(18,2) NOT NULL DEFAULT 0,
  vat_amount          NUMERIC(18,2) NOT NULL DEFAULT 0,
  total_amount        NUMERIC(18,2) NOT NULL DEFAULT 0,
  status              VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→OnayBekliyor→Onaylandi→Reddedildi→KısmiTeslim→Tamamlandi→Iptal
  approval_request_id UUID,
  notes               TEXT
);
CREATE INDEX idx_po_supplier ON purchase_orders(supplier_id);
CREATE INDEX idx_po_project ON purchase_orders(project_id);
CREATE INDEX idx_po_status ON purchase_orders(status);
```

### `purchase_order_lines` — Sipariş Satırları
```sql
CREATE TABLE purchase_order_lines (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  purchase_order_id UUID NOT NULL REFERENCES purchase_orders(id),
  material_id       UUID NOT NULL REFERENCES materials(id),
  quantity          NUMERIC(18,4) NOT NULL,
  unit_id           UUID NOT NULL REFERENCES units_of_measure(id),
  unit_price        NUMERIC(18,4) NOT NULL,
  vat_rate          NUMERIC(5,2) DEFAULT 18,
  total_price       NUMERIC(18,2),
  received_quantity NUMERIC(18,4) DEFAULT 0,
  request_line_id   UUID REFERENCES request_lines(id)
);
```

### `purchase_receipts` — Mal Kabul Başlıkları
```sql
CREATE TABLE purchase_receipts (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  receipt_number         VARCHAR(100) NOT NULL UNIQUE,
  purchase_order_id      UUID NOT NULL REFERENCES purchase_orders(id),
  supplier_id            UUID NOT NULL REFERENCES business_partners(id),
  receipt_date           DATE NOT NULL,
  warehouse_id           UUID NOT NULL REFERENCES warehouses(id),
  status                 VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Tamamlandi
  supplier_delivery_note VARCHAR(200),   -- Tedarikçi irsaliye numarası
  notes                  TEXT
);
```

### `supplier_invoices` — Tedarikçi Faturaları
```sql
CREATE TABLE supplier_invoices (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  invoice_number   VARCHAR(200) NOT NULL,
  supplier_id      UUID NOT NULL REFERENCES business_partners(id),
  purchase_order_id UUID REFERENCES purchase_orders(id),
  invoice_date     DATE NOT NULL,
  due_date         DATE,
  subtotal         NUMERIC(18,2) NOT NULL,
  vat_amount       NUMERIC(18,2) NOT NULL DEFAULT 0,
  total_amount     NUMERIC(18,2) NOT NULL,
  currency_id      UUID NOT NULL REFERENCES currencies(id),
  status           VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Eslesti→ManuelInceleme→Onaylandi→Odenecek→Odendi
  payable_id       UUID,    -- Oluşturulan borç kaydı
  notes            TEXT
);
```

---

## 4.12 Operasyonlar Modülü

### `work_orders` — İş Emirleri
```sql
CREATE TABLE work_orders (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id        UUID NOT NULL REFERENCES companies(id),
  order_number      VARCHAR(100) NOT NULL UNIQUE,
  type_id           UUID NOT NULL REFERENCES work_order_types(id),
  project_id        UUID NOT NULL REFERENCES projects(id),
  phase_id          UUID REFERENCES project_phases(id),
  status            VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Atandi→DevamEdiyor→Askida→Tamamlandi→Kapatildi→Iptal
  priority          VARCHAR(20) NOT NULL DEFAULT 'Normal',
  -- Dusuk, Normal, Yuksek, Kritik
  planned_start     TIMESTAMPTZ,
  planned_end       TIMESTAMPTZ,
  actual_start      TIMESTAMPTZ,
  actual_end        TIMESTAMPTZ,
  description       TEXT,
  location_id       UUID REFERENCES project_locations(id),
  parent_wo_id      UUID REFERENCES work_orders(id)   -- Alt iş emri desteği
);
CREATE INDEX idx_wo_project ON work_orders(project_id);
CREATE INDEX idx_wo_status ON work_orders(status);
```

### `work_order_assignments` — İş Emri Görev Atamaları
```sql
CREATE TABLE work_order_assignments (
  id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  work_order_id  UUID NOT NULL REFERENCES work_orders(id),
  employee_id    UUID NOT NULL REFERENCES employees(id),
  role           VARCHAR(100),    -- 'Lider', 'Destek', vb.
  planned_hours  NUMERIC(8,2),
  actual_hours   NUMERIC(8,2)
);
```

### `work_order_material_plans` — Planlanan Malzemeler
```sql
CREATE TABLE work_order_material_plans (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  work_order_id   UUID NOT NULL REFERENCES work_orders(id),
  material_id     UUID NOT NULL REFERENCES materials(id),
  planned_qty     NUMERIC(18,4) NOT NULL,
  unit_id         UUID NOT NULL REFERENCES units_of_measure(id)
);
```

### `work_order_material_usages` — Gerçekleşen Malzeme Kullanımları
```sql
CREATE TABLE work_order_material_usages (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  work_order_id          UUID NOT NULL REFERENCES work_orders(id),
  material_id            UUID NOT NULL REFERENCES materials(id),
  used_quantity          NUMERIC(18,4) NOT NULL,
  unit_id                UUID NOT NULL REFERENCES units_of_measure(id),
  stock_document_line_id UUID REFERENCES stock_document_lines(id),
  usage_date             DATE NOT NULL
);
```

### `work_order_status_histories` — İş Emri Durum Geçmişi
```sql
CREATE TABLE work_order_status_histories (
  id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  work_order_id  UUID NOT NULL REFERENCES work_orders(id),
  from_status    VARCHAR(50),
  to_status      VARCHAR(50) NOT NULL,
  changed_by     UUID NOT NULL REFERENCES users(id),
  changed_at     TIMESTAMPTZ NOT NULL DEFAULT now(),
  reason         TEXT
);
```

---

## 4.13 Saha Operasyonları Modülü

### `daily_site_reports` — Günlük Saha Raporları
```sql
CREATE TABLE daily_site_reports (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id        UUID NOT NULL REFERENCES projects(id),
  report_date       DATE NOT NULL,
  weather_condition VARCHAR(100),
  status            VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Gonderildi→Onaylandi
  summary           TEXT,
  approval_request_id UUID,
  CONSTRAINT uq_dsr UNIQUE (project_id, report_date)  -- Günde bir rapor
);
```

### `progress_entries` — Proje İlerleme Kayıtları
```sql
CREATE TABLE progress_entries (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id          UUID NOT NULL REFERENCES projects(id),
  phase_id            UUID REFERENCES project_phases(id),
  work_order_id       UUID REFERENCES work_orders(id),
  entry_date          DATE NOT NULL,
  completed_quantity  NUMERIC(18,4) NOT NULL,
  unit_id             UUID NOT NULL REFERENCES units_of_measure(id),
  cumulative_quantity NUMERIC(18,4),    -- Birikimli tamamlanan
  description         TEXT
);
```

### `measurement_sheets` — Metraj Başlıkları
```sql
CREATE TABLE measurement_sheets (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  sheet_number  VARCHAR(100) NOT NULL UNIQUE,
  project_id    UUID NOT NULL REFERENCES projects(id),
  sheet_date    DATE NOT NULL,
  status        VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Gonderildi→Onaylandi
  approved_by   UUID REFERENCES users(id),
  approved_at   TIMESTAMPTZ,
  notes         TEXT
);
```

### `measurement_sheet_lines` — Metraj Satırları
```sql
CREATE TABLE measurement_sheet_lines (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  measurement_sheet_id  UUID NOT NULL REFERENCES measurement_sheets(id),
  phase_id              UUID NOT NULL REFERENCES project_phases(id),
  work_order_id         UUID REFERENCES work_orders(id),
  measured_quantity     NUMERIC(18,4) NOT NULL,
  previous_quantity     NUMERIC(18,4) NOT NULL DEFAULT 0,  -- Önceki dönem
  current_quantity      NUMERIC(18,4) NOT NULL,            -- Bu dönem
  unit_id               UUID NOT NULL REFERENCES units_of_measure(id),
  description           TEXT
);
```

---

## 4.14 Varlık ve Ekipman Modülü

### `equipment_assets` — Ekipman Kartları
```sql
CREATE TABLE equipment_assets (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  asset_code          VARCHAR(100) NOT NULL UNIQUE,
  name                VARCHAR(255) NOT NULL,
  serial_number       VARCHAR(100),
  category_id         UUID REFERENCES material_categories(id),
  brand_id            UUID REFERENCES brands(id),
  purchase_date       DATE,
  purchase_cost       NUMERIC(18,2),
  status              VARCHAR(50) NOT NULL DEFAULT 'Musait',
  -- Musait→KullanımDa→BakımdA→HizmetDisi
  current_project_id  UUID REFERENCES projects(id)
);
```

### `equipment_maintenances` — Bakım Kayıtları
```sql
CREATE TABLE equipment_maintenances (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  equipment_id          UUID NOT NULL REFERENCES equipment_assets(id),
  maintenance_type      VARCHAR(50) NOT NULL,
  -- 'Koruyucu','Düzeltici','Acil'
  maintenance_date      DATE NOT NULL,
  description           TEXT,
  cost                  NUMERIC(18,2),
  technician_id         UUID REFERENCES employees(id),
  next_maintenance_date DATE
);
```

---

## 4.15 Finans Modülü

### `financial_accounts` — Finans Hesapları
```sql
CREATE TABLE financial_accounts (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id    UUID NOT NULL REFERENCES companies(id),
  code          VARCHAR(50) NOT NULL,
  name          VARCHAR(200) NOT NULL,
  account_type  VARCHAR(50) NOT NULL,
  -- Varlik, Yükümlülük, Gelir, Gider, Özkaynak
  currency_id   UUID REFERENCES currencies(id),
  is_active     BOOLEAN NOT NULL DEFAULT true,
  CONSTRAINT uq_financial_accounts UNIQUE (company_id, code)
);
```

### `cost_centers` — Maliyet Merkezleri
```sql
CREATE TABLE cost_centers (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id    UUID NOT NULL REFERENCES companies(id),
  code          VARCHAR(50) NOT NULL,
  name          VARCHAR(200) NOT NULL,
  project_id    UUID REFERENCES projects(id),
  department_id UUID REFERENCES departments(id),
  is_active     BOOLEAN NOT NULL DEFAULT true,
  CONSTRAINT uq_cost_centers UNIQUE (company_id, code)
);
```

### `payables` — Borç Kayıtları
```sql
CREATE TABLE payables (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  invoice_id          UUID REFERENCES supplier_invoices(id),
  original_amount     NUMERIC(18,2) NOT NULL,
  remaining_amount    NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  due_date            DATE,
  status              VARCHAR(50) NOT NULL DEFAULT 'Acik',
  -- Acik→KısmiOdendi→Odendi→Vadesi Gecti→Iptal
  description         TEXT
);
CREATE INDEX idx_payables_due ON payables(due_date) WHERE status NOT IN ('Odendi','Iptal');
```

### `receivables` — Alacak Kayıtları
```sql
CREATE TABLE receivables (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  progress_payment_id UUID,
  original_amount     NUMERIC(18,2) NOT NULL,
  remaining_amount    NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  due_date            DATE,
  status              VARCHAR(50) NOT NULL DEFAULT 'Acik'
  -- Acik→KısmiTahsilat→Tahsil Edildi→Vadesi Gecti→Iptal
);
```

### `payments` — Ödemeler
```sql
CREATE TABLE payments (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  payment_date        DATE NOT NULL,
  amount              NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  payment_method      VARCHAR(50) NOT NULL,
  -- Havale, EFT, Çek, Kasa, KrediKarti
  bank_account_id     UUID REFERENCES bank_accounts(id),
  reference_number    VARCHAR(200),
  status              VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Onaylandi→Tamamlandi→Iptal
  approval_request_id UUID,
  notes               TEXT
);
```

### `payment_allocations` — Ödeme-Borç Dağılımı
```sql
CREATE TABLE payment_allocations (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  payment_id        UUID NOT NULL REFERENCES payments(id),
  payable_id        UUID NOT NULL REFERENCES payables(id),
  allocated_amount  NUMERIC(18,2) NOT NULL,
  CONSTRAINT uq_payment_alloc UNIQUE (payment_id, payable_id)
);
```

### `collections` ve `collection_allocations` — Tahsilatlar
```sql
CREATE TABLE collections (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  collection_date     DATE NOT NULL,
  amount              NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  payment_method      VARCHAR(50) NOT NULL,
  bank_account_id     UUID REFERENCES bank_accounts(id),
  reference_number    VARCHAR(200),
  status              VARCHAR(50) NOT NULL DEFAULT 'Taslak'
);

CREATE TABLE collection_allocations (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  collection_id     UUID NOT NULL REFERENCES collections(id),
  receivable_id     UUID NOT NULL REFERENCES receivables(id),
  allocated_amount  NUMERIC(18,2) NOT NULL
);
```

---

## 4.16 Sözleşmeler Modülü

### `contracts` — Sözleşmeler
```sql
CREATE TABLE contracts (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  contract_number     VARCHAR(100) NOT NULL UNIQUE,
  contract_type       VARCHAR(50) NOT NULL,
  -- Musteri, Tedarikci, Taseron, Kiralama, Hizmet
  project_id          UUID REFERENCES projects(id),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  start_date          DATE NOT NULL,
  end_date            DATE,
  total_amount        NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  status              VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Aktif→Tamamlandi→Feshedildi→Askiya Alindi
  description         TEXT,
  approval_request_id UUID
);
```

### `contract_lines` — Sözleşme Kalemleri
```sql
CREATE TABLE contract_lines (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  contract_id     UUID NOT NULL REFERENCES contracts(id),
  phase_id        UUID REFERENCES project_phases(id),
  description     TEXT NOT NULL,
  quantity        NUMERIC(18,4),
  unit_id         UUID REFERENCES units_of_measure(id),
  unit_price      NUMERIC(18,4) NOT NULL,
  total_price     NUMERIC(18,2),
  sort_order      INTEGER NOT NULL DEFAULT 0
);
```

---

## 4.17 Hakediş Modülü

### `progress_payments` — Hakediş Başlıkları
```sql
CREATE TABLE progress_payments (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id        UUID NOT NULL REFERENCES companies(id),
  payment_number    VARCHAR(100) NOT NULL UNIQUE,
  contract_id       UUID NOT NULL REFERENCES contracts(id),
  project_id        UUID NOT NULL REFERENCES projects(id),
  period_start      DATE NOT NULL,
  period_end        DATE NOT NULL,
  gross_amount      NUMERIC(18,2) NOT NULL,    -- Brüt hakediş
  deduction_amount  NUMERIC(18,2) NOT NULL DEFAULT 0,  -- Toplam kesinti
  net_amount        NUMERIC(18,2) NOT NULL,    -- Net hakediş
  currency_id       UUID NOT NULL REFERENCES currencies(id),
  status            VARCHAR(50) NOT NULL DEFAULT 'Taslak',
  -- Taslak→Gonderildi→OnayDa→Onaylandi→Faturalandı→Odendi
  approval_request_id UUID,
  measurement_sheet_id UUID REFERENCES measurement_sheets(id)
);
```

### `progress_payment_lines` — Hakediş Satırları
```sql
CREATE TABLE progress_payment_lines (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  progress_payment_id    UUID NOT NULL REFERENCES progress_payments(id),
  phase_id               UUID NOT NULL REFERENCES project_phases(id),
  contract_line_id       UUID REFERENCES contract_lines(id),
  measured_quantity      NUMERIC(18,4) NOT NULL,
  unit_id                UUID NOT NULL REFERENCES units_of_measure(id),
  unit_price             NUMERIC(18,4) NOT NULL,   -- Sözleşme birim fiyatı
  current_period_amount  NUMERIC(18,2) NOT NULL,   -- Bu dönem tutarı
  cumulative_amount      NUMERIC(18,2) NOT NULL    -- Birikimli tutar
);
```

### `progress_payment_deductions` — Hakediş Kesintileri
```sql
CREATE TABLE progress_payment_deductions (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  progress_payment_id UUID NOT NULL REFERENCES progress_payments(id),
  deduction_type      VARCHAR(100) NOT NULL,
  -- AvansGeri, TeminatParasi, Ceza, Vergi, Diger
  amount              NUMERIC(18,2) NOT NULL,
  description         TEXT
);
```

---

## 4.18 İş Akışı (Onay) Motoru Modülü

### `approval_definitions` — Onay Akışı Tanımları
```sql
CREATE TABLE approval_definitions (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code                VARCHAR(100) NOT NULL UNIQUE,
  -- APR-SATIN-ALMA, APR-TALEP, APR-HAKEDIS, APR-MASRAF vb.
  name                VARCHAR(255) NOT NULL,
  related_module      VARCHAR(100) NOT NULL,
  related_entity_type VARCHAR(100) NOT NULL,
  is_active           BOOLEAN NOT NULL DEFAULT true
);
```

### `approval_definition_versions` — Akış Versiyonları
```sql
CREATE TABLE approval_definition_versions (
  id                 UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  definition_id      UUID NOT NULL REFERENCES approval_definitions(id),
  version_number     INTEGER NOT NULL,
  is_current_version BOOLEAN NOT NULL DEFAULT false,   -- Yalnızca bir satır true olabilir
  effective_date     DATE NOT NULL,
  description        TEXT
);
```

### `approval_step_definitions` — Onay Adımı Tanımları
```sql
CREATE TABLE approval_step_definitions (
  id                      UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  version_id              UUID NOT NULL REFERENCES approval_definition_versions(id),
  step_number             INTEGER NOT NULL,
  name                    VARCHAR(255) NOT NULL,
  approval_mode           VARCHAR(30) NOT NULL,
  -- Sirali, ParalelBiri, ParalelTumu, Quorum
  required_approval_count INTEGER,    -- Quorum için
  is_required             BOOLEAN NOT NULL DEFAULT true,
  timeout_hours           INTEGER,    -- Zaman aşımı (saat)
  CONSTRAINT uq_step_version UNIQUE (version_id, step_number)
);
```

### `approval_conditions` — Koşul Tanımları
```sql
CREATE TABLE approval_conditions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  version_id  UUID NOT NULL REFERENCES approval_definition_versions(id),
  field_name  VARCHAR(100) NOT NULL,
  -- ToplamTutar, ProjeId, DepartmanId, Oncelik, vb.
  operator    VARCHAR(30) NOT NULL,
  -- Esit, BuyukTur, KucukTur, IceriyorMu, IcermiyorMu
  value       TEXT NOT NULL,
  group_id    INTEGER DEFAULT 1
  -- Aynı gruptaki koşullar AND ile, gruplar OR ile birleşir
);
```

### `approval_requests` — Çalışan Onay Talepleri
```sql
CREATE TABLE approval_requests (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  definition_version_id UUID NOT NULL REFERENCES approval_definition_versions(id),
  related_entity_type   VARCHAR(100) NOT NULL,
  related_entity_id     UUID NOT NULL,
  requested_by          UUID NOT NULL REFERENCES users(id),
  requested_at          TIMESTAMPTZ NOT NULL DEFAULT now(),
  status                VARCHAR(50) NOT NULL DEFAULT 'Bekliyor',
  -- Taslak→Bekliyor→Onaylandi→Reddedildi→Iade Edildi→Iptal
  current_step_number   INTEGER,
  completed_at          TIMESTAMPTZ,
  notes                 TEXT
);
CREATE INDEX idx_apr_entity ON approval_requests(related_entity_type, related_entity_id);
CREATE INDEX idx_apr_status ON approval_requests(status) WHERE status = 'Bekliyor';
```

### `approval_request_approvers` — Gerçek Onaycılar
```sql
CREATE TABLE approval_request_approvers (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  request_step_id UUID NOT NULL REFERENCES approval_request_steps(id),
  user_id         UUID NOT NULL REFERENCES users(id),
  -- Talep anında kopyalanır — sonraki rol değişikliklerinden etkilenmez
  status          VARCHAR(50) NOT NULL DEFAULT 'Bekliyor',
  -- Bekliyor→Onayladi→Reddetti→Devredildi
  delegated_to    UUID REFERENCES users(id)
);
```

### `approval_actions` — Onay Kararları
```sql
CREATE TABLE approval_actions (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  request_step_id UUID NOT NULL REFERENCES approval_request_steps(id),
  approver_id     UUID NOT NULL REFERENCES users(id),
  action_type     VARCHAR(30) NOT NULL,
  -- Onayla, Reddet, IadeEt, Iptal
  action_date     TIMESTAMPTZ NOT NULL DEFAULT now(),
  comment         TEXT    -- Onaycı notu
);
```

### `approval_delegations` — Geçici Onay Yetkisi Devri
```sql
CREATE TABLE approval_delegations (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  delegator_id  UUID NOT NULL REFERENCES users(id),   -- Yetkiyi devreden
  delegate_id   UUID NOT NULL REFERENCES users(id),   -- Yetkiyi alan (vekil)
  valid_from    TIMESTAMPTZ NOT NULL,
  valid_to      TIMESTAMPTZ NOT NULL,
  reason        TEXT,    -- 'Yıllık izin', 'İş seyahati', vb.
  is_active     BOOLEAN NOT NULL DEFAULT true,
  CONSTRAINT chk_delegation_dates CHECK (valid_to > valid_from)
);
CREATE INDEX idx_delegations_active ON approval_delegations(delegator_id, valid_from, valid_to)
  WHERE is_active = true;
```

---

## 4.19 Bildirimler Modülü

### `notifications` — Bildirim Başlıkları
```sql
CREATE TABLE notifications (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  title               VARCHAR(500) NOT NULL,
  body                TEXT NOT NULL,
  notification_type   VARCHAR(100) NOT NULL,
  -- OnayTalebi, StokUyarisi, VadeYaklasıyor, BütçeSapması, SistemBilgisi
  related_entity_type VARCHAR(100),
  related_entity_id   UUID,
  priority            VARCHAR(20) NOT NULL DEFAULT 'Normal',
  -- Dusuk, Normal, Yuksek, Kritik
  created_at          TIMESTAMPTZ NOT NULL DEFAULT now()
);
```

### `notification_recipients` — Bildirim Alıcıları
```sql
CREATE TABLE notification_recipients (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  notification_id UUID NOT NULL REFERENCES notifications(id),
  user_id         UUID NOT NULL REFERENCES users(id),
  is_read         BOOLEAN NOT NULL DEFAULT false,
  read_at         TIMESTAMPTZ,
  channel         VARCHAR(20) NOT NULL DEFAULT 'UygulamaIci',
  -- UygulamaIci, EPosta, SMS
  sent_at         TIMESTAMPTZ,
  delivery_status VARCHAR(30) DEFAULT 'Bekliyor'
  -- Bekliyor, Gonderildi, Ulaşti, Hata
);
CREATE INDEX idx_notif_user_unread ON notification_recipients(user_id, is_read)
  WHERE is_read = false;
```

### `notification_preferences` — Bildirim Tercihleri
```sql
CREATE TABLE notification_preferences (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id           UUID NOT NULL REFERENCES users(id),
  notification_type VARCHAR(100) NOT NULL,
  channel           VARCHAR(20) NOT NULL,
  is_enabled        BOOLEAN NOT NULL DEFAULT true,
  CONSTRAINT uq_notif_pref UNIQUE (user_id, notification_type, channel)
);
```

---

## 4.20 Sohbet Modülü

### `chat_groups` — Sohbet Grupları
```sql
CREATE TABLE chat_groups (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name                VARCHAR(255),
  owner_id            UUID NOT NULL REFERENCES users(id),
  is_private          BOOLEAN NOT NULL DEFAULT false,   -- true = bire bir
  related_entity_type VARCHAR(100),  -- 'Proje', 'Sozlesme', vb.
  related_entity_id   UUID,
  created_at          TIMESTAMPTZ NOT NULL DEFAULT now()
);
```

### `chat_messages` — Sohbet Mesajları
```sql
CREATE TABLE chat_messages (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  group_id            UUID NOT NULL REFERENCES chat_groups(id),
  sender_id           UUID NOT NULL REFERENCES users(id),
  reply_to_message_id UUID REFERENCES chat_messages(id),
  content             TEXT,
  message_type        VARCHAR(30) NOT NULL DEFAULT 'Metin',
  -- Metin, Dosya, Resim, Sistem
  file_url            TEXT,
  is_edited           BOOLEAN NOT NULL DEFAULT false,
  is_deleted          BOOLEAN NOT NULL DEFAULT false,
  created_at          TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX idx_chat_msgs_group ON chat_messages(group_id, created_at DESC);
```

---

# 5. API Spesifikasyonu — Eksiksiz

## 5.1 API Tasarım Standartları

- **Temel URL:** `/api/v1`
- **Auth Başlığı:** `Authorization: Bearer <erişim_token>`
- **İçerik Türü:** `application/json`
- **Sayfalama:** `?page=1&pageSize=20` → yanıt `{ data, total, page, pageSize, totalPages }` içerir
- **Filtreleme:** `?filter[status]=Aktif&filter[projectId]=<uuid>`
- **Sıralama:** `?sort=createdAt:desc`

**Hata formatı:**
```json
{
  "success": false,
  "error": {
    "code": "DOGRULAMA_HATASI",
    "message": "İnsan tarafından okunabilir mesaj",
    "details": [{ "field": "email", "message": "Geçersiz e-posta formatı" }]
  },
  "requestId": "req_abc123"
}
```

**Başarı formatı:**
```json
{
  "success": true,
  "data": { ... },
  "meta": { "total": 100, "page": 1 }
}
```

## 5.2 Standart HTTP Durum Kodları

| Kod | Kullanım |
|-----|---------|
| 200 | Başarılı (GET, PUT, PATCH) |
| 201 | Oluşturuldu (POST) |
| 204 | İçerik yok (DELETE) |
| 400 | Doğrulama hatası |
| 401 | Kimlik doğrulanmamış |
| 403 | Yasak (izin yok) |
| 404 | Bulunamadı |
| 409 | Çakışma (duplicate, durum ihlali) |
| 422 | İş kuralı ihlali |
| 429 | Hız sınırı aşıldı |
| 500 | Sunucu iç hatası |

---

## 5.3 Kimlik Doğrulama Uç Noktaları

### `POST /api/auth/login` — Giriş
```json
// İstek
{
  "username": "ahmet.yilmaz",
  "password": "GucluSifre123!"
}

// Yanıt 200
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGci...",
    "expiresIn": 900,
    "user": {
      "id": "uuid",
      "username": "ahmet.yilmaz",
      "email": "ahmet@sirket.com",
      "firstName": "Ahmet",
      "lastName": "Yılmaz",
      "roles": ["ProjeYoneticisi"],
      "permissions": ["Projeler.Oku", "IsEmirleri.Olustur"]
    }
  }
}
```
**Not:** Yenileme token'ı `HttpOnly` çerez olarak set edilir.

### `POST /api/auth/refresh` — Token Yenileme
Yenileme token çerezini yeni erişim token'ı ile değiştirir.

### `POST /api/auth/logout` — Çıkış
Mevcut yenileme token'ını iptal eder.

### `POST /api/auth/change-password` — Şifre Değiştirme
```json
{
  "currentPassword": "EskiSifre123!",
  "newPassword": "YeniSifre456!",
  "confirmPassword": "YeniSifre456!"
}
```

---

## 5.4 Projeler Modülü Uç Noktaları

### `POST /api/v1/projects` — Proje Oluştur
```json
{
  "name": "Ankara Güneş Enerji Santrali",
  "typeId": "uuid",
  "customerId": "uuid",
  "startDate": "2026-07-01",
  "endDate": "2027-06-30",
  "budgetAmount": 5000000,
  "currencyId": "uuid-try",
  "branchId": "uuid",
  "description": "50MW güneş kurulumu"
}
```
**Yanıt 201:** Otomatik oluşturulan `projectNumber` ile proje nesnesi.

### `PATCH /api/v1/projects/:id/status` — Proje Durumu Değiştir
```json
{ "status": "Aktif", "reason": "Tüm ön koşullar karşılandı" }
```
Geçerli geçişler: `Taslak→Aktif`, `Aktif→Askıda`, `Askıda→Aktif`, `Aktif→Tamamlandı`, `Tamamlandı→Kapatıldı`, `Taslak/Aktif→İptal`

### `GET /api/v1/projects/:id/summary` — Proje Özeti
```json
{
  "projectNumber": "PRJ-2026-0001",
  "name": "Ankara Güneş Enerji Santrali",
  "status": "Aktif",
  "completionPercentage": 34.5,
  "budgetAmount": 5000000,
  "spentAmount": 1725000,
  "remainingBudget": 3275000,
  "activeWorkOrders": 12,
  "pendingApprovals": 3,
  "openPayables": 425000,
  "openReceivables": 890000
}
```

---

## 5.5 Stok Yönetimi Uç Noktaları

### `GET /api/v1/stock-balances` — Stok Bakiyeleri
```
GET /api/v1/stock-balances?filter[warehouseId]=<uuid>&filter[materialId]=<uuid>
```
**Yanıt:**
```json
{
  "data": [{
    "materialCode": "KAB-0001",
    "materialName": "NYY 3x4 Kablo",
    "warehouseName": "Merkez Depo",
    "onHandQuantity": 1200,
    "reservedQuantity": 300,
    "availableQuantity": 900,
    "unitCode": "Metre",
    "averageCost": 18.50
  }]
}
```

### `POST /api/v1/stock-documents` — Stok Belgesi Oluştur
```json
{
  "documentTypeId": "uuid",
  "documentDate": "2026-06-18",
  "warehouseId": "uuid",
  "projectId": "uuid",
  "lines": [{
    "materialId": "uuid",
    "quantity": 500,
    "unitId": "uuid",
    "unitCost": 18.50
  }]
}
```

### `POST /api/v1/stock-documents/:id/post` — Belgeyi Kesinleştir
FIFO lot tahsisi yapar → `stock_transactions` ekler → `stock_balances` günceller.

---

## 5.6 Satın Alma Uç Noktaları

```
POST   /api/v1/supplier-quotes            Teklif oluştur
GET    /api/v1/supplier-quotes/compare    Teklif karşılaştırma (?requestId=uuid)
POST   /api/v1/purchase-orders            Satın alma siparişi oluştur
POST   /api/v1/purchase-orders/:id/submit Onaya gönder
POST   /api/v1/purchase-receipts          Mal kabul oluştur
POST   /api/v1/purchase-receipts/:id/complete  Mal kabulü tamamla → stok girişi otomatik
POST   /api/v1/supplier-invoices          Fatura kaydet
POST   /api/v1/supplier-invoices/:id/match   3-yönlü eşleştirme tetikle
POST   /api/v1/supplier-invoices/:id/approve  Faturayı onayla → borç kaydı oluşturulur
```

---

## 5.7 İş Akışı (Onay) Uç Noktaları

### `GET /api/v1/approval-requests/my-queue` — Bekleyen Onaylarım
Giriş yapan kullanıcı için bekleyen onay kuyruğunu döndürür.

### `POST /api/v1/approval-requests/:id/approve` — Onayla
```json
{ "comment": "Teklifler incelendi, uygun görülmüştür." }
```

### `POST /api/v1/approval-requests/:id/reject` — Reddet
```json
{ "comment": "Fiyat yüksek. Lütfen yeniden teklif alınız." }
```

### `POST /api/v1/approval-requests/:id/return` — İade Et
```json
{ "comment": "Teknik şartname eksik, lütfen revize ediniz." }
```

### `POST /api/v1/approval-delegations` — Onay Yetkisi Devret
```json
{
  "delegateId": "uuid",
  "validFrom": "2026-07-01T00:00:00Z",
  "validTo": "2026-07-15T23:59:59Z",
  "reason": "Yıllık izin"
}
```

---

## 5.8 Finans Uç Noktaları

```
POST   /api/v1/payments               Ödeme emri oluştur
POST   /api/v1/payments/:id/allocate  Borçlara kapat
  Gövde: [{ "payableId": "uuid", "amount": 50000 }]

POST   /api/v1/collections            Tahsilat kaydet
POST   /api/v1/collections/:id/allocate  Alacaklara kapat

GET    /api/v1/finance/dashboard       Finans özeti
  Yanıt: { toplamBorc, vadesGecmisBorc, toplamAlacak,
            vadesGecmisAlacak, nakit, buHaftaOdenecek }
```

---

## 5.9 Hakediş Uç Noktaları

```
POST   /api/v1/progress-payments           Hakediş oluştur
GET    /api/v1/progress-payments           Listele (proje, sözleşme, durum filtresi)
GET    /api/v1/progress-payments/:id       Detay (satırlar ve kesintilerle)
POST   /api/v1/progress-payments/:id/submit  Onaya gönder
POST   /api/v1/progress-payments/:id/invoice  Faturaya çevir → alacak kaydı oluşturulur
GET    /api/v1/progress-payments/:id/pdf   PDF önizleme
```

---

## 5.10 Bildirimler ve Sohbet Uç Noktaları

```
GET    /api/v1/notifications/me          Bildirimlerim (okunmamış sayısı)
PATCH  /api/v1/notifications/:id/read   Okundu işaretle
POST   /api/v1/notifications/mark-all-read  Tümünü okundu yap
PUT    /api/v1/notification-preferences Kanal tercihlerini güncelle

GET    /api/v1/chat/groups             Grup listesi
POST   /api/v1/chat/groups            Grup oluştur
POST   /api/v1/chat/groups/:id/messages  Mesaj gönder
GET    /api/v1/chat/groups/:id/messages  Geçmiş (cursor tabanlı sayfalama)
```

**WebSocket olayları (Socket.IO):**
```
İstemci → Sunucu:
  chat:join_group      { groupId }
  chat:send_message    { groupId, content, replyToId? }
  chat:typing          { groupId }

Sunucu → İstemci:
  chat:new_message     { mesaj nesnesi }
  chat:user_typing     { userId, groupId }
  notification:new     { bildirim nesnesi }
  approval:action      { requestId, action, entityType, entityId }
```

---

# 6. İş Akışları — Uçtan Uca

## 6.1 Malzeme Tedarik Süreci

```
┌────────────────────────────────────────────────────────────────────────┐
│ TETİKLEYİCİ: Proje fazı için stokta yeterlі malzeme yok               │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 1. TALEP OLUŞTURMA                                                     │
│    Aktör: Proje Yöneticisi / Saha Sorumlusu                           │
│    • POST /api/v1/requests (Taslak)                                    │
│    • Satırlar eklenir (malzeme, miktar, istenen tarih)                 │
│    • POST /api/v1/requests/:id/submit → OnayBekliyor                  │
│    • SİSTEM: APR-TALEP onay akışı başlatılır                          │
│    • SİSTEM: Onayculara bildirim gönderilir                           │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │ Onaylandı
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 2. TEKLİF TOPLAMA                                                      │
│    Aktör: Satın Alma Ekibi                                            │
│    • Her tedarikçi için supplier_quotes oluşturulur                   │
│    • Teklifler alınır ve karşılaştırılır                              │
│    • GET /api/v1/supplier-quotes/compare?requestId=<uuid>             │
│    • En uygun teklif seçilir → Durum: Kabul                          │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 3. SATIN ALMA SİPARİŞİ                                                │
│    • POST /api/v1/purchase-orders → Taslak                            │
│    • POST /api/v1/purchase-orders/:id/submit → Onaya gönder           │
│    • SİSTEM: Koşullar değerlendirilir (APR-SATIN-ALMA):               │
│      0–50.000 TL    → Yalnızca Proje Yöneticisi                       │
│      50K–250K TL    → Satın Alma Müd. + Finans Müd. (Sıralı)         │
│      250K+ TL       → + Genel Müdür (Paralel Tümü + Sıralı)          │
│    • Tüm adımlar tamamlandığında → Onaylandı                         │
│    • SİSTEM (opsiyonel): Tedarikçiye sipariş e-postayla gönderilir    │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 4. MAL KABUL                                                           │
│    Aktör: Depo Sorumlusu                                              │
│    • POST /api/v1/purchase-receipts                                   │
│    • POST /api/v1/purchase-receipts/:id/complete                      │
│    • SİSTEM: Otomatik StokBelgesi [Tür=MalKabul] oluşturulur         │
│    • SİSTEM: Belge kesinleştirilir (posted):                          │
│      → StockLots oluşturulur (maliyet katmanı)                       │
│      → StockTransactions eklenir (değiştirilemez)                    │
│      → StockBalances güncellenir                                     │
│    • Sipariş → KısmiTeslim veya Tamamlandı                           │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 5. FATURA İŞLEME (3-Yönlü Eşleştirme)                                │
│    • POST /api/v1/supplier-invoices                                   │
│    • POST /api/v1/supplier-invoices/:id/match                         │
│    • SİSTEM kontrol eder:                                             │
│      Fatura miktarı ≤ Mal Kabul miktarı ≤ Sipariş miktarı?          │
│      Fatura birim fiyatı ile Sipariş fiyatı ±%5 tolerans içinde?     │
│    • Tolerans içindeyse → Eşleşti → Onaylandı                       │
│    • >%5 sapma → ManuelInceleme (Satın Alma Müdürüne bildirim)       │
│    • Onaylandıktan sonra → Borç kaydı (Payables) oluşturulur         │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 6. ÖDEME                                                               │
│    Aktör: Finans Ekibi                                                │
│    • POST /api/v1/payments                                            │
│    • POST /api/v1/payments/:id/allocate → Borçlara bağla             │
│    • Onay gerekliyse: POST /api/v1/payments/:id/submit                │
│    • Tamamlandığında: Payable.remainingAmount azalır                  │
│      Durum → KısmiÖdendi veya Ödendi                                 │
└────────────────────────────────────────────────────────────────────────┘
```

---

## 6.2 Saha Operasyonu ve Hakediş Süreci

```
┌────────────────────────────────────────────────────────────────────────┐
│ 1. İŞ EMİRLERİ VE GÜNLÜK SAHA RAPORLARI                              │
│    • Saha ekibine iş emirleri atanır                                  │
│    • Her gün: POST /api/v1/projects/:id/site-reports                  │
│      → Çalışan personel, kullanılan ekipman, harcanan malzeme         │
│    • İş emri ilerlemesi güncellenir                                   │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 2. İLERLEME KAYITLARI                                                 │
│    • POST /api/v1/work-orders/:id/progress                            │
│    • Her faz/iş emri için tamamlanan miktar kaydedilir                │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │ Dönem sonu
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 3. METRAJ                                                              │
│    Aktör: Saha Sorumlusu                                              │
│    • POST /api/v1/measurement-sheets                                  │
│    • Satırlar: her faz için dönem metrajı                             │
│    • POST /api/v1/measurement-sheets/:id/submit                       │
│    • Onay: Saha Sorumlusu → Proje Yöneticisi → Onaylandı             │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 4. HAKEDİŞ OLUŞTURMA                                                  │
│    Aktör: Proje Yöneticisi                                            │
│    • POST /api/v1/progress-payments                                   │
│    • Satırlar metrajdan otomatik doldurulur:                          │
│      Ölçülen Miktar × Sözleşme Birim Fiyatı = Dönem Tutarı           │
│    • Kesintiler eklenir:                                              │
│      Avans Geri Ödemesi: brüt × %10                                  │
│      Teminat Parası: brüt × %5                                       │
│    • Net Hakediş = Brüt - Kesintiler                                 │
│    • POST /api/v1/progress-payments/:id/submit                        │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 5. HAKEDİŞ ONAYI (APR-HAKEDİŞ)                                       │
│    Adım 1: Saha Sorumlusu (Sıralı) — Saha kontrolü                   │
│    Adım 2: Proje Yöneticisi (Sıralı) — Proje onayı                   │
│    Adım 3: Finans Yöneticisi + Genel Müdür (Paralel Tümü)            │
│    → Durum: Onaylandı                                                │
└──────────────────────────────┬─────────────────────────────────────────┘
                               │
                               ▼
┌────────────────────────────────────────────────────────────────────────┐
│ 6. FATURALAMA VE TAHSİLAT                                             │
│    • POST /api/v1/progress-payments/:id/invoice                       │
│    • SİSTEM: Alacak kaydı (Receivables) oluşturulur                  │
│    • Müşteri ödeme yapınca: POST /api/v1/collections                  │
│    • POST /api/v1/collections/:id/allocate                            │
│    • Receivable.status → Tahsil Edildi                               │
└────────────────────────────────────────────────────────────────────────┘
```

---

## 6.3 Onay Motoru İş Akışı

```
İş belgesi onaya gönderildi (örn. SatınAlmaSiparişi)
         ↓
SERVİS: entityType='purchase_orders' için ApprovalDefinition bulunur
         ↓
Yürürlükteki versiyon seçilir (is_current_version=true)
         ↓
ApprovalConditions değerlendirilir (örn. TotalAmount > 50000?)
         ↓
ApprovalRequests kaydı oluşturulur (Durum=Bekliyor)
         ↓
HER ApprovalStepDefinition için (step_number sırasıyla):
  ApprovalRequestSteps oluşturulur (Durum=Bekliyor)
         ↓
Adım 1 aktif hale getirilir
  Onaycılar çözümlenir (Kullanıcı / Rol / ProjeRolü / BölümYöneticisi)
  ApprovalDelegations kontrol edilir (tarih aralığı aktif mi?)
  ApprovalRequestApprovers anlık görüntüsü alınır
  Her onaycıya bildirim gönderilir
         ↓
Onaycı karar verir → ApprovalActions kaydedilir
         ↓
ApprovalMode'a göre adım tamamlanma kontrolü:
  Sıralı/ParalelTümü → tüm onaycılar onaylamalı
  ParalelBiri        → bir onay yeterli
  Quorum             → requiredApprovalCount kadar onay yeterli
         ↓
[Onayla]: Sonraki adım aktifleştirilir → (döngü)
          Tüm adımlar bittiyse → Onaylandı → Belge durumu güncellenir
[Reddet]: Reddedildi → Talep sahibine bildirim
[İade]:   İade Edildi → Belge Taslak'a döner, revizyon için
[İptal]:  İptal → İşlem sonlandırılır
```

---

# 7. Olay Güdümlü Tasarım

## 7.1 Olay Envanteri

| Olay | Üretici | Tüketiciler |
|------|---------|-------------|
| `onay.gonderildi` | OnayServisi | BildirimServisi, DenetimServisi |
| `onay.onaylandi` | OnayServisi | BildirimServisi, İlgili Servisler |
| `onay.reddedildi` | OnayServisi | BildirimServisi, İlgili Servisler |
| `onay.adim_aktif` | OnayServisi | BildirimServisi |
| `stok.belge_kesinlesti` | StokServisi | BakiyeServisi, MaliyetServisi |
| `stok.bakiye_dusuk` | BakiyeServisi | BildirimServisi |
| `stok.rezervasyon_suresi_doldu` | ZamanlamaServisi | BakiyeServisi |
| `siparis.onaylandi` | OnayServisi | SatınAlmaServisi (tedarikçi bildirim) |
| `mal_kabul.tamamlandi` | SatınAlmaServisi | StokServisi (otomatik giriş) |
| `fatura.eslesti` | SatınAlmaServisi | FinansServisi (borç oluştur) |
| `borclanma.vade_gecti` | ZamanlamaServisi | BildirimServisi, FinansServisi |
| `butce.esik_asildi` | BütçeServisi | BildirimServisi |
| `hakEdis.onaylandi` | OnayServisi | FinansServisi (alacak oluştur) |

## 7.2 Kuyruk Mimarisi

```
Redis Streams (BullMQ ile)

Kuyruklar:
┌──────────────────────────────────────────────────────────────────┐
│ approval-engine (onay-motoru)                                    │
│   İşler: adim-degerlendır, adim-ilerlet, zaman-asimi-kontrol    │
│   Eş zamanlılık: 5                                              │
│   Yeniden deneme: 3 deneme, üstel geri çekilme (1s, 5s, 30s)   │
│   Ölü harf kuyruğu: approval-engine-failed                      │
│                                                                  │
│ stock-recalc (stok-hesaplama)                                    │
│   İşler: bakiye-hesapla, rezervasyon-sona-erdır                 │
│   Eş zamanlılık: 3                                              │
│                                                                  │
│ notification-dispatch (bildirim-gonderimi)                       │
│   İşler: uygulama-ici-gonder, eposta-gonder, sms-gonder         │
│   Eş zamanlılık: 20                                             │
│                                                                  │
│ sequence-generation (sira-numarasi)                              │
│   Eş zamanlılık: 1 ← Seri sayaç için MUTLAKA 1 olmalı          │
│                                                                  │
│ scheduled-jobs (zamanlamali-isler — BullMQ cron)                 │
│   rezervasyon-sona-erdır: her 5 dakika                          │
│   vadesi-gecen-borc-kontrol: günlük 09:00                       │
│   vadesi-gecen-alacak-kontrol: günlük 09:00                     │
│   butce-sapma-kontrol: günlük 08:00                             │
│   onay-zaman-asimi-kontrol: her 30 dakika                       │
└──────────────────────────────────────────────────────────────────┘
```

## 7.3 Olay Payload Standardı

```typescript
interface TemelOlay {
  eventId: string;        // UUID
  eventType: string;      // 'onay.onaylandi'
  occurredAt: string;     // ISO 8601
  correlationId: string;  // İstek izleme ID'si
  userId: string | null;  // Aktör (sistem olayları için null)
  companyId: string;
  payload: Record<string, unknown>;
}
```

---

# 8. Güvenlik Mimarisi

## 8.1 Kimlik Doğrulama Akışı

```
1. POST /api/auth/login
   → Kimlik bilgilerini doğrula
   → is_locked, is_active kontrolü
   → Başarısızlıkta: failed_login_count artır
     5 veya üzeri başarısız giriş → is_locked = true, yöneticiye bildirim
   → Başarıda: failed_login_count sıfırla
     Erişim token'ı üret (JWT, 15dk, RS256)
     Yenileme token'ı üret (crypto.randomBytes(64), SHA-256 hash ile sakla)
     Set-Cookie: refreshToken=<token>; HttpOnly; Secure; SameSite=Strict
     Döndür: { accessToken, expiresIn, user }

2. Her API isteğinde:
   → Authorization başlığından Bearer token'ı al
   → JWT imzasını doğrula (RS256 açık anahtar)
   → exp alanını kontrol et
   → userId, companyId, permissions[] JWT'den çıkar
   → Middleware: endpoint için izin kontrolü yap
   → Başarısızlıkta → 401 veya 403

3. Token yenileme (POST /api/auth/refresh):
   → HttpOnly çerezden yenileme token'ını oku
   → SHA-256 hash'ini al
   → refresh_tokens tablosunda ara (token_hash = ? AND expires_at > now() AND revoked_at IS NULL)
   → Yeni erişim token'ı üret
   → Yenileme token'ını döngüle (eskiyi iptal et, yenisini oluştur)
```

## 8.2 JWT Payload Yapısı

```json
{
  "sub": "kullanici-uuid",
  "iat": 1718710000,
  "exp": 1718710900,
  "jti": "token-uuid",
  "companyId": "uuid",
  "branchId": "uuid",
  "roles": ["ProjeYoneticisi"],
  "permissions": ["Projeler.Oku", "IsEmirleri.Olustur"]
}
```

## 8.3 İzin Değerlendirme Algoritması

```typescript
function izinVarMi(userId: string, izinKodu: string): boolean {
  // 1. Kullanıcı düzeyinde reddetme kontrolü (en yüksek öncelik)
  const kullaniciReddi = kullaniciIzinleri.find(
    p => p.userId === userId && p.permissionCode === izinKodu
       && p.isGranted === false && tarihGecerlimi(p)
  );
  if (kullaniciReddi) return false;

  // 2. Kullanıcı düzeyinde verme kontrolü
  const kullaniciVermesi = kullaniciIzinleri.find(
    p => p.userId === userId && p.permissionCode === izinKodu
       && p.isGranted === true && tarihGecerlimi(p)
  );
  if (kullaniciVermesi) return true;

  // 3. Roller üzerinden kontrol
  const kullaniciRolleri = getKullaniciRolleri(userId).filter(tarihGecerlimi);
  return kullaniciRolleri.some(ur =>
    rolIzinleri.some(rp => rp.roleId === ur.roleId && rp.permissionCode === izinKodu)
  );
}
```

**Önbellekleme:** İzin setleri kullanıcı başına Redis'te 5 dakika TTL ile önbelleğe alınır. Rol/izin değişikliğinde önbellek anahtarı anında geçersiz kılınır.

## 8.4 Hız Sınırlama

| Endpoint Grubu | Limit | Pencere |
|----------------|-------|---------|
| `POST /api/auth/login` | 10 istek | IP başına 15 dakika |
| `POST /api/auth/refresh` | 30 istek | Kullanıcı başına 1 saat |
| Tüm kimlik doğrulanmış API'ler | 1000 istek | Kullanıcı başına 1 dakika |
| Dosya yükleme uç noktaları | 20 istek | Kullanıcı başına 1 saat |

**Uygulama:** Redis kayan pencere sayacı.

## 8.5 Veri Güvenliği

| Konu | Uygulama |
|------|---------|
| Şifreler | bcrypt, maliyet faktörü 12 |
| Gizli bilgiler (API anahtarları, SMTP) | Ortam değişkenleri, asla veritabanında saklanmaz |
| Dosya depolama | S3 imzalı URL (15dk geçerlilik), doğrudan genel erişim yok |
| Aktarım güvenliği | TLS 1.3 zorunlu, HSTS başlığı |
| SQL enjeksiyonu | Drizzle ORM parametreli sorgular |
| XSS | Helmet.js başlıkları, Content-Security-Policy |
| CSRF | SameSite=Strict çerez + form CSRF token'ı |

---

# 9. Gözlemlenebilirlik ve İzleme

## 9.1 Günlük Kayıt Stratejisi

**Kütüphane:** Pino (yapılandırılmış JSON logları)

**Log seviyeleri:**
- `error`: Yakalanmamış istisnalar, iş mantığı hataları
- `warn`: Doğrulama hataları, yavaş sorgular
- `info`: İstek/yanıt yaşam döngüsü, iş olayları
- `debug`: Ayrıntılı akış izleri (yalnızca geliştirme/staging)

**Yapılandırılmış log formatı:**
```json
{
  "level": "info",
  "time": "2026-06-18T14:35:00.123Z",
  "requestId": "req_abc123",
  "correlationId": "corr_xyz",
  "userId": "uuid",
  "companyId": "uuid",
  "method": "POST",
  "path": "/api/v1/purchase-orders",
  "statusCode": 201,
  "durationMs": 87,
  "entityType": "purchase_orders",
  "entityId": "uuid",
  "msg": "SatınAlmaSiparişi oluşturuldu"
}
```

**Hassas veriler:** Şifreler, token'lar, ödeme kartı verileri veya tam PII asla loglanmaz. Yalnızca ID'ler loglanır.

## 9.2 Metrikler (Prometheus)

| Metrik | Tür | Etiketler |
|--------|-----|---------|
| `http_requests_total` | Sayaç | method, path, status_code |
| `http_request_duration_seconds` | Histogram | method, path |
| `db_query_duration_seconds` | Histogram | query_type, table |
| `queue_job_duration_seconds` | Histogram | queue_name, job_type |
| `queue_job_failures_total` | Sayaç | queue_name, job_type |
| `approval_requests_total` | Sayaç | definition_code, outcome |
| `stock_documents_posted_total` | Sayaç | document_type |
| `active_websocket_connections` | Ölçer | — |

## 9.3 Uyarı Kuralları

| Uyarı | Koşul | Önem |
|-------|-------|------|
| Yüksek hata oranı | HTTP 5xx > %1 (5 dakika) | Kritik |
| Yüksek gecikme | P99 > 2s (5 dakika) | Uyarı |
| Veritabanı bağlantı havuzu | Kullanım > %80 | Uyarı |
| Veritabanı bağlantı havuzu | Kullanım > %95 | Kritik |
| Kuyruk derinliği | Herhangi bir kuyruk > 1000 (10 dakika) | Uyarı |
| İş hatası | > 10 hata (5 dakika) | Kritik |
| Onay zaman aşımı | Adım > TimeoutHours süre bekledi | Uyarı |
| Minimum stok altı | Herhangi bir malzeme min_stock_level altında | Uyarı |
| Vadesi geçmiş borç | Yapılandırılmış eşiği aşan tutar | Bilgi |

## 9.4 Sağlık Kontrol Uç Noktaları

```
GET /healthz    → Sunucu çalışıyorsa 200 (yük dengeleyici kontrolü)
GET /readyz     → DB + Redis bağlıysa 200 (hazırlık kontrolü)
GET /api/metrics → Prometheus metrikleri (yalnızca dahili)
```

---

# 10. Altyapı ve Deployment

## 10.1 Ortam Mimarisi

```
┌──────────────────┬──────────────────┬────────────────────┐
│   Geliştirme     │     Staging       │    Üretim           │
├──────────────────┼──────────────────┼────────────────────┤
│ Docker Compose   │ Kubernetes (1 rep)│ Kubernetes (HA)    │
│ Yerel PG + Redis │ Yönetilen PG      │ Yönetilen PG + Rep │
│ Canlı yenileme  │ Staging alan adı  │ Özel alan + CDN     │
│ Auth opsiyonel  │ Tam auth          │ Tam auth + WAF      │
│ Seed verisi     │ Anonimleştirilmiş │ Gerçek veri         │
└──────────────────┴──────────────────┴────────────────────┘
```

## 10.2 Docker Compose (Geliştirme)

```yaml
version: '3.9'
services:
  api:
    build: .
    ports: ["5000:5000"]
    environment:
      DATABASE_URL: postgres://energy:energy@db:5432/energy_dev
      REDIS_URL: redis://redis:6379
      JWT_PRIVATE_KEY_PATH: /keys/private.pem
      SESSION_SECRET: dev-secret
    volumes:
      - .:/app
      - /app/node_modules
    depends_on: [db, redis]

  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: energy_dev
      POSTGRES_USER: energy
      POSTGRES_PASSWORD: energy
    volumes:
      - pg_data:/var/lib/postgresql/data
    ports: ["5432:5432"]

  redis:
    image: redis:7-alpine
    ports: ["6379:6379"]

  worker:
    build: .
    command: node dist/worker.js
    depends_on: [db, redis]

volumes:
  pg_data:
```

## 10.3 Kubernetes Üretim Yapılandırması

```yaml
# Deployment: api-server
replicas: 3
resources:
  requests: { cpu: 500m, memory: 512Mi }
  limits:   { cpu: 2000m, memory: 2Gi }
strategy:
  type: RollingUpdate
  maxUnavailable: 0    # Sıfır kesinti garantisi
  maxSurge: 1

# Yatay Pod Otomatik Ölçeklendirici (HPA)
api:
  minReplicas: 3
  maxReplicas: 10
  metrics:
    - type: Resource
      resource: { name: cpu, target: { averageUtilization: 70 } }
```

## 10.4 Veritabanı Yapılandırması

```sql
-- PgBouncer bağlantı havuzu (pool_mode=transaction)
-- max_client_conn = 200
-- default_pool_size = 25

-- PostgreSQL ayarları (üretim, 8GB RAM)
max_connections = 200
shared_buffers = 2GB
effective_cache_size = 6GB
work_mem = 20MB
wal_level = replica
max_wal_senders = 3
```

**Migrasyon stratejisi:** Drizzle Kit migrasyonları, deployment öncesinde CI/CD tarafından çalıştırılır. Migrasyonlar geri dönük uyumlu olmalıdır (sütun önce eklenir, sonra kaldırılır).

## 10.5 CI/CD Boru Hattı

```yaml
# GitHub Actions
on: push

jobs:
  test:
    - Bağımlılıkları kur (pnpm install --frozen-lockfile)
    - Lint (eslint)
    - Tip kontrolü (tsc --noEmit)
    - Birim testleri (vitest)
    - Entegrasyon testleri (vitest + test DB)

  build:
    needs: test
    - Derleme (esbuild → dist/)
    - Docker derle + kayıt defterine gönder
    - Güvenlik taraması (trivy)

  deploy-staging:
    needs: build
    if: branch = main
    - kubectl set image (staging namespace)
    - DB migrasyonlarını çalıştır
    - Sağlık kontrolü (/readyz)
    - Duman testleri

  deploy-production:
    needs: deploy-staging
    if: branch = main && manuel onay
    - kubectl set image (production namespace)
    - DB migrasyonlarını çalıştır
    - Sağlık kontrolü
    - 10 dakika hata oranı izle
    - Hata oranı > %2 ise otomatik geri al
```

## 10.6 Yedekleme ve Felaket Kurtarma

| Bileşen | Strateji | RTO | RPO |
|---------|----------|-----|-----|
| PostgreSQL | Günlük tam yedek + sürekli WAL arşivleme (S3) | 2 saat | 5 dakika |
| Redis | 15 dakikada bir RDB anlık görüntüsü | 30 dakika | 15 dakika |
| Nesne Depolama (S3/R2) | Bölgeler arası replikasyon | — | Neredeyse sıfır |
| Uygulama konfigürasyonu | Git (IaC) | 30 dakika | — |

**Yedek saklama:** 7 günlük, 4 haftalık, 12 aylık.

---

# 11. Üretime Hazırlık Kontrol Listesi

## 11.1 Veritabanı
- [x] Tüm tablolarda FK indeksleri
- [x] `(status, is_deleted)` kombinasyonlarında bileşik indeksler
- [x] Denetim log tablosu aylık partition'lı
- [x] `stock_transactions` değiştirilemez (UPDATE/DELETE yetkisi yok)
- [x] `sequence_definitions` sayacı SELECT FOR UPDATE ile atomik artırılır
- [x] DB migrasyonları deployment öncesinde çalıştırılır
- [x] PgBouncer bağlantı havuzu yapılandırılmış
- [x] Raporlama sorguları için okuma replikası

## 11.2 API
- [x] OpenAPI 3.1 spec koddan üretilir
- [x] Tüm endpoint'lerde açık izin kontrolü
- [x] Tüm rotalarda çok şirket izolasyon middleware'i
- [x] Her istekte benzersiz requestId üretilir
- [x] Servis katmanına ulaşmadan önce Zod ile tüm girdiler doğrulanır
- [x] İş kuralı hataları 422 ile yapılandırılmış gövde döndürür
- [x] Sayfalama zorunlu (max pageSize=100)
- [x] Dosya yüklemeleri: boyut sınırı, MIME tipi doğrulama

## 11.3 İş Akışı Motoru
- [x] Onaycı anlık görüntüsü talep anında alınır (değiştirilemez)
- [x] Her onay tanımı için yalnızca bir aktif versiyon
- [x] Her bildirim gönderimi öncesinde delege kontrolü
- [x] Zaman aşımı zamanlanmış iş ile yönetilir
- [x] Tüm durum geçişleri atomik (transaction + olay)
- [x] Ret işlemi 60 saniye içinde talep sahibine bildirim gönderir

## 11.4 Stok / Finans
- [x] FIFO tahsisi tek transaction + satır düzeyinde kilit ile yapılır
- [x] `stock_balances` ve `stock_transactions` atomik güncellenir
- [x] 3-yönlü eşleştirme toleransı `system_settings` ile yapılandırılabilir
- [x] Vadesi geçmiş ödeme işi günlük çalışır (idempotent)
- [x] Para birimi dönüşümü işlem tarihindeki kuru kullanır (bugünkü değil)

## 11.5 Güvenlik
- [x] JWT RS256 ile imzalanır (asimetrik anahtarlar)
- [x] Yenileme token'ı her kullanımda döngülenir
- [x] 5 başarısız girişten sonra hesap kilitlenir
- [x] Şifre politikası: min 8 karakter, büyük/küçük harf, rakam, özel karakter
- [x] Auth endpoint'lerinde hız sınırlama
- [x] Tüm dosya URL'leri imzalı (genel erişim yok)
- [x] HTTPS zorunlu, HSTS başlığı
- [x] Her CI çalışmasında bağımlılık denetimi (pnpm audit)

## 11.6 Gözlemlenebilirlik
- [x] requestId ile yapılandırılmış JSON logları
- [x] Prometheus metrik endpoint'i
- [x] OpenTelemetry izleri
- [x] Hata oranı, gecikme, kuyruk derinliği için uyarı kuralları
- [x] Sağlık kontrol endpoint'leri (/healthz, /readyz)
- [x] Uyarı tanımlarında runbook bağlantıları

## 11.7 Operasyonlar
- [x] Sıfır kesintili deployment (RollingUpdate, maxUnavailable=0)
- [x] Deployment sonrası hata artışında otomatik geri alma
- [x] DB migrasyonu geri dönük uyumlu (sütun ekle → test → eski sütunu kaldır)
- [x] Zarif kapatma (SIGTERM'den önce aktif istekler bitirilir)
- [x] Gizli bilgi rotasyon prosedürü belgelenmiş

---

*Energy Üretime Hazır Sistem Tasarımı Dokümanı — v2.0 Sonu*  
*Haziran 2026*
