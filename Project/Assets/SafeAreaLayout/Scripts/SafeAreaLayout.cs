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
    private enum LayoutType
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public bool IsUpdating { get; private set; } = false;
    public Action tempUpdatedCallback = null;

    [SerializeField] private VerticalOutsideLayout top = null;
    [SerializeField] private VerticalOutsideLayout bottom = null;
    [SerializeField] private HorizontalOutsideLayout left = null;
    [SerializeField] private HorizontalOutsideLayout right = null;
    [SerializeField] private bool isInvalidTop = false;
    [SerializeField] private bool isInvalidBottom = false;
    [SerializeField] private bool isInvalidLeft = false;
    [SerializeField] private bool isInvalidRight = false;

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
        foreach (LayoutType layoutType in Enum.GetValues(typeof(LayoutType)))
        {
            if (IsExistUpdate(layoutType))
            {
                isUpdate = true;
                break;
            }
        }

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
        var resolution = Screen.currentResolution;
        var area = Screen.safeArea;
        selfRectTransform_.pivot = new Vector2(0.5f, 0.5f);
        selfRectTransform_.anchorMin = Vector2.zero;
        selfRectTransform_.anchorMax = Vector2.one;
        selfRectTransform_.offsetMin = Vector2.zero;
        selfRectTransform_.offsetMax = Vector2.zero;

        // スケーリング
        Vector2 safeAreaOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 safeAreaOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        Vector2 offsetMin = safeAreaOffsetMin;
        Vector2 offsetMax = safeAreaOffsetMax;

        // トップ設定
        if (!isInvalidTop)
        {
            if (top == null)
            {
                prevTopSize_ = Vector2.zero;
            }
            else
            {
                OutsideLayoutBase outsideLayout = top.GetComponent<OutsideLayoutBase>();
                if (outsideLayout != null) { outsideLayout.UpdateLayout(); }
                offsetMax.y = -top.GetRectTransform().sizeDelta.y;
                prevTopSize_ = top.GetRectTransform().sizeDelta;
            }
        }

        // ボトム設定
        if (!isInvalidBottom)
        {
            if (bottom == null)
            {
                prevBottomSize_ = Vector2.zero;
            }
            else
            {
                OutsideLayoutBase outsideLayout = bottom.GetComponent<OutsideLayoutBase>();
                if (outsideLayout != null) { outsideLayout.UpdateLayout(); }
                offsetMin.y = bottom.GetRectTransform().sizeDelta.y;
                prevBottomSize_ = bottom.GetRectTransform().sizeDelta;
            }
        }

        // レフト設定
        if (!isInvalidLeft)
        {
            if (left == null)
            {
                prevLeftSize_ = Vector2.zero;
            }
            else
            {
                OutsideLayoutBase outsideLayout = left.GetComponent<OutsideLayoutBase>();
                if (outsideLayout != null) { outsideLayout.UpdateLayout(); }
                offsetMin.x = left.GetRectTransform().sizeDelta.x;
                prevLeftSize_ = left.GetRectTransform().sizeDelta;
            }
        }

        // ライト設定
        if (!isInvalidRight)
        {
            if (right == null)
            {
                prevRightSize_ = Vector2.zero;
            }
            else
            {
                OutsideLayoutBase outsideLayout = right.GetComponent<OutsideLayoutBase>();
                if (outsideLayout != null) { outsideLayout.UpdateLayout(); }
                offsetMax.x = -right.GetRectTransform().sizeDelta.x;
                prevRightSize_ = right.GetRectTransform().sizeDelta;
            }
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
    /// 更新が存在するか
    /// </summary>
    /// <param name="layoutType"></param>
    /// <returns></returns>
    private bool IsExistUpdate(LayoutType layoutType)
    {
        switch (layoutType)
        {
            case LayoutType.Top:
                if (isInvalidTop || IsExistUpdateParts(top, prevTopSize_)) { return true; }
                break;
            case LayoutType.Bottom:
                if (isInvalidBottom || IsExistUpdateParts(bottom, prevBottomSize_)) { return true; }
                break;
            case LayoutType.Left:
                if (isInvalidLeft || IsExistUpdateParts(left, prevLeftSize_)) { return true; }
                break;
            case LayoutType.Right:
                if (isInvalidRight || IsExistUpdateParts(right, prevRightSize_)) { return true; }
                break;
            default: break;
        }

        return false;
    }

    /// <summary>
    /// 各部位の更新が存在するか
    /// </summary>
    /// <param name="layoutBase"></param>
    /// <param name="prevSize"></param>
    /// <returns></returns>
    private bool IsExistUpdateParts(OutsideLayoutBase layoutBase, Vector2 prevSize)
    {
        if (layoutBase == null && prevSize != Vector2.zero) { return true; }
        if (layoutBase != null && prevSize != layoutBase.GetRectTransform().sizeDelta) { return true; }

        return false;
    }
}
