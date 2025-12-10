using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public GameObject phase1;
    public GameObject phase2;

    void OnEnable()
    {
        Debug.Log("=== TransitionManager OnEnable ===");
        StartCoroutine(DoTransition());
    }

    IEnumerator DoTransition()
    {
        phase1.SetActive(true);
        phase2.SetActive(false);

        Debug.Log("🎬 Phase1 显示，等待玩家点击...");

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        phase1.SetActive(false);
        phase2.SetActive(true);

        Debug.Log("🎬 Phase2 显示");
    }

    public void StartStage()
    {
        Debug.Log("=== TransitionManager.StartStage 被调用 ===");
        Debug.Log($"当前 currentStage: {UIManager.currentStage}");

        phase2.SetActive(false);

        // ✅ 检查关卡数据
        if (UIManager.currentStage >= UIManager.Instance.stages.Length)
        {
            Debug.LogError($"❌ currentStage ({UIManager.currentStage}) 超出范围！stages 数组长度: {UIManager.Instance.stages.Length}");
            return;
        }
        
        // ✅ 获取当前关卡数据
        StageData currentStageData = UIManager.Instance.stages[UIManager.currentStage];
        Debug.Log($" 关卡数据: {currentStageData.stageTitle}, 目标: {currentStageData.targetExposure}");

        // ✅ 从 StageData 读取目标
        int newTargetExposure = currentStageData.targetExposure;

        // ✅ 显示重置前的数据
        Debug.Log($"重置前 - Exposure: {GameRuntime.currentExposure}, Target: {GameRuntime.targetExposure}");

        // ✅ 重置 GameRuntime
        GameRuntime.ResetForNewStage(currentStageData.targetExposure);

        // ✅ 显示重置后的数据
        Debug.Log($"重置后 - Exposure: {GameRuntime.currentExposure}, Target: {GameRuntime.targetExposure}, Plays: {GameRuntime.remainingPlays}");

        // ✅ 重置 HandManager
        if (HandManager.Instance != null)
        {
            HandManager.Instance.remainingPlays = GameRuntime.maxPlays;
            Debug.Log($"✅ HandManager 出牌次数重置为: {HandManager.Instance.remainingPlays}");
        }

        // ✅ 显示 Stage
        UIManager.Instance.ShowStage();

        // ✅ 刷新 HUD
        if (HUDController.Instance != null)
        {
            Debug.Log("🔄 调用 HUDController.RefreshAll()");
            HUDController.Instance.RefreshAll();
        }
        else
        {
            Debug.LogError("❌ HUDController.Instance 为空！");
        }

        Debug.Log("✅ StartStage 完成");
    }
}