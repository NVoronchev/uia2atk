// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions: 
//  
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software. 
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//  Matt Guo <matt@mattguo.com>
//

using System;
using SW = System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using AEIds = System.Windows.Automation.AutomationElementIdentifiers;

using DC = Mono.UIAutomation.UiaDbus;
using Mono.UIAutomation.UiaDbus.Interfaces;

using DBus;

namespace Mono.UIAutomation.UiaDbusBridge.Wrappers
{
	public class MultipleViewPatternWrapper : IMultipleViewPattern
	{
#region Private Fields

		private IMultipleViewProvider provider;

#endregion

#region Constructor

		public MultipleViewPatternWrapper (IMultipleViewProvider provider)
		{
			this.provider = provider;
		}

#endregion

#region IMultipleViewPattern Members

		public string GetViewName (int viewId)
		{
			return provider.GetViewName (viewId);
		}

		public void SetCurrentView (int viewId)
		{
			provider.SetCurrentView (viewId);
		}

		public int[] GetSupportedViews ()
		{
			return provider.GetSupportedViews ();
		}

		public int CurrentView {
			get {
				return provider.CurrentView;
			}
		}

#endregion
	}
}
