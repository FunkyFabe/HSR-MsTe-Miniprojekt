using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager
        : ManagerBase
    {
        public async Task<List<Kunde>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Kunden.ToListAsync();
        }

        public async Task<Kunde> GetByPrimaryKey(int primaryKey)
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Kunden.FindAsync(primaryKey);
        }
    }
}