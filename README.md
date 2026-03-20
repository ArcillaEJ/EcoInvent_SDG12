# EcoInvent: Smart Inventory and Waste Tracker System

### 🌱 Aligned with UN SDG 12: Responsible Consumption and Production

---

## 📌 Project Overview

EcoInvent is a desktop-based C# Windows Forms application designed to help organizations manage inventory efficiently while promoting sustainable resource usage. The system provides real-time monitoring, reporting, and analysis of inventory data to reduce waste and improve operational efficiency.

This project follows an N-Tier Architecture and applies core software engineering principles such as Object-Oriented Programming, asynchronous processing, and secure data handling.

---

## 🎯 SDG Alignment

This system supports **United Nations Sustainable Development Goal 12 (Responsible Consumption and Production)** by:

* Monitoring resource usage and consumption
* Reducing unnecessary waste through proper inventory tracking
* Providing sustainability reports and insights for better decision-making

---

## ⚙️ Features

### 🔐 Authentication & Security

* Secure login system using **hashed passwords (PBKDF2)**
* Role-based access:

  * **Administrator** – Full system control
  * **Viewer** – Read-only access

---

### 📦 Inventory Management

* Add, update, delete, and view inventory items (CRUD)
* Categorization of resources (e.g., Kitchen, Greenery, Eco Supplies)
* Stock monitoring and status tracking (OK, Low Stock, etc.)

---

### 📊 Dashboard & Reporting

* System Insight Dashboard for quick overview
* Graphical charts:

  * Inventory distribution (Pie/Donut Chart)
  * Resource lifecycle analysis (Bar Chart)
* Sustainability Reporting module

---

### 🔄 Data Management

* JSON Backup and Restore functionality
* Database integration using **Entity Framework Core**
* SQLite database for lightweight storage

---

### ⚡ Performance & Reliability

* Async/Await implementation for smooth UI experience
* Global exception handling
* Logging system for tracking errors and events

---

## 🏗️ System Architecture

EcoInvent follows a **3-Layer N-Tier Architecture**:

* **Presentation Layer (UI)**

  * Windows Forms interface
  * Handles user interaction

* **Business Logic Layer (BLL)**

  * Processes system rules
  * Handles validation and calculations

* **Data Access Layer (DAL)**

  * Manages database operations
  * Uses repositories and EF Core

* **Models Layer**

  * Represents system entities (User, Item, Category)

---

## 🛠️ Technologies Used

* C# (.NET Windows Forms)
* Entity Framework Core
* SQLite Database
* JSON Serialization
* OOP Principles (Encapsulation, Interfaces, Polymorphism)

---

## 📥 Installation & Usage

1. Clone the repository:

```bash
git clone https://github.com/ArcillaEJ/EcoInvent.git
```

2. Open the solution file in Visual Studio:

```
EcoInvent.sln
```

3. Run the application.

---

## 👥 Contributors

* **Arcilla, EJ** – UI Design, Business Logic, System Integration
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
│   └── appsettings.json
├── INPUT_DATA/
│   ├── initial_seed.json
│   └── inventory.db
└── DOCUMENTATION/
    ├── SDAD_EcoInvent.pdf
    ├── Flowchart_CoreAlgorithm.png
    └── Database_Schema_ERD.png
```

---

## 📸 Screenshots

(Add your application screenshots here)

---

## 📌 Key Highlights

* Implements full **CRUD operations**
* Uses **secure password hashing**
* Follows **N-Tier Architecture**
* Supports **data visualization and reporting**
* Includes **JSON backup and restore**
* Designed for **sustainability and resource efficiency**

---

## 📌 Conclusion

EcoInvent demonstrates how software engineering principles can be applied to real-world sustainability challenges. By combining efficient inventory management with data-driven insights, the system promotes responsible consumption and supports sustainable practices aligned with global development goals.

---

## 📜 License

This project is for academic purposes only.
