using Ict.Business.Models;

namespace Ict.Business.Interfaces
{
    public interface IIncrementalReaderService
    {
        IncrementalDataFile ReadCsvData(string csvData);
    }
}
