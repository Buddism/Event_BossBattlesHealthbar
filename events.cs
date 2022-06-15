//clear the boss health bar gui from the screen
registerOutputEvent(MiniGame, "clearBossHealthbar");

registerOutputEvent("Bot", "setBossHealthbar", "string 256 300\tstring 256 300\tlist 0 Do_Nothing 1 Clear_On_Death 2 Clear_on_Despawn");
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
	%this.BBHB_isTracking = true;

	%minigame.BBHB_init();

	cancel(%minigame.BBHB_tickSchedule);
	%minigame.BBHB_doHealthbarTick();
}

function AiPlayer::clearBossHealthbar(%this)
{
	%minigame = getMinigameFromObject(%this);
	if(isObject(%minigame))
		%minigame.BBHB_resetHealthbar();
}

function MiniGame::clearBossHealthbar(%this)
{
	%this.BBHB_resetHealthbar();
}