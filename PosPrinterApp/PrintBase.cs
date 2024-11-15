// Copyright © 2018 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Drawing;
using System.Drawing.Printing;
using Printer;

namespace PosPrinterApp;

public abstract class PrintBase
{
    protected Action<Graphics> print;
    protected float width;

    protected void Print(string printerName, Action<Graphics> print)
    {
        this.print = print;

        var pd = new PrintDocument();

        //pd.PrinterSettings.PrinterName = printerName; // TODO: it would be better to take printer name from the configuration settings
        pd.PrinterSettings.PrinterName = new PrinterSettings().PrinterName;
        
        
        pd.PrintPage += PrintPage;
        width = pd.DefaultPageSettings.PrintableArea.Width * 2.539993f / 10f; // Convert inches to millimeters

        try
        {
            pd.Print();
        }

        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void PrintPage(object sender, PrintPageEventArgs e)
    {
        var g = e.Graphics;

        g.PageUnit = GraphicsUnit.Millimeter;

        print(g);
    }

    protected float DrawTextColumns(Graphics g, float y, params TextColumn[] textColumns)
    {
        var maxHeight = 0f;
        var relativeLeft = 0f;

        foreach (var textColumn in textColumns)
            using (var font = new Font(FontFamily.GenericSansSerif, textColumn.FontSize, FontStyle.Regular))
            {
                var size = g.MeasureString(textColumn.Text, font, new SizeF(width * textColumn.RelativeWidth, 0));

                if (maxHeight < size.Height)
                    maxHeight = size.Height;

                g.DrawString(
                    textColumn.Text,
                    font,
                    Brushes.Black,
                    new RectangleF(relativeLeft, y, width * textColumn.RelativeWidth, size.Height),
                    new StringFormat { Alignment = textColumn.Alignment }
                );

                relativeLeft += width * textColumn.RelativeWidth;
            }

        return maxHeight;
    }
}