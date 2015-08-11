using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.Infrastructure;

namespace PokerLeagueManager.Queries.Core.Infrastructure
{
    public class QueryDataStore : DbContext, IQueryDataStore
    {
        public QueryDataStore()
            : base("default")
        {
            Database.SetInitializer<QueryDataStore>(null);
        }

        public void Insert<T>(T dto) where T : class, IDataTransferObject
        {
            var dtoSet = base.Set<T>();
            dtoSet.Add(dto);
            base.SaveChanges();
        }

        public IEnumerable<T> GetData<T>() where T : class, IDataTransferObject
        {
            var allICollections = typeof(T).GetProperties().Where(p => p.PropertyType.Name == typeof(ICollection<>).Name);
            var dtoCollections = allICollections.Where(c => typeof(IDataTransferObject).IsAssignableFrom(c.PropertyType.GenericTypeArguments.First()));
            var dtoMembers = typeof(T).GetProperties().Where(p => typeof(IDataTransferObject).IsAssignableFrom(p.PropertyType));

            DbQuery<T> results = base.Set<T>();

            foreach (var col in dtoCollections)
            {
                results = results.Include(col.Name);
            }

            foreach (var prop in dtoMembers)
            {
                results = results.Include(prop.Name);
            }

            return results.ToList();
        }

        public void Delete<T>(Guid dtoId) where T : class, IDataTransferObject
        {
            var dtoSet = base.Set<T>();
            var dtoToDelete = dtoSet.Single(d => d.DtoId == dtoId);
            dtoSet.Remove(dtoToDelete);
            base.SaveChanges();
        }

        public void Delete<T>(T dto) where T : class, IDataTransferObject
        {
            var dtoSet = base.Set<T>();
            dtoSet.Remove(dto);
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            AddAllDtoToModel(modelBuilder);
            modelBuilder.Types().Configure(x => x.ToTable(GetTableName(x.ClrType)));
            base.Configuration.LazyLoadingEnabled = false;

            modelBuilder.Entity<GetGameResultsDto>()
                .HasMany(e => e.Players)
                .WithRequired()
                .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }

        private string GetTableName(Type type)
        {
            var result = type.FullName.Substring(type.FullName.LastIndexOf(".") + 1);
            result = result.Replace("+", "_");

            return result;
        }

        private void AddAllDtoToModel(DbModelBuilder modelBuilder)
        {
            var dtoTypes = typeof(IDataTransferObject).Assembly.GetExportedTypes()
                             .Where(x => x.IsClass &&
                                         x.GetInterfaces().Contains(typeof(IDataTransferObject)) &&
                                         x.Name != "BaseDataTransferObject");

            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var dto in dtoTypes)
            {
                entityMethod.MakeGenericMethod(dto)
                            .Invoke(modelBuilder, new object[] { });
            }
        }
    }
}
