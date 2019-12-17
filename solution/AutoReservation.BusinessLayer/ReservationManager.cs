using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
        : ManagerBase
    {
        public async Task<List<Reservation>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen.ToListAsync();
        }

        public async Task<Reservation> GetByPrimaryKey(int primaryKey)
        {
            using AutoReservationContext context = new AutoReservationContext();
            var query =
                from r in context.Reservationen
                join k in context.Kunden
                    on r.KundeId equals k.Id
                join a in context.Autos
                    on r.AutoId equals a.Id
                where r.ReservationsNr == primaryKey
                select new Reservation
                {
                    ReservationsNr = r.ReservationsNr,
                    Bis = r.Bis,
                    Von = r.Von,
                    KundeId = k.Id,
                    Kunde = k,
                    AutoId = a.Id,
                    Auto = a,
                    RowVersion = r.RowVersion
                };
            var result = await query.ToListAsync();
            return result.Count == 0 ? null : result[0];
        }

        public async override Task UpdateEntity<T>(T entity)
        {
            await ReservationPossible(entity as Reservation);
            await base.UpdateEntity(entity);
        }

        public async override Task<T> AddEntity<T>(T entity)
        {
            await ReservationPossible(entity as Reservation);
            return await base.AddEntity(entity);
        }

        public async Task ReservationPossible(Reservation reservation)
        {
            CheckReservationLenght(reservation);
            if (!await IsAutoAvailable(reservation))
            {
                throw new AutoUnavailableException("Auto is not available.");
            }
        }

        public async Task<bool> IsAutoAvailable(Reservation reservation) {
            //Get All Reservations for a Car
            List<Reservation> allReservations = await GetAll();
            List<Reservation> allReservationsForOneCar = allReservations.FindAll(
                delegate (Reservation re) { return re.AutoId == reservation.AutoId; });

            foreach (Reservation res in allReservationsForOneCar)
            {
                if (res.ReservationsNr == reservation.ReservationsNr) { }
                else
                {
                    if (res.Von <= reservation.Von && res.Bis > reservation.Von)
                    {
                        return false;
                    }

                    if (res.Von < reservation.Bis && res.Bis >= reservation.Bis)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void CheckReservationLenght(Reservation reservation)
        {
            int minReservationTimeSpan = 24;

            TimeSpan duration = reservation.Bis.Subtract(reservation.Von);

            if (duration.TotalHours < minReservationTimeSpan)
            {
                throw new InvaildDateRangException("Reservation needs to be more then 24h is: " +
                                                   duration.TotalHours.ToString() + " Hours");
            }
        }
    }
}