using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ETickets.Models;

public partial class ActorMovie
{


    public int? ActorId { get; set; }
    public int? MovieId { get; set; }
    public virtual Actor? Actor { get; set; }

    public virtual Movie? Movie { get; set; }
}
