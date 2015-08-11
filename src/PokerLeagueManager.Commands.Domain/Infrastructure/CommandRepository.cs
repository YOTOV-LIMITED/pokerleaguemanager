using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public class CommandRepository : ICommandRepository
    {
        private IDatabaseLayer _databaseLayer;

        public CommandRepository(IDatabaseLayer databaseLayer)
        {
            _databaseLayer = databaseLayer;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:ParameterMustFollowComma", Justification = "For the DatabaseLayer calls this makes more sense.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public void LogCommand(Common.Commands.Infrastructure.ICommand command)
        {
            _databaseLayer.ExecuteNonQuery(
                "INSERT INTO Commands(CommandId, CommandDateTime, UserName, IpAddress, CommandData) VALUES(@CommandId, @CommandDateTime, @UserName, @IpAddress, @CommandData)",
                "@CommandId", command.CommandId.ToString(),
                "@CommandDateTime", command.Timestamp.ToUniversalTime().ToString("dd-MMM-yyyy HH:mm:ss.ff"),
                "@UserName", command.User,
                "@IpAddress", command.IPAddress,
                "@CommandData", SerializeCommand(command));
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:ParameterMustFollowComma", Justification = "For the DatabaseLayer calls this makes more sense.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public void LogFailedCommand(Common.Commands.Infrastructure.ICommand command, Exception ex)
        {
            _databaseLayer.ExecuteNonQuery(
                "UPDATE Commands SET ExceptionDetails = @ExceptionDetails WHERE CommandId = @CommandId",
                "@ExceptionDetails", ex.ToString(),
                "@CommandId", command.CommandId.ToString());
        }

        private string SerializeCommand<T>(T e)
        {
            var serializer = new DataContractSerializer(e.GetType());

            using (var memStream = new MemoryStream())
            {
                serializer.WriteObject(memStream, e);

                memStream.Position = 0;
                var reader = new StreamReader(memStream);

                return reader.ReadToEnd();
            }
        }
    }
}
