using System;
using System.Collections.Generic;

namespace aspgrupo2.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expires { get; set; }

    public DateTime? Created { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
