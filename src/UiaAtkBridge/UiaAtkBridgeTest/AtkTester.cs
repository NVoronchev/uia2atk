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
//      Andres G. Aragoneses <aaragoneses@novell.com>
// 

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;

using NUnit.Framework;

namespace UiaAtkBridgeTest
{
	
	public abstract class AtkTester {
		
		public abstract object GetAtkObjectThatImplementsInterface <I> (
			BasicWidgetType type, string name, out Atk.Object accessible, bool real);
		
		public abstract object GetAtkObjectThatImplementsInterface <I> (
			BasicWidgetType type, string[] name, out Atk.Object accessible, bool real);
	
		private static AtkTester instance;
		public static AtkTester Instance {
			get { return instance; }
		}
		
		public AtkTester () {
			instance = this;
		}
		
		[Test]
		public void Label ()
		{
			BasicWidgetType type = BasicWidgetType.Label;
			Atk.Object accessible = InterfaceText (type);

			PropertyRole (type, accessible);
			
			//a label always contains this state, not because it's multi_line, but because it can be multi_line
			Assert.IsTrue (accessible.RefStateSet ().ContainsState (Atk.StateType.MultiLine), "RefStateSet().Contains(MultiLine)");
			
			Assert.AreEqual (0, accessible.NAccessibleChildren, "Label numChildren");
		}
		
		[Test]
		public void Button ()
		{
			BasicWidgetType type = BasicWidgetType.NormalButton;
			Atk.Object accessible;
			
			InterfaceText (type);
			
			string name = "test";
			Atk.Component atkComponent = (Atk.Component)
				GetAtkObjectThatImplementsInterface <Atk.Component> (type, name, out accessible, true);
			InterfaceComponent (type, atkComponent);
			
			name = "test";
			Atk.Action atkAction = (Atk.Action)
				GetAtkObjectThatImplementsInterface <Atk.Action> (type, name, out accessible, true);
			InterfaceAction (type, atkAction, accessible);
			
			PropertyRole (type, accessible);
			
			Assert.AreEqual (0, accessible.NAccessibleChildren, "Button numChildren");
		}

		[Test]
		public void CheckBox ()
		{
			BasicWidgetType type = BasicWidgetType.CheckBox;
			Atk.Object accessible;
			
			string name = "test";
			Atk.Component atkComponent = (Atk.Component)
				GetAtkObjectThatImplementsInterface <Atk.Component> (type, name, out accessible, true);
			InterfaceComponent (type, atkComponent);
			
			name = "test";
			Atk.Action atkAction = (Atk.Action)
				GetAtkObjectThatImplementsInterface <Atk.Action> (type, name, out accessible, true);
			
			InterfaceAction (type, atkAction, accessible);
			
			PropertyRole (type, accessible);
			
			Assert.AreEqual (0, accessible.NAccessibleChildren, "CheckBox numChildren");
		}
		
		[Test]
		public void RadioButtons ()
		{
			BasicWidgetType type = BasicWidgetType.RadioButton;
			Atk.Object accessible, accessible2, accessible3;
			
			string name = "test 01";
			Atk.Action atkAction = (Atk.Action)
				GetAtkObjectThatImplementsInterface <Atk.Action> (type, name, out accessible, true);
			InterfaceAction (type, atkAction, accessible);
			
			name = "test 02";
			Atk.Action atkAction2 = (Atk.Action)
				GetAtkObjectThatImplementsInterface <Atk.Action> (type, name, out accessible2, true);
			//the third radio button is disconnected from the previous ones
			name = "test 03";
			Atk.Action atkAction3 = (Atk.Action)
				GetAtkObjectThatImplementsInterface <Atk.Action> (type, name, out accessible3, true);
			
			InterfaceActionFor3RadioButtons (atkAction, accessible,
			                                 atkAction2, accessible2,
			                                 atkAction3, accessible3);
			
			name = "test 04";
			Atk.Component atkComponent = (Atk.Component)
				GetAtkObjectThatImplementsInterface <Atk.Component> (type, name, out accessible, true);
			InterfaceComponent (type, atkComponent);
			
			PropertyRole (type, accessible);
			
			Assert.AreEqual (0, accessible.NAccessibleChildren, "RadioButton numChildren");
		}
		
		//[Test]
		public void TextBoxEntry ()
		{
			BasicWidgetType type = BasicWidgetType.TextBoxEntry;
			Atk.Object accessible = InterfaceText (type);
			
			PropertyRole (type, accessible);
		}
		
