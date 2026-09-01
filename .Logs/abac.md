
# ABAC no OctaPro

## Estado atual

O projeto ja possui uma base de autorizacao por permissoes e parte das regras ABAC por empresa.

### Ja implementado

- Permissoes por role e por usuario.
- Tabelas:
  - `permissions`
  - `permissions_role`
  - `users_permissions`
- Policies usando `Permissions.X`.
- `PermissionAuthorizationHandler` consultando:
  - permissoes herdadas via `permissions_role`;
  - permissoes diretas via `users_permissions`.
- `CorporationId` em recursos principais.
- Escopo por `CorporationId` em:
  - processos;
  - acordos;
  - honorarios;
  - usuarios;
  - entidades/clientes;
  - empresas.
- Checagens de recurso arquivado em acordo/honorario.
- Seed da empresa inicial:
  - `id = 1`
  - `EMPRESA INICIAL`

## O que ainda falta

### 1. Vinculo usuario-processo

Usar a tabela `judicial_process_user` nas regras de acesso.

Exemplo de regra:

```text
Common so acessa processos onde esta vinculado.
Manager/Admin acessam processos da empresa.
```

Hoje o filtro principal e por `CorporationId`, mas ainda nao usa `access_level`.

### 2. Niveis de acesso por recurso

Definir valores padronizados para `judicial_process_user.access_level`, por exemplo:

```text
owner
editor
viewer
```

Exemplo de comportamento:

```text
viewer -> pode ler
editor -> pode ler e editar
owner  -> pode ler, editar e arquivar
```

### 3. Servico central de autorizacao ABAC

Hoje as regras estao espalhadas nos services.

O ideal e concentrar as decisoes em um servico como:

```csharp
IAccessControlService
```

Exemplos de metodos:

```csharp
CanReadProcessAsync(user, process)
CanUpdateProcessAsync(user, process)
CanCreateSettlementAsync(user, process)
CanDeleteLegalFeeAsync(user, legalFee)
```

### 4. Claims/contexto mais rico

Hoje o sistema busca o usuario no banco para obter o `CorporationId`.

Pode ser interessante adicionar ao JWT:

```text
corporation_id
```

Isso melhora performance, mas nao substitui validacoes criticas no banco.

### 5. Tratamento explicito de 403 vs 404

Hoje, quando o recurso pertence a outra empresa, muitas operacoes retornam como se o recurso nao existisse.

Essa decisao precisa ficar formalizada:

```text
404 -> esconder a existencia do recurso
403 -> informar que o recurso existe, mas o usuario nao tem permissao
```

### 6. Permissoes negativas ou revogacao individual

Hoje `users_permissions` apenas adiciona permissoes.

Ainda nao existe mecanismo para negar permissao herdada da role.

Exemplo ainda nao suportado:

```text
usuario e Manager, mas nao pode settlement.delete
```

Possiveis caminhos:

```text
users_permission_denies
```

ou:

```text
users_permissions.is_granted = true/false
```

### 7. Endpoints para gerenciar permissoes

As tabelas existem, mas ainda nao ha CRUD/API para:

```text
listar permissoes
atribuir permissao direta ao usuario
remover permissao direta
ver permissoes efetivas do usuario
```

### 8. Testes de autorizacao

Faltam testes cobrindo cenarios como:

```text
usuario de empresa A nao acessa empresa B
Common sem vinculo nao acessa processo
Common viewer nao edita processo
processo arquivado bloqueia alteracoes
permissao direta em users_permissions libera acao
```

## Resumo

O ABAC por empresa ja esta encaminhado.

O que falta para completar o desenho ABAC e implementar regras por vinculo e nivel de acesso ao recurso, principalmente usando:

```text
judicial_process_user.access_level
```

A proxima etapa mais importante e aplicar `judicial_process_user.access_level` nas regras de acesso dos processos e dos recursos derivados deles, como acordos e honorarios.
