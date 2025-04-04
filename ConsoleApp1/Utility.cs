using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DraftingAutomation
{
    public class Constants
    {
        public const int row = 1;
        public const int footX = 2, footY = 3, footWX = 14, footWY = 15, footD = 16, footCC = 22;
        public const int colX = 5, colY = 6, colWX = 18, colWY = 19, colLen = 20, colCC = 23;
        public const int rubbltThk = 8, pccX = 10, pccY = 11, pccD = 12;
        public const int  FRBottomX = 25, FRBottomY = 26, FRTopX = 27, FRTopY = 28;
        public const int CRVertical = 30, CRStirrups = 31;


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
    }


    
    }
