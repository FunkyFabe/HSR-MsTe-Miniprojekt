using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;
using Xunit.Abstractions;

namespace AutoReservation.Service.Grpc.Testing
{
    public class ReservationServiceTests
        : ServiceTestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ReservationService.ReservationServiceClient _target;
        private readonly AutoService.AutoServiceClient _autoClient;
        private readonly KundeService.KundeServiceClient _kundeClient;
        private readonly Timestamp _von;
        private readonly Timestamp _bis;

        public ReservationServiceTests(ServiceTestFixture serviceTestFixture, ITestOutputHelper testOutputHelper)
            : base(serviceTestFixture)
        {
            _testOutputHelper = testOutputHelper;
            _target = new ReservationService.ReservationServiceClient(Channel);
            _autoClient = new AutoService.AutoServiceClient(Channel);
            _kundeClient = new KundeService.KundeServiceClient(Channel);
            int year = DateTime.Now.Year + 1;
            _von = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(year, 01, 10), DateTimeKind.Utc));
            _bis = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(year, 01, 20), DateTimeKind.Utc));
        }

        [Fact]
        public async Task GetReservationenTest()
        {
            var request = new Empty();
            var response = await _target.GetReservationenAsync(request);
            Assert.Equal(4, response.Items.Count);
        }

        [Fact]
        public async Task GetReservationByIdTest()
        {
            const int id = 1;
            var request = new GetReservationRequest {IdFilter = id};
            var response = await _target.GetReservationByIdAsync(request);
            _testOutputHelper.WriteLine(response.ToString());
            Assert.Equal(id, response.ReservationsNr);
            Assert.Equal(_von, response.Von);
            Assert.Equal(_bis, response.Bis);
            _testOutputHelper.WriteLine(response.ToString());
        }

        [Fact]
        public async Task GetReservationByIdWithIllegalIdTest()
        {
            const int invalidId = 1000;
            var request = new GetReservationRequest {IdFilter = invalidId};
            try
            {
                await _target.GetReservationByIdAsync(request);
            }
            catch (RpcException e)
            {
                Assert.Equal(StatusCode.NotFound, e.StatusCode);
            }
        }

        [Fact]
        public async Task InsertReservationTest()
        {
            var kunde = _kundeClient.GetKunde(new GetKundeRequest {IdFilter = 1});
            
            var autoToInsert = new AutoDto
                {Marke = "Skoda Octavia", Tagestarif = 50, AutoKlasse = AutoKlasse.Mittelklasse};
            var auto = _autoClient.InsertAuto(autoToInsert);
            
            var reservationToInsert = new ReservationDto
                {Bis = _bis, Von = _von, Kunde = kunde, Auto = auto};
            var insertResponse = await _target.InsertReservationAsync(reservationToInsert);
            var getResponse = await _target.GetReservationByIdAsync(new GetReservationRequest{IdFilter = insertResponse.ReservationsNr});
            Assert.Equal(reservationToInsert.Von, getResponse.Von);
            Assert.Equal(reservationToInsert.Bis, getResponse.Bis);
            Assert.Equal(reservationToInsert.Auto, getResponse.Auto);
            Assert.Equal(reservationToInsert.Kunde, getResponse.Kunde);
        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            var kunde = _kundeClient.GetKunde(new GetKundeRequest {IdFilter = 1});
            var autoToInsert = new AutoDto
                {Marke = "Skoda Octavia", Tagestarif = 50, AutoKlasse = AutoKlasse.Mittelklasse};
            var auto = _autoClient.InsertAuto(autoToInsert);
            
            var reservationToInsert = new ReservationDto
                {Bis = _bis, Von = _von, Kunde = kunde, Auto = auto};
            var insertResponse = await _target.InsertReservationAsync(reservationToInsert);
            var reservationToDelete = await _target.GetReservationByIdAsync(new GetReservationRequest{IdFilter = insertResponse.ReservationsNr});
            await _target.DeleteReservationAsync(reservationToDelete);
            try
            {
                await _target.GetReservationByIdAsync(new GetReservationRequest() {IdFilter = reservationToDelete.ReservationsNr});
            }
            catch (RpcException e)
            {
                Assert.Equal(StatusCode.NotFound, e.StatusCode);
            }
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            var kunde = _kundeClient.GetKunde(new GetKundeRequest {IdFilter = 1});
            var autoToInsert = new AutoDto
                {Marke = "Skoda Octavia", Tagestarif = 50, AutoKlasse = AutoKlasse.Mittelklasse};
            var auto = _autoClient.InsertAuto(autoToInsert);
            var reservationToInsert = new ReservationDto
                {Bis = _bis, Von = _von, Kunde = kunde, Auto = auto};
            
            var insertResponse = await _target.InsertReservationAsync(reservationToInsert);
            var reservationToUpdate = await _target.GetReservationByIdAsync(new GetReservationRequest{IdFilter = insertResponse.ReservationsNr});
            var newBis = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(3019, 01, 20), DateTimeKind.Utc));
            reservationToUpdate.Bis = newBis;
            _testOutputHelper.WriteLine(reservationToUpdate.ToString());
            await _target.UpdateReservationAsync(reservationToUpdate);
            var getResponse = await _target.GetReservationByIdAsync(new GetReservationRequest{IdFilter = insertResponse.ReservationsNr});
            
            Assert.Equal(reservationToUpdate.Von, getResponse.Von);
            Assert.Equal(reservationToUpdate.Bis, getResponse.Bis);
            Assert.Equal(reservationToUpdate.Auto, getResponse.Auto);
            Assert.Equal(reservationToUpdate.Kunde, getResponse.Kunde);
        }

        [Fact]
        public async Task UpdateReservationWithOptimisticConcurrencyTest()
        {
            //TODO: Implemente OptimisticConcurrency
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task InsertReservationWithInvalidDateRangeTest()
        {
            //TODO: Adjust exception according to business layer. (First implement different exceptions in business layer.)
            var kunde = _kundeClient.GetKunde(new GetKundeRequest {IdFilter = 1});
            var autoToInsert = new AutoDto
                {Marke = "Skoda Octavia", Tagestarif = 50, AutoKlasse = AutoKlasse.Mittelklasse};
            var auto = _autoClient.InsertAuto(autoToInsert);
            var reservationToInsert = new ReservationDto
                {Bis = _von, Von = _bis, Kunde = kunde, Auto = auto};
            try
            {
                await _target.InsertReservationAsync(reservationToInsert);
            }
            catch (RpcException e)
            {
                Assert.Equal(StatusCode.OutOfRange, e.StatusCode);
            }
        }

        [Fact]
        public async Task InsertReservationWithAutoNotAvailableTest()
        {
            //TODO: Adjust exception according to business layer. (First implement different exceptions in business layer.)
            var kunde1 = _kundeClient.GetKunde(new GetKundeRequest {IdFilter = 1});
            var kunde2 = _kundeClient.GetKunde(new GetKundeRequest {IdFilter = 2});
            var autoToInsert = new AutoDto
                {Marke = "Skoda Octavia", Tagestarif = 50, AutoKlasse = AutoKlasse.Mittelklasse};
            var auto = _autoClient.InsertAuto(autoToInsert);
            var reservationToInsert1 = new ReservationDto
                {Bis = _bis, Von = _von, Kunde = kunde1, Auto = auto};
            await _target.InsertReservationAsync(reservationToInsert1);
            var reservationToInsert2 = new ReservationDto
                {Bis = _bis, Von = _von, Kunde = kunde2, Auto = auto};
            try
            {
                await _target.InsertReservationAsync(reservationToInsert2);
            }
            catch (RpcException e)
            {
                Assert.Equal(StatusCode.OutOfRange, e.StatusCode);
            }
        }

        [Fact]
        public async Task UpdateReservationWithInvalidDateRangeTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateReservationWithAutoNotAvailableTest()
        {
            
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task CheckAvailabilityIsTrueTest()
        {
            //TODO: Implement service in business layer.
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task CheckAvailabilityIsFalseTest()
        {
            //TODO: Implement service in business layer.
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}