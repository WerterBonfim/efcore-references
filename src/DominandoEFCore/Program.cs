using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using DominandoEFCore.Sessoes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DominandoEFCore
{
    internal class Program
    {

        //https://github.com/JonPSmith/EfCore.TestSupport
        private static void Main(string[] args)
        {
            //EnsureCreatedAndDeleted();
            //GapDoEnsureCreated();
            //HealthCheckBancoDeDados();

            //BenchmarkRunner.Run<Monitoramento>();

            //GerenciamentoConexao();

            //ExecuteSQL();
            //SqlInjection();

            //MigracoesPendentes();
            //AplicarMigraçãoEmTempoDeExecução();
            //TodasMigracoes();
            //MigracoesJaAplicadas();
            //ScriptGeralBancoDeDados();

            // Sessão 04 - tipos de carregamento
            
            //Sessao04TiposDeCarregamento.CarregamentoAdiantado();
            //Sessao04TiposDeCarregamento.CarregamentoExplicito();
            //Sessao04TiposDeCarregamento.CarregamentoLento();

            // Sessão 05 - Consultas
            //IgnoreFiltroGlobal();
            //ConsultasProjetadas();
            //ConsultaParmetrizada();
            //ConsultaInterpolada();
            //ConsultaComTAG();
            //EntendendoConsultas1NN1();
            //DivisaoDeConsulta();


            // Sessão 06 - Stored Procedure
            //CriarStoredProcedure();
            //InserirDadosViaProcedure();
            //CriarStoredProcedureDeConsulta();
            //ConsultaViaProcedure();

            // Sessão 07 - Infraestrutura
            //ConsultarDepartamentos();
            // DadosSensiveis();
            //HabilitandoBatchSize();
            //TempoCommandoGeral2();
            //ExecutarEstrategiaResiliencia();

            // Sessão 08 - Modelo de dados
            //Collections();
            //ProgagarDados();
            //Esquema();
            //ConversorDeValor();
            //ConversorCustomizado();
            //ShadowProperty();
            //TrabalhandoComPropriedadesDeSombra();
            // Muito importante, muito bom
            //TiposDePropriedades();

            //Relacionamento1Para1();
            //Relacionamento1ParaMuitos();
            //RelacionamentoMuitosParaMuitos();

            //CampoDeApoio();
            //ExemploTPH_TPT();
            //SacolaDePropriedades();
            
            //Sessao09DataAnnotations.ExecutarExemplos();
            Sessao10EFFunctions.ExecutarExemplos();
        }

      

      


    }
}