using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public abstract class ManagerBase
    {
        //Mebey here as Generic makes it easier
        protected static OptimisticConcurrencyException<T> CreateOptimisticConcurrencyException<T>(AutoReservationContext context, T entity)
            where T : class
        {
            T dbEntity = (T)context.Entry(entity)
                .GetDatabaseValues()
                .ToObject();

            return new OptimisticConcurrencyException<T>($"Update {typeof(T).Name}: Concurrency-Fehler", dbEntity);
        }

        public async Task<T> AddEntity<T>(T entity)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(entity).State = EntityState.Added;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateEntity<T>(T entity)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteEntity<T>(T entity)
        {
            using AutoReservationContext context = new AutoReservationContext();
            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
    }
}