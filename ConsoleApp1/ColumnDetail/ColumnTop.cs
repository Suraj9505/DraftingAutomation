using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DraftingAutomation.Foundation.FoundationComp;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;


namespace DraftingAutomation.ColumnDetail
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

        }

        private void DrawColumnTop()

        {
            Rectangle.DrawNormalRectangle(_widthX, _widthY, _points, layer, _pos, true, _dxf);
        }

        private void DrawBars()
        {

            List<Vector2> cornerBarPositions = new List<Vector2>();
            List<Vector2> verticalBarPositions = new List<Vector2>();
            List<Vector2> horizontalBarPositions = new List<Vector2>();

            // Add 4 corner bars
            cornerBarPositions.Add(new Vector2(_points[0].X + _barDia / 2 + _cover, _points[0].Y + _cover + _barDia / 2));
            cornerBarPositions.Add(new Vector2(_points[1].X - _barDia / 2 - _cover, _points[1].Y + _cover + _barDia / 2));
            cornerBarPositions.Add(new Vector2(_points[2].X - _barDia / 2 - _cover, _points[2].Y - _cover - _barDia / 2));
            cornerBarPositions.Add(new Vector2(_points[3].X + _barDia / 2 + _cover, _points[3].Y - _cover - _barDia / 2));

            // Calculate remaining bars
            int remainingBars = (int)_barNum - 4;
            _verticalBars = Convert.ToInt32(remainingBars * (_widthY / (_widthX + _widthY))) / 2;
            _horizotalbars = (remainingBars - _verticalBars * 2) / 2;

            double distY = _widthY - 2 * (_cover + _barDia);
            double distX = _widthX - 2 * (_cover + _barDia);

            _spaceX = (distX - _horizotalbars * _barDia) / (_horizotalbars + 1);
            _spaceY = (distY - _verticalBars * _barDia) / (_verticalBars + 1);

            // Vertical bars (left & right sides)
            for (int i = 0; i < _verticalBars; i++)
            {
                double yOffset = (_spaceY + _barDia) * (i + 1);
                verticalBarPositions.Add(new Vector2(_points[0].X + _cover + _barDia / 2, _points[0].Y + _cover + _barDia / 2 + yOffset)); // Left
                verticalBarPositions.Add(new Vector2(_points[1].X - _cover - _barDia / 2, _points[1].Y + _cover + _barDia / 2 + yOffset)); // Right
            }

            // Horizontal bars (top & bottom sides)
            for (int i = 0; i < _horizotalbars; i++)
            {
                double xOffset = (_spaceX + _barDia) * (i + 1);
                horizontalBarPositions.Add(new Vector2(_points[0].X + _cover + _barDia / 2 + xOffset, _points[0].Y + _cover + _barDia / 2)); // Top
                horizontalBarPositions.Add(new Vector2(_points[3].X + _cover + _barDia / 2 + xOffset, _points[3].Y - _cover - _barDia / 2)); // Bottom
            }

            // Combine all for drawing
            var allBarPositions = new List<Vector2>();
            allBarPositions.AddRange(cornerBarPositions);
            allBarPositions.AddRange(verticalBarPositions);
            allBarPositions.AddRange(horizontalBarPositions);

            // Draw circles and hatches
            foreach (var pos in allBarPositions)
            {
                Circle hatchBoundaryCircle = new Circle(pos, _barDia / 2) { Layer = barLayer };
                HatchBoundaryPath hatchBoundary = new HatchBoundaryPath(new List<EntityObject> { hatchBoundaryCircle });
                Hatch hatch = new Hatch(HatchPattern.Solid, new List<HatchBoundaryPath> { hatchBoundary }, true)
                {
                    Layer = barLayer,
                };
                _dxf.Entities.Add(hatch);
            }

            horizontalBarPositions = horizontalBarPositions.OrderBy(p => p.Y).ToList();
            verticalBarPositions = verticalBarPositions.OrderBy(p => p.X).ToList();
            Stirrups.DrawInnerStirrups(horizontalBarPositions, _horizotalbars, _barDia / 2, barLayer, _dxf);
            Stirrups.DrawInnerStirrups(verticalBarPositions, _verticalBars, _barDia / 2, barLayer, _dxf);
            Stirrups.DrawOuterStirrups(cornerBarPositions, _barDia / 2, barLayer, _dxf);
        }
    }
}
