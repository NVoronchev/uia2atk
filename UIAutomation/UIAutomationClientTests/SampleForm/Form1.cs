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

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SampleForm {
	public partial class Form1 : Form {

		public Form1 ()
		{
			InitializeComponent ();
		}

		private void button1_Click (object sender, EventArgs e)
		{
			textBox1.Text = "button1_click";
			label1.Text = "button1_click";
			Console.WriteLine ("textbox1 & label1's texts are modified.");
		}

		private void button2_Click (object sender, EventArgs e)
		{
			button1.PerformClick ();
		}
		
		private void btnAddTextbox_Click (object sender, EventArgs e)
		{
			TextBox box = new TextBox();
			box.Width = 30;
			box.Left = 10;
			box.Top = panel1.Controls.Count * 25;
			panel1.Controls.Add(box);
		}

		private void btnRemoveTextbox_Click (object sender, EventArgs e)
		{
			if (panel1.Controls.Count <= 2)
				throw new Exception ("No more child control to delete");
			Control controlToDelete = null;
			foreach (Control c in panel1.Controls)
			{
				if (controlToDelete == null || controlToDelete.Top < c.Top)
					controlToDelete = c;
			}
			if (controlToDelete != null)
				panel1.Controls.Remove (controlToDelete);
		}
	}
}