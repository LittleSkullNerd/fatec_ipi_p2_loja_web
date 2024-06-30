using Domain.Core;
using Domain.Models;
using Domain.ViewModel;
using Infra.Repository;
using Infra.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Loja_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IClientRepository _clientRepository;

        public ClientController(IUnitOfWork uow, IClientRepository client)
        {
            _uow = uow;
            _clientRepository = client;
        }

        [HttpGet("Client/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = _clientRepository.GetById(id);

            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _clientRepository.GetAll());

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Client request)
        {

            var user = await _clientRepository.GetAll();

            var foundClient = user.FirstOrDefault(w => w.Email.Equals(request.Email));

            var client = new Client
            {
                CEP = request.CEP,
                Cidade = request.Cidade,
                ClientId = request.ClientId,
                Codigo = request.Codigo,
                Complemento = request.Complemento,
                Documento = request.Documento,
                Email = request.Email,
                Estado = request.Estado,
                Logradouro = request.Logradouro,
                Nome = request.Nome,
                Telefone = request.Telefone
            };

            if (foundClient != null) return Ok(foundClient);

            _clientRepository.Add(client);
            await _uow.SaveChangesAsync();

            var response = new Client()
            {
                Codigo = client.Codigo,
                Telefone = client.Telefone,
                Nome = client.Nome,
                Logradouro = client.Logradouro,
                Estado = client.Estado,
                Email = client.Email,
                Documento = client.Documento,
                Complemento = client.Complemento,
                ClientId = client.ClientId,
                Cidade = client.Cidade,
                CEP = client.CEP
            };

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            var client = await _clientRepository.GetAll();

            var foundClient = client.FirstOrDefault(w => w.Email.Equals(request.Email));

            if (foundClient == null)
            {
                return NotFound();
            }

            return Ok(foundClient);

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Client request)
        {
            var existingUser = _clientRepository.GetById(request.ClientId);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.CEP = request.CEP;
            existingUser.Email = request.Email;
            existingUser.Nome = request.Nome;
            existingUser.Complemento = request.Complemento;
            existingUser.Documento = request.Documento;
            existingUser.Cidade = request.Cidade;
            existingUser.Telefone = request.Telefone;
            existingUser.Estado = request.Estado;
            existingUser.Logradouro = request.Logradouro;
            existingUser.Codigo = request.Codigo;


            _clientRepository.Update(existingUser);
            await _uow.SaveChangesAsync();

            var response = new Client()
            {
                Codigo = existingUser.Codigo,
                Telefone = existingUser.Telefone,
                Nome = existingUser.Nome,
                Logradouro = existingUser.Logradouro,
                Estado = existingUser.Estado,
                Email = existingUser.Email,
                Documento = existingUser.Documento,
                Complemento = existingUser.Complemento,
                ClientId = existingUser.ClientId,
                Cidade = existingUser.Cidade,
                CEP = existingUser.CEP
            };

            return Ok(response);
        }

        [HttpDelete("{pId}")]
        public async Task<IActionResult> Delete(int pId)
        {
            var produto = _clientRepository.GetById(pId);
            if (produto == null)
            {
                return NotFound();
            }

            _clientRepository.Remove(produto);
            await _uow.SaveChangesAsync();

            return NoContent();
        }
    }
}
