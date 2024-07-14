using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OpenTask.Persistence.Entitys;

namespace OpenTask.Persistence.Contexts;

public partial class OpenTaskContext : DbContext
{
    public OpenTaskContext(DbContextOptions<OpenTaskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OtLocker> OtLockers { get; set; }

    public virtual DbSet<OtServer> OtServers { get; set; }

    public virtual DbSet<OtTaskInfo> OtTaskInfos { get; set; }

    public virtual DbSet<OtTaskLog> OtTaskLogs { get; set; }

    public virtual DbSet<OtUser> OtUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OtLocker>(entity =>
        {
            entity.HasKey(e => e.Resource).HasName("PRIMARY");

            entity.Property(e => e.Resource).HasComment("资源唯唯一标识");
            entity.Property(e => e.LockedAt).HasComment("获取锁的开始时间");
            entity.Property(e => e.LockedBy)
                .HasDefaultValueSql("''")
                .HasComment("加锁者");
            entity.Property(e => e.Version).HasComment("资源版本号");
        });

        modelBuilder.Entity<OtServer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasComment("自增主键");
            entity.Property(e => e.EndPoint).HasComment("集群内部访问地址");
            entity.Property(e => e.HeartAt).HasComment("最后一次心跳时间");
            entity.Property(e => e.ServerId).HasComment("server唯一标识");
            entity.Property(e => e.SlotEnd).HasComment("0~16383");
            entity.Property(e => e.SlotFrom).HasComment("0~16383");
        });

        modelBuilder.Entity<OtTaskInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasComment("自增主键");
            entity.Property(e => e.AlarmConf)
                .HasDefaultValueSql("''")
                .HasComment("报警配置");
            entity.Property(e => e.AlarmType)
                .HasDefaultValueSql("''")
                .HasComment("报警类型");
            entity.Property(e => e.Appid)
                .HasDefaultValueSql("''")
                .HasComment("所属应用appid");
            entity.Property(e => e.AttemptInterval).HasComment("重试间隔 单位s");
            entity.Property(e => e.AttemptMax).HasComment("重试次数");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("''")
                .HasComment("任务描述");
            entity.Property(e => e.Enabled).HasComment("启用/禁用");
            entity.Property(e => e.HandleParams)
                .HasDefaultValueSql("''")
                .HasComment("执行参数");
            entity.Property(e => e.Handler)
                .HasDefaultValueSql("''")
                .HasComment("任务handler名");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("''")
                .HasComment("任务名称");
            entity.Property(e => e.ScheduleMode)
                .HasDefaultValueSql("''")
                .HasComment("调度类型，单机/广播");
            entity.Property(e => e.Slot).HasComment("槽");
            entity.Property(e => e.TimeConf)
                .HasDefaultValueSql("''")
                .HasComment("调度时间配置 根据time_type不同而不同");
            entity.Property(e => e.TimeType)
                .HasDefaultValueSql("''")
                .HasComment("调度时间类型");
            entity.Property(e => e.TriggerLastTime).HasComment("最后一次调度时间");
            entity.Property(e => e.TriggerNextTime).HasComment("下次调度时间");
        });

        modelBuilder.Entity<OtTaskLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasComment("自增主键");
            entity.Property(e => e.AlarmStatus).HasComment("报警状态 0-默认、1-无需告警、2-告警成功、3-告警失败");
            entity.Property(e => e.HandleClient)
                .HasDefaultValueSql("''")
                .HasComment("处理任务客户端id");
            entity.Property(e => e.HandleResult)
                .HasDefaultValueSql("''")
                .HasComment("处理结果");
            entity.Property(e => e.HandleStart).HasComment("处理开始时间");
            entity.Property(e => e.HandleStatus).HasComment("处理状态");
            entity.Property(e => e.TaskId).HasComment("任务id（taskinfo表主键）");
        });

        modelBuilder.Entity<OtUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasComment("用户id");
            entity.Property(e => e.CreatedAt).HasComment("创建时间");
            entity.Property(e => e.Password)
                .HasDefaultValueSql("''")
                .HasComment("密码");
            entity.Property(e => e.UserName)
                .HasDefaultValueSql("''")
                .HasComment("用户名");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
