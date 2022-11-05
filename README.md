# Teste Credz C#


## Status: <b> Em Desenvolvimento v1 </b>

## Geral

O objetivo do teste é testar os conhecimentos do candidato no framework .NET e linguagem C#.
Será analisado a sintaxe C#, legibilidade de código, concorrência, alocação de memória, sincronização, problemas aparentes de layout, performance, uso de recursos e solução de problemas de negócio.

### Contexto
O teste é composto de três aplicações, sendo:

<b>TimeApi:</b> O TimerApi é uma API Rest em .NET Core que possui um método GET que retorna um DateTime atual. Antes de retornar o DateTime, ele usa um delay aleatório de 1s até 10s a fim de simular uma API com problemas de performance.

<b>Portal:</b> O Portal é uma aplicação ASPNET Core que possui 1 action, Index, que é responsável por gerar uma chave aleatória, usando as informações obtidas da TimeApi. A aplicação Portal possui uma restrição de threads disponíveis para simular um servidor com alta carga de trabalho.
No projeto Portal, há uma propriedade estática na classe Program, a NUMBER_OF_VIRTUAL_MACHINES, que simula a quantidade de máquinas utilizadas para atender as requisições.
A simulação utiliza a fórmula (Número de Cores * NUMBER_OF_VIRTUAL_MACHINES) para restringir o número de threads da aplicação Portal, ou seja, 1 máquina da simulação é igual a 8 threads.
Ex.: Se a máquina possui 8 cores e NUMBER_OF_VIRTUAL_MACHINES = 10, então a aplicação Portal simula a utilização de 10 máquinas e poderá utilizar até 80 threads.

<b>Requester:</b> O Requester é uma aplicação console em .NET Core que faz chamadas paralelas para o Portal.
O seu objetivo é refatorar a aplicação, que é funcional, para utilizar o MENOR número possível em NUMBER_OF_VIRTUAL_MACHINES para atender no mínimo 50 requisições simultaneas sem receber BadRequest.

### Regras

A aplicação está funcional. Quando voce acessar http://localhost:5000/Home será calculado uma chave e retornará as informações do processo.
NUMBER_OF_VIRTUAL_MACHINES deve ser no mínimo 1, ou seja, simular a utilização de no mínimo um servidor.
A configuração default de NUMBER_OF_VIRTUAL_MACHINES é 50, o que garante o funcionamento da aplicação e simula uma estrutura de infraestrutura superdimensionada.
Você não possui gestão sobre a TimeAPI, então, não poderá alterar o seu código fonte.
Seu objetivo é refatorar a aplicação Portal.
Você pode refatorar a aplicação Requester, caso julgue necessário.
Use o tempo que achar necessário.
Ao finalizar o teste, envie o link de um repositório Git, onde todo o testes e suas alterações deverão estar comitados.


### Solução
