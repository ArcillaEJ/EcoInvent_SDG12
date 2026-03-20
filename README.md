# EcoInvent: Smart Inventory and Waste Tracker System

### 🌱 Aligned with UN SDG 12: Responsible Consumption and Production

---

## 📌 Project Overview

EcoInvent is a desktop-based C# Windows Forms application designed to manage inventory efficiently while promoting sustainable resource usage. The system allows users to monitor stock levels, manage resources, and generate sustainability reports to reduce waste and improve operational efficiency.

---

## 🎯 SDG Alignment

This system supports **United Nations Sustainable Development Goal 12 (Responsible Consumption and Production)** by:

* Monitoring resource usage and inventory levels
* Preventing overstocking and shortages
* Providing sustainability insights through reports and data visualization

---

## ⚙️ Features

* Secure login system with password hashing
* Role-based access (Administrator and Viewer)
* Full CRUD operations for inventory management
* Resource categorization and stock monitoring
* Dashboard with graphical charts
* Sustainability reporting module
* JSON backup and restore functionality
* Async/Await for improved performance
* Logging and exception handling

---

## 🏗️ System Architecture

The system follows an **N-Tier Architecture**:

* **Presentation Layer (UI)** – Handles user interaction
* **Business Logic Layer (BLL)** – Processes system rules and validation
* **Data Access Layer (DAL)** – Manages database operations
* **Models Layer** – Represents system entities

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
git clone https://github.com/ArcillaEJ/EcoInvent_SDG12.git
```

2. Open the solution file:

```
EcoInventApp.sln
```

3. Run the application using Visual Studio.

---

## 👥 Contributors

* Arcilla, EJ
* Berdejo, Justine Marlowie
* Bitara, Peter John
* Guico, Karlo Emanuel
* King, Allen Marlon

---

## 📁 Project Structure

```
EcoInvent_SDG12
├── EcoInvent.BLL/
├── EcoInvent.DAL/
├── EcoInvent.Models/
├── EcoInvent.UI/
├── EcoInventApp/
├── INPUT_DATA/
│   └── inventory.db
├── DOCUMENTATION/
│   └── Database_Schema_ERD.png
│   ├── Flowchart_CoreAlgorithm.png
│   ├── Flowchart_CoreAlgorithm1.png
│   ├── Flowchart_CoreAlgorithm2.png
│   ├── SDAD_EcoInvent.pdf
├── EcoInventApp.sln
├── README.md
```

---

## 📌 Data Source

The system uses a **persistent SQLite database** managed through Entity Framework Core.
All data is stored and updated dynamically within the application instead of using static input files such as JSON or CSV.

---

## 📌 Conclusion

EcoInvent demonstrates how software engineering principles can be applied to real-world sustainability challenges. The system promotes responsible consumption by combining efficient inventory management with data-driven insights aligned with SDG 12.

---
