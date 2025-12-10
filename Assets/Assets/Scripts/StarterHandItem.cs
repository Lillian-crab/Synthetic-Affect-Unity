using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarterHandItem : MonoBehaviour
{
    public StarterCard cardData;

    public Image cardImage;
    public TMP_Text titleText;
    public TMP_Text statsText;

    public TMP_Text useCounter;      // NEW：右下角的计数文字

    public int usesLeft = 3;         // NEW：每张卡可用次数（之后可由卡牌属性控制）
    public static int totalUses = 5; // NEW：本局总出牌次数
    public static int usedCount = 0; // NEW：本局已用次数

    public bool pendingApplied = false;

    // 初始化手牌 UI
    public void Init(StarterCard data)
    {
        cardData = data;

        titleText.text = data.cardName;
        statsText.text = $"E {data.E} | A {data.A} | P {data.P} | C {data.C}";
        //cardImage.sprite = data.cardImage;
        // ⭐⭐⭐ 不覆盖 sprite，如果 data.cardImage 为空，就保留 prefab 自带的图
        if (data.cardImage != null)
            cardImage.sprite = data.cardImage;

        UpdateUseCounter();
    }

    // 更新右下角计数
    public void UpdateUseCounter()
    {
        if (useCounter != null)
            useCounter.text = $"{usesLeft}/{totalUses}";

        // 若没次数了 → 半透明不可用
        if (usesLeft <= 0 || usedCount >= totalUses)
            GetComponent<CanvasGroup>().alpha = 0.45f;
        else
            GetComponent<CanvasGroup>().alpha = 1f;
    }

    // 当玩家点击手牌时（或拖拽 apply 时调用）
    public void OnUseCard()
    {
        if (usesLeft <= 0) return;        // 用完啦
        if (usedCount >= totalUses) return; // 本局上限到了

        var step = GameRuntime.currentStep;
        if (step == null) return;

        // 把卡加入当前 step 的 pending 列表
        step.AddPendingCard(cardData);

        // 扣次数
        usesLeft--;
        usedCount++;

        UpdateUseCounter();
    }

    public void OnClick()
    {
        CardSellPanel.Instance.Show(this.cardData);
    }

}
