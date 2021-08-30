using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// セーフエリア無視レイアウト
/// </summary>
public class IgnoneSafeAreaLayout : SafeAreaBehaviour
{
    [SerializeField] private bool isTopSafeArea = false;
    [SerializeField] private bool isBottomSafeArea = false;
    [SerializeField] private bool isLeftSafeArea = false;
    [SerializeField] private bool isRightSafeArea = false;

    private RectTransform canvasRectTransform_ = null;
    private bool isSafeAreaLayout_ = false;

    protected override void OnEnable()
    {
        CanvasScaler canvasScaler = SafeAreaUtility.GetParentCanvasScaler(this.transform);
        if (canvasScaler != null) { canvasRectTransform_ = canvasScaler.GetComponent<RectTransform>(); }
        UpdateLayoutLock();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        if (IsUpdating) { return; }
        if (isSafeAreaLayout_) { return; }

        SafeAreaLayout safeAreaLayout = GetSafeAreaLayout(this.transform);
        if (safeAreaLayout != null && safeAreaLayout.IsUpdating)
        {
            isSafeAreaLayout_ = true;
            safeAreaLayout.tempUpdatedCallback += UpdateLayoutLock;
        }
        else
        {
            CanvasScaler canvasScaler = SafeAreaUtility.GetParentCanvasScaler(this.transform);
            if (canvasScaler != null) { canvasRectTransform_ = canvasScaler.GetComponent<RectTransform>(); }
            UpdateLayoutLock();
        }
    }

    /// <summary>
    /// ノッジを更新する
    /// </summary>
    protected override void UpdateLayout()
    {
        if (canvasRectTransform_ == null) { return; }

        isSafeAreaLayout_ = false;

        // セーフエリアを無視したサイズに調整する
        Vector2 sizeDelta = canvasRectTransform_.sizeDelta;
        RectTransform selfRectTransform = GetRectTransform();
        selfRectTransform.pivot = new Vector2(0.5f, 0.5f);
        selfRectTransform.anchorMin = Vector2.zero;
        selfRectTransform.anchorMax = Vector2.one;
        selfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasRectTransform_.sizeDelta.x);
        selfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasRectTransform_.sizeDelta.y);
        selfRectTransform.position = canvasRectTransform_.position;

        // セーフエリア内に収める
        Vector2 offsetMin = selfRectTransform.offsetMin;
        Vector2 offsetMax = selfRectTransform.offsetMax;
        Vector2 outsideOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 outsideOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        if (isTopSafeArea) { offsetMax.y += outsideOffsetMax.y; }
        if (isBottomSafeArea) { offsetMin.y += outsideOffsetMin.y; }
        if (isRightSafeArea) { offsetMax.x += outsideOffsetMax.x; }
        if (isLeftSafeArea) { offsetMin.x += outsideOffsetMin.x; }
        selfRectTransform.offsetMin = offsetMin;
        selfRectTransform.offsetMax = offsetMax;
    }

    /// <summary>
    /// 親SafeAreaLayoutを取得する
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    private SafeAreaLayout GetSafeAreaLayout(Transform transform)
    {
        if (transform.parent == null) { return null; }

        SafeAreaLayout safeAreaLayout = transform.parent.GetComponent<SafeAreaLayout>();
        if (safeAreaLayout == null) { return GetSafeAreaLayout(transform.parent); }
        else { return safeAreaLayout; }
    }
}
