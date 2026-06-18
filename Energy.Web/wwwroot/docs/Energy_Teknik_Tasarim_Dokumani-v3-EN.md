# Energy — Production-Ready System Design
## Complete Technical Architecture, Database Schema, API Specification & Business Flows

**Version:** 2.0  
**Date:** June 2026  
**Status:** Production-Ready Engineering Reference  
**Audience:** Engineering Teams, Architects, DevOps, QA

---

> This document is the authoritative engineering reference for the Energy platform. A team can open a ticket and begin implementation from any section. Nothing is deferred or left ambiguous.

---

## Table of Contents

1. [System Overview](#1-system-overview)
2. [Architecture Design](#2-architecture-design)
3. [Module Breakdown](#3-module-breakdown)
4. [Database Design — Full Schema](#4-database-design--full-schema)
5. [API Specification — Complete](#5-api-specification--complete)
6. [Business Flows — End-to-End](#6-business-flows--end-to-end)
7. [Event-Driven Design](#7-event-driven-design)
8. [Security Architecture](#8-security-architecture)
9. [Observability & Monitoring](#9-observability--monitoring)
10. [Infrastructure & Deployment](#10-infrastructure--deployment)
11. [Final Production-Ready Checklist](#11-final-production-ready-checklist)

---

# 1. System Overview

## 1.1 Purpose

**Energy** is an enterprise project operations platform for energy and construction companies. It manages the full lifecycle of project-based work: from material procurement and field operations to contract management, invoicing, finance, and compliance.

### Core Capabilities

| Domain | Capability |
|--------|-----------|
| Projects | WBS, phases, team assignments, location hierarchy |
| Procurement | RFQ → PO → receipt → 3-way matching → invoice |
| Inventory | Multi-warehouse, FIFO costing, lot tracking, reservations |
| Field Operations | Work orders, daily site reports, progress measurements |
| Finance | Payables, receivables, payments, collections, multi-currency |
| Contracts & Progress Payments | Contract lines, hakedis (progress billing), deductions |
| Workflow Engine | Sequential, Parallel, Quorum approval flows |
| HR & Org | Employees, positions, timesheets, leave, expenses |
| Assets | Equipment lifecycle, assignments, maintenance |
| Documents | Versioned document archive with entity linking |
| Notifications | In-app, email, SMS with per-user preferences |
| Chat | 1:1 and group messaging linked to business entities |
| Reporting | Configurable dashboards and report definitions |

## 1.2 Scale Targets

| Metric | Target |
|--------|--------|
| Concurrent users | 500 |
| Daily API requests | 2,000,000 |
| Database tables | 134 |
| Entity relationships | 539+ |
| Uptime SLA | 99.9% |
| API P99 latency | < 500ms |
| Background job throughput | 10,000 jobs/hour |
| Data retention | 10 years (audit logs: permanent) |

## 1.3 Cross-Cutting Design Principles

### Soft Delete
Every table carries `is_deleted BOOLEAN DEFAULT false`, `deleted_at TIMESTAMPTZ`, `deleted_by UUID`. No record is ever physically destroyed.

### Audit Trail
Every table carries `created_at TIMESTAMPTZ NOT NULL DEFAULT now()`, `created_by UUID`, `updated_at TIMESTAMPTZ`, `updated_by UUID`. All writes go through the audit interceptor.

### Immutable Ledger Tables
`stock_transactions` and `audit_logs` are append-only. Corrections use reversal entries, never updates or deletes.

### Document Numbering
`sequence_definitions` drives auto-numbering for every document type (PO-2026-00042 pattern) with atomic counter increment inside a transaction.

### Module Independence
Inter-module references use either explicit FK (when mandatory) or the generic `(related_entity_type, related_entity_id)` polymorphic pattern (when optional/cross-cutting).

### Multi-Tenancy
The system supports multiple companies (`company_id` on most tables) and branches (`branch_id`). Row-level filtering is enforced by middleware.

---

# 2. Architecture Design

## 2.1 High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          CLIENT LAYER                                    │
│   Web SPA (React)          Mobile App (React Native)     API Clients     │
└──────────────────────────────┬──────────────────────────────────────────┘
                               │ HTTPS / WSS
┌──────────────────────────────▼──────────────────────────────────────────┐
│                         API GATEWAY / REVERSE PROXY                      │
│         (nginx / Kong)  —  TLS termination, rate limiting, routing       │
└──────────┬─────────────────────────────────────────────┬────────────────┘
           │ REST + WebSocket                             │ Auth
┌──────────▼────────────────┐              ┌─────────────▼──────────────┐
│    Application Server      │              │     Auth Service            │
│    (Node.js / Express 5)   │              │     (JWT + Refresh Token)   │
│    Modular Monolith        │              └────────────────────────────┘
│                            │
│  ┌──────────────────────┐  │   ┌─────────────────────────────────────┐
│  │  REST API Handlers   │  │   │         Message Queue               │
│  │  WebSocket Handler   │──┼──►│    (Redis Streams / BullMQ)         │
│  │  Workflow Engine     │  │   │                                     │
│  │  Notification Engine │  │   │  Queues:                            │
│  └──────────────────────┘  │   │  • approval-engine                  │
│                            │   │  • notifications                    │
└─────────────┬──────────────┘   │  • stock-recalc                     │
              │                  │  • sequence-generation              │
    ┌─────────▼──────────┐       │  • email-dispatch                   │
    │   PostgreSQL 16     │       │  • report-generation                │
    │   Primary DB        │       └──────────────┬──────────────────────┘
    │   (RDS / Supabase)  │                      │
    └─────────┬──────────┘              ┌────────▼──────────────────────┐
              │ Replication             │      Worker Processes          │
    ┌─────────▼──────────┐             │      (BullMQ Workers)          │
    │   PostgreSQL        │             └───────────────────────────────┘
    │   Read Replica      │
    └────────────────────┘       ┌──────────────────────────────────────┐
                                 │          Redis Cache                  │
    ┌────────────────────┐       │  • Session store                     │
    │   Object Storage    │       │  • Permission cache (TTL 5min)       │
    │   (S3 / R2)         │       │  • Stock balance cache               │
    │   Document files    │       │  • Exchange rate cache (TTL 1hr)     │
    └────────────────────┘       └──────────────────────────────────────┘
```

## 2.2 Technology Stack

| Layer | Technology | Rationale |
|-------|-----------|-----------|
| Runtime | Node.js 24 LTS | Async I/O, TypeScript-first |
| Framework | Express 5 | Stable, well-understood, async middleware |
| Language | TypeScript 5.9 | Type safety across the full stack |
| Database | PostgreSQL 16 | ACID, JSONB, advanced indexing, partitioning |
| ORM | Drizzle ORM | Type-safe queries, migration-first |
| Validation | Zod v4 | Runtime schema validation on all I/O |
| Cache / Queue | Redis 7 + BullMQ | Low-latency cache + reliable job queues |
| Object Storage | S3-compatible (R2 or AWS S3) | Versioned document storage |
| Auth | JWT (access 15min) + Refresh Token (7d, HttpOnly cookie) | Stateless API, secure refresh |
| API Schema | OpenAPI 3.1 | Contract-first, codegen to client |
| WebSockets | Socket.IO | Real-time notifications and chat |
| Email | Resend / SendGrid | Transactional email |
| SMS | Twilio / Netgsm | SMS notifications |
| Observability | OpenTelemetry → Grafana/Loki/Tempo | Traces, metrics, logs |
| CI/CD | GitHub Actions | Build, test, lint, deploy |
| Container | Docker + Docker Compose (dev) | Reproducible environments |
| Orchestration | Kubernetes (prod) / Railway/Render (staging) | Scalable deployment |

## 2.3 Modular Monolith Structure

The system is structured as a **modular monolith** — a single deployable unit with hard module boundaries. This provides simplicity of deployment while keeping modules independently testable and eventually extractable as microservices.

```
src/
├── core/          # Shared infrastructure (DB, cache, queue, logger, errors)
├── modules/
│   ├── core/          # Companies, Branches, Currencies, Units, Sequences, Settings
│   ├── iam/           # Users, Roles, Permissions, Menus
│   ├── organization/  # Employees, Departments, Positions, Leave, Expenses
│   ├── hr/            # Timesheets
│   ├── business-partners/  # Customers, Suppliers, Subcontractors
│   ├── projects/      # Projects, Phases, Members, Locations
│   ├── catalog/       # Materials, Categories, Attributes, Brands
│   ├── inventory/     # Warehouses, StockDocuments, Lots, Balances
│   ├── requests/      # Material/service requests
│   ├── procurement/   # Quotes, POs, Receipts, Invoices
│   ├── operations/    # Work Orders, Assignments, Checklists
│   ├── field-operations/  # Site Reports, Progress, Measurement Sheets
│   ├── assets/        # Equipment, Assignments, Maintenance
│   ├── finance/       # Payables, Receivables, Payments, Collections
│   ├── budget/        # Budgets, Budget Lines
│   ├── contracts/     # Contracts, Lines, Amendments
│   ├── progress-payments/ # Hakediş
│   ├── documents/     # Document archive
│   ├── workflow/      # Approval engine
│   ├── notifications/ # Notification dispatch
│   ├── chat/          # Messaging
│   └── reporting/     # Reports and dashboards
├── shared/        # Cross-module DTOs, enums, utils
└── app.ts         # Express wiring + middleware
```

### Module Internal Structure (each module)

```
modules/<name>/
├── <name>.router.ts      # Express routes
├── <name>.controller.ts  # Request/response handling
├── <name>.service.ts     # Business logic
├── <name>.repository.ts  # DB access (Drizzle)
├── <name>.schema.ts      # Drizzle table definition
├── <name>.zod.ts         # Zod validation schemas
├── <name>.events.ts      # Events emitted by this module
└── <name>.types.ts       # TypeScript types/interfaces
```

---

# 3. Module Breakdown

| Module | Tables | Depends On | Key Responsibility |
|--------|--------|-----------|-------------------|
| Core | 11 | — | Companies, currencies, units, sequences, settings, audit |
| IAM | 9 | Core | Auth, users, roles, permissions, menus |
| Organization | 7 | IAM | Employees, positions, competencies, leave, expenses |
| HR | 2 | Organization, Projects | Timesheets |
| BusinessPartners | 4 | Core | Customer/supplier/subcontractor master data |
| Projects | 7 | Core, BusinessPartners | Project lifecycle, WBS, team |
| Catalog | 8 | Core | Material master, dynamic attributes |
| Inventory | 14 | Core, Catalog, Projects | Stock movements, lots, FIFO, reservations |
| Requests | 3 | Projects, Inventory | Material/service request workflow |
| Procurement | 8 | Requests, Inventory, BP | RFQ → PO → receipt → 3-way match |
| Operations | 8 | Projects, Inventory | Work orders, assignments, materials |
| FieldOperations | 7 | Projects, Operations | Site reports, progress, measurement |
| Assets | 3 | Core, Projects | Equipment lifecycle |
| Finance | 10 | BusinessPartners, Projects | Payables, receivables, payments |
| Budget | 2 | Projects, Finance | Budget planning and variance |
| Contracts | 4 | BusinessPartners, Projects | Contract lifecycle |
| ProgressPayments | 3 | Contracts, Projects | Progress billing (hakediş) |
| Documents | 5 | All | Versioned file archive |
| Workflow | 10 | IAM | Dynamic multi-step approval engine |
| Notifications | 3 | IAM | In-app, email, SMS dispatch |
| Chat | 4 | IAM | 1:1 and group messaging |
| Reporting | 2 | All | Report and dashboard definitions |

---

# 4. Database Design — Full Schema

## 4.1 Schema Conventions

- **Primary keys:** `UUID` (gen_random_uuid()), named `id`
- **Timestamps:** `TIMESTAMPTZ NOT NULL DEFAULT now()`
- **Soft delete:** `is_deleted BOOLEAN NOT NULL DEFAULT false`, `deleted_at TIMESTAMPTZ`, `deleted_by UUID`
- **Audit fields:** `created_at`, `created_by`, `updated_at`, `updated_by` on every table
- **Naming:** snake_case for columns/tables, PascalCase entity names in code
- **Indexes:** All FKs indexed, all status/is_deleted combinations indexed
- **Constraints:** Unique constraints explicitly named

---

## 4.2 Core Module

### `companies`
```sql
CREATE TABLE companies (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name            VARCHAR(255) NOT NULL,
  tax_number      VARCHAR(50),
  tax_office      VARCHAR(100),
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

### `branches`
```sql
CREATE TABLE branches (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id      UUID NOT NULL REFERENCES companies(id),
  name            VARCHAR(255) NOT NULL,
  code            VARCHAR(50) NOT NULL UNIQUE,
  address         TEXT,
  phone           VARCHAR(30),
  is_active       BOOLEAN NOT NULL DEFAULT true,
  -- (audit fields)
  CONSTRAINT uq_branches_code UNIQUE (code)
);
CREATE INDEX idx_branches_company_id ON branches(company_id);
```

### `departments`
```sql
CREATE TABLE departments (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id            UUID NOT NULL REFERENCES companies(id),
  parent_department_id  UUID REFERENCES departments(id),
  name                  VARCHAR(255) NOT NULL,
  code                  VARCHAR(50) NOT NULL,
  manager_id            UUID, -- FK to employees(id), added after employees table
  is_active             BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete fields)
);
CREATE INDEX idx_departments_company ON departments(company_id);
CREATE INDEX idx_departments_parent ON departments(parent_department_id);
```

### `currencies`
```sql
CREATE TABLE currencies (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code             VARCHAR(3) NOT NULL UNIQUE,   -- ISO 4217
  name             VARCHAR(100) NOT NULL,
  symbol           VARCHAR(5),
  is_base_currency BOOLEAN NOT NULL DEFAULT false,
  is_active        BOOLEAN NOT NULL DEFAULT true,
  -- (audit + soft delete fields)
  CONSTRAINT uq_currencies_code UNIQUE (code),
  CONSTRAINT chk_one_base_currency CHECK (
    (is_base_currency = false) OR
    (SELECT COUNT(*) FROM currencies WHERE is_base_currency = true) <= 1
  )
);
```

### `exchange_rates`
```sql
CREATE TABLE exchange_rates (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  from_currency_id UUID NOT NULL REFERENCES currencies(id),
  to_currency_id   UUID NOT NULL REFERENCES currencies(id),
  rate             NUMERIC(18,6) NOT NULL CHECK (rate > 0),
  rate_date        DATE NOT NULL,
  source           VARCHAR(100),  -- 'TCMB', 'Manual', etc.
  -- (audit + soft delete fields)
  CONSTRAINT uq_exchange_rates_pair_date UNIQUE (from_currency_id, to_currency_id, rate_date)
);
CREATE INDEX idx_exchange_rates_date ON exchange_rates(rate_date DESC);
```

### `units_of_measure`
```sql
CREATE TABLE units_of_measure (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code        VARCHAR(50) NOT NULL UNIQUE,
  name        VARCHAR(100) NOT NULL,
  unit_type   VARCHAR(50),   -- 'Length', 'Weight', 'Volume', 'Count', etc.
  is_active   BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete fields)
);
```

### `unit_conversions`
```sql
CREATE TABLE unit_conversions (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  from_unit_id  UUID NOT NULL REFERENCES units_of_measure(id),
  to_unit_id    UUID NOT NULL REFERENCES units_of_measure(id),
  factor        NUMERIC(18,6) NOT NULL CHECK (factor > 0),
  -- (audit + soft delete fields)
  CONSTRAINT uq_unit_conversions UNIQUE (from_unit_id, to_unit_id)
);
```

### `sequence_definitions`
```sql
CREATE TABLE sequence_definitions (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  entity_type      VARCHAR(100) NOT NULL UNIQUE,  -- 'PurchaseOrder', 'Request', etc.
  prefix           VARCHAR(20),
  suffix           VARCHAR(20),
  pattern          VARCHAR(100) NOT NULL,   -- '{PREFIX}-{YEAR}-{SEQ:5}'
  current_value    INTEGER NOT NULL DEFAULT 0,
  reset_period     VARCHAR(20),             -- 'Yearly', 'Monthly', 'Never'
  last_reset_date  TIMESTAMPTZ,
  -- (audit + soft delete fields)
  CONSTRAINT uq_sequence_definitions_entity UNIQUE (entity_type)
);
```

### `system_settings`
```sql
CREATE TABLE system_settings (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  key         VARCHAR(200) NOT NULL UNIQUE,
  value       TEXT NOT NULL,
  value_type  VARCHAR(20) DEFAULT 'string',  -- 'string','int','bool','json'
  description TEXT,
  is_public   BOOLEAN NOT NULL DEFAULT false
  -- (audit + soft delete fields)
);
```

### `localization_resources`
```sql
CREATE TABLE localization_resources (
  id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  key            VARCHAR(500) NOT NULL,
  language_code  VARCHAR(5) NOT NULL,   -- 'tr', 'en', 'de'
  value          TEXT NOT NULL,
  module         VARCHAR(100),
  -- (audit + soft delete fields)
  CONSTRAINT uq_localization_key_lang UNIQUE (key, language_code)
);
CREATE INDEX idx_localization_lang ON localization_resources(language_code);
```

### `audit_logs`
```sql
CREATE TABLE audit_logs (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id         UUID,               -- nullable: system actions have no user
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
  -- NO soft delete, NO updated_at — immutable append-only
);
CREATE INDEX idx_audit_logs_entity ON audit_logs(entity_type, entity_id);
CREATE INDEX idx_audit_logs_user ON audit_logs(user_id);
CREATE INDEX idx_audit_logs_created ON audit_logs(created_at DESC);
-- Partition by month for performance:
-- PARTITION BY RANGE (created_at)
```

---

## 4.3 IAM Module

### `users`
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
  employee_id          UUID,  -- FK to employees(id), nullable
  -- (audit + soft delete fields)
  CONSTRAINT uq_users_username UNIQUE (username),
  CONSTRAINT uq_users_email UNIQUE (email)
);
CREATE INDEX idx_users_email ON users(email) WHERE is_deleted = false;
CREATE INDEX idx_users_employee ON users(employee_id);
```

### `refresh_tokens`
```sql
CREATE TABLE refresh_tokens (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  token_hash  VARCHAR(255) NOT NULL UNIQUE,
  expires_at  TIMESTAMPTZ NOT NULL,
  revoked_at  TIMESTAMPTZ,
  ip_address  VARCHAR(45),
  user_agent  TEXT,
  created_at  TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX idx_refresh_tokens_user ON refresh_tokens(user_id);
CREATE INDEX idx_refresh_tokens_hash ON refresh_tokens(token_hash);
```

### `roles`
```sql
CREATE TABLE roles (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name            VARCHAR(100) NOT NULL UNIQUE,
  description     TEXT,
  is_system_role  BOOLEAN NOT NULL DEFAULT false,
  is_active       BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete fields)
);
-- Seed: Admin, ProjectManager, WarehouseManager, PurchaseManager,
--       FinanceManager, HRManager, SiteSupervisor
```

### `permissions`
```sql
CREATE TABLE permissions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code        VARCHAR(200) NOT NULL UNIQUE,  -- 'Inventory.StockDocument.Create'
  name        VARCHAR(200) NOT NULL,
  module      VARCHAR(100),
  description TEXT
  -- (audit + soft delete fields)
);
```

### `user_roles`
```sql
CREATE TABLE user_roles (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  role_id     UUID NOT NULL REFERENCES roles(id),
  valid_from  TIMESTAMPTZ,
  valid_to    TIMESTAMPTZ,
  -- (audit fields — no soft delete, use valid_to for expiry)
  CONSTRAINT uq_user_roles UNIQUE (user_id, role_id)
);
CREATE INDEX idx_user_roles_user ON user_roles(user_id);
```

### `role_permissions`
```sql
CREATE TABLE role_permissions (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  role_id         UUID NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
  permission_code VARCHAR(200) NOT NULL REFERENCES permissions(code),
  -- (audit fields)
  CONSTRAINT uq_role_permissions UNIQUE (role_id, permission_code)
);
CREATE INDEX idx_role_permissions_role ON role_permissions(role_id);
```

### `user_permissions`
```sql
CREATE TABLE user_permissions (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  permission_code VARCHAR(200) NOT NULL REFERENCES permissions(code),
  is_granted      BOOLEAN NOT NULL,   -- true=grant, false=deny override
  reason          TEXT,
  valid_from      TIMESTAMPTZ,
  valid_to        TIMESTAMPTZ,
  -- (audit fields)
  CONSTRAINT uq_user_permissions UNIQUE (user_id, permission_code)
);
```

### `menus`
```sql
CREATE TABLE menus (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  parent_id         UUID REFERENCES menus(id),
  title_key         VARCHAR(200) NOT NULL,   -- localization key
  icon              VARCHAR(100),
  route             VARCHAR(500),
  permission_code   VARCHAR(200),
  sort_order        INTEGER NOT NULL DEFAULT 0,
  is_active         BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete fields)
);
```

### `user_settings`
```sql
CREATE TABLE user_settings (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  key         VARCHAR(200) NOT NULL,
  value       TEXT NOT NULL,
  -- (audit fields)
  CONSTRAINT uq_user_settings UNIQUE (user_id, key)
);
```

---

## 4.4 Organization Module

### `positions`
```sql
CREATE TABLE positions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id  UUID NOT NULL REFERENCES companies(id),
  name        VARCHAR(200) NOT NULL,
  code        VARCHAR(50),
  level       INTEGER,
  is_active   BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

### `employees`
```sql
CREATE TABLE employees (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id        UUID NOT NULL REFERENCES companies(id),
  branch_id         UUID REFERENCES branches(id),
  department_id     UUID REFERENCES departments(id),
  position_id       UUID REFERENCES positions(id),
  employee_number   VARCHAR(50) NOT NULL,
  first_name        VARCHAR(100) NOT NULL,
  last_name         VARCHAR(100) NOT NULL,
  email             VARCHAR(255),
  phone             VARCHAR(30),
  hire_date         DATE NOT NULL,
  termination_date  DATE,
  employment_type   VARCHAR(50) NOT NULL,  -- 'FullTime','PartTime','Contract','Intern'
  is_active         BOOLEAN NOT NULL DEFAULT true,
  -- (audit + soft delete)
  CONSTRAINT uq_employees_number UNIQUE (company_id, employee_number)
);
CREATE INDEX idx_employees_company ON employees(company_id);
CREATE INDEX idx_employees_department ON employees(department_id);
```

### `employee_skills`
```sql
CREATE TABLE employee_skills (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name        VARCHAR(200) NOT NULL,
  category    VARCHAR(100)
  -- (audit + soft delete)
);
```

### `employee_skill_assignments`
```sql
CREATE TABLE employee_skill_assignments (
  id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  employee_id    UUID NOT NULL REFERENCES employees(id),
  skill_id       UUID NOT NULL REFERENCES employee_skills(id),
  proficiency    VARCHAR(50),  -- 'Beginner','Intermediate','Advanced','Expert'
  certified_at   DATE,
  expires_at     DATE
  -- (audit fields)
);
```

### `leave_requests`
```sql
CREATE TABLE leave_requests (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  employee_id         UUID NOT NULL REFERENCES employees(id),
  leave_type          VARCHAR(50) NOT NULL,   -- 'Annual','Sick','Unpaid','Compassionate'
  start_date          DATE NOT NULL,
  end_date            DATE NOT NULL,
  total_days          NUMERIC(5,1) NOT NULL,
  reason              TEXT,
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→PendingApproval→Approved→Rejected→Cancelled
  approval_request_id UUID,  -- FK to approval_requests
  -- (audit + soft delete)
);
```

### `expense_claims`
```sql
CREATE TABLE expense_claims (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  employee_id         UUID NOT NULL REFERENCES employees(id),
  project_id          UUID,
  claim_date          DATE NOT NULL,
  claim_number        VARCHAR(100) NOT NULL,
  total_amount        NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft',
  approval_request_id UUID
  -- (audit + soft delete)
);
```

### `expense_claim_lines`
```sql
CREATE TABLE expense_claim_lines (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  expense_claim_id UUID NOT NULL REFERENCES expense_claims(id),
  expense_date     DATE NOT NULL,
  category         VARCHAR(100) NOT NULL,   -- 'Travel','Accommodation','Meal','Other'
  description      TEXT,
  amount           NUMERIC(18,2) NOT NULL,
  currency_id      UUID NOT NULL REFERENCES currencies(id),
  receipt_url      TEXT
  -- (audit fields)
);
```

---

## 4.5 HR Module

### `timesheet_headers`
```sql
CREATE TABLE timesheet_headers (
  id           UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  employee_id  UUID NOT NULL REFERENCES employees(id),
  project_id   UUID REFERENCES projects(id),
  period_start DATE NOT NULL,
  period_end   DATE NOT NULL,
  status       VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Submitted→Approved→Rejected
  total_hours  NUMERIC(8,2),
  -- (audit + soft delete)
  CONSTRAINT uq_timesheet_period UNIQUE (employee_id, period_start, period_end)
);
```

### `timesheet_lines`
```sql
CREATE TABLE timesheet_lines (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  timesheet_header_id UUID NOT NULL REFERENCES timesheet_headers(id),
  work_date           DATE NOT NULL,
  work_order_id       UUID,
  phase_id            UUID,
  regular_hours       NUMERIC(5,2) NOT NULL DEFAULT 0,
  overtime_hours      NUMERIC(5,2) NOT NULL DEFAULT 0,
  description         TEXT
  -- (audit fields)
);
```

---

## 4.6 BusinessPartners Module

### `business_partners`
```sql
CREATE TABLE business_partners (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  partner_type        VARCHAR(50) NOT NULL,  -- 'Customer','Supplier','Subcontractor','Both'
  name                VARCHAR(255) NOT NULL,
  short_name          VARCHAR(100),
  tax_number          VARCHAR(50),
  tax_office          VARCHAR(100),
  website             VARCHAR(255),
  is_active           BOOLEAN NOT NULL DEFAULT true,
  default_currency_id UUID REFERENCES currencies(id),
  payment_terms_days  INTEGER DEFAULT 30,
  credit_limit        NUMERIC(18,2),
  -- (audit + soft delete)
);
CREATE INDEX idx_bp_company ON business_partners(company_id);
CREATE INDEX idx_bp_type ON business_partners(partner_type);
```

### `business_partner_contacts`
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
  -- (audit + soft delete)
);
```

### `business_partner_addresses`
```sql
CREATE TABLE business_partner_addresses (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  address_type        VARCHAR(50),  -- 'Billing','Delivery','Legal'
  street              TEXT,
  city                VARCHAR(100),
  state               VARCHAR(100),
  postal_code         VARCHAR(20),
  country             VARCHAR(100),
  is_default          BOOLEAN NOT NULL DEFAULT false
  -- (audit + soft delete)
);
```

### `business_partner_bank_accounts`
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
  -- (audit + soft delete)
);
```

---

## 4.7 Projects Module

### `project_types`
```sql
CREATE TABLE project_types (
  id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name      VARCHAR(100) NOT NULL,
  code      VARCHAR(50) NOT NULL UNIQUE,
  is_active BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
-- Seed: EPC, Maintenance, Investment, Consulting, Turnkey
```

### `projects`
```sql
CREATE TABLE projects (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  project_number      VARCHAR(100) NOT NULL UNIQUE,
  name                VARCHAR(255) NOT NULL,
  type_id             UUID NOT NULL REFERENCES project_types(id),
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Active→OnHold→Completed→Closed→Cancelled
  customer_id         UUID REFERENCES business_partners(id),
  contract_id         UUID,  -- FK to contracts(id), added after contracts table
  start_date          DATE,
  end_date            DATE,
  actual_start_date   DATE,
  actual_end_date     DATE,
  budget_amount       NUMERIC(18,2),
  currency_id         UUID REFERENCES currencies(id),
  description         TEXT,
  branch_id           UUID REFERENCES branches(id),
  -- (audit + soft delete)
  CONSTRAINT uq_projects_number UNIQUE (project_number)
);
CREATE INDEX idx_projects_company ON projects(company_id);
CREATE INDEX idx_projects_status ON projects(status) WHERE is_deleted = false;
CREATE INDEX idx_projects_customer ON projects(customer_id);
```

### `project_phases`
```sql
CREATE TABLE project_phases (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id        UUID NOT NULL REFERENCES projects(id),
  parent_phase_id   UUID REFERENCES project_phases(id),
  name              VARCHAR(255) NOT NULL,
  code              VARCHAR(50),
  planned_start     DATE,
  planned_end       DATE,
  planned_quantity  NUMERIC(18,3),
  unit_id           UUID REFERENCES units_of_measure(id),
  unit_price        NUMERIC(18,4),
  sort_order        INTEGER NOT NULL DEFAULT 0,
  -- (audit + soft delete)
);
CREATE INDEX idx_project_phases_project ON project_phases(project_id);
```

### `project_locations`
```sql
CREATE TABLE project_locations (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id  UUID NOT NULL REFERENCES projects(id),
  parent_id   UUID REFERENCES project_locations(id),
  name        VARCHAR(255) NOT NULL,
  code        VARCHAR(50),
  latitude    NUMERIC(10,7),
  longitude   NUMERIC(10,7)
  -- (audit + soft delete)
);
```

### `project_members`
```sql
CREATE TABLE project_members (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id            UUID NOT NULL REFERENCES projects(id),
  employee_id           UUID REFERENCES employees(id),
  user_id               UUID REFERENCES users(id),
  project_role          VARCHAR(100) NOT NULL,
  -- 'ProjectManager','SiteSupervisor','Engineer','Foreman','SafetyOfficer'
  start_date            DATE,
  end_date              DATE,
  allocation_percentage NUMERIC(5,2) CHECK (allocation_percentage BETWEEN 0 AND 100)
  -- (audit + soft delete)
);
CREATE INDEX idx_project_members_project ON project_members(project_id);
```

### `project_notes`
```sql
CREATE TABLE project_notes (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id  UUID NOT NULL REFERENCES projects(id),
  note_date   TIMESTAMPTZ NOT NULL DEFAULT now(),
  content     TEXT NOT NULL,
  is_private  BOOLEAN NOT NULL DEFAULT false
  -- (audit + soft delete)
);
```

---

## 4.8 Catalog Module

### `brands`
```sql
CREATE TABLE brands (
  id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name      VARCHAR(200) NOT NULL UNIQUE,
  country   VARCHAR(100),
  is_active BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

### `material_categories`
```sql
CREATE TABLE material_categories (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  parent_id   UUID REFERENCES material_categories(id),
  name        VARCHAR(200) NOT NULL,
  code        VARCHAR(50) NOT NULL UNIQUE,
  description TEXT,
  is_active   BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

### `material_attribute_definitions`
```sql
CREATE TABLE material_attribute_definitions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name        VARCHAR(200) NOT NULL,
  data_type   VARCHAR(50) NOT NULL,  -- 'Text','Number','Boolean','Select'
  is_required BOOLEAN NOT NULL DEFAULT false,
  unit        VARCHAR(50)
  -- (audit + soft delete)
);
```

### `material_attribute_options`
```sql
CREATE TABLE material_attribute_options (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  definition_id   UUID NOT NULL REFERENCES material_attribute_definitions(id),
  value           VARCHAR(200) NOT NULL,
  sort_order      INTEGER NOT NULL DEFAULT 0
  -- (audit + soft delete)
);
```

### `material_category_attributes`
```sql
CREATE TABLE material_category_attributes (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  category_id     UUID NOT NULL REFERENCES material_categories(id),
  definition_id   UUID NOT NULL REFERENCES material_attribute_definitions(id),
  is_required     BOOLEAN NOT NULL DEFAULT false,
  sort_order      INTEGER NOT NULL DEFAULT 0,
  CONSTRAINT uq_mca UNIQUE (category_id, definition_id)
);
```

### `materials`
```sql
CREATE TABLE materials (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code              VARCHAR(100) NOT NULL UNIQUE,
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
  is_stockable      BOOLEAN NOT NULL DEFAULT true,
  -- (audit + soft delete)
  CONSTRAINT uq_materials_code UNIQUE (code)
);
CREATE INDEX idx_materials_category ON materials(category_id);
CREATE INDEX idx_materials_code ON materials(code);
```

### `material_attribute_values`
```sql
CREATE TABLE material_attribute_values (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  material_id     UUID NOT NULL REFERENCES materials(id) ON DELETE CASCADE,
  definition_id   UUID NOT NULL REFERENCES material_attribute_definitions(id),
  value           TEXT NOT NULL,
  CONSTRAINT uq_mav UNIQUE (material_id, definition_id)
);
```

### `material_unit_conversions`
```sql
CREATE TABLE material_unit_conversions (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  material_id   UUID NOT NULL REFERENCES materials(id),
  from_unit_id  UUID NOT NULL REFERENCES units_of_measure(id),
  to_unit_id    UUID NOT NULL REFERENCES units_of_measure(id),
  factor        NUMERIC(18,6) NOT NULL CHECK (factor > 0),
  CONSTRAINT uq_muc UNIQUE (material_id, from_unit_id, to_unit_id)
);
```

---

## 4.9 Inventory Module

### `warehouses`
```sql
CREATE TABLE warehouses (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id      UUID NOT NULL REFERENCES companies(id),
  code            VARCHAR(50) NOT NULL UNIQUE,
  name            VARCHAR(200) NOT NULL,
  warehouse_type  VARCHAR(50) NOT NULL,
  -- 'Central','ProjectSite','Temporary','Vehicle','Consignment'
  project_id      UUID REFERENCES projects(id),
  branch_id       UUID REFERENCES branches(id),
  address         TEXT,
  is_active       BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

### `warehouse_locations`
```sql
CREATE TABLE warehouse_locations (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  warehouse_id  UUID NOT NULL REFERENCES warehouses(id),
  parent_id     UUID REFERENCES warehouse_locations(id),
  name          VARCHAR(100) NOT NULL,
  code          VARCHAR(50) NOT NULL,
  is_active     BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

### `stock_document_types`
```sql
CREATE TABLE stock_document_types (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code              VARCHAR(50) NOT NULL UNIQUE,
  name              VARCHAR(200) NOT NULL,
  direction         VARCHAR(20) NOT NULL,  -- 'In','Out','Transfer'
  requires_approval BOOLEAN NOT NULL DEFAULT false,
  affects_stock     BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
-- Seed: PurchaseReceipt/In, ProjectIssue/Out, CountSurplus/In,
--       CountDeficit/Out, TransferIn/In, TransferOut/Out, Loss/Out
```

### `stock_documents`
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
  status                VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→PendingApproval→Approved→Posted→Cancelled→Rejected
  related_document_id   UUID,
  approval_request_id   UUID,
  description           TEXT
  -- (audit + soft delete)
);
CREATE INDEX idx_stock_docs_warehouse ON stock_documents(warehouse_id);
CREATE INDEX idx_stock_docs_project ON stock_documents(project_id);
CREATE INDEX idx_stock_docs_status ON stock_documents(status);
```

### `stock_document_lines`
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
  lot_id              UUID  -- FK to stock_lots(id)
  -- (audit fields)
);
```

### `stock_lots`
```sql
CREATE TABLE stock_lots (
  id                        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  material_id               UUID NOT NULL REFERENCES materials(id),
  warehouse_id              UUID NOT NULL REFERENCES warehouses(id),
  receipt_document_line_id  UUID NOT NULL REFERENCES stock_document_lines(id),
  lot_number                VARCHAR(100),
  received_quantity         NUMERIC(18,4) NOT NULL,
  remaining_quantity        NUMERIC(18,4) NOT NULL,
  unit_cost                 NUMERIC(18,4) NOT NULL,
  receipt_date              DATE NOT NULL,
  expiry_date               DATE
  -- (audit fields)
);
CREATE INDEX idx_stock_lots_material_wh ON stock_lots(material_id, warehouse_id);
CREATE INDEX idx_stock_lots_receipt_date ON stock_lots(receipt_date) WHERE remaining_quantity > 0;
```

### `stock_issue_allocations`
```sql
CREATE TABLE stock_issue_allocations (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  issue_document_line_id UUID NOT NULL REFERENCES stock_document_lines(id),
  stock_lot_id           UUID NOT NULL REFERENCES stock_lots(id),
  allocated_quantity     NUMERIC(18,4) NOT NULL,
  unit_cost              NUMERIC(18,4) NOT NULL,
  total_cost             NUMERIC(18,2) NOT NULL
  -- (audit fields)
);
```

### `stock_transactions`
```sql
CREATE TABLE stock_transactions (
  id                   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  document_line_id     UUID NOT NULL REFERENCES stock_document_lines(id),
  material_id          UUID NOT NULL REFERENCES materials(id),
  warehouse_id         UUID NOT NULL REFERENCES warehouses(id),
  transaction_date     TIMESTAMPTZ NOT NULL,
  direction            VARCHAR(5) NOT NULL CHECK (direction IN ('In','Out')),
  quantity             NUMERIC(18,4) NOT NULL,
  unit_cost            NUMERIC(18,4),
  total_cost           NUMERIC(18,2),
  created_at           TIMESTAMPTZ NOT NULL DEFAULT now(),
  created_by           UUID
  -- NO updated_at, NO is_deleted — immutable ledger
);
CREATE INDEX idx_stock_txn_material_wh ON stock_transactions(material_id, warehouse_id);
CREATE INDEX idx_stock_txn_date ON stock_transactions(transaction_date DESC);
```

### `stock_balances`
```sql
CREATE TABLE stock_balances (
  material_id         UUID NOT NULL REFERENCES materials(id),
  warehouse_id        UUID NOT NULL REFERENCES warehouses(id),
  on_hand_quantity    NUMERIC(18,4) NOT NULL DEFAULT 0,
  reserved_quantity   NUMERIC(18,4) NOT NULL DEFAULT 0,
  available_quantity  NUMERIC(18,4) GENERATED ALWAYS AS (on_hand_quantity - reserved_quantity) STORED,
  average_cost        NUMERIC(18,4),
  last_updated_at     TIMESTAMPTZ NOT NULL DEFAULT now(),
  PRIMARY KEY (material_id, warehouse_id)
);
```

### `stock_reservations`
```sql
CREATE TABLE stock_reservations (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  material_id         UUID NOT NULL REFERENCES materials(id),
  warehouse_id        UUID NOT NULL REFERENCES warehouses(id),
  reserved_quantity   NUMERIC(18,4) NOT NULL,
  related_entity_type VARCHAR(100) NOT NULL,
  related_entity_id   UUID NOT NULL,
  expiry_date         TIMESTAMPTZ,
  status              VARCHAR(50) NOT NULL DEFAULT 'Active',
  -- Active→Consumed→Expired→Cancelled
  -- (audit fields)
);
CREATE INDEX idx_stock_res_material ON stock_reservations(material_id, warehouse_id) WHERE status = 'Active';
```

### `stock_counts`
```sql
CREATE TABLE stock_counts (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  warehouse_id  UUID NOT NULL REFERENCES warehouses(id),
  count_date    DATE NOT NULL,
  status        VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→InProgress→Completed→Closed
  responsible_id UUID REFERENCES employees(id),
  notes         TEXT
  -- (audit + soft delete)
);
```

### `stock_count_lines`
```sql
CREATE TABLE stock_count_lines (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  stock_count_id        UUID NOT NULL REFERENCES stock_counts(id),
  material_id           UUID NOT NULL REFERENCES materials(id),
  location_id           UUID REFERENCES warehouse_locations(id),
  expected_quantity     NUMERIC(18,4) NOT NULL,
  counted_quantity      NUMERIC(18,4),
  difference            NUMERIC(18,4) GENERATED ALWAYS AS (counted_quantity - expected_quantity) STORED,
  adjustment_document_id UUID REFERENCES stock_documents(id)
);
```

### `warehouse_transfers`
```sql
CREATE TABLE warehouse_transfers (
  id                   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  transfer_number      VARCHAR(100) NOT NULL UNIQUE,
  from_warehouse_id    UUID NOT NULL REFERENCES warehouses(id),
  to_warehouse_id      UUID NOT NULL REFERENCES warehouses(id),
  transfer_date        DATE NOT NULL,
  status               VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→InTransit→Completed→Cancelled
  out_document_id      UUID REFERENCES stock_documents(id),
  in_document_id       UUID REFERENCES stock_documents(id)
  -- (audit + soft delete)
);
```

### `warehouse_transfer_lines`
```sql
CREATE TABLE warehouse_transfer_lines (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  warehouse_transfer_id UUID NOT NULL REFERENCES warehouse_transfers(id),
  material_id           UUID NOT NULL REFERENCES materials(id),
  quantity              NUMERIC(18,4) NOT NULL,
  unit_id               UUID NOT NULL REFERENCES units_of_measure(id)
);
```

---

## 4.10 Requests Module

### `request_types`
```sql
CREATE TABLE request_types (
  id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name      VARCHAR(100) NOT NULL,
  code      VARCHAR(50) NOT NULL UNIQUE,
  is_active BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
-- Seed: MaterialRequest, ServiceRequest, EquipmentRequest
```

### `requests`
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
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→PendingApproval→Approved→Ordered→Closed→Rejected→Cancelled
  priority            VARCHAR(20) DEFAULT 'Normal',
  -- Low,Normal,High,Urgent
  description         TEXT,
  approval_request_id UUID
  -- (audit + soft delete)
);
CREATE INDEX idx_requests_project ON requests(project_id);
CREATE INDEX idx_requests_status ON requests(status);
```

### `request_lines`
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
  -- (audit fields)
);
```

---

## 4.11 Procurement Module

### `supplier_quotes`
```sql
CREATE TABLE supplier_quotes (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  quote_number    VARCHAR(100) NOT NULL UNIQUE,
  request_id      UUID REFERENCES requests(id),
  supplier_id     UUID NOT NULL REFERENCES business_partners(id),
  quote_date      DATE NOT NULL,
  valid_until     DATE,
  currency_id     UUID NOT NULL REFERENCES currencies(id),
  total_amount    NUMERIC(18,2),
  status          VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Sent→Received→Evaluated→Accepted→Rejected
  notes           TEXT
  -- (audit + soft delete)
);
```

### `supplier_quote_lines`
```sql
CREATE TABLE supplier_quote_lines (
  id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  quote_id       UUID NOT NULL REFERENCES supplier_quotes(id),
  material_id    UUID NOT NULL REFERENCES materials(id),
  quantity       NUMERIC(18,4) NOT NULL,
  unit_id        UUID NOT NULL REFERENCES units_of_measure(id),
  unit_price     NUMERIC(18,4) NOT NULL,
  total_price    NUMERIC(18,2),
  delivery_days  INTEGER,
  notes          TEXT
);
```

### `purchase_orders`
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
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→PendingApproval→Approved→Rejected→PartiallyReceived→Received→Cancelled
  delivery_address    TEXT,
  payment_terms       TEXT,
  approval_request_id UUID,
  notes               TEXT
  -- (audit + soft delete)
);
CREATE INDEX idx_po_supplier ON purchase_orders(supplier_id);
CREATE INDEX idx_po_project ON purchase_orders(project_id);
CREATE INDEX idx_po_status ON purchase_orders(status);
```

### `purchase_order_lines`
```sql
CREATE TABLE purchase_order_lines (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  purchase_order_id UUID NOT NULL REFERENCES purchase_orders(id),
  material_id      UUID NOT NULL REFERENCES materials(id),
  quantity         NUMERIC(18,4) NOT NULL,
  unit_id          UUID NOT NULL REFERENCES units_of_measure(id),
  unit_price       NUMERIC(18,4) NOT NULL,
  vat_rate         NUMERIC(5,2) DEFAULT 18,
  total_price      NUMERIC(18,2),
  received_quantity NUMERIC(18,4) DEFAULT 0,
  request_line_id  UUID REFERENCES request_lines(id),
  description      TEXT
);
```

### `purchase_receipts`
```sql
CREATE TABLE purchase_receipts (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  receipt_number         VARCHAR(100) NOT NULL UNIQUE,
  purchase_order_id      UUID NOT NULL REFERENCES purchase_orders(id),
  supplier_id            UUID NOT NULL REFERENCES business_partners(id),
  receipt_date           DATE NOT NULL,
  warehouse_id           UUID NOT NULL REFERENCES warehouses(id),
  status                 VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Completed
  supplier_delivery_note VARCHAR(200),
  notes                  TEXT
  -- (audit + soft delete)
);
```

### `purchase_receipt_lines`
```sql
CREATE TABLE purchase_receipt_lines (
  id                   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  purchase_receipt_id  UUID NOT NULL REFERENCES purchase_receipts(id),
  purchase_order_line_id UUID NOT NULL REFERENCES purchase_order_lines(id),
  material_id          UUID NOT NULL REFERENCES materials(id),
  received_quantity    NUMERIC(18,4) NOT NULL,
  unit_id              UUID NOT NULL REFERENCES units_of_measure(id),
  unit_cost            NUMERIC(18,4) NOT NULL,
  stock_document_line_id UUID REFERENCES stock_document_lines(id)
);
```

### `supplier_invoices`
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
  status           VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Matched→ManualReview→Approved→Payable→Paid
  payable_id       UUID,
  notes            TEXT
  -- (audit + soft delete)
);
```

### `supplier_invoice_lines`
```sql
CREATE TABLE supplier_invoice_lines (
  id                       UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  supplier_invoice_id      UUID NOT NULL REFERENCES supplier_invoices(id),
  purchase_order_line_id   UUID REFERENCES purchase_order_lines(id),
  material_id              UUID REFERENCES materials(id),
  description              TEXT,
  quantity                 NUMERIC(18,4),
  unit_price               NUMERIC(18,4),
  vat_rate                 NUMERIC(5,2),
  total_price              NUMERIC(18,2)
);
```

---

## 4.12 Operations Module

### `work_order_types`
```sql
CREATE TABLE work_order_types (
  id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name      VARCHAR(100) NOT NULL,
  code      VARCHAR(50) NOT NULL UNIQUE,
  is_active BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
-- Seed: Installation, Maintenance, Repair, Testing, Inspection, Dismantling
```

### `work_orders`
```sql
CREATE TABLE work_orders (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id        UUID NOT NULL REFERENCES companies(id),
  order_number      VARCHAR(100) NOT NULL UNIQUE,
  type_id           UUID NOT NULL REFERENCES work_order_types(id),
  project_id        UUID NOT NULL REFERENCES projects(id),
  phase_id          UUID REFERENCES project_phases(id),
  status            VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Assigned→InProgress→OnHold→Completed→Closed→Cancelled
  priority          VARCHAR(20) NOT NULL DEFAULT 'Normal',
  -- Low,Normal,High,Critical
  planned_start     TIMESTAMPTZ,
  planned_end       TIMESTAMPTZ,
  actual_start      TIMESTAMPTZ,
  actual_end        TIMESTAMPTZ,
  description       TEXT,
  location_id       UUID REFERENCES project_locations(id),
  parent_wo_id      UUID REFERENCES work_orders(id)
  -- (audit + soft delete)
);
CREATE INDEX idx_wo_project ON work_orders(project_id);
CREATE INDEX idx_wo_status ON work_orders(status);
```

### `work_order_assignments`
```sql
CREATE TABLE work_order_assignments (
  id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  work_order_id  UUID NOT NULL REFERENCES work_orders(id),
  employee_id    UUID NOT NULL REFERENCES employees(id),
  role           VARCHAR(100),
  planned_hours  NUMERIC(8,2),
  actual_hours   NUMERIC(8,2)
  -- (audit + soft delete)
);
```

### `work_order_material_plans`
```sql
CREATE TABLE work_order_material_plans (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  work_order_id   UUID NOT NULL REFERENCES work_orders(id),
  material_id     UUID NOT NULL REFERENCES materials(id),
  planned_qty     NUMERIC(18,4) NOT NULL,
  unit_id         UUID NOT NULL REFERENCES units_of_measure(id)
  -- (audit + soft delete)
);
```

### `work_order_material_usages`
```sql
CREATE TABLE work_order_material_usages (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  work_order_id          UUID NOT NULL REFERENCES work_orders(id),
  material_id            UUID NOT NULL REFERENCES materials(id),
  used_quantity          NUMERIC(18,4) NOT NULL,
  unit_id                UUID NOT NULL REFERENCES units_of_measure(id),
  stock_document_line_id UUID REFERENCES stock_document_lines(id),
  usage_date             DATE NOT NULL
  -- (audit fields)
);
```

### `work_order_checklists`
```sql
CREATE TABLE work_order_checklists (
  id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  work_order_id  UUID NOT NULL REFERENCES work_orders(id),
  title          VARCHAR(255) NOT NULL,
  description    TEXT
  -- (audit + soft delete)
);
```

### `work_order_checklist_items`
```sql
CREATE TABLE work_order_checklist_items (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  checklist_id    UUID NOT NULL REFERENCES work_order_checklists(id),
  description     TEXT NOT NULL,
  is_checked      BOOLEAN NOT NULL DEFAULT false,
  checked_by      UUID REFERENCES users(id),
  checked_at      TIMESTAMPTZ,
  sort_order      INTEGER NOT NULL DEFAULT 0,
  notes           TEXT
);
```

### `work_order_status_histories`
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

## 4.13 FieldOperations Module

### `daily_site_reports`
```sql
CREATE TABLE daily_site_reports (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id        UUID NOT NULL REFERENCES projects(id),
  report_date       DATE NOT NULL,
  weather_condition VARCHAR(100),
  status            VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Submitted→Approved
  summary           TEXT,
  approval_request_id UUID,
  CONSTRAINT uq_dsr UNIQUE (project_id, report_date)
  -- (audit + soft delete)
);
```

### `daily_site_report_workers`
```sql
CREATE TABLE daily_site_report_workers (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  report_id       UUID NOT NULL REFERENCES daily_site_reports(id),
  employee_id     UUID NOT NULL REFERENCES employees(id),
  work_hours      NUMERIC(5,2) NOT NULL,
  work_type       VARCHAR(50)  -- 'Normal','Overtime','NightShift'
);
```

### `daily_site_report_equipments`
```sql
CREATE TABLE daily_site_report_equipments (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  report_id       UUID NOT NULL REFERENCES daily_site_reports(id),
  equipment_id    UUID NOT NULL REFERENCES equipment_assets(id),
  usage_hours     NUMERIC(5,2),
  notes           TEXT
);
```

### `daily_site_report_materials`
```sql
CREATE TABLE daily_site_report_materials (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  report_id              UUID NOT NULL REFERENCES daily_site_reports(id),
  material_id            UUID NOT NULL REFERENCES materials(id),
  used_quantity          NUMERIC(18,4) NOT NULL,
  unit_id                UUID NOT NULL REFERENCES units_of_measure(id),
  stock_document_line_id UUID REFERENCES stock_document_lines(id)
);
```

### `progress_entries`
```sql
CREATE TABLE progress_entries (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  project_id          UUID NOT NULL REFERENCES projects(id),
  phase_id            UUID REFERENCES project_phases(id),
  work_order_id       UUID REFERENCES work_orders(id),
  entry_date          DATE NOT NULL,
  completed_quantity  NUMERIC(18,4) NOT NULL,
  unit_id             UUID NOT NULL REFERENCES units_of_measure(id),
  cumulative_quantity NUMERIC(18,4),
  description         TEXT
  -- (audit + soft delete)
);
```

### `measurement_sheets`
```sql
CREATE TABLE measurement_sheets (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  sheet_number  VARCHAR(100) NOT NULL UNIQUE,
  project_id    UUID NOT NULL REFERENCES projects(id),
  sheet_date    DATE NOT NULL,
  status        VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Submitted→Approved
  approved_by   UUID REFERENCES users(id),
  approved_at   TIMESTAMPTZ,
  notes         TEXT
  -- (audit + soft delete)
);
```

### `measurement_sheet_lines`
```sql
CREATE TABLE measurement_sheet_lines (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  measurement_sheet_id  UUID NOT NULL REFERENCES measurement_sheets(id),
  phase_id              UUID NOT NULL REFERENCES project_phases(id),
  work_order_id         UUID REFERENCES work_orders(id),
  measured_quantity     NUMERIC(18,4) NOT NULL,
  previous_quantity     NUMERIC(18,4) NOT NULL DEFAULT 0,
  current_quantity      NUMERIC(18,4) NOT NULL,
  unit_id               UUID NOT NULL REFERENCES units_of_measure(id),
  description           TEXT
);
```

---

## 4.14 Assets Module

### `equipment_assets`
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
  status              VARCHAR(50) NOT NULL DEFAULT 'Available',
  -- Available→InUse→UnderMaintenance→Retired
  current_project_id  UUID REFERENCES projects(id),
  notes               TEXT
  -- (audit + soft delete)
);
```

### `equipment_assignments`
```sql
CREATE TABLE equipment_assignments (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  equipment_id    UUID NOT NULL REFERENCES equipment_assets(id),
  project_id      UUID REFERENCES projects(id),
  employee_id     UUID REFERENCES employees(id),
  assigned_date   DATE NOT NULL,
  return_date     DATE,
  status          VARCHAR(50) NOT NULL DEFAULT 'Active'
  -- Active→Returned
  -- (audit + soft delete)
);
```

### `equipment_maintenances`
```sql
CREATE TABLE equipment_maintenances (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  equipment_id          UUID NOT NULL REFERENCES equipment_assets(id),
  maintenance_type      VARCHAR(50) NOT NULL,
  -- Preventive,Corrective,Emergency
  maintenance_date      DATE NOT NULL,
  description           TEXT,
  cost                  NUMERIC(18,2),
  technician_id         UUID REFERENCES employees(id),
  next_maintenance_date DATE
  -- (audit + soft delete)
);
```

---

## 4.15 Finance Module

### `financial_accounts`
```sql
CREATE TABLE financial_accounts (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id    UUID NOT NULL REFERENCES companies(id),
  code          VARCHAR(50) NOT NULL,
  name          VARCHAR(200) NOT NULL,
  account_type  VARCHAR(50) NOT NULL,
  -- Asset,Liability,Income,Expense,Equity
  currency_id   UUID REFERENCES currencies(id),
  is_active     BOOLEAN NOT NULL DEFAULT true,
  CONSTRAINT uq_financial_accounts UNIQUE (company_id, code)
  -- (audit + soft delete)
);
```

### `cost_centers`
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
  -- (audit + soft delete)
);
```

### `financial_transactions`
```sql
CREATE TABLE financial_transactions (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  transaction_number  VARCHAR(100) NOT NULL,
  transaction_type    VARCHAR(50) NOT NULL,
  -- Expense,Income,Payable,Receivable,Payment,Collection
  transaction_date    DATE NOT NULL,
  total_amount        NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  exchange_rate       NUMERIC(18,6),
  related_entity_type VARCHAR(100),
  related_entity_id   UUID,
  description         TEXT,
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft'
  -- Draft→Posted
  -- (audit + soft delete)
);
```

### `financial_transaction_lines`
```sql
CREATE TABLE financial_transaction_lines (
  id                       UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  financial_transaction_id UUID NOT NULL REFERENCES financial_transactions(id),
  account_id               UUID NOT NULL REFERENCES financial_accounts(id),
  cost_center_id           UUID REFERENCES cost_centers(id),
  debit                    NUMERIC(18,2) NOT NULL DEFAULT 0,
  credit                   NUMERIC(18,2) NOT NULL DEFAULT 0,
  description              TEXT,
  CONSTRAINT chk_debit_credit CHECK (debit >= 0 AND credit >= 0 AND (debit > 0 OR credit > 0))
);
```

### `bank_accounts`
```sql
CREATE TABLE bank_accounts (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id  UUID NOT NULL REFERENCES companies(id),
  bank_name   VARCHAR(200) NOT NULL,
  iban        VARCHAR(50) NOT NULL,
  currency_id UUID NOT NULL REFERENCES currencies(id),
  account_name VARCHAR(200),
  is_active   BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

### `payables`
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
  status              VARCHAR(50) NOT NULL DEFAULT 'Open',
  -- Open→PartiallyPaid→Paid→Overdue→Cancelled
  description         TEXT
  -- (audit + soft delete)
);
CREATE INDEX idx_payables_status ON payables(status);
CREATE INDEX idx_payables_due ON payables(due_date) WHERE status NOT IN ('Paid','Cancelled');
```

### `receivables`
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
  status              VARCHAR(50) NOT NULL DEFAULT 'Open'
  -- Open→PartiallyCollected→Collected→Overdue→Cancelled
  -- (audit + soft delete)
);
```

### `payments`
```sql
CREATE TABLE payments (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  payment_date        DATE NOT NULL,
  amount              NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  payment_method      VARCHAR(50) NOT NULL,
  -- Havale,EFT,Cheque,Cash,CreditCard
  bank_account_id     UUID REFERENCES bank_accounts(id),
  reference_number    VARCHAR(200),
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Approved→Completed→Cancelled
  approval_request_id UUID,
  notes               TEXT
  -- (audit + soft delete)
);
```

### `payment_allocations`
```sql
CREATE TABLE payment_allocations (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  payment_id        UUID NOT NULL REFERENCES payments(id),
  payable_id        UUID NOT NULL REFERENCES payables(id),
  allocated_amount  NUMERIC(18,2) NOT NULL,
  CONSTRAINT uq_payment_alloc UNIQUE (payment_id, payable_id)
);
```

### `collections`
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
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft'
  -- (audit + soft delete)
);
```

### `collection_allocations`
```sql
CREATE TABLE collection_allocations (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  collection_id     UUID NOT NULL REFERENCES collections(id),
  receivable_id     UUID NOT NULL REFERENCES receivables(id),
  allocated_amount  NUMERIC(18,2) NOT NULL
);
```

---

## 4.16 Budget Module

### `budgets`
```sql
CREATE TABLE budgets (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id    UUID NOT NULL REFERENCES companies(id),
  project_id    UUID REFERENCES projects(id),
  period_start  DATE NOT NULL,
  period_end    DATE NOT NULL,
  total_amount  NUMERIC(18,2) NOT NULL,
  currency_id   UUID NOT NULL REFERENCES currencies(id),
  status        VARCHAR(50) NOT NULL DEFAULT 'Draft'
  -- Draft→Approved→Closed
  -- (audit + soft delete)
);
```

### `budget_lines`
```sql
CREATE TABLE budget_lines (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  budget_id       UUID NOT NULL REFERENCES budgets(id),
  account_id      UUID NOT NULL REFERENCES financial_accounts(id),
  cost_center_id  UUID REFERENCES cost_centers(id),
  planned_amount  NUMERIC(18,2) NOT NULL,
  actual_amount   NUMERIC(18,2) NOT NULL DEFAULT 0,
  variance_amount NUMERIC(18,2) GENERATED ALWAYS AS (planned_amount - actual_amount) STORED
);
```

---

## 4.17 Contracts Module

### `contracts`
```sql
CREATE TABLE contracts (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id          UUID NOT NULL REFERENCES companies(id),
  contract_number     VARCHAR(100) NOT NULL UNIQUE,
  contract_type       VARCHAR(50) NOT NULL,
  -- Customer,Supplier,Subcontractor,Rental,Service
  project_id          UUID REFERENCES projects(id),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  start_date          DATE NOT NULL,
  end_date            DATE,
  total_amount        NUMERIC(18,2) NOT NULL,
  currency_id         UUID NOT NULL REFERENCES currencies(id),
  status              VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Active→Completed→Terminated→Suspended
  description         TEXT,
  approval_request_id UUID
  -- (audit + soft delete)
);
CREATE INDEX idx_contracts_project ON contracts(project_id);
CREATE INDEX idx_contracts_type ON contracts(contract_type);
```

### `contract_parties`
```sql
CREATE TABLE contract_parties (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  contract_id         UUID NOT NULL REFERENCES contracts(id),
  business_partner_id UUID NOT NULL REFERENCES business_partners(id),
  role                VARCHAR(100) NOT NULL
  -- Client,Contractor,Subcontractor,Consultant
);
```

### `contract_lines`
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

### `contract_amendments`
```sql
CREATE TABLE contract_amendments (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  contract_id       UUID NOT NULL REFERENCES contracts(id),
  amendment_number  INTEGER NOT NULL,
  amendment_date    DATE NOT NULL,
  description       TEXT,
  amount_change     NUMERIC(18,2),
  new_total_amount  NUMERIC(18,2),
  status            VARCHAR(50) NOT NULL DEFAULT 'Draft'
  -- (audit + soft delete)
);
```

---

## 4.18 ProgressPayments Module

### `progress_payments`
```sql
CREATE TABLE progress_payments (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id        UUID NOT NULL REFERENCES companies(id),
  payment_number    VARCHAR(100) NOT NULL UNIQUE,
  contract_id       UUID NOT NULL REFERENCES contracts(id),
  project_id        UUID NOT NULL REFERENCES projects(id),
  period_start      DATE NOT NULL,
  period_end        DATE NOT NULL,
  gross_amount      NUMERIC(18,2) NOT NULL,
  deduction_amount  NUMERIC(18,2) NOT NULL DEFAULT 0,
  net_amount        NUMERIC(18,2) NOT NULL,
  currency_id       UUID NOT NULL REFERENCES currencies(id),
  status            VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→Submitted→UnderApproval→Approved→Invoiced→Paid
  approval_request_id UUID,
  measurement_sheet_id UUID REFERENCES measurement_sheets(id)
  -- (audit + soft delete)
);
```

### `progress_payment_lines`
```sql
CREATE TABLE progress_payment_lines (
  id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  progress_payment_id    UUID NOT NULL REFERENCES progress_payments(id),
  phase_id               UUID NOT NULL REFERENCES project_phases(id),
  contract_line_id       UUID REFERENCES contract_lines(id),
  measured_quantity      NUMERIC(18,4) NOT NULL,
  unit_id                UUID NOT NULL REFERENCES units_of_measure(id),
  unit_price             NUMERIC(18,4) NOT NULL,
  current_period_amount  NUMERIC(18,2) NOT NULL,
  cumulative_amount      NUMERIC(18,2) NOT NULL
);
```

### `progress_payment_deductions`
```sql
CREATE TABLE progress_payment_deductions (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  progress_payment_id UUID NOT NULL REFERENCES progress_payments(id),
  deduction_type      VARCHAR(100) NOT NULL,
  -- AdvanceRecovery,RetentionMoney,Penalty,Tax,Other
  amount              NUMERIC(18,2) NOT NULL,
  description         TEXT
);
```

---

## 4.19 Documents Module

### `document_folders`
```sql
CREATE TABLE document_folders (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  parent_id           UUID REFERENCES document_folders(id),
  name                VARCHAR(255) NOT NULL,
  related_entity_type VARCHAR(100),
  related_entity_id   UUID,
  is_system_folder    BOOLEAN NOT NULL DEFAULT false
  -- (audit + soft delete)
);
```

### `documents`
```sql
CREATE TABLE documents (
  id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  company_id        UUID NOT NULL REFERENCES companies(id),
  title             VARCHAR(500) NOT NULL,
  document_type     VARCHAR(100),
  -- Contract,Drawing,Invoice,Report,Photo,Certificate,Other
  folder_id         UUID REFERENCES document_folders(id),
  status            VARCHAR(50) NOT NULL DEFAULT 'Draft',
  -- Draft→PendingApproval→Approved→Archived
  latest_version_id UUID  -- FK to document_versions(id)
  -- (audit + soft delete)
);
```

### `document_versions`
```sql
CREATE TABLE document_versions (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  document_id     UUID NOT NULL REFERENCES documents(id),
  version_number  VARCHAR(20) NOT NULL,
  file_url        TEXT NOT NULL,
  file_size       BIGINT,
  mime_type       VARCHAR(100),
  uploaded_by     UUID NOT NULL REFERENCES users(id),
  uploaded_at     TIMESTAMPTZ NOT NULL DEFAULT now(),
  checksum        VARCHAR(100)
);
```

### `document_relations`
```sql
CREATE TABLE document_relations (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  document_id         UUID NOT NULL REFERENCES documents(id),
  related_entity_type VARCHAR(100) NOT NULL,
  related_entity_id   UUID NOT NULL,
  CONSTRAINT uq_doc_relation UNIQUE (document_id, related_entity_type, related_entity_id)
);
CREATE INDEX idx_doc_relations_entity ON document_relations(related_entity_type, related_entity_id);
```

### `document_permissions`
```sql
CREATE TABLE document_permissions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  document_id UUID NOT NULL REFERENCES documents(id),
  user_id     UUID REFERENCES users(id),
  role_id     UUID REFERENCES roles(id),
  permission  VARCHAR(20) NOT NULL,
  -- Read,Write,Delete
  CONSTRAINT chk_doc_perm_target CHECK (user_id IS NOT NULL OR role_id IS NOT NULL)
);
```

---

## 4.20 Workflow Module

### `approval_definitions`
```sql
CREATE TABLE approval_definitions (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  code                VARCHAR(100) NOT NULL UNIQUE,
  -- APR-PURCHASE, APR-REQUEST, APR-PROGRESS, etc.
  name                VARCHAR(255) NOT NULL,
  related_module      VARCHAR(100) NOT NULL,
  related_entity_type VARCHAR(100) NOT NULL,
  is_active           BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

### `approval_definition_versions`
```sql
CREATE TABLE approval_definition_versions (
  id                 UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  definition_id      UUID NOT NULL REFERENCES approval_definitions(id),
  version_number     INTEGER NOT NULL,
  is_current_version BOOLEAN NOT NULL DEFAULT false,
  effective_date     DATE NOT NULL,
  description        TEXT,
  CONSTRAINT uq_adv_version UNIQUE (definition_id, version_number)
  -- (audit + soft delete)
);
-- Only one row per definition_id can have is_current_version=true
```

### `approval_conditions`
```sql
CREATE TABLE approval_conditions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  version_id  UUID NOT NULL REFERENCES approval_definition_versions(id),
  field_name  VARCHAR(100) NOT NULL,
  -- TotalAmount,ProjectId,DepartmentId,Priority,etc.
  operator    VARCHAR(30) NOT NULL,
  -- Equals,GreaterThan,LessThan,GreaterOrEqual,LessOrEqual,In,NotIn
  value       TEXT NOT NULL,
  group_id    INTEGER DEFAULT 1
  -- Conditions in same group are AND'd; groups are OR'd
);
```

### `approval_step_definitions`
```sql
CREATE TABLE approval_step_definitions (
  id                      UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  version_id              UUID NOT NULL REFERENCES approval_definition_versions(id),
  step_number             INTEGER NOT NULL,
  name                    VARCHAR(255) NOT NULL,
  approval_mode           VARCHAR(30) NOT NULL,
  -- Sequential,ParallelAny,ParallelAll,Quorum
  required_approval_count INTEGER,
  is_required             BOOLEAN NOT NULL DEFAULT true,
  timeout_hours           INTEGER,
  CONSTRAINT uq_step_version UNIQUE (version_id, step_number)
);
```

### `approval_step_approvers`
```sql
CREATE TABLE approval_step_approvers (
  id                   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  step_definition_id   UUID NOT NULL REFERENCES approval_step_definitions(id),
  approver_type        VARCHAR(50) NOT NULL,
  -- User,Role,ProjectRole,DepartmentManager
  approver_id          UUID,
  role_id              UUID REFERENCES roles(id),
  project_role         VARCHAR(100)
);
```

### `approval_requests`
```sql
CREATE TABLE approval_requests (
  id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  definition_version_id UUID NOT NULL REFERENCES approval_definition_versions(id),
  related_entity_type   VARCHAR(100) NOT NULL,
  related_entity_id     UUID NOT NULL,
  requested_by          UUID NOT NULL REFERENCES users(id),
  requested_at          TIMESTAMPTZ NOT NULL DEFAULT now(),
  status                VARCHAR(50) NOT NULL DEFAULT 'Pending',
  -- Draft→Pending→Approved→Rejected→Returned→Cancelled
  current_step_number   INTEGER,
  completed_at          TIMESTAMPTZ,
  notes                 TEXT
  -- (audit + soft delete)
);
CREATE INDEX idx_apr_entity ON approval_requests(related_entity_type, related_entity_id);
CREATE INDEX idx_apr_status ON approval_requests(status) WHERE status = 'Pending';
```

### `approval_request_steps`
```sql
CREATE TABLE approval_request_steps (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  approval_request_id UUID NOT NULL REFERENCES approval_requests(id),
  step_definition_id  UUID NOT NULL REFERENCES approval_step_definitions(id),
  step_number         INTEGER NOT NULL,
  status              VARCHAR(50) NOT NULL DEFAULT 'Waiting',
  -- Waiting→Active→Approved→Rejected→Returned→Skipped
  activated_at        TIMESTAMPTZ,
  completed_at        TIMESTAMPTZ,
  timeout_at          TIMESTAMPTZ
);
```

### `approval_request_approvers`
```sql
CREATE TABLE approval_request_approvers (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  request_step_id UUID NOT NULL REFERENCES approval_request_steps(id),
  user_id         UUID NOT NULL REFERENCES users(id),
  -- Copied at request time — immutable snapshot
  status          VARCHAR(50) NOT NULL DEFAULT 'Waiting',
  -- Waiting→Approved→Rejected→Delegated
  delegated_to    UUID REFERENCES users(id)
);
```

### `approval_actions`
```sql
CREATE TABLE approval_actions (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  request_step_id UUID NOT NULL REFERENCES approval_request_steps(id),
  approver_id     UUID NOT NULL REFERENCES users(id),
  action_type     VARCHAR(30) NOT NULL,
  -- Approve,Reject,Return,Cancel
  action_date     TIMESTAMPTZ NOT NULL DEFAULT now(),
  comment         TEXT
);
```

### `approval_delegations`
```sql
CREATE TABLE approval_delegations (
  id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  delegator_id  UUID NOT NULL REFERENCES users(id),
  delegate_id   UUID NOT NULL REFERENCES users(id),
  valid_from    TIMESTAMPTZ NOT NULL,
  valid_to      TIMESTAMPTZ NOT NULL,
  reason        TEXT,
  is_active     BOOLEAN NOT NULL DEFAULT true,
  CONSTRAINT chk_delegation_dates CHECK (valid_to > valid_from)
  -- (audit + soft delete)
);
CREATE INDEX idx_delegations_active ON approval_delegations(delegator_id, valid_from, valid_to) WHERE is_active = true;
```

---

## 4.21 Notifications Module

### `notifications`
```sql
CREATE TABLE notifications (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  title               VARCHAR(500) NOT NULL,
  body                TEXT NOT NULL,
  notification_type   VARCHAR(100) NOT NULL,
  -- ApprovalRequest,StockAlert,PaymentDue,BudgetAlert,SystemInfo
  related_entity_type VARCHAR(100),
  related_entity_id   UUID,
  priority            VARCHAR(20) NOT NULL DEFAULT 'Normal',
  created_at          TIMESTAMPTZ NOT NULL DEFAULT now()
  -- (no soft delete — notifications are not deleted)
);
```

### `notification_recipients`
```sql
CREATE TABLE notification_recipients (
  id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  notification_id UUID NOT NULL REFERENCES notifications(id),
  user_id         UUID NOT NULL REFERENCES users(id),
  is_read         BOOLEAN NOT NULL DEFAULT false,
  read_at         TIMESTAMPTZ,
  channel         VARCHAR(20) NOT NULL DEFAULT 'InApp',
  -- InApp,Email,SMS
  sent_at         TIMESTAMPTZ,
  delivery_status VARCHAR(30) DEFAULT 'Pending'
  -- Pending,Sent,Delivered,Failed
);
CREATE INDEX idx_notif_recipients_user ON notification_recipients(user_id, is_read);
```

### `notification_preferences`
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

## 4.22 Chat Module

### `chat_groups`
```sql
CREATE TABLE chat_groups (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name                VARCHAR(255),
  owner_id            UUID NOT NULL REFERENCES users(id),
  is_private          BOOLEAN NOT NULL DEFAULT false,
  related_entity_type VARCHAR(100),
  related_entity_id   UUID,
  created_at          TIMESTAMPTZ NOT NULL DEFAULT now()
  -- (soft delete)
);
```

### `chat_group_members`
```sql
CREATE TABLE chat_group_members (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  group_id    UUID NOT NULL REFERENCES chat_groups(id),
  user_id     UUID NOT NULL REFERENCES users(id),
  joined_at   TIMESTAMPTZ NOT NULL DEFAULT now(),
  is_admin    BOOLEAN NOT NULL DEFAULT false,
  CONSTRAINT uq_cgm UNIQUE (group_id, user_id)
);
```

### `chat_messages`
```sql
CREATE TABLE chat_messages (
  id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  group_id            UUID NOT NULL REFERENCES chat_groups(id),
  sender_id           UUID NOT NULL REFERENCES users(id),
  reply_to_message_id UUID REFERENCES chat_messages(id),
  content             TEXT,
  message_type        VARCHAR(30) NOT NULL DEFAULT 'Text',
  -- Text,File,Image,System
  file_url            TEXT,
  is_edited           BOOLEAN NOT NULL DEFAULT false,
  is_deleted          BOOLEAN NOT NULL DEFAULT false,
  created_at          TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX idx_chat_msgs_group ON chat_messages(group_id, created_at DESC);
```

### `chat_message_reactions`
```sql
CREATE TABLE chat_message_reactions (
  id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  message_id  UUID NOT NULL REFERENCES chat_messages(id),
  user_id     UUID NOT NULL REFERENCES users(id),
  emoji       VARCHAR(20) NOT NULL,
  CONSTRAINT uq_reaction UNIQUE (message_id, user_id, emoji)
);
```

---

## 4.23 Reporting Module

### `report_definitions`
```sql
CREATE TABLE report_definitions (
  id                      UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name                    VARCHAR(255) NOT NULL,
  description             TEXT,
  module                  VARCHAR(100),
  query_definition        JSONB NOT NULL,
  required_permission_code VARCHAR(200),
  is_public               BOOLEAN NOT NULL DEFAULT false,
  is_active               BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

### `dashboard_widgets`
```sql
CREATE TABLE dashboard_widgets (
  id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name             VARCHAR(255) NOT NULL,
  widget_type      VARCHAR(50) NOT NULL,
  -- Chart,Table,KPI,Map,Timeline
  data_source      JSONB NOT NULL,
  default_position JSONB,
  is_active        BOOLEAN NOT NULL DEFAULT true
  -- (audit + soft delete)
);
```

---

# 5. API Specification — Complete

## 5.1 API Design Standards

- **Base URL:** `/api/v1`
- **Auth Header:** `Authorization: Bearer <access_token>`
- **Content-Type:** `application/json`
- **Pagination:** `?page=1&pageSize=20` → response includes `{ data, total, page, pageSize, totalPages }`
- **Filtering:** `?filter[status]=Active&filter[projectId]=<uuid>`
- **Sorting:** `?sort=createdAt:desc`
- **Error format:**
  ```json
  {
    "success": false,
    "error": {
      "code": "VALIDATION_ERROR",
      "message": "Human-readable message",
      "details": [{ "field": "email", "message": "Invalid email format" }]
    },
    "requestId": "req_abc123"
  }
  ```
- **Success format:**
  ```json
  {
    "success": true,
    "data": { ... },
    "meta": { "total": 100, "page": 1 }
  }
  ```

## 5.2 Standard HTTP Status Codes

| Code | Usage |
|------|-------|
| 200 | Success (GET, PUT, PATCH) |
| 201 | Created (POST) |
| 204 | No content (DELETE) |
| 400 | Validation error |
| 401 | Unauthenticated |
| 403 | Forbidden (missing permission) |
| 404 | Not found |
| 409 | Conflict (duplicate, state violation) |
| 422 | Business rule violation |
| 429 | Rate limit exceeded |
| 500 | Internal server error |

---

## 5.3 Authentication Endpoints

### `POST /api/auth/login`
Login with username + password.

**Request:**
```json
{
  "username": "john.doe",
  "password": "SecurePass123!"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 900,
    "user": {
      "id": "uuid",
      "username": "john.doe",
      "email": "john@company.com",
      "firstName": "John",
      "lastName": "Doe",
      "roles": ["ProjectManager"],
      "permissions": ["Projects.Read", "WorkOrders.Create"]
    }
  }
}
```
**Note:** Refresh token is set as `HttpOnly` cookie.

**Error 401:** Invalid credentials  
**Error 423:** Account locked (too many failed attempts)

---

### `POST /api/auth/refresh`
Exchange refresh token cookie for new access token.

**Response 200:** Same structure as login `data` object.  
**Error 401:** Token expired or revoked.

---

### `POST /api/auth/logout`
Revoke current refresh token.

**Response 204:** No content.

---

### `POST /api/auth/change-password`
**Request:**
```json
{
  "currentPassword": "OldPass123!",
  "newPassword": "NewPass456!",
  "confirmPassword": "NewPass456!"
}
```
**Response 204**

---

## 5.4 Core Module Endpoints

### Companies
| Method | Path | Permission | Description |
|--------|------|-----------|-------------|
| GET | `/api/v1/companies` | `Core.Company.Read` | List companies |
| POST | `/api/v1/companies` | `Core.Company.Create` | Create company |
| GET | `/api/v1/companies/:id` | `Core.Company.Read` | Get company |
| PUT | `/api/v1/companies/:id` | `Core.Company.Update` | Update company |
| DELETE | `/api/v1/companies/:id` | `Core.Company.Delete` | Soft delete |

### Currencies
| Method | Path | Permission | Description |
|--------|------|-----------|-------------|
| GET | `/api/v1/currencies` | Public | List currencies |
| POST | `/api/v1/currencies` | `Core.Currency.Create` | Add currency |
| GET | `/api/v1/exchange-rates` | Public | List rates (filterable by date) |
| POST | `/api/v1/exchange-rates` | `Core.ExchangeRate.Create` | Add rate |
| GET | `/api/v1/exchange-rates/latest` | Public | Latest rates for all pairs |

### Units of Measure
| Method | Path | Permission |
|--------|------|-----------|
| GET | `/api/v1/units-of-measure` | Public |
| POST | `/api/v1/units-of-measure` | `Core.UOM.Create` |
| GET | `/api/v1/unit-conversions` | Public |
| POST | `/api/v1/unit-conversions` | `Core.UOM.Create` |

---

## 5.5 IAM Module Endpoints

### `POST /api/v1/users`
Create user.
```json
{
  "username": "jane.smith",
  "email": "jane@company.com",
  "password": "TempPass123!",
  "firstName": "Jane",
  "lastName": "Smith",
  "phoneNumber": "+905001234567",
  "roleIds": ["uuid-role"]
}
```

### `GET /api/v1/users`
List users. Supports `?filter[isActive]=true&filter[roleId]=<uuid>`.

### `PATCH /api/v1/users/:id/lock` / `PATCH /api/v1/users/:id/unlock`
Lock or unlock a user account.

### `POST /api/v1/users/:id/roles`
Assign role: `{ "roleId": "uuid", "validFrom": "2026-01-01", "validTo": null }`

### `DELETE /api/v1/users/:id/roles/:roleId`
Remove role assignment.

### `GET /api/v1/users/:id/permissions`
Get effective permissions (merged from roles + user-level overrides).

### `POST /api/v1/roles`
Create role: `{ "name": "SiteEngineer", "description": "..." }`

### `GET /api/v1/roles/:id/permissions`
List permissions assigned to role.

### `POST /api/v1/roles/:id/permissions`
Assign permissions to role: `{ "permissionCodes": ["Inventory.Read", "Inventory.Create"] }`

---

## 5.6 Projects Module Endpoints

### `POST /api/v1/projects`
```json
{
  "name": "Ankara Solar Power Plant",
  "typeId": "uuid",
  "customerId": "uuid",
  "startDate": "2026-07-01",
  "endDate": "2027-06-30",
  "budgetAmount": 5000000,
  "currencyId": "uuid-try",
  "branchId": "uuid",
  "description": "50MW solar installation"
}
```
**Response 201:** Project object with auto-generated `projectNumber`.

### `GET /api/v1/projects`
Filterable by `status`, `customerId`, `typeId`, `branchId`. Sortable by `startDate`, `name`.

### `PATCH /api/v1/projects/:id/status`
```json
{ "status": "Active", "reason": "All preconditions met" }
```
Valid transitions: `Draft→Active`, `Active→OnHold`, `OnHold→Active`, `Active→Completed`, `Completed→Closed`, `Draft/Active→Cancelled`.

### `GET /api/v1/projects/:id/summary`
**Response:**
```json
{
  "projectNumber": "PRJ-2026-0001",
  "name": "Ankara Solar Power Plant",
  "status": "Active",
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

### Project Phases
| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/v1/projects/:id/phases` | List WBS phases |
| POST | `/api/v1/projects/:id/phases` | Create phase |
| PUT | `/api/v1/projects/:id/phases/:phaseId` | Update phase |

### Project Members
| Method | Path |
|--------|------|
| GET | `/api/v1/projects/:id/members` |
| POST | `/api/v1/projects/:id/members` |
| DELETE | `/api/v1/projects/:id/members/:memberId` |

---

## 5.7 Inventory Module Endpoints

### `GET /api/v1/stock-balances`
```
GET /api/v1/stock-balances?filter[warehouseId]=<uuid>&filter[materialId]=<uuid>
```
**Response:**
```json
{
  "data": [{
    "materialId": "uuid",
    "materialCode": "CAB-0001",
    "materialName": "NYY 3x4 Cable",
    "warehouseId": "uuid",
    "warehouseName": "Central Warehouse",
    "onHandQuantity": 1200,
    "reservedQuantity": 300,
    "availableQuantity": 900,
    "unitCode": "Meter",
    "averageCost": 18.50
  }]
}
```

### `POST /api/v1/stock-documents`
Create a stock movement document.
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

### `POST /api/v1/stock-documents/:id/submit`
Submit for approval.

### `POST /api/v1/stock-documents/:id/post`
Post (finalize) an approved document. Triggers:
1. FIFO lot allocation (for issues)
2. Append to `stock_transactions`
3. Update `stock_balances`

### `GET /api/v1/stock-documents/:id/transactions`
View the immutable transactions generated by this document.

### `POST /api/v1/warehouse-transfers`
```json
{
  "fromWarehouseId": "uuid",
  "toWarehouseId": "uuid",
  "transferDate": "2026-06-18",
  "lines": [{"materialId": "uuid", "quantity": 100, "unitId": "uuid"}]
}
```

---

## 5.8 Procurement Module Endpoints

### Supplier Quotes
```
POST   /api/v1/supplier-quotes          Create quote
GET    /api/v1/supplier-quotes          List (filter by requestId, supplierId)
POST   /api/v1/supplier-quotes/:id/send Mark as sent to supplier
POST   /api/v1/supplier-quotes/:id/receive  Register received quote with lines
GET    /api/v1/supplier-quotes/compare?requestId=<uuid>  Side-by-side comparison
```

### Purchase Orders
```
POST   /api/v1/purchase-orders          Create PO (from approved request or direct)
GET    /api/v1/purchase-orders          List (filterable by status, supplier, project)
GET    /api/v1/purchase-orders/:id      Get PO detail
POST   /api/v1/purchase-orders/:id/submit   Submit for approval
POST   /api/v1/purchase-orders/:id/cancel   Cancel PO
```

**POST /api/v1/purchase-orders request body:**
```json
{
  "supplierId": "uuid",
  "requestId": "uuid",
  "projectId": "uuid",
  "currencyId": "uuid",
  "expectedDelivery": "2026-07-15",
  "lines": [{
    "materialId": "uuid",
    "quantity": 500,
    "unitId": "uuid",
    "unitPrice": 18.50,
    "vatRate": 18
  }]
}
```

### Purchase Receipts
```
POST   /api/v1/purchase-receipts        Create receipt (partial or full)
GET    /api/v1/purchase-receipts        List
POST   /api/v1/purchase-receipts/:id/complete  Complete receipt → auto-creates stock document
```

### Supplier Invoices
```
POST   /api/v1/supplier-invoices        Register invoice
POST   /api/v1/supplier-invoices/:id/match   Trigger 3-way matching
GET    /api/v1/supplier-invoices/:id/match-result  View matching result
POST   /api/v1/supplier-invoices/:id/approve  Approve invoice → creates payable
POST   /api/v1/supplier-invoices/:id/reject   Reject (with reason)
```

---

## 5.9 Operations Module Endpoints

```
POST   /api/v1/work-orders              Create work order
GET    /api/v1/work-orders              List (filter by project, status, priority)
GET    /api/v1/work-orders/:id          Detail
PATCH  /api/v1/work-orders/:id/status   Transition status
POST   /api/v1/work-orders/:id/assignments     Assign employees
DELETE /api/v1/work-orders/:id/assignments/:assignmentId
POST   /api/v1/work-orders/:id/material-plans  Plan materials
POST   /api/v1/work-orders/:id/material-usages Record actual usage → triggers stock issue
POST   /api/v1/work-orders/:id/checklists      Create checklist
PATCH  /api/v1/work-orders/:id/checklists/:cid/items/:itemId  Check/uncheck item
GET    /api/v1/work-orders/:id/status-history
```

---

## 5.10 Workflow Module Endpoints

### `POST /api/v1/approval-requests`
Programmatic submission (called internally by service layer).
```json
{
  "definitionCode": "APR-PURCHASE",
  "relatedEntityType": "purchase_orders",
  "relatedEntityId": "uuid",
  "requestedBy": "uuid"
}
```

### `GET /api/v1/approval-requests/my-queue`
Returns pending approvals for the current user.

### `POST /api/v1/approval-requests/:id/approve`
```json
{ "comment": "Approved after reviewing quotes." }
```

### `POST /api/v1/approval-requests/:id/reject`
```json
{ "comment": "Price too high. Please re-quote." }
```

### `POST /api/v1/approval-requests/:id/return`
```json
{ "comment": "Missing technical specs, please revise." }
```

### `POST /api/v1/approval-delegations`
```json
{
  "delegateId": "uuid",
  "validFrom": "2026-07-01T00:00:00Z",
  "validTo": "2026-07-15T23:59:59Z",
  "reason": "Annual leave"
}
```

---

## 5.11 Finance Module Endpoints

```
POST   /api/v1/payments                 Create payment
GET    /api/v1/payments                 List
POST   /api/v1/payments/:id/submit      Submit for approval
POST   /api/v1/payments/:id/complete    Mark as completed
POST   /api/v1/payments/:id/allocate    Allocate to payable(s)
  Body: [{ "payableId": "uuid", "amount": 50000 }]

GET    /api/v1/payables                 List open payables (filterable)
GET    /api/v1/receivables              List open receivables
POST   /api/v1/collections             Register incoming payment
POST   /api/v1/collections/:id/allocate Allocate to receivable(s)

GET    /api/v1/finance/dashboard        Summary KPIs
  Response: { totalPayables, overduePayables, totalReceivables,
              overdueReceivables, cashPosition, paymentsDueThisWeek }
```

---

## 5.12 ProgressPayments (Hakediş) Endpoints

```
POST   /api/v1/progress-payments        Create hakediş
GET    /api/v1/progress-payments        List (filter by project, contract, status)
GET    /api/v1/progress-payments/:id    Detail with lines and deductions
POST   /api/v1/progress-payments/:id/submit   Submit for approval
POST   /api/v1/progress-payments/:id/invoice  Mark as invoiced → creates receivable
GET    /api/v1/progress-payments/:id/pdf Preview PDF
```

---

## 5.13 Notifications & Chat Endpoints

```
GET    /api/v1/notifications/me          My notifications (unread count)
PATCH  /api/v1/notifications/:id/read    Mark read
POST   /api/v1/notifications/mark-all-read  Mark all read
PUT    /api/v1/notification-preferences  Update channel preferences

GET    /api/v1/chat/groups              My chat groups
POST   /api/v1/chat/groups             Create group
POST   /api/v1/chat/groups/:id/messages  Send message
GET    /api/v1/chat/groups/:id/messages  History (cursor-based pagination)
DELETE /api/v1/chat/messages/:id        Soft-delete message
```

**WebSocket events (Socket.IO):**
```
Client → Server:
  chat:join_group      { groupId }
  chat:send_message    { groupId, content, replyToId? }
  chat:typing          { groupId }

Server → Client:
  chat:new_message     { message object }
  chat:user_typing     { userId, groupId }
  notification:new     { notification object }
  approval:action      { requestId, action, entityType, entityId }
```

---

# 6. Business Flows — End-to-End

## 6.1 Material Procurement Flow

```
┌─────────────────────────────────────────────────────────────────────┐
│ TRIGGER: Project phase requires material not in stock               │
└───────────────────────────┬─────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────────┐
│ 1. CREATE REQUEST                                                    │
│    Actor: ProjectManager / SiteSupervisor                           │
│    • POST /api/v1/requests                                          │
│    • Status: Draft                                                  │
│    • Add RequestLines (material, qty, required date)                │
│    • POST /api/v1/requests/:id/submit → Status: PendingApproval    │
│    • SYSTEM: Creates ApprovalRequest for APR-REQUEST flow           │
│    • SYSTEM: Sends notification to approvers                        │
└───────────────────────────┬─────────────────────────────────────────┘
                            │ Approval: Approved
                            ▼
┌─────────────────────────────────────────────────────────────────────┐
│ 2. SUPPLIER QUOTE PHASE                                             │
│    Actor: PurchaseManager                                           │
│    • POST /api/v1/supplier-quotes (one per supplier, linked to req) │
│    • POST /api/v1/supplier-quotes/:id/send                          │
│    • Supplier responds → POST /api/v1/supplier-quotes/:id/receive  │
│    • GET /api/v1/supplier-quotes/compare?requestId=<uuid>          │
│    • PurchaseManager selects best quote → Status: Accepted         │
└───────────────────────────┬─────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────────┐
│ 3. PURCHASE ORDER                                                   │
│    • POST /api/v1/purchase-orders (from accepted quote)             │
│    • Status: Draft                                                  │
│    • POST /api/v1/purchase-orders/:id/submit                        │
│    • SYSTEM: Evaluates ApprovalConditions (total amount)            │
│      - 0-50K TRY  → ProjectManager only                            │
│      - 50K-250K   → PurchaseManager + FinanceManager (Sequential)  │
│      - 250K+      → +Admin (ParallelAll for FinMgr+PM, then Admin) │
│    • Approval chain executes (notifications at each step)           │
│    • On final approval → Status: Approved                           │
│    • SYSTEM: Notifies supplier via email (optional)                 │
└───────────────────────────┬─────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────────┐
│ 4. GOODS RECEIPT                                                    │
│    Actor: WarehouseManager                                          │
│    • POST /api/v1/purchase-receipts                                 │
│    • Lines: actual received qty per PO line                         │
│    • POST /api/v1/purchase-receipts/:id/complete                    │
│    • SYSTEM: Auto-creates StockDocument [Type=PurchaseReceipt]      │
│    • SYSTEM: Posts StockDocument                                    │
│      → Creates StockLots (cost layer)                               │
│      → Appends StockTransactions (immutable)                        │
│      → Updates StockBalances                                        │
│    • PurchaseOrder status → PartiallyReceived or Received           │
└───────────────────────────┬─────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────────┐
│ 5. INVOICE PROCESSING (3-Way Matching)                              │
│    Actor: PurchaseManager / FinanceManager                          │
│    • POST /api/v1/supplier-invoices                                 │
│    • POST /api/v1/supplier-invoices/:id/match                       │
│    • SYSTEM checks:                                                 │
│      Invoice.qty ≤ Receipt.qty ≤ Order.qty ?                       │
│      Invoice.unitPrice vs Order.unitPrice (tolerance ±5%)           │
│    • If within tolerance → Status: Matched → Approved               │
│    • If >5% price variance → Status: ManualReview                   │
│    • On Approved → SYSTEM creates Payables record                   │
│    • Request.Status → Ordered                                       │
└───────────────────────────┬─────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────────┐
│ 6. PAYMENT                                                          │
│    Actor: FinanceManager                                            │
│    • POST /api/v1/payments                                          │
│    • POST /api/v1/payments/:id/allocate → links to Payable          │
│    • Approval if required → POST /api/v1/payments/:id/submit        │
│    • On completion:                                                 │
│      Payable.remainingAmount decreases                              │
│      Payable.status → PartiallyPaid or Paid                        │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 6.2 Field Operations → Hakediş Flow

```
┌────────────────────────────────────────────────────────────────────┐
│ 1. WORK ORDERS & DAILY SITE REPORTS                               │
│    • Work orders assigned to site team                             │
│    • Daily: POST /api/v1/projects/:id/site-reports                 │
│      → Workers, equipment, materials used                          │
│    • SYSTEM: Cross-references stock reservations                   │
│    • Work order progress updated                                   │
└───────────────────────────┬────────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────────┐
│ 2. PROGRESS ENTRIES                                                │
│    • POST /api/v1/work-orders/:id/progress                         │
│    • CompletedQuantity recorded per phase/WO                       │
└───────────────────────────┬────────────────────────────────────────┘
                            │ End of billing period
                            ▼
┌────────────────────────────────────────────────────────────────────┐
│ 3. MEASUREMENT SHEET                                               │
│    Actor: SiteSupervisor                                           │
│    • POST /api/v1/measurement-sheets                               │
│    • Lines: each phase → MeasuredQty (current period)              │
│    • POST /api/v1/measurement-sheets/:id/submit                    │
│    • Approval: SiteSupervisor → PM → Status: Approved              │
└───────────────────────────┬────────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────────┐
│ 4. HAKEDIŞ CREATION                                                │
│    Actor: ProjectManager                                           │
│    • POST /api/v1/progress-payments                                │
│    • Lines auto-populated from MeasurementSheet:                   │
│      MeasuredQty × ContractLine.UnitPrice = CurrentPeriodAmount    │
│    • Add deductions:                                               │
│      AdvanceRecovery: 10% of gross                                 │
│      RetentionMoney: 5% of gross                                   │
│    • NetAmount = GrossAmount - Deductions                           │
│    • POST /api/v1/progress-payments/:id/submit                      │
└───────────────────────────┬────────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────────┐
│ 5. HAKEDIŞ APPROVAL (APR-PROGRESS)                                 │
│    Step 1: SiteSupervisor (Sequential)                             │
│    Step 2: ProjectManager (Sequential)                             │
│    Step 3: FinanceManager + Admin (ParallelAll)                    │
│    → Status: Approved                                              │
└───────────────────────────┬────────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────────┐
│ 6. INVOICING & COLLECTION                                          │
│    • POST /api/v1/progress-payments/:id/invoice                    │
│    • SYSTEM creates Receivables record                             │
│    • Customer pays → POST /api/v1/collections                      │
│    • POST /api/v1/collections/:id/allocate                         │
│    • Receivable.status → Collected                                 │
└────────────────────────────────────────────────────────────────────┘
```

---

## 6.3 Stock Issue for Work Order Flow

```
WorkOrder created + materials planned
         ↓
SYSTEM checks StockBalances.availableQuantity
         ↓
IF available ≥ planned qty:
  StockReservation created (status=Active)
  WorkOrder.status can proceed to Assigned
         ↓
  WO starts → POST material-usages
         ↓
  SYSTEM: Creates StockDocument [Type=ProjectIssue]
  POST /api/v1/stock-documents/:id/submit (if approval required)
  POST /api/v1/stock-documents/:id/post
  SYSTEM FIFO allocation:
    → Oldest StockLot consumed first
    → StockIssueAllocations records written
    → StockTransactions appended (immutable)
    → StockBalances updated
    → StockReservation.status → Consumed
         ↓
ELSE (insufficient stock):
  Alert generated → Notification to WarehouseManager
  Request automatically created (optional setting)
```

---

## 6.4 Approval Engine Flow

```
Business document submitted (e.g., PurchaseOrder)
         ↓
SERVICE: findApprovalDefinition(entityType='purchase_orders')
         ↓
Select current version (is_current_version=true)
         ↓
Evaluate ApprovalConditions (e.g., TotalAmount > 50000?)
         ↓
SELECT matching version for condition set
         ↓
CREATE ApprovalRequests record (status=Pending)
         ↓
FOR each ApprovalStepDefinition (ordered by step_number):
  CREATE ApprovalRequestSteps (status=Waiting)
         ↓
Activate step 1:
  ApprovalRequestSteps[1].status = Active
  Resolve approvers from ApprovalStepApprovers:
    User → direct user ID
    Role → all users with that role
    ProjectRole → members of the project with that role
    DepartmentManager → manager of entity's department
  Check ApprovalDelegations for each approver (date range active?)
  CREATE ApprovalRequestApprovers (copied snapshot)
  EMIT notification to each approver
         ↓
Approver acts (Approve/Reject/Return/Cancel):
  WRITE ApprovalActions
         ↓
UPDATE ApprovalRequestApprovers.status
         ↓
Check step completion based on ApprovalMode:
  Sequential/ParallelAll → all approvers must approve
  ParallelAny            → one approval sufficient
  Quorum                 → count approved ≥ requiredApprovalCount
         ↓
IF step approved:
  Update step status → Approved
  IF more steps exist → Activate next step (repeat)
  ELSE → ApprovalRequests.status = Approved
         Entity.status = Approved
         EMIT approval-complete event
         ↓
IF any step rejected:
  ApprovalRequests.status = Rejected
  Entity.status = Rejected
  EMIT rejection notification to requester
         ↓
IF returned:
  ApprovalRequests.status = Returned
  Entity.status = Draft (ready for revision)
  EMIT return notification to requester
```

---

# 7. Event-Driven Design

## 7.1 Event Inventory

| Event | Producer | Consumers |
|-------|----------|-----------|
| `approval.submitted` | WorkflowService | NotificationService, AuditService |
| `approval.approved` | WorkflowService | NotificationService, entity Services |
| `approval.rejected` | WorkflowService | NotificationService, entity Services |
| `approval.step_activated` | WorkflowService | NotificationService |
| `stock.document_posted` | InventoryService | StockBalanceService, CostService |
| `stock.balance_low` | StockBalanceService | NotificationService |
| `stock.reservation_expired` | SchedulerService | StockBalanceService |
| `po.approved` | WorkflowService | ProcurementService (notify supplier) |
| `receipt.completed` | ProcurementService | InventoryService (auto stock-in) |
| `invoice.matched` | ProcurementService | FinanceService (create payable) |
| `payable.overdue` | SchedulerService | NotificationService, FinanceService |
| `receivable.overdue` | SchedulerService | NotificationService |
| `budget.threshold_exceeded` | BudgetService | NotificationService |
| `progress_payment.approved` | WorkflowService | FinanceService (create receivable) |
| `measurement_sheet.approved` | FieldOpService | ProgressPaymentService |
| `user.created` | IAMService | NotificationService (welcome email) |
| `document.uploaded` | DocumentService | NotificationService (stakeholders) |

---

## 7.2 Queue Architecture

```
Redis Streams (via BullMQ)

Queues:
┌─────────────────────────────────────────────────────────┐
│ approval-engine                                          │
│   Jobs: evaluate-step, advance-step, check-timeout      │
│   Concurrency: 5                                        │
│   Retry: 3 attempts, exponential backoff (1s, 5s, 30s) │
│   DLQ: approval-engine-failed                           │
│                                                         │
│ stock-recalc                                            │
│   Jobs: recalculate-balance, expire-reservations        │
│   Concurrency: 3                                        │
│   Retry: 5 attempts                                     │
│                                                         │
│ notification-dispatch                                   │
│   Jobs: send-inapp, send-email, send-sms                │
│   Concurrency: 20                                       │
│   Retry: 3 attempts                                     │
│   DLQ: notification-failed (with reason)                │
│                                                         │
│ sequence-generation                                     │
│   Jobs: get-next-sequence                               │
│   Concurrency: 1  ← MUST be 1 for serial counter inc   │
│                                                         │
│ report-generation                                       │
│   Jobs: generate-report, export-pdf                     │
│   Concurrency: 2                                        │
│                                                         │
│ scheduled-jobs (BullMQ cron)                            │
│   expire-reservations: every 5 minutes                  │
│   check-overdue-payables: daily 09:00                  │
│   check-overdue-receivables: daily 09:00               │
│   budget-variance-check: daily 08:00                   │
│   approval-timeout-check: every 30 minutes              │
└─────────────────────────────────────────────────────────┘
```

## 7.3 Event Payload Standards

All events carry:
```typescript
interface BaseEvent {
  eventId: string;        // UUID
  eventType: string;      // 'approval.approved'
  occurredAt: string;     // ISO 8601
  correlationId: string;  // request tracing ID
  userId: string | null;  // actor (null for system events)
  companyId: string;
  payload: Record<string, unknown>;
}
```

### Example: `approval.approved`
```json
{
  "eventId": "uuid",
  "eventType": "approval.approved",
  "occurredAt": "2026-06-18T14:35:00Z",
  "correlationId": "req_xyz",
  "userId": "uuid-approver",
  "companyId": "uuid",
  "payload": {
    "approvalRequestId": "uuid",
    "definitionCode": "APR-PURCHASE",
    "relatedEntityType": "purchase_orders",
    "relatedEntityId": "uuid-po",
    "stepNumber": 2
  }
}
```

---

# 8. Security Architecture

## 8.1 Authentication Flow

```
1. POST /api/auth/login
   → Validate credentials
   → Check is_locked, is_active
   → On failure: increment failed_login_count
     If failed_login_count ≥ 5 → set is_locked = true, notify admin
   → On success: reset failed_login_count
     Generate accessToken (JWT, 15min, RS256)
     Generate refreshToken (crypto.randomBytes(64), stored as SHA-256 hash)
     Set-Cookie: refreshToken=<token>; HttpOnly; Secure; SameSite=Strict; Path=/api/auth
     Return: { accessToken, expiresIn, user }

2. Every API request:
   → Extract Bearer token from Authorization header
   → Verify JWT signature (RS256 public key)
   → Check exp claim
   → Extract userId, companyId, permissions[] from payload
   → Check permission for endpoint (middleware)
   → If fail → 401 or 403

3. Token refresh (POST /api/auth/refresh):
   → Read refreshToken from HttpOnly cookie
   → SHA-256 hash it
   → Look up in refresh_tokens table (WHERE token_hash = ? AND expires_at > now() AND revoked_at IS NULL)
   → Issue new accessToken
   → Rotate refreshToken (revoke old, create new)

4. Logout:
   → Set refresh_tokens.revoked_at = now()
   → Clear cookie
```

## 8.2 JWT Payload Structure

```json
{
  "sub": "user-uuid",
  "iat": 1718710000,
  "exp": 1718710900,
  "jti": "token-uuid",
  "companyId": "uuid",
  "branchId": "uuid",
  "roles": ["ProjectManager"],
  "permissions": ["Projects.Read", "WorkOrders.Create"]
}
```

## 8.3 Permission Evaluation Algorithm

```typescript
function hasPermission(userId: string, permissionCode: string): boolean {
  // 1. Check user-level deny override (highest priority)
  const userDeny = userPermissions.find(
    p => p.userId === userId && p.permissionCode === permissionCode
       && p.isGranted === false && isWithinValidity(p)
  );
  if (userDeny) return false;

  // 2. Check user-level grant override
  const userGrant = userPermissions.find(
    p => p.userId === userId && p.permissionCode === permissionCode
       && p.isGranted === true && isWithinValidity(p)
  );
  if (userGrant) return true;

  // 3. Check via roles
  const userRoles = getUserRoles(userId).filter(r => isWithinValidity(r));
  const hasViaRole = userRoles.some(ur =>
    rolePermissions.some(rp => rp.roleId === ur.roleId && rp.permissionCode === permissionCode)
  );
  return hasViaRole;
}
```

**Cache:** Permission sets are cached in Redis per user with a 5-minute TTL. On role/permission change, the cache key is invalidated immediately.

## 8.4 Authorization Middleware

```typescript
// Usage in routes:
router.post('/purchase-orders', requirePermission('Procurement.PurchaseOrder.Create'), handler);
router.get('/purchase-orders', requirePermission('Procurement.PurchaseOrder.Read'), handler);

// Multi-company isolation:
router.use(companyIsolationMiddleware); // Injects WHERE company_id = :userCompanyId on all queries
```

## 8.5 Rate Limiting

| Endpoint Group | Limit | Window |
|----------------|-------|--------|
| `POST /api/auth/login` | 10 requests | 15 minutes per IP |
| `POST /api/auth/refresh` | 30 requests | 1 hour per user |
| All authenticated APIs | 1000 requests | 1 minute per user |
| File upload endpoints | 20 requests | 1 hour per user |
| Report generation | 5 concurrent | Per user |

Implementation: Redis sliding window counter.

## 8.6 Data Security

| Concern | Implementation |
|---------|---------------|
| Passwords | bcrypt, cost factor 12 |
| Secrets (API keys, SMTP) | Environment variables, never in DB |
| PII at rest | PostgreSQL transparent data encryption (TDE) or column-level encryption for sensitive fields |
| File storage | S3 presigned URLs (15min expiry), never direct public access |
| Transport | TLS 1.3 enforced, HSTS header |
| SQL injection | Parameterized queries via Drizzle ORM |
| XSS | Helmet.js headers, Content-Security-Policy |
| CSRF | SameSite=Strict cookie + CSRF token for forms |

---

# 9. Observability & Monitoring

## 9.1 Logging Strategy

**Library:** Pino (structured JSON logging)

**Log levels:**
- `error`: Uncaught exceptions, business logic failures
- `warn`: Validation failures, rate limit approaching, slow queries
- `info`: Request/response lifecycle, business events
- `debug`: Detailed flow traces (dev/staging only)

**Structured log format:**
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
  "msg": "PurchaseOrder created"
}
```

**Sensitive data:** Never log passwords, tokens, payment card data, or full PII. Log IDs only.

## 9.2 Metrics (Prometheus)

| Metric | Type | Labels |
|--------|------|--------|
| `http_requests_total` | Counter | method, path, status_code |
| `http_request_duration_seconds` | Histogram | method, path |
| `db_query_duration_seconds` | Histogram | query_type, table |
| `queue_job_duration_seconds` | Histogram | queue_name, job_type |
| `queue_job_failures_total` | Counter | queue_name, job_type |
| `approval_requests_total` | Counter | definition_code, outcome |
| `stock_documents_posted_total` | Counter | document_type |
| `active_websocket_connections` | Gauge | — |
| `cache_hits_total` / `cache_misses_total` | Counter | cache_key_prefix |

## 9.3 Distributed Tracing

OpenTelemetry with automatic instrumentation for Express, PostgreSQL (pg), Redis, and BullMQ.

Every request gets a `traceId` propagated through:
- HTTP headers (`traceparent`)
- BullMQ job data
- Logs (`correlationId` = `traceId`)
- Database audit fields (`request_path` + request context)

## 9.4 Alerting Rules

| Alert | Condition | Severity |
|-------|-----------|----------|
| High error rate | HTTP 5xx > 1% over 5 min | Critical |
| High latency | P99 > 2s over 5 min | Warning |
| DB connection exhaustion | Pool utilization > 80% | Warning |
| DB connection exhaustion | Pool utilization > 95% | Critical |
| Queue depth | Any queue depth > 1000 for 10min | Warning |
| Job failure spike | > 10 failures in 5 min | Critical |
| Disk space | > 80% | Warning |
| Approval timeout | Step pending > TimeoutHours | Warning |
| Overdue payables | Amount > configured threshold | Info |
| Stock below min | Any material below min_stock_level | Warning |

## 9.5 Health Check Endpoints

```
GET /healthz         → 200 if server is running (load balancer probe)
GET /readyz           → 200 if DB + Redis connected (readiness probe)
GET /api/metrics     → Prometheus metrics (internal only)
```

---

# 10. Infrastructure & Deployment

## 10.1 Environment Architecture

```
┌──────────────────┬──────────────────┬────────────────────┐
│   Development    │     Staging       │    Production       │
├──────────────────┼──────────────────┼────────────────────┤
│ Docker Compose   │ Kubernetes (1 rep)│ Kubernetes (HA)    │
│ Local PG + Redis │ Managed PG       │ Managed PG + Replica│
│ Hot reload       │ Staging domain   │ Custom domain + CDN │
│ No auth enforced │ Full auth        │ Full auth + WAF     │
│ Seed data        │ Anonymized data  │ Real data          │
└──────────────────┴──────────────────┴────────────────────┘
```

## 10.2 Docker Compose (Development)

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
    environment: *api_env
    depends_on: [db, redis]

volumes:
  pg_data:
```

## 10.3 Kubernetes Production Setup

```yaml
# Deployment: api-server
replicas: 3
resources:
  requests: { cpu: 500m, memory: 512Mi }
  limits:   { cpu: 2000m, memory: 2Gi }
strategy:
  type: RollingUpdate
  maxUnavailable: 0
  maxSurge: 1

# Deployment: worker
replicas: 2
resources:
  requests: { cpu: 250m, memory: 256Mi }
  limits:   { cpu: 1000m, memory: 1Gi }

# HPA (Horizontal Pod Autoscaler)
api:
  minReplicas: 3
  maxReplicas: 10
  metrics:
    - type: Resource
      resource: { name: cpu, target: { type: Utilization, averageUtilization: 70 } }
```

## 10.4 Database Configuration

```sql
-- Connection pooling via PgBouncer
-- pool_mode = transaction
-- max_client_conn = 200
-- default_pool_size = 25

-- PostgreSQL tuning (prod, 8GB RAM)
max_connections = 200
shared_buffers = 2GB
effective_cache_size = 6GB
work_mem = 20MB
maintenance_work_mem = 512MB
wal_level = replica
max_wal_senders = 3
```

**Migration strategy:** Drizzle Kit migrations, applied via CI/CD before deployment, backwards compatible.

## 10.5 CI/CD Pipeline

```yaml
# GitHub Actions
on: push

jobs:
  test:
    - Checkout
    - Install dependencies (pnpm install --frozen-lockfile)
    - Lint (eslint)
    - Type check (tsc --noEmit)
    - Unit tests (vitest)
    - Integration tests (vitest + test DB)

  build:
    needs: test
    - Build (esbuild → dist/)
    - Docker build + push to registry
    - Security scan (trivy image scan)

  deploy-staging:
    needs: build
    if: branch = main
    - kubectl set image (staging namespace)
    - Run DB migrations (kubectl exec)
    - Health check (wait for /readyz)
    - Smoke tests

  deploy-production:
    needs: deploy-staging
    if: branch = main && manual approval
    - kubectl set image (production namespace)
    - Run DB migrations
    - Health check
    - Monitor error rate for 10 minutes
    - Auto-rollback if error rate > 2%
```

## 10.6 Backup & Disaster Recovery

| Component | Strategy | RTO | RPO |
|-----------|----------|-----|-----|
| PostgreSQL | Daily full + continuous WAL archiving (S3) | 2h | 5min |
| Redis | RDB snapshots every 15min | 30min | 15min |
| Object Storage (S3/R2) | Cross-region replication | — | near-zero |
| Application config | Git (IaC) | 30min | — |

**Backup retention:** 7 daily, 4 weekly, 12 monthly.

**Restore procedure:**
1. Provision new DB instance from latest snapshot
2. Replay WAL to target point-in-time
3. Verify integrity with checksum
4. Update connection string in K8s secret
5. Restart pods

## 10.7 Scaling Strategy

| Load Level | Strategy |
|-----------|----------|
| Normal | 3 API pods, 2 worker pods |
| Elevated (>500 RPS) | HPA scales API to 10 pods |
| Heavy reports | Dedicated report worker pod, separate queue |
| DB read-heavy | Read replica for GET /reports, /dashboard |
| Spike mitigation | Rate limiting + queue depth throttling |

---

# 11. Final Production-Ready Checklist

## 11.1 Database

- [x] All tables with FK indexes
- [x] Composite indexes on (status, is_deleted) columns
- [x] Audit log partitioned by month
- [x] stock_transactions immutable (no UPDATE/DELETE grants)
- [x] sequence_definitions counter incremented with SELECT FOR UPDATE
- [x] DB migrations run before deployment, never during
- [x] Connection pooling (PgBouncer) configured
- [x] Read replica for reporting queries

## 11.2 API

- [x] OpenAPI 3.1 spec generated from code
- [x] All endpoints require explicit permission check
- [x] Multi-company isolation middleware on all routes
- [x] Request ID generated and propagated
- [x] All inputs validated via Zod before reaching service layer
- [x] Business rule errors return 422 with structured body
- [x] Idempotency key support on payment/collection endpoints
- [x] Pagination enforced (max pageSize=100)
- [x] File uploads: size limit, mime-type validation, virus scan (ClamAV or SaaS)

## 11.3 Workflow Engine

- [x] Approver snapshot taken at request time (immutable)
- [x] Only one active version per approval definition
- [x] Delegation check before every notification dispatch
- [x] Timeout handling via scheduled job
- [x] All state transitions atomic (transaction + event)
- [x] Rejection notifies requester within 60 seconds

## 11.4 Stock / Finance

- [x] FIFO allocation in single transaction with row-level locks
- [x] stock_balances updated atomically with stock_transactions
- [x] 3-way matching tolerance configurable via system_settings
- [x] Overdue payment job runs daily with idempotent status update
- [x] Currency conversion uses rate for transaction date (not today)

## 11.5 Security

- [x] JWT signed with RS256 (asymmetric keys)
- [x] Refresh token rotation on every use
- [x] Account lockout after 5 failed logins
- [x] Password policy: min 8 chars, upper, lower, digit, special
- [x] Rate limiting on auth endpoints
- [x] All file URLs are presigned (not public)
- [x] HTTPS enforced, HSTS header
- [x] Dependencies audited on every CI run (pnpm audit)

## 11.6 Observability

- [x] Structured JSON logs with requestId
- [x] Prometheus metrics endpoint
- [x] OpenTelemetry traces
- [x] Alerting rules configured for error rate, latency, queue depth
- [x] Health check endpoints (/healthz, /readyz)
- [x] Runbook links in alert definitions

## 11.7 Operations

- [x] Zero-downtime deployment (RollingUpdate, maxUnavailable=0)
- [x] Auto-rollback on error spike post-deploy
- [x] Database migration is backwards compatible (column add before column remove)
- [x] Graceful shutdown (drain in-flight requests before SIGTERM)
- [x] Secret rotation procedure documented

---

*End of Energy Production-Ready System Design Document — v2.0*  
*Generated: June 2026*
