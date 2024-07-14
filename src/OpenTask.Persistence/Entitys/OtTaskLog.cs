using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenTask.Persistence.Entitys;

[Table("ot_task_log")]
[Index("TaskId", Name = "idx_task_id")]
public partial class OtTaskLog
{
    /// <summary>
    /// 自增主键
    /// </summary>
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// 任务id（taskinfo表主键）
    /// </summary>
    [Column("task_id")]
    public long TaskId { get; set; }

    /// <summary>
    /// 处理开始时间
    /// </summary>
    [Column("handle_start", TypeName = "datetime")]
    public DateTime? HandleStart { get; set; }

    /// <summary>
    /// 处理结果
    /// </summary>
    [Column("handle_result")]
    [StringLength(255)]
    public string HandleResult { get; set; } = null!;

    /// <summary>
    /// 处理状态
    /// </summary>
    [Column("handle_status")]
    public sbyte HandleStatus { get; set; }

    /// <summary>
    /// 处理任务客户端id
    /// </summary>
    [Column("handle_client")]
    [StringLength(128)]
    public string HandleClient { get; set; } = null!;

    /// <summary>
    /// 报警状态 0-默认、1-无需告警、2-告警成功、3-告警失败
    /// </summary>
    [Column("alarm_status")]
    public sbyte AlarmStatus { get; set; }
}
