using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardSellPanel : MonoBehaviour
{
    public static CardSellPanel Instance;

    public GameObject panel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI statsText;
    public Button sellButton;

    private StarterCard currentCard;
    public bool isOpen = false;


    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(StarterCard card)
    {
        // 如果已经打开并且点的是同一张卡 → 关闭
        if (isOpen && currentCard == card)
        {
            Hide();
            return;
        }

        // 否则更新内容并显示
        currentCard = card;
        titleText.text = card.cardName;
        descText.text = card.description;
        statsText.text = $"E {card.E}  A {card.A}  P {card.P}  C {card.C}";

        // 绑定 Sell 按钮
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => SellCard());

        panel.SetActive(true);
        isOpen = true;
    }

    void SellCard()
    {
        UIManager.Instance.totalFragments += 1;
        HUDController.Instance.RefreshHUD();
        HandManager.Instance.RemoveCard(currentCard);

        panel.SetActive(false);
        Hide();
    }

    public void Hide()
    {
        panel.SetActive(false);
        isOpen = false;
    }
}
