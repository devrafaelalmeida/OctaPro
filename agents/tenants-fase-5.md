Preciso aplicar criptografia (SQLCipher) no tenants.db (SQLite), que hoje 
guarda credenciais de conexão dos tenants em texto puro. As Fases 1 a 3 do 
sistema multi-tenant já estão prontas: TenantDbContext, ITenantRepository, 
EfTenantRepository, ITenantContext, TenantResolutionMiddleware, AppDbContext 
com conexão dinâmica, TenantMigrationRunner e TenantMigrationInspector.

O tenants.db atual será recriado do zero (dados são apenas de teste, não 
precisam ser preservados).

A chave de criptografia (TENANT_DB_KEY) já foi adicionada ao .env do 
projeto e será carregada pelo EnvFileLoader já existente, seguindo o mesmo 
padrão usado para as demais variáveis de ambiente do projeto (ex: como 
CORS_ALLOWED_ORIGINS é lida via GetRequiredConfigurationValue).

Antes de alterar qualquer coisa, revise o Program.cs atual e a função local 
GetRequiredConfigurationValue já existente, para reaproveitá-la.

Tarefas:

1. Remover o pacote padrão do provider SQLite (o que vier por dependência 
   transitiva de Microsoft.EntityFrameworkCore.Sqlite como 
   SQLitePCLRaw.bundle_e_sqlite3, se estiver referenciado explicitamente no 
   .csproj) e adicionar o pacote:
   
   dotnet add package SQLitePCLRaw.bundle_e_sqlcipher
   
   Manter Microsoft.EntityFrameworkCore.Sqlite como está (continua 
   necessário para a integração com EF Core; o que muda é apenas o provider 
   nativo usado por baixo).

2. No Program.cs, logo no início do arquivo (antes de qualquer uso de 
   DbContext, idealmente logo após EnvFileLoader.Load()), adicionar a 
   inicialização do provider SQLCipher:
   
   SQLitePCL.Batteries_V2.Init();
   
   Isso precisa rodar antes de qualquer conexão SQLite ser aberta, para que 
   o provider correto (sqlcipher) seja usado ao invés do padrão.

3. Ler a chave mestra de criptografia usando a mesma função local já 
   existente GetRequiredConfigurationValue(builder.Configuration, 
   "TENANT_DB_KEY") — não criar um novo mecanismo de leitura, reaproveitar 
   o padrão já usado no arquivo para outras variáveis de ambiente. Fazer 
   essa leitura próximo de onde já existe o registro do TenantDbContext.

4. Ajustar o registro de TenantDbContext no Program.cs para incluir a senha 
   de criptografia na connection string, usando SqliteConnectionStringBuilder 
   (namespace Microsoft.Data.Sqlite) para montar a string, ao invés de 
   concatenar manualmente. Exemplo do padrão esperado (adaptar ao que já 
   existe no arquivo):
   
   var tenantDbPath = builder.Configuration.GetConnectionString("TenantDb") 
       ?? "Data Source=tenants.db";
   var tenantDbKey = GetRequiredConfigurationValue(builder.Configuration, "TENANT_DB_KEY");
   
   var tenantConnStringBuilder = new SqliteConnectionStringBuilder(tenantDbPath)
   {
       Password = tenantDbKey
   };
   
   builder.Services.AddDbContext<TenantDbContext>(options =>
       options.UseSqlite(tenantConnStringBuilder.ConnectionString)
   );

5. Verificar se TenantMigrationRunner e TenantMigrationInspector abrem 
   alguma conexão direta ou adicional com o tenants.db (não deveriam, eles 
   trabalham com o AppDbContext do Postgres) — se não abrirem, não precisa 
   alterar nada neles.

6. Confirmar que .env.example (ou arquivo similar de exemplo, se existir no 
   projeto) recebe uma entrada TENANT_DB_KEY= (vazia ou com placeholder tipo 
   "sua-chave-aqui"), para documentar a variável exigida sem expor o valor 
   real.

7. NÃO alterar a lógica de negócio de tenant resolution, cache, ou qualquer 
   outra fase já implementada — esta tarefa é EXCLUSIVAMENTE sobre 
   criptografar a conexão com o tenants.db.

Ao final, rode dotnet build para confirmar que compila sem erros. NÃO rode 
migrations nem tente conectar ainda — apenas garanta que compila. Mostre um 
resumo dos arquivos modificados.