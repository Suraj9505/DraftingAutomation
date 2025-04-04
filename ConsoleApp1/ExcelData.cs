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

namespace DraftingAutomation
{
    public class ExcelData
    {

     DxfDocument dxf = new DxfDocument();
     DimensionStyle dimensionStyle = new DimensionStyle("MyFontStyle")
      {
            TextHeight = 40,
            ArrowSize = 20,
            LengthPrecision = 0,
            ExtLineExtend = 20,
            ExtLineOffset = 20,
         //DimLengthUnits = LinearUnitType.Architectural,
     };



        public void GetData()
        {
            string filePath = @"C:\Users\SurajJha\Desktop\practice_projects\c#_projects\Drafting Automation.xlsx";

            Excel.Application excelApp = new Excel.Application();

            excelApp.Visible = false;

            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);


            Excel.Worksheet sheet = workbook.Worksheets[2];

            int footerCol = 3;
            
            Vector2 ColTopStartPos = new Vector2(15000, 500);

            while (true)
            {
                string footingName = sheet.Cells[Constants.row, footerCol].Value2;
                   

                if (footingName == null) break;

                double footingWX = sheet.Cells[Constants.footWX, footerCol].Value2;
                double footingWY = sheet.Cells[Constants.footWY, footerCol].Value2;
                double footingDepth = sheet.Cells[Constants.footD, footerCol].Value2;

                double pccWidthX = sheet.Cells[Constants.pccX, footerCol].Value2;
                double pccWidthY = sheet.Cells[Constants.pccY, footerCol].Value2;
                double pccDepth = sheet.Cells[Constants.pccD, footerCol].Value2;

                double colWidthX = sheet.Cells[ Constants.colWX, footerCol].Value2 ;
                double colWidthY = sheet.Cells[Constants.colWY, footerCol].Value2 ;
                double colLength = sheet.Cells[Constants.colLen, footerCol].Value2;

                double footCover = sheet.Cells[Constants.footCC, footerCol].Value2;
                double colCover = sheet.Cells[Constants.colCC, footerCol].Value2;

                List<double> footingReinBotX = [];
                List<double> footingReinBotY = [];
                List<double> footingReinTopX = [];
                List<double> footingReinTopY = [];

                List<double> colReinVertical = [];
                List<double> colReinStirrups = [];

                footingReinBotX = Constants.ExtractNumbers(sheet.Cells[Constants.FRBottomX, footerCol].Value2);
                footingReinBotY = Constants.ExtractNumbers(sheet.Cells[Constants.FRBottomY, footerCol].Value2);
                footingReinTopX = Constants.ExtractNumbers(sheet.Cells[Constants.FRTopX, footerCol].Value2);
                footingReinTopY = Constants.ExtractNumbers(sheet.Cells[Constants.FRTopY, footerCol].Value2);

                colReinVertical = Constants.ExtractNumbers(sheet.Cells[Constants.CRVertical, footerCol].Value2);
                colReinStirrups = Constants.ExtractNumbers(sheet.Cells[Constants.CRStirrups, footerCol].Value2);


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

                ColumnTop colTop = new ColumnTop(ColTopStartPos, footingName, colWidthX, colWidthY, colLength, colCover, CRVerticalNum, CRVerticalDia, dxf, dimensionStyle);

                dxf.Save("C:\\Users\\SurajJha\\Desktop\\practice_projects\\DraftingAutomation\\Foundation.dxf");

                ColTopStartPos.X += colWidthX + 1000;

                footerCol++;
            }

        }

    }
}
