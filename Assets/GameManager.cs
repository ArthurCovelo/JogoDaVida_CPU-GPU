using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameOfLifeGPU gameOfLifeGPU; // Referência ao script GameOfLifeGPU
    public GameOfLifeCPU gameOfLifeCPU; // Referência ao script GameOfLifeCPU
    public GameObject startButton; // Referência ao botão de iniciar simulação

    private bool simulationRunning; // Indica se a simulação está em execução
    private float gpuStartTime; // Tempo de início da execução da GPU
    private float cpuStartTime; // Tempo de início da execução da CPU

    public GameObject objetoGPU;
    public GameObject objetoCPU;
    public void StartSimulationGPU()
    {
        gameOfLifeGPU.enabled = true; // Ativa o script GameOfLifeGPU
        gameOfLifeCPU.enabled = false; // Desativa o script GameOfLifeCPU
        objetoCPU.SetActive(false);

        gameOfLifeGPU.IniciarSimulacao(); // Inicia a simulação usando GPU
        simulationRunning = true; // Define que a simulação está em execução
        startButton.SetActive(false); // Desativa o botão de iniciar simulação
        gpuStartTime = Time.realtimeSinceStartup; // Registra o tempo de início da execução da GPU
    }

    public void StartSimulationCPU()
    {
       
        gameOfLifeCPU.enabled = true; // Ativa o script GameOfLifeCPU
        gameOfLifeGPU.enabled = false; // Desativa o script GameOfLifeGPU
        
        objetoGPU.SetActive(false);

        gameOfLifeCPU.IniciarSimulacao(); // Inicia a simulação usando CPU
        simulationRunning = true; // Define que a simulação está em execução
        startButton.SetActive(false); // Desativa o botão de iniciar simulação
        cpuStartTime = Time.realtimeSinceStartup; // Registra o tempo de início da execução da CPU
    }

    private void Update()
    {
        objetoGPU = GameObject.Find("ManagerGPU");
        objetoCPU = GameObject.Find("ManagerCPU");

        if (simulationRunning)
        {
            if (gameOfLifeGPU.enabled)
            {
                gameOfLifeGPU.IniciarSimulacao(); // Continua a execução da simulação usando GPU
                CheckSimulationComplete(); // Verifica se a simulação terminou
            }
            else if (gameOfLifeCPU.enabled)
            {
                gameOfLifeCPU.IniciarSimulacao(); // Continua a execução da simulação usando CPU
                CheckSimulationComplete(); // Verifica se a simulação terminou
            }
        }
    }

    private void CheckSimulationComplete()
    {
        if (gameOfLifeGPU.enabled)
        {
            simulationRunning = false; // A simulação terminou, portanto, define que não está mais em execução
            startButton.SetActive(true); // Ativa novamente o botão de iniciar simulação
            float gpuEndTime = Time.realtimeSinceStartup; // Tempo de término da execução da GPU
            float gpuExecutionTime = gpuEndTime - gpuStartTime; // Calcula o tempo total de execução da GPU
            Debug.Log("GPU Tempo de Execução: " + gpuExecutionTime + " Segundos"); // Exibe o tempo de execução da GPU no console
        }
        else if (gameOfLifeCPU.enabled)
        {
            simulationRunning = false; // A simulação terminou, portanto, define que não está mais em execução
            startButton.SetActive(true); // Ativa novamente o botão de iniciar simulação
            float cpuEndTime = Time.realtimeSinceStartup; // Tempo de término da execução da CPU
            float cpuExecutionTime = cpuEndTime - cpuStartTime; // Calcula o tempo total de execução da CPU
            Debug.Log("CPU Tempo de Execução: " + cpuExecutionTime + " Segundos"); // Exibe o tempo de execução da CPU no console
        }
    }
}
