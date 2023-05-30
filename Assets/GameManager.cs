using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameOfLifeGPU gameOfLifeGPU; // Refer�ncia ao script GameOfLifeGPU
    public GameOfLifeCPU gameOfLifeCPU; // Refer�ncia ao script GameOfLifeCPU
    public GameObject startButton; // Refer�ncia ao bot�o de iniciar simula��o

    private bool simulationRunning; // Indica se a simula��o est� em execu��o
    private float gpuStartTime; // Tempo de in�cio da execu��o da GPU
    private float cpuStartTime; // Tempo de in�cio da execu��o da CPU

    public GameObject objetoGPU;
    public GameObject objetoCPU;
    public void StartSimulationGPU()
    {
        gameOfLifeGPU.enabled = true; // Ativa o script GameOfLifeGPU
        gameOfLifeCPU.enabled = false; // Desativa o script GameOfLifeCPU
        objetoCPU.SetActive(false);

        gameOfLifeGPU.IniciarSimulacao(); // Inicia a simula��o usando GPU
        simulationRunning = true; // Define que a simula��o est� em execu��o
        startButton.SetActive(false); // Desativa o bot�o de iniciar simula��o
        gpuStartTime = Time.realtimeSinceStartup; // Registra o tempo de in�cio da execu��o da GPU
    }

    public void StartSimulationCPU()
    {
       
        gameOfLifeCPU.enabled = true; // Ativa o script GameOfLifeCPU
        gameOfLifeGPU.enabled = false; // Desativa o script GameOfLifeGPU
        
        objetoGPU.SetActive(false);

        gameOfLifeCPU.IniciarSimulacao(); // Inicia a simula��o usando CPU
        simulationRunning = true; // Define que a simula��o est� em execu��o
        startButton.SetActive(false); // Desativa o bot�o de iniciar simula��o
        cpuStartTime = Time.realtimeSinceStartup; // Registra o tempo de in�cio da execu��o da CPU
    }

    private void Update()
    {
        objetoGPU = GameObject.Find("ManagerGPU");
        objetoCPU = GameObject.Find("ManagerCPU");

        if (simulationRunning)
        {
            if (gameOfLifeGPU.enabled)
            {
                gameOfLifeGPU.IniciarSimulacao(); // Continua a execu��o da simula��o usando GPU
                CheckSimulationComplete(); // Verifica se a simula��o terminou
            }
            else if (gameOfLifeCPU.enabled)
            {
                gameOfLifeCPU.IniciarSimulacao(); // Continua a execu��o da simula��o usando CPU
                CheckSimulationComplete(); // Verifica se a simula��o terminou
            }
        }
    }

    private void CheckSimulationComplete()
    {
        if (gameOfLifeGPU.enabled)
        {
            simulationRunning = false; // A simula��o terminou, portanto, define que n�o est� mais em execu��o
            startButton.SetActive(true); // Ativa novamente o bot�o de iniciar simula��o
            float gpuEndTime = Time.realtimeSinceStartup; // Tempo de t�rmino da execu��o da GPU
            float gpuExecutionTime = gpuEndTime - gpuStartTime; // Calcula o tempo total de execu��o da GPU
            Debug.Log("GPU Tempo de Execu��o: " + gpuExecutionTime + " Segundos"); // Exibe o tempo de execu��o da GPU no console
        }
        else if (gameOfLifeCPU.enabled)
        {
            simulationRunning = false; // A simula��o terminou, portanto, define que n�o est� mais em execu��o
            startButton.SetActive(true); // Ativa novamente o bot�o de iniciar simula��o
            float cpuEndTime = Time.realtimeSinceStartup; // Tempo de t�rmino da execu��o da CPU
            float cpuExecutionTime = cpuEndTime - cpuStartTime; // Calcula o tempo total de execu��o da CPU
            Debug.Log("CPU Tempo de Execu��o: " + cpuExecutionTime + " Segundos"); // Exibe o tempo de execu��o da CPU no console
        }
    }
}
