#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    -- Esta é a migração inicial para o banco de dados PostgreSQL
    -- Em um projeto real, você usaria o EF Core para gerar migrações

    -- Criação das tabelas

    -- Tabela Usuarios
    CREATE TABLE IF NOT EXISTS "Usuarios" (
        "Id" UUID PRIMARY KEY,
        "Nome" VARCHAR(100) NOT NULL,
        "Email" VARCHAR(255) NOT NULL,
        "EhGerente" BOOLEAN NOT NULL DEFAULT FALSE
    );

    -- Tabela Projetos
    CREATE TABLE IF NOT EXISTS "Projetos" (
        "Id" UUID PRIMARY KEY,
        "Nome" VARCHAR(100) NOT NULL,
        "Descricao" VARCHAR(500) NULL,
        "DataCriacao" TIMESTAMP NOT NULL,
        "UsuarioId" UUID NOT NULL,
        CONSTRAINT "FK_Projetos_Usuarios_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios" ("Id") ON DELETE CASCADE
    );

    -- Tabela Tarefas
    CREATE TABLE IF NOT EXISTS "Tarefas" (
        "Id" UUID PRIMARY KEY,
        "Titulo" VARCHAR(100) NOT NULL,
        "Descricao" VARCHAR(500) NULL,
        "DataVencimento" TIMESTAMP NOT NULL,
        "Status" INTEGER NOT NULL,
        "Prioridade" INTEGER NOT NULL,
        "DataCriacao" TIMESTAMP NOT NULL,
        "ProjetoId" UUID NOT NULL,
        CONSTRAINT "FK_Tarefas_Projetos_ProjetoId" FOREIGN KEY ("ProjetoId") REFERENCES "Projetos" ("Id") ON DELETE CASCADE
    );

    -- Tabela AlteracoesTarefas
    CREATE TABLE IF NOT EXISTS "AlteracoesTarefas" (
        "Id" UUID PRIMARY KEY,
        "TarefaId" UUID NOT NULL,
        "Descricao" VARCHAR(500) NOT NULL,
        "DataAlteracao" TIMESTAMP NOT NULL,
        "UsuarioId" UUID NOT NULL,
        CONSTRAINT "FK_AlteracoesTarefas_Tarefas_TarefaId" FOREIGN KEY ("TarefaId") REFERENCES "Tarefas" ("Id") ON DELETE CASCADE
    );

    -- Tabela Comentarios
    CREATE TABLE IF NOT EXISTS "Comentarios" (
        "Id" UUID PRIMARY KEY,
        "Conteudo" VARCHAR(500) NOT NULL,
        "DataCriacao" TIMESTAMP NOT NULL,
        "TarefaId" UUID NOT NULL,
        "UsuarioId" UUID NOT NULL,
        CONSTRAINT "FK_Comentarios_Tarefas_TarefaId" FOREIGN KEY ("TarefaId") REFERENCES "Tarefas" ("Id") ON DELETE CASCADE
    );

    -- Inserir dados iniciais para testes
    INSERT INTO "Usuarios" ("Id", "Nome", "Email", "EhGerente")
    VALUES 
        ('11111111-1111-1111-1111-111111111111', 'Usuário Teste', 'usuario@teste.com', FALSE),
        ('22222222-2222-2222-2222-222222222222', 'Gerente Teste', 'gerente@teste.com', TRUE)
    ON CONFLICT DO NOTHING;
EOSQL