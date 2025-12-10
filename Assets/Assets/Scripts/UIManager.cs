using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject creatorStrategyPanel;
    public GameObject cardSelectPanel;
    public GameObject stagePanel;

    [Header("Strategy Hands")]
    public Image handsLeft;
    public Image handsRight;

    [Header("Strategy Hands")]
    public Image leftHandImage;
    public Image rightHandImage;

    public Sprite[] leftHandSprites;   // 3 sprites
    public Sprite[] rightHandSprites;  // 3 sprites

    [Header("Hand Sprites")]
    public Sprite leftPerformer;
    public Sprite rightPerformer;

    public Sprite leftEmpathiser;
    public Sprite rightEmpathiser;

    public Sprite leftOptimizer;
    public Sprite rightOptimizer;

    [Header("Game HUD (Always in GamePlay Scene)")]
    public GameObject TopBar;
    public GameObject CardArea;
    public GameObject BottomBar;

    [Header("Starter Card Pools")]
    public StarterCard[] performerStarters;
    public StarterCard[] empathiserStarters;
    public StarterCard[] optimizerStarters;

    // 玩家永久卡池（从商店购买的卡）
    [Header("Player Cards")]
    public List<StarterCard> unlockedCards = new List<StarterCard>();

    public CreatorStrategy[] strategies;   // array of 3 strategies
    public static CreatorStrategy selectedStrategyData;

    public StageData[] stages;     // 关卡数据列表
    public static int currentStage = 0;   // 从0开始 = Stage 1

    public GameObject shopPanel;

    public static UIManager Instance;
    void Awake()
    {
        Instance = this;
    }


    // 记录玩家选中的起始卡
    public static List<StarterCard> selectedStarterCards = new List<StarterCard>();

    // ?? 这句一定要写在 class 里面、方法外面
    public static int selectedStrategy = 1;

    private void Start()
    {
        // 游戏一开始默认显示创作者策略页
        ShowCreatorStrategy();
    }

    public void UpdateHands()
    {
        if (handsLeft == null || handsRight == null)
            return;

        switch (selectedStrategy)
        {
            case 0: // Performer
                handsLeft.sprite = leftPerformer;
                handsRight.sprite = rightPerformer;
                break;

            case 1: // Empathiser
                handsLeft.sprite = leftEmpathiser;
                handsRight.sprite = rightEmpathiser;
                break;

            case 2: // Optimizer
                handsLeft.sprite = leftOptimizer;
                handsRight.sprite = rightOptimizer;
                break;
        }

        handsLeft.gameObject.SetActive(true);
        handsRight.gameObject.SetActive(true);
    }

    public void ShowCreatorStrategy()
    {
        creatorStrategyPanel.SetActive(true);
        cardSelectPanel.SetActive(false);

        TopBar.SetActive(false);
        CardArea.SetActive(false);
        BottomBar.SetActive(false);
    }

    public void ShowCardSelect()
    {
        creatorStrategyPanel.SetActive(false);
        cardSelectPanel.SetActive(true);

        TopBar.SetActive(false);
        CardArea.SetActive(false);
        BottomBar.SetActive(false);
    }

    // 以后 Confirm 按钮可以调用这个，把当前策略 index 存起来
    public void SetStrategy(int index)
    {
        selectedStrategy = index;
        selectedStrategyData = strategies[index];
        Debug.Log("Selected Strategy: " + selectedStrategyData.strategyName);

        // ★ 更换左右两只手的图片
        if (leftHandImage != null && rightHandImage != null)
        {
            leftHandImage.sprite = leftHandSprites[index];
            rightHandImage.sprite = rightHandSprites[index];
        }

    }

    public GameObject transitionPanel;

    public void ShowTransition()
    {
        creatorStrategyPanel.SetActive(false);
        cardSelectPanel.SetActive(false);
        stagePanel.SetActive(false);

        // ★ 自动关闭售卖框
        if (CardSellPanel.Instance != null)
            CardSellPanel.Instance.Hide();

        transitionPanel.SetActive(true);
    }

    public void ShowStage()
    {
        // === 1) 设置本关的目标曝光值 ===
        int target = 120;  // 默认 Stage 1

        if (currentStage == 1)
            target = 240;  // Stage 2
        else if (currentStage == 2)
            target = 420;  // Stage 3
        GameRuntime.targetExposure = target;

        GameRuntime.ResetForNewStage(target);
        Debug.Log("🎯 当前设置目标曝光 = " + target);
        Debug.Log("🎯 GameRuntime.targetExposure = " + GameRuntime.targetExposure);

        creatorStrategyPanel.SetActive(false);
        cardSelectPanel.SetActive(false);
        transitionPanel.SetActive(false);
        stagePanel.SetActive(true);

        // ✅ 每次进入 Stage 都重新初始化 Steps
        PipelineController pc = stagePanel.GetComponentInChildren<PipelineController>();
        if (pc != null)
        {
            Debug.Log("找到 PipelineController，调用 InitSteps");
            pc.InitSteps();
        }
        else
        {
            Debug.LogError("❌ 找不到 PipelineController！");
        }

        // ✅ 初始化手牌
        if (HandManager.Instance != null)
        {
            HandManager.Instance.InitHand();
        }
        else
        {
            Debug.LogError("❌ HandManager.Instance 为空！");
        }

        if (CardSellPanel.Instance != null)
            CardSellPanel.Instance.Hide();

        // ✅ 刷新 HUD
        if (HUDController.Instance != null)
        {
            HUDController.Instance.RefreshAll();
        }
    }

    public void ShowResult()
    {
        Debug.Log("结算页功能还没做，这只是占位！");
    }
    // ======================
    // 注意力碎片（最小可用功能）
    // ======================
    public int totalFragments = 0;

    public void AddFragments(int amount)
    {
        totalFragments += amount;
        Debug.Log("Fragments added: " + totalFragments);
    }



    // ======================
    // 重开当前关卡（最简实现）
    // ======================
    public void RestartStage()
    {
        // 最简单稳定的做法：重新加载当前场景
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }



    // ======================
    // 返回主菜单（如果没有主菜单就回到当前场景）
    // ======================
    public void ReturnToMainMenu()
    {
        // 如果你没有 MainMenu，就加载 Scene 0 或当前场景
        // 你可以自己改成你项目的 MainMenu 场景 index
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ShowShop()
    {
        // 关闭其他页面，只打开商店
        creatorStrategyPanel.SetActive(false);
        cardSelectPanel.SetActive(false);
        transitionPanel.SetActive(false);
        stagePanel.SetActive(false);

        shopPanel.SetActive(true);
    }

}


