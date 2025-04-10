using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using netDxf.Units;
using DraftingAutomation.Foundation.FoundationComp;
using DraftingAutomation.Foundation;

namespace DraftingAutomation
{

    public class ExcelData
    {
        public static List<double> _planPointsX = new List<double>();
        public static List<double> _planPointsY = new List<double>();

        DxfDocument dxf = new DxfDocument();

        public void GetData()
        {
            string filePath = @"C:\Users\SurajJha\Desktop\practice_projects\c#_projects\Drafting Automation.xlsx";

            Excel.Application excelApp = new Excel.Application();

            excelApp.Visible = false;

            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);


            Excel.Worksheet planData = workbook.Worksheets[1];
            Excel.Worksheet sheet = workbook.Worksheets[2];


            int footingCol = 3;


            int planRow = 2;
            int planColX = 2;
            int planColY = 3;

            Vector2 ColTopStartPos = new Vector2(15000, 500);
            Vector2 PccStartPos = new Vector2(50000, 20000);

            while (true)
            {
                var x = planData.Cells[planRow, planColX].Value2;
                var y = planData.Cells[planRow, planColY].Value2;

                if (x == null && y == null) break;

                if (x != null)
                {
                    _planPointsX.Add(x);
                }

                if (y != null)
                {
                    _planPointsY.Add(y);
                }

                planRow++;

            }

            Plan.DrawGrid(_planPointsX, _planPointsY, dxf);

            while (true)
            {
                string footingName = sheet.Cells[Constants.row, footingCol].Value2;


                if (footingName == null) break;

                Vector2 footLoc = new Vector2();
                Vector2 colLoc = new Vector2();

                footLoc.X = sheet.Cells[Constants.footX, footingCol].Value2;
                footLoc.Y = sheet.Cells[Constants.footY, footingCol].Value2;

                colLoc.X = sheet.Cells[Constants.colX, footingCol].Value2;
                colLoc.Y = sheet.Cells[Constants.colY, footingCol].Value2;

                footLoc.X = sheet.Cells[Constants.footX, footingCol].Value2;

                double rubbleThk = sheet.Cells[Constants.rubbleThk, footingCol].Value2;

                double footWidthX = sheet.Cells[Constants.footWX, footingCol].Value2;
                double footWidthY = sheet.Cells[Constants.footWY, footingCol].Value2;
                double footingDepth = sheet.Cells[Constants.footD, footingCol].Value2;

                double pccWidthX = sheet.Cells[Constants.pccX, footingCol].Value2;
                double pccWidthY = sheet.Cells[Constants.pccY, footingCol].Value2;
                double pccDepth = sheet.Cells[Constants.pccD, footingCol].Value2;

                double colWidthX = sheet.Cells[Constants.colWX, footingCol].Value2;
                double colWidthY = sheet.Cells[Constants.colWY, footingCol].Value2;
                double colLength = sheet.Cells[Constants.colLen, footingCol].Value2;

                double footCover = sheet.Cells[Constants.footCC, footingCol].Value2;
                double colCover = sheet.Cells[Constants.colCC, footingCol].Value2;

                string footingReinBotX;
                string footingReinBotY;
                string footingReinTopX;
                string footingReinTopY;

                string colReinVertical;
                string colReinStirrups;

                footingReinBotX = sheet.Cells[Constants.FRBottomX, footingCol].Value2;
                footingReinBotY = sheet.Cells[Constants.FRBottomY, footingCol].Value2;
                footingReinTopX = sheet.Cells[Constants.FRTopX, footingCol].Value2;
                footingReinTopY = sheet.Cells[Constants.FRTopY, footingCol].Value2;

                colReinVertical = sheet.Cells[Constants.CRVertical, footingCol].Value2;
                colReinStirrups = sheet.Cells[Constants.CRStirrups, footingCol].Value2;

                dxf.DrawingVariables.InsUnits = DrawingUnits.Millimeters;

                ColumnTop colTop = new ColumnTop(ColTopStartPos, footingName, colWidthX, colWidthY, colLength, colCover, colReinVertical, dxf);

                Plan.DrawFoundationLayot(pccWidthX, pccWidthY, footLoc, footWidthX, footWidthY, colLoc, colWidthX, colWidthY, dxf);

                Foundation.Foundation.DrawFoundation(colReinStirrups, PccStartPos, rubbleThk, pccWidthX, pccDepth, footWidthX, footingDepth, colWidthX, colLength, footCover, colCover, footingReinBotX, footingReinTopX, colReinVertical, dxf);

                dxf.Save("C:\\Users\\SurajJha\\Desktop\\practice_projects\\DraftingAutomation\\Foundation.dxf");

                ColTopStartPos.X += colWidthX + 1000;
                PccStartPos.X += pccWidthX + 3000;

                footingCol++;
            }

        }

    }
}
