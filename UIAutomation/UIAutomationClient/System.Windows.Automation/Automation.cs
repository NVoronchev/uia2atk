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
//  Sandy Armstrong <sanfordarmstrong@gmail.com>
// 

using System;
using System.Collections.Generic;

using AEIds = System.Windows.Automation.AutomationElementIdentifiers;
using MUS = Mono.UIAutomation.Source;
using Mono.UIAutomation.Services;

namespace System.Windows.Automation
{
	public static class Automation
	{
		private static Dictionary<AutomationFocusChangedEventHandler, MUS.FocusChangedEventHandler>
			focusHandlerMapping = new Dictionary<AutomationFocusChangedEventHandler, MUS.FocusChangedEventHandler> ();

		static Automation ()
		{
			RawViewCondition = Condition.TrueCondition;
			ControlViewCondition =
				new NotCondition (new PropertyCondition (AEIds.IsControlElementProperty, false));
			ContentViewCondition =
				new NotCondition (new OrCondition (new PropertyCondition (AEIds.IsControlElementProperty, false),
				                                   new PropertyCondition (AEIds.IsContentElementProperty, false)));
		}

		public static bool Compare (int [] runtimeId1, int [] runtimeId2)
		{
			if (runtimeId1 == null)
				throw new ArgumentNullException ("runtimeId1");
			if (runtimeId2 == null)
				throw new ArgumentNullException ("runtimeId2");
			if (runtimeId1.Length != runtimeId2.Length)
				return false;
			for (int i = 0; i < runtimeId1.Length; i++)
				if (runtimeId1 [i] != runtimeId2 [i])
					return false;
			return true;
		}

		public static bool Compare (AutomationElement el1, AutomationElement el2)
		{
			if (el1 == null)
				throw new ArgumentNullException ("el1");
			if (el2 == null)
				throw new ArgumentNullException ("el2");
			return Compare (el1.GetRuntimeId (),
			                el2.GetRuntimeId ());
		}

		public static string PatternName (AutomationPattern pattern)
		{
			if (pattern == null)
				throw new ArgumentNullException ("pattern");

			if (pattern == DockPatternIdentifiers.Pattern)
				return "Dock";
			else if (pattern == ExpandCollapsePatternIdentifiers.Pattern)
				return "ExpandCollapse";
			else if (pattern == GridItemPatternIdentifiers.Pattern)
				return "GridItem";
			else if (pattern == GridPatternIdentifiers.Pattern)
				return "Grid";
			else if (pattern == InvokePatternIdentifiers.Pattern)
				return "Invoke";
			else if (pattern == LegacyIAccessiblePatternIdentifiers.Pattern)
				return "LegacyIAccessible";
			else if (pattern == MultipleViewPatternIdentifiers.Pattern)
				return "MultipleView";
			else if (pattern == RangeValuePatternIdentifiers.Pattern)
				return "RangeValue";
			else if (pattern == ScrollItemPatternIdentifiers.Pattern)
				return "ScrollItem";
			else if (pattern == ScrollPatternIdentifiers.Pattern)
				return "Scroll";
			else if (pattern == SelectionItemPatternIdentifiers.Pattern)
				return "SelectionItem";
			else if (pattern == SelectionPatternIdentifiers.Pattern)
				return "Selection";
			else if (pattern == TableItemPatternIdentifiers.Pattern)
				return "TableItem";
			else if (pattern == TablePatternIdentifiers.Pattern)
				return "Table";
			else if (pattern == TextPatternIdentifiers.Pattern)
				return "Text";
			else if (pattern == TogglePatternIdentifiers.Pattern)
				return "Toggle";
			else if (pattern == TransformPatternIdentifiers.Pattern)
				return "Transform";
			else if (pattern == ValuePatternIdentifiers.Pattern)
				return "Value";
			else if (pattern == WindowPatternIdentifiers.Pattern)
				return "Window";

			return null;
		}

