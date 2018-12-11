using System.Collections.Generic;
using IctTriangle.Business.Models;

namespace IctTriangle.Business.Interfaces
{
    public interface IIncrementalRecordProvider
    {
        IEnumerable<IncrementalRecord> GetRecords();
        IIncrementalRecordValidator RecordValidator { get; set; }
        bool HasErrors { get; }
    }
}
