# OpenTask

[![Build status](https://github.com/SpringHgui/OpenTask/workflows/build/badge.svg)](https://github.com/SpringHgui/OpenTask/actions)
[![Nuget](https://img.shields.io/nuget/v/OpenTask.Core)](https://www.nuget.org/packages/OpenTask.Core/)

去中心设计的分布式任务调度平台，本项目不仅仅是创造一个新的轮子，旨在补充dotnet生态下分布式任务调度系统的空白，但本项目设计的并非只支持dotnet平台；

调度中心与执行器的通讯协议采用mqtt协议，方便多种语言的快速接入，调度中心采用去中心化设计，各个调度中心之间亦采用mqtt协议通讯；

调度中心自动进行任务分片，以平均每个调度节点的负载。

## 仓库地址

[Github](https://github.com/SpringHgui/OpenTask)

[Gitee](https://gitee.com/SpringHgui/OpenTask)

## 当前进度
⚠⚠⚠ 当前尚处于开发初期，请勿在重要的生产环境中使用。

- [x] web管理后台
- [x] cron任务
- [ ] 工作流（DAG）
- [ ] 固定周期任务
- [x] 调度中心集群部署
- [x] 调度中心自动分片
- [x] 任务重试
- [ ] 任务故障转移
- [ ] 进行中的任务支持取消
- [x] 失败告警

未完待续...

欢迎提[issue](https://github.com/SpringHgui/OpenTask/issues)补充您的需求
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
## 3. 管理后台查看

登录后台 localhost:8080
默认账号密码:  admin/OpenTask

### 其他语言开发

## 高级配置
1. 使用已有的数据库


# 贡献指南

开源项目的发展离不开社区的贡献，如果您在使用本项目中遇到问题或建议，欢迎多多提[issue](https://github.com/SpringHgui/OpenTask/issues)，对于符合大众的需求，我们将尽快进行完善。

如果您在使用中遇到了bug或者有bug的修改建议等，您可以直接创建[PR](https://github.com/SpringHgui/OpenTask/pulls)，感谢~

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
