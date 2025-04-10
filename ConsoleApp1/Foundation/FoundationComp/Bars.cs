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
    public class Bars
    {

        public static void DrawBars(List<Vector2> linePoints, DxfDocument dxf)
        {

            Line line1 = new Line(linePoints[0], linePoints[1]) { Layer = new Layer("verticalBarsLayer") };
            Line line2 = new Line(linePoints[0], linePoints[2]) { Layer = new Layer("verticalBarsLayer") };
            Line line3 = new Line(linePoints[2], linePoints[3]) { Layer = new Layer("verticalBarsLayer") };

            dxf.Entities.Add(line1);
            dxf.Entities.Add(line2);
            dxf.Entities.Add(line3);


        }
    }
}
