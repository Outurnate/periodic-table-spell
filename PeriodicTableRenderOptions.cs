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

namespace Spell
{
  struct PeriodicTableRenderOptions
  {
    public int ElementWidth
    {
      get;
      set;
    }
    public int ElementHeight
    {
      get;
      set;
    }
    public Pen LinePen
    {
      get;
      set;
    }
    public bool ForceAntiAlias
    {
      get;
      set;
    }
    public Brush SymbolBrush
    {
      get;
      set;
    }
    public Brush NumberBrush
    {
      get;
      set;
    }
    public Brush NameBrush
    {
      get;
      set;
    }
    public string Font
    {
      get;
      set;
    }
  }
}