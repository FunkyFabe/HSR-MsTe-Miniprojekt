using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    [Table("Kunde", Schema = "dbo")]
    public class Kunde
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "DATETIME2(7)")]
        public DateTime Geburtsdatum { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string Nachname { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string Vorname { get; set; }
        [Required]
        public ICollection<Reservation> Reservationen { get; set; }
        [Timestamp]
        [Column(TypeName = "TIMESTAMP")]
        public byte[] RowVersion { get; set; }

        public Kunde(){}
        public Kunde(int id, DateTime geburtsdatum, string nachname, string vorname,
            ICollection<Reservation> reservationen, byte[] rowVersion)
        {
            Id = id;
            Geburtsdatum = geburtsdatum;
            Nachname = nachname;
            Vorname = vorname;
            Reservationen = reservationen;
            RowVersion = rowVersion;
        }
    }
}