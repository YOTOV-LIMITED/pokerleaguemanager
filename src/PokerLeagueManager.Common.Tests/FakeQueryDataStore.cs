using System;
using System.Collections.Generic;
using System.Linq;
using PokerLeagueManager.Common.DTO.Infrastructure;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Common.Tests
{
    public class FakeQueryDataStore : IQueryDataStore
    {
        private Dictionary<Type, List<IDataTransferObject>> dataStore = new Dictionary<Type, List<IDataTransferObject>>();

        public void Insert<T>(T dto) where T : class, IDataTransferObject
        {
            if (!dataStore.ContainsKey(typeof(T)))
            {
                dataStore.Add(typeof(T), new List<IDataTransferObject>());
            }

            dataStore[typeof(T)].Add(dto);
        }

        public IEnumerable<T> GetData<T>() where T : class, IDataTransferObject
        {
            if (!dataStore.ContainsKey(typeof(T)))
            {
                return new List<T>();
            }

            return dataStore[typeof(T)].Cast<T>();
        }

        public void Delete<T>(Guid dtoId) where T : class, IDataTransferObject
        {
            var dtoToDelete = dataStore[typeof(T)].Single(d => d.DtoId == dtoId);
            dataStore[typeof(T)].Remove(dtoToDelete);
        }

        public void Delete<T>(T dto) where T : class, IDataTransferObject
        {
            dataStore[typeof(T)].Remove(dto);
        }

        public int SaveChanges()
        {
            return 0;
        }
    }
}
