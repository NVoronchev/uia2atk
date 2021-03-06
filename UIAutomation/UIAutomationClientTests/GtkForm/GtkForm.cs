using System;

using Gtk;

namespace GtkForm
{
	public class DemoMain
	{
		private Gtk.Window window;
		private Gtk.HBox hbox1;
		private Gtk.Label label1;
		private Gtk.Entry textBox1;
		private Gtk.TextView textBox3;
		private Gtk.TreeView treeView1;
		private Gtk.TreeView treeView2;
		private Gtk.TreeView listView1;
		private Gtk.TreeView dataGridView1;
		private Gtk.TreeStore tableStore;
		private Gtk.ScaleButton scaleButton1;
		private Gtk.HBox hboxPanel;
		private Gtk.Entry textBoxExtra;
		private Gtk.Entry txtCommand;
		private Gtk.Button button1;
		private Gtk.Button button2;
		private Gtk.Button button3;
		private Gtk.FileChooserDialog chooser;

		public static void Main (string[] args)
		{
			Application.Init ();
			new DemoMain ();
			Application.Run ();
		}

		public DemoMain ()
		{
			window = new Gtk.Window ("TestForm1");
			Gtk.HBox hbox = new Gtk.HBox (false, 0);
			hbox1 = new Gtk.HBox (false, 0);
			Gtk.HBox hbox2 = new Gtk.HBox (false, 0);
			Gtk.HBox hbox3 = new Gtk.HBox (false, 0);
			hbox.Add (hbox1);
			window.SetDefaultSize (600, 400);
			window.DeleteEvent += new DeleteEventHandler (WindowDelete);

			button1 = new Gtk.Button ("button1");
			button1.Clicked += Button1Clicked;
			button2 = new Gtk.Button ("button2");
			button3 = new Gtk.Button ("button3");
			Gtk.Button button4 = new Gtk.Button ("button4");
			button4.Clicked += Button4Clicked;
			Gtk.Button button5 = new Gtk.Button ("button5");
			Gtk.Button button6 = new Gtk.Button ("button6");
			Gtk.Button button7 = new Gtk.Button ("button7");
			button7.Sensitive = false;

			scaleButton1 = new Gtk.ScaleButton (0, 0, 100, 10, new string [0]);

			hbox1.Add (hbox3);
			hbox1.Add (hbox2);
			hbox1.Add (button3);
			hbox1.Add (button2);

			button3.Accessible.Description = "help text 3";
			button3.Sensitive = false;

			label1 = new Gtk.Label ("label1");

			textBox1 = new Gtk.Entry ();
			Gtk.Entry textBox2 = new Gtk.Entry ();
			textBox2.Visibility = false;
			textBox2.Sensitive = false;
			textBox2.IsEditable = false;
			textBox3 = new Gtk.TextView ();
			// TODO: scrollbars
			Gtk.CheckButton checkbox1 = new Gtk.CheckButton ("checkbox1");
			Gtk.CheckButton checkbox2 = new Gtk.CheckButton ("checkbox2");
			checkbox2.Sensitive = false;

			Gtk.TreeStore store = new Gtk.TreeStore (typeof (string), typeof (string));
			Gtk.TreeIter [] iters = new Gtk.TreeIter [2];
			iters [0] = store.AppendNode ();
			store.SetValues (iters [0], "item 1", "item 1 (2)");
			iters [1] = store.AppendNode (iters [0]);
			store.SetValues (iters [1], "item 1a", "item 1a (2)");
			iters [0] = store.AppendNode ();
			store.SetValues (iters [0], "item 2", "item 2 (2)");
			iters [1] = store.AppendNode (iters [0]);
			store.SetValues (iters [1], "item 2a", "item 2a (2)");
			iters [1] = store.AppendNode (iters [0]);
			store.SetValues (iters [1], "item 2b", "item 2b (2)");
			treeView1 = new Gtk.TreeView (store);
			AddTreeViewColumn (treeView1, 0, "column 1");
			treeView1.CollapseAll ();

			treeView2 = new Gtk.TreeView (store);
			AddTreeViewColumn (treeView2, 0, "column 1");
			AddTreeViewColumn (treeView2, 1, "column 2");
			treeView2.CollapseAll ();
			treeView2.Accessible.Name = "treeView2";

			tableStore = new Gtk.TreeStore (typeof (string), typeof (string), typeof (string), typeof (string));
			iters [0] = tableStore.AppendNode ();
			tableStore.SetValues (iters [0], "False", "Alice", "24", "");
			iters [0] = tableStore.AppendNode ();
			tableStore.SetValues (iters [0], "True", "Bob", "28", "");
			dataGridView1 = new Gtk.TreeView (tableStore);
			dataGridView1 = new Gtk.TreeView (tableStore);
			dataGridView1 = new Gtk.TreeView (tableStore);
			AddTreeViewColumn (dataGridView1, 0, "Gender");
			AddTreeViewColumn (dataGridView1, 1, "Name");
			AddTreeViewColumn (dataGridView1, 2, "Age");
			dataGridView1.Accessible.Name = "dataGridView1";

			hboxPanel = new Gtk.HBox ();
			Gtk.Button btnRemoveTextBox = new Gtk.Button ("Remove");
			btnRemoveTextBox.Clicked += RemoveTextBoxClicked;
			Gtk.Button btnAddTextBox = new Gtk.Button ("Add");
			btnAddTextBox.Clicked += AddTextBoxClicked;
			txtCommand = new Gtk.Entry ();
			txtCommand.Accessible.Name = "txtCommand";
			Gtk.Button btnRun = new Gtk.Button ("Run");
			btnRun.Clicked += btnRunClicked;
			hboxPanel.Add (btnRemoveTextBox);
			hboxPanel.Add (btnAddTextBox);

			Gtk.TreeStore treeStore = new Gtk.TreeStore (typeof (string));
			Gtk.TreeIter iter = treeStore.AppendNode ();
			treeStore.SetValue (iter, 0, "Item 0");
			iter = treeStore.AppendNode ();
			treeStore.SetValue (iter, 0, "Item 1");
			listView1 = new Gtk.TreeView (treeStore);
			AddTreeViewColumn (listView1, 0, "items");
			listView1.Accessible.Name = "listView1";
			listView1.ExpandAll ();

			hbox2.Add (button5);
			hbox2.Add (checkbox1);
			hbox2.Add (checkbox2);
			hbox2.Add (button4);
			hbox2.Accessible.Name = "groupBox2";

			hbox3.Add (button7);
			hbox3.Add (button6);
			hbox3.Sensitive = false;
			hbox3.Accessible.Name = "groupBox3";

			hbox.Add (textBox3);
			hbox.Add (textBox2);
			hbox.Add (textBox1);
			hbox.Add (label1);
			hbox.Add (button1);
			hbox.Add (treeView1);
			hbox.Add (treeView2);
			hbox.Add (listView1);
			hbox.Add (dataGridView1);
			hbox.Add (txtCommand);
			hbox.Add (btnRun);
			hbox.Add (hboxPanel);
			hbox.Add (scaleButton1);

			Gtk.Menu file = new Gtk.Menu ();
			file.Append (new Gtk.MenuItem ("_New"));
			file.Append (new Gtk.MenuItem ("_Open"));
			file.Append (new Gtk.CheckMenuItem ("Check"));
			Gtk.MenuItem fileItem = new Gtk.MenuItem ("File");
			fileItem.Submenu = file;
			Gtk.Menu edit = new Gtk.Menu ();
			edit.Append (new Gtk.MenuItem ("_Undo"));
			edit.Append (new Gtk.SeparatorMenuItem ());
			edit.Append (new Gtk.MenuItem ("_Cut"));
			edit.Append (new Gtk.MenuItem ("Copy"));
			edit.Append (new Gtk.MenuItem ("_Paste"));
			Gtk.MenuItem editItem = new Gtk.MenuItem ("Edit");
			editItem.Submenu = edit;
			Gtk.MenuBar menuBar = new Gtk.MenuBar ();
			menuBar.Append (fileItem);
			menuBar.Append (editItem);
			hbox.Add (menuBar);

		hbox.Add (new Gtk.SpinButton (0, 100, 1));
		hbox.Add (new Gtk.ToggleButton ("ToggleButton"));
			Gtk.Adjustment adj = new Gtk.Adjustment (50, 0, 100,
				1, 10, 10);
		hbox.Add (new Gtk.VScrollbar (adj));

			window.Add (hbox);
			window.ShowAll ();
		}

