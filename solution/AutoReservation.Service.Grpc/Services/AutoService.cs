using System.Threading.Tasks;
using AutoReservation.BusinessLayer;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class AutoService : Grpc.AutoService.AutoServiceBase
    {
        private readonly ILogger<AutoService> _logger;
        private AutoManager manager = new AutoManager();

        public AutoService(ILogger<AutoService> logger)
        {
            _logger = logger;
        }

        public override async Task<AutoDtoList> GetAutos(Empty request, ServerCallContext context)
        {
            var response = new AutoDtoList();
            response.Items.AddRange(await manager.GetAll().ConvertToDtos());
            return await Task.FromResult(response);
        }

        public override async Task<AutoDto> GetAuto(GetAutoRequest request, ServerCallContext context)
        {
            return await manager.GetByPrimaryKey(request.IdFilter).ConvertToDto();
        }
    }
}