using System;
using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        public int Id { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public ICollection<Reservation> Reservationen { get; set; }
        public byte[] RowVersion { get; set; }

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