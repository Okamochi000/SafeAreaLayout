using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

/// <summary>
/// セーフエリアレイアウト
/// </summary>
[DisallowMultipleComponent]
[ExecuteAlways]
public class SafeAreaLayout : UIBehaviour
{
    public bool IsUpdating { get; private set; } = false;
    public Action tempUpdatedCallback = null;

    [SerializeField] private VerticalOutsideLayout top = null;
    [SerializeField] private VerticalOutsideLayout bottom = null;
    [SerializeField] private HorizontalOutsideLayout left = null;
    [SerializeField] private HorizontalOutsideLayout right = null;

    private RectTransform selfRectTransform_ = null;
    private Vector2 prevTopSize_ = Vector2.zero;
    private Vector2 prevBottomSize_ = Vector2.zero;
    private Vector2 prevLeftSize_ = Vector2.zero;
    private Vector2 prevRightSize_ = Vector2.zero;
    private bool isChangedValidate_ = false;

    protected override void Start()
    {
        UpdateLayout();
    }

    // Update is called once per frame
    protected void Update()
    {
        bool isUpdate = isChangedValidate_;
        if ((top == null && prevTopSize_ != Vector2.zero) || (top != null && prevTopSize_ != top.GetRectTransform().sizeDelta)) { isUpdate = true; }
        else if ((bottom == null && prevBottomSize_ != Vector2.zero) || (bottom != null && prevBottomSize_ != bottom.GetRectTransform().sizeDelta)) { isUpdate = true; }
        else if ((left == null && prevLeftSize_ != Vector2.zero) || (left != null && prevLeftSize_ != left.GetRectTransform().sizeDelta)) { isUpdate = true; }
        else if ((right == null && prevRightSize_ != Vector2.zero) || (right != null && prevRightSize_ != right.GetRectTransform().sizeDelta)) { isUpdate = true; }
        if (isUpdate) { UpdateLayout(); }
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        UpdateLayout();
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクター変更検知
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        isChangedValidate_ = true;
    }
#endif

    /// <summary>
    /// ノッジを更新する
    /// </summary>
    private void UpdateLayout()
    {
        if (IsUpdating) { return; }

        IsUpdating = true;
        isChangedValidate_ = false;

        // 初期設定
        if (selfRectTransform_ == null) { selfRectTransform_ = this.GetComponent<RectTransform>(); }
        var resolition = Screen.currentResolution;
        var area = Screen.safeArea;
        selfRectTransform_.pivot = new Vector2(0.5f, 0.5f);
        selfRectTransform_.anchorMin = Vector2.zero;
        selfRectTransform_.anchorMax = Vector2.one;
        selfRectTransform_.offsetMin = Vector2.zero;
        selfRectTransform_.offsetMax = Vector2.zero;

        // スケーリング
        float scale = 1.0f;
        CanvasScaler scaler = GetParentCanvasScaler(this.transform);
        if (scaler != null && scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) { scale = scaler.referenceResolution.y / resolition.height; }

        Vector2 offsetMin = Vector2.zero;
        Vector2 offsetMax = Vector2.zero;

        // トップ設定
        if (top == null)
        {
            offsetMax.y = (area.yMax - resolition.height) * scale;
            prevTopSize_ = Vector2.zero;
        }
        else
        {
            OutsideLayoutBase outsideLayout = top.GetComponent<OutsideLayoutBase>();
            if (outsideLayout != null) { outsideLayout.UpdateLayout(); }
            offsetMax.y = -top.GetRectTransform().sizeDelta.y;
            prevTopSize_ = top.GetRectTransform().sizeDelta;
        }

        // ボトム設定
        if (bottom == null)
        {
            offsetMin.y = area.yMin * scale;
            prevBottomSize_ = Vector2.zero;
        }
        else
        {
            OutsideLayoutBase outsideLayout = bottom.GetComponent<OutsideLayoutBase>();
            if (outsideLayout != null) { outsideLayout.UpdateLayout(); }
            offsetMin.y = bottom.GetRectTransform().sizeDelta.y;
            prevBottomSize_ = bottom.GetRectTransform().sizeDelta;
        }

        // レフト設定
        if (left == null)
        {
            offsetMin.x = area.xMin * scale;
            prevLeftSize_ = Vector2.zero;
        }
        else
        {
            OutsideLayoutBase outsideLayout = left.GetComponent<OutsideLayoutBase>();
            if (outsideLayout != null) { outsideLayout.UpdateLayout(); }
            offsetMin.x = left.GetRectTransform().sizeDelta.x;
            prevLeftSize_ = left.GetRectTransform().sizeDelta;
        }

        // ライト設定
        if (right == null)
        {
            offsetMax.x = (area.xMax - resolition.width) * scale;
            prevRightSize_ = Vector2.zero;
        }
        else
        {
            OutsideLayoutBase outsideLayout = right.GetComponent<OutsideLayoutBase>();
            if (outsideLayout != null) { outsideLayout.UpdateLayout(); }
            offsetMax.x = -right.GetRectTransform().sizeDelta.x;
            prevRightSize_ = right.GetRectTransform().sizeDelta;
        }

        // 更新
        selfRectTransform_.offsetMin = offsetMin;
        selfRectTransform_.offsetMax = offsetMax;

        // 更新コールバック呼び出し
        if (tempUpdatedCallback != null)
        {
            tempUpdatedCallback();
            tempUpdatedCallback = null;
        }

        IsUpdating = false;
    }

    /// <summary>
    /// 親キャンバスを取得する
    /// </summary>
    /// <returns></returns>
    private CanvasScaler GetParentCanvasScaler(Transform transform)
    {
        if (transform.parent == null) { return null; }

        CanvasScaler canvas = transform.parent.GetComponent<CanvasScaler>();
        if (canvas == null) { return GetParentCanvasScaler(transform.parent); }
        else { return canvas; }
    }
}
