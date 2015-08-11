using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Commands.Infrastructure
{
    public static class CommandTypeProvider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "provider", Justification = "Parameter required by function")]
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            return from t in Assembly.GetExecutingAssembly().GetExportedTypes()
                   where t.IsClass && t.GetInterfaces().Where(i => i == typeof(ICommand)).Count() > 0
                   select t;
        }
    }
}
