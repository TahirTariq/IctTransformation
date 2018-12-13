using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Ict.Business.DataReaders;
using Ict.Business.Interfaces;
using Ict.Business.Models;

namespace Ict.Business.UnitTests
{   
    [TestClass]
    public class IncrementalRecordCsvReaderTest
    {
        [TestMethod]
        public void Null_File_Test()
        {
            string data = null;

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            List<IncrementalRecord> records = fileReader.GetRecords().ToList();

            Assert.AreEqual(records.Count, 0);
        }

        [TestMethod]
        public void Empty_File_Test()
        {
            string data = @"";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            List<IncrementalRecord> records = fileReader.GetRecords().ToList();

            Assert.AreEqual(records.Count, 0);
        }

        [TestMethod]
        public void Invalid_Header_Test()
        {
            string data = @"Product, Earliest OriginYear, Development Year, Incremental Value,
                            P1,1995,1995,100";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            List<IncrementalRecord> records = fileReader.GetRecords().ToList();

            Assert.AreEqual(records.Count, 1);
            Assert.IsFalse(fileReader.HasErrors);

            IncrementalRecord record = records[0];

            Assert.AreEqual(record.Product, "P1");
            Assert.AreEqual(record.OriginYear, 1995);
            Assert.AreEqual(record.DevelopmentYear, 1995);
            Assert.AreEqual(record.IncrementalValue, 100);
        }

        [TestMethod]
        public void Read_Simple_Triangle_File_Test()
        {
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            P1,1995,1995,100";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            List<IncrementalRecord> records = fileReader.GetRecords().ToList();

            Assert.AreEqual(records.Count, 1);
            Assert.IsFalse(fileReader.HasErrors);

            IncrementalRecord record = records[0];

            Assert.AreEqual(record.Product, "P1");
            Assert.AreEqual(record.OriginYear, 1995);
            Assert.AreEqual(record.DevelopmentYear, 1995);
            Assert.AreEqual(record.IncrementalValue, 100);
        }

        [TestMethod]
        public void No_Product_File_Test()
        {            
            string data = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            ,1995,1995,100";

            IIncrementalRecordProvider fileReader = new IncrementalRecordCsvReader(data);

            List<IncrementalRecord> records = fileReader.GetRecords().ToList();                     

            Assert.AreEqual(records.Count, 0);
            Assert.IsTrue(fileReader.HasErrors);
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

            List<IncrementalRecord> records = fileReader.GetRecords().ToList();

            Assert.AreEqual(records.Count, 6);
            Assert.IsFalse(fileReader.HasErrors);
        }
    }
}
