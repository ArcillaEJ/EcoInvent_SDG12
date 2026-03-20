# EcoInvent: Smart Inventory and Waste Tracker System  
### 🌱 Aligned with UN SDG 12: Responsible Consumption and Production

## 📌 Project Overview
EcoInvent is a desktop-based C# Windows Forms application designed to help organizations manage inventory efficiently while promoting sustainable resource usage. The system provides real-time monitoring, reporting, and analysis of inventory data to reduce waste and improve operational efficiency.

This project follows an N-Tier Architecture and applies core software engineering principles such as Object-Oriented Programming, asynchronous processing, and secure data handling.

---

## 🎯 SDG Alignment
This system supports **United Nations Sustainable Development Goal 12 (Responsible Consumption and Production)** by:
- Monitoring resource usage
- Preventing overstocking and waste
- Providing sustainability reports and insights

---

## ⚙️ Features

### 🔐 Authentication & Security
- Hashed login system using secure password hashing (PBKDF2)
- Role-based access (Administrator & Viewer)

### 📦 Inventory Management
- Add, update, delete, and view inventory items
- Categorization of resources
- Stock monitoring and alerts

### 📊 Dashboard & Reports
- Real-time system insights
- Graphical charts (inventory distribution & lifecycle analysis)
- Sustainability reporting module

### 🔄 Data Management
- JSON backup and restore functionality
- Database integration using Entity Framework Core

### ⚡ Performance & Reliability
- Async/Await for smooth UI performance
- Global exception handling
- Logging system for errors and system events

---

## 🏗️ System Architecture
The system follows an **N-Tier Architecture**:

- **Presentation Layer (UI)** – Windows Forms
- **Business Logic Layer (BLL)** – Service classes handling logic
- **Data Access Layer (DAL)** – Repository pattern & database access
- **Models Layer** – Entity classes

---

## 🛠️ Technologies Used
- C# (.NET)
- Windows Forms
- Entity Framework Core
- SQLite Database
- JSON Serialization

---

## 📥 Installation & Usage

1. Clone the repository:
```bash
git clone https://github.com/YOUR_USERNAME/EcoInvent.git
