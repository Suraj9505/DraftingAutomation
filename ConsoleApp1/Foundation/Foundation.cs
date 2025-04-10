using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DraftingAutomation.Foundation.FoundationComp;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;

namespace DraftingAutomation.Foundation
{
    public class Foundation
    {
        public static void DrawFoundation(string stirrups, Vector2 pos, double rubbleThk, double pccWX, double pccDepth, double footWX, double footDepth, double colWX, double colLen, double footCC, double colCC, string footBarsBtmX, string footBarsTopX, string colReinVertical, DxfDocument dxf)
        {
           
            PCC.DrawPCC(pccWX, pccDepth, pos, dxf);

            double stirrupSpacing = Constants.ExtractNumbers(stirrups)[1];
            double botBarDia = Constants.ExtractNumbers(footBarsBtmX)[0];

            Vector2 footingCenter = new();
            Vector2 columnCenter = new();

            footingCenter.Y = pos.Y + pccDepth / 2 + footDepth /2;
            footingCenter.X = pos.X;

            columnCenter.X = pos.X;
            columnCenter.Y = footingCenter.Y + footDepth / 2 + colLen / 2;

            Footing.DrawFooting(footWX, footDepth, footCC, footingCenter, footBarsBtmX, footBarsTopX, dxf);

            Column.DrawColumn(colWX, colLen, columnCenter, stirrups, dxf);

            List <Vector2> footingPoints = Constants.GetRectanglePointsFromCenter(columnCenter, colWX, colLen);

            List<Vector2> bar1 = new List<Vector2>
            {
                new Vector2(footingPoints[3].X + colCC, footingPoints[3].Y - 70),
                new Vector2(footingPoints[2].X - 100, footingPoints[2].Y - 70),
                new Vector2(footingPoints[3].X + colCC, footingPoints[3].Y + 20 + botBarDia - colLen - footDepth),
                new Vector2(footingPoints[3].X - 400, footingPoints[3].Y + 20 + botBarDia - colLen - footDepth)
            };

            List<Vector2> bar2 = new List<Vector2>
            {
                new Vector2(footingPoints[2].X - colCC, footingPoints[2].Y - colCC),
                new Vector2(footingPoints[3].X + 100, footingPoints[3].Y - colCC),
                new Vector2(footingPoints[2].X - colCC, footingPoints[2].Y + 20 + botBarDia - colLen - footDepth),
                new Vector2(footingPoints[2].X + 400, footingPoints[3].Y + 20 + botBarDia - colLen - footDepth)
            };

            Bars.DrawBars(bar1, dxf);
            Bars.DrawBars(bar2, dxf);

            Leader verticalBarsLeader = new Leader(new List<Vector2>
            {
                new Vector2(bar2[0].X, bar2[0].Y - 120),
                new Vector2(bar2[0].X + 400, bar2[0].Y - 120),

            })
            {
                Layer = new Layer("leaderLayer"),
                Style = new DimensionStyle("LeaderDimStyle"),
                Annotation = new MText(colReinVertical, new Vector2(bar2[0].X + 400, bar2[0].Y - 120), 30)
                {
                    Layer = new Layer("leaderLayer"),
                    AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                    Color = AciColor.Green,
                },

            };

            dxf.Entities.Add(verticalBarsLeader);


        }

    }
}
