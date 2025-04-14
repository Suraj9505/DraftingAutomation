using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf.Entities;
using netDxf.Tables;
using netDxf;
using DraftingAutomation.Foundation.FoundationComp;

namespace DraftingAutomation.Extras
{
    public class AngularLeader
    {
        private static DimensionStyle leaderDim1 = new DimensionStyle("leaderDim1")
        {
            TextColor = AciColor.Red,
            LeaderArrow = DimensionArrowhead.OriginIndicator,
            ArrowSize = 30,
        };

        public static void SameXAndYRein(List<Vector2> leaderLines1, List<Vector2> leaderLines2, string desc, DxfDocument dxf)
        {
            Leader leader = new Leader(leaderLines2)
            {
                Layer = Column.leaderLayer,
                Style = leaderDim1,
            };

            Leader leader1 = new Leader(leaderLines1)
            {
                Layer = Column.leaderLayer,
                Style = Column.leaderDim,
                Annotation = new MText(desc, new Vector2(leaderLines1[2].X, leaderLines1[2].Y), 30)
                {
                    Layer = Column.leaderLayer,
                    AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                    Color = AciColor.Green,
                },
            };


            dxf.Entities.Add(leader);
            dxf.Entities.Add(leader1);
        }

        public static void DifferentXAndYRein(List<Vector2> leaderLines1, List<Vector2> leaderLines2, string descX, string descY, DxfDocument dxf)
        {
            Leader leader = new Leader(leaderLines1)
            {
                Layer = Column.leaderLayer,
                Style = Column.leaderDim,
                Annotation = new MText(descX, leaderLines1[2], 30)
                {
                    Layer = Column.leaderLayer,
                    AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                    Color = AciColor.Green,
                },
            };

            Leader leader1 = new Leader(leaderLines2)
            {
                Layer = Column.leaderLayer,
                Style = leaderDim1,
                Annotation = new MText(descY, leaderLines2[2], 30)
                {
                    Layer = Column.leaderLayer,
                    AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                    Color = AciColor.Green,
                },
            };


            dxf.Entities.Add(leader);
            dxf.Entities.Add(leader1);
        }
    }
}
