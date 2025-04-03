# Synthetic Arbitration Project

## Запуск проекта  
Весь проект можно запустить через контейнер с помощью команды:  
```sh
docker compose up -d --build
```

## Доступ к сервисам  
После успешного запуска контейнеров, сервисы будут доступны по следующим адресам:

- **API**: [`http://localhost:8081/synthetic-arbitration-api`](http://localhost:8081/synthetic-arbitration-api)  
- **Grafana**: [`http://localhost:8081/grafana`](http://localhost:8081/grafana)  
- **Loki**: [`http://localhost:3100`](http://localhost:3100)  
- **Promtail**: [`http://localhost:9080`](http://localhost:9080)  
- **PostgreSQL**: `localhost:5431` (можно подключиться через клиент, например, DBeaver)
- 

## Дополнительно  
- Все сервисы работают на порту **8081** вместо **80**, чтобы избежать проблем с правами доступа.  
