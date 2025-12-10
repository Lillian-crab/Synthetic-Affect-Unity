using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static DragHandler draggingItem;   // 当前正在拖的物体
    public StarterHandItem handItem;          // 卡牌数据
    public CanvasGroup canvasGroup;           // 用于控制透明度和交互
    public Transform originalParent;          // 回家位置
    public Vector3 originalPosition;

    private Canvas rootCanvas;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        handItem = GetComponent<StarterHandItem>();
        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingItem = this;

        originalParent = transform.parent;
        originalPosition = transform.position;

        // 拖动时让卡牌在最上层
        transform.SetParent(rootCanvas.transform);

        // 半透明 + 不挡射线
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 跟随鼠标
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rootCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var globalPos
        );

        transform.position = globalPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;

        // 如果没有成功放到 ProcessingBox → 回家
        if (!handItem.pendingApplied)
        {
            ReturnToOrigin();
        }

        // 恢复透明度
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void ReturnToOrigin()
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
    }
}
