using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoReservation.BusinessLayer
{
    public class AutoManager
        : ManagerBase
    {
        public async Task<List<Auto>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Autos.ToListAsync();
        }

        public Auto GetByPrimaryKey(int primaryKey) {
            using AutoReservationContext context = new AutoReservationContext();
            Auto foundAuto = context.Autos.Find(primaryKey);
            return foundAuto;
        }

        public void AddAuto(Auto auto) {
            using AutoReservationContext context = new AutoReservationContext();
            context.Autos.Add(auto);
            context.SaveChanges();

        }
    }
}