		//it's safer to put this test the last, apparently Atk makes it unresponsive after dealing with
		//the widget, so we kill all with the method marked as [TestFixtureTearDown]
		[Test]
		public void ComboBox ()
		{
			BasicWidgetType type = BasicWidgetType.ComboBox;
			Atk.Object accessible;
			
			string[] names = new string[] { "First item", "Second Item", "Last Item" };
			Atk.Component atkComponent = (Atk.Component)
				GetAtkObjectThatImplementsInterface <Atk.Component> (type, names, out accessible, true);

			InterfaceComponent (type, atkComponent);
			
			PropertyRole (type, accessible);
			
			Atk.Action atkAction = (Atk.Action)
				GetAtkObjectThatImplementsInterface <Atk.Action> (type, names, out accessible, true);
			InterfaceAction (type, atkAction, accessible);
			
			Atk.Selection atkSelection = (Atk.Selection)
				GetAtkObjectThatImplementsInterface <Atk.Selection> (type, names, out accessible, true);
			InterfaceSelection (atkSelection, names, accessible);
			
			Assert.AreEqual (1, accessible.NAccessibleChildren, "ComboBox#RO numChildren");
			//FIXME: enable this when we test the editable combobox:
			//Assert.AreEqual (2, accessible.NAccessibleChildren, "ComboBox#editable numChildren");
			
			Atk.Object menuChild = accessible.RefAccessibleChild (0);
			Assert.IsNotNull (menuChild, "ComboBox child#0 should not be null");
			Assert.AreEqual (menuChild.Name, null, "the ComboBox menu should not have a name");
			Assert.AreEqual (menuChild.Role, Atk.Role.Menu, "ComboBox child#0 should be a menu");
			
			Assert.AreEqual (names.Length, menuChild.NAccessibleChildren, "ComboBox menu numChildren");
			Atk.Object menuItemChild = menuChild.RefAccessibleChild (0);
			Assert.IsNotNull (menuItemChild, "ComboBox child#0 child#0 should not be null");
			Assert.AreEqual (menuItemChild.Role, Atk.Role.MenuItem, "ComboBox child#0 child#0 should be a menuItem");
			Assert.AreEqual (menuItemChild.Name, names[0], "ComboBox menuitem names should be the same as the items");
			
			Assert.AreEqual (0, menuItemChild.NAccessibleChildren, "ComboBox menuItem numChildren");
		}
		
		[Test]
		public void Window ()
		{
			BasicWidgetType type = BasicWidgetType.Window;
			Atk.Object accessible;
			
			string name = "test";
			Atk.Component atkComponent = (Atk.Component)
				GetAtkObjectThatImplementsInterface <Atk.Component> (type, name, out accessible, true);
			InterfaceComponent (type, atkComponent);
			
			PropertyRole (type, accessible);
		}
		
		private void InterfaceComponent (BasicWidgetType type, Atk.Component implementor)
		{
			Assert.AreEqual (1, implementor.Alpha, "Component.Alpha");

			if (type == BasicWidgetType.Window) {
				Assert.AreEqual (Atk.Layer.Window, implementor.Layer, "Component.Layer(Window)");
				Assert.AreEqual (-1, implementor.MdiZorder, "Component.MdiZorder(Window)");
			}
			else {
				Assert.AreEqual (Atk.Layer.Widget, implementor.Layer, "Component.Layer(notWindow)");
				//FIXME: still don't know why this is failing in the GailTester, accerciser is lying me?
				//Assert.AreEqual (0, implementor.MdiZorder, "Component.MdiZorder(notWindow)");
			}
		}
		
		protected abstract int ValidNumberOfActionsForAButton { get; }
		
