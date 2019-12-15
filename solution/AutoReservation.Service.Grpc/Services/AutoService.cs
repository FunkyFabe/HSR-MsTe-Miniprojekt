using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
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
            try
            {
                var response = new AutoDtoList();
                response.Items.AddRange(await manager.GetAll().ConvertToDtos());
                return response;
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<AutoDto> GetAuto(GetAutoRequest request, ServerCallContext context)
        {
            AutoDto response;
            try
            {
                response = await manager.GetByPrimaryKey(request.IdFilter).ConvertToDto();
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
            return response ?? throw new RpcException(new Status(StatusCode.NotFound, "ID is invalid."));
        }

        public override async Task<AutoDto> InsertAuto(AutoDto request, ServerCallContext context)
        {
            try
            {
                var entity = request.ConvertToEntity();
                var response = await manager.AddEntity(entity);
                return response.ConvertToDto();
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<AutoDto> UpdateAuto(AutoDto request, ServerCallContext context)
        {
            try
            {
                var entity = request.ConvertToEntity();
                var response = await manager.UpdateEntity(entity);
                return response.ConvertToDto();
            }
            catch (OptimisticConcurrencyException<Auto> e)
            {
                throw new RpcException(new Status(StatusCode.Aborted, e.Message), e.MergedEntity.ToString());
            }
        }
        
        public override async Task<AutoDto> DeleteAuto(AutoDto request, ServerCallContext context)
        {
            try
            {
                var entity = request.ConvertToEntity();
                var response = await manager.DeleteEntity(entity);
                return response.ConvertToDto();
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }
    }
}