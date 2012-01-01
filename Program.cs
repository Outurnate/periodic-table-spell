using System;
using System.Collections.Generic;
using System.Xml;

namespace Spell
{
  static class Program
  {
    public static void Main()
    {
      PeriodicTable table = new PeriodicTable();
      table.Init();
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
  }
}