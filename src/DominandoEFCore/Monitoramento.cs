using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore
{
    [SimpleJob(RunStrategy.Monitoring, targetCount: 2, id: "EFCore Gereciamento")]
    [MinColumn]
    [Q1Column]
    [Q3Column]
    [MaxColumn]
    public class Monitoramento
    {
        private static int _count;


        [Benchmark]
        public void SemGerenciamentoDaConexao()
        {
            using var db = new ApplicatonContextSemLog();

            var conexao = db.Database.GetDbConnection();

            conexao.StateChange += (_, __) => ++_count;

            for (var i = 0; i < 200; i++)
                db.Departamentos.AsNoTracking().Any();
        }

        [Benchmark]
        public void ComGerenciamentoDaConexao()
        {
            using var db = new ApplicatonContextSemLog();

            var conexao = db.Database.GetDbConnection();

            conexao.StateChange += (_, __) => ++_count;
            conexao.Open();

            for (var i = 0; i < 200; i++)
                db.Departamentos.AsNoTracking().Any();
        }
    }
}