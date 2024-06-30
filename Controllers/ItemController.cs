using Domain.Core;
using Domain.Models;
using Infra.Repository;
using Infra.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Loja_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IItemRepository _itemRepository;

        public ItemController(IUnitOfWork uow, IItemRepository item)
        {
            _uow = uow;
            _itemRepository = item;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _itemRepository.GetAll());

        [HttpGet("Item/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = _itemRepository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Item request)
        {
            var item = new Item
            {
                Valor = request.Valor,
                CodigoProduto = request.CodigoProduto,
                ItemId = request.ItemId,
                NomeProduto = request.NomeProduto,
                Qtd = request.Qtd,
                Total = request.Total
            };

            _itemRepository.Add(item);
            await _uow.SaveChangesAsync();

            var response = new Item()
            {
                Total = item.Total,
                Qtd = item.Qtd,
                NomeProduto= item.NomeProduto,
                ItemId= item.ItemId,
                CodigoProduto = item.CodigoProduto,
                Valor = item.Valor
            };

            return Ok(response);
        }

    }


}
