package Event_BossBattles_Bot
{
	function GameConnection::autoAdminCheck(%this)
	{
		%parent = parent::autoAdminCheck(%this);

		//inform the client this server handles BBGUI
		commandToClient(%this, 'BBGui', "handshake");

		//need to create BossBattlesClassGroup to prevent console errors in "newRound"
		commandToClient(%this, 'BBGui', "clearClasses");
		commandToClient(%this, 'BBGui', "addClass", "");
		commandToClient(%this, 'BBGui', "setBlockers", 1, 1, 1, 1, 1, 1);
		%this.BBHB_resetHealthbar();

		return %parent;
	}

	function MiniGameSO::addMember (%minigame, %client)
	{
		%parent = parent::addMember(%minigame, %client);
		
		%client.BBHB_minigameAddMember(%minigame);

		return %parent;
	}

	function MinigameSO::removeMember(%minigame, %client)
	{
		%client.BBHB_resetHealthbar();
		return parent::removeMember(%minigame, %client);
	}
};
activatePackage(Event_BossBattles_Bot);