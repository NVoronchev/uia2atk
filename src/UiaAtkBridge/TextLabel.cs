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
using System.Windows.Automation;
using System.Windows.Automation.Provider;

namespace UiaAtkBridge
{

	public class TextLabel : Adapter , Atk.TextImplementor 
	{
		private IRawElementProviderSimple provider;
		
		private int cursorPosition = 0;
		
		public TextLabel ()
		{
			Role = Atk.Role.Label;
			
			string text = (string) provider.GetPropertyValue (AutomationElementIdentifiers.NameProperty.Id);
			Name = text;
		}
		
		public override IRawElementProviderSimple Provider {
			get { return provider; }
		}

		public int CaretOffset {
			get {
				return cursorPosition;
			}
		}

		public GLib.SList DefaultAttributes {
			get {
				//TODO:
				GLib.SList attribs = new GLib.SList(typeof(Atk.TextAttribute));
				return attribs;
			}
		}

		public int CharacterCount {
			get {
				return Name.Length;
			}
		}

		public int NSelections {
			get {
				throw new NotImplementedException();
			}
		}
		
		public override void RaiseAutomationPropertyChangedEvent (AutomationPropertyChangedEventArgs e)
		{
			// TODO
		}
		
		public override void RaiseAutomationEvent (AutomationEvent eventId, AutomationEventArgs e)
		{
			// TODO
		}

		public string GetText (int startOffset, int endOffset)
		{
			return Name.Substring (startOffset, endOffset);
		}

		public string GetTextAfterOffset (int offset, Atk.TextBoundary boundary_type, out int start_offset, out int end_offset)
		{
			throw new NotImplementedException ();
		}

		public string GetTextAtOffset (int offset, Atk.TextBoundary boundaryType, out int startOffset, out int end_offset)
		{
			throw new NotImplementedException();
		}

		public char GetCharacterAtOffset (int offset)
		{
			if (offset >= Name.Length)
				return '\0';
			return Name.ToCharArray () [offset];
		}

		public string GetTextBeforeOffset (int offset, Atk.TextBoundary boundaryType, out int startOffset, out int endOffset)
		{
			throw new NotImplementedException();
		}

		public GLib.SList GetRunAttributes (int offset, out int startOffset, out int endOffset)
		{
			// don't ask me why, this is what gail does 
			// (instead of throwing or returning null):
			if (offset > Name.Length)
				offset = Name.Length;
			else if (offset < 0)
				offset = 0;
			
			//just test values for now:
			endOffset = Name.Length;
			startOffset = offset;
				
			//TODO:
			GLib.SList attribs = new GLib.SList(typeof(Atk.TextAttribute));
			return attribs;
		}

		public void GetCharacterExtents (int offset, out int x, out int y, out int width, out int height, Atk.CoordType coords)
		{
			throw new NotImplementedException();
		}

		public int GetOffsetAtPoint (int x, int y, Atk.CoordType coords)
		{
			throw new NotImplementedException();
		}

		public string GetSelection (int selectionNum, out int startOffset, out int endOffset)
		{
			throw new NotImplementedException();
		}

		public bool AddSelection (int startOffset, int endOffset)
		{
			throw new NotImplementedException();
		}

		public bool RemoveSelection (int selectionNum)
		{
			throw new NotImplementedException();
		}

		public bool SetSelection (int selectionNum, int startOffset, int endOffset)
		{
			throw new NotImplementedException();
		}

		public bool SetCaretOffset (int offset)
		{
			throw new NotImplementedException();
		}

		public void GetRangeExtents (int startOffset, int endOffset, Atk.CoordType coordType, Atk.TextRectangle rect)
		{
			throw new NotImplementedException();
		}

		public Atk.TextRange GetBoundedRanges (Atk.TextRectangle rect, Atk.CoordType coordType, Atk.TextClipType xClipType, Atk.TextClipType yClipType)
		{
			throw new NotImplementedException();
		}
		
	}
}
