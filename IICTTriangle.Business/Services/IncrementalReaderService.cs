using Ict.Business.DataReaders;
using Ict.Business.Interfaces;
using Ict.Business.Models;

namespace Ict.Business.Services
{
    public class IncrementalReaderService : IIncrementalReaderService
    {
        public IncrementalDataFile ReadCsvData(string csvData)
        {
            IIncrementalRecordProvider csvReader = new IncrementalRecordCsvReader(csvData);

            return new IncrementalDataFileGenerator(csvReader);
        }
    }
}
