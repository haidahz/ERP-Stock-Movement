# ERP Stock Movement

A modular monolith ERP sample built with **ASP.NET Core 8 Web API**. It demonstrates three bounded modules—**Product**, **Inventory**, and **Order**—each with its own database and HTTP-based communication between modules.

## Architecture

```
┌─────────────┐     HTTP      ┌──────────────┐
│   Order     │──────────────▶│  Inventory   │
│   Module    │               │   Module     │
│ (orders.db) │     HTTP      │(inventory.db)│
└──────┬──────┘──────────────▶└──────────────┘
       │
       │ HTTP
       ▼
┌─────────────┐
│   Product   │
│   Module    │
│(products.db)│
└─────────────┘
```

- **Modular monolith**: one deployable API, clear folder boundaries per module.
- **No shared databases**: each module uses a separate SQLite file.
- **HTTP communication**: the Order module calls Product and Inventory via `HttpClient` (configured in `ModuleCommunication:BaseUrl`).
- **Order flow**: validate products exist → atomically validate and deduct stock → persist order.

### Concurrency

Inventory deductions run inside a single database transaction: stock is validated for all line items before any quantity is reduced, preventing partial deductions when stock is insufficient.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup

Clone the source code from my git (master branch) : https://github.com/haidahz/ERP-Stock-Movement.git

## Run the application

```bash
cd ERP-Stock-Movement
dotnet restore
dotnet run --project ERP-Stock-Movement
```

Use Ctrl+ C when want to shutting down to restart

Swagger UI: **http://localhost:5094/swagger**

Ensure `ModuleCommunication:BaseUrl` in `appsettings.json` matches the HTTP port in `Properties/launchSettings.json` (default `http://localhost:5094`).

## API overview

| Module    | Method | Endpoint                    | Description                          |
|-----------|--------|-----------------------------|--------------------------------------|
| Product   | GET    | `/api/product`              | List all products                    |
| Product   | GET    | `/api/product/{id}`         | Get product by id                    |
| Product   | POST   | `/api/product`              | Create product                       |
| Inventory | GET    | `/api/inventory`            | List all stock records               |
| Inventory | GET    | `/api/inventory/{productId}`| Get stock for a product              |
| Inventory | POST   | `/api/inventory`            | Set stock quantity                   |
| Inventory | POST   | `/api/inventory/add`        | Add to existing stock                |
| Inventory | POST   | `/api/inventory/try-deduct` | Validate and deduct stock (atomic)   |
| Order     | GET    | `/api/order`                | List orders                          |
| Order     | GET    | `/api/order/{id}`           | Get order by id                      |
| Order     | POST   | `/api/order`                | Create order (validates & deducts)   |

## Example workflow

1. **Create a product**

```json
POST /api/product
{
  "name": "Product",
  "price": 19.99
}
```

2. **Set inventory** (use the product `id` from step 1)

```json
POST /api/inventory
{
  "productId": 1,
  "quantity": 100
}
```

3. **Create an order**

```json
POST /api/order
{
  "items": [
    { "productId": 1, "quantity": 5 }
  ]
}
```

4. **Try ordering more than stock** — returns `400` with an insufficient-stock message.

## Project structure

```
ERP-Stock-Movement/
├── Products/          # Product catalog module
├── Inventory/         # Stock management module
├── Orders/            # Order processing module
│   └── Clients/       # HTTP clients for cross-module calls
├── Program.cs
└── appsettings.json
```

## Can delete all the three db to create new data
