using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenTask.Persistence.Entitys;

[Table("ot_server")]
[Index("ServerId", Name = "idx_server_id", IsUnique = true)]
public partial class OtServer
{
    /// <summary>
    /// 自增主键
    /// </summary>
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// server唯一标识
    /// </summary>
    [Column("server_id")]
    [StringLength(128)]
    public string ServerId { get; set; } = null!;

    /// <summary>
    /// 集群内部访问地址
    /// </summary>
    [Column("end_point")]
    [StringLength(64)]
    public string EndPoint { get; set; } = null!;

    /// <summary>
    /// 最后一次心跳时间
    /// </summary>
    [Column("heart_at", TypeName = "datetime")]
    public DateTime HeartAt { get; set; }

    /// <summary>
    /// 0~16383
    /// </summary>
    [Column("slot_from")]
    public int SlotFrom { get; set; }

    /// <summary>
    /// 0~16383
    /// </summary>
    [Column("slot_end")]
    public int SlotEnd { get; set; }
}
