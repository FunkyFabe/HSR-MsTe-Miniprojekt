using System;
using System.IO;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class ReservationService : Grpc.ReservationService.ReservationServiceBase
    {
        private readonly ILogger<ReservationService> _logger;
        private readonly ReservationManager _manager;

        public ReservationService(ILogger<ReservationService> logger)
        {
            _logger = logger;
            _manager = new ReservationManager();
        }

        public override async Task<ReservationDtoList> GetReservationen(Empty request, ServerCallContext context)
        {
            try
            {
                var response = new ReservationDtoList();
                response.Items.AddRange(await _manager.GetAll().ConvertToDtos());
                return response;
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<ReservationDto> GetReservationById(GetReservationRequest request,
            ServerCallContext context)
        {
            ReservationDto response;
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

        public override async Task<ReservationDto> InsertReservation(ReservationDto request, ServerCallContext context)
        {
            try
            {
                var reservation = request.ConvertToEntity();
                var response = await _manager.AddEntity(reservation);
                return response.ConvertToDto();
            }
            catch (Exception e)
            {
                if (e is InvaildDateRangException)
                {
                    throw new RpcException(new Status(StatusCode.OutOfRange, e.Message));
                }
                
                if (e is AutoUnavailableException)
                {
                    throw new RpcException(new Status(StatusCode.ResourceExhausted, e.Message));
                }
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<Empty> UpdateReservation(ReservationDto request, ServerCallContext context)
        {
            try
            {
                var reservation = request.ConvertToEntity();
                await _manager.UpdateEntity(reservation);
                return new Empty();
            }
            catch (Exception e)
            {
                if (e is OptimisticConcurrencyException<Reservation> specificException)
                {
                    throw new RpcException(new Status(StatusCode.Aborted, e.Message), specificException.MergedEntity.ToString());
                }
                if (e is InvaildDateRangException)
                {
                    throw new RpcException(new Status(StatusCode.OutOfRange, e.Message));   
                }
                if (e is AutoUnavailableException)
                {
                    throw new RpcException(new Status(StatusCode.ResourceExhausted, e.Message));
                }
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }
        
        public override async Task<Empty> DeleteReservation(ReservationDto request, ServerCallContext context)
        {
            try
            {
                var reservation = request.ConvertToEntity();
                await _manager.DeleteEntity(reservation);
                return new Empty();
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occured."));
            }
        }

        public override async Task<CheckAvailabilityResponse> CheckAvailability(ReservationDto request, ServerCallContext context)
        {
            var reservation = request.ConvertToEntity();
            bool isAvailable = await _manager.IsAutoAvailable(reservation);
            return new CheckAvailabilityResponse{AutoIsAvailable = isAvailable};
        }
    }
}