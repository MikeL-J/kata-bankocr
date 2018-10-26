using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace BankOCR
{
    public static class Config  
    {
        public static string GetFilePath(){
            return ConfigurationManager.AppSettings["filePath"];
        }

        public static string GetValidFileName(){
            return ConfigurationManager.AppSettings["validFileName"];
        }

        public static string GetInvalidFileName(){
            return ConfigurationManager.AppSettings["invalidFileName"];
        }
        
        public static string GetFullValidFilePath(){
            string currDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(currDirectory, GetFilePath(), GetValidFileName());
        } 

        public static string GetFullInvalidFilePath(){
            string currDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(currDirectory, GetFilePath(), GetInvalidFileName());
        }

        public static int GetNumberOfLinesPerEntry(){
            return Convert.ToInt32(ConfigurationManager.AppSettings["numberOfLinesPerEntry"]);
        }

        public static int GetNumberOfCharactersPerLine(){
            return Convert.ToInt32(ConfigurationManager.AppSettings["numberOfCharactersPerLine"]);
        }

        public static int GetNumberOfCharactersPerSegment(){
            return Convert.ToInt32(ConfigurationManager.AppSettings["numberOfCharactersPerSegment"]);
        }

        public static int GetMinOptionValue(){
            return Convert.ToInt32(ConfigurationManager.AppSettings["minOptionValue"]);        
        }

        public static int GetMaxOptionValue(){
            return Convert.ToInt32(ConfigurationManager.AppSettings["maxOptionValue"]);
        }
    }
}