		public static string PropertyName (AutomationProperty property)
		{
			if (property == null)
				throw new ArgumentNullException ("property");

			if (property == AEIds.AcceleratorKeyProperty)
				return "AcceleratorKey";
			else if (property == AEIds.AccessKeyProperty)
				return "AccessKey";
			else if (property == AEIds.AutomationIdProperty)
				return "AutomationId";
			else if (property == AEIds.BoundingRectangleProperty)
				return "BoundingRectangle";
			else if (property == AEIds.ClassNameProperty)
				return "ClassName";
			else if (property == AEIds.ClickablePointProperty)
				return "ClickablePoint";
			else if (property == AEIds.ControlTypeProperty)
				return "ControlType";
			else if (property == AEIds.CultureProperty)
				return "Culture";
			else if (property == AEIds.FrameworkIdProperty)
				return "FrameworkId";
			else if (property == AEIds.HasKeyboardFocusProperty)
				return "HasKeyboardFocus";
			else if (property == AEIds.HelpTextProperty)
				return "HelpText";
			else if (property == AEIds.IsContentElementProperty)
				return "IsContentElement";
			else if (property == AEIds.IsControlElementProperty)
				return "IsControlElement";
			else if (property == AEIds.IsDockPatternAvailableProperty)
				return "IsDockPatternAvailable";
			else if (property == AEIds.IsEnabledProperty)
				return "IsEnabled";
			else if (property == AEIds.IsExpandCollapsePatternAvailableProperty)
				return "IsExpandCollapsePatternAvailable";
			else if (property == AEIds.IsGridItemPatternAvailableProperty)
				return "IsGridItemPatternAvailable";
			else if (property == AEIds.IsGridPatternAvailableProperty)
				return "IsGridPatternAvailable";
			else if (property == AEIds.IsInvokePatternAvailableProperty)
				return "IsInvokePatternAvailable";
			else if (property == AEIds.IsKeyboardFocusableProperty)
				return "IsKeyboardFocusable";
			else if (property == AEIds.IsLegacyIAccessiblePatternAvailableProperty)
				return "IsLegacyIAccessiblePatternAvailable";
			else if (property == AEIds.IsMultipleViewPatternAvailableProperty)
				return "IsMultipleViewPatternAvailable";
			else if (property == AEIds.IsOffscreenProperty)
				return "IsOffscreen";
			else if (property == AEIds.IsPasswordProperty)
				return "IsPassword";
			else if (property == AEIds.IsRangeValuePatternAvailableProperty)
				return "IsRangeValuePatternAvailable";
			else if (property == AEIds.IsRequiredForFormProperty)
				return "IsRequiredForForm";
			else if (property == AEIds.IsScrollItemPatternAvailableProperty)
				return "IsScrollItemPatternAvailable";
			else if (property == AEIds.IsScrollPatternAvailableProperty)
				return "IsScrollPatternAvailable";
			else if (property == AEIds.IsSelectionItemPatternAvailableProperty)
				return "IsSelectionItemPatternAvailable";
			else if (property == AEIds.IsSelectionPatternAvailableProperty)
				return "IsSelectionPatternAvailable";
			else if (property == AEIds.IsTableItemPatternAvailableProperty)
				return "IsTableItemPatternAvailable";
			else if (property == AEIds.IsTablePatternAvailableProperty)
				return "IsTablePatternAvailable";
			else if (property == AEIds.IsTextPatternAvailableProperty)
				return "IsTextPatternAvailable";
			else if (property == AEIds.IsTogglePatternAvailableProperty)
				return "IsTogglePatternAvailable";
			else if (property == AEIds.IsTransformPatternAvailableProperty)
				return "IsTransformPatternAvailable";
			else if (property == AEIds.IsValuePatternAvailableProperty)
				return "IsValuePatternAvailable";
			else if (property == AEIds.IsWindowPatternAvailableProperty)
				return "IsWindowPatternAvailable";
			else if (property == AEIds.ItemStatusProperty)
				return "ItemStatus";
			else if (property == AEIds.ItemTypeProperty)
				return "ItemType";
			else if (property == AEIds.LabeledByProperty)
				return "LabeledBy";
			else if (property == AEIds.LocalizedControlTypeProperty)
				return "LocalizedControlType";
			else if (property == AEIds.NameProperty)
				return "Name";
			else if (property == AEIds.NativeWindowHandleProperty)
				return "NativeWindowHandle";
			else if (property == AEIds.OrientationProperty)
				return "Orientation";
			else if (property == AEIds.ProcessIdProperty)
				return "ProcessId";
			else if (property == AEIds.RuntimeIdProperty)
				return "RuntimeId";
			else if (property == DockPatternIdentifiers.DockPositionProperty)
				return "DockPosition";
			else if (property == ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty)
				return "ExpandCollapseState";
			else if (property == GridItemPatternIdentifiers.ColumnProperty)
				return "Column";
			else if (property == GridItemPatternIdentifiers.ColumnSpanProperty)
				return "ColumnSpan";
			else if (property == GridItemPatternIdentifiers.ContainingGridProperty)
				return "ContainingGrid";
			else if (property == GridItemPatternIdentifiers.RowProperty)
				return "Row";
			else if (property == GridItemPatternIdentifiers.RowSpanProperty)
				return "RowSpan";
			else if (property == GridPatternIdentifiers.ColumnCountProperty)
				return "ColumnCount";
			else if (property == GridPatternIdentifiers.RowCountProperty)
				return "RowCount";
			else if (property == LegacyIAccessiblePatternIdentifiers.ChildIdProperty)
				return "ChildId";
			else if (property == LegacyIAccessiblePatternIdentifiers.DefaultActionProperty)
				return "DefaultAction";
			else if (property == LegacyIAccessiblePatternIdentifiers.DescriptionProperty)
				return "Description";
			else if (property == LegacyIAccessiblePatternIdentifiers.HelpProperty)
				return "Help";
			else if (property == LegacyIAccessiblePatternIdentifiers.KeyboardShortcutProperty)
				return "KeyboardShortcut";
			else if (property == LegacyIAccessiblePatternIdentifiers.NameProperty)
				return "Name";
			else if (property == LegacyIAccessiblePatternIdentifiers.RoleProperty)
				return "Role";
			else if (property == LegacyIAccessiblePatternIdentifiers.StateProperty)
				return "State";
			else if (property == LegacyIAccessiblePatternIdentifiers.ValueProperty)
				return "Value";
			else if (property == MultipleViewPatternIdentifiers.CurrentViewProperty)
				return "CurrentView";
			else if (property == MultipleViewPatternIdentifiers.SupportedViewsProperty)
				return "SupportedViews";
			else if (property == RangeValuePatternIdentifiers.IsReadOnlyProperty)
				return "IsReadOnly";
			else if (property == RangeValuePatternIdentifiers.LargeChangeProperty)
				return "LargeChange";
			else if (property == RangeValuePatternIdentifiers.MaximumProperty)
				return "Maximum";
			else if (property == RangeValuePatternIdentifiers.MinimumProperty)
				return "Minimum";
			else if (property == RangeValuePatternIdentifiers.SmallChangeProperty)
				return "SmallChange";
			else if (property == RangeValuePatternIdentifiers.ValueProperty)
				return "Value";
			else if (property == ScrollPatternIdentifiers.HorizontallyScrollableProperty)
				return "HorizontallyScrollable";
			else if (property == ScrollPatternIdentifiers.HorizontalScrollPercentProperty)
				return "HorizontalScrollPercent";
			else if (property == ScrollPatternIdentifiers.HorizontalViewSizeProperty)
				return "HorizontalViewSize";
			else if (property == ScrollPatternIdentifiers.VerticallyScrollableProperty)
				return "VerticallyScrollable";
			else if (property == ScrollPatternIdentifiers.VerticalScrollPercentProperty)
				return "VerticalScrollPercent";
			else if (property == ScrollPatternIdentifiers.VerticalViewSizeProperty)
				return "VerticalViewSize";
			else if (property == SelectionItemPatternIdentifiers.IsSelectedProperty)
				return "IsSelected";
			else if (property == SelectionItemPatternIdentifiers.SelectionContainerProperty)
				return "SelectionContainer";
			else if (property == SelectionPatternIdentifiers.CanSelectMultipleProperty)
				return "CanSelectMultiple";
			else if (property == SelectionPatternIdentifiers.IsSelectionRequiredProperty)
				return "IsSelectionRequired";
			else if (property == SelectionPatternIdentifiers.SelectionProperty)
				return "Selection";
			else if (property == TableItemPatternIdentifiers.ColumnHeaderItemsProperty)
				return "ColumnHeaderItems";
			else if (property == TableItemPatternIdentifiers.RowHeaderItemsProperty)
				return "RowHeaderItems";
			else if (property == TablePatternIdentifiers.ColumnHeadersProperty)
				return "ColumnHeaders";
			else if (property == TablePatternIdentifiers.RowHeadersProperty)
				return "RowHeaders";
			else if (property == TablePatternIdentifiers.RowOrColumnMajorProperty)
				return "RowOrColumnMajor";
			else if (property == TogglePatternIdentifiers.ToggleStateProperty)
				return "ToggleState";
			else if (property == TransformPatternIdentifiers.CanMoveProperty)
				return "CanMove";
			else if (property == TransformPatternIdentifiers.CanResizeProperty)
				return "CanResize";
			else if (property == TransformPatternIdentifiers.CanRotateProperty)
				return "CanRotate";
			else if (property == ValuePatternIdentifiers.IsReadOnlyProperty)
				return "IsReadOnly";
			else if (property == ValuePatternIdentifiers.ValueProperty)
				return "Value";
			else if (property == WindowPatternIdentifiers.CanMaximizeProperty)
				return "CanMaximize";
			else if (property == WindowPatternIdentifiers.CanMinimizeProperty)
				return "CanMinimize";
			else if (property == WindowPatternIdentifiers.IsModalProperty)
				return "IsModal";
			else if (property == WindowPatternIdentifiers.IsTopmostProperty)
				return "IsTopmost";
			else if (property == WindowPatternIdentifiers.WindowInteractionStateProperty)
				return "WindowInteractionState";
			else if (property == WindowPatternIdentifiers.WindowVisualStateProperty)
				return "WindowVisualState";

			return null;
		}

