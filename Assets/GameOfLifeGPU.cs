using UnityEngine;

public class GameOfLifeGPU : MonoBehaviour
{
    public int larguraTextura;
    public int alturaTextura;

    [Space]
    public ComputeShader shader;
    public RenderTexture rt;

    private int initKernel;
    private int mainKernel;
    private int tamanhoGrupoX = 10;
    private int tamanhoGrupoY = 10;

    private bool inputMode = true; // Modo de entrada ativado

    private void Start()
    {
        // Cria��o e configura��o da Render Texture
        rt = new RenderTexture(larguraTextura, alturaTextura, 0);
        rt.enableRandomWrite = true;
        rt.Create();

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_MainTex", rt);

        InicializarComputeShader(); // Inicializa o Compute Shader
        InicializarSimulacao(); // Inicializa a simula��o
    }

    public void Update()
    {
        if (inputMode)
        {
            // Modo de entrada ativado
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                // Verifica se o bot�o esquerdo ou direito do mouse est� pressionado
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Obt�m as coordenadas do pixel com base na posi��o de impacto do raio
                    Vector2 textureCoord = hit.textureCoord;
                    Vector2Int pixelCoord = new Vector2Int((int)(textureCoord.x * larguraTextura), (int)(textureCoord.y * alturaTextura));
                    bool ativar = Input.GetMouseButton(0); // true para bot�o esquerdo, false para bot�o direito
                    AlterarEstadoCelula(pixelCoord, ativar); // Altera o estado da c�lula
                }
            }
        }
        else
        {
            // Modo de entrada desativado
            DefinirParametrosShader(); // Define os par�metros do Compute Shader
            ExecutarComputeShader(); // Executa o Compute Shader
        }
    }

    private void OnDestroy()
    {
        DesligarComputeShader(); // Desliga o Compute Shader
    }

    public void InicializarSimulacao()
    {
        initKernel = shader.FindKernel("CSInit"); // Obt�m o kernel "CSInit" do Compute Shader
        shader.SetTexture(initKernel, "Result", rt); // Define a Render Texture como textura de sa�da do kernel
        shader.Dispatch(initKernel, Mathf.CeilToInt(larguraTextura / (float)tamanhoGrupoX), Mathf.CeilToInt(alturaTextura / (float)tamanhoGrupoY), 1); // Executa o kernel
    }

    public void InicializarComputeShader()
    {
        mainKernel = shader.FindKernel("CSMain"); // Obt�m o kernel "CSMain" do Compute Shader
    }

    public void DefinirParametrosShader()
    {
        shader.SetTexture(mainKernel, "Result", rt); // Define a Render Texture como textura de sa�da do kernel
    }

    private void ExecutarComputeShader()
    {
        shader.Dispatch(mainKernel, Mathf.CeilToInt(larguraTextura / (float)tamanhoGrupoX), Mathf.CeilToInt(alturaTextura / (float)tamanhoGrupoY), 1); // Executa o kernel
    }

    private void DesligarComputeShader()
    {
        rt.Release(); // Libera a Render Texture
    }

    private void AlterarEstadoCelula(Vector2Int pixelCoord, bool ativar)
    {
        if (pixelCoord.x >= 0 && pixelCoord.x < larguraTextura && pixelCoord.y >= 0 && pixelCoord.y < alturaTextura)
        {
            Color color = ativar ? Color.white : Color.black; // Define a cor com base no estado da c�lula
            PintarCelula(pixelCoord, color); // Pinta a c�lula com a cor definida
        }
    }

    private void PintarCelula(Vector2Int pixelCoord, Color color)
    {
        if (pixelCoord.x >= 0 && pixelCoord.x < larguraTextura && pixelCoord.y >= 0 && pixelCoord.y < alturaTextura)
        {
            RenderTexture.active = rt; // Define a Render Texture como ativa
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, larguraTextura, 0, alturaTextura);
            GL.Begin(GL.QUADS);
            GL.Color(color);
            GL.Vertex3(pixelCoord.x, pixelCoord.y, 0);
            GL.Vertex3(pixelCoord.x + 1, pixelCoord.y, 0);
            GL.Vertex3(pixelCoord.x + 1, pixelCoord.y + 1, 0);
            GL.Vertex3(pixelCoord.x, pixelCoord.y + 1, 0);
            GL.End();
            GL.PopMatrix();
            RenderTexture.active = null; // Desativa a Render Texture
        }
    }

    public void IniciarSimulacao()
    {
        inputMode = false; // Desativa o modo de entrada para iniciar a simula��o
    }

    public void SetEstadoInicial(bool[,] estadoInicial)
    {
        // Define o estado inicial das c�lulas no compute shader
        for (int x = 0; x < larguraTextura; x++)
        {
            for (int y = 0; y < alturaTextura; y++)
            {
                shader.SetInt("InitialCell_" + x.ToString() + "_" + y.ToString(), estadoInicial[x, y] ? 1 : 0); // Define o valor no shader
            }
        }
    }
}
