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
        private AutoManager _manager;

        public AutoService(ILogger<AutoService> logger)
        {
            _logger = logger;
            _manager = new AutoManager();
        }

        public override async Task<AutoDtoList> GetAutos(Empty request, ServerCallContext context)
        {
            try
            {
                var response = new AutoDtoList();
                response.Items.AddRange(await _manager.GetAll().ConvertToDtos());
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
                response = await _manager.GetByPrimaryKey(request.IdFilter).ConvertToDto();
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
                var response = await _manager.AddEntity(entity);
                return response.ConvertToDto();
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<Empty> UpdateAuto(AutoDto request, ServerCallContext context)
        {
            try
            {
                var entity = request.ConvertToEntity();
                await _manager.UpdateEntity(entity);
                return new Empty();
            }
            catch (Exception e)
            {
                if (e is OptimisticConcurrencyException<Auto> specificException)
                {
                    throw new RpcException(new Status(StatusCode.Aborted, e.Message), specificException.MergedEntity.ToString());
                }
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<Empty> DeleteAuto(AutoDto request, ServerCallContext context)
        {
            try
            {
                var entity = request.ConvertToEntity();
                await _manager.DeleteEntity(entity);
                return new Empty();
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }
    }
}