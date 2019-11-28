using System;
using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public abstract class Auto
    {
        public int Id { get; set; }
        public string Marke { get; set; }
        public int Tagestarif { get; set; }
        public ICollection<Reservation> Reservationen { get; set; }
        public byte[] RowVersion { get; set; }

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