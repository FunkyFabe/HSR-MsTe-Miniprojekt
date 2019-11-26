using System;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        public int Id { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public int RowVersion { get; set; }

        public Kunde(int id, DateTime geburtsdatum, string nachname, string vorname, int rowVersion)
        {
            Id = id;
            Geburtsdatum = geburtsdatum;
            Nachname = nachname ?? throw new ArgumentNullException(nameof(nachname));
            Vorname = vorname ?? throw new ArgumentNullException(nameof(vorname));
            RowVersion = rowVersion;
        }
    }
}