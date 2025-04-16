using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DraftingAutomation.Extras;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;

namespace DraftingAutomation.Foundation.FoundationComp
{
    public class Footing
    {
        private static Layer footingLayer = new Layer("FootingLayer1")
        {
            Color = AciColor.Green,
        };

        private static Layer layer = new Layer("verticalBarsLayer")
        {
            Color = new AciColor(142),
        };

        private static double scaleFactor = 2;

        public static void DrawFooting(double footWX, double footDepth, double footCC, Vector2 pos, string footBarBtmX, string footBarBtmY, string footBarTopX, string footBarTopY, DxfDocument dxf)
        {
            Rectangle.DrawRectangleWithCenter(pos, footWX, footDepth, false, footingLayer, dxf);

            List<Vector2> footPoints = new(Constants.GetRectanglePointsFromCenter(pos, footWX, footDepth));

            double botBarRadiusX = Constants.ExtractNumbers(footBarBtmX)[0] / 2;
            double topBarRadiusX = Constants.ExtractNumbers(footBarTopX)[0] / 2;

            // Bars
            double BotBarSpacing = Constants.ExtractNumbers(footBarBtmX)[1];
            double TopBarSpacing = Constants.ExtractNumbers(footBarTopX)[1];
            double topBotSpace = 20;

            double BotBarNum = Math.Round((footWX - 2 * footCC - 2 * botBarRadiusX) / BotBarSpacing);
            double TopBarNum = Math.Round((footWX - 2 * footCC - 2 * topBarRadiusX) / TopBarSpacing);

            double newBotBarSpacing = (footWX - 2 * footCC - 2 * botBarRadiusX) / BotBarNum;
            double newTopBarSpacing = (footWX - 2 * footCC - 2 * topBarRadiusX) / TopBarNum;


            Vector2 BotBarStart = new Vector2(footPoints[0].X + footCC + botBarRadiusX , footPoints[0].Y + topBotSpace + botBarRadiusX);

            List<Vector2> botBarLinePoints = new List<Vector2>();

            botBarLinePoints.Add(new Vector2(BotBarStart.X, BotBarStart.Y - (botBarRadiusX * scaleFactor)));
            botBarLinePoints.Add(new Vector2(BotBarStart.X - (botBarRadiusX * scaleFactor), BotBarStart.Y));

            Vector2 TopBarStart = new Vector2(footPoints[3].X + footCC + topBarRadiusX, footPoints[3].Y - topBotSpace - botBarRadiusX);

            List<Vector2> topBarLinePoints = new List<Vector2>();
            double extraspace = 30;

            topBarLinePoints.Add(new Vector2(TopBarStart.X - extraspace, TopBarStart.Y + (topBarRadiusX * scaleFactor)));
            topBarLinePoints.Add(new Vector2(TopBarStart.X - (topBarRadiusX * scaleFactor) - extraspace, TopBarStart.Y));

            for (int i = 0; i < BotBarNum + 1; i++)
            {
                Circle circle = new Circle(BotBarStart, botBarRadiusX * scaleFactor)
                {
                    Layer = new Layer("barLayer")
                };

                HatchBoundaryPath hatchBoundry = new HatchBoundaryPath(new List<EntityObject> { circle });
                Hatch hatch = new Hatch(HatchPattern.Solid, new List<HatchBoundaryPath> { hatchBoundry }, true)
                {
                    Layer = new Layer("barLayer")
                };

                if (i == BotBarNum - 3)
                {
                    if(footBarBtmX == footBarBtmY)
                    {
                        List<Vector2> leaderPoints1 = new List<Vector2>();
                        leaderPoints1.Add(new Vector2(BotBarStart.X + 100, BotBarStart.Y - botBarRadiusX * scaleFactor));
                        leaderPoints1.Add(new Vector2(BotBarStart.X + 100, BotBarStart.Y - 200));
                        leaderPoints1.Add(new Vector2(BotBarStart.X + 150, BotBarStart.Y - 200));

                        List<Vector2> leaderPoints2 = new List<Vector2>();
                        leaderPoints2.Add(BotBarStart);
                        leaderPoints2.Add(new Vector2(BotBarStart.X + 100, BotBarStart.Y - 200));

                        AngularLeader.SameXAndYRein(leaderPoints1, leaderPoints2, footBarBtmX, dxf);
                    }

                    else
                    {
                        List<Vector2> leaderPoints1 = new List<Vector2>();
                        leaderPoints1.Add(new Vector2(BotBarStart.X + 100, BotBarStart.Y - botBarRadiusX * scaleFactor));
                        leaderPoints1.Add(new Vector2(BotBarStart.X + 100, BotBarStart.Y - 200));
                        leaderPoints1.Add(new Vector2(BotBarStart.X + 150, BotBarStart.Y - 200));

                        List<Vector2> leaderPoints2 = new List<Vector2>();
                        leaderPoints2.Add(BotBarStart);
                        leaderPoints2.Add(new Vector2(BotBarStart.X, BotBarStart.Y - 400));
                        leaderPoints2.Add(new Vector2(BotBarStart.X + 20, BotBarStart.Y - 400));

                        AngularLeader.DifferentXAndYRein(leaderPoints1, leaderPoints2, footBarBtmX, footBarBtmY, dxf);
                    }
                }
                BotBarStart.X += newBotBarSpacing;

                dxf.Entities.Add(hatch);
            }
            botBarLinePoints.Add(new Vector2(BotBarStart.X - newBotBarSpacing, BotBarStart.Y - (botBarRadiusX * scaleFactor)));
            botBarLinePoints.Add(new Vector2(BotBarStart.X - newBotBarSpacing + (botBarRadiusX * scaleFactor) , BotBarStart.Y));

            for (int i = 0; i < TopBarNum + 1; i++)
            {
                Circle circle = new Circle(TopBarStart, topBarRadiusX * scaleFactor)
                {
                    Layer = new Layer("barLayer")
                };

                HatchBoundaryPath hatchBoundaryPath = new HatchBoundaryPath(new List<EntityObject> { circle });
                Hatch hatch = new Hatch(HatchPattern.Solid, new List<HatchBoundaryPath> { hatchBoundaryPath }, true)
                {
                    Layer = new Layer("barLayer")
                };

                if (i == BotBarNum - 3)
                {
                    if(footBarTopX == footBarTopY)
                    {
                        List<Vector2> leaderPoints1 = new List<Vector2>();
                        leaderPoints1.Add(new Vector2(TopBarStart.X + 100, TopBarStart.Y + botBarRadiusX * scaleFactor));
                        leaderPoints1.Add(new Vector2(TopBarStart.X + 100, TopBarStart.Y + 200));
                        leaderPoints1.Add(new Vector2(TopBarStart.X + 150, TopBarStart.Y + 200));

                        List<Vector2> leaderPoints2 = new List<Vector2>();
                        leaderPoints2.Add(TopBarStart);
                        leaderPoints2.Add(new Vector2(TopBarStart.X + 100, TopBarStart.Y + 200));

                        AngularLeader.SameXAndYRein(leaderPoints1 , leaderPoints2, footBarTopX, dxf);
                    }
                    else
                    {
                        List<Vector2> leaderPoints1 = new List<Vector2>();
                        leaderPoints1.Add(new Vector2(TopBarStart.X + 100, TopBarStart.Y + botBarRadiusX * scaleFactor));
                        leaderPoints1.Add(new Vector2(TopBarStart.X + 100, TopBarStart.Y + 200));
                        leaderPoints1.Add(new Vector2(TopBarStart.X + 150, TopBarStart.Y + 200));

                        List<Vector2> leaderPoints2 = new List<Vector2>();
                        leaderPoints2.Add(TopBarStart);
                        leaderPoints2.Add(new Vector2(TopBarStart.X, TopBarStart.Y + 400));
                        leaderPoints2.Add(new Vector2(TopBarStart.X + 20, TopBarStart.Y + 400));

                        AngularLeader.DifferentXAndYRein(leaderPoints1, leaderPoints2, footBarTopX, footBarTopY, dxf);
                    }

                }

                TopBarStart.X += newTopBarSpacing;
                //dxf.Entities.Add(circle);
                dxf.Entities.Add(hatch);
            }
            topBarLinePoints.Add(new Vector2(TopBarStart.X - newTopBarSpacing + extraspace, TopBarStart.Y + (topBarRadiusX * scaleFactor)));
            topBarLinePoints.Add(new Vector2(TopBarStart.X - newTopBarSpacing + (topBarRadiusX* scaleFactor) + extraspace, TopBarStart.Y));


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

            Arc arc1 = new Arc(new Vector2(botBarLinePoints[0].X, botBarLinePoints[1].Y), botBarRadiusX * scaleFactor, 180, 270)
            {
                Layer = layer,
            };

            Arc arc2 = new Arc(new Vector2(botBarLinePoints[2].X, botBarLinePoints[1].Y), botBarRadiusX * scaleFactor, 270, 0)
            {
                Layer = layer,
            };

            Arc arc3 = new Arc(new Vector2(topBarLinePoints[0].X, topBarLinePoints[1].Y), topBarRadiusX * scaleFactor, 90, 180)
            {
                Layer = layer
            };

            Arc arc4 = new Arc(new Vector2(topBarLinePoints[2].X, topBarLinePoints[1].Y), topBarRadiusX * scaleFactor, 0, 90)
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
