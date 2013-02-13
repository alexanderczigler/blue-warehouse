using System.Linq;

namespace BlueWarehouse
{
    public static class StringExtensions
    {
        public static string Replace(this string s, string[] oldValues, string newValue)
        {
            return oldValues.Aggregate(s, (current, oldValue) => current.Replace(oldValue, newValue));
        }
    }
}
