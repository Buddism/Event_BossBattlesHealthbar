//clear the boss health bar gui from the screen
registerOutputEvent(MiniGame, "clearBossHealthbar");

registerOutputEvent("Bot", "setBossHealthbar", "string 200 300\tstring 200 300\tlist Do_Nothing 0 Clear_On_Death 1 Clear_on_Despawn 2");
registerOutputEvent("Bot", "clearHealthbar", "");

function AiPlayer::setBossHealthbar(%this, %bossName, %bossClass, %clearSetting, %client)
{
	%minigame = getMinigameFromObject(%this);
	if(!isObject((%minigame)))
		return;

	%minigame.BBHB_trackObj = %this;
	%minigame.BBHB_trackName = %bossName;
	%minigame.BBHB_trackClass = %bossClass;
	%minigame.BBHB_trackClearType = %clearSetting;
	%minigame.BBHB_isTracking = true;

	%minigame.BBHB_init();
	
	%this.BBHB_lastTrackMinigame = %minigame;

	cancel(%minigame.BBHB_tickSchedule);
	%minigame.BBHB_doHealthbarTick();
}

function AiPlayer::clearBossHealthbar(%this)
{
	%minigame = getMinigameFromObject(%this);
	if(!isObject(%minigame))
	{
		%minigame = %this.BBHB_lastTrackMinigame;
		if(%minigame.BBHB_trackObj != %this || !%minigame.BBHB_isTracking)
			return;
	}

	if(isObject(%minigame))
		%minigame.BBHB_resetHealthbar();
}

function MiniGameSO::clearBossHealthbar(%this)
{
	%this.BBHB_resetHealthbar();
}