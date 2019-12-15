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
            Reservation res = await _target.GetByPrimaryKey(primaryKey);

            // act
            res.AutoId = autoID;
            await _target.UpdateEntity(res);

            // assert
            Reservation changedReservation = _target.GetByPrimaryKey(primaryKey).Result;

            Assert.True(changedReservation.AutoId == autoID);
        }
    }
}
