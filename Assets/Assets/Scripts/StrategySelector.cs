using UnityEngine;
using UnityEngine.UI;

public class StrategySelector : MonoBehaviour
{
    public RectTransform[] cards;   // 3 cards
    public RectTransform highlightFrame;
    
    [Header("Animation Settings")]
    public float moveSpeed = 10f;      // 滑动速度
    public float scaleSpeed = 10f;     // 缩放速度
    public Vector3 leftPos = new Vector3(-400, 0, 0);
    public Vector3 midPos = new Vector3(0, 0, 0);
    public Vector3 rightPos = new Vector3(400, 0, 0);

    public float scaleSelected = 1.2f;
    public float scaleUnselected = 0.8f;

    private Vector3[] targetPositions = new Vector3[3];
    private float[] targetScales = new float[3];

    private int currentIndex = 1; // default start = middle

    void Start()
    {
        UpdateTargets();
        InstantApply(); // 直接应用一次，防止初始跳动
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            UpdateTargets();
            // ★ 通知 UIManager 当前策略
            UIManager.selectedStrategy = currentIndex;

            // ★ 更新手部图像
            UIManager.Instance.UpdateHands();

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = Mathf.Min(2, currentIndex + 1);
            UpdateTargets();
            UIManager.selectedStrategy = currentIndex;
            UIManager.Instance.UpdateHands();

        }

        SmoothApply();
    }

    void UpdateTargets()
    {
        targetPositions[0] = leftPos;
        targetPositions[1] = midPos;
        targetPositions[2] = rightPos;

        for (int i = 0; i < 3; i++)
            targetScales[i] = (i == currentIndex ? scaleSelected : scaleUnselected);
    }

    void InstantApply()
    {
        for (int i = 0; i < 3; i++)
        {
            cards[i].anchoredPosition = targetPositions[i - currentIndex + 1];
            cards[i].localScale = Vector3.one * targetScales[i];
        }
    }

    void SmoothApply()
    {
        for (int i = 0; i < 3; i++)
        {
            int relative = Mathf.Clamp(i - currentIndex + 1, 0, 2);

            cards[i].anchoredPosition = Vector3.Lerp(
                cards[i].anchoredPosition,
                targetPositions[relative],
                Time.deltaTime * moveSpeed
            );

            cards[i].localScale = Vector3.Lerp(
                cards[i].localScale,
                Vector3.one * targetScales[i],
                Time.deltaTime * scaleSpeed
            );
        }
    }

    void UpdateHandsVisual()
    {
        // do nothing now
    }


    public int GetSelectedStrategy()
    {
        return currentIndex;
    }
}
