using ETicaretAPI.Application.Repositories.Customer;
using ETicaretAPI.Application.Repositories.Order;
using ETicaretAPI.Application.Repositories.Product;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        readonly private ICustomerWriteRepository _customerWriteRepository;
        readonly private IOrderWriteRepository _orderWriteRepository;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, ICustomerWriteRepository customerWriteRepository, IOrderWriteRepository orderWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _customerWriteRepository = customerWriteRepository;
            _orderWriteRepository = orderWriteRepository;
        }

        [HttpGet()]
        public async Task Get()
        {
            var IDD = Guid.NewGuid();
            await _customerWriteRepository.AddAsyc(new() { Id = IDD, Name = "customer Adi" });
            await _orderWriteRepository.AddAsyc(new() { Description = "zart zurut", Address = "adresimiz 1", CustomerId = IDD });
            await _orderWriteRepository.AddAsyc(new() { Description = "zart zurut 2", Address = "adresimiz 2", CustomerId = IDD });
            await _orderWriteRepository.SaveAsync();
        }


    }
}
