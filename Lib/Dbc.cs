using System.Collections;
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
                Requires(condition, message);
                return this;
            }
        }

        public static Ander NotNull(object value, string message = null)
        {
            Requires(value != null, message);
            return new Ander();
        }
        
        public static Ander NotNull(object[] values, string message = null)
        {
            values.ToList().ForEach(v => Requires(v != null, message));
            return new Ander();
        }

        public static void NotNullOrEmpty(string[] values, string message = null)
        {
            values.ToList().ForEach(v => NotNullOrEmpty(v, message));
        }

        public static void NotNullOrEmpty(IEnumerable collection, string message = null)
        {
            NotNull(collection, message);
            var generic = collection.Cast<object>();
            Requires(generic.Any(), message);
        }
    }
}
