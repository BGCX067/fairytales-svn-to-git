using System.Xml.Serialization;

namespace ConeFabric.FairyTales.Core
{
    public abstract class DomainObject : IDomainObject
    {
        protected DomainObject(string name, string abbreviation)
        {
            this.name = name;
            this.abbreviation = abbreviation;
        }

        [XmlElement(Order = 1)]
        public virtual string Abbreviation
        {
            get { return abbreviation; }
            set { abbreviation = value; }
        }

        [XmlElement(Order = 2)]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string name, abbreviation;
    }
}