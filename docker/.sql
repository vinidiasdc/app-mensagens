CREATE EXTENSION IF NOT EXISTS "pg_trgm";

CREATE TABLE usuarios (
    id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome_usuario     VARCHAR(50)    NOT NULL UNIQUE,
    email            VARCHAR(254)   NOT NULL UNIQUE,
    telefone         VARCHAR(20)    UNIQUE,
    senha_hash       TEXT           NOT NULL,
    nome_exibicao    VARCHAR(100)   NOT NULL,
    bio              TEXT,
    url_foto         TEXT,
    status           SMALLINT       NOT NULL DEFAULT 0, -- 0=offline 1=online 2=ausente 3=ocupado
    ultimo_acesso_em TIMESTAMPTZ,
    criado_em        TIMESTAMPTZ    NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_status_usuario CHECK (status BETWEEN 0 AND 3)
);

CREATE INDEX idx_usuarios_nome_usuario  ON usuarios USING gin (nome_usuario gin_trgm_ops);
CREATE INDEX idx_usuarios_email         ON usuarios (email);

CREATE TABLE conversas (
    id         UUID     PRIMARY KEY DEFAULT gen_random_uuid(),
    tipo       SMALLINT NOT NULL DEFAULT 0, -- 0=privado 1=grupo
    nome       VARCHAR(100),
    descricao  TEXT,
    url_foto   TEXT,
    criado_por UUID     NOT NULL REFERENCES usuarios (id),
    criado_em  TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_tipo_conversa CHECK (tipo BETWEEN 0 AND 1),
    CONSTRAINT chk_direto_sem_nome  CHECK (tipo != 0 OR nome IS NULL)
);

CREATE INDEX idx_conversas_tipo       ON conversas (tipo);

CREATE TABLE membros_conversa (
    id          UUID     PRIMARY KEY DEFAULT gen_random_uuid(),
    conversa_id UUID     NOT NULL REFERENCES conversas (id) ON DELETE CASCADE,
    usuario_id  UUID     NOT NULL REFERENCES usuarios (id)  ON DELETE CASCADE,
    funcao      SMALLINT NOT NULL DEFAULT 0, -- 0=membro 1=administrador
    saiu_em     TIMESTAMPTZ,

    UNIQUE (conversa_id, usuario_id),
    CONSTRAINT chk_funcao_membro CHECK (funcao BETWEEN 0 AND 1)
);

CREATE INDEX idx_membros_usuario_id  ON membros_conversa (usuario_id) WHERE saiu_em IS NULL;
CREATE INDEX idx_membros_conversa_id ON membros_conversa (conversa_id);

CREATE TABLE mensagens (
    id               UUID     PRIMARY KEY DEFAULT gen_random_uuid(),
    conversa_id      UUID     NOT NULL REFERENCES conversas (id) ON DELETE CASCADE,
    remetente_id     UUID     NOT NULL REFERENCES usuarios (id),
    resposta_para_id UUID     REFERENCES mensagens (id),
    tipo             SMALLINT NOT NULL DEFAULT 0, -- 0=texto 1=imagem 2=video 3=audio 4=arquivo
    conteudo         TEXT,
    editada          BOOLEAN  NOT NULL DEFAULT FALSE,
    excluida         BOOLEAN  NOT NULL DEFAULT FALSE,
    editada_em       TIMESTAMPTZ,
    criada_em        TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_tipo_mensagem CHECK (tipo BETWEEN 0 AND 4)
);

CREATE INDEX idx_mensagens_conversa ON mensagens (conversa_id, criada_em DESC) WHERE excluida = FALSE;
CREATE INDEX idx_mensagens_remetente   ON mensagens (remetente_id);
CREATE INDEX idx_mensagens_busca_texto ON mensagens USING gin (to_tsvector('portuguese', conteudo))
    WHERE tipo = 0 AND excluida = FALSE;

CREATE TABLE anexos_mensagem (
    id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    mensagem_id   UUID NOT NULL REFERENCES mensagens (id) ON DELETE CASCADE,
    nome_arquivo  VARCHAR(255) NOT NULL,
    tipo_arquivo  VARCHAR(127) NOT NULL, -- tipo MIME (ex: image/png)
    tamanho_bytes BIGINT       NOT NULL,
    url_arquivo   TEXT         NOT NULL
);

CREATE INDEX idx_anexos_mensagem_id ON anexos_mensagem (mensagem_id);

CREATE TABLE leituras_mensagem (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    mensagem_id UUID NOT NULL REFERENCES mensagens (id) ON DELETE CASCADE,
    usuario_id  UUID NOT NULL REFERENCES usuarios (id)  ON DELETE CASCADE,
    lido_em     TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    UNIQUE (mensagem_id, usuario_id)
);

CREATE INDEX idx_leituras_mensagem_id ON leituras_mensagem (mensagem_id);
CREATE INDEX idx_leituras_usuario_id  ON leituras_mensagem (usuario_id);

CREATE TABLE tokens_push (
    id         UUID     PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id UUID     NOT NULL REFERENCES usuarios (id) ON DELETE CASCADE,
    token      TEXT     NOT NULL UNIQUE,
    plataforma SMALLINT NOT NULL, -- 0=android 1=ios
    ativo      BOOLEAN  NOT NULL DEFAULT TRUE,
    criado_em  TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_plataforma_push CHECK (plataforma BETWEEN 0 AND 1)
);

CREATE INDEX idx_tokens_push_usuario ON tokens_push (usuario_id) WHERE ativo = TRUE;