using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class LuxusklasseAuto : Auto
    {
        public int Basistarif { get; set; }

        public LuxusklasseAuto(int id, string marke, int tagestarif, ICollection<Reservation> reservationen,
            byte[] rowVersion, int basistarif) : base(id, marke, tagestarif, reservationen, rowVersion)
        {
            Basistarif = basistarif;
        }
    }
}