		private void InterfaceActionFor3RadioButtons (Atk.Action actionable1, Atk.Object accessible1,
		                                               Atk.Action actionable2, Atk.Object accessible2,
		                                               Atk.Action actionable3, Atk.Object accessible3)
		{
			Assert.IsTrue (actionable1.DoAction (0), "IAF3RB::DoAction#1");
			System.Threading.Thread.Sleep (3000);
			Assert.IsTrue (actionable3.DoAction (0), "IAF3RB::DoAction#1");
			System.Threading.Thread.Sleep (3000);
			
			Assert.IsTrue (accessible1.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #1");
			Assert.IsFalse (accessible2.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #2");
			Assert.IsTrue (accessible3.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #3");
			
			Assert.IsTrue (actionable2.DoAction (0), "IAF3RB::DoAction#1");
			System.Threading.Thread.Sleep (3000);
			
			Assert.IsFalse (accessible1.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #4");
			Assert.IsTrue (accessible2.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #5");
			Assert.IsTrue (accessible3.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #6");

			Assert.IsTrue (actionable1.DoAction (0), "IAF3RB::DoAction#2");
			System.Threading.Thread.Sleep (3000);
			
			Assert.IsTrue (accessible1.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #7");
			Assert.IsFalse (accessible2.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #8");
			Assert.IsTrue (accessible3.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #9");

			Assert.IsTrue (actionable1.DoAction (0), "IAF3RB::DoAction#3");
			System.Threading.Thread.Sleep (3000);
			
			Assert.IsTrue (accessible1.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #10");
			Assert.IsFalse (accessible2.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #11");
			Assert.IsTrue (accessible3.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #12");
			
			Assert.IsTrue (actionable3.DoAction (0), "IAF3RB::DoAction#4");
			System.Threading.Thread.Sleep (3000);
			
			Assert.IsTrue (accessible1.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #13");
			Assert.IsFalse (accessible2.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #14");
			Assert.IsTrue (accessible3.RefStateSet ().ContainsState (Atk.StateType.Checked), "IAF3RB::Checked #15");
		}
		
		private void InterfaceAction (BasicWidgetType type, Atk.Action implementor, Atk.Object accessible)
		{
			if (type == BasicWidgetType.ComboBox) {
				Assert.AreEqual (1, implementor.NActions, "NActions");
				Assert.AreEqual ("press", implementor.GetName (0), "GetName press");
			} else { //Button and Checkbox and RadioButton
				Assert.AreEqual (ValidNumberOfActionsForAButton, implementor.NActions, "NActions");
				
				Assert.AreEqual ("click", implementor.GetName (0), "GetName click");
				if (ValidNumberOfActionsForAButton > 1) {
					Assert.AreEqual ("press", implementor.GetName (1), "GetName press");
					Assert.AreEqual ("release", implementor.GetName (2), "GetName release");
				}
			}
			
			bool actionPerformed = true;
			//this only applies if the CheckBox is not real (in Gail) :-?
//			if (type == BasicWidgetType.CheckBox)
//				actionPerformed = false;
			
			Atk.StateSet state = accessible.RefStateSet();
			Assert.IsFalse (state.IsEmpty, "RefStateSet.IsEmpty");
			Assert.IsTrue (state.ContainsState (Atk.StateType.Enabled), "RefStateSet.Enabled #1");
			//a radio button is checked by default
			if (type != BasicWidgetType.RadioButton)
				Assert.IsFalse (state.ContainsState (Atk.StateType.Checked), "RefStateSet.!Checked #1");
			
			if (type != BasicWidgetType.ComboBox) {
				// only valid actions should work
				for (int i = 0; i < ValidNumberOfActionsForAButton; i++) 
					Assert.AreEqual (actionPerformed, implementor.DoAction (i), "DoAction");
				if ((ValidNumberOfActionsForAButton > 1) // does not apply in UIA because 1 doaction==1click==checked
				                                         // (in GAIL click+press+release==2clicks==unchecked)
				     && (type == BasicWidgetType.CheckBox))
					//one more, to leave it checked
					Assert.AreEqual (actionPerformed, implementor.DoAction (0), "DoAction_Corrective");
			}
			else
			{
				Assert.AreEqual (actionPerformed, implementor.DoAction (0), "DoAction Combo#1");
				Assert.AreEqual (1, implementor.NActions, "NActions doesn't change");
				Assert.AreEqual ("press", implementor.GetName (0), "Action[0] doesn't change");
				Assert.AreEqual (false, implementor.DoAction (0), "DoAction Combo#2");
			}
			// it takes a bit before the State is propagated!
			System.Threading.Thread.Sleep (4000);
			
			state = accessible.RefStateSet();
			Assert.IsTrue (state.ContainsState (Atk.StateType.Enabled), "RefStateSet.Enabled #2");
			if ((type == BasicWidgetType.CheckBox) || (type == BasicWidgetType.RadioButton))
				Assert.IsTrue (state.ContainsState (Atk.StateType.Checked), "RefStateSet.Checked");
			else
				Assert.IsFalse (state.ContainsState (Atk.StateType.Checked), "RefStateSet.!Checked #2");
			
			
			//still need to figure out why this is null in gail
//				Assert.IsNull (implementor.GetLocalizedName (0));
//				Assert.IsNull (implementor.GetLocalizedName (1));
//				Assert.IsNull (implementor.GetLocalizedName (2));
			
			for (int i = 0; i < ValidNumberOfActionsForAButton; i++) 
				Assert.IsNull (implementor.GetDescription (i), "GetDescription null");
			
			//out of range
			Assert.IsFalse (implementor.DoAction (-1), "DoAction OOR#1");
			Assert.IsFalse (implementor.DoAction (ValidNumberOfActionsForAButton), "DoAction OOR#2");
			Assert.IsNull (implementor.GetName (-1), "GetName OOR#1");
			Assert.IsNull (implementor.GetName (ValidNumberOfActionsForAButton), "GetName OOR#2");
			Assert.IsNull (implementor.GetDescription (-1), "GetDescription OOR#1");
			Assert.IsNull (implementor.GetDescription (ValidNumberOfActionsForAButton), "GetDescription OOR#2");
			Assert.IsNull (implementor.GetLocalizedName (-1), "GetLocalizedName OOR#1");
			Assert.IsNull (implementor.GetLocalizedName (ValidNumberOfActionsForAButton), "GetLocalizedName OOR#2");
			
			string descrip = "Some big ugly description";
			int nActions = ValidNumberOfActionsForAButton;
			if (type == BasicWidgetType.ComboBox)
				nActions = 1;
			for (int i = 0; i < nActions; i++) {
				Assert.IsTrue (implementor.SetDescription(i, descrip), "SetDescription");
				Assert.AreEqual (descrip, implementor.GetDescription (i), "GetDescription");
				descrip += ".";
			}
			Assert.IsFalse (implementor.SetDescription(ValidNumberOfActionsForAButton, descrip), "SetDescription OOR");
			Assert.IsNull (implementor.GetDescription (ValidNumberOfActionsForAButton), "GetDescription OOR#3");
			
			// With no keybinding set, everything should return null
			Assert.IsNull (implementor.GetKeybinding (0), "GetKeyBinding#1");
			Assert.IsNull (implementor.GetKeybinding (1), "GetKeyBinding#2");
			Assert.IsNull (implementor.GetKeybinding (2), "GetKeyBinding#3");
			
			//out of range items too
			Assert.IsNull (implementor.GetKeybinding (-1), "GetKeyBinding OOR#1");
			Assert.IsNull (implementor.GetKeybinding (3), "GetKeyBinding OOR#2");
		}
		
		private void InterfaceSelection (Atk.Selection implementor, string[] names, Atk.Object accessible)
		{
			Assert.AreEqual (null, accessible.Name, "AtkObj Name");
			for (int i = 0; i < names.Length; i++) {
				Assert.IsFalse (implementor.IsChildSelected (i), "isChildSelected(" + i + ")");
			}
			
			Assert.AreEqual (0, implementor.SelectionCount, "SelectionCount == 0");
			
			for (int i = 0; i < names.Length; i++) {
				Assert.IsTrue (implementor.AddSelection (i), "AddSelection(" + i + ")");
				Assert.AreEqual (names[i], accessible.Name, "AtkObj Name #" + i);
				Assert.AreEqual (accessible.Name, implementor.RefSelection (0).Name, "AtkObj NameRefSel#" + i);
				Assert.AreEqual (1, implementor.SelectionCount, "SelectionCount == 1");
				Assert.IsTrue (implementor.IsChildSelected (i), "childSelected(" + i + ")");
				
				int refSel = i;
				if (refSel == 0)
					refSel = -1;
				Assert.IsNull (implementor.RefSelection (refSel), "RefSelection OOR#-" + i);
			}
			
			Assert.IsNotNull (implementor.RefSelection (0), "RefSel!=null");
			
			string lastName = accessible.Name;
			//strangely, OOR selections return true (valid)
			Assert.IsTrue (implementor.AddSelection (-1), "AddSelection OOR#1");
			Assert.IsTrue (implementor.AddSelection (names.Length), "AddSelection OOR#2");
			//OOR selections don't affect name:
			Assert.AreEqual (lastName, accessible.Name);
			
			Assert.IsNull (implementor.RefSelection (-1), "RefSelection OOR#1");
			Assert.IsNull (implementor.RefSelection (names.Length), "RefSelection OOR#2");
			
			Assert.IsTrue (implementor.ClearSelection (), "ClearSelection");
			Assert.IsNull (implementor.RefSelection (0), "RefSel after CS");
			
			//this is a normal combobox (not multiline) (TODO: research multiline comboboxes?)
			Assert.IsFalse (implementor.SelectAllSelection ());
			
			Assert.IsNull (implementor.RefSelection (0), "RefSel after SAS");
			
			Assert.IsTrue (names.Length > 0, "Please use a names variable that is not empty");
			Assert.IsTrue (implementor.AddSelection (0), "AddSelection->0");
			Assert.IsNotNull (implementor.RefSelection (0), "RefSel!=null after AS0");
			//again, it's surprising to return TRUE on OOR
			Assert.IsTrue (implementor.RemoveSelection (-1), "RemoveSelection OOR#<0");
			Assert.IsTrue (implementor.RemoveSelection (names.Length), "RemoveSelection OOR#>n");
			Assert.IsTrue (implementor.RemoveSelection (0), "RemoveSelection");
			Assert.IsNull (implementor.RefSelection (0), "RefSel after RemoveSel");
		}
		
		private void PropertyRole (BasicWidgetType type, Atk.Object accessible)
		{
			Atk.Role role = Atk.Role.Unknown;
			switch (type) {
			case BasicWidgetType.Label:
				role = Atk.Role.Label;
				break;
			case BasicWidgetType.NormalButton:
				role = Atk.Role.PushButton;
				break;
			case BasicWidgetType.Window:
				role = Atk.Role.Frame;
				break;
			case BasicWidgetType.CheckBox:
				role = Atk.Role.CheckBox;
				break;
			case BasicWidgetType.ComboBox:
				role = Atk.Role.ComboBox;
				break;
			case BasicWidgetType.RadioButton:
				role = Atk.Role.RadioButton;
				break;
			case BasicWidgetType.TextBoxEntry:
				role = Atk.Role.Text;
				break;
			default:
				throw new NotImplementedException ();
			}
			Assert.AreEqual (role, accessible.Role, "Atk.Role");
		}
		
		private Atk.Object InterfaceText (BasicWidgetType type)
		{
			int startOffset, endOffset;
			string expected;
			Atk.Text atkText;
			string name = "This is a test sentence.\r\nSecond line. Other phrase.\nThird line?";

			Atk.Object accessible;
			atkText = (Atk.Text)
				GetAtkObjectThatImplementsInterface <Atk.Text> (type, name, out accessible, false);
			
			int nSelections = -1;
			if (type == BasicWidgetType.Label)
				nSelections = 0;
			
			Assert.AreEqual (0, atkText.CaretOffset, "CaretOffset");
			Assert.AreEqual (name.Length, atkText.CharacterCount, "CharacterCount");
			Assert.AreEqual (name [0], atkText.GetCharacterAtOffset (0), "GetCharacterAtOffset");
			Assert.AreEqual (name, atkText.GetText (0, name.Length), "GetText");
			
			//any value
			Assert.AreEqual (false, atkText.SetCaretOffset (-1), "SetCaretOffset#1");
			Assert.AreEqual (false, atkText.SetCaretOffset (0), "SetCaretOffset#2");
			Assert.AreEqual (false, atkText.SetCaretOffset (1), "SetCaretOffset#3");
			Assert.AreEqual (false, atkText.SetCaretOffset (15), "SetCaretOffset#4");
			
			// don't do this until bug#393565 is fixed:
			//Assert.AreEqual (typeof(Atk.TextAttribute), atkText.DefaultAttributes[0].GetType());

			Assert.AreEqual (nSelections, atkText.NSelections, "NSelections#1");
			
			// you cannot select a label AFAIK so, all zeroes returned!
			Assert.AreEqual (null, atkText.GetSelection (0, out startOffset, out endOffset), "GetSelection#1");
			Assert.AreEqual (0, startOffset, "GetSelection#2");
			Assert.AreEqual (0, endOffset, "GetSelection#3");
			Assert.AreEqual (null, atkText.GetSelection (1, out startOffset, out endOffset), "GetSelection#4");
			Assert.AreEqual (0, startOffset, "GetSelection#5");
			Assert.AreEqual (0, endOffset, "GetSelection#6");
			Assert.AreEqual (null, atkText.GetSelection (-1, out startOffset, out endOffset), "GetSelection#7");
			Assert.AreEqual (0, startOffset, "GetSelection#8");
			Assert.AreEqual (0, endOffset, "GetSelection#9");
			
			// you cannot select a label AFAIK so, false always returned!
			Assert.AreEqual (false, atkText.SetSelection (0, 1, 2), "SetSelection#1");
			// test GetSelection *after* SetSelection
			Assert.AreEqual (null, atkText.GetSelection (0, out startOffset, out endOffset), "GetSelection#10");
			Assert.AreEqual (0, startOffset, "GetSelection#11");
			Assert.AreEqual (0, endOffset, "GetSelection#12");
			//test crazy numbers for SetSelection
			Assert.AreEqual (false, atkText.SetSelection (-3, 10, -2), "SetSelection#2");
			Assert.AreEqual (null, atkText.GetSelection (0, out startOffset, out endOffset), "GetSelection#13");
			Assert.AreEqual (0, startOffset, "GetSelection#14");
			Assert.AreEqual (0, endOffset, "GetSelection#15");
			
			//did NSelections changed?
			Assert.AreEqual (false, atkText.SetSelection (1, 2, 3), "SetSelection#3");
			Assert.AreEqual (nSelections, atkText.NSelections, "NSelections#2");
			Assert.AreEqual (false, atkText.RemoveSelection (0), "RemoveSelection#1");
			Assert.AreEqual (nSelections, atkText.NSelections, "NSelections#3");
			Assert.AreEqual (false, atkText.RemoveSelection (1), "RemoveSelection#2");
			Assert.AreEqual (nSelections, atkText.NSelections, "NSelections#4");
			Assert.AreEqual (false, atkText.RemoveSelection (-1), "RemoveSelection#3");
			Assert.AreEqual (nSelections, atkText.NSelections, "NSelections#5");


			//IMPORTANT NOTE about GetText*Offset methods [GetTextAtOffset(),GetTextAfterOffset(),GetTextBeforeOffset()]:
			//in Gail, they all return null if GetText() has not been called yet, however we may
			//prefer not to follow this wierd behaviour in the bridge
			
			//GetTextAtOffset
			expected = " test";
			Assert.AreEqual (expected, 
				atkText.GetTextAtOffset (12, Atk.TextBoundary.WordEnd, out startOffset, out endOffset),
				"GetTextAtOffset,WordEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAtOffset,WordEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAtOffset,WordEnd,eo");
			
			//test selections after obtaining text with a different API than GetText
			Assert.AreEqual (nSelections, atkText.NSelections, "NSelections#6");
			//NSelections == 0, however we have one selection, WTF?:
			Assert.AreEqual (null, atkText.GetSelection (0, out startOffset, out endOffset), "GetSelection#16");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetSelection#17");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetSelection#18");
			Assert.AreEqual (null, atkText.GetSelection (1, out startOffset, out endOffset), "GetSelection#19");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetSelection#20");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetSelection#21");
			Assert.AreEqual (null, atkText.GetSelection (30, out startOffset, out endOffset), "GetSelection#22");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetSelection#23");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetSelection#24");
			Assert.AreEqual (null, atkText.GetSelection (-1, out startOffset, out endOffset), "GetSelection#25");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetSelection#26");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetSelection#27");
			
			Assert.AreEqual (false, atkText.SetSelection (0, 0, 0), "SetSelection#3");
			Assert.AreEqual (null, atkText.GetSelection (0, out startOffset, out endOffset), "GetSelection#28");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetSelection#29");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetSelection#30");
			
			
			expected = "test ";
			Assert.AreEqual (expected, 
				atkText.GetTextAtOffset (12, Atk.TextBoundary.WordStart, out startOffset, out endOffset),
				"GetTextAtOffset,WordStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAtOffset,WordStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAtOffset,WordStart,eo");
			
			expected = "This is a test sentence.";
			Assert.AreEqual (expected,
				atkText.GetTextAtOffset (12, Atk.TextBoundary.LineEnd, out startOffset, out endOffset),
				"GetTextAtOffset,LineEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAtOffset,LineEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAtOffset,LineEnd,eo");

			expected = "This is a test sentence.\r\n";
			Assert.AreEqual (expected,
				atkText.GetTextAtOffset (12, Atk.TextBoundary.LineStart, out startOffset, out endOffset),
				"GetTextAtOffset,LineStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAtOffset,LineStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAtOffset,LineStart,eo");
			
			expected = "This is a test sentence.";
			Assert.AreEqual (expected,
				atkText.GetTextAtOffset (18, Atk.TextBoundary.SentenceEnd, out startOffset, out endOffset),
				"GetTextAtOffset,SentenceEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAtOffset,SentenceEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAtOffset,SentenceEnd,eo");
			
			expected = "This is a test sentence.\r\n";
			Assert.AreEqual (expected,
				atkText.GetTextAtOffset (18, Atk.TextBoundary.SentenceStart, out startOffset, out endOffset),
				"GetTextAtOffset,SentenceStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAtOffset,SentenceStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAtOffset,SentenceStart,eo");
			
			Assert.AreEqual ("t",
				atkText.GetTextAtOffset (18, Atk.TextBoundary.Char, out startOffset, out endOffset), "GetTextAtOffset,Char1");
			Assert.AreEqual (18, startOffset, "GetTextAtOffset,Char1,so");
			Assert.AreEqual (19, endOffset, "GetTextAtOffset,Char1,eo");
			Assert.AreEqual (".",
				atkText.GetTextAtOffset (23, Atk.TextBoundary.Char, out startOffset, out endOffset), "GetTextAtOffset,Char2");
			Assert.AreEqual (23, startOffset, "GetTextAtOffset,Char2,so");
			Assert.AreEqual (24, endOffset, "GetTextAtOffset,Char2,eo");
			Assert.AreEqual ("e",
				atkText.GetTextAtOffset (name.Length - 2, Atk.TextBoundary.Char, out startOffset, out endOffset), "GetTextAtOffset,Char3");
			Assert.AreEqual (name.Length - 2, startOffset, "GetTextAtOffset,Char3,so");
			Assert.AreEqual (name.Length - 1, endOffset, "GetTextAtOffset,Char3,eo");
			Assert.AreEqual ("?",
				atkText.GetTextAtOffset (name.Length - 1, Atk.TextBoundary.Char, out startOffset, out endOffset), "GetTextAtOffset,Char4");
			Assert.AreEqual (name.Length - 1, startOffset, "GetTextAtOffset,Char4,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextAtOffset,Char4,eo");
			Assert.AreEqual (String.Empty,
				atkText.GetTextAtOffset (name.Length, Atk.TextBoundary.Char, out startOffset, out endOffset), "GetTextAtOffset,Char5");
			Assert.AreEqual (name.Length, startOffset, "GetTextAtOffset,Char5,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextAtOffset,Char5,eo");
			Assert.AreEqual (String.Empty,
				atkText.GetTextAtOffset (-1, Atk.TextBoundary.Char, out startOffset, out endOffset), "GetTextAtOffset,Char6");
			Assert.AreEqual (name.Length, startOffset, "GetTextAtOffset,Char6,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextAtOffset,Char6,eo");


			//GetTextAfterOffset
			expected = " sentence";
			Assert.AreEqual (expected, 
				atkText.GetTextAfterOffset (12, Atk.TextBoundary.WordEnd, out startOffset, out endOffset),
				"GetTextAfterOffset,WordEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAfterOffset,WordEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAfterOffset,WordEnd,eo");
			
			expected = "sentence.\r\n";
			Assert.AreEqual (expected, 
				atkText.GetTextAfterOffset (12, Atk.TextBoundary.WordStart, out startOffset, out endOffset),
				"GetTextAfterOffset,WordStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAfterOffset,WordStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAfterOffset,WordStart,eo");
			
			expected = "\r\nSecond line. Other phrase.";
			Assert.AreEqual (expected,
				atkText.GetTextAfterOffset (12, Atk.TextBoundary.LineEnd, out startOffset, out endOffset),
				"GetTextAfterOffset,LineEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAfterOffset,LineEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAfterOffset,LineEnd,eo");

			expected = "Second line. Other phrase.\n";
			Assert.AreEqual (expected,
				atkText.GetTextAfterOffset (12, Atk.TextBoundary.LineStart, out startOffset, out endOffset),
				"GetTextAfterOffset,LineStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAfterOffset,LineStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAfterOffset,LineStart,eo");

			expected = "\r\nSecond line.";
			Assert.AreEqual (expected,
				atkText.GetTextAfterOffset (18, Atk.TextBoundary.SentenceEnd, out startOffset, out endOffset),
				"GetTextAfterOffset,SentenceEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAfterOffset,SentenceEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAfterOffset,SentenceEnd,eo");

			expected = " Other phrase.";
			Assert.AreEqual (expected,
				atkText.GetTextAfterOffset (24, Atk.TextBoundary.SentenceEnd, out startOffset, out endOffset),
				"GetTextAfterOffset,SentenceEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAfterOffset,SentenceEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAfterOffset,SentenceEnd,eo");
			
			expected = "Second line. ";
			Assert.AreEqual (expected,
				atkText.GetTextAfterOffset (18, Atk.TextBoundary.SentenceStart, out startOffset, out endOffset),
				"GetTextAfterOffset,SentenceStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAfterOffset,SentenceStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAfterOffset,SentenceStart,eo");
			
			Assert.AreEqual ("e",
				atkText.GetTextAfterOffset (18, Atk.TextBoundary.Char, out startOffset, out endOffset),
				"GetTextAfterOffset,Char");
			Assert.AreEqual (19, startOffset, "GetTextAfterOffset,Char,so");
			Assert.AreEqual (20, endOffset, "GetTextAfterOffset,Char,eo");
			Assert.AreEqual ("\r",
				atkText.GetTextAfterOffset (23, Atk.TextBoundary.Char, out startOffset, out endOffset),
				"GetTextAfterOffset,Char");
			Assert.AreEqual (24, startOffset, "GetTextAfterOffset,Char,so");
			Assert.AreEqual (25, endOffset, "GetTextAfterOffset,Char,eo");
			Assert.AreEqual ("?",
				atkText.GetTextAfterOffset (name.Length - 2, Atk.TextBoundary.Char, out startOffset, out endOffset),
				"GetTextAfterOffset,Char");
			Assert.AreEqual (name.Length - 1, startOffset, "GetTextAfterOffset,Char,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextAfterOffset,Char,eo");
			Assert.AreEqual (String.Empty,
				atkText.GetTextAfterOffset (name.Length - 1, Atk.TextBoundary.Char, out startOffset, out endOffset),
				"GetTextAfterOffset,Char");
			Assert.AreEqual (name.Length, startOffset, "GetTextAfterOffset,Char,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextAfterOffset,Char,eo");
			Assert.AreEqual (String.Empty,
				atkText.GetTextAfterOffset (name.Length, Atk.TextBoundary.Char, out startOffset, out endOffset),
				"GetTextAfterOffset,Char");
			Assert.AreEqual (name.Length, startOffset, "GetTextAfterOffset,Char,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextAfterOffset,Char,eo");
			Assert.AreEqual (null,
				atkText.GetTextAfterOffset (-1, Atk.TextBoundary.Char, out startOffset, out endOffset),
				"GetTextAfterOffset,Char");
			Assert.AreEqual (name.Length, startOffset, "GetTextAfterOffset,Char,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextAfterOffset,Char,eo");
			
			
			//GetTextBeforeOffset
			expected = " a";
			Assert.AreEqual (expected, 
				atkText.GetTextBeforeOffset (12, Atk.TextBoundary.WordEnd, out startOffset, out endOffset),
				"GetTextBeforeOffset,WordEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextBeforeOffset,WordEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextBeforeOffset,WordEnd,eo");
			
			expected = "a ";
			Assert.AreEqual (expected, 
				atkText.GetTextBeforeOffset (12, Atk.TextBoundary.WordStart, out startOffset, out endOffset),
				"GetTextBeforeOffset,WordStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextBeforeOffset,WordStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextBeforeOffset,WordStart,eo");
			
			expected = String.Empty;
			Assert.AreEqual (expected,
				atkText.GetTextBeforeOffset (12, Atk.TextBoundary.LineEnd, out startOffset, out endOffset),
				"GetTextBeforeOffset,LineEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextBeforeOffset,LineEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextBeforeOffset,LineEnd,eo");

			Assert.AreEqual (expected,
				atkText.GetTextBeforeOffset (12, Atk.TextBoundary.LineStart, out startOffset, out endOffset),
				"GetTextBeforeOffset,LineStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextBeforeOffset,LineStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextBeforeOffset,LineStart,eo");
			
			Assert.AreEqual (expected,
				atkText.GetTextBeforeOffset (18, Atk.TextBoundary.SentenceEnd, out startOffset, out endOffset),
				"GetTextBeforeOffset,SentenceEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextBeforeOffset,SentenceEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextBeforeOffset,SentenceEnd,eo");
			
			Assert.AreEqual (expected,
				atkText.GetTextBeforeOffset (18, Atk.TextBoundary.SentenceStart, out startOffset, out endOffset),
				"GetTextBeforeOffset,SentenceStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextBeforeOffset,SentenceStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextBeforeOffset,SentenceStart,eo");
			
			Assert.AreEqual ("n",
				atkText.GetTextBeforeOffset (18, Atk.TextBoundary.Char, out startOffset, out endOffset),
				"GetTextBeforeOffset,Char");
			Assert.AreEqual (17, startOffset, "GetTextBeforeOffset,Char,so");
			Assert.AreEqual (18, endOffset, "GetTextBeforeOffset,Char,eo");
			Assert.AreEqual ("e",
				atkText.GetTextBeforeOffset (23, Atk.TextBoundary.Char, out startOffset, out endOffset));
			Assert.AreEqual (22, startOffset, "GetTextBeforeOffset,Char,so");
			Assert.AreEqual (23, endOffset, "GetTextBeforeOffset,Char,eo");
			Assert.AreEqual ("n",
				atkText.GetTextBeforeOffset (name.Length - 2, Atk.TextBoundary.Char, out startOffset, out endOffset));
			Assert.AreEqual (name.Length - 3, startOffset, "GetTextBeforeOffset,Char,so");
			Assert.AreEqual (name.Length - 2, endOffset, "GetTextBeforeOffset,Char,eo");
			Assert.AreEqual ("e",
				atkText.GetTextBeforeOffset (name.Length - 1, Atk.TextBoundary.Char, out startOffset, out endOffset));
			Assert.AreEqual (name.Length - 2, startOffset, "GetTextBeforeOffset,Char,so");
			Assert.AreEqual (name.Length - 1, endOffset, "GetTextBeforeOffset,Char,eo");
			Assert.AreEqual ("?",
				atkText.GetTextBeforeOffset (name.Length, Atk.TextBoundary.Char, out startOffset, out endOffset));
			Assert.AreEqual (name.Length - 1, startOffset, "GetTextBeforeOffset,Char,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextBeforeOffset,Char,eo");
			Assert.AreEqual (null,
				atkText.GetTextBeforeOffset (-1, Atk.TextBoundary.Char, out startOffset, out endOffset));
			Assert.AreEqual (name.Length - 1, startOffset, "GetTextBeforeOffset,Char,so");
			Assert.AreEqual (name.Length, endOffset, "GetTextBeforeOffset,Char,eo");
			
			
			
			
			name = "Tell me; here a sentence\r\nwith EOL but without dot, and other phrase... Heh!";

			atkText = (Atk.Text)
				GetAtkObjectThatImplementsInterface <Atk.Text> (type, name, out accessible, false);
			Assert.AreEqual (name, atkText.GetText(0, name.Length), "GetText#2");
			
			expected = "\r\nwith EOL but without dot, and other phrase...";
			Assert.AreEqual (expected,
				atkText.GetTextAfterOffset (3, Atk.TextBoundary.SentenceEnd, out startOffset, out endOffset),
				"GetTextAfterOffset,SentenceEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAfterOffset,SentenceEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAfterOffset,SentenceEnd,eo");
			
			expected = "Tell me; here a sentence\r\n";
			Assert.AreEqual (expected,
				atkText.GetTextAtOffset (4, Atk.TextBoundary.SentenceStart, out startOffset, out endOffset),
				"GetTextAtOffset,SentenceStart");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAtOffset,SentenceStart,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAtOffset,SentenceStart,eo");
			
			expected = "Tell me; here a sentence";
			Assert.AreEqual (expected,
				atkText.GetTextAtOffset (4, Atk.TextBoundary.SentenceEnd, out startOffset, out endOffset),
				"GetTextAtOffset,SentenceEnd");
			Assert.AreEqual (name.IndexOf (expected), startOffset, "GetTextAtOffset,SentenceEnd,so");
			Assert.AreEqual (name.IndexOf (expected) + expected.Length, endOffset, "GetTextAtOffset,SentenceEnd,eo");
			
			
			return accessible;
		}
	}
}
