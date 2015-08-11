using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.Infrastructure;

namespace PokerLeagueManager.Infrastructure.Tests
{
    [TestClass]
    public class DtoFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullDataRow()
        {
            var sut = new DtoFactory();

            sut.Create<GetGameCountByDateDto>(null);
        }

        [TestMethod]
        public void SampleDtoFromDataRow()
        {
            var gameId = Guid.NewGuid();
            var gameYear = 2010;
            var gameMonth = 7;
            var gameDay = 3;

            var sampleTable = GenerateSampleDataTable();
            sampleTable.Rows.Add(gameId, gameYear, gameMonth, gameDay);

            var sut = new DtoFactory();
            var result = sut.Create<GetGameCountByDateDto>(sampleTable.Rows[0]);

            Assert.IsNotNull(result);
            Assert.AreEqual(gameId, result.GameId);
            Assert.AreEqual(gameYear, result.GameYear);
            Assert.AreEqual(gameMonth, result.GameMonth);
            Assert.AreEqual(gameDay, result.GameDay);
        }

        private DataTable GenerateSampleDataTable()
        {
            var result = new DataTable();

            try
            {
                result.Columns.Add("GameId", typeof(Guid));
                result.Columns.Add("GameYear", typeof(int));
                result.Columns.Add("GameMonth", typeof(int));
                result.Columns.Add("GameDay", typeof(int));

                return result;
            }
            catch
            {
                result.Dispose();
                throw;
            }
        }
    }
}
