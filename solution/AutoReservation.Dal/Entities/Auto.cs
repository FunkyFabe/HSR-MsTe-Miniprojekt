using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public abstract class Auto
    {
        [Key] [Required] public int Id { get; set; }
        [Required] public string Marke { get; set; }
        [Required] public int Tagestarif { get; set; }
        [Required] public ICollection<Reservation> Reservationen { get; set; }
        [Required] public byte[] RowVersion { get; set; }

        protected Auto(int id, string marke, int tagestarif, ICollection<Reservation> reservationen, byte[] rowVersion)
        {
            Id = id;
            Marke = marke ?? throw new ArgumentNullException(nameof(marke));
            Tagestarif = tagestarif;
            Reservationen = reservationen ?? throw new ArgumentNullException(nameof(reservationen));
            RowVersion = rowVersion ?? throw new ArgumentNullException(nameof(rowVersion));
        }
    }
}