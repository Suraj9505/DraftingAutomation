using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DraftingAutomation.Foundation.FoundationComp;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;


namespace DraftingAutomation
{
    public class ColumnTop
    {
        double _widthX, _widthY, _length, _cover, _barNum, _barDia, _spaceX, _spaceY;
        int _horizotalbars, _verticalBars;
        public DxfDocument _dxf;
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

        public ColumnTop(Vector2 pos, string colName, double widthX, double widthY, double length, double cover, string colReinVertical, DxfDocument dxf)
        {
            _widthX = widthX;
            _widthY = widthY;
            _length = length;
            _cover = cover;
            _dxf = dxf;
            _pos = pos;
            _colName = colName;
            _barNum = Constants.ExtractNumbers(colReinVertical)[0];
            _barDia = Constants.ExtractNumbers(colReinVertical)[1];
            _points = Constants.GetRectanglePoints(_pos, _widthX, _widthY);

            DrawColumnTop();
            DrawBars();
            DrawStirrups();

        }

        private void DrawColumnTop()

        {
            Rectangle.DrawNormalRectangle(_widthX, _widthY, _points, layer, _pos, true, _dxf);
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

            List<Circle> entities = new List<Circle>();

            entities.Add(circle0);
            entities.Add(circle0);
            entities.Add(circle1);
            entities.Add(circle2);
            entities.Add(circle3);

            int reaminingBars = (int)_barNum - 4;
            _verticalBars = Convert.ToInt32(reaminingBars * (_widthY / (_widthX + _widthY))) / 2;
            _horizotalbars = (reaminingBars - _verticalBars * 2) / 2;

            double distY = _widthY - 2 * (_cover + _barDia);
            double distX = _widthX - 2 * (_cover + _barDia);

             _spaceX = (distX - (_horizotalbars * _barDia)) / (_horizotalbars + 1);
             _spaceY = (distY - (_verticalBars * _barDia)) / (_verticalBars + 1);


            for (int i = 0; i < _verticalBars; i++)
            {
                double x = _points[0].X + _cover + (_barDia / 2), y = _points[0].Y + _cover + (_barDia / 2) + (_spaceY + _barDia) * (i + 1);

                Circle circle = new Circle(new Vector2(x, y), _barDia / 2) { Layer = barLayer };
                entities.Add(circle);

            }

            for (int i = 0; i < _verticalBars; i++)
            {
                double x = _points[1].X - _cover - (_barDia / 2), y = _points[1].Y + _cover + (_barDia / 2) + (_spaceY + _barDia) * (i + 1);

                Circle circle = new Circle(new Vector2(x, y), _barDia / 2) { Layer = barLayer };
                entities.Add(circle);

            }

            for (int i = 0; i < _horizotalbars; i++)
            {
                double x = _points[0].X + _cover + (_barDia / 2) + (_spaceX + _barDia) * (i + 1), y = _points[0].Y + _cover + (_barDia / 2);

                Circle circle = new Circle(new Vector2(x, y), _barDia / 2) { Layer = barLayer };
                entities.Add(circle);

            }

            for (int i = 0; i < _horizotalbars; i++)
            {
                double x = _points[3].X + _cover + (_barDia / 2) + (_spaceX + _barDia) * (i + 1), y = _points[3].Y - _cover - (_barDia / 2);

                Circle circle = new Circle(new Vector2(x, y), _barDia / 2) { Layer = barLayer };
                entities.Add(circle);

            }
            foreach (var item in entities)
            {
                Circle hatchBoundaryCircle = new Circle(item.Center, item.Radius) { Layer = item.Layer };
                HatchBoundaryPath hatchBoundary = new HatchBoundaryPath(new List<EntityObject> { hatchBoundaryCircle });

                Hatch hatch = new Hatch(HatchPattern.Solid, new List<HatchBoundaryPath> { hatchBoundary }, true)
                {
                    Layer = item.Layer,
                };

                _dxf.Entities.Add(hatch);
            }

        }

        private void DrawStirrups()
        {
            double space = _cover + _barDia / 2;
            double barRadius = _barDia / 2;
            double stirrupsExtension = 75;

            Line outer_stirrup1 = new Line(new Vector2(_points[0].X + space, _points[0].Y + _cover), new Vector2(_points[1].X - space, _points[0].Y + _cover)) { Layer = barLayer }; 

            Line outer_stirrup2 = new Line(new Vector2(_points[1].X - _cover, _points[1].Y + space), new Vector2(_points[2].X - _cover, _points[2].Y - space)) { Layer = barLayer }; 
            
            Line outer_stirrup3 = new Line(new Vector2(_points[2].X - space, _points[2].Y - _cover), new Vector2(_points[3].X + space, _points[3].Y - _cover)) { Layer = barLayer }; 

            Line outer_stirrup4 = new Line(new Vector2(_points[3].X + _cover, _points[3].Y - space), new Vector2(_points[0].X + _cover, _points[0].Y + space)) { Layer = barLayer };

            Vector2 outStirrupExt1 = new Vector2();

            outStirrupExt1.X = _points[3].X + _cover + barRadius * Math.Cos(45 * Math.PI / 180);
            outStirrupExt1.Y = _points[3].Y - space + barRadius * Math.Sin(45 * Math.PI / 180);

            Line outStiEx = new Line(outStirrupExt1, new Vector2(outStirrupExt1.X + stirrupsExtension, outStirrupExt1.Y + stirrupsExtension)) { Layer = barLayer };

            //outStiEx.Direction = ;

            _dxf.Entities.Add(outer_stirrup1);
            _dxf.Entities.Add(outer_stirrup2);
            _dxf.Entities.Add(outer_stirrup3);
            _dxf.Entities.Add(outer_stirrup4);
            _dxf.Entities.Add(outStiEx);
        }
    }
}
