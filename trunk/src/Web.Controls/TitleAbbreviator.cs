

namespace Conefabric.FairyTales.Web.Controls
{
    public class TitleAbbreviator
    {
        private readonly int maxlenght;

        public TitleAbbreviator(int maxlenght)
        {
            this.maxlenght = maxlenght;
        }

        public string Abbreviate(string what)
        {
            if (what.Length <= maxlenght)
                return what;

            var to = maxlenght;
            for (var i = maxlenght; i >= 0; --i)
            {
                if (!Equals(what[i], ' '))
                    continue;
                to = i;
                break;
            }

            return string.Format("{0} ...", what.Substring(0, to));

        }
    }
}