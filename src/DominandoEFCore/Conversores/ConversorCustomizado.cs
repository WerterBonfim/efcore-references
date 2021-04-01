using System;
using System.Linq;
using System.Linq.Expressions;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DominandoEFCore.Conversores
{
    public class ConversorCustomizado : ValueConverter<Status, string>
    {
        public ConversorCustomizado() : base(
            p => ConverterParaOBancoDeDados(p),
            value => ConverterParaAplicacao(value), new ConverterMappingHints(1))
        {
        }

        static string ConverterParaOBancoDeDados(Status status) => status.ToString()[0..1];

        static Status ConverterParaAplicacao(string value) =>
            Enum.GetValues<Status>()
                .FirstOrDefault(p => p.ToString()[0..1] == value);
    }
}