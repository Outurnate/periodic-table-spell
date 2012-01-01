using System;
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
      string Name;
      string Symbol;
      int Atomic;
    }

    periodictable table;
    Element[] elements;

    public PeriodicTable()
    {
      table = new periodictable();
    }

    public void Init()
    {
      XmlDocument resolver = new XmlDocument();
      resolver.LoadXml(table.GetAtoms());
    }
  }
}