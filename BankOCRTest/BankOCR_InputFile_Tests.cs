using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankOCR;

namespace BankOCR.Test
{
    [TestClass]
    public class BankOCR_InputFile_Tests
    {
        int _numberOfLinesPerEntry;
        string _filePath;
        string _fileName;
        string _fullFilePath;
        string[] _lines;
        string[] _invalidLines;

        
        [TestInitialize]
        public void Initialize()
        {
            _numberOfLinesPerEntry = Config.GetNumberOfLinesPerEntry();
            _filePath = Config.GetFilePath();
            _fileName = Config.GetValidFileName();
            _fullFilePath = Config.GetFullValidFilePath();
            _lines = FileParser.ReadFile(Config.GetFullValidFilePath());
            _invalidLines = FileParser.ReadFile(Config.GetFullInvalidFilePath());
        }

        #region "Test Methods"

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void BankOCRFileDirectoryNotFoundException()
        {
            IEnumerable<string> Files = Directory.EnumerateFiles(@"c:\NonExistantFolder", "NonExistantFile.txt");
        }

        [TestMethod]
        public void BankOCRFileNotFound()
        {
            IEnumerable<string> Files = Directory.EnumerateFiles(_filePath, "NonExistantFile.txt");
            Assert.IsFalse(Files.ToList().Count > 0);
        }

        [TestMethod]
        public void BankOCRFileExists()
        {
            IEnumerable<string> Files = Directory.EnumerateFiles(_filePath, _fileName);
            Assert.IsTrue(Files.ToList().Count > 0);
        }

        [TestMethod]
        public void BankOCRFileCanBeRead()
        {
            Assert.IsTrue(_lines.Count() > 0);
        }

        [TestMethod]
        public void AllCharactersAreValid()
        {
            Assert.IsTrue(Validator.ValidateCharacters(_lines));
        }

        [TestMethod]
        public void InvalidCharacterExist()
        {
            Assert.IsFalse(Validator.ValidateCharacters(_invalidLines));
        }

        [TestMethod]
        public void AllLinesAreTheCorrectLength()
        {
            Assert.IsTrue(Validator.CheckLineLenghts(_lines));
        }

        [TestMethod]
        public void InvalidLineLenghtsExist()
        {
            Assert.IsTrue(Validator.CheckLineLenghts(_lines));
        }

        [TestMethod]
        public void AccountEntriesAreFourLinesLong()
        {
            Assert.IsTrue(_lines.Count() % _numberOfLinesPerEntry == 0);
        }

        [TestMethod]
        public void NotAllAccountEntriesAreFourLinesLong()
        {
            Assert.IsFalse(_invalidLines.Count() % _numberOfLinesPerEntry == 0);
        }

        #endregion
    }
}
