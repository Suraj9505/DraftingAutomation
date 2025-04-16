using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;

namespace DraftingAutomation
{
    public class Rectangle
    {
        private static DimensionStyle dimensionStyle = Constants.dimensionStyle;

        public static void DrawNormalRectangle(double widthX, double widthY, List<Vector2> points, Layer layer, Vector2 pos, bool dimension, DxfDocument dxf)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                {
                    Line line = new Line(points[i], points[0]) { Layer = layer };
                    dxf.Entities.Add(line);
                }
                else
                {
                    Line line = new Line(points[i], points[i + 1]) { Layer = layer };
                    dxf.Entities.Add(line);
                }
            }

            if (dimension)
            {
                AddDimension(pos, widthX, widthY, dimensionStyle, dxf);
            }
        }

        public static void DrawRectangleWithCenter(Vector2 Center, double widthX, double widthY, bool dimension, Layer layer, DxfDocument dxf)
        {
            List<Vector2> rectVertex = Constants.GetRectanglePointsFromCenter(Center, widthX, widthY);

            foreach (Vector2 point in rectVertex)
            {
                Line line = new Line(point, rectVertex[(rectVertex.IndexOf(point) + 1) % rectVertex.Count]) { Layer = layer };
                dxf.Entities.Add(line);
            }

            if (dimension)
            {
                Vector2 pos = new Vector2();
                pos.X = Center.X - (widthX / 2);
                pos.Y = Center.Y - (widthY / 2);

                AlignedDimension dim = new()
                {
                    FirstReferencePoint = new Vector2(pos.X, pos.Y),
                    SecondReferencePoint = new Vector2(pos.X + widthX, pos.Y),
                    Style = new DimensionStyle("GridDim"),

                };

                if (layer != null)

                {
                    dim.Layer = layer;
                }
                else
                {
                    dim.Layer = new Layer("GridDimLayer");
                }

                AlignedDimension dim1 = new()
                {
                    FirstReferencePoint = new Vector2(pos.X + widthX, pos.Y),
                    SecondReferencePoint = new Vector2(pos.X + widthX, pos.Y + widthY),
                    Layer = new Layer("GridDimLayer"),
                    Style = new DimensionStyle("GridDim")
                };

                dim.SetDimensionLinePosition(new Vector2(pos.X, pos.Y - 500)); // this sets the postion of the dimension


                dim1.SetDimensionLinePosition(new Vector2(pos.X + widthX + 500, pos.Y)); // this sets the postion of the dimension

                dxf.Entities.Add(dim);

                dxf.Entities.Add(dim1);
            }
        }


        public static void AddDimension(Vector2 pos, double widthX, double widthY, DimensionStyle dimStyle, DxfDocument dxf)
        {
            AlignedDimension dim = new()
            {
                FirstReferencePoint = new Vector2(pos.X, pos.Y),
                SecondReferencePoint = new Vector2(pos.X + widthX, pos.Y),
                Layer = new Layer("dimensionLayer")
                {
                    Color = AciColor.Red
                },
                Style = dimStyle
            };

            AlignedDimension dim1 = new()
            {
                FirstReferencePoint = new Vector2(pos.X + widthX, pos.Y),
                SecondReferencePoint = new Vector2(pos.X + widthX, pos.Y + widthY),
                Layer = new Layer("dimensionLayer"),
                Style = dimStyle
            };

            dim.SetDimensionLinePosition(new Vector2(pos.X, pos.Y - 100)); // this sets the postion of the dimension

            dim1.SetDimensionLinePosition(new Vector2(pos.X + widthX + 100, pos.Y)); // this sets the postion of the dimension

            dxf.Entities.Add(dim);

            dxf.Entities.Add(dim1);
        }

    }
}
