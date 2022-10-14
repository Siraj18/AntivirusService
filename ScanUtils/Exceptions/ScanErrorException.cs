using System;

namespace ScanUtils.Exceptions
{
    public class ScanErrorException : Exception
    {
        public ScanErrorException(string message) : base(message)
        {
            
        }
    }
}