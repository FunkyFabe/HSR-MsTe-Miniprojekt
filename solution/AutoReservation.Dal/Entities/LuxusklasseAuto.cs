namespace AutoReservation.Dal.Entities
{
    public class LuxusklasseAuto : Auto
    {
        public int Basistarif { get; set; }

        public LuxusklasseAuto(int id, string marke, int rowVersion, int tagestarif) : base(id, marke, rowVersion,
            tagestarif)
        {
        }
    }
}