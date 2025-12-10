using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineController : MonoBehaviour
{
    public List<RectTransform> steps;      // 轨道上的 StepCards（按顺序）
    public RectTransform processingBox;
    public float moveSpeed = 6f;
    public StepFlyOut flyOut;

    // ✅ 新增：初始位置列表
    private List<Vector3> initialPositions = new List<Vector3>();
    private List<RectTransform> initialSteps = new List<RectTransform>();

    void Start()
    {
        InitSteps();
        MoveNextStep();
    }

    // ✅ 修改：初始化 steps 并重置位置
    public void InitSteps()
    {
        Debug.Log("=== InitSteps ===");

        // 第一次初始化时，保存初始状态
        if (initialSteps.Count == 0)
        {
            initialSteps = new List<RectTransform>(steps);

            // 保存每个 step 的初始位置
            foreach (var step in steps)
            {
                if (step != null)
                {
                    initialPositions.Add(step.anchoredPosition);
                }
            }
        }
        else
        {
            // 重新加载时，从初始列表恢复
            steps = new List<RectTransform>(initialSteps);
        }

        // ✅ 激活所有 steps 并重置位置
        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i] != null)
            {
                steps[i].gameObject.SetActive(true);

                // ✅ 恢复初始位置
                if (i < initialPositions.Count)
                {
                    steps[i].anchoredPosition = initialPositions[i];
                }

                Debug.Log($"✅ Step {i} 激活: {steps[i].name}, 位置: {steps[i].anchoredPosition}");
            }
        }

        GameRuntime.currentStep = steps[0].GetComponent<StepCardController>();

        Debug.Log($"Steps Count: {steps.Count}");
    }

    public void MoveNextStep()
    {
        Debug.Log("Checking references...");
        //Debug.Log("GameRuntime = " + (GameRuntime != null));
        Debug.Log("HandManager.Instance = " + (HandManager.Instance != null));
        Debug.Log("ResultPanelController.Instance = " + (ResultPanelController.Instance != null));

        // 如果没有剩余 Step：说明流程全部完成
        if (steps.Count == 0)
        {
            Debug.Log(" All steps finished — SHOW RESULT!");
            // 使用 GameRuntime 正确的字段名
            int finalExposure = GameRuntime.currentExposure;
            int goalExposure = GameRuntime.targetExposure;
            // 获取剩余出牌次数（从 HandManager 而不是 GameRuntime）
            int remainingPlays = HandManager.Instance.remainingPlays;
            // 调用结果界面
            ResultPanelController.Instance.ShowResult(finalExposure, goalExposure, remainingPlays);
            return;
        }

        // 还有 steps，继续移动下一张
        StartCoroutine(MoveToProcessing(steps[0]));
    }

    IEnumerator MoveToProcessing(RectTransform step)
    {
        Vector3 start = step.anchoredPosition;
        Vector3 target = processingBox.anchoredPosition;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            step.anchoredPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }
        GameRuntime.currentStep = step.GetComponent<StepCardController>();
    }

    //  外部调用：完成当前 step
    public void StepCompleted(RectTransform finishedStep)
    {
        StartCoroutine(StepCompletedRoutine(finishedStep));
    }

        IEnumerator StepCompletedRoutine(RectTransform finishedStep)
    {
        // 1. 获取当前 Step
        StepCardController step = finishedStep.GetComponent<StepCardController>();

        // 根据 step 名称决定权重（或你可以在 ScriptableObject 里配置）
        float wE = 1f;
        float wA = 1f;
        float wP = 1f;
        float wC = 1f;

        switch (step.stepTitle.text)
        {
            case "Post Theme":
                wE = 1.2f; wA = 1.0f; wP = 0.5f; wC = 0.5f;
                break;

            case "Title Crafting":
                wE = 1.0f; wA = 0.5f; wP = 1.0f; wC = 1.2f;
                break;

            case "Visual Composition":
                wE = 0.8f; wA = 0.8f; wP = 1.5f; wC = 0.5f;
                break;

            case "Hashtag Mapping":
                wE = 0.3f; wA = 0.5f; wP = 0.8f; wC = 1.5f;
                break;

            case "Timing Strategy":
                wE = 0.2f; wA = 0.3f; wP = 1.5f; wC = 1.0f;
                break;

            case "Publication":
                wE = 0.7f; wA = 0.7f; wP = 0.7f; wC = 0.7f;
                break;
        }

        // 3. 计算 Step 曝光值
        int stepExposure = step.CalculateStepExposure(wE, wA, wP, wC);
        // 加到总曝光
        GameRuntime.currentExposure += stepExposure;

        Debug.Log($"[Step Exposure] {step.stepTitle.text} → {stepExposure}");

        // 1) 先记录"剩余 steps 的原位置"
        List<Vector3> oldPositions = new List<Vector3>();
        foreach (var s in steps)
            oldPositions.Add(s.anchoredPosition);

        // 2) 移除并隐藏当前完成的 step
        int finishedIndex = steps.IndexOf(finishedStep);
        if (finishedIndex >= 0)
        {
            steps.RemoveAt(finishedIndex);
        }
        yield return StartCoroutine(flyOut.PlayFlyOut(finishedStep));

        // 3) 让剩余的 steps 补位：各自移动到"前一张的原位置"
        for (int i = 0; i < steps.Count; i++)
        {
            Vector3 start = steps[i].anchoredPosition;
            // i=0 的目标是 oldPositions[0]（也就是原来第 0 张的位置）
            // i=1 的目标是 oldPositions[1] ... 以此类推
            Vector3 target = oldPositions[i];
            yield return StartCoroutine(MoveRect(steps[i], start, target));
        }

        // 4) 补位完成后，再让新的第 0 张进 ProcessingBox
        MoveNextStep();
    }

    IEnumerator MoveRect(RectTransform rt, Vector3 start, Vector3 target)
    {
        Debug.Log("Moving: " + rt.name);
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            rt.anchoredPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
}