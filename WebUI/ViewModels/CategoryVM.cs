using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;

namespace WebUI.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public static explicit operator Category(CategoryVM objVM)
        {
            Category obj = new Category() {Id = objVM.Id, Title = objVM.Title};

            return obj;
        }
    }
}
