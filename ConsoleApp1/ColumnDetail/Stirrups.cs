using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DraftingAutomation.Extras;
using DraftingAutomation.Foundation;
using DraftingAutomation.Foundation.FoundationComp;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;

namespace DraftingAutomation.ColumnDetail
{
    public class Stirrups
    {
        private static void DrawStirrupLine(DxfDocument dxf, Layer layer, Vector2 startPoint, Vector2 endPoint)
        {
            dxf.Entities.Add(new Line(startPoint, endPoint) { Layer = layer });
        }

        private static void DrawLeaderWithText(DxfDocument dxf, Layer layer, List<Vector2> points, string text, DimensionStyle style)
        {
            if (points != null && points.Count >= 2)
            {
                Leader leader = new Leader(points)
                {
                    Layer = layer,
                    Style = style,
                    Annotation = new MText(text, points[1], 30)
                    {
                        Layer = Column.leaderLayer,
                        AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                        Color = AciColor.Green,
                    }
                };
                dxf.Entities.Add(leader);
            }
        }

        public static void DrawInnerStirrups(List<Vector2> points, int number, double radius, Layer layer, DxfDocument dxf)
        {
            int leaderLength = 400;

            if (number % 2 != 0 && number > 0)
            {
                int firstIndex = number / 2;
                int secondIndex = number + firstIndex;

                if (points[firstIndex].X == points[secondIndex].X)
                {
                    DrawStirrupLine(dxf, layer, new Vector2(points[firstIndex].X - radius, points[firstIndex].Y), new Vector2((points[secondIndex].X - radius), points[secondIndex].Y));
                    DrawStirrupLine(dxf, layer, new Vector2(points[firstIndex].X + radius, points[firstIndex].Y), new Vector2(points[firstIndex].X + radius, points[firstIndex].Y + 30));
                    DrawStirrupLine(dxf, layer, new Vector2(points[secondIndex].X + radius, points[secondIndex].Y), new Vector2(points[secondIndex].X + radius, points[secondIndex].Y - 30));

                    int l = 0, r = number - 1;
                    while (l < r && number > 3)
                    {
                        DrawStirrupLine(dxf, layer, new Vector2(points[l].X - radius, points[l].Y), new Vector2(points[number + l].X - radius, points[number + l].Y));
                        DrawStirrupLine(dxf, layer, new Vector2(points[l + 1].X + radius, points[l + 1].Y), new Vector2(points[l + 1 + number].X + radius, points[l + 1 + number].Y));
                        DrawStirrupLine(dxf, layer, new Vector2(points[r].X + radius, points[r].Y), new Vector2(points[number + r].X + radius, points[number + r].Y));
                        DrawStirrupLine(dxf, layer, new Vector2(points[r - 1].X - radius, points[r - 1].Y), new Vector2(points[r - 1 + number].X - radius, points[r - 1 + number].Y));
                        l += 2;
                        r -= 2;
                        number -= 4;
                    }

                    if (number == 3)
                    {
                        DrawStirrupLine(dxf, layer, new Vector2(points[l].X - radius, points[l].Y), new Vector2(points[l + number].X - radius, points[l + number].Y));
                        DrawStirrupLine(dxf, layer, new Vector2(points[r].X + radius, points[r].Y), new Vector2(points[r + number].X + radius, points[r + number].Y));
                    }
                }
                else if (number > 1)
                {
                    DrawStirrupLine(dxf, layer, new Vector2(points[firstIndex].X, points[firstIndex].Y - radius), new Vector2(points[secondIndex].X, points[secondIndex].Y - radius));
                    DrawStirrupLine(dxf, layer, new Vector2(points[firstIndex].X, points[firstIndex].Y + radius), new Vector2(points[firstIndex].X + 30, points[firstIndex].Y + radius));
                    DrawStirrupLine(dxf, layer, new Vector2(points[secondIndex].X, points[secondIndex].Y + radius), new Vector2(points[secondIndex].X - 30, points[secondIndex].Y + radius));

                    int l = 0, r = number - 1;
                    while (l < r && number > 3)
                    {
                        DrawStirrupLine(dxf, layer, new Vector2(points[l].X, points[l].Y - radius), new Vector2(points[number + l].X, points[number + l].Y - radius));
                        DrawStirrupLine(dxf, layer, new Vector2(points[l + 1].X, points[l + 1].Y + radius), new Vector2(points[l + 1 + number].X, points[l + 1 + number].Y + radius));
                        DrawStirrupLine(dxf, layer, new Vector2(points[r].X, points[r].Y + radius), new Vector2(points[number + r].X, points[number + r].Y + radius));
                        DrawStirrupLine(dxf, layer, new Vector2(points[r - 1].X, points[r - 1].Y - radius), new Vector2(points[r - 1 + number].X, points[r - 1 + number].Y - radius));
                        l += 2;
                        r -= 2;
                        number -= 4;
                    }

                    if (number == 3)
                    {
                        DrawStirrupLine(dxf, layer, new Vector2(points[l].X, points[l].Y - radius), new Vector2(points[l + number].X, points[l + number].Y - radius));
                        DrawStirrupLine(dxf, layer, new Vector2(points[r].X, points[r].Y + radius), new Vector2(points[r + number].X, points[r + number].Y + radius));
                    }

                    List<Vector2> leaderPoints1 = new List<Vector2> { points[firstIndex], new Vector2(points[firstIndex].X - leaderLength, points[firstIndex].Y) };
                    DrawLeaderWithText(dxf, layer, leaderPoints1, ExcelData.colRein, AngularLeader.leaderDim1);

                    double dist = Math.Abs(points[firstIndex + 2].Y - points[firstIndex].Y);
                    List<Vector2> leaderPoints2 = new List<Vector2> { new Vector2(points[firstIndex + 1].X - radius, points[firstIndex + 1].Y + dist / 2), new Vector2(points[firstIndex + 1].X - radius - leaderLength, points[firstIndex + 1].Y + dist / 2) };
                    DrawLeaderWithText(dxf, layer, leaderPoints2, ExcelData.colStirrups, Constants.leaderDim);
                }

                else
                {
                    DrawStirrupLine(dxf, layer, new Vector2(points[firstIndex].X, points[firstIndex].Y - radius), new Vector2(points[secondIndex].X, points[secondIndex].Y - radius));
                    DrawStirrupLine(dxf, layer, new Vector2(points[firstIndex].X, points[firstIndex].Y + radius), new Vector2(points[firstIndex].X + 30, points[firstIndex].Y + radius));
                    DrawStirrupLine(dxf, layer, new Vector2(points[secondIndex].X, points[secondIndex].Y + radius), new Vector2(points[secondIndex].X - 30, points[secondIndex].Y + radius));
                }
            }
            else
            {
                int l = 0, r = number - 1;

                if (number > 2)
                {
                    while (l < r)
                    {
                        if (points[l].Y == points[r].Y)
                        {
                            DrawStirrupLine(dxf, layer, new Vector2(points[l].X - radius, points[l].Y), new Vector2(points[l + number].X - radius, points[l + number].Y));
                            DrawStirrupLine(dxf, layer, new Vector2(points[l + 1].X + radius, points[l + 1].Y), new Vector2(points[l + number + 1].X + radius, points[l + number + 1].Y));
                            DrawStirrupLine(dxf, layer, new Vector2(points[r].X + radius, points[r].Y), new Vector2(points[r + number].X + radius, points[r + number].Y));
                            DrawStirrupLine(dxf, layer, new Vector2(points[r - 1].X - radius, points[r - 1].Y), new Vector2(points[r + number - 1].X - radius, points[r + number - 1].Y));
                        }
                        else
                        {
                            DrawStirrupLine(dxf, layer, new Vector2(points[l].X, points[l].Y - radius), new Vector2(points[l + number].X, points[l + number].Y - radius));
                            DrawStirrupLine(dxf, layer, new Vector2(points[l + 1].X, points[l + 1].Y + radius), new Vector2(points[l + 1 + number].X, points[l + 1 + number].Y + radius));
                            DrawStirrupLine(dxf, layer, new Vector2(points[r].X, points[r].Y + radius), new Vector2(points[r + number].X, points[r + number].Y + radius));
                            DrawStirrupLine(dxf, layer, new Vector2(points[r - 1].X, points[r - 1].Y - radius), new Vector2(points[r + number - 1].X, points[r + number - 1].Y - radius));
                        }
                        l += 2;
                        r -= 2;
                    }
                }
                else if ( number <= 2 && number != 0 )
                {
                    if (points[l].Y == points[r].Y)
                    {
                        DrawStirrupLine(dxf, layer, new Vector2(points[l].X - radius, points[l].Y), new Vector2(points[l + number].X - radius, points[l + number].Y));
                        DrawStirrupLine(dxf, layer, new Vector2(points[r].X + radius, points[r].Y), new Vector2(points[r + number].X + radius, points[r + number].Y));
                    }
                    else
                    {
                        DrawStirrupLine(dxf, layer, new Vector2(points[l].X, points[l].Y - radius), new Vector2(points[l + number].X, points[l + number].Y - radius));
                        DrawStirrupLine(dxf, layer, new Vector2(points[r].X, points[r].Y + radius), new Vector2(points[r + number].X, points[r + number].Y + radius));

                        int index = number / 2 - 1;
                        List<Vector2> leaderPoints1 = new List<Vector2> { points[index], new Vector2(points[index].X - leaderLength, points[index].Y) };
                        DrawLeaderWithText(dxf, layer, leaderPoints1, ExcelData.colRein, AngularLeader.leaderDim1);

                        double dist = Math.Abs(points[index + 1].Y - points[index].Y);
                        List<Vector2> leaderPoints2 = new List<Vector2> { new Vector2(points[index + 1].X - radius, points[index + 1].Y + dist / 2), new Vector2(points[index + 1].X - radius - leaderLength, points[index + 1].Y + dist / 2) };
                        DrawLeaderWithText(dxf, layer, leaderPoints2, ExcelData.colStirrups, Constants.leaderDim);
                    }
                }
            }
        }

        public static void DrawOuterStirrups(List<Vector2> points, double radius, Layer layer, DxfDocument dxf)
        {
            DrawStirrupLine(dxf, layer, new Vector2(points[0].X - radius, points[0].Y), new Vector2(points[3].X - radius, points[3].Y));
            DrawStirrupLine(dxf, layer, new Vector2(points[0].X, points[0].Y - radius), new Vector2(points[1].X, points[1].Y - radius));
            DrawStirrupLine(dxf, layer, new Vector2(points[1].X + radius, points[1].Y), new Vector2(points[2].X + radius, points[2].Y));
            DrawStirrupLine(dxf, layer, new Vector2(points[2].X, points[2].Y + radius), new Vector2(points[3].X, points[3].Y + radius));
        }
    }
}