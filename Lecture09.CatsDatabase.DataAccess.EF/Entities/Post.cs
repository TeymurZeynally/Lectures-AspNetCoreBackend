using System;
using System.Collections.Generic;

namespace Lecture09.CatsDatabase.DataAccess.EF.Entities;

public partial class Post
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public Guid Uid { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Cat> Cats { get; set; } = new List<Cat>();
}
