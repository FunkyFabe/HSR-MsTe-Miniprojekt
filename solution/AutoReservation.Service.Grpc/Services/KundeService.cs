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
    internal class KundeService : Grpc.KundeService.KundeServiceBase
    {
        private readonly ILogger<KundeService> _logger;
        private KundeManager _manager;

        public KundeService(ILogger<KundeService> logger)
        {
            _logger = logger;
            _manager = new KundeManager();
        }

        public override async Task<KundeDtoList> GetKunden(Empty request, ServerCallContext context)
        {
            try
            {
                var response = new KundeDtoList();
                response.Items.AddRange(await _manager.GetAll().ConvertToDtos());
                return response;
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<KundeDto> GetKunde(GetKundeRequest request, ServerCallContext context)
        {
            KundeDto response;
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

        public override async Task<KundeDto> InsertKunde(KundeDto request, ServerCallContext context)
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

        public override async Task<Empty> UpdateKunde(KundeDto request, ServerCallContext context)
        {
            try
            {
                var entity = request.ConvertToEntity();
                await _manager.UpdateEntity(entity);
                return new Empty();
            }
            catch (Exception e)
            {
                if (e is OptimisticConcurrencyException<Kunde> specificException)
                {
                    throw new RpcException(new Status(StatusCode.Aborted, e.Message), specificException.MergedEntity.ToString());
                }
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<Empty> DeleteKunde(KundeDto request, ServerCallContext context)
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