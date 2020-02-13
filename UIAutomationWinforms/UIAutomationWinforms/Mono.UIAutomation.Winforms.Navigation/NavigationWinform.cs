// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// Copyright (c) 2019 AxxonSoft (http://axxonsoft.com)
//
// Authors:
//   Nikita Voronchev <nikita.voronchev@ru.axxonsoft.com>
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Automation.Provider;
using Mono.UIAutomation.Helpers;
using Mono.UIAutomation.Services;
using Mono.UIAutomation.Winforms.UserCustom;

namespace Mono.UIAutomation.Winforms.Navigation
{
	internal partial class NavigationWinform
	{
		private static bool NavigationTreeErrAsException = (EnvironmentVaribles.MONO_UIA_NAVIGATION_TREE_ERR == MONO_UIA_NAVIGATION_TREE_ERR.Exception);

		public NavigationWinform (FragmentControlProvider provider)
		{
			Provider = provider;
		}

		public readonly FragmentControlProvider Provider;
		public UserCustomFragmentProviderWrapper UserCustomProvider { get; private set; }

		public IRawElementProviderFragment PreviousVisibleProvider
		{
			get
			{
				var p = PreviousWinformSibling;
				while (p != null)
				{
					if (p.IsReallyVisible ())
						return p;
					p = p.Navigation.PreviousWinformSibling;
				}
				return null;
			}
		}

		public  IRawElementProviderFragment NextVisibleProvider
		{
			get
			{
				var p = NextWinformSibling;
				while (p != null)
				{
					if (p.IsReallyVisible ())
						return p;
					p = p.Navigation.NextWinformSibling;
				}
				return Parent?.Navigation.UserCustomProvider;
			}
		}

		public IRawElementProviderFragment FirstVisibleChildProvider
		{
			get
			{
				var first = FirstWinformChild;
				if (first == null)
					return null;
				if (first.IsReallyVisible ())
					return first;
				return first.Navigation.NextVisibleProvider ?? UserCustomProvider;
			}
		}

		public IRawElementProviderFragment LastVisibleChildProvider
		{
			get
			{
				if (UserCustomProvider != null)
					return UserCustomProvider;
				var last = LastWinformChild;
				if (last == null)
					return null;
				if (last.IsReallyVisible ())
					return last;
				return last.Navigation.PreviousVisibleProvider;
			}
		}

		public FragmentControlProvider Parent { get; protected set; }
		public FragmentControlProvider PreviousWinformSibling { get; protected set; }
		public FragmentControlProvider NextWinformSibling { get; protected set; }
		public FragmentControlProvider FirstWinformChild { get; protected set; }
		public FragmentControlProvider LastWinformChild { get; protected set; }

		public void InsertChildBefore (FragmentControlProvider newChild, FragmentControlProvider baseChild)
		{
			if (newChild == null)
				throw new ArgumentNullException ("newChild");
			if (baseChild == null)
				throw new ArgumentNullException ("baseChild");

			if (!newChild.Navigation.IsCleared ()) {
				var errMsg = GetNewChildIsClearedErrorMessage (newChild);
				if (newChild.Navigation.Parent == null || NavigationTreeErrAsException)
					throw new ArgumentException (errMsg);
				else
					Log.Error (errMsg);
			}

			InsertWinformChildBefore (newChild, baseChild);
		}

		public void AppendChild (FragmentControlProvider newChild)
		{
			if (newChild == null)
				throw new ArgumentNullException ("newChild");

			if (!newChild.Navigation.IsCleared ()) {
				var errMsg = GetNewChildIsClearedErrorMessage (newChild);
				if (newChild.Navigation.Parent == null || NavigationTreeErrAsException)
					throw new ArgumentException (errMsg);
				else
					Log.Error (errMsg);
			}

			AppendWinformChildToEnd (newChild);
		}

		public void RemoveChild (FragmentControlProvider child)
		{
			if (child == null)
				throw new ArgumentNullException ("child");
			RemoveWinformChild (child);
		}

		public void SetWfcUserCustomProviderWrapper (UserCustomFragmentProviderWrapper wrapper)
		{
			if (UserCustomProvider != null)
				throw new Exception ($"Provider {Provider} already has User Custom Provider: {UserCustomProvider}. Cann't set the new {wrapper}");
			UserCustomProvider = wrapper;
		}

		public int VisibleWinformChildrenCount => EnumerateVisibleChildren ().Count ();

		public bool ContainsVisibleChildComponent (Component child)
		{
			return GetVisibleChildByComponent (child) != default (FragmentControlProvider);
		}

		public bool ContainsAnyChildComponent (Component child)
		{
			return GetAnyChildByComponent (child) != default (FragmentControlProvider);
		}

		public bool ContainsAnyChild (FragmentControlProvider child)
		{
			return IterateWinformsChildren ().Contains (child);
		}
		
		public FragmentControlProvider GetVisibleChildByComponent (Component child)
		{
			return IterateWinformsChildren ().FirstOrDefault (c => c.Component == child);
		}

		public FragmentControlProvider GetAnyChildByComponent (Component child)
		{
			return IterateWinformsChildren ().FirstOrDefault (c => c.Component == child);
		}

		public FragmentControlProvider GetVisibleChild (int index)
		{
			return IterateWinformsChildren ()
				.SkipWhile ((ch,i) => i < index)
				.First ();
		}

