Preciso implementar a Fase 3 de um sistema multi-tenant (database-per-tenant) 
na minha aplicação ASP.NET Core Web API (.NET 10). As Fases 1 e 2 já estão 
prontas: existe TenantDbContext (SQLite) com metadados de tenants, 
ITenantRepository, ITenantContext (Scoped) e TenantResolutionMiddleware, 
todos já registrados e funcionando no Program.cs. Já existe também um 
controller de debug com o endpoint GET /api/debug/tenant-atual, criado na 
Fase 2.

O objetivo desta fase é fazer o AppDbContext (Postgres, já existente, usado 
pelo Identity e pelo restante da aplicação) resolver sua connection string 
dinamicamente, baseado no tenant resolvido pelo middleware, ao invés de usar 
uma connection string fixa vinda do .env.

Antes de alterar qualquer coisa, revise o Program.cs atual (já anexo ao 
contexto do projeto) para entender o registro atual do AppDbContext e a 
função BuildConnectionString, e localize o controller de debug já existente 
com o endpoint tenant-atual.

Tarefas:

1. No Program.cs, substituir o registro atual do AppDbContext:
   builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseNpgsql(BuildConnectionString(builder.Configuration))
   );
   
   Pela versão que resolve a connection string via ITenantContext, usando a 
   sobrecarga de AddDbContext que recebe (IServiceProvider, 
   DbContextOptionsBuilder):
   
   builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
   {
       var tenantContext = serviceProvider.GetRequiredService<ITenantContext>();
       var tenant = tenantContext.Current
           ?? throw new InvalidOperationException("Tenant não resolvido para esta requisição.");
       options.UseNpgsql(BuildTenantConnectionString(tenant));
   });

2. Substituir a função local BuildConnectionString(IConfiguration) por 
   BuildTenantConnectionString(TenantDto tenant), que monta a connection 
   string usando NpgsqlConnectionStringBuilder com Host = tenant.DataSource, 
   Database = tenant.Database, Username = tenant.Username, 
   Password = tenant.Password. Remover a função antiga e a chamada a 
   GetRequiredConfigurationValue para DB_HOST/DB_PORT/DB_NAME/DB_USER/DB_PASSWORD, 
   já que não são mais usadas.

3. Remover do .env (ou de onde EnvFileLoader carrega) as chaves: 
   DefaultConnection, DB_HOST, DB_PORT, DB_NAME, DB_USER, DB_PASSWORD — 
   caso existam em um arquivo .env.example ou similar no repositório, 
   remova de lá também.

4. Remover o bloco de seeding automático no Program.cs (o bloco 
   `using (var scope = app.Services.CreateScope())` que chama 
   CorporationSeeder, RoleSeeder, PermissionSeeder e AdminUserSeeder), pois 
   ele rodava fora do contexto de uma requisição HTTP e agora o AppDbContext 
   depende do ITenantContext, que só é populado pelo TenantResolutionMiddleware 
   durante uma requisição real. NÃO delete as classes de seeder em si 
   (CorporationSeeder, RoleSeeder, etc.) — apenas remova a chamada automática 
   no startup. Deixe um comentário no lugar removido explicando que o seed 
   agora deve ser executado manualmente por tenant (será tratado em uma fase 
   futura).

5. Confirme que a ordem de middlewares no pipeline continua correta: 
   TenantResolutionMiddleware precisa rodar ANTES de qualquer código que 
   resolva o AppDbContext (isso já deveria estar certo, mas valide).

6. No MESMO controller de debug já existente (onde está o endpoint 
   GET /api/debug/tenant-atual da Fase 2), adicionar um novo endpoint:
   
   GET /api/debug/teste-conexao-tenant
   
   Este endpoint deve:
   - Injetar o AppDbContext via construtor (não via método), seguindo o 
     mesmo padrão de injeção já usado nos outros controllers do projeto
   - Tentar abrir a conexão com o banco (ex: via 
     await appDbContext.Database.CanConnectAsync())
   - Se conectar com sucesso, retornar 200 com um objeto JSON simples 
     contendo: o nome do banco conectado (tenant.Database, via 
     ITenantContext), e a mensagem "Conexão estabelecida com sucesso."
   - Se falhar ao conectar, capturar a exceção e retornar 500 com uma 
     mensagem de erro simples (sem vazar detalhes sensíveis como senha na 
     resposta, mas pode logar a exceção completa via ILogger se o projeto 
     já usar logging)
   - Adicionar o mesmo comentário // TODO: remover este endpoint após 
     validar a Fase 3, seguindo o padrão já usado no endpoint da Fase 2

7. NÃO implementar ainda: cache de tenant (Fase 4), criptografia do 
   tenants.db (Fase 5), qualquer automação de criação de banco por tenant 
   ou seed automatizado (fases futuras).

Ao final, rode `dotnet build` para confirmar que compila sem erros, e mostre 
um resumo dos arquivos modificados.