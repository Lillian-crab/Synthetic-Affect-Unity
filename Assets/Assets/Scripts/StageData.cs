using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/StageData")]
public class StageData : ScriptableObject
{
    [Header("Basic Info")]
    public string stageID;        // e.g., "Stage 1"
    public string stageTitle;     // e.g., "Travel Log"

    [Header("Stage Narrative")]
    [TextArea(3, 6)]
    public string stageSummary;   // English briefing summary

    [Header("Creative Requirements")]
    public string trend;          // e.g., "Current Trend: ..."
    public string emotionDemand;  // emotional requirement
    public string brush;          // suggested brush style

    [Header("Game Settings")]
    public int targetExposure = 10;  //  新增：目标曝光度
    public int maxPlays = 5;         //  可选：每关的出牌次数上限
}