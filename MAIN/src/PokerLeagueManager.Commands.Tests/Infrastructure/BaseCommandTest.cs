using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Common.Commands.Infrastructure;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Common.Tests;
using PokerLeagueManager.Common.Utilities;
using PokerLeagueManager.Queries.Core;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Commands.Tests.Infrastructure
{
    [TestClass]
    public abstract class BaseCommandTest
    {
        public virtual void Setup()
        {
        }

        public virtual IEnumerable<IEvent> Given()
        {
            return new List<IEvent>();
        }

        public virtual IEnumerable<IEvent> ExpectedEvents()
        {
            return new List<IEvent>();
        }

        public virtual Exception ExpectedException()
        {
            return null;
        }

        public void RunTest(ICommand command)
        {
            Setup();

            var repository = new FakeEventRepository();
            repository.InitialEvents = Given();

            var queryDataStore = new FakeQueryDataStore();
            var queryService = new QueryHandler(queryDataStore);

            var commandRepository = new Mock<ICommandRepository>();

            HandleEvents(repository.InitialEvents, queryDataStore);
            
            Exception caughtException = null;
            var commandHandlerFactory = new CommandHandlerFactory(repository, queryService, commandRepository.Object);

            try
            {
                commandHandlerFactory.ExecuteCommand(command);
            }
            catch (Exception e)
            {
                if (ExpectedException() == null)
                {
                    throw;
                }

                caughtException = e;
            }

            if (caughtException != null || ExpectedException() != null)
            {
                if (caughtException != null && ExpectedException() != null)
                {
                    Assert.AreEqual(ExpectedException().GetType(), caughtException.GetType());
                }
                else
                {
                    Assert.Fail("There was an Expected Exception but none was thrown.");
                }
            }

            ValidateExpectedEvents(ExpectedEvents(), repository.EventList);
        }

        public Guid AnyGuid()
        {
            return ObjectComparer.AnyGuid();
        }

        private void ValidateExpectedEvents(IEnumerable<IEvent> expected, IEnumerable<IEvent> actual)
        {
            var expectedSegment = new List<IEvent>();
            var actualSegment = new List<IEvent>();
            int i = 0;

            foreach (var e in expected)
            {
                if (e is VerifyEventsNow)
                {
                    ObjectComparer.AreEqual(expectedSegment, actualSegment, false);
                    expectedSegment = new List<IEvent>();
                    actualSegment = new List<IEvent>();
                }
                else
                {
                    expectedSegment.Add(e);

                    if (i < actual.Count())
                    {
                        actualSegment.Add(actual.ElementAt(i++));
                    }
                }
            }

            for (int x = i; x < actual.Count(); x++)
            {
                actualSegment.Add(actual.ElementAt(x));
            }

            ObjectComparer.AreEqual(expectedSegment, actualSegment, false);
        }

        private void HandleEvents(IEnumerable<IEvent> events, IQueryDataStore queryDataStore)
        {
            var mockIdempotencyChecker = new Mock<IIdempotencyChecker>();
            var mockDatabaseLayer = new Mock<IDatabaseLayer>();

            mockDatabaseLayer.Setup(x => x.ExecuteInTransaction(It.IsAny<Action>())).Callback<Action>(x => x());

            var eventHandler = new EventHandlerFactory(queryDataStore, mockIdempotencyChecker.Object, mockDatabaseLayer.Object);

            foreach (IEvent e in events)
            {
                eventHandler.HandleEvent(e);
            }
        }
    }
}
