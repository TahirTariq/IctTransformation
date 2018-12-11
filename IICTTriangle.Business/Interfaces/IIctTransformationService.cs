using Ict.Business.Models;

namespace Ict.Business.Interfaces
{
    public interface IIctTransformationService
    {
        IncrementalDataFile ReadIncrementalCsvData(string csvData);
        TriangleDataFile CreateCumulativeData(IncrementalDataFile incrementalDataFile);
    }
}