using Microsoft.EntityFrameworkCore.Storage;

namespace PaparaApp.Project.API.Models.UnitOfWorks
{
    public interface IUnitOfWork
    {
        int Commit();
        IDbContextTransaction BeginTransaction();
    }
}
