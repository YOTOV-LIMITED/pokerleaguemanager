using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Utilities
{
    public static class ReflectionHelper
    {
        public static IEnumerable<Type> FindHandlers<TCommand>(this Type handlerGenericType, Assembly assemblyToSearch)
        {
            if (assemblyToSearch == null)
            {
                throw new ArgumentNullException("assemblyToSearch");
            }

            return from t in assemblyToSearch.GetExportedTypes()
                   where t.IsClass &&
                         t.GetInterfaces().Where(i => i.IsGenericType &&
                                                 i.GetGenericTypeDefinition() == handlerGenericType &&
                                                 i.GetGenericArguments()[0] == typeof(TCommand)).Count() > 0
                   select t;
        }
    }
}
