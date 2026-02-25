-- ============================================================
--  Banco de Dados: Aplicativo de Mensagens (PostgreSQL)
-- ============================================================

-- TABELA: usuarios
CREATE TABLE usuarios (
    id         SERIAL       PRIMARY KEY,
    nome       VARCHAR(100) NOT NULL,
    email      VARCHAR(150) NOT NULL UNIQUE,
    senha_hash TEXT         NOT NULL,
    criado_em  TIMESTAMPTZ  NOT NULL DEFAULT NOW()
);

-- TABELA: conversas
CREATE TABLE conversas (
    id        SERIAL      PRIMARY KEY,
    nome      VARCHAR(100),            -- NULL para conversas privadas
    tipo      VARCHAR(10) NOT NULL DEFAULT 'privado'
                          CHECK (tipo IN ('privado', 'grupo')),
    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- TABELA: participantes (N:N entre usuarios e conversas)
CREATE TABLE participantes (
    usuario_id  INT NOT NULL REFERENCES usuarios(id)  ON DELETE CASCADE,
    conversa_id INT NOT NULL REFERENCES conversas(id) ON DELETE CASCADE,
    PRIMARY KEY (usuario_id, conversa_id)
);

-- TABELA: mensagens
CREATE TABLE mensagens (
    id          SERIAL      PRIMARY KEY,
    conversa_id INT         NOT NULL REFERENCES conversas(id) ON DELETE CASCADE,
    usuario_id  INT         NOT NULL REFERENCES usuarios(id)  ON DELETE CASCADE,
    conteudo    TEXT        NOT NULL,
    enviada_em  TIMESTAMPTZ NOT NULL DEFAULT NOW()
);