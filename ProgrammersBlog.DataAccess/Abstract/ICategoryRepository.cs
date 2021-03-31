using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Shared.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.DataAccess.Abstract
{
    public interface ICategoryRepository :IEntityRepository<Category>
    {
    }
}
