// TestBase.cs: Base class of tests
//
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
// Copyright (c) 2010 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Ray Wang <rawang@novell.com>
//	Felicia Mu <fxmu@novell.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using System.Diagnostics;
using System.Windows.Automation;

namespace Mono.UIAutomation.TestFramework
{
	// TestBase class helps to launch sample, initiation etc
	[TestFixture]
	public class TestBase
	{
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();
		protected Helper helper = new Helper ();

		[SetUp]
		public void Init ()
		{
			ProcedureLogger.Init ();
			LaunchSample ();
			Thread.Sleep (Config.Instance.MediumDelay);
			OnSetup ();
		}

		[TearDown]
		public void Quit ()
		{
			OnQuit ();
		}

		protected virtual void LaunchSample ()
		{
		}

		protected virtual void OnSetup ()
		{
		}

		protected virtual void OnQuit ()
		{
		}
		

		public void HandleException (Exception ex)
		{
			procedureLogger.Action ("Error: " + ex.ToString());
			procedureLogger.ExpectedResult ("A Exception has been thrown.");
			procedureLogger.Save ();
		}

		protected void Run (System.Action action)
		{
			try {
				action ();
			} catch (Exception ex) {
				HandleException (ex);
				throw;
			}
		}

		protected void SingleInstance (string windowTitle)
		{
			var ae = AutomationElement.RootElement.FindFirst (TreeScope.Children,
			                                                  new PropertyCondition (
			                                                  AutomationElementIdentifiers.NameProperty,
			                                                  windowTitle));
			if (ae != null) {
				Process.GetProcessById (ae.Current.ProcessId).Kill ();
				Thread.Sleep (Config.Instance.LongDelay);
			}
		}
	}
}