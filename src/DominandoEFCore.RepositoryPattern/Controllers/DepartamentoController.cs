using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominandoEFCore.RepositoryPattern.Data;
using DominandoEFCore.RepositoryPattern.Data.Repositories;
using DominandoEFCore.RepositoryPattern.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.RepositoryPattern.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly ILogger<DepartamentoController> _logger;
        private readonly IUnitOfWork _uow;

        

        public DepartamentoController(
            ILogger<DepartamentoController> logger, 
            IDepartamentoRepository departamento,
            IUnitOfWork uow
            )
        {
            _logger = logger;
            _uow = uow;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var departamento = await _uow.DepartamentoRepository.GetByIdAsync(id);
            return Ok(departamento);
        }

        [HttpPost]
        public IActionResult CriarDepartamento([FromBody]Departamento departamento)
        {
            _uow.DepartamentoRepository.Add(departamento);
            
            var salvou = _uow.Commit();

            return Ok(departamento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverDepartamento(int id)
        {
            var departamento = await _uow.DepartamentoRepository.GetByIdAsync(id);
            
            _uow.DepartamentoRepository.Remove(departamento);

            _uow.Commit();

            return Ok(departamento);
        }
        
        //api/departamentos?descricao=teste
        [HttpGet]
        public async Task<IActionResult> Consultar([FromQuery] string descricao)
        {
            var departamentos = await _uow.DepartamentoRepository
                .GetDataAsync(
                    x => x.Descricao.Contains(descricao),
                    x => x.Include(y => y.Colaboradores)
                    );

            return Ok(departamentos);

        }
    }
}