using ConeFabric.FairyTales.Core;

namespace Conefabric.FairyTales.Web.Controls
{
    public interface IBacklogView
    {
        Project[] Projects { set; }
        Project ActiveProject { set; }
        string ErrorMessage { set; }
        bool CreateProjectControlsVisible { get; set; }
        void Edit();
        void StopEditing();
        bool IsEditing { get; }
    }
}
