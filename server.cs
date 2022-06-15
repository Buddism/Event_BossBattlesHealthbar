//function clientCmdBBGUI(%action, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13) //Cramming this all into one function.
// ACTIONS:
//	setScore
//	closeGui
//	removeClass
//	clearClasses
//	setClassFull
//	setClassError
//	setItemPurchase
//	setTeam
//	setMinibossFull

//	setBlockers
//	addClass
//	handshake

//	newRound - RESETS GUI DATA (AND CLOSES THE HEALTHBAR)
//	setInfoString - Sets the message telling who the boss is. (real INFOSTRING is for the class selection gui)
//			0, clear the INFOSTRING
//			3, straight assignment of INFOSTRING to %a2
//		1, %a2 = bossName	%a3 = bossClass (INCREMENTS BOSSCOUNT) (ACTIVATES THE HEALTHBAR)
//		2, %a2 = bossName			INFOSTRING:choosing class



exec("./package.cs");
exec("./events.cs");

function GameConnection::BBHB_setInfoString(%this, %type, %str0, %str1)
{
	commandToClient(%this, 'BBGUI', "setInfoString", %type, %str0, %str1);
}

function GameConnection::BBHB_resetHealthbar(%this)
{
	commandToClient(%this, 'BBGui', "newRound");
}

function MinigameSO::BBHB_resetHealthbar(%this)
{
	%this.BBHB_isTracking = false;
	for(%i = %this.numMembers - 1; %i >= 0; %i--)
		%this.member[%i].BBHB_resetHealthbar();
}


//need to display the healthbar if they join late and we are still tracking it
function GameConnection::BBHB_minigameAddMember(%this, %minigame)
{
	%trackObj = %minigame.trackObj;
	%name = %minigame.BBHB_trackName;
	%class = %minigame.BBHB_trackClass;

	%this.BBHB_resetHealthbar();

	//increment the healthbar count and set the name/class
	%this.BBHB_setInfoString(1, %name, %class);

	if(%minigame.BBHB_isTracking)
	{
		%healthStr = %minigame.BBHB_lastTrackStr;
		%this.bottomPrint(%healthStr, 5);
	}
}

function MinigameSO::BBHB_init(%this)
{
	%name = %this.BBHB_trackName;
	%class = %this.BBHB_trackClass;

	for(%i = %this.numMembers - 1; %i >= 0; %i--)
	{
		%this.member[%i].BBHB_resetHealthbar();

		//increment the healthbar count and set the name/class
		%this.member[%i].BBHB_setInfoString(1, %name, %class);
	}
}

function GameConnection::BBHB_setInfo(%this, %name, %health, %percentage)
{
	%bossStr = "Boss Health (" @ %name @"): "@ %percentage SPC %health;
	commandToClient(%this, 'bottomPrint', %bossStr, 3, 1);
}

function MinigameSO::BBHB_getHealthBarString(%this)
{
	%target = %this.BBHB_trackObj;
	%msg = "\c5Boss Health";

	%name = %this.BBHB_trackName;
	%msg = %msg SPC "(" @ %name @ "\c5):";
	
	%hpPercent = mCeil(100 - %target.getDamagePercent() * 100);
	%hpMax = %target.getDatablock().maxDamage;
	%hpTotal = %hpMax - %target.getDamageLevel();

	if(%target.tf2Overheal > 0)
		%hpTotal += %target.tf2Overheal;

	%msg = %msg SPC "\c6" @ %hpPercent @ "%";

	%hpTotal = mCeil(%hpTotal);
	%hpMax = mCeil(%hpMax);

	%msg = %msg SPC "\c3" @ %hpTotal @ "/" @ %hpMax;

	return %msg;
}

//robbed from the boss battles add-on dump "function Slayer_OMA_printBossHealth(%mini, %player)"
function MinigameSO::BBHB_printBossHealth(%this)
{
	if(!isObject(%this.trackObj))
		%healthStr = %this.BBHB_lastTrackStr;
	else {
		%healthStr = %this.BBHB_getHealthBarString();
		%this.BBHB_lastTrackStr = %healthStr;
	}
	
	%this.bottomPrintAll(%healthStr, 5);
}

function MinigameSO::BBHB_doHealthbarTick(%this)
{
	cancel(%this.BBHB_tickSchedule);

	%trackObj = %this.BBHB_trackObj;
	%trackClearType = %this.BBHB_trackClearType;

	if(!isObject(%trackObj))
	{
		// no-object state & trackClearType = Clear_On_Despawn
		if(%trackClearType > 0)
			%this.BBHB_resetHealthbar();

		return;
	}

	%this.BBHB_printBossHealth(%trackObj);

	// disabled/dead state & trackClearType = Clear_On_Death
	if(%trackObj.isDisabled() && %trackClearType == 1)
	{
		%this.BBHB_resetHealthbar();
		return;
	}

	%this.BBHB_tickSchedule = %this.schedule(31, BBHB_doHealthbarTick);
}