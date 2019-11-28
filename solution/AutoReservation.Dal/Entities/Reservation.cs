using System;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        public int ReservationsNr { get; set; }
        public DateTime Bis { get; set; }
        public DateTime Von { get; set; }
        public int KundenId { get; set; }
        public Kunde Kunde { get; set; }
        public int AutoId { get; set; }
        public Auto Auto { get; set; }
        public byte[] RowVersion { get; set; }

        public Reservation(int reservationsNr, DateTime bis, DateTime von, int kundenId, Kunde kunde, int autoId,
            Auto auto, byte[] rowVersion)
        {
            ReservationsNr = reservationsNr;
            Bis = bis;
            Von = von;
            KundenId = kundenId;
            Kunde = kunde ?? throw new ArgumentNullException(nameof(kunde));
            AutoId = autoId;
            Auto = auto ?? throw new ArgumentNullException(nameof(auto));
            RowVersion = rowVersion ?? throw new ArgumentNullException(nameof(rowVersion));
        }
    }
}