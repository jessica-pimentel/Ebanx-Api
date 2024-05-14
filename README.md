# EBANX assignment - Desenvolvedor de Software

Este repositório consiste na implementação de uma API básica desenvolvida em ASP.NET Core MVC. A aplicação utiliza métodos HTTP para manipular requisições e recebe dados diretamente da requisição usando os atributos [FromBody] e [FromQuery] para realizar o binding de parâmetros da requisição diretamente aos parâmetros dos métodos da API. O objetivo principal é implementar um serviço para criar, gerenciar e transferir valores entre contas. Para armazenamento dos dados, é empregado o uso de um Dictionary. Além disso, a aplicação segue uma arquitetura de camadas para separar a lógica de negócio da camada de apresentação.

### Endpoints

A API possui os seguintes endpoints:

## **POST /reset**
Este endpoint limpa e reseta os dados dos id das contas, depositos, transferências e valores, basta enviar uma requisição POST para `/accounts/reset`. Não é necessário enviar nenhum parâmetro adicional.

## **GET /balance**:
Este endpoint retorna o saldo atual da conta. Para fazer uma solicitação, envie uma requisição GET para `/accounts/balance` com o parâmetro `accountId` especificando o número da conta.

## **POST /event**: 
Este endpoint permite adicionar um evento à conta. Para fazer uma solicitação, envie uma requisição POST para `/accounts/event` com os seguintes parâmetros:

- `type:` O tipo de evento, que pode ser "deposit" (depósito), "withdraw" (retirada), "transfer" (transferência).
- `amount:` O valor do evento. Para depósitos, o valor deve ser positivo. Para retiradas, o valor deve ser negativo.
- `origin:` O valor do id da conta da qual o valor vai ser retirado (withdraw) ou transferido (transfer).
- `destination:` O valor do id da conta a qual vai ser criada (deposit) ou transferida (transfer).

### Corpo requisição:

POST /event deposit:
```json
{
"type": "deposit",
"destination": "idConta",
"amount": 50
}
```
POST /event withdraw:
```json
{
"type": "withdraw",
"origin": "100",
"amount": 5
}
```
Post /event transfer:
```json
{
"type": "transfer",
"destination": "idConta",
"origin": "idConta",
"amount": 5
}
```
