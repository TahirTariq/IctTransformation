using System;

namespace IctTriangle.Business.Enums
{
    [Flags]
    public enum IncrementalDataFileStatus
    {
        NoError          = 0,
        InvalidRowsFound = 1,
        NoProductsFound  = 2
    }
}