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
//      Mario Carrion <mcarrion@novell.com>
// 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;

using Mono.UIAutomation.Winforms.Behaviors;
using Mono.UIAutomation.Winforms.Events;

namespace Mono.UIAutomation.Winforms
{
	public abstract class SimpleControlProvider : IRawElementProviderSimple
	{
#region Protected Fields
		
		protected Control control;
		
#endregion
		
#region Private Fields

		private Dictionary<EventStrategyType, IEventStrategy> events;
		private Dictionary<AutomationPattern, IProviderBehavior> providerBehaviors;

#endregion
		
#region Constructors
		
		protected SimpleControlProvider (Control control)
		{
			this.control = control;
			
			events = new Dictionary<EventStrategyType,IEventStrategy> ();
			providerBehaviors =
				new Dictionary<AutomationPattern,IProviderBehavior> ();
		}
		
#endregion
		
#region Public

		public virtual void InitializeEvents ()
		{
			// TODO: Add: EventStrategyType.IsOffscreenProperty, DefaultIsOffscreenPropertyEvent
			SetEvent (EventStrategyType.IsEnabledProperty, 
			          new DefaultIsEnabledPropertyEvent (this, control));
			SetEvent (EventStrategyType.NameProperty,
			          new DefaultNamePropertyEvent (this, control));
			SetEvent (EventStrategyType.HasKeyboardFocusProperty,
			          new DefaultHasKeyboardFocusPropertyEvent (this, control));
			SetEvent (EventStrategyType.BoundingRectangleProperty,
			          new DefaultBoundingRectanglePropertyEvent (this, control));
			SetEvent (EventStrategyType.StructureChangedEvent,
			          new DefaultStructureChangedEvent (this, control));
		}
		
#endregion
		
#region Protected
				
		protected void SetEvent (EventStrategyType type, IEventStrategy strategy)
		{
			IEventStrategy value;
			
			if (events.TryGetValue (type, out value) == true) {			
				value.Disconnect ();
				events.Remove (type);
			}

			if (strategy != null) {
				events [type] = strategy;
				strategy.Connect ();
			}
		}
		
		protected void SetBehavior (AutomationPattern pattern, IProviderBehavior behavior)
		{
			IProviderBehavior oldBehavior;
			if (providerBehaviors.TryGetValue (pattern, out oldBehavior) == true) {
				oldBehavior.Dispose ();
				providerBehaviors.Remove (pattern);
			}
			
			if (behavior != null) {
				providerBehaviors [pattern] = behavior;
				behavior.Initialize (control);
			}
		}
		
		protected IProviderBehavior GetBehavior (AutomationPattern pattern)
		{
			IProviderBehavior behavior;
			if (providerBehaviors.TryGetValue (pattern, out behavior))
				return behavior;
			
			return null;
		}
		
		protected IEnumerable<IProviderBehavior> ProviderBehaviors
		{
			get {
				return providerBehaviors.Values;
			}
		}
#endregion
		
#region IRawElementProviderSimple Members
	
		// TODO: Get this used in all base classes. Consider refactoring
		//       so that *all* pattern provider behaviors are dynamically
		//       attached to make this more uniform.
		public virtual object GetPatternProvider (int patternId)
		{
			foreach (IProviderBehavior behavior in ProviderBehaviors)
				if (behavior.ProviderPattern.Id == patternId)
					return behavior;
			return null;
		}
		
		public virtual object GetPropertyValue (int propertyId)
		{
			if (propertyId == AutomationElementIdentifiers.AutomationIdProperty.Id)
				return control.GetHashCode (); // TODO: Ensure uniqueness
			else if (propertyId == AutomationElementIdentifiers.IsEnabledProperty.Id)
				return control.Enabled;
			else if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
				return control.Text;
			else if (propertyId == AutomationElementIdentifiers.IsKeyboardFocusableProperty.Id)
				return control.CanFocus;
			else if (propertyId == AutomationElementIdentifiers.IsOffscreenProperty.Id)
				return !control.Visible;
			else if (propertyId == AutomationElementIdentifiers.HasKeyboardFocusProperty.Id)
				return control.Focused;
			else if (propertyId == AutomationElementIdentifiers.IsControlElementProperty.Id)
				return true;
			else if (propertyId == AutomationElementIdentifiers.IsContentElementProperty.Id)
				return true;
			else if (propertyId == AutomationElementIdentifiers.BoundingRectangleProperty.Id) {
				if (control.Parent == null)
					return Helper.RectangleToRect (control.Bounds);
				else
					return Helper.RectangleToRect (control.Parent.RectangleToScreen (control.Bounds));
			} else if (propertyId == AutomationElementIdentifiers.ClickablePointProperty.Id) {
				if (control.Visible == false)
					return null;
				else {
					// TODO: Test. MS behavior is different.
					Rect rectangle = (Rect) GetPropertyValue (AutomationElementIdentifiers.BoundingRectangleProperty.Id);
					return new Point (rectangle.X, rectangle.Y);
				}
			}
			
			foreach (IProviderBehavior behavior in ProviderBehaviors) {
				object val = behavior.GetPropertyValue (propertyId);
				if (val != null)
					return val;
			}
			
			return null;
		}

		public virtual IRawElementProviderSimple HostRawElementProvider {
			get {
				return AutomationInteropProvider.HostProviderFromHandle (control.TopLevelControl.Handle);
			}
		}
		
		public virtual ProviderOptions ProviderOptions {
			get {
				return ProviderOptions.ServerSideProvider;
			}
		}

#endregion
	}
}
