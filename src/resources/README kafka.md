# 🛠️ Kafka Local Environment Setup (Windows + Docker)

This guide helps you set up a local **Apache Kafka** environment using **Docker** on a Windows machine. It includes **Zookeeper** and **Kafka Broker**, configured and ready to use.

---

## 📦 Prerequisites

* [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop)

  * Enable **WSL 2 Backend**
  * Use **Linux containers** (default)
* Docker Compose enabled

---

## 📁 Directory Structure

Create a project directory with the following structure:

```
/kafka-local/
  └── docker-compose.yml
```

---

## 📟 docker-compose.yml

```yaml
version: '3.8'
services:
  zookeeper:
    image: confluentinc/cp-zookeeper:7.3.0
    container_name: zookeeper
    ports:
      - "2181:2181"
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000

  kafka:
    image: confluentinc/cp-kafka:7.3.0
    container_name: kafka
    ports:
      - "9092:9092"
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://localhost:9092
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
```

---

## 🚀 Running Kafka Locally

### 1. Start the environment

Open terminal in the `kafka-local/` directory:

```bash
docker-compose up -d
```

### 2. Verify containers

```bash
docker ps
```

You should see `zookeeper` and `kafka` containers running.

---

## 📲 Kafka CLI Tools (Optional)

### Access Kafka container

```bash
docker exec -it kafka bash
```

### Create a topic

```bash
kafka-topics --create \
  --topic permission-operations \
  --bootstrap-server localhost:9092 \
  --replication-factor 1 \
  --partitions 1
```

### List topics

```bash
kafka-topics --list --bootstrap-server localhost:9092
```

---

## 📌 Configuration Summary

| Component | Port | Notes                |
| --------- | ---- | -------------------- |
| Zookeeper | 2181 | Required by Kafka    |
| Kafka     | 9092 | Broker for messaging |

Kafka is now accessible at: `PLAINTEXT://localhost:9092`

---

## 💡 Tips

* Use [Kafdrop](https://github.com/obsidiandynamics/kafdrop) for a web UI (can be added to compose file).
* Ensure Docker has enough memory allocated (at least 2 GB recommended).

---

## 📼 Stopping the Services

```bash
docker-compose down
```

---

## 🔙 Connect from .NET

Use `localhost:9092` in your `Confluent.Kafka` producer/consumer setup.

```csharp
var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};
```

---

## 📃 Additional Resources

* [Kafka Documentation](https://kafka.apache.org/documentation/)
* [Confluent Docker Images](https://hub.docker.com/r/confluentinc/cp-kafka)
