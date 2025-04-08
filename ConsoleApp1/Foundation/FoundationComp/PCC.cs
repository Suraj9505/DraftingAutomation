using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;

namespace DraftingAutomation.Foundation.FoundationComp
{
    public class PCC
    {
        private static Layer pccLayer = new Layer("PCCLayer")
        {
            Color = new AciColor(8),
        };

        public static void DrawPCC(double pccWX, double pccDepth, Vector2 pos, DxfDocument dxf)
        {
            Rectangle.DrawRectangleWithCenter(pos, pccWX, pccDepth, false, pccLayer, dxf);

            // hatch
            List<Vector2> boundary =
            [
                new(pos.X - pccWX / 2, pos.Y - pccDepth / 2),
                new(pos.X + pccWX / 2, pos.Y - pccDepth / 2),
                new(pos.X + pccWX / 2, pos.Y + pccDepth / 2),
                new(pos.X - pccWX / 2, pos.Y + pccDepth / 2)
            ];

            List<Line> lines =
            [
                new(boundary[0], boundary[1]) { Layer = pccLayer },
                new(boundary[1], boundary[2]) { Layer = pccLayer },
                new(boundary[2], boundary[3]) { Layer = pccLayer },
                new(boundary[3], boundary[0]) { Layer = pccLayer }
            ];

            //string patternDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Patterns");

            //HatchPattern concrete = HatchPattern.Load(patternDirectory, "concre");

            HatchBoundaryPath hatchBoundaryPath = new HatchBoundaryPath(lines);

            Hatch hatch = new Hatch(HatchPattern.Line, new List<HatchBoundaryPath> { hatchBoundaryPath }, true);
            
            hatch.Layer = pccLayer; // Set the hatch layer
            hatch.Pattern.Scale = 30; // Set the hatch pattern scale

            dxf.Entities.Add(hatch); // Add the hatch to the document
        }
    }
}