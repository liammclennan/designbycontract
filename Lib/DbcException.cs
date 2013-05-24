using System;

namespace DesignByContract
{
    public class DbcException : Exception
    {
        public DbcException(string message)
            : base(message)
        {
        }

        public DbcException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}