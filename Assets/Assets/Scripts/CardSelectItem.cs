using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class CardSelectItem : MonoBehaviour
{
    public StarterCard cardData;          // 这张卡对应的 ScriptableObject
    public Image highlight;               // 选中高亮框（你稍后加）
    public bool isSelected = false;

    public void Init(StarterCard data)
    {
        cardData = data;
        SetSelected(false);
    }

    public void OnClickCard()
    {
        CardSelectManager.Instance.ToggleSelect(this);
    }

    public void SetSelected(bool value)
    {
        isSelected = value;
        if (highlight != null)
            highlight.gameObject.SetActive(value);
    }

    // 实现鼠标悬停放大效果
    Vector3 originalScale;
    Vector3 hoverScale;

    void Start()
    {
        originalScale = transform.localScale;
        hoverScale = originalScale * 1.08f;  // 放大 8%
    }

    public void OnHoverEnter()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(hoverScale));
    }

    public void OnHoverExit()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
    }

    IEnumerator ScaleTo(Vector3 target)
    {
        float t = 0;
        Vector3 start = transform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }

}

