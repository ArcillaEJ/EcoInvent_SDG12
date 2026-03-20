# EcoInvent: Smart Inventory and Waste Tracker System

### 🌱 Aligned with UN SDG 12: Responsible Consumption and Production

---

## 📌 Project Overview

EcoInvent is a desktop-based C# Windows Forms application designed to help organizations manage inventory efficiently while promoting sustainable resource usage. The system provides real-time monitoring, tracking, and reporting of inventory data to reduce waste and improve operational efficiency.

---

## 🎯 SDG Alignment

This system supports **United Nations Sustainable Development Goal 12 (Responsible Consumption and Production)** by monitoring resource usage, minimizing waste, and providing sustainability insights for better decision-making.

---

## ⚙️ Features

* Secure login system with hashed passwords
* Role-based access (Administrator & Viewer)
* Full CRUD operations for inventory management
* Categorization and stock monitoring
* Dashboard with graphical charts
* Sustainability reporting
* JSON backup and restore
* Async/Await for smooth performance
* Logging and exception handling

---

## 🏗️ System Architecture

The system follows an **N-Tier Architecture**:

* Presentation Layer (UI)
* Business Logic Layer (BLL)
* Data Access Layer (DAL)
* Models Layer

---

## 🛠️ Technologies Used

* C# (.NET Windows Forms)
* Entity Framework Core
* SQLite Database
* JSON Serialization

---

## 📥 Installation & Usage

1. Clone the repository:

```bash
git clone https://github.com/ArcillaEJ/EcoInvent.git
```

2. Open:

```
EcoInvent.sln
```

3. Run the application in Visual Studio.

---

## 👥 Contributors

* Arcilla, EJ
* (Add your group members here)

---

## 📁 Project Structure

```
EcoInvent
├── CODE/
│   ├── EcoInvent.sln
│   ├── EcoInvent.UI/
│   ├── EcoInvent.BLL/
│   ├── EcoInvent.DAL/
│   ├── EcoInvent.Models/
├── INPUT_DATA/
│   ├── initial_seed.json
│   └── inventory.db
├── DOCUMENTATION/
│   ├── SDAD_EcoInvent.pdf
│   ├── Flowchart_CoreAlgorithm.png
│   └── Database_Schema_ERD.png
└── assets/
    ├── login.png
    ├── dashboard.png
    ├── inventory.png
    ├── reports.png
    ├── viewer.png
    └── search.png
```

---

## 📸 System Preview

### 🔐 Login Page

![Login](assets/login.png)

### 🧭 Admin Dashboard

![Dashboard](assets/dashboard.png)

### 📦 Inventory Ledger

![Inventory](assets/inventory.png)

### 🌱 Sustainability Reports

![Reports](assets/reports.png)

### 👤 Viewer Dashboard

![Viewer](assets/viewer.png)

### 🔍 Search & Resource Tracking

![Search](assets/search.png)

---

## 📌 Conclusion

EcoInvent demonstrates how software engineering principles can be applied to real-world sustainability challenges. The system promotes responsible consumption by combining efficient inventory management with data-driven insights.

---
