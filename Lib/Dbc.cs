using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignByContract
{
    public static class Dbc
    {
        public static Ander Requires(bool condition, string message = null)
        {
            if (!condition)
            {
                throw new DbcException("Failed Requires test. " + message ?? "");
            }
            return new Ander();
        }

        public class Ander
        {
            public Ander And(bool condition, string message = null)
            {
                Dbc.Requires(condition, message);
                return this;
            }
        }
    }
}
