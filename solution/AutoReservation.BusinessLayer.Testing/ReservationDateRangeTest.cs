using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationDateRangeTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationDateRangeTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public void ScenarioOkay01TestAsync()
        {
            //throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            // arrange
            //| ---Date 1--- |
            //                 | ---Date 2--- |
            Reservation res = new Reservation(new DateTime(2020, 2, 15, 0, 0, 0), new DateTime(2020, 1, 22, 0, 0, 0), 2);
            Exception expectedExcetpion = null;

            // act
            try
            {
                 _target.ReservationPossible(res);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.Null(expectedExcetpion);
        }

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            // less then 24 Hours
            // Arrange
            DateTime start = new DateTime(2019, 12, 12, 0, 0, 0);
            DateTime end = new DateTime(2019, 12, 11, 12, 0, 0);
            Reservation reservation = new Reservation(start, end, 2);
            // Act
            // Assert
            Assert.ThrowsAsync<InvaildDateRangException>(async () => {  await _target.ReservationPossible(reservation); });
        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            // Start and End reversed
            // Arrange
            DateTime start = new DateTime(2019, 12, 12, 0, 0, 0);
            DateTime end = new DateTime(2019, 12, 11, 12, 0, 0);
            Reservation reservation = new Reservation(start, end, 2);
            // Act
            Assert.ThrowsAsync<InvaildDateRangException>(async () => { await _target.ReservationPossible(reservation); });
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            //throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}
