using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankOCR
{
    public class Validator
    {
        public static bool CheckLineLenghts(string[] lines)
        {
            bool linesAreCorrectLength = true;

            foreach (var line in lines)
            {
                if (line.Length != Config.GetNumberOfCharactersPerLine())
                {
                    linesAreCorrectLength = false;
                    break;
                }
            }
            return linesAreCorrectLength;
        }

        public static bool CheckLineCount(string[] accountFile)
        {
            bool lineCountIsCorrect = true;
            if (accountFile.Count() % Config.GetNumberOfLinesPerEntry() != 0)
            {
                lineCountIsCorrect = false;
            }
            return lineCountIsCorrect;
        }

        public static bool ValidateCharacters(string[] lines)
        {
            foreach (var line in lines)
            {
                if(!CharactersAreValid(line)){
                return false;
                }
            }
            return true;
        }

        public static bool CharactersAreValid(string line)
        {
            foreach (var character in line)
            {
                if (!IsCharacterValid(character)){
                    return false;
                 }
            }
            return true;
        }

        private static bool IsCharacterValid(char x)
        {
            bool isValid = false;
            string y = x.ToString();

            if (IComparable.Equals(y, " ")) isValid = true;
            else if (IComparable.Equals(y, "_")) isValid = true;
            else if (IComparable.Equals(y, "|")) isValid = true;

            return isValid;
        }

        public static bool IsCheckSumValid(string decodedAccountNumber)
        {
            int checkMultiplier = decodedAccountNumber.Length;
            List<int> checkNumbers = new List<int>();
            foreach (var item in decodedAccountNumber)
            {
                checkNumbers.Add(int.Parse(item.ToString()) * checkMultiplier);
                checkMultiplier--;
            }

            return checkNumbers.Sum() % 11 == 0 ? true : false;
        }

        public static bool ValidateOption(string option)
        {
            if (!IsNumeric(option))
            { return false; }
            int iOption = Convert.ToInt32(option);

            if (iOption >= Config.GetMinOptionValue() && iOption <= Config.GetMaxOptionValue())
            { return true; }
            else
            { return false; }
        }

        private static Boolean IsNumeric(string stringToTest)
        {
            int result;
            return int.TryParse(stringToTest, out result);
        }

    }
}
