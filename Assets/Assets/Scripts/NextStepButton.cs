using UnityEngine;

public class NextStepButton : MonoBehaviour
{
    public void OnNextStep()
    {
        var step = GameRuntime.currentStep;

        if (step == null)
        {
            Debug.LogError("没有当前步骤！");
            return;
        }

        // 1) 先结算 pending
        step.ApplyPending();

        // 2) 再推进一次
        FindObjectOfType<PipelineController>()
            .StepCompleted((RectTransform)step.transform);
    }
}
