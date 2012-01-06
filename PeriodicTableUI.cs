/*
    periodic-table-spell, useless program
    Copyright (C) 2011 Joseph Dillon

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using Gdk;
using Gtk;
using Glade;

using GladeXml = Glade.XML;
using GtkWindow = Gtk.Window;
using GtkImage = Gtk.Image;

namespace Spell
{
  class PeriodicTableUI
  {
    class Loader
    {
      [Widget]
      Label progressStep;
      [Widget]
      ProgressBar totalProgress;
      [Widget]
      GtkWindow loaderWindow;
      [Widget]
      Button cancelButton;

      PeriodicTableLogic logic;
      Thread loaderThread;

      public Loader(PeriodicTableLogic logic, GtkWindow mainWindow)
      {
        GladeXml dlg_loading = new GladeXml(null, "loader.glade", "loaderWindow", null);
        dlg_loading.Autoconnect(this);
        this.logic = logic;
        loaderWindow.Hidden += new EventHandler(delegate(object sender, EventArgs e)
	{
	  Application.Invoke(delegate(object sender2, EventArgs e2) { mainWindow.ShowAll(); });
	});
        loaderWindow.DeleteEvent += new DeleteEventHandler(delegate(object sender, DeleteEventArgs e)
	{
	  loaderThread.Abort();
	});
        cancelButton.Clicked += new EventHandler(delegate(object sender, EventArgs e)
	{
	  loaderWindow.Destroy();
	  loaderThread.Abort();
	});
      }

      public void Run()
      {
        loaderThread = new Thread(new ThreadStart(delegate()
        {
          logic.Init("./PeriodicTable.dat", new PeriodicTableLogic.HandleProgress(delegate(double progress, string status)
          {
            try
	    {
              Application.Invoke(delegate(object sender2, EventArgs e2) { totalProgress.Adjustment.Value = progress; progressStep.Text = status; });
            }
            catch (NullReferenceException)
	    {
	      return;
	    }
          }));
          try
	  {
            Application.Invoke(delegate(object sender2, EventArgs e2) { loaderWindow.Hide(); });
          }
          catch (NullReferenceException)
          {
            return;
          }
        }));
        loaderThread.Start();
      }
    }

    Bitmap loadedBitmap;
    Loader loader;
    AboutDialog aboutDialog;

    public PeriodicTableUI(PeriodicTableLogic logic, PeriodicTableRenderer renderer)
    {
      Application.Init();
      aboutDialog = new AboutDialog();
      GladeXml dlg_main = new GladeXml(null, "main.glade", "mainWindow", null);
      dlg_main.Autoconnect(this);
      chunkSelect.Active = true;
      loader = new Loader(logic, mainWindow);
      generate.Clicked += new EventHandler(delegate (object sender, EventArgs e)
      {
        if (!string.IsNullOrEmpty(text.Buffer.Text))
	{
          MemoryStream ms = new MemoryStream();
          (loadedBitmap = renderer.Render(logic.Spell(text.Buffer.Text, chunkSelect.Active && !elementSearch.Active ? PeriodicTableLogic.SearchAlgorithm.ChunkSearch : PeriodicTableLogic.SearchAlgorithm.ElementBased))).Save(ms, ImageFormat.Png);
          output.Clear();
          ms.Position = 0;
	  using (Pixbuf oldPixbuf = output.Pixbuf)
	    output.Pixbuf = new Pixbuf(ms);
	}
        else
	{
	  MessageDialog md = new MessageDialog (mainWindow, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, "Please enter text into the conversion field");
          md.Run();
          md.Destroy();
	}
        UpdateSensitivity();
      });
      saveAs.Clicked += new EventHandler(delegate (object sender, EventArgs e)
      {
        FileChooserDialog fc = new Gtk.FileChooserDialog("Save As...", mainWindow, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept)
	{
	  DoOverwriteConfirmation = true
	};
        FileFilter filter = new FileFilter();
	filter.Name = "PNG images";
	filter.AddMimeType("image/png");
	filter.AddPattern("*.png");
        fc.AddFilter(filter);
        if (fc.Run() == (int)ResponseType.Accept)
	{
          Stream file = File.Open(string.IsNullOrEmpty(Path.GetExtension(fc.Filename)) ? (fc.Filename + ".png") : fc.Filename, FileMode.OpenOrCreate);
          loadedBitmap.Save(file, ImageFormat.Png);
	  file.Close();
	}
	fc.Destroy();
      });
      about.Clicked += new EventHandler(delegate (object sender, EventArgs e)
      {
        aboutDialog.Run();
      });
      Assembly asm = Assembly.GetExecutingAssembly ();
      aboutDialog.ProgramName = (asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title;
      aboutDialog.Version = asm.GetName().Version.ToString ();
      aboutDialog.Comments = (asm.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute).Description;
      aboutDialog.Copyright = (asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute).Copyright;
      aboutDialog.License = license;
      aboutDialog.Authors = authors;
    }

    void UpdateSensitivity()
    {
      saveAs.Sensitive = loadedBitmap != null;
    }

    public void Run()
    {
      UpdateSensitivity();
      mainWindow.Hide();
      loader.Run();
      Application.Run();
    }

    static string[] authors = new string[]
    {
      "Joseph Dillon <joe@w00t42.net>"
    };
	
    static string license =
@"Copyright (C) 2011 Joseph Dillon

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, either version 3 of the
License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.";

    [Widget]
    GtkWindow mainWindow;
    [Widget]
    TextView text;
    [Widget]
    RadioButton chunkSelect;
    [Widget]
    RadioButton elementSearch;
    [Widget]
    Button generate;
    [Widget]
    GtkImage output;
    [Widget]
    Button saveAs;
    [Widget]
    Button about;
  }
}