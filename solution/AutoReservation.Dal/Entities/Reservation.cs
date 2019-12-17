using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    [Table("Reservation", Schema = "dbo")]
    public class Reservation
    {
        [Key]
        [Required]
        public int ReservationsNr { get; set; }
        [Required]
        [Column(TypeName = "DATETIME2(7)")]
        public DateTime Bis { get; set; }
        [Required]
        [Column(TypeName = "DATETIME2(7)")]
        public DateTime Von { get; set; }
        [Required]
        public int KundeId { get; set; }
        [Required]
        public Kunde Kunde { get; set; }
        [Required]
        public int AutoId { get; set; }
        [Required]
        public Auto Auto { get; set; }
        [Timestamp]
        [Column(TypeName = "TIMESTAMP")]
        public byte[] RowVersion { get; set; }

        public Reservation(){}
        public Reservation(int reservationsNr, DateTime bis, DateTime von, int kundeId, Kunde kunde, int autoId,
            Auto auto, byte[] rowVersion)
        {
            ReservationsNr = reservationsNr;
            Bis = bis;
            Von = von;
            KundeId = kundeId;
            Kunde = kunde;
            AutoId = autoId;
            Auto = auto;
            RowVersion = rowVersion;
        }

        public Reservation(DateTime bis, DateTime von, int autoId)
        {
            Bis = bis;
            Von = von;
            AutoId = autoId;
        }
    }
}