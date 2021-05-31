# Aplicação Multi-tenant

* 1 - Arquitetura Multi-tenant
* 2 - Single-tenant vs Multi-tenant
* 3 - Estratégias Multi-tenant
* 4 - Criando o projeto
* 5 - Instalando dependências
* 6 - Preparando o ambiente
* 7 - Estratégia 1 - Identificar na tabela
* 8 - Estratégia 2 - Schema
* 9 - Estratégia 3 - Banco de dados



## 1 - Arquitetura Multi-tenant

Multitenant (Mult-inquilino) é um estilo de arquitetura de software que utilizava uma 
única aplicação para diversos clientes(inquilinos). Esta arquitertura é muito utlizada no 
conceito de cloud--computings (SaaS) onde aplicas-se uma estratégia de isolamento dos
dados para cada inquilino.
## 2 - Single-tenant vs Multi-tenant

### Arquitetura Sigle-tenant

Tem com objetivo realizar o issolamento tanto de suas aplicações quanto de seus dados.

Vantagens:
* Fácil customização
* Segurança

Desvantagens
* Manutenção
* Custo

### Arquitetura Multi-tenant

Tem com objetivo compartilhar a mesma instância da aplicação 
para tender vários clientes (inquilinos).

Aplicação pode ser escalada com varias instancias da aplicação através do load balancer

Vantagens
* Manutenção
* Custo

Desvantagens
* Segurança
* Customização

## 3 - Estratégias Multi-tenant

Três estratégias
* Banco de dados: Vários bancos de dados acessados por uma unica aplicação.
```
tenant-a.seudominio.com
tenant-b.seudominio.com

seudominio.com/tenant-a/produtos
seudominio.com/tenant-b/produtos
```

Melhor estratégia para se utilizar por causa do LGPD

* Schema: Um unico banco de dados onde os tenant são separados
por schema
```
```

* Identificador na tabela: Um unico banco de dados com um unico schema e as
tabelas possui uma coluna adicional para indentificar o tenant.
Mais fácil, porem não muito segura.
```
```

## 4 - Criando o projeto

Projeto ASP.NET Core WEB API

## 5 - Instalando dependências

Microsoft.EntityFrameworCore.SqlServer
Microsoft.EntityFrameworCore.Design
Microsoft.EntityFrameworCore.Tools

## 6 - Preparando o ambiente

```c#
```

## 7 - Estratégia 1 - Identificar na tabela

```c#
```

## 8 - Estratégia 2 - Schema

```c#
```

## 9 - Estratégia 3 - Banco de dados

* Banco de dados: Vários bancos de dados acessados por uma unica aplicação.
```
tenant-dev.seudominio.com
tenant-a.seudominio.com
tenant-b.seudominio.com

seudominio.com/tenant-a/produtos
seudominio.com/tenant-b/produtos
```

Eu adotei a seguinte estrategia para estudar esse exemplo:
 * Defini o banco principal como tenant-dev para simplificar os testes antes de replicar para os tenants
 * O tentant-dev é criado via Factory [Design-time DbContext Creation][design-time]

```c#
public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationContext>();
        builder
            .UseSqlServer("Server=localhost, 1433;Database=tentant-dev;User Id=sa;Password=!123Senha;Application Name=\"CursoEFCore\";pooling=true;")
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging();
        
        return new ApplicationContext(builder.Options);
    }
}
```

* Cada tenant é criado desta forma:

```bash
dotnet ef database update --context ApplicationContext --connection 'Server=localhost, 1433;Database=tenant-a;User Id=sa;Password=!123Senha;Application Name="CursoEFCore";pooling=true;'
```


```c#
services.AddHttpContextAccessor();

// Toda vez que requisição precisar de um context 
services.AddScoped<ApplicationContext>(provider =>
{
    var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>();

    var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
    var tenantId = httpContext.GetTenantId();

    var stringDeConexao = Configuration.GetConnectionString(tenantId);

    optionBuilder
        .UseSqlServer(stringDeConexao)
        .LogTo(Console.WriteLine)
        .EnableSensitiveDataLogging();

    return new ApplicationContext(optionBuilder.Options);
});
```



[design-time]:https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
