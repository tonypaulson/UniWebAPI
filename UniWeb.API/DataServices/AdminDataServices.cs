using UniWeb.API.DataContext;
using UniWeb.API.DTO;
using UniWeb.API.Entities;

namespace UniWeb.API.DataServices
{
    public class AdminDataServices : IAdminDataServices
    {
        private EFDataContext _dataContext;

        public AdminDataServices(EFDataContext dataContext) {
            _dataContext = dataContext;
        }



        //public AdminDto UpdateAdmin(AdminDto admin)
        //{
        //    try
        //    {
        //    }
        //    catch { 


        //    }
        //}


        public Admin GetAdminDetails(int id)
        {
            try
            {
                return _dataContext.Admin.Where(x => x.Id == id).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

    }
}
