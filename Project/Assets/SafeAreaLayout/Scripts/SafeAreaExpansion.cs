using UnityEngine;

/// <summary>
/// セーフエリア分拡張する
/// </summary>
public class SafeAreaExpansion : SafeAreaBehaviour
{
    [SerializeField] private bool isTop = false;
    [SerializeField] private bool isBottom = false;
    [SerializeField] private bool isLeft = false;
    [SerializeField] private bool isRight = false;

    /// <summary>
    /// ノッジを更新する
    /// </summary>
    protected override void UpdateLayout()
    {
        if (!isTop && !isBottom && !isLeft && !isRight) { return; }

        // アンカー設定
        RectTransform selfRectTransform = GetRectTransform();
        selfRectTransform.pivot = new Vector2(0.5f, 0.5f);
        selfRectTransform.anchorMin = Vector2.zero;
        selfRectTransform.anchorMax = Vector2.one;

        // オフセット設定
        Vector2 offsetMin = selfRectTransform.offsetMin;
        Vector2 offsetMax = selfRectTransform.offsetMax;
        Vector2 outsideOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 outsideOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        if (isTop) { offsetMax.y = -outsideOffsetMax.y; }
        if (isBottom) { offsetMin.y = -outsideOffsetMin.y; }
        if (isRight) { offsetMax.x = -outsideOffsetMax.x; }
        if (isLeft) { offsetMin.x = -outsideOffsetMin.x; }
        selfRectTransform.offsetMin = offsetMin;
        selfRectTransform.offsetMax = offsetMax;
    }
}
