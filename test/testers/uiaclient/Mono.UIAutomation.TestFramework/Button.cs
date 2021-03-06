// Button.cs: Button control class wrapper.
//
// This program is free software; you can redistribute it and/or modify it under
// the terms of the GNU General Public License version 2 as published by the
// Free Software Foundation.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License along with
// this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
//
// Copyright (c) 2010 Novell, Inc (http://www.novell.com)
//
// Authors:
//	Ray Wang  (rawang@novell.com)
//	Felicia Mu  (fxmu@novell.com)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace Mono.UIAutomation.TestFramework
{
	// The wrapper class of Button class.
	public class Button : Element
	{
		public static readonly ControlType UIAType = ControlType.Button;
		
		//List the patterns that the control must support
		public override List<AutomationPattern> SupportedPatterns {
			get { return null; }
		}

		public Button (AutomationElement elm)
			: base (elm)
		{
		}
		
		#region Invoke Pattern
		// Perform "Click" action.
		public void Click ()
		{
			Click (true);
		}

		public void Click (bool log)
		{
			if (log)
				procedureLogger.Action (string.Format ("Click {0}.", this.NameAndType));

			InvokePattern ip = (InvokePattern) element.GetCurrentPattern (InvokePattern.Pattern);
			ip.Invoke ();
		}
		#endregion
	}
}