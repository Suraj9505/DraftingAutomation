using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;

namespace DraftingAutomation.Extras
{
    public class FootingDimFromGrid
    {

        private static DimensionStyle footDim = new DimensionStyle("FootDim")
        {
            ArrowSize = 100,
            LengthPrecision = 0,
            ExtLineExtend = 20,
            ExtLineOffset = 20,
            TextVerticalPlacement = DimensionStyleTextVerticalPlacement.Below,
            TextOffset = 50,
            TextColor = AciColor.Red,
            TextHeight = 60
        };
        public static void DrawFoundationFootDimension(Vector2 footLoc, double footWX, double footWY, DxfDocument dxf)
        {

            List<double> _gridVertexX = new List<double>(ExcelData._planPointsX);
            List<double> _gridVertexY = new List<double>(ExcelData._planPointsY);

            List<Vector2> horizontalDimVertex = new List<Vector2>();
            List<Vector2> verticalDimVertex = new List<Vector2>();

            List<Vector2> footingPoints = Constants.GetRectanglePointsFromCenter(footLoc, footWX, footWY);
            List<Vector2> footingPointsY = new(footingPoints);


            horizontalDimVertex.Add(new Vector2(footingPoints[0].X, footingPoints[0].Y));
            verticalDimVertex.Add(new Vector2(footingPoints[1].X, footingPoints[1].Y));

            double minDistY = double.PositiveInfinity;
            double remainingX = footWX;
            double remainingY = footWY;
            Vector2 newPoint = footingPoints[0];
            Vector2 newPointY = footingPoints[1];

            for (int i = 0; i < footingPoints.Count; i++)
            {
                double minDistx = double.PositiveInfinity;
                int gridIndex = 0;
                int index = 0;

                double globalDist = double.PositiveInfinity;
                double globalDistFromNext = double.PositiveInfinity;

                if (remainingX == 0) break;

                foreach (double vertex in _gridVertexX)
                {
                    double dist = vertex - newPoint.X;
                    double distFromNext = newPoint.X - footingPoints[1].X;
                    if (Math.Abs(dist) <= Math.Abs(minDistx) && dist != 0 && Math.Abs(dist) < Math.Abs(distFromNext))
                    {
                        minDistx = Math.Abs(dist);
                        globalDist = Math.Abs(dist);
                        gridIndex = _gridVertexX.IndexOf(vertex);
                    }

                    else if (Math.Abs(distFromNext) <= Math.Abs(minDistx) && distFromNext != 0 && Math.Abs(dist) > Math.Abs(distFromNext))
                    {
                        minDistx = Math.Abs(distFromNext);
                        globalDistFromNext = Math.Abs(distFromNext);
                        index = i;

                    }
                }
                if (_gridVertexX.Count != 0 && newPoint.X < _gridVertexX[gridIndex] && Math.Abs(globalDist) < Math.Abs(globalDistFromNext))
                {
                    remainingX -= Math.Abs(minDistx);
                    horizontalDimVertex.Add(new Vector2(_gridVertexX[gridIndex], newPoint.Y));
                    horizontalDimVertex = Constants.SortByX(horizontalDimVertex);

                    newPoint = horizontalDimVertex[horizontalDimVertex.Count - 1];
                    _gridVertexX.Remove(_gridVertexX[gridIndex]);
                }

                else if (newPoint.X < footingPoints[index].X && Math.Abs(globalDist) > Math.Abs(globalDistFromNext))
                {
                    remainingX -= Math.Abs(minDistx);
                    horizontalDimVertex.Add(new Vector2(footingPoints[index].X, newPoint.Y));

                    horizontalDimVertex = Constants.SortByX(horizontalDimVertex);

                    newPoint = horizontalDimVertex[horizontalDimVertex.Count - 1];
                    footingPoints.Remove(footingPoints[i]);
                }

                else if (_gridVertexX.Count != 0 && newPoint.X >= _gridVertexX[gridIndex])
                {
                    horizontalDimVertex.Add(new Vector2(_gridVertexX[gridIndex], newPoint.Y));
                    horizontalDimVertex = Constants.SortByX(horizontalDimVertex);

                    newPoint = horizontalDimVertex[horizontalDimVertex.Count - 1];
                    _gridVertexX.Remove(_gridVertexX[gridIndex]);
                }

                else if (newPoint.X >= footingPoints[index].X)
                {
                    horizontalDimVertex.Add(new Vector2(footingPoints[index].X, newPoint.Y));
                    horizontalDimVertex = Constants.SortByX(horizontalDimVertex);

                    newPoint = horizontalDimVertex[horizontalDimVertex.Count - 1];
                    footingPoints.Remove(footingPoints[i]);
                }


            }

            for (int i = 0; i < footingPointsY.Count; i++)
            {
                int gridIndex = 0;
                int index = 0;

                double globalDist = double.PositiveInfinity;
                double globalDistFromNext = double.PositiveInfinity;

                if (remainingY == 0) break;

                foreach (double vertex in _gridVertexY)
                {
                    double dist = vertex - newPointY.Y;
                    double distFromNext = newPointY.Y - footingPointsY[2].Y; // Adjust this line to look at the last Y point
                    if (Math.Abs(dist) <= Math.Abs(minDistY) && dist != 0 && Math.Abs(dist) < Math.Abs(distFromNext))
                    {
                        minDistY = Math.Abs(dist);
                        globalDist = Math.Abs(dist);
                        gridIndex = _gridVertexY.IndexOf(vertex);
                    }
                    else if (Math.Abs(distFromNext) <= Math.Abs(minDistY) && distFromNext != 0 && Math.Abs(dist) > Math.Abs(distFromNext))
                    {
                        minDistY = Math.Abs(distFromNext);
                        globalDistFromNext = Math.Abs(distFromNext);
                        index = i;
                    }
                }

                if (_gridVertexY.Count != 0 && newPointY.Y < _gridVertexY[gridIndex] && Math.Abs(globalDist) < Math.Abs(globalDistFromNext))
                {
                    remainingY -= Math.Abs(minDistY);
                    verticalDimVertex.Add(new Vector2(newPointY.X, _gridVertexY[gridIndex]));
                    verticalDimVertex = Constants.SortByY(verticalDimVertex);

                    newPointY = verticalDimVertex[verticalDimVertex.Count - 1];
                    _gridVertexY.Remove(_gridVertexY[gridIndex]);
                }
                else if (newPoint.Y < footingPointsY[index].Y && Math.Abs(globalDist) > Math.Abs(globalDistFromNext))
                {
                    remainingY -= Math.Abs(minDistY);
                    verticalDimVertex.Add(new Vector2(newPointY.X, footingPointsY[index].Y));

                    verticalDimVertex = Constants.SortByY(verticalDimVertex);

                    newPointY = verticalDimVertex[verticalDimVertex.Count - 1];
                    footingPointsY.Remove(footingPointsY[i]);
                }
                else if (_gridVertexY.Count != 0 && newPoint.Y >= _gridVertexY[gridIndex])
                {
                    verticalDimVertex.Add(new Vector2(newPointY.X, _gridVertexY[gridIndex]));
                    verticalDimVertex = Constants.SortByY(verticalDimVertex);

                    newPointY = verticalDimVertex[verticalDimVertex.Count - 1];
                    _gridVertexY.Remove(_gridVertexY[gridIndex]);
                }
                else if (newPointY.Y >= footingPointsY[index].Y)
                {
                    verticalDimVertex.Add(new Vector2(newPointY.X, footingPointsY[index].Y));
                    verticalDimVertex = Constants.SortByY(verticalDimVertex);

                    newPointY = verticalDimVertex[verticalDimVertex.Count - 1];
                    footingPointsY.Remove(footingPointsY[i]);
                }
            }

            for (int i = 0; i <= horizontalDimVertex.Count - 2; i++)
            {
                AlignedDimension dim = new()
                {
                    FirstReferencePoint = new Vector2(horizontalDimVertex[i].X, horizontalDimVertex[i].Y),
                    SecondReferencePoint = new Vector2(horizontalDimVertex[i + 1].X, horizontalDimVertex[i].Y),
                    Style = footDim,
                    Layer = new Layer("GridDimLayer")
                };

                dim.SetDimensionLinePosition(new Vector2(horizontalDimVertex[i].X, horizontalDimVertex[i].Y - 300)); // this sets the postion of the dimension

                dxf.Entities.Add(dim);
            }

            for (int i = 0; i <= verticalDimVertex.Count - 2; i++)
            {
                AlignedDimension dim = new()
                {
                    FirstReferencePoint = new Vector2(verticalDimVertex[i].X, verticalDimVertex[i].Y),
                    SecondReferencePoint = new Vector2(verticalDimVertex[i + 1].X, verticalDimVertex[i + 1].Y),
                    Style = footDim,
                    Layer = new Layer("GridDimLayer")
                };

                dim.SetDimensionLinePosition(new Vector2(verticalDimVertex[i].X + 300, verticalDimVertex[i].Y)); // this sets the postion of the dimension
             

                dxf.Entities.Add(dim);
            }



        }
    }
}
