using BaseApp.Business.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Business.ViewModels.VO
{
    public class BusinessLocationInfo: BusinessLocation
    {
        public string? BoxInfo { get; set; }
    }
}
