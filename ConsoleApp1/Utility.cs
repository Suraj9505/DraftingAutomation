﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using netDxf;
using netDxf.Tables;

namespace DraftingAutomation
{
    public class Constants
    {
        public const int row = 1;
        public const int footX = 2, footY = 3, footWX = 14, footWY = 15, footD = 16, footCC = 22;
        public const int colX = 5, colY = 6, colWX = 18, colWY = 19, colLen = 20, colCC = 23;
        public const int rubbleThk = 8, pccX = 10, pccY = 11, pccD = 12;
        public const int FRBottomX = 25, FRBottomY = 26, FRTopX = 27, FRTopY = 28;
        public const int CRVertical = 30, CRStirrups = 31;


        public static DimensionStyle gridDim = new DimensionStyle("GridDim")
        {
            TextHeight = 200,
            TextColor = AciColor.Red,
            TextVerticalPlacement = DimensionStyleTextVerticalPlacement.Above,
            TextOffset = 100,
            ArrowSize = 200,
            LengthPrecision = 0,
            ExtLineExtend = 50,
            ExtLineOffset = 100,
        };

        public static DimensionStyle dimensionStyle = new DimensionStyle("MyFontStyle")
        {
            TextHeight = 40,
            ArrowSize = 60,
            LengthPrecision = 0,
            ExtLineExtend = 20,
            ExtLineOffset = 20,
            //DimLengthUnits = LinearUnitType.Architectural,
        };

        public static DimensionStyle leaderDim = new DimensionStyle("LeaderDimStyle")
        {
            ArrowSize = 40,
            TextColor = AciColor.Red,
        };

        public static Layer hatchLayer = new Layer("HatchLayer")
        {
            Color = new AciColor(136),
        };


        public static List<double> ExtractNumbers(string input)
        {
            // Regular expression pattern to match all numbers
            string pattern = @"\d+";

            List<double> numbers = new List<double>();

            Regex regex = new Regex(pattern);

            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                numbers.Add(Convert.ToDouble(match.Value));
            }

            return numbers;
        }


        public static List<Vector2> GetRectanglePoints(Vector2 points, double widthX, double widthY)
        {
            return new List<Vector2>
            {
                new Vector2(points.X, points.Y),
                new Vector2(points.X + widthX, points.Y),
                new Vector2(points.X + widthX, points.Y + widthY),
                new Vector2(points.X, points.Y + widthY)
            };
        }

        public static List<Vector2> GetRectanglePointsFromCenter(Vector2 points, double widthX, double widthY)
        {
            List<Vector2> rectanglePoints = new List<Vector2>();

            rectanglePoints.Add(new Vector2(points.X - (widthX / 2), points.Y - (widthY / 2)));
            rectanglePoints.Add(new Vector2(points.X + (widthX / 2), points.Y - (widthY / 2)));
            rectanglePoints.Add(new Vector2(points.X + (widthX / 2), points.Y + (widthY / 2)));
            rectanglePoints.Add(new Vector2(points.X - (widthX / 2), points.Y + (widthY / 2)));


            return rectanglePoints;
        }

        public static List<Vector2> SortByX(List<Vector2> inputList)
        {
            List<Vector2> sortedList = inputList
            .GroupBy(v => new { v.X, v.Y })  
            .Select(group => group.First()) 
            .OrderBy(v => v.X)              
            .ToList();

            return sortedList;
        }
        public static List<Vector2> SortByY(List<Vector2> inputList)
        {
            List<Vector2> sortedList = inputList
            .GroupBy(v => new { v.X, v.Y })  
            .Select(group => group.First()) 
            .OrderBy(v => v.Y)              
            .ToList();

            return sortedList;
        }
    }




}
