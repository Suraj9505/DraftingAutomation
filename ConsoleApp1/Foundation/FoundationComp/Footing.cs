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
    public class Footing
    {
        private static Layer footingLayer = new Layer("FootingLayer")
        {
            Color = AciColor.Green,
        };

        private static Layer layer = new Layer("verticalBarsLayer")
        {
            Color = AciColor.Cyan,
        };


        public static void DrawFooting(double footWX, double footDepth, double footCC, Vector2 pos, string footBarBtmX, string footBarTopX, DxfDocument dxf)
        {
            Rectangle.DrawRectangleWithCenter(pos, footWX, footDepth, false, footingLayer, dxf);

            List<Vector2> footPoints = new(Constants.GetRectanglePointsFromCenter(pos, footWX, footDepth));

            double botBarRadiusX = Constants.ExtractNumbers(footBarBtmX)[0] / 2;
            double topBarRadiusX = Constants.ExtractNumbers(footBarTopX)[0] / 2;

            // Bars
            double BotBarSpacing = Constants.ExtractNumbers(footBarBtmX)[1];
            double TopBarSpacing = Constants.ExtractNumbers(footBarTopX)[1];
            double topBotSpace = 20;

            double BotBarNum = Math.Floor((footWX - footCC) / BotBarSpacing);
            double TopBarNum = Math.Floor((footWX - footCC) / TopBarSpacing);

            Vector2 BotBarStart = new Vector2(footPoints[0].X + footCC + botBarRadiusX + BotBarSpacing / 2, footPoints[0].Y + topBotSpace + botBarRadiusX);

            List<Vector2> botBarLinePoints = new List<Vector2>();

            botBarLinePoints.Add(new Vector2(BotBarStart.X, BotBarStart.Y - botBarRadiusX));
            botBarLinePoints.Add(new Vector2(BotBarStart.X - botBarRadiusX, BotBarStart.Y));

            Vector2 TopBarStart = new Vector2(footPoints[3].X + footCC + topBarRadiusX + TopBarSpacing / 2, footPoints[3].Y - topBotSpace - botBarRadiusX);

            List<Vector2> topBarLinePoints = new List<Vector2>();
            double extraspace = 30;

            topBarLinePoints.Add(new Vector2(TopBarStart.X - extraspace, TopBarStart.Y + topBarRadiusX));
            topBarLinePoints.Add(new Vector2(TopBarStart.X - topBarRadiusX - extraspace, TopBarStart.Y));

            for (int i = 0; i < BotBarNum; i++)
            {
                Circle circle = new Circle(BotBarStart, botBarRadiusX)
                {
                    Layer = new Layer("barLayer")
                };
                BotBarStart.X += BotBarSpacing;

                HatchBoundaryPath hatchBoundry = new HatchBoundaryPath(new List<EntityObject> { circle });
                Hatch hatch = new Hatch(HatchPattern.Solid, new List<HatchBoundaryPath> { hatchBoundry }, true)
                {
                    Layer = new Layer("barLayer")
                };

                dxf.Entities.Add(hatch);
            }
            botBarLinePoints.Add(new Vector2(BotBarStart.X - 150, BotBarStart.Y - botBarRadiusX));
            botBarLinePoints.Add(new Vector2(BotBarStart.X - 150 + botBarRadiusX, BotBarStart.Y));

            for (int i = 0; i < TopBarNum; i++)
            {
                Circle circle = new Circle(TopBarStart, topBarRadiusX)
                {
                    Layer = new Layer("barLayer")
                };
                TopBarStart.X += TopBarSpacing;

                HatchBoundaryPath hatchBoundaryPath = new HatchBoundaryPath(new List<EntityObject> { circle });
                Hatch hatch = new Hatch(HatchPattern.Solid, new List<HatchBoundaryPath> { hatchBoundaryPath }, true)
                {
                    Layer = new Layer("barLayer")
                };

                //dxf.Entities.Add(circle);
                dxf.Entities.Add(hatch);
            }
            topBarLinePoints.Add(new Vector2(TopBarStart.X - 150 + extraspace, TopBarStart.Y + topBarRadiusX));
            topBarLinePoints.Add(new Vector2(TopBarStart.X - 150 + topBarRadiusX + extraspace, TopBarStart.Y));


            //Lines to show bars in Y axis
            Line botLine1 = new Line(botBarLinePoints[0], botBarLinePoints[2])
            {
                Layer = layer
            };

            Line botLine2 = new Line(botBarLinePoints[1], new Vector2(botBarLinePoints[1].X, botBarLinePoints[1].Y + 200))
            {
                Layer = layer
            };

            Line botLine3 = new Line(botBarLinePoints[3], new Vector2(botBarLinePoints[3].X, botBarLinePoints[1].Y + 200))
            {
                Layer = layer
            };

            Line topLine1 = new Line(topBarLinePoints[0], topBarLinePoints[2])
            {
                Layer = layer
            };

            Line topLine2 = new Line(topBarLinePoints[1], new Vector2(topBarLinePoints[1].X, topBarLinePoints[1].Y - 200))
            {
                Layer = layer
            };

            Line topLine3 = new Line(topBarLinePoints[3], new Vector2(topBarLinePoints[3].X, topBarLinePoints[1].Y - 200))
            {
                Layer = layer
            };

            //Arcs

            Arc arc1 = new Arc(new Vector2(botBarLinePoints[0].X, botBarLinePoints[1].Y), botBarRadiusX, 180, 270)
            {
                Layer = layer,
            };
            
            Arc arc2 = new Arc(new Vector2(botBarLinePoints[2].X, botBarLinePoints[1].Y), botBarRadiusX, 270, 0)
            {
                Layer = layer,
            };

            Arc arc3 = new Arc(new Vector2(topBarLinePoints[0].X, topBarLinePoints[1].Y), topBarRadiusX, 90, 180)
            {
                Layer = layer
            };

            Arc arc4 = new Arc(new Vector2(topBarLinePoints[2].X, topBarLinePoints[1].Y), topBarRadiusX, 0, 90)
            {
                Layer = layer
            };

            dxf.Entities.Add(botLine1);
            dxf.Entities.Add(botLine2);
            dxf.Entities.Add(botLine3);

            dxf.Entities.Add(topLine1);
            dxf.Entities.Add(topLine2);
            dxf.Entities.Add(topLine3);


            dxf.Entities.Add(arc1);
            dxf.Entities.Add(arc2);
            dxf.Entities.Add(arc3);
            dxf.Entities.Add(arc4);
        }
    }
}
