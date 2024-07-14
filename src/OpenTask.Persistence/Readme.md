
dotnet tool install --global dotnet-ef
dotnet tool update dotnet-ef

db first
```
dotnet ef dbcontext scaffold "server=127.0.0.1;Port=3306;user id=root;database=open_task;pooling=true;password=OPEN_TASK_!@#;" MySql.EntityFrameworkCore --schema open_task --context-dir Contexts --output-dir Entitys --no-onconfiguring -d --no-build --force
```

code first
https://learn.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

```
dotnet ef migrations add InitialCreate
dotnet ef database update

dotnet ef migrations remove
```