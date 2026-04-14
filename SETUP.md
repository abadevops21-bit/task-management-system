# ⚙️ Setup Guide

This project uses Docker images hosted on Docker Hub.

---

## 🔐 Docker Login (if needed)

```bash
docker login
```

---

## 🐳 Pull Images Manually

```bash
docker pull abadevops21/taskmanagement-api:latest
docker pull abadevops21/taskmanagement-client:latest
```

---

## ▶️ Run Services Individually

### SQL Server

```bash
docker run -d -p 1433:1433 \
-e SA_PASSWORD=YourStrong@Pass123 \
-e ACCEPT_EULA=Y \
mcr.microsoft.com/mssql/server:2022-latest
```

---

### API

```bash
docker run -d -p 5000:80 \
-e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=TaskDb;User=sa;Password=YourStrong@Pass123;TrustServerCertificate=True;" \
abadevops21/taskmanagement-api:latest
```

---

### Client

```bash
docker run -d -p 4200:80 \
abadevops21/taskmanagement-client:latest
```

---

## 🧠 Notes

* Ensure SQL Server is running before API
* API must be reachable from client

---
