using System;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        public int ReservationsNr { get; set; }
        public DateTime Bis { get; set; }
        public DateTime Von { get; set; }
        public int KundenId { get; set; }
        public int AutoId { get; set; }
        public byte RowVersion { get; set; }

        public Reservation(int reservationsNr, DateTime bis, DateTime von, int kundenId, int autoId, byte rowVersion)
        {
            ReservationsNr = reservationsNr;
            Bis = bis;
            Von = von;
            KundenId = kundenId;
            AutoId = autoId;
            RowVersion = rowVersion;
        }
    }
}