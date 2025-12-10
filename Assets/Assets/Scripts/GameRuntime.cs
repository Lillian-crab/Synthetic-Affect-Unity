using UnityEngine;

public class GameRuntime : MonoBehaviour
{
    //public static GameRuntime Instance;
    public static int maxPlays = 5;          // 每局最大可出牌次数
    public static int remainingPlays = 5;    // 当前剩余次数
    public static int currentE;
    public static int currentA;
    public static int currentP;
    public static int currentC;

    private void Awake()
    {
        // ✅ 添加单例模式 + DontDestroyOnLoad
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);  // ← 关键：场景切换时保留
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    // 当前正在处理的 StepCard
    public static StepCardController currentStep;

    // 玩家本局手牌（来自 UIManager.selectedStarterCards）
    public static StarterCard[] handCards;

    public static int currentExposure = 0;
    public static int targetExposure = 10;
    public static int attentionFragments = 0;   // ← 新增，玩家当前注意力碎片

    // ✅ 新增：重置关卡数据
    public static void ResetForNewStage(int newTargetExposure)
    {
        Debug.Log("=== ResetForNewStage ===");
        Debug.Log($"旧 currentExposure: {currentExposure}");

        // ✅ 选择1：每关重新计分（从0开始）
        currentExposure = 0;

        // ✅ 选择2：累积分数（注释上面一行，打开下面这行）
        // currentExposure = currentExposure; // 保留累积

        targetExposure = newTargetExposure;
        remainingPlays = maxPlays;

        Debug.Log($"新 currentExposure: {currentExposure}");
        Debug.Log($"新 targetExposure: {targetExposure}");
        Debug.Log($"remainingPlays: {remainingPlays}");
    }
}