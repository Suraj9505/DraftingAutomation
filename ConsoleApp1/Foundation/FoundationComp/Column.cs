using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Tables;

namespace DraftingAutomation.Foundation.FoundationComp
{
    public class Column
    {

        private static Layer columnLayer = new Layer("ColumnLayer")
        {
            Color = new AciColor(8),
        };

        public static void DrawColumn(double colWX, double colLen, Vector2 pos, DxfDocument dxf)
        {
            Rectangle.DrawRectangleWithCenter(pos, colWX, colLen, false, columnLayer, dxf);
        }
    }
}
