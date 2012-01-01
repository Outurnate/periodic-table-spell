using System;

namespace Spell
{

  static class Program
  {
    public static void Main()
    {
      PeriodicTable table = new PeriodicTable();
      Console.WriteLine(table.GetAtoms());
    }
  }
}