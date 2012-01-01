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
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Spell
{
  static class Program
  {
    public static void Main()
    {
      PeriodicTable table = new PeriodicTable();
      table.Init();
      table.Spell("bacon");
    }
  }

  class PeriodicTable
  {
    struct Element
    {
      public string Name;
      public string Symbol;
      public int Atomic;
    }

    periodictable table;
    Element[] elements;

    public PeriodicTable()
    {
      table = new periodictable();
    }

    public void Init()
    {
      List<Element> elements = new List<Element>();
      XmlDocument resolver = new XmlDocument();
      resolver.LoadXml(table.GetAtoms());
      foreach (XmlNode tag in resolver.GetElementsByTagName("ElementName"))
      {
        Element element = new Element() { Name = tag.InnerXml };
        resolver.LoadXml(table.GetAtomicNumber(element.Name));
        element.Symbol = resolver.GetElementsByTagName("Symbol")[0].InnerXml;
	element.Atomic = int.Parse(resolver.GetElementsByTagName("AtomicNumber")[0].InnerXml);
      }
      this.elements = elements.ToArray();
    }

    public void Spell(string word)
    {
      List<int> indexes = new List<int>();
      for (int i = 0; i < word.Length; i++)
        indexes.Add(i);
      foreach (Element element in elements)
	if (word.Contains(element.Symbol))
	  for (int i = 0; i < element.Symbol.Length; i++)
	  {
	    try
	    {
	      indexes.Remove(i);
	    }
            catch (Exception) { }
	  }
    }
  }
}