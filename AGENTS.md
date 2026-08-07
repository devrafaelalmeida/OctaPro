## Ambiente de desenvolvimento

Este projeto utiliza Docker para todo o ambiente de desenvolvimento.

O sistema operacional host deve ser utilizado apenas para:

- editar arquivos;
- executar comandos Git;
- executar comandos Docker e Docker Compose.

## Execução de comandos

Todo comando que dependa do ambiente de desenvolvimento deve ser executado **exclusivamente** dentro do container do serviço `dev-env`.

Isso inclui, mas não se limita a:

- dotnet
- dotnet ef
- dotnet restore
- dotnet build
- dotnet run
- dotnet test
- msbuild
- npm
- npx
- node
- yarn
- pnpm
- vite

**Nunca execute esses comandos diretamente no host.**

Sempre utilize:

```bash
docker compose exec dev-env <comando>
```

Exemplos:

```bash
docker compose exec dev-env dotnet restore
docker compose exec dev-env dotnet build
docker compose exec dev-env dotnet test
docker compose exec dev-env dotnet ef database update
docker compose exec dev-env dotnet ef migrations add NomeDaMigration

docker compose exec dev-env npm install
docker compose exec dev-env npm run dev
docker compose exec dev-env npm run build
docker compose exec dev-env npx prisma migrate dev
docker compose exec dev-env vite
```

Antes de executar qualquer comando, verifique se o container `dev-env` está em execução. Caso não esteja, informe o usuário e aguarde instruções antes de iniciar ou reiniciar containers.


## Alterações de código

Os arquivos do projeto estão montados como volumes Docker.

Edite normalmente os arquivos no workspace do host.

Nunca copie arquivos para dentro do container para realizar alterações.

## Prioridade

Em caso de dúvida entre executar um comando no host ou no container, **sempre utilize o container `dev-env`**.

Se um comando puder ser executado tanto no host quanto no container, prefira sempre a execução no container.

## Entity Framework Migrations

Antes de executar qualquer comando que atualize o banco de dados (`dotnet ef database update`):

1. Liste as migrations utilizando:

```bash
docker compose exec dev-env dotnet ef migrations list
```

2. Verifique se a migration desejada ainda está marcada como **(Pending)**.

3. Peça permissão para executar a migration. 

4. Caso autorizado, execute `dotnet ef database update` somente se existir pelo menos uma migration pendente.

5. Caso não existam migrations pendentes, informe ao usuário e não execute o comando.