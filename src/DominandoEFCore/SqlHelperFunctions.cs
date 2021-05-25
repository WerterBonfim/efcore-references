using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

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

        [DbFunction(name: "LEFT", IsBuiltIn = true)]
        public static string Left(string dados, int quantidade)
        {
            throw new NotImplementedException();
        }
    }
}