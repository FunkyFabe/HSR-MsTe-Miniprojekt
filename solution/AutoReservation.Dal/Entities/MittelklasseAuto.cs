using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class MittelklasseAuto : Auto
    {
        public MittelklasseAuto(int id, string marke, int tagestarif, ICollection<Reservation> reservationen,
            byte[] rowVersion) : base(id, marke, tagestarif, reservationen, rowVersion)
        {
        }
    }
}