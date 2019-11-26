using System;

namespace AutoReservation.Dal.Entities
{
    public abstract class Auto
    {
        public int Id { get; set; }
        public string Marke { get; set; }
        public int RowVersion { get; set; }
        public int Tagestarif { get; set; }

        protected Auto(int id, string marke, int rowVersion, int tagestarif)
        {
            Id = id;
            Marke = marke ?? throw new ArgumentNullException(nameof(marke));
            RowVersion = rowVersion;
            Tagestarif = tagestarif;
        }
    }
}