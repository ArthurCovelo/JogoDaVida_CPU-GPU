using UnityEngine;

public class GameOfLifeCPU : MonoBehaviour
{
    public int larguraTextura; // Largura da textura (quantidade de c�lulas na horizontal)
    public int alturaTextura; // Altura da textura (quantidade de c�lulas na vertical)

    [Space]
    public Material material; // Material usado para renderizar a textura
    public int QuantidadeCelulas; // Quantidade total de c�lulas na simula��o
    private bool[,] estadoAtual; // Estado atual das c�lulas
    private Texture2D texture; // Textura usada para representar as c�lulas

    private bool inputMode = true; // Modo de entrada ativado

    private void Start()
    {
        estadoAtual = new bool[larguraTextura, alturaTextura]; // Inicializa o estado atual das c�lulas
        texture = new Texture2D(larguraTextura, alturaTextura); // Cria a textura com as dimens�es especificadas
        Renderer renderer = GetComponent<Renderer>(); // Obt�m o componente Renderer do objeto
        renderer.material = material; // Define o material do Renderer
        renderer.material.mainTexture = texture; // Define a textura do material como a textura criada

        InicializarSimulacao(larguraTextura * alturaTextura / QuantidadeCelulas); // Inicializa a simula��o com a quantidade de c�lulas desejada
    }

    public void Update()
    {
        if (inputMode)
        {
            // Modo de entrada ativado

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                // Quando o bot�o esquerdo ou direito do mouse � pressionado

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Lan�a um raio a partir da posi��o do mouse e verifica se colide com algum objeto

                    Vector2 textureCoord = hit.textureCoord;
                    Vector2Int pixelCoord = new Vector2Int((int)(textureCoord.x * larguraTextura), (int)(textureCoord.y * alturaTextura));
                    bool ativar = Input.GetMouseButton(0); // true para bot�o esquerdo, false para bot�o direito
                    AlterarEstadoCelula(pixelCoord, ativar);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Quando a tecla de espa�o � pressionada, o modo de entrada � desativado

                inputMode = false;
            }
        }
        else
        {
            // Modo de entrada desativado

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Quando a tecla de espa�o � pressionada, o modo de entrada � ativado novamente

                inputMode = true;
            }

            AtualizarSimulacao(); // Atualiza a simula��o
        }
    }

    public void InicializarSimulacao(int quantidadeCelulas)
    {
        estadoAtual = new bool[larguraTextura, alturaTextura]; // Reinicia o estado atual das c�lulas
        texture.SetPixels32(GerarCoresIniciais(quantidadeCelulas)); // Gera as cores iniciais das c�lulas na textura
        texture.Apply(); // Aplica as altera��es na textura
    }

    private Color32[] GerarCoresIniciais(int quantidadeCelulas)
    {
        Color32[] colors = new Color32[larguraTextura * alturaTextura];
        for (int x = 0; x < larguraTextura; x++)
        {
            for (int y = 0; y < alturaTextura; y++)
            {
                bool ativo = Random.value > 0.75f; // Define aleatoriamente se a c�lula est� ativa ou n�o
                estadoAtual[x, y] = ativo; // Atualiza o estado atual da c�lula
                colors[x + y * larguraTextura] = ativo ? Color.white : Color.black; // Define a cor da c�lula na textura

                quantidadeCelulas--;
                if (quantidadeCelulas <= 0)
                    return colors;
            }
        }
        return colors;
    }

    private void AtualizarSimulacao()
    {
        bool[,] novoEstado = new bool[larguraTextura, alturaTextura]; // Cria um novo estado das c�lulas

        for (int x = 0; x < larguraTextura; x++)
        {
            for (int y = 0; y < alturaTextura; y++)
            {
                int vizinhosVivos = ContarVizinhosVivos(x, y); // Conta a quantidade de vizinhos vivos da c�lula atual
                bool celulaAtual = estadoAtual[x, y]; // Obt�m o estado atual da c�lula

                if (celulaAtual && (vizinhosVivos == 2 || vizinhosVivos == 3))
                {
                    // Se a c�lula est� viva e tem 2 ou 3 vizinhos vivos, continua viva

                    novoEstado[x, y] = true;
                    texture.SetPixel(x, y, Color.white);
                }
                else if (!celulaAtual && vizinhosVivos == 3)
                {
                    // Se a c�lula est� morta e tem exatamente 3 vizinhos vivos, nasce

                    novoEstado[x, y] = true;
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    // Caso contr�rio, a c�lula morre

                    novoEstado[x, y] = false;
                    texture.SetPixel(x, y, Color.black);
                }
            }
        }

        estadoAtual = novoEstado; // Atualiza o estado atual das c�lulas
        texture.Apply(); // Aplica as altera��es na textura
    }

    private int ContarVizinhosVivos(int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int neighborX = x + i;
                int neighborY = y + j;

                if (neighborX >= 0 && neighborX < larguraTextura && neighborY >= 0 && neighborY < alturaTextura)
                {
                    if (estadoAtual[neighborX, neighborY])
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    private void AlterarEstadoCelula(Vector2Int pixelCoord, bool ativar)
    {
        if (pixelCoord.x >= 0 && pixelCoord.x < larguraTextura && pixelCoord.y >= 0 && pixelCoord.y < alturaTextura)
        {
            estadoAtual[pixelCoord.x, pixelCoord.y] = ativar; // Atualiza o estado da c�lula
            texture.SetPixel(pixelCoord.x, pixelCoord.y, ativar ? Color.white : Color.black); // Atualiza a cor da c�lula na textura
            texture.Apply(); // Aplica as altera��es na textura
        }
    }

    public void IniciarSimulacao()
    {
        inputMode = false; // Desativa o modo de entrada
    }

    public void SetEstadoInicial(bool[,] estadoInicial)
    {
        estadoAtual = estadoInicial; // Define o estado inicial das c�lulas
        AtualizarTextura(); // Atualiza a textura com base no estado inicial das c�lulas
    }

    private void AdicionarCelula(Vector2Int pixelCoord)
    {
        if (pixelCoord.x >= 0 && pixelCoord.x < larguraTextura && pixelCoord.y >= 0 && pixelCoord.y < alturaTextura)
        {
            estadoAtual[pixelCoord.x, pixelCoord.y] = true; // Ativa a c�lula
            texture.SetPixel(pixelCoord.x, pixelCoord.y, Color.white); // Atualiza a cor da c�lula na textura
            texture.Apply(); // Aplica as altera��es na textura
        }
    }

    private void AtualizarTextura()
    {
        Color32[] colors = new Color32[larguraTextura * alturaTextura];
        for (int x = 0; x < larguraTextura; x++)
        {
            for (int y = 0; y < alturaTextura; y++)
            {
                colors[x + y * larguraTextura] = estadoAtual[x, y] ? Color.white : Color.black;
            }
        }
        texture.SetPixels32(colors);
        texture.Apply();
    }
}
