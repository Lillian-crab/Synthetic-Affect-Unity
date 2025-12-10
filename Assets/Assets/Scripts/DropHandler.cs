using UnityEngine;
using UnityEngine.EventSystems;   // ← IDropHandler, PointerEventData
using System.Collections;         // ← IEnumerator（如果有用到的话）

public class DropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var drag = DragHandler.draggingItem;
        if (drag == null) return;

        StarterHandItem hand = drag.handItem;
        if (hand == null) return;

        // 当前 Step
        StepCardController step = GameRuntime.currentStep;
        if (step == null)
        {
            drag.ReturnToOrigin();
            return;
        }

        // 不能超过两张
        if (step.pendingCards.Count >= 2)
        {
            drag.ReturnToOrigin();
            return;
        }

        // **没有剩余次数 → 不允许上牌**
        if (!HandManager.Instance.CanPlay())
        {
            drag.ReturnToOrigin();
            return;
        }

        // ====== 正式加入 Step ======
        step.AddPendingCard(hand.cardData);

        // 扣掉本局出牌次数
        HandManager.Instance.UseOnePlay();

        // 卡回到原位置（不消失）
        drag.ReturnToOrigin();
    }
}
