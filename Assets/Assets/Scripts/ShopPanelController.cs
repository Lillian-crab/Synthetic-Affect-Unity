using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPanelController : MonoBehaviour
{
    public static ShopPanelController Instance;

    [Header("References")]
    public GameObject panel;                 // ShopPanel 本体
    public Transform cardSlotsParent;        // 放置 3~4 张卡牌的父物体
    public TextMeshProUGUI fragmentsText;    // 显示玩家碎片数量

    [Header("Card Slot Prefab")]
    public GameObject shopCardSlotPrefab;    // 一个卡槽的 Prefab（你需要做一个）

    [Header("Card Pools")]
    public List<StarterCard> attributeCards;
    public List<StarterCard> burstCards;
    public List<StarterCard> systemCards;
    public List<StarterCard> starterCards;  // 稀有池（出现几率低）

    public Button nextStageButton;


    private List<GameObject> activeSlots = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        nextStageButton.onClick.AddListener(OnNextStage);
    }

    public void ShowShop()
    {
        panel.SetActive(true);
        ClearSlots();
        UpdateFragmentsUI();
        GenerateShopCards();
    }

    // 清空之前的卡槽
    void ClearSlots()
    {
        foreach (var slot in activeSlots)
            Destroy(slot);

        activeSlots.Clear();
    }

    // 生成 3 张普通卡 + 可能的 1 张稀有 Starter 卡
    void GenerateShopCards()
    {
        List<StarterCard> shopPool = new List<StarterCard>();
        shopPool.AddRange(attributeCards);
        shopPool.AddRange(burstCards);
        shopPool.AddRange(systemCards);

        // 抽 3 张普通卡
        List<StarterCard> selected = PickRandom(shopPool, 3);

        foreach (var card in selected)
        {
            CreateCardSlot(card);
        }

        // 10% 概率出现1张Starter卡
        if (Random.value < 0.1f)
        {
            StarterCard extraCard = starterCards[Random.Range(0, starterCards.Count)];
            extraCard.price = 6; // 稀有卡价格
            CreateCardSlot(extraCard);
        }
    }

    // 创建卡槽并填数据
    void CreateCardSlot(StarterCard card)
    {
        GameObject slot = Instantiate(shopCardSlotPrefab, cardSlotsParent);
        activeSlots.Add(slot);

        // 获取 UI 元素
        slot.transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = card.cardName;
        slot.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = card.description;

        slot.transform.Find("Stats").GetComponent<TextMeshProUGUI>().text =
            $"E {card.E}   A {card.A}   P {card.P}   C {card.C}";

        slot.transform.Find("Price").GetComponent<TextMeshProUGUI>().text =
            $"Cost: {card.price}";

        slot.transform.Find("CardImage").GetComponent<Image>().sprite = card.cardImage;

        Button buyBtn = slot.transform.Find("BuyButton").GetComponent<Button>();

        buyBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.AddListener(() =>
        {
            BuyCard(card);
        });
    }

    void UpdateFragmentsUI()
    {
        fragmentsText.text = $"Fragments: {UIManager.Instance.totalFragments}";
    }

    void BuyCard(StarterCard card)
    {
        if (UIManager.Instance.totalFragments < card.price)
        {
            Debug.Log("Not enough fragments!");
            return;
        }

        UIManager.Instance.totalFragments -= card.price;
        HUDController.Instance.RefreshAll();

        UIManager.Instance.unlockedCards.Add(card);
        HandManager.Instance.CreateOneHandCard(card);

        Debug.Log("Bought: " + card.cardName);
    }


    // 工具方法：从卡池随机抽 N 张
    List<StarterCard> PickRandom(List<StarterCard> list, int count)
    {
        List<StarterCard> result = new List<StarterCard>();
        List<StarterCard> temp = new List<StarterCard>(list);

        for (int i = 0; i < count; i++)
        {
            if (temp.Count == 0) break;

            int index = Random.Range(0, temp.Count);
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return result;
    }

    void OnNextStage()
    {
        panel.SetActive(false); // 关闭商店

        // 切换到 TransitionPanel
        UIManager.Instance.ShowTransition();

        // 前进到下一关卡
        UIManager.currentStage++;

        Debug.Log("Going to next Stage: " + UIManager.currentStage);
        CardSellPanel.Instance.Hide();

    }

}
