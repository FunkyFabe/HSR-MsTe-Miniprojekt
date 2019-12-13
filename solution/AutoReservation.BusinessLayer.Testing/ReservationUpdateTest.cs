using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationUpdateTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationUpdateTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            // arrange
            int autoID = 2;
            int primaryKey = 1;
            Reservation res = _target.GetByPrimaryKey(primaryKey).Result;

            // act
            res.AutoId = autoID;
            _target.UpdateEntity(res);

            // assert
            Reservation changedReservation = _target.GetByPrimaryKey(primaryKey).Result;

            Assert.True(changedReservation.AutoId == autoID);
        }
    }
}
