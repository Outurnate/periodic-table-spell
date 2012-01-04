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
using System.Collections;

namespace Spell
{
  static class Program
  {
    public static void Main()
    {
      PeriodicTableLogic table = new PeriodicTableLogic();
      PeriodicTableRenderer renderer = new PeriodicTableRenderer(new PeriodicTableRenderOptions() 
								 {
								   ElementWidth = 64,
                                                                   ElementHeight = 64,
                                                                   LinePen = new System.Drawing.Pen(System.Drawing.Color.Black),
                                                                   ForceAntiAlias = true,
                                                                   SymbolBrush = System.Drawing.Brushes.Black,
                                                                   Font = "Tahoma"
								 });
      PeriodicTableUI ui = new PeriodicTableUI(table, renderer);
      ui.Run();
    }

    public static IEnumerable IndexOfAll(this string input, string search)
    {
      int pos, offset = 0;
      int length = search.Length;
      while ((pos = input.IndexOf(search, offset)) != -1)
      {
        yield return pos;
        offset = pos + length;
      }
    }
  }
}