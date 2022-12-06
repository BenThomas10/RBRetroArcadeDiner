using System;
using System.Collections.Generic;

namespace ArcadeDiner.Models;

public partial class ReservationInfo
{
    public int ReservationId { get; set; }

    public string PartyName { get; set; } = null!;

    public int NumberInParty { get; set; }

    public DateTime ReservationDate { get ; set; }

    public TimeSpan ReservationTime { get; set; }

    public int ReservationNumber { get; set; }

    public DateTime SubmissionDateTime { get; set; }

    public string Email { get; set; } = null!;

    public bool IsFulfilled { get; set; }

    public DateTime LastUpdateDate { get; set; }
}
