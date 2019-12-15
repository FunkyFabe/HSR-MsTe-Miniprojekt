using System;
using System.Threading.Tasks;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing
{
    public class KundeServiceTests
        : ServiceTestBase
    {
        private readonly KundeService.KundeServiceClient _target;
        private readonly Timestamp _geburtsdatum;

        public KundeServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new KundeService.KundeServiceClient(Channel);
            _geburtsdatum = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(1981, 05, 05), DateTimeKind.Utc));
        }

        [Fact]
        public async Task GetKundenTest()
        {
            var request = new Empty();
            var response = await _target.GetKundenAsync(request);
            Assert.Equal(4, response.Items.Count);
        }

        [Fact]
        public async Task GetKundeByIdTest()
        {
            const int id = 1;
            var request = new GetKundeRequest {IdFilter = id};
            var response = await _target.GetKundeAsync(request);
            Assert.Equal(id, response.Id);
            Assert.Equal("Nass", response.Nachname);
            Assert.Equal("Anna", response.Vorname);
            Assert.Equal(new DateTime(1981, 05, 05), response.Geburtsdatum.ToDateTime());
        }

        [Fact]
        public async Task GetKundeByIdWithIllegalIdTest()
        {
            const int invalidId = 1000;
            var request = new GetKundeRequest {IdFilter = invalidId};
            try
            {
                await _target.GetKundeAsync(request);
            }
            catch (RpcException e)
            {
                Assert.Equal(StatusCode.NotFound, e.StatusCode);
            }
        }

        [Fact]
        public async Task InsertKundeTest()
        {
            var kundeToInsert = new KundeDto
                {Vorname = "Seven", Nachname = "Müller", Geburtsdatum = _geburtsdatum};
            var insertResponse = await _target.InsertKundeAsync(kundeToInsert);
            var getResponse = await _target.GetKundeAsync(new GetKundeRequest() {IdFilter = insertResponse.Id});
            Assert.Equal(kundeToInsert.Vorname, getResponse.Vorname);
            Assert.Equal(kundeToInsert.Nachname, getResponse.Nachname);
            Assert.Equal(_geburtsdatum, getResponse.Geburtsdatum);
        }

        [Fact]
        public async Task DeleteKundeTest()
        {
            var kundeToInsert = new KundeDto
                {Vorname = "Seven", Nachname = "Müller", Geburtsdatum = _geburtsdatum};
            var kundeToDelete = await _target.InsertKundeAsync(kundeToInsert);
            await _target.DeleteKundeAsync(kundeToDelete);
            try
            {
                await _target.GetKundeAsync(new GetKundeRequest {IdFilter = kundeToDelete.Id});
            }
            catch (RpcException e)
            {
                Assert.Equal(StatusCode.NotFound, e.StatusCode);
            }
        }

        [Fact]
        public async Task UpdateKundeTest()
        {
            var kundeToInsert = new KundeDto
                {Vorname = "Seven", Nachname = "Müller", Geburtsdatum = _geburtsdatum};
            const string newVorname = "Svenja";

            var kundeToUpdate = await _target.InsertKundeAsync(kundeToInsert);
            kundeToUpdate.Vorname = newVorname;
            var updateResponse = await _target.UpdateKundeAsync(kundeToUpdate);

            Assert.Equal(kundeToInsert.Nachname, updateResponse.Nachname);
            Assert.Equal(newVorname, updateResponse.Vorname);
            Assert.Equal(kundeToInsert.Geburtsdatum, updateResponse.Geburtsdatum);
        }

        [Fact]
        public async Task UpdateKundeWithOptimisticConcurrencyTest()
        {
            var kundeToInsert = new KundeDto
                {Vorname = "Seven", Nachname = "Müller", Geburtsdatum = _geburtsdatum};
            const string newVornameA = "Svenja";
            const string newVornameB = "Sveen";

            var kundeToUpdateA = await _target.InsertKundeAsync(kundeToInsert);
            kundeToUpdateA.Vorname = newVornameA;
            await _target.UpdateKundeAsync(kundeToUpdateA);

            var autoToUpdateB = kundeToUpdateA;
            autoToUpdateB.Vorname = newVornameB;
            try
            {
                await _target.UpdateKundeAsync(autoToUpdateB);
            }
            catch (RpcException e)
            {
                Assert.Equal(StatusCode.Aborted, e.StatusCode);
            }
        }
    }
}