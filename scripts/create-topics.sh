#!/bin/bash

echo "⏳ Waiting for Kafka to be ready..."

sleep 10

echo "✅ Creating Kafka topics..."

kafka-topics --create --if-not-exists \
  --topic order-created \
  --bootstrap-server kafka:9092 \
  --partitions 1 --replication-factor 1

kafka-topics --create --if-not-exists \
  --topic payment-success \
  --bootstrap-server kafka:9092 \
  --partitions 1 --replication-factor 1

kafka-topics --create --if-not-exists \
  --topic payment-retry \
  --bootstrap-server kafka:9092 \
  --partitions 1 --replication-factor 1

kafka-topics --create --if-not-exists \
  --topic payment-dlq \
  --bootstrap-server kafka:9092 \
  --partitions 1 --replication-factor 1

echo "✅ All topics created successfully!"
