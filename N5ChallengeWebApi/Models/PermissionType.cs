using System;
using System.Collections.Generic;

namespace N5ChallengeWebApi.Models;

public partial class PermissionType
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Permission> Permissions { get; } = new List<Permission>();
}
