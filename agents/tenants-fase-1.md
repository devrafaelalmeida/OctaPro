Preciso implementar a Fase 1 de um sistema multi-tenant (database-per-tenant) 
na minha aplicação ASP.NET Core Web API (.NET 10, EF Core já usado com PostgreSQL).

O objetivo desta fase é criar um banco de metadados SEPARADO (SQLite) que vai 
armazenar as configurações de conexão de cada tenant (cliente). Este banco é 
independente do banco de dados principal da aplicação (Postgres) e não deve 
se misturar com os DbContexts existentes.

Antes de criar qualquer coisa, inspecione a estrutura atual do projeto 
(pastas, namespaces, convenção de nomes, onde ficam os DbContexts existentes, 
padrão de configuração no Program.cs e appsettings.json) e siga o mesmo padrão.

Tarefas:

1. O pacote Microsoft.EntityFrameworkCore.Sqlite já está instalado no projeto. 
   Apenas confirme (via dotnet list package) que a versão está alinhada (10.x) 
   com o Microsoft.EntityFrameworkCore já usado para o Postgres, e que o 
   Microsoft.EntityFrameworkCore.Design também está presente (instale-o 
   apenas se estiver realmente faltando). Não reinstale o pacote do Sqlite.

2. Criar uma entidade `Tenant` com as seguintes propriedades:
   - Id (int, PK)
   - Domain (string, obrigatório, único) — domínio do cliente, ex: empresa1.sistema.com
   - ConnectionName (string, obrigatório) — nome/apelido lógico do tenant
   - DataSource (string, obrigatório) — host do servidor de banco do cliente
   - Database (string, obrigatório) — nome do banco (Postgres) do cliente
   - Username (string, obrigatório) — usuário de acesso ao banco do cliente
   - Password (string, obrigatório) — senha de acesso ao banco do cliente
   - Ativo (bool, default true)
   - CriadoEm (DateTime, default UtcNow)

3. Criar um `TenantDbContext` (separado de qualquer DbContext já existente 
   no projeto) com:
   - DbSet<Tenant> Tenants
   - Índice único na coluna Domain (via OnModelCreating)
   - Sem lógica de conexão fixa no OnConfiguring — a connection string deve 
     vir via injeção de dependência (DbContextOptions), registrada no 
     Program.cs, seguindo o mesmo padrão usado para o DbContext do Postgres 
     já existente no projeto.

4. Registrar o TenantDbContext no Program.cs usando 
   builder.Services.AddDbContext<TenantDbContext>(...), lendo a connection 
   string de uma chave chamada "TenantDb" em ConnectionStrings no 
   appsettings.json. Se a chave não existir, usar como fallback: 
   "Data Source=tenants.db"

5. Adicionar a chave "TenantDb": "Data Source=tenants.db" dentro de 
   "ConnectionStrings" no appsettings.json (ou appsettings.Development.json, 
   se for esse o padrão do projeto para configs locais).

6. Adicionar "tenants.db" e "tenants.db-*" ao .gitignore (criar a entrada 
   se ainda não existir um .gitignore, ou adicionar ao existente sem 
   duplicar entradas).

7. Gerar a migration inicial e aplicar no banco:
   dotnet ef migrations add InitTenants --context TenantDbContext
   dotnet ef database update --context TenantDbContext
   
   Use --output-dir apontando para uma pasta de Migrations específica do 
   TenantDbContext (não misturar com migrations do DbContext principal), 
   seguindo a convenção de nomes de pasta já usada no projeto para 
   organizar contexts/migrations.

8. NÃO criar ainda: middleware de resolução de tenant, TenantContext, 
   repositório, cache, ou qualquer lógica de conexão dinâmica com o banco 
   do cliente. Esta fase é APENAS a fundação do banco de metadados.

Ao final, rode `dotnet build` para confirmar que compila sem erros, e me 
mostre um resumo dos arquivos criados/modificados e a estrutura de pastas 
resultante.