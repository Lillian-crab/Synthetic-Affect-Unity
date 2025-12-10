using System.Collections;
using UnityEngine;

public class StepFlyOut : MonoBehaviour
{
    public float flyDistance = 800f;
    public float duration = 0.5f;

    public IEnumerator PlayFlyOut(RectTransform rt)
    {
        Vector3 start = rt.anchoredPosition;
        Vector3 end = start + new Vector3(flyDistance, 0, 0);

        CanvasGroup cg = rt.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = rt.gameObject.AddComponent<CanvasGroup>();

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            rt.anchoredPosition = Vector3.Lerp(start, end, t);
            cg.alpha = 1f - t;  // 飞出时渐隐
            yield return null;
        }

        // 隐藏对象
        rt.gameObject.SetActive(false);

        // ✅ 重置 Alpha，为下次重用做准备
        cg.alpha = 1f;

        Debug.Log($" {rt.name} 飞出完成，Alpha 重置为 1");
    }
}