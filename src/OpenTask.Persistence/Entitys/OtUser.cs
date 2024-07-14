using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenTask.Persistence.Entitys;

[Table("ot_user")]
[Index("UserName", Name = "idx_user_name", IsUnique = true)]
public partial class OtUser
{
    /// <summary>
    /// 用户id
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [Column("user_name")]
    [StringLength(128)]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 密码
    /// </summary>
    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }
}
