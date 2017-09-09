using ApiView.Code;
using ApiView.OpenApi;
namespace ApiView.Models
{
    public class ApiDetailsModel
    {
        public Api api { get; set; }

        public OpenDocAttribute doc { get; set; }

    }
}