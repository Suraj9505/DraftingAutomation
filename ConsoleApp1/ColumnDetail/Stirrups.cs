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
        public static void DrawInnerStirrups(List<Vector2> points, int number, double radius, Layer layer, DxfDocument dxf)
        {

            int leaderLength = 400;

            if (number % 2 != 0)
            {
                int firstIndex = number / 2;
                int secondIndex = number + firstIndex;
                // center stirrup

                if (points[firstIndex].X == points[secondIndex].X)
                {
                    Line line = new Line((new Vector2(points[firstIndex].X - radius, points[firstIndex].Y)), (new Vector2((points[secondIndex].X - radius), points[secondIndex].Y))) { Layer = layer };

                    Line botExtention = new Line(new Vector2(points[firstIndex].X + radius, points[firstIndex].Y), new Vector2(points[firstIndex].X + radius, points[firstIndex].Y + 30)) { Layer = layer };

                    Line topExtention = new Line(new Vector2(points[secondIndex].X + radius, points[secondIndex].Y), new Vector2(points[secondIndex].X + radius, points[secondIndex].Y - 30)) { Layer = layer };

                    dxf.Entities.Add(line);
                    dxf.Entities.Add(botExtention);
                    dxf.Entities.Add(topExtention);

                    int l = 0, r = number - 1;

                    while (l < r && number > 3)
                    {
                        Line leftStirrups1 = new Line(new Vector2(points[l].X - radius, points[l].Y), new Vector2(points[number + l].X - radius, points[number + l].Y)) { Layer = layer };

                        Line leftStirrups2 = new Line(new Vector2(points[l + 1].X + radius, points[l + 1].Y), new Vector2(points[l + 1 + number].X + radius, points[l + 1 + number].Y)) { Layer = layer };

                        Line rightStirrups1 = new Line(new Vector2(points[r].X + radius, points[r].Y), new Vector2(points[number + r].X + radius, points[number + r].Y)) { Layer = layer };

                        Line rightStirrups2 = new Line(new Vector2(points[r - 1].X - radius, points[r - 1].Y), new Vector2(points[r - 1 + number].X - radius, points[r - 1 + number].Y)) { Layer = layer };

                        dxf.Entities.Add(leftStirrups1);
                        dxf.Entities.Add(leftStirrups2);
                        dxf.Entities.Add(rightStirrups1);
                        dxf.Entities.Add(rightStirrups2);

                        l += 2;
                        r -= 2;
                        number -= 4;

                    }

                    if (number == 3)
                    {
                        Line midStirrups1 = new Line(new Vector2(points[l].X - radius, points[l].Y), new Vector2(points[l + number].X - radius, points[l + number].Y)) { Layer = layer };
                        Line midStirrups2 = new Line(new Vector2(points[r].X + radius, points[r].Y), new Vector2(points[r + number].X + radius, points[r + number].Y)) { Layer = layer };

                        dxf.Entities.Add(midStirrups1);
                        dxf.Entities.Add(midStirrups2);
                    }

                }

                else
                {
                    Line line = new Line(new Vector2(points[firstIndex].X, points[firstIndex].Y - radius), new Vector2(points[secondIndex].X, points[secondIndex].Y - radius)) { Layer = layer };

                    Line botExtention = new Line(new Vector2(points[firstIndex].X, points[firstIndex].Y + radius), new Vector2(points[firstIndex].X + 30, points[firstIndex].Y + radius)) { Layer = layer };

                    Line topExtention = new Line(new Vector2(points[secondIndex].X, points[secondIndex].Y + radius), new Vector2(points[secondIndex].X - 30, points[secondIndex].Y + radius)) { Layer = layer };

                    dxf.Entities.Add(line);
                    dxf.Entities.Add(botExtention);
                    dxf.Entities.Add(topExtention);


                    int l = 0, r = number - 1;

                    while (l < r && number > 3)
                    {
                        Line leftStirrups1 = new Line(new Vector2(points[l].X, points[l].Y - radius), new Vector2(points[number + l].X, points[number + l].Y - radius)) { Layer = layer };

                        Line leftStirrups2 = new Line(new Vector2(points[l + 1].X, points[l + 1].Y + radius), new Vector2(points[l + 1 + number].X, points[l + 1 + number].Y + radius)) { Layer = layer };

                        Line rightStirrups1 = new Line(new Vector2(points[r].X, points[r].Y + radius), new Vector2(points[number + r].X, points[number + r].Y + radius)) { Layer = layer };

                        Line rightStirrups2 = new Line(new Vector2(points[r - 1].X, points[r - 1].Y - radius), new Vector2(points[r - 1 + number].X, points[r - 1 + number].Y - radius)) { Layer = layer };

                        dxf.Entities.Add(leftStirrups1);
                        dxf.Entities.Add(leftStirrups2);
                        dxf.Entities.Add(rightStirrups1);
                        dxf.Entities.Add(rightStirrups2);

                        l += 2;
                        r -= 2;
                        number -= 4;

                    }

                    if (number == 3)
                    {
                        Line midStirrups1 = new Line(new Vector2(points[l].X, points[l].Y - radius), new Vector2(points[l + number].X, points[l + number].Y - radius)) { Layer = layer };
                        Line midStirrups2 = new Line(new Vector2(points[r].X, points[r].Y + radius), new Vector2(points[r + number].X, points[r + number].Y + radius)) { Layer = layer };

                        dxf.Entities.Add(midStirrups1);
                        dxf.Entities.Add(midStirrups2);
                    }

                    List<Vector2> leaderPoints1 = new List<Vector2>();
                    List<Vector2> leaderPoints2 = new List<Vector2>();

                    leaderPoints1.Add(points[firstIndex]);
                    leaderPoints1.Add(new Vector2(points[firstIndex].X - leaderLength, points[firstIndex].Y));

                    double dist = Math.Abs(points[firstIndex + 2].Y - points[firstIndex].Y);


                    leaderPoints2.Add(new Vector2(points[firstIndex + 1].X - radius, points[firstIndex + 1].Y + dist / 2));
                    leaderPoints2.Add(new Vector2(leaderPoints2[0].X - leaderLength, leaderPoints2[0].Y));

                    Leader leader1 = new Leader(leaderPoints1)
                    {
                        Layer = layer,
                        Style = AngularLeader.leaderDim1,
                        Annotation = new MText(ExcelData.colRein, leaderPoints1[1], 30)
                        {
                            Layer = Column.leaderLayer,
                            AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                            Color = AciColor.Green,
                        }
                    };

                    Leader leader2 = new Leader(leaderPoints2)
                    {
                        Layer = layer,
                        Style = Constants.leaderDim,
                        Annotation = new MText(ExcelData.colStirrups, leaderPoints2[1], 30)
                        {
                            Layer = Column.leaderLayer,
                            AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                            Color = AciColor.Green,
                        }
                    };

                    dxf.Entities.Add(leader1);
                    dxf.Entities.Add(leader2);

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
                            Line leftStirrups1 = new Line(new Vector2(points[l].X - radius, points[l].Y), new Vector2(points[l + number].X - radius, points[l + number].Y));

                            Line leftStirrups2 = new Line(new Vector2(points[l + 1].X + radius, points[l + 1].Y), new Vector2(points[l + number + 1].X + radius, points[l + number + 1].Y));

                            Line rightStirrups1 = new Line(new Vector2(points[r].X + radius, points[l].Y), new Vector2(points[r + number].X + radius, points[l + number].Y));

                            Line rightStirrups2 = new Line(new Vector2(points[r - 1].X - radius, points[r - 1].Y), new Vector2(points[r + number - 1].X - radius, points[l + number - 1].Y));

                            dxf.Entities.Add(leftStirrups1);
                            dxf.Entities.Add(leftStirrups2);
                            dxf.Entities.Add(rightStirrups1);
                            dxf.Entities.Add(rightStirrups2);

                        }

                        else
                        {
                            Line leftStirrups1 = new Line(new Vector2(points[l].X, points[l].Y - radius), new Vector2(points[l + number].X, points[l + number].Y - radius));

                            Line leftStirrups2 = new Line(new Vector2(points[l + 1].X, points[l + 1].Y + radius), new Vector2(points[l + number + 1].X, points[l + number + 1].Y + radius));

                            Line rightStirrups1 = new Line(new Vector2(points[r].X, points[l].Y + radius), new Vector2(points[r + number].X, points[l + number].Y + radius));

                            Line rightStirrups2 = new Line(new Vector2(points[r - 1].X, points[r - 1].Y - radius), new Vector2(points[r + number - 1].X, points[l + number - 1].Y - radius));

                            dxf.Entities.Add(leftStirrups1);
                            dxf.Entities.Add(leftStirrups2);
                            dxf.Entities.Add(rightStirrups1);
                            dxf.Entities.Add(rightStirrups2);

                        }
                        l += 2;
                        r -= 2;
                    }
                }

                else
                {
                    if (points[l].Y == points[r].Y)
                    {
                        Line line1 = new Line(new Vector2(points[l].X - radius, points[l].Y), new Vector2(points[l + number].X - radius, points[l + number].Y)) { Layer = layer };

                        Line line2 = new Line(new Vector2(points[r].X + radius, points[r].Y), new Vector2(points[r + number].X + radius, points[r + number].Y)) { Layer = layer };

                        dxf.Entities.Add(line1);
                        dxf.Entities.Add(line2);
                    }

                    else
                    {
                        Line line1 = new Line(new Vector2(points[l].X, points[l].Y - radius), new Vector2(points[l + number].X, points[l + number].Y - radius)) { Layer = layer };

                        Line line2 = new Line(new Vector2(points[r].X, points[r].Y + radius), new Vector2(points[r + number].X, points[r + number].Y + radius)) { Layer = layer };

                        dxf.Entities.Add(line1);
                        dxf.Entities.Add(line2);


                        int index = number / 2 - 1;

                        List<Vector2> leaderPoints1 = new List<Vector2>();
                        List<Vector2> leaderPoints2 = new List<Vector2>();

                        leaderPoints1.Add(points[index]);
                        leaderPoints1.Add(new Vector2(points[index].X - leaderLength, points[index].Y));


                        double dist = Math.Abs(points[index + 1].Y - points[index].Y);
                        leaderPoints2.Add(new Vector2(points[index + 1].X - radius, points[index + 1].Y + dist / 2));
                        leaderPoints2.Add(new Vector2(leaderPoints2[0].X - leaderLength, leaderPoints2[0].Y));

                        Leader leader1 = new Leader(leaderPoints1)
                        {
                            Layer = layer,
                            Style = AngularLeader.leaderDim1,
                            Annotation = new MText(ExcelData.colRein, leaderPoints1[1], 30)
                            {
                                Layer = Column.leaderLayer,
                                AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                                Color = AciColor.Green,
                            }
                        };

                        Leader leader2 = new Leader(leaderPoints2)
                        {
                            Layer = layer,
                            Style = Constants.leaderDim,
                            Annotation = new MText(ExcelData.colStirrups, leaderPoints2[1], 30)
                            {
                                Layer = Column.leaderLayer,
                                AttachmentPoint = MTextAttachmentPoint.MiddleLeft,
                                Color = AciColor.Green,
                            }
                        };

                        dxf.Entities.Add(leader1);
                        dxf.Entities.Add(leader2);
                    }
                }
            }




        }

        public static void DrawOuterStirrups(List<Vector2> points, double radius, Layer layer, DxfDocument dxf)
        {
            Line line1 = new Line(new Vector2(points[0].X - radius, points[0].Y), new Vector2(points[3].X - radius, points[3].Y)) { Layer = layer };

            Line line2 = new Line(new Vector2(points[0].X, points[0].Y - radius), new Vector2(points[1].X, points[1].Y - radius)) { Layer = layer };

            Line line3 = new Line(new Vector2(points[1].X + radius, points[1].Y), new Vector2(points[2].X + radius, points[2].Y)) { Layer = layer };

            Line line4 = new Line(new Vector2(points[2].X, points[2].Y + radius), new Vector2(points[3].X, points[3].Y + radius)) { Layer = layer };

            dxf.Entities.Add(line1);
            dxf.Entities.Add(line2);
            dxf.Entities.Add(line3);
            dxf.Entities.Add(line4);
        }

    }
}
