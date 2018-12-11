using Microsoft.VisualStudio.TestTools.UnitTesting;
using IctTriangle.Business.Models;
using IctTriangle.Business.DataReaders;
using IctTriangle.Business.Interfaces;
using IctTriangle.Business.Services;
using IctTriangle.Business.Helper;

namespace IctTriangle.Business.UnitTests
{

    [TestClass]
    public class IncrementalDataFileGeneratorTest
    {
        [TestMethod]
        public void Null_File_Test()
        {
            string data = null;

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            IncrementalDataFile incrementalDataFile = new IncrementalDataFileGenerator(fileReader);

            Assert.IsFalse(incrementalDataFile.IsValid);
        }

        [TestMethod]
        public void Empty_File_Test()
        {
            string data = @"";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            IncrementalDataFile incrementalDataFile = new IncrementalDataFileGenerator(fileReader);

            Assert.IsFalse(incrementalDataFile.IsValid);
        }

        [TestMethod]
        public void No_Product_File_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            ,1995,1995,100";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            IncrementalDataFile incrementalDataFile = new IncrementalDataFileGenerator(fileReader);

            Assert.IsFalse(incrementalDataFile.IsValid);
        }

        [TestMethod]
        public void Invalid_Header_Test()
        {
            string data = @"Product, Earliest OriginYear, Development Year, Incremental Value,
                            P1,1995,1995,100";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            IncrementalDataFile incrementalDataFile = new IncrementalDataFileGenerator(fileReader);

            Assert.IsTrue(incrementalDataFile.IsValid);
            Assert.AreEqual(incrementalDataFile.Claims.Count, 1);
            Assert.AreEqual(incrementalDataFile.EarliestOriginYear, 1995);
            Assert.AreEqual(incrementalDataFile.DevelopmentYears, 1);
        }

        [TestMethod]
        public void Read_Simple_Triangle_File_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            P1,1995,1995,100";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            IncrementalDataFile incrementalDataFile = new IncrementalDataFileGenerator(fileReader);

            Assert.IsTrue(incrementalDataFile.IsValid);
            Assert.AreEqual(incrementalDataFile.Claims.Count, 1);
            Assert.AreEqual(incrementalDataFile.EarliestOriginYear, 1995);
            Assert.AreEqual(incrementalDataFile.DevelopmentYears, 1);           
        }

        [TestMethod]
        public void Basic_Product_File_Ignore_Empty_Rows_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            Basic,1995,1995,100
                            Basic,1995,1996,50

                            Basic,1995,1997,200
                            Basic,1996,1996,80
                            Basic,1996,1997,40


                            Basic,1997,1997,40
                          ";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            IncrementalDataFile incrementalDataFile = new IncrementalDataFileGenerator(fileReader);

            Assert.IsTrue(incrementalDataFile.IsValid);
            Assert.AreEqual(incrementalDataFile.Claims.Count, 6);
            Assert.AreEqual(incrementalDataFile.EarliestOriginYear, 1995);
            Assert.AreEqual(incrementalDataFile.DevelopmentYears, 3);
            Assert.IsFalse(fileReader.HasErrors);
        }
    }
}
