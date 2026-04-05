using System;
using System.Collections.Generic;

namespace Lecture10.ClientAppIntegration.CatsDatabase.DataAccess.EF.Entities;

public partial class User
{
    public long Id { get; set; }

    public Guid Uid { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Cat> Cats { get; set; } = new List<Cat>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
