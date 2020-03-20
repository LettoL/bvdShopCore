using Microsoft.AspNetCore.Http;

namespace WebUI.ViewModels
{
    public class ProductsFileVM
    {
        public int ShopId { get; set; }
        public IFormFile File { get; set; }
    }
}
