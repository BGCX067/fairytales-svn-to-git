using System;
using System.IO;

namespace Web.FairyTales
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Fairy Tales" + VersionNumber;   
        }

        protected string VersionNumber
        {
            get
            {
                if (ViewState["version"] == null)
                {
                    FileInfo fi = new FileInfo(Server.MapPath("build.number"));
                    if (!fi.Exists)
                        throw new FileNotFoundException(Server.MapPath("build.number"));
                    using (StreamReader reader = new StreamReader(fi.OpenRead()))
                    {
                        ViewState["version"] = reader.ReadLine();
                    }
                }
                return ViewState["version"] as string;
            }
        }
        protected string Disclaimer { get { return "© 2007-2008 The ConeFabric"; } }
    }
}
