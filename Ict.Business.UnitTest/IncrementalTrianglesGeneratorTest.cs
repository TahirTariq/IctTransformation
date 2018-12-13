using System;
using Ict.Business.DataReaders;
using Ict.Business.Helper;
using Ict.Business.Interfaces;
using Ict.Business.Models;
using Ict.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ict.Business.UnitTests
{
    [TestClass]
    public class IncrementalTrianglesGeneratorTest
    {
        private IIncrementalReaderService _readerService;
        private IIncrementalTrianglesGenerator _trianglesGenerator;

        [TestInitialize]        
        public void TestInitialize()
        {
            _readerService = new IncrementalReaderService();
            _trianglesGenerator = new IncrementalTrianglesGenerator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Null_File_Test()
        {
            string data = null;

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateIncrementalTriangles(incrementalDataFile);
            
            Assert.IsNull(triangleDataFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Empty_File_Test()
        {
            string data = @"";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateIncrementalTriangles(incrementalDataFile);

            Assert.IsNull(triangleDataFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void No_Product_File_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            ,1995,1995,100";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateIncrementalTriangles(incrementalDataFile);

            Assert.IsNull(triangleDataFile);            
        }

        [TestMethod]
        public void Invalid_Header_Test()
        {
            string data = @"Product, Earliest OriginYear, Development Year, Incremental Value,
                            P1,1995,1995,100";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateIncrementalTriangles(incrementalDataFile);

            Assert.IsNotNull(triangleDataFile);
            Assert.AreEqual(triangleDataFile.ProductTriangles.Count, 1);
            Assert.AreEqual(triangleDataFile.EarliestOriginYear, 1995);
            Assert.AreEqual(triangleDataFile.DevelopmentYears, 1);
        }

        [TestMethod]
        public void Read_Simple_Triangle_File_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            P1,1995,1995,100";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateIncrementalTriangles(incrementalDataFile);

            Assert.AreEqual(triangleDataFile.ProductTriangles.Count, 1);
            Assert.AreEqual(triangleDataFile.EarliestOriginYear, 1995);
            Assert.AreEqual(triangleDataFile.DevelopmentYears, 1);
        }

        [TestMethod]
        public void Simple_Triangle_Csv_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            P1,1995,1995,100";

            string expected = "1995, 1\r\nP1,100\r\n";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateIncrementalTriangles(incrementalDataFile);
            
            string result = triangleDataFile.ToCsvString();
            
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Basic_Product_File_Csv_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            Basic,1995,1995,100
                            basic,1995,1996,50
                            Basic,1995,1997,200
                            Basic,1996,1996,80
                            basic,1996,1997,40
                            Basic,1997,1997,120
                          ";

            string expected = "1995, 3\r\nBasic,100,50,200,80,40,120\r\n";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateIncrementalTriangles(incrementalDataFile);

            string result = triangleDataFile.ToCsvString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Multiple_Products_File_Csv_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            Comp, 1992, 1992, 110.0
                            Comp, 1992, 1993, 170.0
                            Comp, 1993, 1993, 200.0
                            Non-Comp, 1990, 1990, 45.2
                            Non-Comp, 1990, 1991, 64.8
                            Non-Comp, 1990, 1993, 37.0
                            Non-Comp, 1991, 1991, 50.0
                            Non-Comp, 1991, 1992, 75.0
                            Non-Comp, 1991, 1993, 25.0
                            Non-Comp, 1992, 1992, 55.0
                            Non-Comp, 1992, 1993, 85.0
                            Non-Comp, 1993, 1993, 100.0
                          ";

            string expected = "1990, 4\r\nComp,0,0,0,0,0,0,0,110,170,200\r\nNon-Comp,45.2,64.8,0,37,50,75,25,55,85,100\r\n";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateIncrementalTriangles(incrementalDataFile);

            string result = triangleDataFile.ToCsvString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Basic_Product_Cumulative_Csv_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            Basic,1995,1995,100
                            basic,1995,1996,50
                            Basic,1995,1997,200
                            Basic,1996,1996,80
                            basic,1996,1997,40
                            Basic,1997,1997,120
                          ";

            string expected = "1995, 3\r\nBasic,100,150,350,80,120,120\r\n";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile triangleDataFile = _trianglesGenerator.GenerateCumulativeTriangles(incrementalDataFile);

            string result = triangleDataFile.ToCsvString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Multiple_Products_Cumulative_Csv_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            Comp, 1992, 1992, 110.0
                            Comp, 1992, 1993, 170.0
                            Comp, 1993, 1993, 200.0
                            Non-Comp, 1990, 1990, 45.2
                            Non-Comp, 1990, 1991, 64.8
                            Non-Comp, 1990, 1993, 37.0
                            Non-Comp, 1991, 1991, 50.0
                            Non-Comp, 1991, 1992, 75.0
                            Non-Comp, 1991, 1993, 25.0
                            Non-Comp, 1992, 1992, 55.0
                            Non-Comp, 1992, 1993, 85.0
                            Non-Comp, 1993, 1993, 100.0
                          ";

            string expected =
                "1990, 4\r\nComp,0,0,0,0,0,0,0,110,280,200\r\nNon-Comp,45.2,110,110,147,50,125,150,55,140,100\r\n";

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(data);

            TriangleDataFile cumulativeTriangles = _trianglesGenerator.GenerateCumulativeTriangles(incrementalDataFile);

            string result = cumulativeTriangles.ToCsvString();

            Assert.AreEqual(expected, result);
        }
    }
}

