# Task Management System

REST API для управления задачами.

**Стек:** .NET 9, ASP.NET Core, EF Core (Code First), PostgreSQL 17

## Инструкция по запуску

### Docker:

> Требуется установленный [Docker Desktop](https://www.docker.com/products/docker-desktop)

Откройте терминал в папке, где находится файл `docker-compose.yml`, и выполните команду:

```bash
docker-compose up --build
```

### После запуска:
Открыть в браузере API: [http://localhost:8080/swagger](http://localhost:8080/swagger)

Для удобства проверки добавил пару пользователей и задач.

**Добавленные пользователи (их ID):**
- `f1811537-a05b-49bb-bee9-7a9480267c12`
- `f67b8dc6-0bee-4732-85fc-ff31a90615ad`

## Реализовано:
- CRUD для задач
- Назначение задач пользователям
- Фильтрация, сортировка и пагинация
- Изменение статуса задачи
- Мягкое удаление
- Валидация данных (FluentValidation)
- Swagger-документация
- Логирование (Serilog)
- Оптимизация запросов
- Защита от конфликтов (RowVersion)

## Структура проекта

```bash
TaskManagementSystem/
├── API/             # Web API (контроллеры, middleware, Swagger)
├── Application/     # Сервисы, DTO, валидация
├── Domain/          # Сущности, enum'ы
├── Infrastructure/  # EF Core, PostgreSQL, миграции, репозиторий
├── Tests/           # xUnit + Moq
└── docker-compose.yml
```

