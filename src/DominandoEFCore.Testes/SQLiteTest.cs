using System;
using System.Linq;
using DominandoEFCore.Testes.Data;
using DominandoEFCore.Testes.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DominandoEFCore.Testes
{
    
    public class SQLiteTest
    {
        [Theory]
        [InlineData("Tecnologia")]
        [InlineData("RH")]
        public void DeveInserirEConsultarUmDepartamento(string descricao)
        {
            // Arg
            var departamento = new Departamento
            {
                Descricao = descricao,
                DataCadastro = DateTime.Now
            };

            // Setup
            var context = CreateContext();
            context.Database.EnsureCreated();
            context.Departamentos.Add(departamento);
            
            // Act

            var inseridos = context.SaveChanges();
            var departamentoInserido = context.Departamentos
                .FirstOrDefault(x => x.Descricao == descricao);

            // Assert
            Assert.Equal(1, inseridos);
            Assert.Equal(departamento, departamentoInserido);
        }
        
        private ApplicationContext CreateContext()
        {
            // SQLite em memoria
            var conexao = new SqliteConnection("DataSource=:memory:");
            conexao.Open();
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(conexao)
                .Options;

            return new ApplicationContext(options);
        }
    }
}