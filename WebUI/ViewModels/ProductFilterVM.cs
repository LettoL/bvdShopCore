using Data.FiltrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class ProductFilterVM
    {
        public ProductFiltrationModel ProductFiltrationModel { get; set; }
        public int UserId { get; set; }
    }
}
