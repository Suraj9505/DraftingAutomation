using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DraftingAutomation.Foundation.FoundationComp;
using netDxf;
using netDxf.Tables;

namespace DraftingAutomation.Foundation
{
    public class Foundation
    {
        public static void DrawFoundation(Vector2 pos, double rubbleThk, double pccWX, double pccDepth, double footWX, double footDepth, double colWX, double colLen, double footCC, double colCC, List<double> FRBotX, List<double> FRTopX, double CRSpacing, DxfDocument dxf)
        {
            PCC.DrawPCC(pccWX, pccDepth, pos, dxf);

            Vector2 footingCenter = new();
            Vector2 columnCenter = new();

            footingCenter.Y = pos.Y + pccDepth / 2 + footDepth /2;
            footingCenter.X = pos.X;

            columnCenter.X = pos.X;
            columnCenter.Y = footingCenter.Y + footDepth / 2 + colLen / 2;

            Footing.DrawFooting(footWX, footDepth, footingCenter, dxf);

            Column.DrawColumn(colWX, colLen, columnCenter, dxf);


        }
        
    }
}
