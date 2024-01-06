using kurs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace kurs.Services
{
    public abstract class EntityManager<TEntity, TForm> where TEntity : class where TForm : class
    {
        protected readonly DatabaseContext _context;

        public EntityManager(DatabaseContext context)
        {
            _context = context;
        }

        public abstract TForm CreateForm(TEntity entity);
        public abstract TEntity CreateEntity(TForm form);
        public abstract Task Update(TEntity entity, TForm form);

        public virtual List<TEntity> GetEntities()
        {
            return _context.Set<TEntity>().ToList();
        }
        public virtual TEntity? GetEntity(int? id)
        {
            return _context.Find<TEntity>(id);
        }
        public virtual async Task<List<TEntity>> GetEntitiesAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        public virtual async Task<TEntity?> GetEntityAsync(int? id)
        {
            return await _context.FindAsync<TEntity>(id);
        }
        public virtual async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            var result = _context.Add(entity);
            await _context.SaveChangesAsync();
            return result;
        }
        public virtual async Task<EntityEntry<TEntity>> AddAsync(TForm form)
        {
            var entity = CreateEntity(form);
            return await AddAsync(entity);
        }
        public virtual async Task<EntityEntry<TEntity>> RemoveAsync(TEntity entity)
        {
            var result = _context.Remove(entity);
            await _context.SaveChangesAsync();
            return result;
        }
    }

    public static class EntityManagerExtensions
    {
        public static IServiceCollection AddEntityManagers(this IServiceCollection services)
        {
            services.AddScoped<CountryManager>();
            services.AddScoped<CityManager>();
            services.AddScoped<TagGroupManager>();
            services.AddScoped<TagManager>();
            services.AddScoped<ShippingManager>();
            services.AddScoped<CategoryManager>();
            services.AddScoped<CompanyManager>();
            services.AddScoped<ProductManager>();
            services.AddScoped<ReviewManager>();
            return services;
        }
    }

}
