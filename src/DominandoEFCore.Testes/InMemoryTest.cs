using System;
using System.Linq;
using DominandoEFCore.Testes.Data;
using DominandoEFCore.Testes.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DominandoEFCore.Testes
{
    public class InMemoryTest
    {
        [Fact]
        public void DeveInserirUmDepartamento()
        {
            // Arg
            var departamento = new Departamento
            {
                Descricao = "Tecnologia",
                DataCadastro = DateTime.Now
            };

            // Setup
            var context = CreateContext();
            context.Departamentos.Add(departamento);
            
            // Act
            var inseridos = context.SaveChanges();

            // Assert
            Assert.Equal(1, inseridos);

        }

        [Fact]
        public void NaoImplementadaAFuncaoDeDadaParaOProviderInMemory()
        {
            var context = CreateContext();

            Action acao = () => context
                .Departamentos
                .First(x => EF.Functions.DateDiffDay(DateTime.Now, x.DataCadastro) > 0);

            Assert.Throws<InvalidOperationException>(acao); 
        }
        
        private ApplicationContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("InMemoryTeste")
                .Options;

            return new ApplicationContext(options);
        }
    }
}