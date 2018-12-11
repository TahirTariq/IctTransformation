using System.Collections.Generic;
using Ict.Business.Models;

namespace Ict.Business.Interfaces
{
    public interface IIncrementalRecordProvider
    {
        IEnumerable<IncrementalRecord> GetRecords();
        IIncrementalRecordValidator RecordValidator { get; set; }
        bool HasErrors { get; }
    }
}
