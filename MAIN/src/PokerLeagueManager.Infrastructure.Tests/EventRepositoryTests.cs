using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerLeagueManager.Commands.Domain.Aggregates;
using PokerLeagueManager.Commands.Domain.Exceptions;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Commands.Domain.Infrastructure.Exceptions;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Infrastructure.Tests
{
    [TestClass]
    public class EventRepositoryTests
    {
        [TestMethod]
        public void AggregateVersionIsSetToLatestEventId()
        {
            var testAggregateId = Guid.NewGuid();
            
            var firstEvent = new GameCreatedEvent() { AggregateId = testAggregateId, GameDate = DateTime.Now };
            var secondEvent = new PlayerAddedToGameEvent() { AggregateId = testAggregateId, PlayerName = "Dylan", Placing = 1, Winnings = 150 };

            var testEvents = CreateEventsTable();
            testEvents.Rows.Add(SerializeEvent(firstEvent), firstEvent.GetType().AssemblyQualifiedName);
            testEvents.Rows.Add(SerializeEvent(secondEvent), secondEvent.GetType().AssemblyQualifiedName);

            var mockDatabaseLayer = new Mock<IDatabaseLayer>();
            mockDatabaseLayer.Setup(x => x.GetDataTable(It.IsAny<string>(), It.IsAny<object[]>())).Returns(testEvents);

            var sut = new EventRepository(
                mockDatabaseLayer.Object, 
                null, 
                null,
                null);

            var result = sut.GetAggregateById<Game>(testAggregateId);

            Assert.AreEqual(secondEvent.EventId, result.AggregateVersion);
        }

        [TestMethod]
        [ExpectedException(typeof(OptimisticConcurrencyException))]
        public void PublishEventsWithAnOldVersionOfTheAggregateThrowsConcurrencyException()
        {
            Game testGame = (Game)System.Activator.CreateInstance(typeof(Game), true);
            testGame.AggregateId = Guid.NewGuid();

            var originalVersion = Guid.NewGuid();

            var testEvent = new PlayerAddedToGameEvent() { AggregateId = testGame.AggregateId, PlayerName = "Dylan", Placing = 1, Winnings = 150 };
            testGame.PendingEvents.Add(testEvent);

            var sut = new EventRepository(null, null, null, null);

            sut.PublishEvents(testGame, null, originalVersion);
        }

        [TestMethod]
        public void PublishEventsWithAnOldVersionOfTheAggregateButNoEventsToPublishShouldSucceed()
        {
            Game testGame = (Game)System.Activator.CreateInstance(typeof(Game), true);
            testGame.AggregateId = Guid.NewGuid();

            var originalVersion = Guid.NewGuid();

            var sut = new EventRepository(null, null, null, null);

            sut.PublishEvents(testGame, null, originalVersion);
        }

        private DataTable CreateEventsTable()
        {
            var result = new DataTable();

            try
            {
                result.Columns.Add("EventData", typeof(string));
                result.Columns.Add("EventType", typeof(string));

                return result;
            }
            catch
            {
                result.Dispose();
                throw;
            }
        }

        private string SerializeEvent<T>(T e)
        {
            var serializer = new DataContractSerializer(e.GetType());

            using (var memStream = new System.IO.MemoryStream())
            {
                serializer.WriteObject(memStream, e);

                memStream.Position = 0;
                var reader = new StreamReader(memStream);

                return reader.ReadToEnd();
            }
        }
    }
}
