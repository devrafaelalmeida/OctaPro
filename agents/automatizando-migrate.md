Preciso criar uma rotina para aplicar as migrations do EF Core (AppDbContext, 
Postgres) em TODOS os bancos de todos os tenants cadastrados no tenants.db, 
de uma vez só. As Fases 1, 2 e 3 do sistema multi-tenant já estão prontas: 
TenantDbContext, ITenantRepository (com GetByDomainAsync), ITenantContext, 
TenantResolutionMiddleware e AppDbContext com conexão dinâmica por tenant, 
todos funcionando.

Antes de criar qualquer coisa, revise a estrutura atual do projeto: onde 
fica o ITenantRepository, o TenantDto, o AppDbContext, e o padrão de nomes 
de pastas para esse tipo de rotina utilitária/administrativa (ex: se já 
existe uma pasta Commands, Jobs, Scripts, ou similar; se não existir, 
sugira uma pasta apropriada seguindo a convenção do projeto).

Tarefas:

1. Se ainda não existir, adicionar o método GetAllAsync() na interface 
   ITenantRepository e na implementação EfTenantRepository:
   Task<IEnumerable<TenantDto>> GetAllAsync();
   Deve retornar todos os tenants com Ativo = true, mapeados para TenantDto 
   (mesma lógica de mapeamento já usada em GetByDomainAsync), usando 
   AsNoTracking().

2. Criar uma classe TenantMigrationRunner com um método estático (ou de 
   instância, dependendo do padrão do projeto) assíncrono, por exemplo:
   
   public static async Task RunAsync(IServiceProvider services)
   
   Esse método deve:
   
   a) Resolver o ITenantRepository via services e chamar GetAllAsync() para 
      obter a lista de tenants ativos.
   
   b) Logar no console quantos tenants foram encontrados, ex: 
      Console.WriteLine($"[MigrationRunner] {tenants.Count()} tenant(s) encontrado(s).")
   
   c) Para CADA tenant, dentro de um try/catch individual (para que uma 
      falha em um tenant não interrompa o processamento dos demais):
      
      - Logar no console o início do processamento, ex: 
        Console.WriteLine($"[MigrationRunner] Iniciando migration para tenant '{tenant.ConnectionName}' (domain: {tenant.Domain})...")
      
      - Montar a connection string do Postgres daquele tenant usando 
        NpgsqlConnectionStringBuilder (Host = tenant.DataSource, 
        Database = tenant.Database, Username = tenant.Username, 
        Password = tenant.Password) — reaproveitar a mesma lógica de 
        BuildTenantConnectionString já criada na Fase 3, se possível 
        extraindo para um método compartilhado/estático reutilizável ao 
        invés de duplicar código.
      
      - Criar uma instância de AppDbContext manualmente usando 
        DbContextOptionsBuilder<AppDbContext> com UseNpgsql(connectionString), 
        SEM passar pelo container de DI padrão (já que não há tenant 
        resolvido por requisição neste contexto).
      
      - Chamar await context.Database.MigrateAsync() nessa instância.
      
      - Se aplicar com sucesso, logar: 
        Console.WriteLine($"[MigrationRunner] Tenant '{tenant.ConnectionName}': migrations aplicadas com sucesso.")
      
      - No catch, capturar a exceção, logar no console o erro completo 
        (mensagem e, se fizer sentido, o tipo da exceção) de forma clara, 
        incluindo qual tenant falhou, ex: 
        Console.WriteLine($"[MigrationRunner] ERRO ao aplicar migration no tenant '{tenant.ConnectionName}': {ex.Message}")
        
        e SEGUIR para o próximo tenant do loop (não relançar a exceção, 
        não interromper o processamento dos demais).
      
      - Fazer dispose do DbContext criado manualmente ao final do 
        processamento de cada tenant (using/await using).
   
   d) Ao final do loop, logar um resumo simples, ex: quantos tenants 
      tiveram sucesso e quantos falharam (pode usar contadores simples 
      incrementados no try e no catch).

3. No Program.cs, ANTES da linha "var app = builder.Build();" não é 
   necessário mexer. Depois de "var app = builder.Build();", adicionar uma 
   verificação de argumento de linha de comando logo no início do arquivo 
   (ou próximo ao bloco de seeding removido na Fase 3), do tipo:
   
   if (args.Contains("migrate-tenants"))
   {
       using var scope = app.Services.CreateScope();
       await TenantMigrationRunner.RunAsync(scope.ServiceProvider);
       return;
   }
   
   Isso deve rodar a rotina e ENCERRAR a aplicação (return, sem chamar 
   app.Run()), sem subir o servidor web, quando o app for iniciado com 
   `dotnet run -- migrate-tenants`. Posicione esse bloco depois do registro 
   de todos os services e do app.Build(), mas antes do app.Run() final, 
   respeitando a ordem que já existe no arquivo.

4. NÃO implementar ainda: nenhuma outra automação (criação de bancos, seed 
   de dados iniciais, agendamento automático). Esta tarefa é APENAS a 
   rotina de aplicar migrations em todos os tenants sob demanda, via 
   comando manual.

Ao final, rode `dotnet build` para confirmar que compila sem erros, e me 
mostre um resumo dos arquivos criados/modificados, além de como rodar o 
comando (ex: dotnet run -- migrate-tenants).