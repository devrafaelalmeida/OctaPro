# Codex Log

## Sumario

| Tema | Data |
| --- | --- |
| [Autenticacao, autorizacao e base ABAC](#2026-08-26-autenticacao-autorizacao-e-base-abac) | [2026-08-26](#2026-08-26-autenticacao-autorizacao-e-base-abac) |

## 2026-08-26 - Autenticacao, autorizacao e base ABAC

### Objetivo

Analisar a implementacao atual de autenticacao e autorizacao do projeto OctaPro, explicar como seria uma evolucao para ABAC e iniciar a implementacao da base de permissoes com as tabelas `permissions`, `permissions_role` e `users_permissions`.

Esta etapa evoluiu de analise conceitual para implementacao parcial. Ainda nao foi implementado o ABAC completo por atributo de recurso, como `CorporationId`, processo arquivado ou usuario vinculado ao processo.

### Contexto do projeto

O projeto usa ASP.NET Core com ASP.NET Identity e JWT.

O ambiente de desenvolvimento deve ser executado dentro do container Docker `dev-env`. Comandos como `dotnet`, `dotnet ef`, `dotnet build`, `dotnet run` e similares nao devem ser executados diretamente no host.

Antes de comandos dependentes do ambiente, foi verificado o container:

```bash
docker compose ps dev-env
```

O container estava em execucao. O projeto esta montado dentro do container em:

```text
/app/backend
```

### Estado inicial encontrado

A autenticacao estava centralizada em:

- `AuthService`
- `TokenService`
- `JwtConfiguration`
- ASP.NET Identity com `User`, `IdentityRole<long>` e `IdentityUserRole<long>`

O login usa:

```csharp
_signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true)
```

Quando o login e valido, `TokenService` gera um JWT contendo:

- `ClaimTypes.NameIdentifier` com `user.IdPublic`
- `ClaimTypes.Email`
- `ClaimTypes.Role` para cada role do usuario

As roles existentes sao:

```text
1 -> Admin
2 -> Common
3 -> Manager
```

Elas sao criadas pelo `RoleSeeder`.

A autorizacao inicial era majoritariamente RBAC, com atributos como:

```csharp
[Authorize(Roles = "Admin,Manager,Common")]
[Authorize(Roles = "Admin,Manager")]
```

Tambem existe um filtro global em `Program.cs`:

```csharp
options.Filters.Add(new AuthorizeFilter());
```

Isso exige usuario autenticado por padrao, salvo endpoints com `[AllowAnonymous]`.

### Como a role e atribuida ao usuario

A role do usuario e atribuida no `UserService`.

No cadastro de usuario, apos criar o usuario com `_userManager.CreateAsync`, o codigo insere manualmente uma linha em `user_roles`:

```csharp
_context.UserRoles.Add(new IdentityUserRole<long>
{
    UserId = user.Id,
    RoleId = request.RoleId
});
```

Na edicao de usuario, o metodo `SyncUserRoleAsync` remove as roles atuais do usuario e adiciona a nova role:

```csharp
var currentRoles = await _context.UserRoles
    .Where(userRole => userRole.UserId == userId)
    .ToListAsync();

if (currentRoles.Count > 0)
    _context.UserRoles.RemoveRange(currentRoles);

_context.UserRoles.Add(new IdentityUserRole<long>
{
    UserId = userId,
    RoleId = roleId
});
```

Portanto, embora `user_roles` permita multiplas roles por usuario, a regra atual da aplicacao trata o usuario como tendo uma role principal.

### Conceito discutido: RBAC, permissoes e ABAC

Foi definido que:

- `roles` continuam representando perfis amplos, como `Admin`, `Manager` e `Common`.
- `permissions` representam acoes granulares, como `judicial_process.create`, `settlement.delete` e `user.read`.
- `permissions_role` associa permissoes padrao a uma role.
- `users_permissions` permite permissoes especificas diretamente para um usuario.

O modelo conceitual ficou:

```text
Usuario -> user_roles -> roles
Role -> permissions_role -> permissions
Usuario -> users_permissions -> permissions
```

As permissoes efetivas de um usuario sao a uniao de:

```text
permissoes herdadas pelas roles
+
permissoes diretas do usuario
```

Adicionar uma nova permissao em `permissions_role` nao recria permissoes do usuario. Ela adiciona apenas uma nova relacao role/permissao e passa a valer para todos os usuarios daquela role.

Adicionar uma nova permissao em `users_permissions` afeta apenas aquele usuario.

### Decisao: seed ou migration para popular permissoes

Foi decidido:

- Migration cria a estrutura das tabelas.
- Seed idempotente popula o catalogo e os vinculos padrao.

Motivo:

- A migration deve representar estrutura de banco.
- O seed permite atualizar descricoes, adicionar novas permissoes e associar roles sem duplicar dados.
- O projeto ja usa seed para roles via `RoleSeeder`.

### Decisao: Configuration mesmo usando Model First

Foi esclarecido que, no EF Core, Model First significa que o modelo C# e as configuracoes EF sao a fonte da verdade para gerar migrations.

Usar arquivos em `Data/Configurations` continua sendo Model First.

O projeto ja usa:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
```

As configurations foram usadas porque expressam melhor:

- indices unicos compostos;
- nomes explicitos de constraints;
- comportamento de delete;
- defaults SQL como `now()`;
- relacoes entre tabelas.

### Tabelas adicionadas

Foram criados os models:

- `Models/Permission.cs`
- `Models/RolePermission.cs`
- `Models/UserPermission.cs`

#### permissions

Tabela de catalogo de permissoes.

Campos principais:

```text
id
key
description
created_at
updated_at
```

Exemplos de `key`:

```text
judicial_process.read
judicial_process.create
settlement.delete
user.update
```

#### permissions_role

Tabela de associacao entre role e permissao.

Campos principais:

```text
id
role_id
permission_id
created_at
updated_at
```

Tem indice unico composto em:

```text
role_id + permission_id
```

#### users_permissions

Tabela de associacao direta entre usuario e permissao.

Campos principais:

```text
id
user_id
permission_id
created_at
updated_at
```

Tem indice unico composto em:

```text
user_id + permission_id
```

### Configuracoes EF adicionadas

Foram criados:

- `Data/Configurations/PermissionConfiguration.cs`
- `Data/Configurations/RolePermissionConfiguration.cs`
- `Data/Configurations/UserPermissionConfiguration.cs`

Elas configuram:

- primary keys;
- indices unicos;
- foreign keys;
- cascade delete;
- default `now()` para timestamps;
- nomes de constraints.

### AppDbContext atualizado

Foram adicionados os `DbSet`s:

```csharp
public virtual DbSet<Permission> Permissions { get; set; }
public virtual DbSet<RolePermission> RolePermissions { get; set; }
public virtual DbSet<UserPermission> UserPermissions { get; set; }
```

### Catalogo de permissoes

Foi criado:

```text
Authorization/Permissions.cs
```

Esse arquivo contem constantes para as permissoes do sistema.

Permissoes criadas:

```text
corporation.read
corporation.create
corporation.update
corporation.delete

entity.read
entity.create
entity.update
entity.delete

installment.reverse

judicial_process.read
judicial_process.create
judicial_process.update
judicial_process.archive
judicial_process.delete

legal_fee.read
legal_fee.create
legal_fee.update
legal_fee.delete
legal_fee.add_installment

settlement.read
settlement.create
settlement.update
settlement.delete
settlement.add_installment

user.read
user.create
user.update
user.delete
```

Tambem foi adicionado:

```csharp
public static readonly string[] All = [...]
```

Essa lista permite registrar automaticamente uma policy para cada permissao no `Program.cs`.

### PermissionSeeder

Foi criado:

```text
Data/Seeds/PermissionSeeder.cs
```

Responsabilidades:

- inserir ou atualizar permissoes no catalogo `permissions`;
- inserir vinculos padrao em `permissions_role`;
- nao duplicar registros ja existentes;
- manter descricoes atualizadas.

Associacoes padrao definidas:

```text
Admin   -> todas as permissoes
Manager -> todas as permissoes
Common  -> permissoes de leitura
```

Permissoes de `Common`:

```text
corporation.read
entity.read
judicial_process.read
legal_fee.read
settlement.read
user.read
```

O seed foi ligado no startup depois do `RoleSeeder`:

```csharp
await RoleSeeder.SeedRolesAsync(roleManager);
await PermissionSeeder.SeedPermissionsAsync(dbContext);
```

Isso e importante porque `PermissionSeeder` depende dos IDs das roles ja existentes.

### Migration criada

Foi gerada a migration:

```text
Migrations/20260826004026_AddPermissionsAbacTables.cs
Migrations/20260826004026_AddPermissionsAbacTables.Designer.cs
```

Comando usado dentro do container:

```bash
docker compose exec -T dev-env dotnet ef migrations add AddPermissionsAbacTables --project /app/backend/OctaPro.csproj --startup-project /app/backend/OctaPro.csproj
```

O primeiro comando sem `--project` falhou porque o diretorio padrao do container era `/app`, e nao `/app/backend`.

O comando correto usou explicitamente:

```text
--project /app/backend/OctaPro.csproj
--startup-project /app/backend/OctaPro.csproj
```

Nao foi executado `dotnet ef database update` pelo Codex.

### Como rodar a seed

A seed roda automaticamente quando a aplicacao inicia, desde que as tabelas existam.

Fluxo correto:

1. Listar migrations:

```bash
docker compose exec dev-env dotnet ef migrations list --project /app/backend/OctaPro.csproj --startup-project /app/backend/OctaPro.csproj
```

2. Se a migration estiver pendente, aplicar:

```bash
docker compose exec dev-env dotnet ef database update --project /app/backend/OctaPro.csproj --startup-project /app/backend/OctaPro.csproj
```

3. Iniciar a API:

```bash
docker compose exec dev-env dotnet run --project /app/backend/OctaPro.csproj
```

Ao iniciar, o `Program.cs` executa:

```csharp
await RoleSeeder.SeedRolesAsync(roleManager);
await PermissionSeeder.SeedPermissionsAsync(dbContext);
```

### Camada de autorizacao por permissao

Foi criada uma camada permission-based usando policies do ASP.NET Core.

Arquivos criados:

- `Authorization/PermissionRequirement.cs`
- `Authorization/PermissionAuthorizationHandler.cs`

#### PermissionRequirement

`PermissionRequirement` representa uma exigencia simples:

```text
o usuario precisa possuir a permissao X
```

Codigo:

```csharp
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}
```

Ele possui apenas um parametro porque, nesta fase, a verificacao e nominal:

```text
usuario tem judicial_process.create?
usuario tem settlement.delete?
usuario tem user.read?
```

No futuro, regras ABAC por recurso podem ser adicionadas em outra camada ou em requirements especificos por recurso.

#### Registro das policies

No `Program.cs`, foi adicionado:

```csharp
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Permissions.All)
    {
        options.AddPolicy(permission, policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});
