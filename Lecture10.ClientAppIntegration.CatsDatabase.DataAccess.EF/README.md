# Подходы работы с Entity Framework Core

В Entity Framework Core обычно используют два основных подхода:

- **Database First** - сначала создаётся база данных, а затем по её структуре генерируются классы сущностей и `DbContext`.
- **Code First** - сначала пишутся классы моделей и `DbContext` в коде, а затем по ним создаётся или изменяется база данных через миграции.

## Подготовка

### Установить пакеты
```powershell
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Описание:

- `Npgsql.EntityFrameworkCore.PostgreSQL` - добавляет поддержку PostgreSQL для Entity Framework Core.
- `Microsoft.EntityFrameworkCore.Design` - нужен для работы команд `dotnet ef`.

### Установить инструмент `dotnet-ef`
```powershell
dotnet tool install --global dotnet-ef
```

Описание:

- `dotnet-ef` - инструмент для работы с Entity Framework Core из терминала.
- Он нужен для генерации моделей, `DbContext`, создания миграций и обновления базы данных.


---

## 1. Database First

### Сгенерировать модели и `DbContext` для существующей базы данных
```powershell
dotnet ef dbcontext scaffold "Host=127.0.0.1;Port=5432;Database=cats_db_local;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -o Entities -c CatsDbContext --context-dir .
```

Описание:

- `scaffold` считывает структуру **уже существующей** базы данных и автоматически создаёт классы сущностей и `DbContext`.
- `-o Entities` - сохраняет модели в папку `Entities`.
- `-c CatsDbContext` - задаёт имя класса контекста базы данных.
- `--context-dir .` - сохраняет файл `DbContext` в текущую папку.

Результат:

- в папке `Entities` появятся классы сущностей;
- в текущей папке будет создан файл `CatsDbContext`.

### Почему это Database First

Этот подход называется **Database First**, потому что:

- база данных уже существует заранее;
- код приложения строится на основе структуры таблиц из базы;
- классы сущностей и `DbContext` генерируются автоматически из БД.

То есть направление такое:

**База данных → C#-классы**

---

## 2. Code First

В подходе **Code First** разработчик сначала создаёт классы моделей и `DbContext` в коде, а затем Entity Framework Core формирует изменения для базы данных через миграции.

То есть направление здесь обратное:

**C#-классы → База данных**

### Как выглядит Code First

1. Создаются классы сущностей вручную.
2. Создаётся класс `DbContext`.
3. На основе изменений в коде создаются миграции.
4. Миграции применяются к базе данных.

Пример сущности:

```csharp
public class Cat
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
}
```

Пример `DbContext`:

```csharp
using Microsoft.EntityFrameworkCore;

public class CatsDbContext : DbContext
{
    public CatsDbContext(DbContextOptions<CatsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cat> Cats { get; set; } = null!;
}
```

---

### Миграции в Code First

После создания или изменения моделей используются миграции.

#### Создать первую миграцию
```powershell
dotnet ef migrations add InitialScaffold --startup-project ..\Lecture10.ClientAppIntegration.CatsApi\Lecture10.ClientAppIntegration.CatsApi.csproj
```

Описание:

- `migrations add` создаёт новую миграцию на основе текущего состояния моделей и `DbContext`;
- `InitialScaffold` - имя миграции;
- `--startup-project` указывает стартовый проект, из которого будут читаться настройки приложения, например строка подключения и конфигурация DI.

Обычно первая миграция фиксирует начальную структуру базы данных.

#### Создать следующую миграцию после изменения моделей
```powershell
dotnet ef migrations add AddUserAddress --startup-project ..\Lecture10.ClientAppIntegration.CatsApi\Lecture10.ClientAppIntegration.CatsApi.csproj
```

Описание:

- эта команда создаёт ещё одну миграцию;
- она используется после того, как в код были внесены изменения, например добавлено новое свойство, новая сущность или связь;
- `AddUserAddress` - это имя миграции, которое обычно отражает сделанное изменение.

Например, если в модель пользователя добавили адрес, EF Core сформирует SQL-изменения для обновления структуры БД.

#### Применить миграции к базе данных
```powershell
dotnet ef database update --startup-project ..\Lecture10.ClientAppIntegration.CatsApi\Lecture10.ClientAppIntegration.CatsApi.csproj
```

Описание:

- `database update` применяет все ещё не применённые миграции к базе данных;
- в результате база данных создаётся или обновляется в соответствии с текущими миграциями.
