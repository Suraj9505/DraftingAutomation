using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Tables;
using netDxf.Entities;

namespace DraftingAutomation.Foundation.FoundationComp
{
    public class Column
    {

        private static Layer columnLayer = new Layer("ColumnLayer")
        {
            Color = new AciColor(8),
        };

        private static Layer leaderLayer = new Layer("leaderLayer")
        {
            Color = AciColor.Red,
        };

        private static DimensionStyle leaderDim = new DimensionStyle("LeaderDimStyle")
        {
            ArrowSize = 30,
            TextColor = AciColor.Red,
        };



        public static void DrawColumn(double colWX, double colLen, Vector2 pos, string stirrups, DxfDocument dxf)
        {
            double stirrupSpacing = Constants.ExtractNumbers(stirrups)[1];

            double stirrupNum;

            stirrupNum = Math.Floor(colLen / stirrupSpacing);

            Rectangle.DrawRectangleWithCenter(pos, colWX, colLen, false, columnLayer, dxf);

            List<Vector2> colVertex = Constants.GetRectanglePointsFromCenter(pos, colWX, colLen);

            Vector2 startPoint = new Vector2(colVertex[0].X + 20, colVertex[0].Y + stirrupSpacing);
            Vector2 endPoint = new Vector2(colVertex[0].X + colWX - 20, colVertex[0].Y + stirrupSpacing);

            for (int i = 0; i < stirrupNum; i++)
            {
                Line line = new Line(startPoint, endPoint)
                {
                    Layer = new Layer("barLayer")
                };

                startPoint.Y += stirrupSpacing;
                endPoint.Y += stirrupSpacing;
                dxf.Entities.Add(line);


                if (i == stirrupNum / 2)
                {

                    List<Vector2> leaderPoints = new List<Vector2>
                    {
                        new Vector2(endPoint.X, endPoint.Y),
                        new Vector2(endPoint.X + 200, endPoint.Y + 150),
                        new Vector2(endPoint.X + 200, endPoint.Y + 150),
                        new Vector2(endPoint.X + 400, endPoint.Y + 150),
                    };

                    Leader stirrupDes = new Leader( leaderPoints)
                    {
                        Layer = leaderLayer,
                        Style = leaderDim,
                        Annotation = new MText(stirrups, new Vector2(endPoint.X + 400, endPoint.Y + 150), 30)
                        {
                            Layer = leaderLayer,
                            AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                            Color = AciColor.Green,
                        },

                    };


                    dxf.Entities.Add(stirrupDes);
                }
            }
        }
    }
}
