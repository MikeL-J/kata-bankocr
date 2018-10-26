using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankOCR;

namespace BankOCR.Test
{


    [TestClass]
    public class EntityProcessor_Tests
    {
        int _numberOfLinesPerEntry;
        int _numberOfCharactersPerLine;
        int _numberOfCharactersPerSegment;
        string[] _lines;

        #region Test Methods
        [TestInitialize]
        public void Initialize()
        {
            _numberOfCharactersPerLine = Config.GetNumberOfCharactersPerLine();
            _numberOfLinesPerEntry = Config.GetNumberOfLinesPerEntry();
            _numberOfCharactersPerSegment = Config.GetNumberOfCharactersPerSegment();
            _lines = FileParser.ReadFile(Config.GetFullValidFilePath());
        }

        [TestMethod]
        public void DecodeZero()
        {
            List<string> codes = new List<string>();
            string zero = " _ | ||_|   ";
            codes.Add(zero);

            Assert.AreEqual("0", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeOne()
        {
            List<string> codes = new List<string>();
            string one = "     |  |   ";
            codes.Add(one);

            Assert.AreEqual("1", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeTwo()
        {
            List<string> codes = new List<string>();
            string two = " _  _||_    ";
            codes.Add(two);

            Assert.AreEqual("2", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeThree()
        {
            List<string> codes = new List<string>();
            string three = " _  _| _|   ";
            codes.Add(three);

            Assert.AreEqual("3", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeFour()
        {
            List<string> codes = new List<string>();
            string Four = "   |_|  |   ";
            codes.Add(Four);

            Assert.AreEqual("4", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeFive()
        {
            List<string> codes = new List<string>();
            string five = " _ |_  _|   ";
            codes.Add(five);

            Assert.AreEqual("5", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeSix()
        {
            List<string> codes = new List<string>();
            string six = " _ |_ |_|   ";
            codes.Add(six);

            Assert.AreEqual("6", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeSeven()
        {
            List<string> codes = new List<string>();
            string seven = " _   |  |   ";
            codes.Add(seven);

            Assert.AreEqual("7", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeEight()
        {
            List<string> codes = new List<string>();
            string eight = " _ |_||_|   ";
            codes.Add(eight);

            Assert.AreEqual("8", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeNine()
        {
            List<string> codes = new List<string>();
            string nine = " _ |_| _|   ";
            codes.Add(nine);

            Assert.AreEqual("9", Decoder.DecodeAccountNumber(codes));

        }

        [TestMethod]
        public void DecodeInvalidCode()
        {
            List<string> codes = new List<string>();
            string invalid = "||||||||||||";
            codes.Add(invalid);

            Assert.IsTrue(Decoder.DecodeAccountNumber(codes).Contains("?"));

        }

        [TestMethod]
        public void InputLinesParsedCorrectly()
        {
            var entities = FileParser.ParseEntities(_lines.ToList());
            Assert.IsTrue(entities.Count() == (_lines.Count() / _numberOfLinesPerEntry));
        }

        [TestMethod]
        public void EntitiesParsedCorrectly()
        {
            var entities = FileParser.ParseEntities(_lines.ToList());
            bool lineNumbersAreCorrect = false;

            foreach (var entity in entities)
            {
                if (entity.ToList().Count() == _numberOfLinesPerEntry) lineNumbersAreCorrect = true;
                break;
            }

            Assert.IsTrue(lineNumbersAreCorrect);
        }

        [TestMethod]
        public void SegmentsParsedCorrectly()
        {
            var entities = FileParser.ParseEntities(_lines.ToList());
            bool segmentNumbersAreCorrect = false;


            foreach (var entity in entities)
            {
                var segments = FileParser.ParseLineSegments(entity);
                var segmentCount = 0;

                foreach (var segment in segments)
                {
                    segmentCount += (segment.Count());
                }


                if (segmentCount == (_numberOfLinesPerEntry *
                            (_numberOfCharactersPerLine / _numberOfCharactersPerSegment)))
                {
                    segmentNumbersAreCorrect = true;
                    break;
                }

            }

            Assert.IsTrue(segmentNumbersAreCorrect);
        }

        [TestMethod]
        public void CheckSumIsValid()
        {
            var entities = FileParser.ParseEntities(_lines.ToList());
            List<string> codedAccountNumber = new List<string>();

            List<List<string>> segments = FileParser.ParseEntityLines(entities[entities.Count()-1]);
            codedAccountNumber = FileParser.CombineSegments(segments);
            string decodedAccountNumber = Decoder.DecodeAccountNumber(codedAccountNumber);

            Assert.IsTrue(Validator.IsCheckSumValid(decodedAccountNumber));
        }

        [TestMethod]
        public void CheckSumIsInvalid()
        {
            var entities = FileParser.ParseEntities(_lines.ToList());
            List<string> codedAccountNumber = new List<string>();

            List<List<string>> segments = FileParser.ParseEntityLines(entities[1]);
            codedAccountNumber = FileParser.CombineSegments(segments);
            string decodedAccountNumber = Decoder.DecodeAccountNumber(codedAccountNumber);

            Assert.IsFalse(Validator.IsCheckSumValid(decodedAccountNumber));
        }

        #endregion

    }
}
