using System;
using System.Collections.Generic;

namespace Lecture10.ClientAppIntegration.CatsDatabase.DataAccess.EF.Entities;

public partial class Cat
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public Guid Uid { get; set; }

    public string Name { get; set; } = null!;

    public string Breed { get; set; } = null!;

    public int Age { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
