# 🚀 Task Management System

A full-stack Task Management application built with:

* ⚙️ .NET 10 Web API
* 🌐 Angular
* 🛢️ SQL Server
* 🐳 Docker (prebuilt images)

---

## 📦 Docker Images

The application is already available on Docker Hub:

* API → `abadevops21/taskmanagement-api:latest`
* Client → `abadevops21/taskmanagement-client:latest`

---

## ⚡ Quick Start (Recommended)

### 1️⃣ Clone the repository

```bash
git clone https://github.com/abadevops21-bit/task-management-system.git
cd task-management-system
```

---

### 2️⃣ Run using Docker Compose

```bash
docker compose up
```

---

### 3️⃣ Access the application

* 🌐 Frontend: http://localhost:4200
* 🔧 API: http://localhost:5000

---

## ⚙️ Configuration

### Database Connection (Already Configured)

```text
Server=sqlserver;Database=TaskDb;User=sa;Password=YourStrong@Pass123;
```

---

## 📌 Features

* 🔐 Authentication (Login/Register)
* ✅ Create, Update, Delete Tasks
* 📋 Task List with Pagination & Filters
* 🔄 Toggle Task Status (Completed / Pending)

---

## 🧪 API Endpoints

### 🔑 Auth

| Method | Endpoint           |
| ------ | ------------------ |
| POST   | /api/auth/register |
| POST   | /api/auth/login    |

---

### 📋 Tasks

| Method | Endpoint               |
| ------ | ---------------------- |
| GET    | /api/tasks             |
| GET    | /api/tasks/{id}        |
| POST   | /api/tasks             |
| PUT    | /api/tasks/{id}        |
| DELETE | /api/tasks/{id}        |
| PATCH  | /api/tasks/{id}/toggle |

---

## 🐳 Notes

* No local setup required
* Docker images are prebuilt
* Just run `docker compose up`

---

## 🚀 Future Enhancements

* Kubernetes Deployment
* CI/CD Pipeline
* Role-based Access Control

---
