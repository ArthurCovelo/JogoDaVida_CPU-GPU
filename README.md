# Projeto Jogo da vida (GPU e CPU)
![preview](./.github/JogoDaVida.gif)

> Projeto: Um Jogo da vida (GPU e CPU) com Utilização Compute Shader.

Projeto desenvolvido por Arthur Covelo: Simulação do Jogo da Vida de John Conway, fazendo uso tanto da capacidade de processamento da GPU quanto da CPU. 

A ferramenta escolhida para essa implementação é a Unity3D.

 O Jogo da Vida é um autômato celular que busca simular a evolução de seres vivos com base em regras específicas.

### Implementação da Simulação:

A simulação será realizada por meio de um kernel implementado em um compute shader, permitindo aproveitar a capacidade de processamento paralelo da GPU para acelerar a execução. 
O uso de uma linguagem baseada em GLSL ou HLSL possibilitará a execução do kernel de processamento de maneira independente da ordem tradicional do pipeline gráfico.

### Regras do Jogo da Vida:

O Jogo da Vida segue as regras definidas por John Conway:

- Toda célula morta com exatamente três vizinhos vivos torna-se viva (nascimento).
- Toda célula viva com menos de dois vizinhos vivos morre por isolamento.
- Toda célula viva com mais de três vizinhos vivos morre por superpopulação.
- Toda célula viva com dois ou três vizinhos vivos permanece viva.

### Interação do Usuário:
O estado inicial da simulação será definido pelo usuário por meio do posicionamento das células no mundo virtual. 

Isso pode ser implementado por meio de uma interface interativa na Unity3D, permitindo que o usuário selecione as células iniciais que deseja ativar para a simulação.

### Processamento CPU e GPU:
A implementação será dividida em duas partes: processamento CPU e processamento GPU.

#### CPU: 
A CPU será responsável por receber as informações iniciais das células e aplicar as regras do Jogo da Vida em cada iteração.

#### GPU: 
A GPU, por meio do compute shader, realizará cálculos paralelos em massa para a evolução das células de acordo com as regras do Jogo da Vida.

## 🛠 Tecnologias

- Unity3D
- Compute Shader
- Git e Github

## 🖤 Contato

Arthurcovelo@gmail.com

[🔗 Clique aqui para acessar](https://arthurcovelo.github.io/ProjetoWeb_Profile/)

