using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class StepCardController : MonoBehaviour
{
    public TMP_Text stepTitle;
    public TMP_Text baseExposureText;
    public TMP_Text appliedEffectText;

    public int baseExposure = 100;

    public int CalculateStepExposure(float wE, float wA, float wP, float wC)
    {
        int exposure =
            baseExposure
          + Mathf.RoundToInt(GameRuntime.currentE * wE)
          + Mathf.RoundToInt(GameRuntime.currentA * wA)
          + Mathf.RoundToInt(GameRuntime.currentP * wP)
          + Mathf.RoundToInt(GameRuntime.currentC * wC);

        return exposure;
    }

    // pending 卡最多 2 张
    public List<StarterCard> pendingCards = new List<StarterCard>();
    public List<GameObject> pendingCardObjects = new List<GameObject>();  // ← 添加这行

    public Image icon1;
    public Image icon2;

    void Start()
    {
        baseExposureText.text = $"Base Exposure: +{baseExposure}";
        appliedEffectText.text = "";
        UpdateIcons();
    }

    // ✅ 被 DropHandler 调用：只“登记卡”，不结算、不推进
    public void AddPendingCard(StarterCard card)
    {
        if (pendingCards.Count >= 2) return;   // 最多两张
        pendingCards.Add(card);
        UpdateIcons();
    }

    public void UpdateIcons()
    {
        if (icon1 != null) icon1.gameObject.SetActive(pendingCards.Count >= 1);
        if (icon2 != null) icon2.gameObject.SetActive(pendingCards.Count >= 2);
    }

    // ✅ 只算效果，不推进
    public void ApplyPending()
    {
        appliedEffectText.text = "";

        int totalE = 0;
        int totalA = 0;
        int totalP = 0;
        int totalC = 0;

        // --- 应用所有卡的效果 ---
        foreach (var card in pendingCards)
        {
            // 显示效果
            appliedEffectText.text += $"E {card.E}, A {card.A}, P {card.P}, C {card.C}\n";

            //// 曝光量计算（保留你原本的逻辑）
            //int gain = card.P * 5 + card.E * 2;
            //GameRuntime.currentExposure += gain;

            // 四维属性累计
            totalE += card.E;
            totalA += card.A;
            totalP += card.P;
            totalC += card.C;
        }

        // --- 将累计的四维加到 GameRuntime ---
        GameRuntime.currentE += totalE;
        GameRuntime.currentA += totalA;
        GameRuntime.currentP += totalP;
        GameRuntime.currentC += totalC;

        // --- 清除 pending 列表 & 更新图标 ---
        pendingCards.Clear();
        UpdateIcons();

        // --- HUD 刷新（用新的 HUDController） ---
        HUDController.Instance.RefreshAll();
    }


    public void UndoAllPending()
    {
        if (pendingCards.Count == 0)
        {
            Debug.LogWarning("[Undo] 没有待撤销的卡牌！");
            return;
        }

        int cardsToRestore = pendingCards.Count;

        Debug.Log($"[Undo] 撤销前剩余次数: {HandManager.Instance.remainingPlays}");

        // ✅ 恢复次数
        HandManager.Instance.remainingPlays += cardsToRestore;
        HandManager.Instance.RefreshPlayUI();

        Debug.Log($"[Undo] 恢复了 {cardsToRestore} 次");
        Debug.Log($"[Undo] 撤销后剩余次数: {HandManager.Instance.remainingPlays}");

        // ✅ 恢复卡牌到手牌区
        foreach (var cardObj in pendingCardObjects)
        {
            if (cardObj != null)
            {
                cardObj.SetActive(true);
                cardObj.transform.SetParent(HandManager.Instance.handArea);
            }
        }

        pendingCards.Clear();
        pendingCardObjects.Clear();
        UpdateIcons();
        HUDController.Instance.RefreshHUD();
    }
    // ✅ 新增：重置 Step 的所有状态
public void ResetStep()
{
    Debug.Log($"🔄 重置 Step: {gameObject.name}");
    
    // 重置文本显示
    if (baseExposureText != null)
        baseExposureText.text = $"Base Exposure: +{baseExposure}";
    
    if (appliedEffectText != null)
        appliedEffectText.text = "";  // ✅ 清空效果文本
    
    // 清空 pending 数据
    if (pendingCards != null)
        pendingCards.Clear();
    
    if (pendingCardObjects != null)
        pendingCardObjects.Clear();
    
    // 更新图标
    UpdateIcons();
}
}