```

Isso cria uma policy para cada permissao.

Exemplo conceitual:

```text
Policy: judicial_process.read
Requirement: PermissionRequirement("judicial_process.read")

Policy: judicial_process.create
Requirement: PermissionRequirement("judicial_process.create")

Policy: settlement.delete
Requirement: PermissionRequirement("settlement.delete")
```

#### PermissionAuthorizationHandler

O handler consulta se o usuario possui a permissao exigida.

Fluxo:

1. Le `ClaimTypes.NameIdentifier` do JWT.
2. Converte para `Guid`.
3. Busca o usuario por `IdPublic`.
4. Verifica permissao direta em `users_permissions`.
5. Se nao encontrou, verifica permissao herdada por role via `user_roles -> permissions_role -> permissions`.
6. Se encontrar, chama:

```csharp
context.Succeed(requirement);
```

Se nao chamar `context.Succeed`, o ASP.NET retorna `403 Forbidden` para usuario autenticado sem permissao.

### Quando HandleRequirementAsync e executado

`HandleRequirementAsync` nao e chamado manualmente.

Ele e chamado automaticamente pelo pipeline de autorizacao do ASP.NET Core quando uma requisicao chega em endpoint protegido com:

```csharp
[Authorize(Policy = Permissions.JudicialProcessCreate)]
```

Fluxo:

```text
1. Requisicao HTTP chega
2. JWT e autenticado
3. ASP.NET monta o ClaimsPrincipal
4. Endpoint exige uma policy
5. ASP.NET localiza os requirements da policy
6. PermissionAuthorizationHandler.HandleRequirementAsync e chamado
7. Se context.Succeed for chamado, a request continua
8. Caso contrario, retorna 403 Forbidden
```

Diferenca:

```text
Token ausente ou invalido -> 401 Unauthorized
Token valido sem permissao -> 403 Forbidden
```

### Controllers atualizados

Foram trocados os atributos baseados em role por policies baseadas em permissao nos controllers de dominio.

#### CorporationController

Antes:

```csharp
[Authorize(Roles = "Admin,Manager,Common")]
[Authorize(Roles = "Admin,Manager")]
```

Depois:

```csharp
[Authorize(Policy = Permissions.CorporationRead)]
[Authorize(Policy = Permissions.CorporationCreate)]
[Authorize(Policy = Permissions.CorporationUpdate)]
[Authorize(Policy = Permissions.CorporationDelete)]
```

#### EntityController

Policies usadas:

```text
entity.read
entity.create
entity.update
entity.delete
```

#### InstallmentController

Policy usada:

```text
installment.reverse
```

O `[Authorize(Roles = "Admin,Manager,Common")]` no nivel do controller foi removido, porque o controller so possuia uma acao protegida especifica.

#### JudicialProcessController

Policies usadas:

```text
judicial_process.read
judicial_process.create
judicial_process.archive
judicial_process.delete
```

#### LegalFeeController

Policies usadas:

```text
legal_fee.read
legal_fee.create
legal_fee.update
legal_fee.delete
legal_fee.add_installment
```

#### SettlementController

Policies usadas:

```text
settlement.read
settlement.create
settlement.update
settlement.delete
settlement.add_installment
```

#### UserController

Policies usadas:

```text
user.read
user.create
user.update
user.delete
```

### AuthController

`AuthController` ainda ficou com roles:

```csharp
[Authorize(Roles = "Admin,Manager")]
[Authorize(Roles = "Admin,Manager,Common")]
```

Motivo:

- `login` e anonimo;
- `me` e `logout` nao foram tratados como recursos de dominio do catalogo de permissoes nesta etapa;
- a mudanca foi mantida focada nos controllers de dominio.

### Behavior importante das policies em controller e action

Quando uma policy e aplicada no controller e outra na action, o usuario precisa atender ambas.

Exemplo:

```csharp
[Authorize(Policy = Permissions.JudicialProcessRead)]
public class JudicialProcessController : ControllerBase
{
    [Authorize(Policy = Permissions.JudicialProcessCreate)]
    public async Task<IActionResult> SaveProcess(...)
}
```

Para chamar `POST /api/process`, o usuario precisa:

```text
judicial_process.read
judicial_process.create
```

Isso esta coerente com o seed atual, porque `Admin` e `Manager` possuem todas as permissoes.

### O que ainda nao e ABAC completo

A implementacao atual e principalmente:

```text
permission-based authorization
```

Ela responde:

```text
o usuario tem a permissao X?
```

ABAC completo precisa responder tambem:

```text
o usuario tem permissao X sobre este recurso especifico, considerando atributos do usuario e do recurso?
```

Exemplos de regras ABAC ainda pendentes:

```text
process.corporation_id == user.corporation_id
settlement.corporation_id == user.corporation_id
legal_fee.corporation_id == user.corporation_id
process.is_archived == false para certas acoes
usuario esta vinculado ao processo em judicial_process_user
usuario comum so enxerga processos atribuidos a ele
```

Tambem foi observado que algumas listagens atuais nao filtram por `CorporationId`. Para ABAC real, nao basta bloquear detalhes e escritas; as queries de listagem tambem precisam aplicar escopo.

### Validacoes executadas

Foi executado build dentro do container:

```bash
docker compose exec -T dev-env dotnet build /app/backend/OctaPro.csproj
```

Resultado:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

### Arquivos adicionados

```text
Authorization/Permissions.cs
Authorization/PermissionRequirement.cs
Authorization/PermissionAuthorizationHandler.cs

