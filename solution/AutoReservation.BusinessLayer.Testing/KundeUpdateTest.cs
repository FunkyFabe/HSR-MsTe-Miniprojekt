using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class KundeUpdateTest
        : TestBase
    {
        private readonly KundeManager _target;

        public KundeUpdateTest()
        {
            _target = new KundeManager();
        }
        
        [Fact]
        public async Task UpdateKundeTest()
        {
            // arrange
            String nachName = "Test Nachname";
            int primaryKey = 1;
            Kunde kunde = _target.GetByPrimaryKey(primaryKey).Result;

            // act
            kunde.Nachname = nachName;
            await _target.UpdateEntity(kunde);

            // assert
            Kunde changedKunde = _target.GetByPrimaryKey(primaryKey).Result;

            Assert.True(changedKunde.Nachname == nachName);
        }
    }
}
