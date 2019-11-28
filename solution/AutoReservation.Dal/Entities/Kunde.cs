using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        [Key] [Required] public int Id { get; set; }
        [Required] public DateTime Geburtsdatum { get; set; }
        [Required] public string Nachname { get; set; }
        [Required] public string Vorname { get; set; }
        [Required] public ICollection<Reservation> Reservationen { get; set; }
        [Required] public byte[] RowVersion { get; set; }

        public Kunde(int id, DateTime geburtsdatum, string nachname, string vorname,
            ICollection<Reservation> reservationen, byte[] rowVersion)
        {
            Id = id;
            Geburtsdatum = geburtsdatum;
            Nachname = nachname ?? throw new ArgumentNullException(nameof(nachname));
            Vorname = vorname ?? throw new ArgumentNullException(nameof(vorname));
            Reservationen = reservationen ?? throw new ArgumentNullException(nameof(reservationen));
            RowVersion = rowVersion ?? throw new ArgumentNullException(nameof(rowVersion));
        }
    }
}