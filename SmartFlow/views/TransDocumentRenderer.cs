using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;

namespace SmartFlow.views
{
    class TransDocumentRenderer : IDocumentRenderer
    {
        public void Render(FlowDocument doc, object data)
        {
            TableRowGroup group = doc.FindName("rowsDetails") as TableRowGroup;
            Style styleCell = doc.Resources["BorderedCell"] as Style;
            foreach (TransDetail item in ((TransMaster)data).TransDetails)
            {
                TableRow row = new TableRow();

                TableCell cell = new TableCell(new Paragraph(new Run(item.ProductName)));
                cell.Style = styleCell;
                row.Cells.Add(cell);

                cell = new TableCell(new Paragraph(new Run(item.Quantity.ToString("N2"))));
                cell.Style = styleCell;
                row.Cells.Add(cell);

                cell = new TableCell(new Paragraph(new Run(item.Price.ToString("N2"))));
                cell.Style = styleCell;
                row.Cells.Add(cell);

                cell = new TableCell(new Paragraph(new Run((item.Quantity * item.Price).ToString("N2"))));
                cell.Style = styleCell;
                row.Cells.Add(cell);


                group.Rows.Add(row);
            }
        }
    }
}
