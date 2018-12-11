using System;
using IctTriangle.Business.DataReaders;
using IctTriangle.Business.Interfaces;
using IctTriangle.Business.Models;

namespace IctTriangle.Business.Services
{
    public class IctTransformationService
    {
        public IncrementalDataFile ReadIncrementalCsvData(string data)
        {
            IIncrementalRecordProvider csvFileReader = new IncrementalRecordCsvReader(data);
            
            return new IncrementalDataFileGenerator(csvFileReader);
        }    

        public TriangleDataFile CreateCumulativeData(IncrementalDataFile claimsFile)
        {
            if (claimsFile == null)
                throw new ArgumentNullException(nameof(claimsFile));

            if(!claimsFile.IsValid)
                throw new ArgumentException("invalid IncrementalDataFile");

            IIncrementalTrianglesGenerator trianglesGenerator = new IncrementalTrianglesGenerator(claimsFile);

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
