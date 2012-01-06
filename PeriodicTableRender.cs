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
using System.Drawing.Text;

namespace Spell
{
  class PeriodicTableRenderer
  {
    PeriodicTableRenderOptions options;

    public PeriodicTableRenderer(PeriodicTableRenderOptions options)
    {
      this.options = options;
    }

    public Bitmap Render(Element?[] elements)
    {
      Bitmap output = new Bitmap(elements.Length * (options.ElementWidth + 1), options.ElementHeight);
      Graphics g = Graphics.FromImage(output);
      StringFormat symbolFormat = new StringFormat();
      symbolFormat.Alignment = symbolFormat.LineAlignment = StringAlignment.Center;
      for (int i = 0; i < elements.Length; i++)
        if (elements[i].HasValue)
	{
          Rectangle currentRectangle = new Rectangle(i * (options.ElementWidth + 1), 0, options.ElementWidth - 1, options.ElementHeight - 1);
          g.TextRenderingHint = options.ForceAntiAlias ? TextRenderingHint.AntiAlias : TextRenderingHint.SystemDefault;
	  g.DrawRectangle(options.LinePen, currentRectangle);
          g.DrawString(elements[i].Value.Symbol, new Font(options.Font, 20), options.SymbolBrush, new Rectangle(currentRectangle.X, currentRectangle.Y, currentRectangle.Width, currentRectangle.Height), symbolFormat);
          g.DrawString(elements[i].Value.Atomic.ToString(), new Font(options.Font, 8), options.SymbolBrush, new Rectangle(currentRectangle.X + (options.ElementWidth - 18), currentRectangle.Y, 16, 16), symbolFormat);
          g.DrawString(elements[i].Value.Name, new Font(options.Font, 6), options.SymbolBrush, new Rectangle(currentRectangle.X, options.ElementHeight - 16, options.ElementWidth, 16), symbolFormat);
	}
      return output;
    }
  }
}