		private void AddTreeViewColumn (Gtk.TreeView treeView, int i, string name)
		{
			AddTreeViewColumn (treeView, i, name, false);
		}

		private void AddTreeViewColumn (Gtk.TreeView treeView, int i, string name, bool toggle)
		{
			Gtk.TreeViewColumn col = new Gtk.TreeViewColumn ();
			col.Title = name;
			treeView.AppendColumn (col);
			Gtk.CellRenderer cell;
			if (toggle)
				cell = new Gtk.CellRendererToggle ();
			else
				cell = new Gtk.CellRendererText ();
			col.PackStart (cell, true);
			col.AddAttribute (cell, "text", i);
		}

		private void Button1Clicked (object o, EventArgs args)
		{
			textBox1.Text = "button1_click";
			label1.Text = "button1_click";
		}

		private void Button4Clicked (object o, EventArgs args)
		{
			treeView1.Sensitive = !treeView1.Sensitive;
			treeView2.Sensitive = !treeView2.Sensitive;
			scaleButton1.Sensitive = !scaleButton1.Sensitive;
		}

		private void RemoveTextBoxClicked (object o, EventArgs args)
		{
			if (textBoxExtra == null)
				throw new Exception ("No textBox to remove");
			hboxPanel.Remove (textBoxExtra);
			textBoxExtra = null;
		}

