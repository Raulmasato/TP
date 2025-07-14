using System;
using System.Collections.Generic;

namespace aspgrupo2.Models;

public partial class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<UserPrivilege> UserPrivileges { get; set; }
    public virtual ICollection<SportsArticle> SportsArticles { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
}