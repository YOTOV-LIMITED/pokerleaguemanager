
if (Test-Path PokerLeagueManager.Commands.WCF -pathType container)
{
    cd PokerLeagueManager.Commands.WCF
    cmd /c PokerLeagueManager.Commands.WCF.deploy.cmd /Y
    cd ..
}

if (Test-Path PokerLeagueManager.DB.EventStore -pathType container)
{
    cd PokerLeagueManager.DB.EventStore
    cmd /c "C:\Program Files (x86)\Microsoft SQL Server\110\DAC\bin\sqlpackage.exe" `
        /Action:Publish `
        /SourceFile:PokerLeagueManager.DB.EventStore.dacpac `
		/Profile:PokerLeagueManager.DB.EventStore.publish.xml
    cd ..
}

if (Test-Path PokerLeagueManager.DB.QueryStore -pathType container)
{
    cd PokerLeagueManager.DB.QueryStore
    cmd /c "C:\Program Files (x86)\Microsoft SQL Server\110\DAC\bin\sqlpackage.exe" `
        /Action:Publish `
        /SourceFile:PokerLeagueManager.DB.QueryStore.dacpac `
		/Profile:PokerLeagueManager.DB.QueryStore.publish.xml
    cd ..
}

if (Test-Path PokerLeagueManager.Queries.WCF -pathType container)
{
    cd PokerLeagueManager.Queries.WCF
    cmd /c PokerLeagueManager.Queries.WCF.deploy /Y
    cd ..
}

if (Test-Path PokerLeagueManager.UI.WPF -pathType container)
{
    if (Test-Path C:\PokerLeagueManager.UI.WPF -pathType container)
	{
	    rmdir C:\PokerLeagueManager.UI.WPF -recurse -force
	}
	copy -path PokerLeagueManager.UI.WPF -dest C:\ -recurse
}

if (Test-Path PokerLeagueManager.Utilities.ProcessEvents -pathType container)
{
	cd PokerLeagueManager.Utilities.ProcessEvents
	cmd /c PokerLeagueManager.Utilities.ProcessEvents.exe
	cd ..
}