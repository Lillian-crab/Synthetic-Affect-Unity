using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    [Header("Hand")]
    public Transform handArea;
    public GameObject handCardPrefab;

    [Header("Play Limit")]
    public int totalPlays = 5;       // 默认本局出牌上限
    public int remainingPlays;       // 剩余可用次数
    public TMP_Text playCountText;   // UI 显示：Plays: x/x

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        remainingPlays = totalPlays;
        // 不在 Start 生成手牌！等待 UIManager.ShowStage() 调用 InitHand()

    }

    // ===== 手牌系统 =====
    public void InitHand()
    {
        // STEP 1：构建玩家的卡池
        List<StarterCard> playerPool = new List<StarterCard>();

        // 起始卡（第一关用）
        playerPool.AddRange(UIManager.selectedStarterCards);

        // 商店购买的卡
        playerPool.AddRange(UIManager.Instance.unlockedCards);

        // 最大手牌 5 张
        int handCount = Mathf.Min(playerPool.Count, 5);

        // 清空当前手牌
        foreach (Transform child in handArea)
            Destroy(child.gameObject);

        // 生成手牌
        for (int i = 0; i < handCount; i++)
        {
            CreateOneHandCard(playerPool[i]);
        }

        // 重置出牌次数
        remainingPlays = totalPlays;
        RefreshPlayUI();
    }


    // ===== 出牌次数系统 =====

    public bool CanPlay()
    {
        return remainingPlays > 0;
    }

    public void UseOnePlay()
    {
        remainingPlays--;
        if (remainingPlays < 0)
            remainingPlays = 0;

        RefreshPlayUI();
    }

    public void RefreshPlayUI()
    {
        if (playCountText != null)
            playCountText.text = $"Plays: {remainingPlays}/{totalPlays}";
    }

    public void CreateOneHandCard(StarterCard card)
    {
        GameObject c = Instantiate(handCardPrefab, handArea);
        c.GetComponent<StarterHandItem>().Init(card);
    }

    public void RemoveCard(StarterCard card)
    {
        // 在 handArea 下找到这个卡对应的 UI，并销毁
        foreach (Transform child in handArea)
        {
            StarterHandItem item = child.GetComponent<StarterHandItem>();

            if (item != null && item.cardData == card)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }
}
