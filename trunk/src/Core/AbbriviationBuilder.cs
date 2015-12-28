using System;
using System.Text;

namespace ConeFabric.FairyTales.Core
{
    public static class AbbriviationBuilder
    {
        public static string GetAbbriviation(string from)
        {
            if (from == null)
                throw new ArgumentException("Can't create an abbreviation on <null>");
            
            from = from.Replace("-", string.Empty).Replace("  ", " ");

            if (from.Length < 3)
                return from.ToUpper();

            var tmp = from.Split(new[] { ' ' });
            if (tmp.Length < 2)
                return from.Substring(0, 3).ToUpper();
            
            var builder = new StringBuilder();
            foreach (var x in tmp)
            {
                int number;
                if (Int32.TryParse(x, out number))
                    builder.Append(number);
                else
                    builder.Append(x[0]);
            }
            return builder.ToString().ToUpper();
        }
    }
}
