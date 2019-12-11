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

        public async Task<Auto> GetByPrimaryKey(int primaryKey) {
            using AutoReservationContext context = new AutoReservationContext();
            Auto foundAuto = context.Autos.Find(primaryKey);
            return foundAuto;
        }

        public async void AddAuto<T>(T auto) {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(auto).State = EntityState.Added;
            //context.Autos.Add(auto);
            context.SaveChanges();

        }

        public async void UpdateAuto(Auto auto) {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(auto).State = EntityState.Modified;
            context.SaveChanges();
        }

        public async void DeleteAuto(Auto auto) {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(auto).State = EntityState.Deleted;
            context.SaveChanges();
        }
    }
}