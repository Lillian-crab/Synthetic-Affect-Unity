using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarterCardManager : MonoBehaviour
{
    public Transform cardPoolArea;             // 卡池容器
    public GameObject cardPrefab;              // StarterCardTemplate
    public UIManager uiManager;

    private StarterCard[] pool;                // 当前策略的卡池（3 张）
    public StarterCard[] drawnCards;           // 最终抽出的 3 张卡

    void Start()
    {
        LoadStrategyPool();
        DrawRandomCards();
        GenerateCardUI();
    }

    void LoadStrategyPool()
    {
        int index = UIManager.selectedStrategy;

        if (index == 0)
            pool = uiManager.performerStarters;
        else if (index == 1)
            pool = uiManager.empathiserStarters;
        else
            pool = uiManager.optimizerStarters;
    }

    void DrawRandomCards()
    {
        drawnCards = new StarterCard[3];

        // 简单随机 3 张（从 3 张池里抽 3 张）
        // 若之后你把每个卡池扩展到 5 张，可以写真正的抽卡逻辑
        for (int i = 0; i < 3; i++)
            drawnCards[i] = pool[i];
    }

    void GenerateCardUI()
    {
        foreach (StarterCard card in drawnCards)
        {
            GameObject c = Instantiate(cardPrefab, cardPoolArea);
            CardSelectItem item = c.GetComponent<CardSelectItem>();
            item.Init(card);
            
            c.transform.Find("TitleText").GetComponent<TMP_Text>().text = card.cardName;
            c.transform.Find("DescText").GetComponent<TMP_Text>().text = card.description;
            c.transform.Find("CardImage").GetComponent<Image>().sprite = card.cardImage;

            c.transform.Find("StatsText").GetComponent<TMP_Text>().text =
                $"E {card.E} / A {card.A} / P {card.P} / C {card.C}";

        }
    }
}
