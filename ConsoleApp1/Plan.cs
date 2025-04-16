using System;
using System.Collections.Generic;
using DraftingAutomation.Extras;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using static DraftingAutomation.Constants;

namespace DraftingAutomation
{
    public class Plan
    {
        private const double VerticalExtension = 6000;
        private const double HorizontalStartExtension = 6000;
        private const double HorizontalEndExtension = 3000;
        private const int lineTypeScale = 200;
        private const double DimensionOffset = 3000; // Adjust as needed for dimension line spacing
        private const double adjustmentVal = 600;

        private static Layer pccLayer = new Layer("PccLayer")
        {
            Color = AciColor.DarkGray,
        };

        private static Layer footLayer = new Layer("FootingLayer")
        {
            Color = AciColor.Cyan,
        };

        private static Layer colLayer = new Layer("ColumnLayer")
        {
            Color = new AciColor(30),
        };

        private static Layer textLayer = new Layer("TextLayer")
        {
            Color = AciColor.Red,
        };

        public static void DrawGrid(List<double> planPointsX, List<double> planPointsY, DxfDocument dxf)
        {
            Layer gridLayer = new Layer("GridLayer")
            {
                Color = AciColor.LightGray,
            };

            Layer gridDimLayer = new Layer("GridDimLayer")
            {
                Color = AciColor.Yellow,
            };

            // Draw vertical lines
            for (int i = 0; i < planPointsX.Count; i++)
            {
                if (planPointsY.Count > 0)
                {
                    Vector2 startPoint = new Vector2(planPointsX[i], planPointsY[0] - VerticalExtension);
                    Vector2 endPoint = new Vector2(planPointsX[i], planPointsY[planPointsY.Count - 1] + VerticalExtension);
                    Line fullVertical = new Line(startPoint, endPoint);
                    fullVertical.Layer = gridLayer;
                    fullVertical.Linetype = Linetype.Center;
                    fullVertical.LinetypeScale = lineTypeScale;
                    dxf.Entities.Add(fullVertical);

                    Circle circle = new Circle(new Vector2(planPointsX[i], planPointsY[0] - VerticalExtension - adjustmentVal), adjustmentVal) { Layer = textLayer };

                    Text mText = new Text(GetColumnLabel(i), new Vector2(planPointsX[i] - 200, planPointsY[0] - VerticalExtension - 900), adjustmentVal)
                    {
                        Layer = textLayer,
                        Alignment = TextAlignment.MiddleCenter,
                        Color = new AciColor(255),
                    };

                    dxf.Entities.Add(mText);
                    dxf.Entities.Add(circle);

                    // Add dimensions for vertical grid segments (for the first vertical line only)
                    if (i == 0 && planPointsY.Count > 1)
                    {
                        for (int j = 0; j < planPointsY.Count - 1; j++)
                        {
                            AlignedDimension dim = new AlignedDimension
                            {
                                FirstReferencePoint = new Vector2(planPointsX[i], planPointsY[j]),
                                SecondReferencePoint = new Vector2(planPointsX[i], planPointsY[j + 1]),
                                Layer = gridDimLayer,
                                Style = Constants.gridDim,
                            };
                            dim.SetDimensionLinePosition(new Vector2(planPointsX[i] - DimensionOffset, planPointsY[j]));
                            dxf.Entities.Add(dim);
                        }


                    }
                }
            }

            // Draw horizontal lines
            for (int j = 0; j < planPointsY.Count; j++)
            {
                if (planPointsX.Count > 0)
                {
                    Vector2 startPoint = new Vector2(planPointsX[0] - HorizontalStartExtension, planPointsY[j]);
                    Vector2 endPoint = new Vector2(planPointsX[planPointsX.Count - 1] + HorizontalEndExtension, planPointsY[j]);
                    Line fullHorizontal = new Line(startPoint, endPoint);
                    fullHorizontal.Layer = gridLayer;
                    fullHorizontal.Linetype = Linetype.Center;
                    fullHorizontal.LinetypeScale = lineTypeScale;
                    dxf.Entities.Add(fullHorizontal);

                    Circle circle = new Circle(new Vector2(planPointsX[0] - HorizontalStartExtension - adjustmentVal, planPointsY[j]), adjustmentVal) { Layer = textLayer };

                    Text mText = new Text((j + 1).ToString(), new Vector2(planPointsX[0] - HorizontalStartExtension - adjustmentVal - 200, planPointsY[j] - 300), adjustmentVal)
                    {
                        Layer = textLayer,
                        Alignment = TextAlignment.MiddleCenter,
                        Color = new AciColor(255),
                    };

                    dxf.Entities.Add(mText);
                    dxf.Entities.Add(circle);

                    if (j == planPointsY.Count - 1 && planPointsX.Count > 1)
                    {
                        for (int i = 0; i < planPointsX.Count - 1; i++)
                        {
                            AlignedDimension dim = new AlignedDimension
                            {
                                FirstReferencePoint = new Vector2(planPointsX[i], planPointsY[j]),
                                SecondReferencePoint = new Vector2(planPointsX[i + 1], planPointsY[j]),
                                Layer = gridDimLayer,
                                Style = new DimensionStyle("GridDim"),
                            };
                            dim.SetDimensionLinePosition(new Vector2(planPointsX[i], planPointsY[j] + DimensionOffset));
                            dxf.Entities.Add(dim);
                        }
                    }

                }
            }
        }


        public static void DrawFoundationLayot(double pccWX, double pccWY, Vector2 footLoc, double footWX, double footWY, Vector2 colLoc, double colWX, double colWY, DxfDocument dxf)
        {

            Rectangle.DrawRectangleWithCenter(footLoc, pccWX, pccWY, false, pccLayer, dxf);
            Rectangle.DrawRectangleWithCenter(footLoc, footWX, footWY, false, footLayer, dxf);
            Rectangle.DrawRectangleWithCenter(colLoc, colWX, colWY, false, colLayer, dxf);

            List<Vector2> boundary =
            [
                new(colLoc.X - colWX/2, colLoc.Y - colWY / 2),
                new(colLoc.X + colWX /2, colLoc.Y -colWY / 2),
                new(colLoc.X + colWX/2, colLoc.Y + colWY/2),
                new(colLoc.X - colWX /2, colLoc.Y + colWY / 2)
            ];

            List<Line> lines =
            [
                new(boundary[0], boundary[1]) { Layer = colLayer },
                new(boundary[1], boundary[2]) { Layer = colLayer },
                new(boundary[2], boundary[3]) { Layer = colLayer },
                new(boundary[3], boundary[0]) { Layer = colLayer }
            ];


            HatchBoundaryPath hatchBoundaryPath = new HatchBoundaryPath(lines);


            Hatch hatch = new Hatch(HatchPattern.Line, new List<HatchBoundaryPath> { hatchBoundaryPath }, true);

            hatch.Pattern.Scale = 30;
            hatch.Pattern.Angle = 45;
            hatch.Layer = Constants.hatchLayer;

            dxf.Entities.Add(hatch);


            FootingDimFromGrid.DrawFoundationFootDimension(footLoc, footWX, footWY, dxf);

        }

        private static string GetColumnLabel(int gridIndex)

        {

            const int alphabetSize = 26;
            string label = "";
            if (gridIndex >= 0)

            {

                while (gridIndex >= 0)

                {

                    int remainder = gridIndex % alphabetSize;

                    label = (char)('A' + remainder) + label;

                    gridIndex = (gridIndex / alphabetSize) - 1;

                }

            }

            return label;

        }
    }
}