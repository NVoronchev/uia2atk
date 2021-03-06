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
// Copyright (c) 2008 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//	Sandy Armstrong <sanfordarmstrong@gmail.com>
// 

using System;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using SWF = System.Windows.Forms;
using Mono.UIAutomation.Winforms;

namespace Mono.UIAutomation.Winforms.Events.TreeView.TreeNode
{
	
	internal class AutomationHasKeyboardFocusPropertyEvent 
		: BaseAutomationPropertyEvent
	{
		#region Private Members

		private SWF.TreeView treeView;

		#endregion
		
		#region Constructors

		public AutomationHasKeyboardFocusPropertyEvent (TreeNodeProvider nodeProvider) 
			: base (nodeProvider, 
			        AutomationElementIdentifiers.HasKeyboardFocusProperty)
		{
			treeView = nodeProvider.TreeNode.TreeView;
		}
		
		#endregion
		
		#region IConnectable Overrides		 
		
		public override void Connect ()
		{
			if (treeView != null) {
				treeView.AfterSelect += HandleAfterSelect;
				treeView.LostFocus += OnFocus;
				treeView.GotFocus += OnFocus;
			}
		}

		public override void Disconnect ()
		{
			if (treeView != null) {
				treeView.AfterSelect -= HandleAfterSelect;
				treeView.LostFocus -= OnFocus;
				treeView.GotFocus -= OnFocus;
			}
		}
		
		#endregion
		
		#region Private Methods

		private void HandleAfterSelect(object sender, SWF.TreeViewEventArgs e)
		{
			RaiseAutomationPropertyChangedEvent ();
		}
		
		private void OnFocus (object sender, EventArgs e)
		{
			RaiseAutomationPropertyChangedEvent ();
		}
		
		#endregion 
		
	}
}
