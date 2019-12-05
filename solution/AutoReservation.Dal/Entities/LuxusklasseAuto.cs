using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public class LuxusklasseAuto : Auto
    {
        [Required]
        public int Basistarif { get; set; }

        public LuxusklasseAuto(){}
        public LuxusklasseAuto(int id, string marke, int tagestarif, ICollection<Reservation> reservationen,
            byte[] rowVersion, int basistarif) : base(id, marke, tagestarif, reservationen, rowVersion)
        {
            Basistarif = basistarif;
        }
    }
}