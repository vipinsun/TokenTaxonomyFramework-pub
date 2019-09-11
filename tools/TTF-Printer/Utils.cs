using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TTI.TTF.Taxonomy
{
    public static class Utils
    {
        public static Table GetNewTable(int columns)
        {
            Table table = new Table();

            TableProperties props = new TableProperties(
                new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    }));

            table.AppendChild<TableProperties>(props);

            for (var j = 0; j <= columns; j++)
            {
                var tc = new TableCell();
                // Code removed here…
                table.Append(tc);
            }

            return table;
        }
    }
}