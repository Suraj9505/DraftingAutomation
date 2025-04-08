using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Tables;

namespace DraftingAutomation.Foundation.FoundationComp
{
    public class Footing
    {
        private static Layer footingLayer = new Layer("FootingLayer")
        {
            Color = AciColor.Green,
        };
        public static void DrawFooting(double footWX, double footDepth, Vector2 pos, DxfDocument dxf)
        {
            Rectangle.DrawRectangleWithCenter(pos, footWX, footDepth, false, footingLayer, dxf);
        }
    }
}
