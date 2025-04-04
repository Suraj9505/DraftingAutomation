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
    public class ColumnTop
    {
        double _widthX, _widthY, _length, _cover, _barNum, _barDia, _spaceX, _spaceY;
        public DxfDocument _dfx;
        public DimensionStyle _dimStyle;
        Vector2 _pos;
        public string _colName;
        private List<Vector2> _points = new List<Vector2>();

        Layer layer = new Layer("ColumnTop")
        {
            Color = AciColor.Green,
            //Linetype = Linetype.ByLayer,
            //Lineweight = Lineweight.ByLayer
        };

        Layer textLayer = new Layer("TextLayer")
        {
            Color = AciColor.Red,
            //Linetype = Linetype.ByLayer,
            //Lineweight = Lineweight.ByLayer
        };

        Layer barLayer = new Layer("barLayer")
        {
            Color = new AciColor(240),
        };

        public ColumnTop(Vector2 pos, string colName, double widthX, double widthY, double length, double cover, double barNum, double barDia, DxfDocument dxf, DimensionStyle dimStyle)
        {
            _widthX = widthX;
            _widthY = widthY;
            _length = length;
            _cover = cover;
            _dfx = dxf;
            _dimStyle = dimStyle;
            _pos = pos;
            _colName = colName;
            _barNum = barNum;
            _barDia = barDia;
            _points = new List<Vector2>
            {
                new Vector2(_pos.X, _pos.Y),
                new Vector2(_pos.X + _widthX, _pos.Y),
                new Vector2(_pos.X + _widthX, _pos.Y + _widthY),
                new Vector2(_pos.X, _pos.Y + _widthY)
            };

            DrawColumnTop();
            DrawBars();

        }

        private void DrawColumnTop()

        {
            for (int i = 0; i < _points.Count; i++)
            {
                if (i == _points.Count - 1)
                {
                    Line line = new Line(_points[i], _points[0]) { Layer = layer };
                    _dfx.Entities.Add(line);
                }
                else
                {
                    Line line = new Line(_points[i], _points[i + 1]) { Layer = layer };
                    _dfx.Entities.Add(line);
                }
            }

            AlignedDimension dim = new()
            {
                FirstReferencePoint = new Vector2(_pos.X, _pos.Y),
                SecondReferencePoint = new Vector2(_pos.X + _widthX, _pos.Y),
                Style = DimensionStyle.Default
            };
            
            AlignedDimension dim1 = new()
            {
                FirstReferencePoint = new Vector2(_pos.X + _widthX , _pos.Y),
                SecondReferencePoint = new Vector2(_pos.X + _widthX, _pos.Y + _widthY),
                Style = DimensionStyle.Default
            };

            dim.SetDimensionLinePosition(new Vector2(_pos.X , _pos.Y - 100)); // this sets the postion of the dimension
            dim.Style = _dimStyle;

            dim1.SetDimensionLinePosition(new Vector2(_pos.X + _widthX + 100, _pos.Y)); // this sets the postion of the dimension
            dim1.Style = _dimStyle;

            _dfx.Entities.Add(dim);

            _dfx.Entities.Add(dim1);

            //MText text = new MText(_colName, new Vector2(_pos.X + 150, _pos.Y - 200), 100, 20) { Layer = textLayer };

            //_dfx.Entities.Add(text);
        }

        private void DrawBars()
        {
            Circle circle0 = new Circle(new Vector2(_points[0].X + _barDia / 2 + _cover, _points[0].Y + _cover + _barDia / 2), _barDia / 2)
            { Layer = barLayer };
            Circle circle1 = new Circle(new Vector2(_points[1].X - _barDia / 2 - _cover, _points[1].Y + _cover + _barDia / 2), _barDia / 2)
            { Layer = barLayer };
            Circle circle2 = new Circle(new Vector2(_points[2].X - _barDia / 2 - _cover, _points[2].Y - _cover - _barDia / 2), _barDia / 2)
            { Layer = barLayer };
            Circle circle3 = new Circle(new Vector2(_points[3].X + _barDia / 2 + _cover, _points[3].Y - _cover - _barDia / 2), _barDia / 2)
            { Layer = barLayer };


            _dfx.Entities.Add(circle0);
            _dfx.Entities.Add(circle1);
            _dfx.Entities.Add(circle2);
            _dfx.Entities.Add(circle3);

            int reaminingBars = (int)_barNum - 4;
            int verticalBars = Convert.ToInt32(reaminingBars * (_widthY / (_widthX + _widthY))) / 2;
            int horizontalBars = (reaminingBars - verticalBars * 2) / 2;

            double distY = _widthY - 2 * (_cover + _barDia);
            double distX = _widthX - 2 * (_cover + _barDia);

            double _spaceX = (distX - (horizontalBars * _barDia)) / (horizontalBars + 1);
            double _spaceY = (distY - (verticalBars * _barDia)) / (verticalBars + 1);

            for (int i = 0; i < verticalBars; i++)
            {
                double x = _points[0].X + _cover + (_barDia / 2), y = _points[0].Y + _cover + (_barDia / 2) + (_spaceY + _barDia) * (i + 1);

                Circle circle = new Circle(new Vector2(x, y), _barDia / 2) { Layer = barLayer };
                _dfx.Entities.Add(circle);

            }

            for (int i = 0; i < verticalBars; i++)
            {
                double x = _points[1].X - _cover - (_barDia / 2), y = _points[1].Y + _cover + (_barDia / 2) + (_spaceY + _barDia) * (i + 1);

                Circle circle = new Circle(new Vector2(x, y), _barDia / 2) { Layer = barLayer };
                _dfx.Entities.Add(circle);

            }

            for (int i = 0; i < horizontalBars; i++)
            {
                double x = _points[0].X + _cover + (_barDia / 2) + (_spaceX + _barDia) * (i + 1), y = _points[0].Y + _cover + (_barDia / 2);

                Circle circle = new Circle(new Vector2(x, y), _barDia / 2) { Layer = barLayer };
                _dfx.Entities.Add(circle);

            }

            for (int i = 0; i < horizontalBars; i++)
            {
                double x = _points[3].X + _cover + (_barDia / 2) + (_spaceX + _barDia) * (i + 1), y = _points[3].Y - _cover - (_barDia / 2);

                Circle circle = new Circle(new Vector2(x, y), _barDia / 2) { Layer = barLayer };
                _dfx.Entities.Add(circle);

            }

        }
    }
}
