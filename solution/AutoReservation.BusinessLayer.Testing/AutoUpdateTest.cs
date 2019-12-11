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
        public async Task UpdateAutoTest()
        {
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task GetAllAutos()
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
            Auto auto = _target.GetByPrimaryKey(1);
            // assert
            Assert.True("Fiat Punto" == auto.Marke, "It should be Fiat Punto and not: " + auto.Marke);
        }

        [Fact]
        public void addAuto() {
            // arrange
            Auto auto = new LuxusklasseAuto { Marke = "Mercedes", Tagestarif = 200 };

            // act
            _target.AddAuto(auto);

            // assert
            Auto testAuto = _target.GetByPrimaryKey(5);
            Assert.True("Mercedes" == testAuto.Marke, "It should be Mercedes and not: " + testAuto.Marke);
        }

    }
}
