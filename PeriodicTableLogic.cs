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
using System.Text.RegularExpressions;
using System.Xml;

namespace Spell
{
  class PeriodicTableLogic
  {
    public struct Element
    {
      public string Name;
      public string Symbol;
      public int Atomic;
    }

    periodictable table;
    Element[] elements;

    public PeriodicTableLogic()
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
        elements.Add(element);
      }
      this.elements = elements.OrderByDescending(e => e.Symbol.Length).ToArray();
    }

    public Element?[] Spell(string word)
    {
      Dictionary<int, Element> indexed = new Dictionary<int, Element>();
      word = Regex.Replace(word.ToLower(), "[^a-z\\s]", "");
      foreach (Element element in elements)
      {
        string symbol = element.Symbol.ToLower();
	if (word.Contains(symbol))
	{
	  foreach (int i in word.IndexOfAll(symbol))
	    indexed.Add(i, element);
          word = word.Replace(symbol, new string ('_', symbol.Length));
	}
      }
      List<Element?> spelled = new List<Element?>();
      int max = indexed.Max(item => item.Key);
      Element value;
      for (int i = 0; i < max; i++)
      {
        if (indexed.TryGetValue(i, out value))
          spelled.Add(value);
        else if (word[i] == ' ')
          spelled.Add(null);
      }
      return spelled.ToArray();
    }
  }
}