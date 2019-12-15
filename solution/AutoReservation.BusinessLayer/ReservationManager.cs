using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
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
            return await context.Reservationen.FindAsync(primaryKey);
        }

        public void ReservationPossible(Reservation reservation, int autoID)
        {

            CheckReservationLenght(reservation);

            //Get All Reservations for a Car
            List<Reservation> allReservations = GetAll().Result;
            List<Reservation> allReservationsForOneCar = allReservations.FindAll(
                delegate (Reservation re)
                {
                    return re.AutoId == autoID;
                });

            foreach (Reservation res in allReservationsForOneCar)
            {
                if (res.Von <= reservation.Von && res.Bis > reservation.Von)
                {
                    throw new InvaildDateRangException("Starpoint Reservation in between");
                }
                if (res.Von < reservation.Bis && res.Bis >= reservation.Bis)
                {
                    throw new InvaildDateRangException("Endpoint Reservation in between");
                }
            }
        }

        private void CheckReservationLenght(Reservation reservation)
        {

            int minReservationTimeSpan = 24;

            TimeSpan duration = reservation.Bis.Subtract(reservation.Von);

            if (duration.TotalHours < minReservationTimeSpan)
            {
                throw new InvaildDateRangException("Reservation needs to be more then 24h is: " + duration.TotalHours.ToString() + " Hours");
            }
        }
    }
}