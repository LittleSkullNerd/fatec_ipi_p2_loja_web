using Domain.Core;
using Domain.Models;
using Infra.Repository;
using Infra.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Loja_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IUnitOfWork uow, IProdutoRepository produto)
        {
            _uow = uow;
            _produtoRepository = produto;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _produtoRepository.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = _produtoRepository.GetById(id);

            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Produto request)
        {
            var produto = new Produto
            {
                Codigo = request.Codigo,
                ImagemProduto = request.ImagemProduto,
                Descritivo = request.Descritivo,
                Destaque = request.Destaque,
                Estoque = request.Estoque,
                Nome = request.Nome,
                ProdutoId = request.ProdutoId,
                Valor = request.Valor,
                ValorPromo = request.ValorPromo
            };

            _produtoRepository.Add(produto);
            await _uow.SaveChangesAsync();

            var response = new Produto()
            {
                ValorPromo = produto.ValorPromo,
                Valor = produto.Valor,
                ProdutoId=produto.ProdutoId,
                ImagemProduto = produto.ImagemProduto,
                Nome = produto.Nome,
                Estoque=produto.Estoque,
                Destaque=produto.Destaque,
                Descritivo=produto.Descritivo,
                Codigo = produto.Codigo                
            };

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Produto request)
        {
            var existingProduto = _produtoRepository.GetById(request.ProdutoId);
            if (existingProduto == null)
            {
                return NotFound();
            }

            existingProduto.Codigo = request.Codigo;
            existingProduto.ImagemProduto = request.ImagemProduto;
            existingProduto.Descritivo = request.Descritivo;
            existingProduto.Destaque = request.Destaque;
            existingProduto.Estoque = request.Estoque;
            existingProduto.Nome = request.Nome;
            existingProduto.Valor = request.Valor;
            existingProduto.ValorPromo = request.ValorPromo;

            _produtoRepository.Update(existingProduto);
            await _uow.SaveChangesAsync();

            var response = new Produto()
            {
                ValorPromo = existingProduto.ValorPromo,
                Valor = existingProduto.Valor,
                ProdutoId = existingProduto.ProdutoId,
                ImagemProduto = existingProduto.ImagemProduto,
                Nome = existingProduto.Nome,
                Estoque = existingProduto.Estoque,
                Destaque = existingProduto.Destaque,
                Descritivo = existingProduto.Descritivo,
                Codigo = existingProduto.Codigo
            };

            return Ok(response);
        }

        [HttpDelete("{pId}")]
        public async Task<IActionResult> Delete(int pId)
        {
            var produto = _produtoRepository.GetById(pId);
            if (produto == null)
            {
                return NotFound();
            }

            _produtoRepository.Remove(produto);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

    }
}
