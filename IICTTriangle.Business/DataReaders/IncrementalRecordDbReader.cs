using System;
using System.Collections.Generic;
using Ict.Business.Interfaces;
using Ict.Business.Models;

namespace Ict.Business.DataReaders
{
    public class IncrementalRecordDbReader : IIncrementalRecordProvider
    {
        public IIncrementalRecordValidator RecordValidator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool HasErrors => throw new NotImplementedException();

        public IEnumerable<IncrementalRecord> GetRecords()
        {
            throw new NotImplementedException();
        }
    }
}
