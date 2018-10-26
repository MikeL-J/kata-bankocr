using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankOCR
{
    public class Decoder
    {
        public static string DecodeAccountNumber(List<string> numberCodes)
        {
            Dictionary<string, string> dt = new Dictionary<string, string>();
            dt.Add(" _ | ||_|   ", "0");
            dt.Add("     |  |   ", "1");
            dt.Add(" _  _||_    ", "2");
            dt.Add(" _  _| _|   ", "3");
            dt.Add("   |_|  |   ", "4");
            dt.Add(" _ |_  _|   ", "5");
            dt.Add(" _ |_ |_|   ", "6");
            dt.Add(" _   |  |   ", "7");
            dt.Add(" _ |_||_|   ", "8");
            dt.Add(" _ |_| _|   ", "9");
            StringBuilder sb = new StringBuilder();

            foreach (var code in numberCodes)
            {
                var num2 = dt.Where(p => p.Key == code).FirstOrDefault();
                sb.Append(string.IsNullOrWhiteSpace(num2.Key) ? "?" : num2.Value);
            }

            return sb.ToString();
        }
    }
}
 