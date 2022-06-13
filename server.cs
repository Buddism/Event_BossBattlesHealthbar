//function clientCmdBBGUI(%action, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13) //Cramming this all into one function.
// ACTIONS:
//	handshake
//	setScore
//	closeGui
//	addClass
//	removeClass
//	setClassFull
//	setClassError
//	clearClasses
//	setItemPurchase
//	setBlockers
//	setTeam
//	setMinibossFull

//	newRound - RESETS GUI DATA
//	setInfoString - Sets the message telling who the boss is.
//		0, reset
//		1, currentBoss - %a2 = bossname, %a3 = typeOfBoss
//		2, choosing class - %a2 = bossname
//		3, straight assignment of infostring to %a2
