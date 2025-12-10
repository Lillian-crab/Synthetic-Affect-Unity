using UnityEngine;

public class StepManager : MonoBehaviour
{
    public static StepManager Instance;

    public GameObject stagePanel;
    public GameObject[] steps;    // Step1 ~ Step6

    private int currentStep = 0;

    void Awake()
    {
        Instance = this;
    }

    public void StartStage()
    {
        stagePanel.SetActive(true);

        // 激活第一步
        currentStep = 0;
        ShowStep(currentStep);
    }

    public void NextStep()
    {
        currentStep++;

        if (currentStep >= steps.Length)
        {
            EndStage();
            return;
        }

        ShowStep(currentStep);
    }

    void ShowStep(int index)
    {
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].SetActive(i == index);
        }
    }

    void EndStage()
    {
        stagePanel.SetActive(false);

        // 完成本关 → 进入下一关 TransitionPanel
        UIManager.currentStage++;
        UIManager.Instance.ShowTransition();
    }
}
