using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private readonly AutoManager _target;

        public AutoUpdateTests()
        {
            _target = new AutoManager();
        }


        [Fact]
        public async Task GetAllAutosTest()
        {
            // arrange
            Task<List<Auto>> taskList = _target.GetAll();
            List<Auto> listAutos = taskList.Result;
            // act
            // assert
            Assert.True(listAutos.Count == 4, "Count Should be 4 not: " + listAutos.Count);
        }

        [Fact]
        public void FindAutoByPrimaryKey()
        {
            // arrange
            // act
            Auto auto = _target.GetByPrimaryKey(1).Result;
            // assert
            Assert.True("Fiat Punto" == auto.Marke, "It should be Fiat Punto and not: " + auto.Marke);
        }

        [Fact]
        public async Task addAutoTest() {
            // arrange
            Auto auto = new LuxusklasseAuto { Marke = "Mercedes", Tagestarif = 200 };

            // act
            _target.AddEntity<Auto>(auto);

            // assert
            Auto testAuto = _target.GetByPrimaryKey(5).Result;
            Assert.True("Mercedes" == testAuto.Marke, "It should be Mercedes and not: " + testAuto.Marke);
        }


        [Fact]
        public async Task updateAutoTest() {
            // arrange
            String testMarke = "Test Marke";
            int primaryKey = 1;
            Auto auto = _target.GetByPrimaryKey(primaryKey).Result;

            // act
            auto.Marke = testMarke;
            _target.UpdateEntity(auto);

            // assert
            Auto changedCar = _target.GetByPrimaryKey(primaryKey).Result;

            Assert.True(changedCar.Marke == testMarke);
        }

        [Fact]
        public async Task deleteAutoTest() {
            // arrange
            Auto auto = _target.GetByPrimaryKey(1).Result;

            // act
            _target.DeleteEntity(auto);

            List<Auto> listAfterDeletion = _target.GetAll().Result;
            // assert
            Assert.True(listAfterDeletion.Count == 3);

        }
    }
}
