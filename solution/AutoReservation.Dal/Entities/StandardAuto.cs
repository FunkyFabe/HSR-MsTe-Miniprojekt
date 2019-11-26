namespace AutoReservation.Dal.Entities
{
    public class StandardAuto : Auto
    {
        public StandardAuto(int id, string marke, int rowVersion, int tagestarif) : base(id, marke, rowVersion,
            tagestarif)
        {
        }
    }
}