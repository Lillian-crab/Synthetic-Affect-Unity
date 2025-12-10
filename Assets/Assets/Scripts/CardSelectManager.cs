using System.Collections.Generic;
using UnityEngine;

public class CardSelectManager : MonoBehaviour
{
    public static CardSelectManager Instance;

    public List<CardSelectItem> selectedCards = new List<CardSelectItem>();
    public int maxSelect = 2;
    public GameObject confirmButton;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleSelect(CardSelectItem card)
    {
        if (card.isSelected)
        {
            // 取消选择
            card.SetSelected(false);
            selectedCards.Remove(card);
        }
        else
        {
            // 如果还没选够
            if (selectedCards.Count < maxSelect)
            {
                card.SetSelected(true);
                selectedCards.Add(card);
            }
        }

        UpdateConfirmButton();
    }

    void UpdateConfirmButton()
    {
        confirmButton.GetComponent<UnityEngine.UI.Button>().interactable =
            (selectedCards.Count == maxSelect);
    }

    public List<StarterCard> GetSelectedStarterCards()
    {
        List<StarterCard> list = new List<StarterCard>();
        foreach (var c in selectedCards)
            list.Add(c.cardData);

        return list;
    }
    public void ConfirmSelection()
    {
        UIManager.selectedStarterCards = GetSelectedStarterCards();
        Debug.Log("Starter Cards Selected: " + UIManager.selectedStarterCards.Count);
    }

}
