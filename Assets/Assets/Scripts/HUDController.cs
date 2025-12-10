using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [Header("Exposure")]
    public TMP_Text currentExposureText;
    public TMP_Text targetExposureText;

    [Header("Fragments")]
    public TMP_Text fragmentsText;  // 显示 Attention Fragments

    [Header("EAPC Stats")]
    public TMP_Text eText;
    public TMP_Text aText;
    public TMP_Text pText;
    public TMP_Text cText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshAll();
    }

    // 刷新所有 HUD 显示
    public void RefreshAll()
    {
        RefreshExposure();
        RefreshFragments();
        RefreshStats();
    }

    public void RefreshExposure()
    {
        currentExposureText.text = $"Exposure: {GameRuntime.currentExposure}";
        targetExposureText.text = $"Goal: {GameRuntime.targetExposure}";
    }

    public void RefreshFragments()
    {
        fragmentsText.text = $"Fragments: {UIManager.Instance.totalFragments}";
    }

    public void RefreshStats()
    {
        //var rt = GameRuntime;

        eText.text = $"E: {GameRuntime.currentE}";
        aText.text = $"A: {GameRuntime.currentA}";
        pText.text = $"P: {GameRuntime.currentP}";
        cText.text = $"C: {GameRuntime.currentC}";
    }
    public void RefreshHUD()
    {
        RefreshAll();
    }

}
