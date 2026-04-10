# CLI-Based Inventory Management System

A C# console application that implements a command-line interface-based inventory management system using Object-Oriented Programming principles.

---

## Submitted By:

**Niña Jezreen M. Bron**  
**BSIT-3A**


**Bicol University Polangui**  

---

## Features

- **Manage Categories** — Add and view product categories
- **Manage Suppliers** — Add and view suppliers
- **Manage Products** — Add, view, search, update, and delete products
- **Stock Operations** — Restock products and deduct stock
- **Transaction History** — View a log of all inventory transactions
- **Low-Stock Alerts** — See which products are running low
- **Total Inventory Value** — View a breakdown of total stock value

---

## OOP Concepts Used

- Classes and Objects
- Constructors
- Properties
- Encapsulation
- Access Modifiers
- Methods
- Exception Handling

---

## Models

| Model | Description |
|---|---|
| `Product` | Stores product details like name, price, and stock |
| `Category` | Groups products into categories |
| `Supplier` | Stores supplier contact information |
| `User` | Handles login credentials and roles |
| `TransactionRecord` | Logs every inventory action |

---

## Default Login Credentials

| Username | Password | Role |
|---|---|---|
| admin | admin123 | Admin |
| staff | staff123 | Staff |

---

## How to Run

1. Clone the repository:
   ```
   git clone https://github.com/JezreenB/CLI-Based-Inventory-System.git
   ```
2. Open the `.slnx` file in **Visual Studio**
3. Press **Ctrl + F5** to run

---

## Storage

This application uses `List<T>` for in-memory storage. No database is required.

---

## Requirements

- .NET 8.0
- Visual Studio 2022 or later
