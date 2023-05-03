using UniWeb.API.Entities;

namespace UniWeb.API.DataServices
{
    public interface IAdminDataServices
    {

        Admin GetAdminDetails(int id);
    }
}
