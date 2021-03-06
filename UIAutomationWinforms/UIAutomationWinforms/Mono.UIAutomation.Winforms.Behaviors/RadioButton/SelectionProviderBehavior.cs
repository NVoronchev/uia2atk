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
//      Sandy Armstrong <sanfordarmstrong@gmail.com>
// 


using System;
using SWF = System.Windows.Forms;

using System.Windows.Automation;
using System.Windows.Automation.Provider;

namespace Mono.UIAutomation.Winforms.Behaviors.RadioButton
{
	// TODO: Should this class subclass from ProviderBehavior?
	// This behavior is designed to be attached to a control that contains
	// RadioButton children.
	internal class SelectionProviderBehavior : IProviderBehavior, ISelectionProvider
	{
#region Private Members
		
		private SWF.Control control;
#endregion

#region Constructor

		public SelectionProviderBehavior (FragmentRootControlProvider rootProvider)
		{
			control = rootProvider.Control;
		}
		
#endregion
		
#region ISelectionProvider Overrides

		public bool CanSelectMultiple {
			get { return false; }
		}

		public bool IsSelectionRequired {
			get { return true; }
		}

		public IRawElementProviderSimple[] GetSelection ()
		{
			foreach (SWF.Control childControl in control.Controls) {
				IRawElementProviderSimple childProvider =
					ProviderFactory.GetProvider (childControl);
				
				ISelectionItemProvider selectionItem = null;
				if (childProvider != null)
					selectionItem = childProvider.GetPatternProvider (SelectionItemPatternIdentifiers.Pattern.Id)
						as ISelectionItemProvider;
				
				if (selectionItem != null &&
				    selectionItem.IsSelected)
					return new IRawElementProviderSimple[] { childProvider };
			}
			
			return new IRawElementProviderSimple [0];
		}

#endregion
		
#region IProviderBehavior Overrides
		
		public void Disconnect ()
		{
			
		}

		public void Connect ()
		{
			
		}

		public object GetPropertyValue (int propertyId)
		{
			if (propertyId == SelectionPatternIdentifiers.CanSelectMultipleProperty.Id)
				return CanSelectMultiple;
			else if (propertyId == SelectionPatternIdentifiers.IsSelectionRequiredProperty.Id)
				return IsSelectionRequired;
			else if (propertyId == SelectionPatternIdentifiers.SelectionProperty.Id)
				return GetSelection ();
			else
				return null;
		}
		
		public AutomationPattern ProviderPattern {
			get {
				return SelectionPatternIdentifiers.Pattern;
			}
		}

#endregion
	}
}