		public FragmentControlProvider GetChild (int index)
		{
			return IterateWinformsChildren ()
				.SkipWhile ((ch,i) => i < index)
				.First ();
		}

		public FragmentControlProvider[] GetVisibleChildren ()
		{
			return EnumerateVisibleChildren ().ToArray ();
		}

		public FragmentControlProvider[] GetAllChildren ()
		{
			return IterateWinformsChildren ().ToArray ();
		}

		private IEnumerable<FragmentControlProvider> EnumerateVisibleChildren ()
		{
			return IterateWinformsChildren ().Where (ch => ch.IsReallyVisible ());
		}

		public void Clear ()
		{
			foreach (var ch in GetAllChildren ())
				ch.Navigation.Clear ();
			Parent = null;
			PreviousWinformSibling = null;
			NextWinformSibling = null;
			FirstWinformChild = null;
			LastWinformChild = null;
		}

		public bool IsCleared ()
		{
			return Parent == null && PreviousWinformSibling == null  && NextWinformSibling == null; // && _chainWinforms.Count == 0 && _chainCustoms.Count == 0;
		}

		public override string ToString ()
		{
			return $"<{this.GetType ()}:{Provider}>";
		}

		private string GetNewChildIsClearedErrorMessage (FragmentControlProvider newChild)
		{
			var errMsg =
				$"NewChildIsClearedError:"
				+ Environment.NewLine +  "  this.Provider:" + Environment.NewLine + this.Provider.Navigation.ToStringDetailed (indent: 4)
				+ Environment.NewLine +  "  newChild:" + Environment.NewLine + newChild.Navigation.ToStringDetailed (indent: 4)
				+ Environment.NewLine +  "  Old _parent:" + Environment.NewLine + $"{newChild.Navigation.Parent?.Navigation.ToStringDetailed (indent: 4)}"
				+ Environment.NewLine + $"  *** Set env variable MONO_UIA_NAVIGATION_TREE_ERR=(log|exception) to throw an Exception or correct error amd just log it."
				                      +  " Option 'log' is default. ***";
			return errMsg;
		}

		public string ToStringDetailed (int indent = 0)
		{
			var sindent =  new String (' ', indent);;
			return sindent + $"<{this.GetType ()}:{Provider}>" + Environment.NewLine
				+ sindent +  $"  Parent =               {Parent}" + Environment.NewLine
				+ sindent +  $"  PreviousWinform =     {PreviousWinformSibling}" + Environment.NewLine
				+ sindent +  $"  NextWinform =         {NextWinformSibling}" + Environment.NewLine
				+ sindent +  $"  FirstWinformChild =   {FirstWinformChild}" + Environment.NewLine
				+ sindent +  $"  LastWinformChild =    {LastWinformChild}" + Environment.NewLine
				+ sindent +  $"  UserCustomProvider =  {UserCustomProvider}";
		}

		private IEnumerable<FragmentControlProvider> IterateWinformsChildren ()
		{
			if (FirstWinformChild == null)
				yield break;

			var p = FirstWinformChild;
			while (true)
			{
				yield return p;
				if (p == LastWinformChild)
					yield break;
				p = p.Navigation.NextWinformSibling;
			}
		}

		private void InsertWinformChildBefore (FragmentControlProvider newChild, FragmentControlProvider baseChild)
		{
			LinkOrdered (baseChild.Navigation.PreviousWinformSibling, newChild);
			LinkOrdered (newChild, baseChild);

			if (baseChild == FirstWinformChild)
				SetAsFirst (newChild);

			newChild.Navigation.Parent = this.Provider;
		}

		private void AppendWinformChildToEnd (FragmentControlProvider newChild)
		{
			LinkOrdered (LastWinformChild, newChild);

			SetAsLast (newChild);
			if (FirstWinformChild == null)
				SetAsFirst (newChild);

			newChild.Navigation.Parent = this.Provider;
		}

		private void RemoveWinformChild (FragmentControlProvider child)
		{
			if (child == null)
				throw new ArgumentNullException ();

			var childIsFirst = (child == FirstWinformChild);
			var childIsLast = (child == LastWinformChild);

			LinkOrdered (child.Navigation.PreviousWinformSibling, child.Navigation.NextWinformSibling);
			
			if (childIsFirst && childIsLast)
				SetLinksForEmptyCase ();
			else if (childIsFirst)
				SetAsFirst (child.Navigation.NextWinformSibling);
			else if (childIsLast)
				SetAsLast (child.Navigation.PreviousWinformSibling);

			child.Navigation.Parent = null;
			child.Navigation.NextWinformSibling = null;
			child.Navigation.PreviousWinformSibling = null;
		}

		private void SetLinksForEmptyCase ()
		{
			FirstWinformChild = null;
			LastWinformChild = null;
		}

		private void SetAsLast (FragmentControlProvider child)
		{
			LastWinformChild = child;
			LastWinformChild.Navigation.NextWinformSibling = null;
		}

		private void SetAsFirst (FragmentControlProvider child)
		{
			if (child == null)
				throw new Exception($"child==null");
			FirstWinformChild = child;
			FirstWinformChild.Navigation.PreviousWinformSibling = null;
		}

		private static void LinkOrdered (FragmentControlProvider p1, FragmentControlProvider p2)
		{
			if (p1 != null)
				p1.Navigation.NextWinformSibling = p2;
			if (p2 != null)
				p2.Navigation.PreviousWinformSibling = p1;
		}
	}
}
