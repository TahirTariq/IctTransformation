using System;
using Ict.Business.Interfaces;
using Ict.Business.DataReaders;
using Ict.Business.Models;

namespace Ict.Business.Services
{
    public class IctTransformationService : IIctTransformationService
    {
        public IncrementalDataFile ReadIncrementalCsvData(string csvData)
        {
            IIncrementalRecordProvider csvFileReader = new IncrementalRecordCsvReader(csvData);
            
            return new IncrementalDataFileGenerator(csvFileReader);
        }    

        public TriangleDataFile CreateCumulativeData(IncrementalDataFile incrementalDataFile)
        {
            if (incrementalDataFile == null)
                throw new ArgumentNullException(nameof(incrementalDataFile));

            if(!incrementalDataFile.IsValid)
                throw new ArgumentException("invalid IncrementalDataFile");

            IIncrementalTrianglesGenerator trianglesGenerator = new IncrementalTrianglesGenerator(incrementalDataFile);

            TriangleDataFile incrementalTrianglesFile = trianglesGenerator.GenerateIncrementalTriangles();

            return trianglesGenerator.GenerateCumulativeTriangles(incrementalTrianglesFile);
        }

        #region Possible usages

        public IncrementalDataFile ReadFromDb()
        {
            IIncrementalRecordProvider dbFileReader = new IncrementalRecordDbReader();
            
            return new IncrementalDataFileGenerator(dbFileReader);
        }

        #endregion
    }
}
