# 🚀 Event Driven Order System (.NET + Kafka)

This project demonstrates a **production-style event-driven microservices architecture** using **.NET and Apache Kafka (KRaft mode - no Zookeeper)**.

---

## 🏗️ Architecture

OrderService → Kafka → PaymentService → Kafka → NotificationService

---

## 🔄 Flow

1. OrderService publishes **Order Created event**
2. PaymentService consumes the event and processes payment
3. PaymentService publishes **Payment Success event**
4. NotificationService consumes the event and sends notification

---

## 🧰 Tech Stack

- .NET 6/7
- Apache Kafka (KRaft mode)
- Docker
- Confluent.Kafka

---

## 📁 Project Structure

event-driven-order-system/

- OrderService/
- PaymentService/
- NotificationService/
- docker-compose.yml
- README.md

---

## ⚙️ Prerequisites

Make sure you have installed:

- .NET SDK (6 or above)
- Docker Desktop
- Git (optional)

---

## 🐳 Step 1: Start Kafka (KRaft Mode - No Zookeeper)

Run:

docker-compose up -d

---

## ✅ Verify Kafka is running

Run:

docker ps

You should see Kafka running on:

localhost:9092

---

## 🏗️ Step 2: Build Projects

Run the following inside each service:

cd OrderService
dotnet build

Repeat for:

cd PaymentService
dotnet build

cd NotificationService
dotnet build

---

## ▶️ Step 3: Run Services (3 Terminals)

Terminal 1 → Notification Service

cd NotificationService
dotnet run

---

Terminal 2 → Payment Service

cd PaymentService
dotnet run

---

Terminal 3 → Order Service

cd OrderService
dotnet run

---

## 🎯 Expected Output

You will see logs like:

✅ Order Created: ORD-12345  
💳 Processing payment for Order: ORD-12345  
✅ Payment Successful  
📧 Sending notification for Order: ORD-12345  

---

## 🧠 Key Concepts Demonstrated

- Event-driven architecture  
- Producer / Consumer pattern  
- Microservices communication via Kafka  
- Decoupled system design  
- Real-time data processing  

---

## 💡 Real-World Use Cases

- E-commerce order processing systems  
- Payment processing pipelines  
- Real-time analytics platforms  
- Distributed system communication  

---

## 🚀 Future Enhancements

- Retry mechanism  
- Dead Letter Queue (DLQ)  
- MongoDB persistence  
- Centralized logging (Serilog)  
- Full Dockerized microservices  

---

## 👨‍💻 Author

Manoj Patel  
Engineering Manager | Solution Architect  

GitHub: <https://github.com/ManojPatel1708>  
LinkedIn: <https://linkedin.com/in/manojcse2007>  
