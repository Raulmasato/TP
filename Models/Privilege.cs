using System;
using System.Collections.Generic;

namespace aspgrupo2.Models;

public partial class Privilege
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public virtual ICollection<UserPrivilege> UserPrivileges { get; set; } = new List<UserPrivilege>();
}