		private void AddTextBoxClicked (object o, EventArgs args)
		{
			if (textBoxExtra != null)
				throw new Exception ("Adding more than one TextBox not supported");
			textBoxExtra = new Gtk.Entry ();
			hboxPanel.Add (textBoxExtra);
		}

		private void btnRunClicked (object o, EventArgs args)
		{
			const string sampleText = "Lorem ipsum dolor sit amet";

			string cmd = txtCommand.Text;
			if (cmd == "click button1")
				button1.Activate ();
			else if (cmd == "set textbox3 text")
				textBox3.Buffer.Text = sampleText;
			else if (cmd == "set textBox3 long text")
				textBox3.Buffer.Text = "very very very very very very very very long text to enable the horizontal scroll bar";
			else if (cmd.StartsWith ("set textbox3 to "))
				textBox3.Buffer.Text = cmd.Substring (16).
					Replace ("\\n", "\n").
					Replace ("\\r", "\r");
			else if (cmd == "select textbox3") {
				if (textBox3.Buffer.Text.Length < 4)
					textBox3.Buffer.Text = sampleText;
				Gtk.TextIter startIter, endIter;
				textBox3.Buffer.GetSelectionBounds (out startIter, out endIter);
				int start, end;
				start = startIter.Offset;
				end = endIter.Offset;
				end = (end-start == 3? 4: 3);
				start = 0;
				startIter = textBox3.Buffer.GetIterAtOffset (start);
				endIter = textBox3.Buffer.GetIterAtOffset (end);
				textBox3.Buffer.MoveMark ("selection_bound", startIter);
				textBox3.Buffer.MoveMark ("insert", endIter);
			} else if (cmd == "add table row") {
				Gtk.TreeIter iter;
				iter = tableStore.AppendNode ();
				tableStore.SetValues (iter, "true", "Mallory", "40");
			} else if (cmd == "add table column")
			AddTreeViewColumn (treeView2, 3, "more");
			else if (cmd == "enable multiselect")
				listView1.Selection.Mode = Gtk.SelectionMode.Multiple;
			else if (cmd == "disable multiselect")
				listView1.Selection.Mode = Gtk.SelectionMode.Single;
			else if (cmd == "change button3 name")
				button3.Accessible.Name = "xyzzy";
			else if (cmd == "change button3 helptext")
				button3.Accessible.Description = "plugh";
			else if (cmd == "enable button3")
				button3.Sensitive = true;
			else if (cmd == "disable button3")
				button3.Sensitive = false;
			else if (cmd == "focus textBox3")
				textBox3.GrabFocus ();
			else if (cmd == "focus button2")
				button2.GrabFocus ();
			else if (cmd.StartsWith ("change title:"))
				window.Title = cmd.Substring (cmd.IndexOf (':') + 1);
			else if (cmd == "open FileChooser" && chooser == null) {
				chooser = new Gtk.FileChooserDialog ("FileChooser", window, FileChooserAction.Open, "data", 0);
				chooser.Show ();
			}
			else if (cmd == "close FileChooser" && chooser != null) {
				chooser.Destroy ();
				chooser = null;
			}
		}

		private void WindowDelete (object o, DeleteEventArgs args)
		{
			Application.Quit ();
			args.RetVal = true;
		}
	}
}
