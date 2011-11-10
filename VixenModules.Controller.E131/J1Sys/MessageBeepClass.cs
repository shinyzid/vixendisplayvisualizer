﻿using System;
using System.Runtime.InteropServices;

namespace J1Sys
{
	public class MessageBeepClass
	{
	//-------------------------------------------------------------
	//
	//	MessageBeep()
	//
	//-------------------------------------------------------------
	
	public enum BeepType
	{
		SimpleBeep		= -1,
		IconAsterisk	= 0x00000040,
		IconExclamation = 0x00000030,
		IconHand		= 0x00000010,
		IconQuestion	= 0x00000020,
		Ok				= 0x00000000
	}

	[DllImport("user32.dll")]
	public static extern bool MessageBeep(BeepType beepType);
	}
}
