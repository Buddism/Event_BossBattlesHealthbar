package Event_BossBattles_Bot
{
	function GameConnection::onClientEnterGame(%this)
	{
		%parent = parent::onClientEnterGame(%this);

		//inform the client this server handles BBGUI
		commandToClient(%this, 'BBGui', "handshake");

		//need to create BossBattlesClassGroup to prevent console errors in "newRound"
		commandToClient(%this, 'BBGui', "clearClasses");
		commandToClient(%this, 'BBGui', "addClass", "");
		commandToClient(%this, 'BBGui', "setBlockers", 1, 1, 1, 1, 1, 1);
		%client.BBHB_resetHealthbar();

		return %parent;
	}

	function MiniGameSO::addMember (%minigame, %client)
	{
		%parent = parent::addMember(%minigame, %client);

		%client.BBHB_resetHealthbar();
		%client.BBHB_minigameAddMember(%minigame);

		return %parent;
	}
};
activatePackage(Event_BossBattles_Bot);