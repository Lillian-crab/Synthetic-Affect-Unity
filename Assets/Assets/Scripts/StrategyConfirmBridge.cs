using UnityEngine;

public class StrategyConfirmBridge : MonoBehaviour
{
    public StrategySelector selector;
    public UIManager uiManager;

    public void ConfirmStrategy()
    {
        int index = selector.GetSelectedStrategy();
        uiManager.SetStrategy(index);
        uiManager.ShowCardSelect();
    }
}
