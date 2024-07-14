using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenTask.Persistence.Entitys;

[Table("ot_task_info")]
[Index("Name", Name = "idx_name", IsUnique = true)]
[Index("Slot", Name = "idx_slot")]
public partial class OtTaskInfo
{
    /// <summary>
    /// 自增主键
    /// </summary>
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// 所属应用appid
    /// </summary>
    [Column("appid")]
    [StringLength(128)]
    public string Appid { get; set; } = null!;

    /// <summary>
    /// 任务名称
    /// </summary>
    [Column("name")]
    [StringLength(128)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 任务handler名
    /// </summary>
    [Column("handler")]
    [StringLength(128)]
    public string Handler { get; set; } = null!;

    /// <summary>
    /// 任务描述
    /// </summary>
    [Column("description")]
    [StringLength(255)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// 调度时间类型
    /// </summary>
    [Column("time_type")]
    [StringLength(16)]
    public string TimeType { get; set; } = null!;

    /// <summary>
    /// 调度时间配置 根据time_type不同而不同
    /// </summary>
    [Column("time_conf")]
    [StringLength(64)]
    public string TimeConf { get; set; } = null!;

    /// <summary>
    /// 重试间隔 单位s
    /// </summary>
    [Column("attempt_interval")]
    public sbyte AttemptInterval { get; set; }

    /// <summary>
    /// 重试次数
    /// </summary>
    [Column("attempt_max")]
    public sbyte AttemptMax { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    [Column("handle_params")]
    [StringLength(255)]
    public string HandleParams { get; set; } = null!;

    /// <summary>
    /// 调度类型，单机/广播
    /// </summary>
    [Column("schedule_mode")]
    [StringLength(16)]
    public string ScheduleMode { get; set; } = null!;

    /// <summary>
    /// 报警类型
    /// </summary>
    [Column("alarm_type")]
    [StringLength(16)]
    public string AlarmType { get; set; } = null!;

    /// <summary>
    /// 报警配置
    /// </summary>
    [Column("alarm_conf")]
    [StringLength(255)]
    public string AlarmConf { get; set; } = null!;

    /// <summary>
    /// 下次调度时间
    /// </summary>
    [Column("trigger_next_time")]
    public long TriggerNextTime { get; set; }

    /// <summary>
    /// 最后一次调度时间
    /// </summary>
    [Column("trigger_last_time")]
    public long TriggerLastTime { get; set; }

    /// <summary>
    /// 槽
    /// </summary>
    [Column("slot")]
    public int Slot { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [Column("enabled")]
    public bool Enabled { get; set; }
}
