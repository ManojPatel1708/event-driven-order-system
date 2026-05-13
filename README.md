# 🚀 Event Driven Order System (.NET + Kafka)

This project demonstrates a **production-style event-driven microservices architecture** using **.NET and Apache Kafka**.

⚠️ Note: Initially implemented using Kafka KRaft mode (no Zookeeper), but switched to Zookeeper-based Kafka for stability during local development.

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
- Apache Kafka
- Zookeeper
- Docker
- Confluent.Kafka

---

## 📁 Project Structure

event-driven-order-system/
├── OrderService/
├── PaymentService/
├── NotificationService/
├── docker-compose.yml
└── README.md

---

## ⚙️ Prerequisites

- .NET SDK 6+
- Docker Desktop

---

## 🐳 Step 1: Start Kafka

docker-compose up -d

---

## ✅ Verify Kafka

docker ps

You should see:
kafka  
zookeeper  

---

## 🧱 Step 2: Create Topics

docker exec -i kafka bash -c "kafka-topics --create --topic order-created --bootstrap-server localhost:9092 --if-not-exists && kafka-topics --create --topic payment-success --bootstrap-server localhost:9092 --if-not-exists && kafka-topics --create --topic payment-retry --bootstrap-server localhost:9092 --if-not-exists && kafka-topics --create --topic payment-dlq --bootstrap-server localhost:9092 --if-not-exists"

---

## ✅ Verify Topics

docker exec -it kafka kafka-topics --list --bootstrap-server localhost:9092

Expected:
order-created  
payment-success  
payment-retry  
payment-dlq  

---

## 🔍 Debug Kafka Messages

docker exec -it kafka kafka-console-consumer --topic order-created --from-beginning --bootstrap-server localhost:9092

---

## 🏗️ Build Services

cd OrderService && dotnet build  
cd ../PaymentService && dotnet build  
cd ../NotificationService && dotnet build  

---

## ▶️ Run Services (Important Order)

Terminal 1:
cd NotificationService  
dotnet run  

Terminal 2:
cd PaymentService  
dotnet run  

Terminal 3:
cd OrderService  
dotnet run  

---

## 🎯 Expected Output

OrderService:
✅ Order Created

PaymentService:
📩 Received Order  
💳 Processing payment  
✅ Payment success  

NotificationService:
📧 Sending notification  

---

## 🔄 Retry & DLQ

Kafka topics used:

- order-created
- payment-success
- payment-retry
- payment-dlq

---

## ⚠️ Common Issues

### No messages in consumer

- Ensure topics exist
- Start consumers BEFORE producer
- Use new consumer GroupId

### Kafka shows 0 messages

- Inside container → use kafka:9092  
- Outside → use localhost:9092  

### KRaft setup failed

- Listener misconfiguration
- Topic not persisted
- Switching to Zookeeper resolved stability issues

---

## 🧠 Key Learnings

- Kafka topics must exist before producing
- Consumer groups control message consumption
- Docker networking differs from host
- Kafka listener configuration is critical

---

## 💡 Use Cases

- Order processing systems
- Payment pipelines
- Event-driven microservices
- Real-time processing

---

## 🚀 Future Enhancements

- MongoDB persistence
- Logging (Serilog)
- Retry backoff strategy
- Dockerized microservices
- Kubernetes deployment

---

## 👨‍💻 Author

Manoj Patel  
Engineering Manager | Solution Architect  

GitHub: <https://github.com/ManojPatel1708>  
LinkedIn: <https://linkedin.com/in/manojcse2007>  
