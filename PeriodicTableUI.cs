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
using System.Threading;
using Gtk;
using Glade;

using GladeXml = Glade.XML;

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
      Window loaderWindow;
      [Widget]
      Button cancelButton;

      PeriodicTableLogic logic;
      Thread loaderThread;

      public Loader(PeriodicTableLogic logic, Window mainWindow)
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

    PeriodicTableLogic logic;
    PeriodicTableRenderer renderer;
    Loader loader;

    public PeriodicTableUI(PeriodicTableLogic logic, PeriodicTableRenderer renderer)
    {
      this.logic = logic;
      this.renderer = renderer;
      Application.Init();
      GladeXml dlg_main = new GladeXml(null, "main.glade", "mainWindow", null);
      dlg_main.Autoconnect(this);
      loader = new Loader(logic, mainWindow);
    }

    public void Run()
    {
      mainWindow.Hide();
      loader.Run();
      Application.Run();
    }

    [Widget]
    Window mainWindow;
  }
}