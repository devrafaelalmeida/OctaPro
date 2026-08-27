# Codex Log

## Indice

| Tema | Data |
| --- | --- |
| [Autenticacao, autorizacao e base ABAC](#2026-08-26-autenticacao-autorizacao-e-base-abac) | [2026-08-26](#2026-08-26-autenticacao-autorizacao-e-base-abac) |
| [Estado atual do ABAC](#2026-08-26-estado-atual-do-abac) | [2026-08-26](#2026-08-26-estado-atual-do-abac) |
| [Servico central de autorizacao](#2026-08-26-servico-central-de-autorizacao) | [2026-08-26](#2026-08-26-servico-central-de-autorizacao) |
| [Claims do JWT](#2026-08-26-claims-do-jwt) | [2026-08-26](#2026-08-26-claims-do-jwt) |
| [CRUD de permissoes](#2026-08-26-crud-de-permissoes) | [2026-08-26](#2026-08-26-crud-de-permissoes) |
| [Tratamento 401, 403, 404 e 400](#2026-08-26-tratamento-401-403-404-e-400) | [2026-08-26](#2026-08-26-tratamento-401-403-404-e-400) |

## 2026-08-26 - Autenticacao, autorizacao e base ABAC

### Contexto inicial

O projeto usa ASP.NET Core, ASP.NET Identity e JWT.

A autenticacao estava concentrada em:

- `AuthService`
- `TokenService`
- `JwtConfiguration`
- `User`, `IdentityRole<long>` e `IdentityUserRole<long>`

O login valida credenciais com:

```csharp
_signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true)
```

Quando o login e valido, o `TokenService` gera um JWT.

Inicialmente o token continha:

- `ClaimTypes.NameIdentifier` com `User.IdPublic`;
- `ClaimTypes.Email`;
- `ClaimTypes.Role`.

A autorizacao inicial era baseada em roles:

```csharp
[Authorize(Roles = "Admin,Manager,Common")]
[Authorize(Roles = "Admin,Manager")]
```

As roles existentes sao:

```text
1 -> Admin
2 -> Common
3 -> Manager
```

Elas sao criadas pelo `RoleSeeder`.

### Como a role e atribuida ao usuario

A role do usuario e atribuida no `UserService`.

No cadastro, depois de criar o usuario com `_userManager.CreateAsync`, o sistema insere diretamente em `user_roles`:

```csharp
_context.UserRoles.Add(new IdentityUserRole<long>
{
    UserId = user.Id,
    RoleId = request.RoleId
});
```

Na edicao, `SyncUserRoleAsync` remove as roles atuais e adiciona a nova.

Apesar de `user_roles` suportar multiplas roles, o comportamento atual do projeto trata o usuario como tendo uma role principal.

### Modelo de permissoes

Foi definido o seguinte modelo:

```text
Usuario -> user_roles -> roles
Role -> permissions_role -> permissions
Usuario -> users_permissions -> permissions
```

As permissoes efetivas do usuario sao:

```text
permissoes herdadas das roles
+
permissoes diretas do usuario
```

Adicionar permissao em `permissions_role` afeta todos os usuarios daquela role.

Adicionar permissao em `users_permissions` afeta apenas aquele usuario.

### Tabelas adicionadas

Foram adicionados models e configuracoes para:

- `permissions`
- `permissions_role`
- `users_permissions`

`permissions` e o catalogo global.

Exemplos:

```text
judicial_process.read
judicial_process.create
settlement.delete
user.update
```

`permissions_role` associa permissoes padrao a uma role.

`users_permissions` associa permissoes diretas a um usuario.

### Seed de permissoes

Foi criado `PermissionSeeder`.

Ele:

- cria/atualiza o catalogo de permissoes;
- cria vinculos padrao em `permissions_role`;
- nao duplica registros existentes.

Regra inicial:

```text
Admin   -> todas as permissoes
Manager -> todas as permissoes
Common  -> permissoes de leitura
```

### PermissionAuthorizationHandler

Foi criado `PermissionRequirement` e `PermissionAuthorizationHandler`.

O ASP.NET chama o handler automaticamente quando uma action/controller possui:

```csharp
[Authorize(Policy = Permissions.SettlementDelete)]
```

Fluxo:

```text
1. Requisicao chega
2. JWT e autenticado
3. ASP.NET le a policy exigida pelo endpoint
4. Handler verifica se o usuario tem a permissao
5. Se tiver, chama context.Succeed(requirement)
6. Se nao tiver, retorna 403
```

### Controllers migrados para policies

Controllers de dominio deixaram de usar roles hardcoded e passaram a usar policies.

Exemplo:

```csharp
[Authorize(Policy = Permissions.JudicialProcessRead)]
[Authorize(Policy = Permissions.JudicialProcessCreate)]
```

O `AuthController` continuou usando roles para `me`/`logout`, pois nao foi tratado como recurso de dominio nesta etapa.

## 2026-08-26 - Estado atual do ABAC

O projeto ja possui uma base de autorizacao por permissoes e parte das regras ABAC por empresa.

### Ja implementado

- Permissoes por role e por usuario.
- Tabelas:
  - `permissions`
  - `permissions_role`
  - `users_permissions`
- Policies usando `Permissions.X`.
- `PermissionAuthorizationHandler`.
- `CorporationId` em recursos principais.
- Escopo por `CorporationId` em:
  - processos;
  - acordos;
  - honorarios;
  - usuarios;
  - entidades/clientes;
  - empresas.
- Checagens de processo arquivado em acordo/honorario.
- Seed da empresa inicial:
  - `id = 1`
  - `EMPRESA INICIAL`

### CorporationId em entities

Foi adicionado `CorporationId` em `Entity`.

Tambem foi configurada a FK:

```text
entities.corporation_id -> corporations.id
```

Como `Corporation.Id` e `long`, os `CorporationId` dos models relacionados foram ajustados para `long`.

Models/DTOs ajustados:

- `User`
- `JudicialProcess`
- `Settlement`
- `LegalFee`
- `Entity`
- `UserRequest`
- `UserResponse`

### Seed da empresa inicial

Foi criado `CorporationSeeder`.

Regra:

```text
se corporations.id = 1 existir, nao faz nada
se nao existir, cria EMPRESA INICIAL com id = 1
```

Foi usado SQL com:

```sql
OVERRIDING SYSTEM VALUE
```

Motivo: `corporations.id` e identity e a seed precisa garantir `id = 1`.

### Ponto de atencao na migration de entities

Ao adicionar `corporation_id` em `entities`, o EF gerou:

```csharp
defaultValue: 0L
```

Isso causou erro de FK:

```text
23503: insert or update on table "entities" violates foreign key constraint "fk_entities_corporation"
```

Motivo:

```text
entities.corporation_id = 0
```

mas nao existe:

```text
corporations.id = 0
```

A correcao recomendada para migration e:

1. adicionar `corporation_id` como nullable;
2. popular registros existentes com uma empresa valida;
3. alterar para `NOT NULL`;
4. criar indice e FK.

Como os registros existentes sao poucos, foi decidido que eles serao ajustados manualmente.

### O que ainda falta no ABAC

Ainda falta implementar:

- uso de `judicial_process_user`;
- padronizacao de `access_level`;
- regras por vinculo e nivel de acesso ao recurso;
- testes de autorizacao.

Valores sugeridos para `access_level`:

```text
owner
editor
viewer
```

Exemplo:

```text
viewer -> pode ler
editor -> pode ler e editar
owner  -> pode ler, editar e arquivar
```

## 2026-08-26 - Servico central de autorizacao

Foi discutido criar um middleware customizado para verificar todas as permissions do usuario em toda requisicao autenticada.

Decisao: nao criar middleware.

Motivo: o ASP.NET Core ja possui esse pipeline nativo por meio de:

- `[Authorize]`;
- policies;
- requirements;
- authorization handlers.

O mecanismo nativo:

- respeita `[AllowAnonymous]`;
- diferencia 401 e 403 corretamente;
- le metadata de controller/action;
- suporta multiplas policies;
- integra com MVC e Swagger.

### IAccessControlService

Foi criado:

```text
Services/interfaces/IAccessControlService.cs
Services/AccessControlService.cs
```

Interface:

```csharp
public interface IAccessControlService
{
    Task<IReadOnlySet<string>> GetEffectivePermissionsAsync(User user);
    Task<bool> HasPermissionAsync(User user, string permission);
}
```

Responsabilidade:

```text
centralizar o calculo das permissoes efetivas do usuario
```

Calculo:

```text
permissoes herdadas das roles
+
permissoes diretas do usuario
```

O `PermissionAuthorizationHandler` foi simplificado.

Antes ele consultava diretamente:

- `UserRoles`;
- `RolePermissions`;
- `UserPermissions`.

Agora ele usa:

```csharp
_accessControlService.HasPermissionAsync(user, requirement.Permission)
```

Nao foi implementado cache por request nesta etapa.

## 2026-08-26 - Claims do JWT

Foi discutido que JWT nao e criptografado, apenas assinado.

Qualquer pessoa com o token pode ler o payload em ferramentas como jwt.io.

Regra definida:

```text
Nunca colocar segredo em claim.
```

`IdPublic` no token foi considerado aceitavel, pois e identificador publico e nao deve ser tratado como segredo.

Foi decidido remover o email do token e adicionar `corporation_id`.

Antes:

```text
NameIdentifier -> IdPublic
Email -> email do usuario
Role -> roles
```

Depois:

```text
NameIdentifier -> IdPublic
corporation_id -> CorporationId
Role -> roles
```

Observacao:

```text
corporation_id no JWT ajuda contexto/performance,
mas o banco continua sendo a fonte de verdade para decisoes criticas.
```

## 2026-08-26 - CRUD de permissoes

Foi criado CRUD administrativo para gerenciar permissoes diretas de usuarios.

Regra geral:

```text
Apenas usuarios com role Admin ou Manager podem acessar.
```

Tambem foi aplicada regra de escopo:

```text
Admin/Manager so gerenciam usuarios da propria CorporationId.
```

### Diferenca entre listar permissoes e permissoes efetivas

`Listar permissoes`:

```text
lista o catalogo global de permissoes existentes no sistema
```

Exemplo:

```text
judicial_process.read
settlement.delete
user.update
```

`Ver permissoes efetivas do usuario`:

```text
calcula o que o usuario realmente possui
```

Inclui:

```text
permissoes herdadas pelas roles
+
permissoes diretas em users_permissions
```

### Endpoints criados

Controller:

```text
Controllers/PermissionController.cs
```

Rotas:

```text
GET    /api/permissions
GET    /api/permissions/users/{userIdPublic}/direct
GET    /api/permissions/users/{userIdPublic}/effective
POST   /api/permissions/users/{userIdPublic}
DELETE /api/permissions/users/{userIdPublic}/{permissionId}
```

Significado:

```text
GET /api/permissions
Lista catalogo global.

GET /direct
Lista permissoes diretas em users_permissions.

GET /effective
Lista permissoes efetivas: role + diretas.

POST
Atribui permissao direta.

DELETE
Remove permissao direta.
```

Arquivos criados:

- `PermissionController`
- `IPermissionService`
- `PermissionService`
- `UserPermissionRequest`
- `PermissionResponse`
- `EffectivePermissionResponse`

### Permissoes negativas

Foi discutida a possibilidade de um Manager continuar Manager, mas perder uma permissao herdada, como `user.create`.

Conclusao:

```text
isso exigiria uma tabela de negacoes, nao remocao de permissions_role
```

Exemplo possivel:

```text
users_permission_denies
```

O modelo atual nao tera negacoes por enquanto.

Modelo mantido:

```text
permissoes efetivas =
permissoes herdadas da role
+
permissoes diretas do usuario
```

## 2026-08-26 - Tratamento 401, 403, 404 e 400

Recomendacao definida:

```text
Sem token/token invalido -> 401
Sem permissao -> 403
Fora da empresa/tenant -> 404
Regra de negocio violada -> 400
```

### 401 Unauthorized

Quando:

```text
nao enviou token
token invalido
token expirado
```

### 403 Forbidden

Quando:

```text
usuario autenticado nao possui a permissao/policy necessaria
```

Exemplo:

```text
Common tentando user.create
Manager sem settlement.delete
```

Isso e tratado pelas policies:

```csharp
[Authorize(Policy = Permissions.UserCreate)]
```

### 404 Not Found

Quando:

```text
recurso nao existe
ou
recurso existe, mas esta fora da CorporationId do usuario
```

Motivo:

```text
fora do tenant = nao existe para esse usuario
```

Isso evita revelar existencia de dados de outra empresa.

### 400 Bad Request

Quando:

```text
requisicao invalida
estado de negocio impede a operacao
```

Exemplos:

```text
criar acordo em processo arquivado
editar honorario de processo arquivado
role invalida
permission inexistente no body
```

Ponto pendente:

```text
padronizar excecoes de regra de negocio para nao virarem 500
```

Alguns casos poderiam ser `409 Conflict`, mas manter `400` e aceitavel se for padronizado no projeto.
