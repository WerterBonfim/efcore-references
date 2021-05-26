using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DominandoEFCore
{
    public class SqlHelperFunctions
    {
        public static void Registrar(ModelBuilder builder)
        {
            var funcoes = typeof(SqlHelperFunctions)
                .GetMethods()
                .Where(x => MetodoFoiDefinidoComAtributoDbFunction(x));

            foreach (var funcao in funcoes)
                builder.HasDbFunction(funcao);
        }
        
        private static bool MetodoFoiDefinidoComAtributoDbFunction(MethodInfo informacoesDoMetodo)
        {
            return Attribute
                .IsDefined(informacoesDoMetodo, typeof(DbFunctionAttribute));
        }

        public static void RegistrarViaFluentAPI(ModelBuilder builder)
        {
            builder
                .HasDbFunction(ObterMethodInfo(nameof(DateDiff)))
                .HasName("DATEDIFF")
                .HasTranslation(x => DateDiffTranslation(x))
                .IsBuiltIn();
        }

        private static MethodInfo ObterMethodInfo(string nomeDoMetodo)
        {
            return typeof(SqlHelperFunctions)
                .GetMethod(nomeDoMetodo);
        }

        
        
        [DbFunction(name: "LEFT", IsBuiltIn = true)]
        public static string Left(string dados, int quantidade)
        {
            throw new NotImplementedException();
        }

        [DbFunction(name:"ConverterParaLetrasMaiusculas",  Schema = "dbo")]
        public static string LetrasMaiusculas(string dados)
        {
            throw new NotImplementedException();
        }
        
        
        
        public static int DateDiff(string identificador, DateTime dataInicial, DateTime dataFinal)
        {
            throw new NotImplementedException();
        }
        
        public static SqlExpression DateDiffTranslation(IReadOnlyCollection<SqlExpression> arguments)
        {
            var args = arguments.ToArray();
            var datepart = (SqlConstantExpression) args[0];
            var newArgs = new[]
            {
                new SqlFragmentExpression(datepart.Value.ToString()),
                args[1],
                args[2]
            };

            return new SqlFunctionExpression(
                "DATEDIFF",
                newArgs,
                false,
                new[] {false, false, false},
                typeof(int),
                null
            );
        }
    }
}