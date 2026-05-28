# Requisitos do Projeto — App de Mensagens

## 1. Visão Geral

Sistema de troca de mensagens em tempo real composto por:

- **Backend:** ASP.NET Core (.NET 10) com SignalR
- **Frontend:** Flutter (mobile/desktop)
- **Protocolo de tempo real:** WebSocket via SignalR
- **Autenticação:** JWT (JSON Web Token)

---

## 2. Requisitos Funcionais

### 2.1 Cadastro e Autenticação

- **DEVE** permitir que um novo usuário se cadastre informando nome de usuário único e senha.
- **DEVE** validar que o nome de usuário não está em uso antes de concluir o cadastro.
- **DEVE** autenticar o usuário com suas credenciais.
- **DEVE** rejeitar o acesso de usuários não autenticados.
- **DEVE** encerrar a sessão ao detectar credencial expirada ou inválida.

### 2.2 Conexão em Tempo Real

- **DEVE** oferecer canal de comunicação em tempo real para troca de mensagens.
- **DEVE** exigir autenticação para estabelecer a conexão.
- **DEVE** registrar cada conexão e desconexão de usuário.

### 2.3 Chat 1-para-1 (Privado)

- **DEVE** permitir que um usuário envie mensagem de texto para outro usuário específico.
- **DEVE** entregar a mensagem em tempo real apenas ao remetente e ao destinatário.
- **DEVE** identificar o remetente pela sessão ativa, nunca por dado informado pelo cliente.
- **DEVE** rejeitar mensagens com texto vazio ou acima do limite de caracteres.
- **DEVE** exibir mensagens enviadas e recebidas de forma diferenciada visualmente.

### 2.4 Grupos / Salas de Chat

- **DEVE** permitir a criação de grupos com nome e lista de participantes.
- **DEVE** permitir que o criador adicione ou remova participantes.
- **DEVE** entregar mensagens do grupo em tempo real a todos os membros conectados.
- **NÃO DEVE** permitir que usuários não membros enviem ou recebam mensagens do grupo.

---

## 3. Requisitos Não Funcionais

### 3.1 Backend — ASP.NET Core

- **DEVE** utilizar .NET 10 ou superior.
- **DEVE** seguir o padrão Minimal API.
- **DEVE** utilizar FluentValidation para validação de todas as requisições de entrada.
- **DEVE** utilizar Serilog para logging estruturado.
- **DEVE** possuir middleware global de tratamento de exceções, retornando respostas padronizadas em JSON.
- **DEVE** separar responsabilidades em camadas: Apresentação, Aplicação, Dominio, Infraestrutura.
- **DEVE** conter `Dockerfile` para containerização com imagem base Linux.

### 3.2 Segurança

- **DEVE** armazenar senhas com hash HASHMAC512.
- **DEVE** autenticar e autorizar via JWT, validando assinatura, emissor, público e expiração em cada requisição.
- **DEVE** ler configurações sensíveis (chave, issuer, audience) via variáveis de ambiente — nunca hardcoded.
- **NÃO DEVE** expor detalhes internos de erro (stack trace, mensagens de exceção) em respostas de produção.
- **DEVE** aplicar CORS restritivo, permitindo apenas origens conhecidas.
- **NÃO DEVE** expor IDs internos sequenciais — usar UUID/GUID.

### 3.3 Banco de Dados

- **DEVE** utilizar PostgreSQL.
- **DEVE** versionar o schema inicial via script SQL no repositório.
- **DEVE** concentrar toda interação com o banco em repositórios, nunca diretamente nos endpoints ou hub.
- **DEVE** utilizar índices nas colunas `remetente_id`, `destinatario_id` e `data_envio` para queries de histórico.

### 3.4 Frontend — Flutter

- **DEVE** utilizar o padrão de gerenciamento de estado Cubit (flutter_bloc).
- **DEVE** seguir arquitetura em camadas: Apresentacao -> Telas/Cubits/Estados, Dominio -> Modelos/Entidades/Enumeradores, Infraestrutura -> Repositorio/Servicos.
- **DEVE** possuir, inicialmente, as telas: Login, Cadastro, Conversas e Chat.
- **DEVE** reconectar automaticamente ao hub em caso de queda de conexão.

### 3.5 Infraestrutura e Ambiente

- **DEVE** prover ambiente de desenvolvimento via Docker Compose, incluindo banco de dados.
- **DEVE** conter `.gitignore` que exclua arquivos de segredos, binários e dependências.

---

## 5. Glossário

| Termo | Definição |
|-------|-----------|
| Hub | Ponto central de comunicação SignalR no servidor. |
| Cubit | Classe de gerenciamento de estado do pacote `flutter_bloc`. |
| JWT | JSON Web Token — token de autenticação stateless. |
| Remetente | Usuário que envia a mensagem. |
| Destinatário | Usuário ou grupo que recebe a mensagem. |
