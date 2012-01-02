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
      table.Init();
      table.Spell("bacon");
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