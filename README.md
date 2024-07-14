# OpenTask

[![Build status](https://github.com/SpringHgui/OpenTask/workflows/build/badge.svg)](https://github.com/SpringHgui/OpenTask/actions)
[![Nuget](https://img.shields.io/nuget/v/OpenTask.Core)](https://www.nuget.org/packages/OpenTask.Core/)

去中心设计的分布式任务调度平台

# 快速开始
## 1. 调度中心部署
1. docker-compose
```
# 克隆本项目
cd deploy/docker-compose
docker-compose up -d
```
2. k8s

3. docker

4. 传统手动部署

## 2. 执行节点开发
### .NetCore
可参考 [`src/OpenTask.Client` 
](https://github.com/SpringHgui/OpenTask/tree/f37696f51cf642a8dbf043fabb90568bdbf295e7/src/OpenTask.Client)

1. 安装nuget依赖 `dotnet add package OpenTask.Core`
2. 注册Handler
```
IHost host = Host.CreateDefaultBuilder(args)
.ConfigureServices((ctx, services) =>
{
    _ = services.AddOpenTaskWorker(ctx.Configuration.GetSection("OpenTaskWorker"), options =>
    {
        options.AddHandler<DemoJobHandler>();
        options.AddHandler<JobHandler>();
    });
})
.Build();

host.Run();
```
3. 配置文件
```
{
  "OpenTaskWorker": {
    "Addr": [ "127.0.0.1:1883" ],
    "AppName": "default"
  }
}
```
### java

### 其他语言开发

## 高级配置
1. 使用已有的数据库


# 贡献指南
1. 开发数据库搭建
```
docker run -e MYSQL_DATABASE=open_task -e MYSQL_ROOT_PASSWORD=OPEN_TASK_!@# -p 3308:3306 --name=mysql8 -d registry.cn-hangzhou.aliyuncs.com/hgui/mysql:8.4.1
```
2. 安装vs2022
打开`OpenTask.sln`解决方案进行开发

3. 运行 `OpenTask.WebApi` 后端

3. 切到 `ui/vite-opentask` 运行前端项目

需要安装`node20`
```
npm i
npm run dev
```

默认用户名密码 admin/OpenTask

# 开源协议
MIT
