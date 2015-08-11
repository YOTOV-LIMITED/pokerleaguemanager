using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.Infrastructure;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Utilities;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Infrastructure.Tests
{
    [TestClass]
    public class QueryEventHandlerTests
    {
        [TestMethod]
        public void ProcessNewEvent()
        {
            var testEvent = new GameCreatedEvent();
            var mockIdempotencyChecker = new Mock<IIdempotencyChecker>();
            var mockQueryDataStore = new Mock<IQueryDataStore>();
            var mockDatabaseLayer = new Mock<IDatabaseLayer>();

            mockDatabaseLayer.Setup(x => x.ExecuteInTransaction(It.IsAny<Action>())).Callback<Action>(x => x());

            mockIdempotencyChecker.Setup(x => x.CheckIdempotency(testEvent.EventId)).Returns(false);

            var sut = new EventHandlerFactory(mockQueryDataStore.Object, mockIdempotencyChecker.Object, mockDatabaseLayer.Object);

            sut.HandleEvent(testEvent);

            mockQueryDataStore.Verify(x => x.Insert(It.IsAny<GetGameCountByDateDto>()));
        }

        [TestMethod]
        public void ProcessDuplicateEvent()
        {
            var testEvent = new GameCreatedEvent();
            var mockIdempotencyChecker = new Mock<IIdempotencyChecker>();
            var mockQueryDataStore = new Mock<IQueryDataStore>();

            mockIdempotencyChecker.Setup(x => x.CheckIdempotency(testEvent.EventId)).Returns(true);

            var sut = new EventHandlerFactory(mockQueryDataStore.Object, mockIdempotencyChecker.Object, null);

            sut.HandleEvent(testEvent);

            mockQueryDataStore.Verify(x => x.Insert(It.IsAny<GetGameCountByDateDto>()), Times.Never());
        }
    }
}
