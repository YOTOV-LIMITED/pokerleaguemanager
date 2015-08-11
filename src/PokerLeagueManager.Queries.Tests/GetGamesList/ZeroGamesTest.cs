using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Common.DTO.Infrastructure;
using PokerLeagueManager.Queries.Tests.Infrastructure;

namespace PokerLeagueManager.Queries.Tests
{
    [TestClass]
    public class ZeroGamesTest : BaseQueryTest
    {
        [TestMethod]
        public void GetGamesList_ZeroGames()
        {
            RunTest(x => x.GetGamesList());
        }

        public override IEnumerable<IDataTransferObject> ExpectedDtos()
        {
            return new List<IDataTransferObject>();
        }
    }
}
