DELETE FROM Subscribers

IF '$(PublishEnvironment)' = 'dev'
BEGIN
INSERT INTO Subscribers(SubscriberId, SubscriberUrl) VALUES(newid(), 'http://localhost:1766/Infrastructure/EventService.svc')
END

IF '$(PublishEnvironment)' = 'Local'
BEGIN
INSERT INTO Subscribers(SubscriberId, SubscriberUrl) VALUES(newid(), 'http://localhost:83/PokerLeagueManager.Queries.WCF/Infrastructure/EventService.svc')
END

IF '$(PublishEnvironment)' = 'Build'
BEGIN
INSERT INTO Subscribers(SubscriberId, SubscriberUrl) VALUES(newid(), 'http://localhost:783/PokerLeagueManager.Queries.WCF/Infrastructure/EventService.svc')
END