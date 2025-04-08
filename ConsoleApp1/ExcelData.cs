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
        private List<double> _planPointsX = new List<double>();
        private List<double> _planPointsY = new List<double>();

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

                if(x != null)
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

                List<double> footingReinBotX = [];
                List<double> footingReinBotY = [];
                List<double> footingReinTopX = [];
                List<double> footingReinTopY = [];

                List<double> colReinVertical = [];
                List<double> colReinStirrups = [];

                footingReinBotX = Constants.ExtractNumbers(sheet.Cells[Constants.FRBottomX, footingCol].Value2);
                footingReinBotY = Constants.ExtractNumbers(sheet.Cells[Constants.FRBottomY, footingCol].Value2);
                footingReinTopX = Constants.ExtractNumbers(sheet.Cells[Constants.FRTopX, footingCol].Value2);
                footingReinTopY = Constants.ExtractNumbers(sheet.Cells[Constants.FRTopY, footingCol].Value2);

                colReinVertical = Constants.ExtractNumbers(sheet.Cells[Constants.CRVertical, footingCol].Value2);
                colReinStirrups = Constants.ExtractNumbers(sheet.Cells[Constants.CRStirrups, footingCol].Value2);


                double FRBotXDia = footingReinBotX[0];
                double FRBotXSpacing = footingReinBotX[1];
                double FRBotYDia = footingReinBotY[0];
                double FRBotYSpacing = footingReinBotY[1];

                double FRTopXDia = footingReinTopX[0];
                double FRTopXSpacing = footingReinTopX[1];
                double FRTopYDia = footingReinTopY[0];
                double FRTopYSpacing = footingReinTopY[1];

                double CRVerticalNum = colReinVertical[0];
                double CRVerticalDia = colReinVertical[1];

                double CRStirrupsDia = colReinStirrups[0];
                double CRStirrupsSpacing = colReinStirrups[1];

                dxf.DrawingVariables.InsUnits = DrawingUnits.Millimeters;

                ColumnTop colTop = new ColumnTop(ColTopStartPos, footingName, colWidthX, colWidthY, colLength, colCover, CRVerticalNum, CRVerticalDia, dxf);

                Plan.DrawFoundationLayot(pccWidthX, pccWidthY, footLoc, footWidthX, footWidthY, colLoc, colWidthX, colWidthY, dxf);

                Foundation.Foundation.DrawFoundation(PccStartPos, rubbleThk, pccWidthX, pccDepth, footWidthX, footingDepth, colWidthX, colLength, footCover, colCover, footingReinBotX, footingReinTopX, CRStirrupsSpacing, dxf);

                dxf.Save("C:\\Users\\SurajJha\\Desktop\\practice_projects\\DraftingAutomation\\Foundation.dxf");

                ColTopStartPos.X += colWidthX + 1000;
                PccStartPos.X += pccWidthX + 3000;

                footingCol++;
            }

        }

    }
}
