# 🎲 Tombola Management System

The **Tombola Management System** is a web application built with **ASP.NET Core 8** that allows you to manage players, awards, and tombolas.  
It provides a clean API for managing entities and simulating a Tombola-style game.  
The system is containerized with **Docker** and can be easily started using `docker-compose`.

---

The project uses:
- **PostgreSQL** – relational database for storing application data  
- **RabbitMQ** – message broker for event-driven communication  
- **Unit & Integration Tests** – ensure correctness and reliability of the system    

- **API with Swagger UI**  
  - Easy testing of endpoints directly from the browser
 
---

## 🚀 Features
- **Player Management (CRUD)**  
  - Create, Read, Update, Delete players  

- **Award Management (CRUD)**  
  - Create, Read, Update, Delete awards  

- **Tombola Management**  
  - Standard CRUD operations (create tombola, list all, update, delete)  
  - Add players to a tombola  
  - Add awards to a tombola  
  - Draw players randomly to assign awards

---

## 📖 Example API Endpoints

### 🎮 Player Management (CRUD)

- `GET /api/players` – Get all players  
- `GET /api/players/{id}` – Get player by ID  
- `POST /api/players` – Create a new player  
- `PUT /api/players/{id}` – Update existing player  
- `DELETE /api/players/{id}` – Delete player  

---

### 🏆 Award Management (CRUD)

- `GET /api/awards` – Get all awards  
- `GET /api/awards/{id}` – Get award by ID  
- `POST /api/awards` – Create a new award  
- `PUT /api/awards/{id}` – Update existing award  
- `DELETE /api/awards/{id}` – Delete award  

---

### 🎲 Tombola Management

- `GET /api/tombola` – Get all tombolas  
- `GET /api/tombola/{id}` – Get tombola by ID  
- `POST /api/tombola` – Create new tombola  
- `PUT /api/tombola/{id}` – Update tombola  
- `DELETE /api/tombola/{id}` – Delete tombola  

#### Tombola Actions
- `POST /api/tombola/{id}/add-player/{playerId}` – Add player to tombola  
- `POST /api/tombola/{id}/add-award/{awardId}` – Add award to tombola  
- `POST /api/tombola/{id}/draw` – Perform draw (randomly assign awards to players) 
