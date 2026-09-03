Preciso implementar a Fase 2 de um sistema multi-tenant (database-per-tenant) 
na minha aplicação ASP.NET Core Web API (.NET 10). A Fase 1 já está pronta: 
existe um TenantDbContext (SQLite) com uma entidade Tenant e a tabela já 
populada com tenants de teste.

Antes de criar qualquer coisa, inspecione a estrutura atual do projeto: 
onde ficam os DTOs (o projeto já segue o padrão de usar DTOs, não expor 
entities do EF diretamente), onde ficam interfaces/repositórios, como é o 
padrão de registro de Scoped/Singleton no Program.cs, e onde ficam os 
middlewares customizados, se já existir algum. Siga a mesma convenção.

IMPORTANTE: em nenhum momento a entity `Tenant` (do EF Core) deve vazar 
para fora da camada de acesso a dados. Toda a aplicação (middleware, 
TenantContext, controllers) deve trabalhar exclusivamente com um DTO.

Tarefas:

1. Criar um `TenantDto` (seguindo a convenção de nomenclatura/pasta de DTOs 
   já usada no projeto) com as propriedades: Domain, ConnectionName, 
   DataSource, Database, Username, Password. Não incluir Id, Ativo ou 
   CriadoEm no DTO — esses campos são detalhe de persistência, não 
   necessários para o resto da aplicação.

2. Criar a interface `ITenantRepository` com o método:
   Task<TenantDto?> GetByDomainAsync(string domain);

3. Criar `EfTenantRepository : ITenantRepository`, usando o TenantDbContext 
   já existente. A query deve filtrar por Domain e por Ativo = true, usar 
   AsNoTracking(), e mapear a entity Tenant para TenantDto antes de 
   retornar (nunca retornar a entity diretamente).

4. Criar a interface `ITenantContext` com:
   - TenantDto? Current { get; }
   - void SetTenant(TenantDto tenant);

5. Criar `TenantContext : ITenantContext` (implementação simples, guarda o 
   TenantDto atual em uma propriedade).

6. Criar `TenantResolutionMiddleware`:
   - Lê context.Request.Host.Host
   - Chama ITenantRepository.GetByDomainAsync(host)
   - Se retornar null, responde com status 404 e uma mensagem simples 
     ("Tenant não encontrado."), e interrompe o pipeline (não chama _next)
   - Se encontrar, chama tenantContext.SetTenant(dto) e chama _next(context)

7. Registrar no Program.cs:
   - builder.Services.AddScoped<ITenantRepository, EfTenantRepository>();
   - builder.Services.AddScoped<ITenantContext, TenantContext>();
   - app.UseMiddleware<TenantResolutionMiddleware>(); — posicione isso 
     logo após middlewares de infraestrutura básica (exception handling, 
     HTTPS redirection, etc.) e antes de qualquer middleware/endpoint que 
     dependa do tenant resolvido (ex: antes de MapControllers ou 
     UseAuthorization, se existir).

8. Criar um endpoint de debug temporário GET /api/debug/tenant-atual que 
   injeta ITenantContext e retorna o TenantDto atual (ou 404 se nulo). 
   Deixe um comentário // TODO: remover este endpoint após validar a Fase 2 
   acima dele.

9. NÃO implementar ainda: conexão dinâmica com o banco Postgres do cliente, 
   cache de tenant (isso é Fase 4), criptografia do tenants.db (Fase 5).

Ao final, rode `dotnet build` para confirmar que compila sem erros, e me 
mostre um resumo dos arquivos criados/modificados.