using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    [Table("Auto", Schema = "dbo")]
    public abstract class Auto
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string Marke { get; set; }
        [Required]
        public int Tagestarif { get; set; }
        [Required]
        public ICollection<Reservation> Reservationen { get; set; }
        [Column(TypeName = "TIMESTAMP")]
        public byte[] RowVersion { get; set; }

        protected Auto() {}
        protected Auto(int id, string marke, int tagestarif, ICollection<Reservation> reservationen, byte[] rowVersion)
        {
            Id = id;
            Marke = marke;
            Tagestarif = tagestarif;
            Reservationen = reservationen;
            RowVersion = rowVersion;
        }
    }
}