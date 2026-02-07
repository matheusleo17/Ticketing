# 🎟️ Ticketing – Secure Ticket Transaction (Learning Project)

Este projeto é um **laboratório de aprendizado** focado em arquitetura limpa, mensageria e processamento assíncrono usando **.NET**, **RabbitMQ** e **Docker**.

A ideia central é simular uma **transação segura de ingressos** entre comprador e vendedor, não no sentido financeiro, mas no sentido **arquitetural**: concorrência, eventos, isolamento de responsabilidades e comunicação assíncrona.

O foco **não é o domínio de ingressos**, e sim **como estruturar um sistema moderno e desacoplado**.

---

## 🎯 Objetivos do projeto

- Aprender e aplicar **Clean Architecture**
- Aplicar os princípios do **SOLID**
- Entender **mensageria na prática** com RabbitMQ
- Trabalhar com **event-driven architecture**
- Criar um **Worker** para processamento assíncrono
- Usar **Docker** para infraestrutura local
- Aprender a debugar sistemas distribuídos

---

## 🧱 Arquitetura

O projeto segue uma separação clara de responsabilidades:

Src/
├─ Ticketing.Domain → Entidades e regras de negócio
├─ Ticketing.Application → UseCases, interfaces e eventos
├─ Ticketing.Infrastructure → Implementações técnicas (RabbitMQ)
├─ Ticketing.Worker → Consumer de eventos (BackgroundService)
├─ Ticketing.Publisher → Publisher de teste (temporário)
Tests/
└─ Ticketing.Tests → Testes unitários com fakes


### Visão geral do fluxo

Publisher / API
↓
EventBus (RabbitMQ)
↓
Exchange
↓
Queue
↓
Worker (Consumer)


---

## 🐇 RabbitMQ

O RabbitMQ é usado para **publicar e consumir eventos**, simulando comunicação entre serviços desacoplados.

### Evento implementado

- `OrderCreated`
  - Publicado quando um pedido é criado
  - Consumido pelo Worker para processamento assíncrono

---

## 🐳 Docker

O RabbitMQ roda localmente via Docker.

### Subir o RabbitMQ

```bash
docker compose up -d
A interface de administração fica disponível em:

http://localhost:15672
Credenciais padrão:

User: guest

Password: guest

⚙️ Configuração
O projeto utiliza um appsettings.json na raiz da solution.

Exemplo:

{
  "RabbitMq": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest",
    "Exchange": "ticketing.events",
    "Queue": "ticketing.order-created"
  }
}
⚠️ Em um cenário real, esse arquivo não deve ser versionado.
Aqui ele é mantido por fins didáticos.

🚀 Como rodar o projeto
1. Subir o RabbitMQ
docker compose up -d
2. Rodar o Worker
dotnet run --project .\Ticketing.Worker\Ticketing.Worker.csproj
O Worker irá:

Criar a exchange

Criar a fila

Se conectar ao RabbitMQ

Aguardar mensagens

3. Rodar o Publisher (teste)
dotnet run --project .\Ticketing.Publisher\Ticketing.Publisher.csproj
Você verá:

A mensagem sendo publicada

O Worker consumindo o evento

🧪 Testes
Os testes utilizam:

Repositórios em memória

FakeClock

FakeEventBus

O foco é validar:

Regras de negócio

Fluxo do UseCase

Independência de infraestrutura

📌 Observações importantes
O Publisher é temporário e existe apenas para validar a mensageria

No futuro, ele será substituído por uma Web API

O projeto não é um produto final, e sim um ambiente de aprendizado controlado

Decisões foram tomadas priorizando clareza arquitetural, não complexidade