		public static void AddAutomationEventHandler (AutomationEvent eventId,
		                                       AutomationElement element,
		                                       TreeScope scope,
		                                       AutomationEventHandler eventHandler)
		{
			CheckAutomationEventId (eventId);
			ArgumentCheck.NotNull (element, "element");
			ArgumentCheck.NotNull (eventHandler, "eventHandler");

			//TODO In theory we shall also check scope not equals to Parent or Ancestors,
			//but .Net didn't test/throw exceptions for "scope"

			if (element == AutomationElement.RootElement)
				foreach (var source in SourceManager.GetAutomationSources ())
					source.AddAutomationEventHandler (
						eventId, null, scope, eventHandler);
			else {
				var source = element.SourceElement.AutomationSource;
				source.AddAutomationEventHandler (
					eventId, element.SourceElement, scope, eventHandler);
			}
		}

		public static void AddAutomationFocusChangedEventHandler (AutomationFocusChangedEventHandler eventHandler)
		{
			ArgumentCheck.NotNull (eventHandler, "eventHandler");

			MUS.FocusChangedEventHandler sourceHandler;
			//according to the spec, all static methods in the UIA lib shall be thread safe.
			lock (focusHandlerMapping) {
				if (!focusHandlerMapping.TryGetValue (eventHandler, out sourceHandler)) {
					sourceHandler = (element, objectId, childId) => eventHandler (
						SourceManager.GetOrCreateAutomationElement (element),
						new AutomationFocusChangedEventArgs (objectId, childId));
					focusHandlerMapping.Add (eventHandler, sourceHandler);
				}
			}
			foreach (var source in SourceManager.GetAutomationSources ())
				source.AddAutomationFocusChangedEventHandler (sourceHandler);
		}

