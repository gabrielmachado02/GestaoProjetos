# Gestão de Tarefas - API

API de gerenciamento de tarefas desenvolvida com .NET 8, seguindo os princípios de Clean Architecture e utilizando PostgreSQL como banco de dados.

## Tecnologias utilizadas

- .NET 8
- PostgreSQL
- Entity Framework Core
- Docker e Docker Compose
- AutoMapper
- Swagger
- xUnit (para testes)

## Estrutura do Projeto

O projeto segue a arquitetura Clean Architecture, com as seguintes camadas:

- **GestaoTarefas.Domain**: Contém as entidades, enumerações, exceções, interfaces e lógica específica do domínio.
- **GestaoTarefas.Application**: Contém a lógica de aplicação, incluindo DTOs, mapeamentos e serviços.
- **GestaoTarefas.Infrastructure**: Implementa interfaces do domínio, como acesso a banco de dados, integrações externas, etc.
- **GestaoTarefas.Api**: Camada da API REST com controladores.
- **Testes**: Testes para cada camada da aplicação.

## Como executar o projeto

### Requisitos

- Docker e Docker Compose

### Passos para execução

1. Clone o repositório:
   ```
   git clone <url-do-repositorio>
   cd GestaoTarefas
   ```

2. Execute o projeto usando Docker Compose:
   ```
   docker-compose up -d
   ```

3. A API estará disponível em:
   - Swagger UI: http://localhost:5000/swagger
   - API: http://localhost:5000/api

## Endpoints da API

### Projetos

- `GET /api/projetos/usuario/{usuarioId}` - Listar todos os projetos do usuário
- `GET /api/projetos/{id}` - Obter detalhes de um projeto específico
- `POST /api/projetos` - Criar um novo projeto
- `DELETE /api/projetos/{id}` - Remover um projeto

### Tarefas

- `GET /api/tarefas/projeto/{projetoId}` - Listar todas as tarefas de um projeto
- `GET /api/tarefas/{id}` - Obter detalhes de uma tarefa específica
- `POST /api/tarefas` - Criar uma nova tarefa
- `PUT /api/tarefas` - Atualizar uma tarefa existente
- `DELETE /api/tarefas/{id}` - Remover uma tarefa
- `POST /api/tarefas/comentario` - Adicionar um comentário a uma tarefa

### Relatórios

- `POST /api/relatorios/desempenho/tarefas-concluidas` - Obter média de tarefas concluídas por usuário nos últimos 30 dias 
  - Requer payload: `{ "ehGerente": true, "usuarioId": "guid-do-usuario" }`
  - Acesso restrito a usuários com a função de gerente

## Regras de Negócio Implementadas

1. **Prioridades de Tarefas**:
   - Cada tarefa tem uma prioridade (baixa, média, alta)
   - Não é possível alterar a prioridade após a criação

2. **Restrições de Remoção de Projetos**:
   - Um projeto não pode ser removido se tiver tarefas pendentes
   - A API retorna erro indicando que as tarefas devem ser concluídas ou removidas primeiro

3. **Histórico de Atualizações**:
   - Cada alteração em uma tarefa é registrada no histórico
   - O histórico inclui data, descrição da alteração e usuário responsável

4. **Limite de Tarefas por Projeto**:
   - Cada projeto tem limite máximo de 20 tarefas

5. **Relatórios de Desempenho**:
   - A API fornece endpoints para relatórios como média de tarefas concluídas
   - Relatórios acessíveis apenas para usuários com função de "gerente"

6. **Comentários nas Tarefas**:
   - Usuários podem adicionar comentários às tarefas
   - Comentários são registrados no histórico da tarefa

## Perguntas para Refinamento (Fase 2)

### Perguntas para o PO

1. **Autenticação e Autorização**:
   - Como a autenticação será integrada com este sistema?
   - Quais perfis de usuário precisamos suportar além de "gerente"?
   - Existe uma preferência por OAuth, JWT ou outro mecanismo de autenticação?

2. **Performance e Escalabilidade**:
   - Qual é o volume esperado de usuários e tarefas no sistema?
   - Devemos implementar paginação nos endpoints que retornam múltiplos itens?
   - Existe necessidade de caching para melhorar a performance?

3. **Funcionalidades Adicionais**:
   - Existe necessidade de suporte a anexos em tarefas?
   - Devemos implementar notificações (e-mail, push) para prazos de tarefas?
   - Seria útil adicionar etiquetas/tags para categorizar tarefas?
   - Existe necessidade de uma funcionalidade de busca/filtro avançada?

4. **UX e Integração**:
   - Quais campos devem ser obrigatórios ao criar tarefas e projetos?
   - Devemos fornecer endpoints para busca/filtro por diferentes critérios?
   - Existem sistemas externos que precisarão se integrar com esta API?

5. **Recursos de Colaboração**:
   - Como deve funcionar o compartilhamento de projetos entre usuários?
   - Devemos implementar controle de acesso por projeto?
   - Existe necessidade de histórico de atividade por usuário?

6. **Relatórios e Análises**:
   - Quais outros tipos de relatórios seriam úteis além do média de tarefas concluídas?
   - Existe necessidade de exportação de dados (CSV, PDF)?
   - Devemos implementar métricas de produtividade ou KPIs específicos?

7. **Regulamentações e Compliance**:
   - Existem requisitos de retenção de dados específicos?
   - Precisamos implementar alguma funcionalidade específica para LGPD ou outras regulamentações?
   - Existe necessidade de logs de auditoria para ações específicas?