Models/Permission.cs
Models/RolePermission.cs
Models/UserPermission.cs

Data/Configurations/PermissionConfiguration.cs
Data/Configurations/RolePermissionConfiguration.cs
Data/Configurations/UserPermissionConfiguration.cs

Data/Seeds/PermissionSeeder.cs

Migrations/20260826004026_AddPermissionsAbacTables.cs
Migrations/20260826004026_AddPermissionsAbacTables.Designer.cs
```

### Arquivos modificados

```text
Program.cs
Data/AppDbContext.cs
Migrations/AppDbContextModelSnapshot.cs

Controllers/CorporationController.cs
Controllers/EntityController.cs
Controllers/InstallmentController.cs
Controllers/JudicialProcessController.cs
Controllers/LegalFeeController.cs
Controllers/SettlementController.cs
Controllers/UserController.cs
```

### Pendencias e proximas fases sugeridas

1. Implementar ABAC por recurso nas queries e servicos.
2. Garantir filtro por `CorporationId` nas listagens.
3. Definir regra para `Common`: acesso por empresa, por vinculo em `judicial_process_user`, ou ambos.
4. Criar servico central de acesso, por exemplo `IAccessControlService`.
5. Decidir se `users_permissions` sera apenas aditiva ou se tambem havera negacoes explicitas.
6. Expor endpoints administrativos para conceder/remover permissoes individuais.
7. Considerar incluir `corporation_id` no token apenas como otimizacao, mantendo validacoes criticas no banco.

### Estado final desta sessao

Base de permissoes criada e integrada ao pipeline de autorizacao.

A API passou a usar policies por permissao nos controllers de dominio.

A estrutura ABAC ainda nao foi concluida; o proximo passo tecnico e aplicar atributos de usuario e recurso nas decisoes de acesso e no escopo das consultas.
