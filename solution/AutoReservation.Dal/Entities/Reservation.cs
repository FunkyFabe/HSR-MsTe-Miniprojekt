using System;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        [Key] [Required] public int ReservationsNr { get; set; }
        [Required] public DateTime Bis { get; set; }
        [Required] public DateTime Von { get; set; }
        [Required] public int KundenId { get; set; }
        [Required] public Kunde Kunde { get; set; }
        [Required] public int AutoId { get; set; }
        [Required] public Auto Auto { get; set; }
        [Required] public byte[] RowVersion { get; set; }

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