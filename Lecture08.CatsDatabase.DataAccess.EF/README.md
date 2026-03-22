#### 1. Установить пакеты:
```powershell
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Описание:
- Npgsql.EntityFrameworkCore.PostgreSQL — добавляет поддержку PostgreSQL для Entity Framework Core.
- Microsoft.EntityFrameworkCore.Design — нужен для работы команд dotnet ef.

#### 2. Установить инструмент dotnet-ef:
```powershell
dotnet tool install --global dotnet-ef
```

Описание:
- dotnet-ef — это инструмент для работы с Entity Framework Core из терминала.
- Он нужен для генерации моделей, DbContext и других команд EF Core.

#### 3. Сгенерировать модели и DbContext для существующей базы данных:
```powershell
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=cats_db_local;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -o Entities -c CatsDbContext --context-dir .
```
Описание:
- scaffold считывает структуру существующей базы данных и автоматически создаёт классы сущностей и DbContext.
- -o Entities - сохраняет модели в папку Entities.
- -c CatsDbContext - задаёт имя класса контекста базы данных.
- --context-dir . - сохраняет файл DbContext в текущую папку.

Результат:
- в папке Entities появятся классы сущностей;
- в текущей папке будет создан файл CatsDbContext.