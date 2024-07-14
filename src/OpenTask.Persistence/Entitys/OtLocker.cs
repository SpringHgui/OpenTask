using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenTask.Persistence.Entitys;

[Table("ot_locker")]
public partial class OtLocker
{
    /// <summary>
    /// 资源唯唯一标识
    /// </summary>
    [Key]
    [Column("resource")]
    [StringLength(64)]
    public string Resource { get; set; } = null!;

    /// <summary>
    /// 资源版本号
    /// </summary>
    [Column("version")]
    public int Version { get; set; }

    /// <summary>
    /// 加锁者
    /// </summary>
    [Column("locked_by")]
    [StringLength(64)]
    public string LockedBy { get; set; } = null!;

    /// <summary>
    /// 获取锁的开始时间
    /// </summary>
    [Column("locked_at", TypeName = "datetime")]
    public DateTime LockedAt { get; set; }
}
