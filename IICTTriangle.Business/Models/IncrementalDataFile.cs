using System.Collections.Generic;
using IctTriangle.Business.Enums;
using IctTriangle.Business.Interfaces;

namespace IctTriangle.Business.Models
{
    /// <summary>
    /// This is a abstract base class wrapper over <see cref="IIncrementalRecordProvider"/>
    /// </summary>
    public abstract class IncrementalDataFile
    {
        public List<IncrementalRecord> Claims { get; protected set; }

        public List<string> Products { get; protected set; }
        /// <summary>
        /// Is file data valid for further processing
        /// </summary>
        public bool IsValid { get; protected set; }

        public IncrementalDataFileStatus Status { get; protected set; }

        public int EarliestOriginYear { get; protected set; }

        public int DevelopmentYears { get; protected set; }
    }
}
