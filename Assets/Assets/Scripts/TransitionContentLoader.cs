using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TransitionContentLoader : MonoBehaviour
{
    [Header("Phase2 UI")]
    public TMP_Text stageTitle;
    public TMP_Text stageSummary;
    public TMP_Text trendText;
    public TMP_Text emotionText;
    public TMP_Text brushText;

    void OnEnable()
    {
        LoadStageContent();
    }

    void LoadStageContent()
    {
        StageData data = UIManager.Instance.stages[UIManager.currentStage];

        stageTitle.text = $"{data.stageID}: {data.stageTitle}";
        stageSummary.text = data.stageSummary;

        trendText.text = data.trend;
        emotionText.text = data.emotionDemand;
        brushText.text = data.brush;
    }
}