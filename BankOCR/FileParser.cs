using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BankOCR
{
    public class FileParser
    {

        public static string[] ReadFile(string filePath, string fileName)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("The file path is invalid.", filePath);

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("The file name is invalid.", fileName);

            string fullFilePath = string.Format("{0}{1}", filePath, fileName);
            if (!File.Exists(fullFilePath))
                throw new ArgumentException("The file can not be found.");

            return File.ReadAllLines(fullFilePath, Encoding.Default);
        }

        public static string[] ReadFile(string fullFilePath)
        {
            if (string.IsNullOrEmpty(fullFilePath))
                throw new ArgumentException("The full file path is invalid.", fullFilePath);

            if (!File.Exists(fullFilePath))
                throw new SystemException("File path is not valid!");

            return File.ReadAllLines(fullFilePath, Encoding.Default);
        }

        public static List<List<string>> ParseEntities(List<string> validLines)
        {
            if (validLines == null || validLines.ToArray().Length < 1)
            { throw new ArgumentException("The file has no accounts or could no be read.", "validLines"); }

            List<List<string>> entities = new List<List<string>>();
            for (int i = 0; i < validLines.Count(); i += (Config.GetNumberOfLinesPerEntry()))
            {
                List<string> lines = validLines.Skip(i).Take(Config.GetNumberOfLinesPerEntry()).ToList();
                entities.Add(lines);
            }

            return entities;
        }

        public static List<List<string>> ParseEntityLines(List<string> entity)
        {
            if (entity == null || entity.ToArray().Length < 1)
            { throw new ArgumentException("The number entry is not valid.", "entity"); }

            List<List<string>> segments = new List<List<string>>();
            segments = ParseLineSegments(entity);

            return segments;
        }

        public static List<List<string>> ParseEntityLines(List<List<string>> entities)
        {
            if (entities == null || entities.ToArray().Length < 1)
            { throw new ArgumentException("The number entries are not valid.", "entities"); }

            List<List<string>> segments = new List<List<string>>();
            foreach (var entity in entities)
            {
                segments = ParseLineSegments(entity);
            }

            return segments;
        }

        public static List<List<string>> ParseLineSegments(List<string> entity)
        {
            if (entity == null || entity.ToArray().Length < 1)
            { throw new ArgumentException("The entity line is not valid.", "entity"); }

            List<List<string>> segments = new List<List<string>>();


            foreach (string line in entity)
            {
                    List<string> lineSegments = new List<string>();

                    for (int i = 0; i < Config.GetNumberOfCharactersPerLine(); i += Config.GetNumberOfCharactersPerSegment())
                    {
                        string segment = new string(line.Skip(i).Take(Config.GetNumberOfCharactersPerSegment()).ToArray());
                        lineSegments.Add(segment);
                    }
                    segments.Add(lineSegments);
            }

            return segments;
        }

        public static List<string> CombineSegments(List<List<string>> segments)
        {
            if (segments == null || segments.ToArray().Length < 1)
            { throw new ArgumentException("The number segments are not valid", "segments"); }

            List<string> numbers = new List<String>();
            for (int i = 0; i < (Config.GetNumberOfCharactersPerLine() / Config.GetNumberOfCharactersPerSegment()); i++)
            {
                StringBuilder number = new StringBuilder();
                foreach (var lineOfSegments in segments)
                {
                    number.Append(lineOfSegments[i]);
                }
                numbers.Add(number.ToString());
            }


            return numbers;
        }

    }
}
