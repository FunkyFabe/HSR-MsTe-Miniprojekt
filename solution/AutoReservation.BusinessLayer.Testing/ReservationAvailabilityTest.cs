using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationAvailabilityTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task ScenarioOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //               | ---Date 2--- |
            Reservation res = new Reservation(new DateTime(2020, 2, 15, 0, 0, 0), new DateTime(2020, 1, 20, 0, 0, 0));
            Exception expectedExcetpion = null;

            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.Null(expectedExcetpion);
        }

        [Fact]
        public async Task ScenarioOkay02Test()
        {
            // arrange
            //| ---Date 1--- |
            //                 | ---Date 2--- |
            Reservation res = new Reservation(new DateTime(2020, 2, 15, 0, 0, 0), new DateTime(2020, 1, 22, 0, 0, 0));
            Exception expectedExcetpion = null;

            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.Null(expectedExcetpion);
        }

        [Fact]
        public async Task ScenarioOkay03Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2-- - |
            // act
            // assert
            Reservation res = new Reservation(new DateTime(2020, 1, 10, 0, 0, 0), new DateTime(2019, 12, 20, 0, 0, 0));
            Exception expectedExcetpion = null;

            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.Null(expectedExcetpion);
        }

        [Fact]
        public async Task ScenarioOkay04Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2--- |
            // act
            // assert
            Reservation res = new Reservation(new DateTime(2020, 1, 9, 0, 0, 0), new DateTime(2019, 12, 20, 0, 0, 0));
            Exception expectedExcetpion = null;

            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.Null(expectedExcetpion);
        }

        [Fact]
        public async Task ScenarioNotOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //    | ---Date 2--- |
            Reservation res = new Reservation(new DateTime(2020, 1, 15, 0, 0, 0), new DateTime(2020, 1, 25, 0, 0, 0));
            Exception expectedExcetpion = null;

            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.NotNull(expectedExcetpion);
        }
        // assert
    

        [Fact]
        public async Task ScenarioNotOkay02Test()
        {
            // arrange
            //    | ---Date 1--- |
            //| ---Date 2--- |
            Reservation res = new Reservation(new DateTime(2019, 12, 15, 0, 0, 0), new DateTime(2020, 1, 15, 0, 0, 0));
            Exception expectedExcetpion = null;

            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.NotNull(expectedExcetpion);
        }

        [Fact]
        public async Task ScenarioNotOkay03Test()
        {
            // arrange
            //| ---Date 1--- |
            //| --------Date 2-------- |
            Reservation res = new Reservation(new DateTime(2020, 1, 10, 0, 0, 0), new DateTime(2020, 12, 25, 0, 0, 0));
            Exception expectedExcetpion = null;
            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }
            // Assert
            Assert.NotNull(expectedExcetpion);
        }

        [Fact]
        public async Task ScenarioNotOkay04Test()
        {
            // arrange
            //| --------Date 1-------- |
            //| ---Date 2--- |
            Reservation res = new Reservation(new DateTime(2020, 1, 10, 0, 0, 0), new DateTime(2020, 1, 15, 0, 0, 0));
            Exception expectedExcetpion = null;

            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.NotNull(expectedExcetpion);
        }

        [Fact]
        public async Task ScenarioNotOkay05Test()
        {
            // arrange
            //| ---Date 1--- |
            //| ---Date 2--- |
            Reservation res = new Reservation(new DateTime(2020, 5, 19, 0, 0, 0), new DateTime(2020, 6, 19, 0, 0, 0));
            Exception expectedExcetpion = null;

            // act
            try
            {
                _target.ReservationPossible(res, 2);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.NotNull(expectedExcetpion);
        }
    }
}