		public static void AddAutomationPropertyChangedEventHandler (AutomationElement element,
		                                                      TreeScope scope,
		                                                      AutomationPropertyChangedEventHandler eventHandler,
		                                                      params AutomationProperty [] properties)
		{
			ArgumentCheck.NotNull (element, "element");
			ArgumentCheck.NotNull (eventHandler, "eventHandler");

			if (element == AutomationElement.RootElement)
				foreach (var source in SourceManager.GetAutomationSources ())
					source.AddAutomationPropertyChangedEventHandler (
						null, scope, eventHandler, properties);
			else {
				var source = element.SourceElement.AutomationSource;
				source.AddAutomationPropertyChangedEventHandler (
					element.SourceElement, scope, eventHandler, properties);
			}
		}

		public static void AddStructureChangedEventHandler (AutomationElement element,
		                                             TreeScope scope,
		                                             StructureChangedEventHandler eventHandler)
		{
			ArgumentCheck.NotNull (element, "element");
			ArgumentCheck.NotNull (eventHandler, "eventHandler");

			if (element == AutomationElement.RootElement)
				foreach (var source in SourceManager.GetAutomationSources ())
					source.AddStructureChangedEventHandler (
						null, scope, eventHandler);
			else {
				var source = element.SourceElement.AutomationSource;
				source.AddStructureChangedEventHandler (
					element.SourceElement, scope, eventHandler);
			}
		}

