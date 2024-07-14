# OpenTask

[![Build status](https://github.com/SpringHgui/OpenTask/workflows/build/badge.svg)](https://github.com/SpringHgui/OpenTask/actions)

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
参考 [`src/OpenTask.Client` 
](https://github.com/SpringHgui/OpenTask/tree/f37696f51cf642a8dbf043fabb90568bdbf295e7/src/OpenTask.Client)
## 高级配置
1. 使用已有的数据库


# 贡献指南
1. 开发数据库搭建
```
docker run -e MYSQL_DATABASE=open_task -e MYSQL_ROOT_PASSWORD=OPEN_TASK_!@# -p 3308:3306 --name=mysql8 -d registry.cn-hangzhou.aliyuncs.com/hgui/mysql:8.4.1
```
2. 安装vs2022
打开`OpenTask.sln`解决方案进行开发

# 开源协议
MIT
