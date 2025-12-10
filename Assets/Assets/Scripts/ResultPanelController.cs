using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultPanelController : MonoBehaviour
{
    public static ResultPanelController Instance;

    [Header("UI References")]
    public GameObject panel;                // 整个 ResultPanel
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI exposureText;    // 显示曝光：xxx / 目标：xxx
    public TextMeshProUGUI rewardText;      // 显示奖励内容

    [Header("Reward Settings")]
    public int fragmentPerPlay = 1; // 每剩余一次出牌 → 1碎片（可改）

    [Header("Buttons")]
    public Button successButton;   // shown when success
    public Button failButton;      // shown when fail


    private void Awake()
    {
        Instance = this;
        panel.SetActive(false); // 默认隐藏
    }

    /// <summary>
    /// 在流水线完成后调用这个方法
    /// </summary>
    public void ShowResult(int finalExposure, int goalExposure, int remainingPlays)
    {
        panel.SetActive(true);

        bool success = finalExposure >= goalExposure;

        // ----------- UI Text -----------
        titleText.text = success ? "Goal Achieved!" : "Goal Not Met";
        exposureText.text =
            $"Exposure: {finalExposure}\nTarget: {goalExposure}";

        // Reward calculation
        int fragmentReward = remainingPlays * fragmentPerPlay;

        // 成功额外奖励 +2 fragment
        if (success)
        {
            fragmentReward += 2;
            rewardText.text = $"Attention Fragments Earned: {fragmentReward} (+2 Success Bonus)";
        }
        else
        {
            rewardText.text = $"Attention Fragments Earned: {fragmentReward}";
        }

        // ⭐ 把奖励加给玩家的总碎片数
        rewardText.text = $"Attention Fragments Earned: {fragmentReward}";
        UIManager.Instance.AddFragments(fragmentReward);

        // ----------- Toggle Buttons (Success / Fail) -----------
        successButton.gameObject.SetActive(success);
        failButton.gameObject.SetActive(!success);

        // ----------- Success Button Action -----------
        successButton.onClick.RemoveAllListeners();
        successButton.onClick.AddListener(() =>
        {
            // 进入商店
            ShopPanelController.Instance.ShowShop();
            panel.SetActive(false);
            Debug.Log("Success: Enter Shop");
        });

        // ----------- Failure Button Action -----------
        failButton.onClick.RemoveAllListeners();
        failButton.onClick.AddListener(() =>
        {
            // Return to main menu
            UIManager.Instance.ReturnToMainMenu();
        });
    }

}
