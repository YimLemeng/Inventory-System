# Inventory & POS Management System

A desktop-based Point of Sale (POS) and Inventory Management System built using the **C# Windows Forms Application** framework and **SQL Server** for robust data persistence. This application is designed with a clean multi-tier (N-Tier/Layered) architecture containing separated layers for better maintainability and scalability.

---

## 🛠️ Tech Stack & Architecture
* **Frontend:** C# Windows Forms App (.NET Framework 4.7.2)
* **Database:** Microsoft SQL Server
* **Architecture:** Layered Architecture
  * **Entity:** Defines the core data models/objects.
  * **DAL (Data Access Layer):** Handles direct database interactions, queries, and connection mapping.
  * **BLL (Business Logic Layer):** Manages intermediate processing rules between UI and DAL.
  * **Form (UI):** Contains the Windows Forms layouts and desktop user interface components.

---

## 🚀 Features & Modules
The system comes with an integrated Dashboard sidebar supporting several operations:
* **Product Management:** Add, update, view, and track product items.
* **Category Setup:** Organize stock into searchable categories.
* **Stock In & Stock Out:** Manage incoming supplies and outgoing product records.
* **Payment Processing:** Handles retail transaction workflows.
* **Supplier Directory:** Keeps data records of active warehouse suppliers.
* **Reporting:** Visualizes sales, stock statistics, and transaction summaries.
* **User Security:** Secure user login configuration with session management parameters.

---

## ⚙️ Prerequisites & Setup

### 1. Database Configuration
Before running the application, make sure to attach or configure your local SQL Server instance. Update the database properties inside the `App.config` file to match your SQL Server environment credentials:

```xml
<connectionStrings>
    <add name="MyDbConnection" 
         connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=POSDB;User ID=user;Password=YOUR_PASSWORD;TrustServerCertificate=True;" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
