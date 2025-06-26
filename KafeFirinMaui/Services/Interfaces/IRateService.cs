using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services.Interfaces
{
    public interface IRateService
    {
        Task<List<Rates>> GetRatesByEmployeeIDAsync(int employeeId);
        Task<bool> SaveRateAsync(Rates rate);
    }
}
