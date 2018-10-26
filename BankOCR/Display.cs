using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace BankOCR
{
    class Display
    {
        #region Options

        public static void PresentOptions()
        {
            WriteToConsole("Please enter the number of the option you want to execute.");
            WriteToConsole("");
            WriteToConsole("1. Validate the format of a valid file");
            WriteToConsole("2. Validate the format of an invalid file");
            WriteToConsole("3. Process a file with valid Account numbers");
            WriteToConsole("4. Process a file with illegal characters");
            WriteToConsole("5. Exit");
            WriteToConsole("");
            WriteToConsole("Enter value: ");
            var option = Console.ReadLine();
            WriteToConsole("");

            if (Validator.ValidateOption(option))
            { Console.Write("You have selected option {0}. Press enter to continue.", option); }
            else
            {
                Console.Write("{0} is not a valid option.", option);
                ReturnToOptions();

            }

            Console.ReadLine();
            Console.Clear();

            switch (Convert.ToInt32(option))
            {
                case 1:
                    DisplayFileValidation(Config.GetFullValidFilePath(), "File Format Validation");
                    break;
                case 2:
                    DisplayFileValidation(Config.GetFullInvalidFilePath(), "File Format Validation");
                    break;
                case 3:
                    DisplayProcessedFile(Config.GetFullValidFilePath(), "Use Case 1 Account Numbers");
                    break;
                case 4:
                    DisplayProcessedFile(Config.GetFullInvalidFilePath(), "Invalid Account Numbers");
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    ReturnToOptions();
                    break;
            }
        }

        #endregion

        #region File Validation

        private static void DisplayFileValidation(string filePath, string Title)
        {
            WriteToConsole(Title);
            WriteToConsole("");
            string[] accountFile = FileParser.ReadFile(filePath);
            try
            {
                if (Validator.CheckLineLenghts(accountFile)) WriteToConsole(" Line lengths are valid");
                else WriteToConsole(" * Incorrect line length(s)", ConsoleColor.Red);
                if (Validator.CheckLineLenghts(accountFile)) WriteToConsole(" Characters are valid");
                else WriteToConsole(" * Invalid character(s)", ConsoleColor.Red);
                if (Validator.CheckLineCount(accountFile)) WriteToConsole(" Number of lines are correct");
                else WriteToConsole(" * Incorrect number of lines", ConsoleColor.Red);
                ReturnToOptions();
            }
            catch (Exception ex)
            {
                WriteToConsole("");
                WriteToConsole(string.Format(" * {0}", ex.Message), ConsoleColor.Red);
                ReturnToOptions();
            }

        }
        #endregion

        #region ProcessFiles

        public static void DisplayProcessedFile(string filePath, string Title)
        {
            WriteToConsole(Title);
            WriteToConsole("");
            string[] accountFile = FileParser.ReadFile(filePath);
            List<List<string>> entities = FileParser.ParseEntities(accountFile.ToList());
            List<string> codedAccountNumber = new List<string>();

            foreach (var entity in entities)
            {
                List<List<string>> segments = FileParser.ParseEntityLines(entity);
                codedAccountNumber = FileParser.CombineSegments(segments);
                string decodedNumber = Decoder.DecodeAccountNumber(codedAccountNumber);
                if (decodedNumber.Contains("?"))
                {
                    WriteToConsole(string.Format("{0} - ILL", decodedNumber), ConsoleColor.Red);
                }
                else
                {
                    if (Validator.IsCheckSumValid(decodedNumber))
                    {
                        WriteToConsole(decodedNumber);
                    }
                    else
                    {
                        WriteToConsole(string.Format("{0} - ERR", decodedNumber), ConsoleColor.Cyan);
                    }
                }
            }
            ReturnToOptions();
        }

        #endregion

        #region Common Methods

        private static void ReturnToOptions()
        {
            WriteToConsole("");
            WriteToConsole("Press enter to return.");
            Console.ReadLine();
            Console.Clear();
            PresentOptions();
        }

        public static void WriteToConsole(string message)
        {
            WriteToConsole(message, Console.ForegroundColor);
        }

        public static void WriteToConsole(string message, ConsoleColor color)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }

        #endregion

    }
}