		public static void RemoveAllEventHandlers ()
		{
			lock (focusHandlerMapping)
				focusHandlerMapping.Clear ();
			foreach (var source in SourceManager.GetAutomationSources ())
				source.RemoveAllEventHandlers ();
			AddAutomationFocusChangedEventHandler (AutomationElement.OnFocusChanged);
		}

		public static void RemoveAutomationEventHandler (
			AutomationEvent eventId,
			AutomationElement element,
			AutomationEventHandler eventHandler)
		{
			CheckAutomationEventId (eventId);
			ArgumentCheck.NotNull (element, "element");
			ArgumentCheck.NotNull (eventHandler, "eventHandler");

			if (element == AutomationElement.RootElement)
				foreach (var source in SourceManager.GetAutomationSources ())
					source.RemoveAutomationEventHandler (
						eventId, element.SourceElement, eventHandler);
			else {
				var source = element.SourceElement.AutomationSource;
				source.RemoveAutomationEventHandler (eventId, element.SourceElement, eventHandler);
			}
		}

		public static void RemoveAutomationFocusChangedEventHandler (AutomationFocusChangedEventHandler eventHandler)
		{
			ArgumentCheck.NotNull (eventHandler, "eventHandler");

			MUS.FocusChangedEventHandler sourceHandler;
			lock (focusHandlerMapping) {
				if (focusHandlerMapping.TryGetValue (eventHandler, out sourceHandler)) {
					focusHandlerMapping.Remove (eventHandler);
					foreach (var source in SourceManager.GetAutomationSources ())
						source.RemoveAutomationFocusChangedEventHandler (sourceHandler);
				}
			}
		}

		public static void RemoveAutomationPropertyChangedEventHandler (
			AutomationElement element, AutomationPropertyChangedEventHandler eventHandler)
		{
			ArgumentCheck.NotNull (element, "element");
			ArgumentCheck.NotNull (eventHandler, "eventHandler");

			if (element == AutomationElement.RootElement)
				foreach (var source in SourceManager.GetAutomationSources ())
					source.RemoveAutomationPropertyChangedEventHandler (
						null, eventHandler);
			else {
				var source = element.SourceElement.AutomationSource;
				source.RemoveAutomationPropertyChangedEventHandler (
					element.SourceElement, eventHandler);
			}
		}

		public static void RemoveStructureChangedEventHandler (
			AutomationElement element, StructureChangedEventHandler eventHandler)
		{
			ArgumentCheck.NotNull (element, "element");
			ArgumentCheck.NotNull (eventHandler, "eventHandler");

			if (element == AutomationElement.RootElement)
				foreach (var source in SourceManager.GetAutomationSources ())
					source.RemoveStructureChangedEventHandler (
						null, eventHandler);
			else {
				var source = element.SourceElement.AutomationSource;
				source.RemoveStructureChangedEventHandler (
					element.SourceElement, eventHandler);
			}
		}

		private static void CheckAutomationEventId (AutomationEvent eventId)
		{
			ArgumentCheck.NotNull (eventId, "eventId");
			if (AutomationElementIdentifiers.AutomationFocusChangedEvent == eventId
				|| AutomationElementIdentifiers.AutomationFocusChangedEvent == eventId
				|| AutomationElementIdentifiers.AutomationPropertyChangedEvent == eventId
				|| AutomationElementIdentifiers.StructureChangedEvent == eventId)
				throw new ArgumentException ("eventId");
		}

		public static readonly Condition ContentViewCondition;

		public static readonly Condition ControlViewCondition;

		public static readonly Condition RawViewCondition;
	}
}
