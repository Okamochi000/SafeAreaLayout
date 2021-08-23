using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// セーフエリア外レイアウト
/// </summary>
[DisallowMultipleComponent]
[ExecuteAlways]
public abstract class OutsideLayoutBase : UIBehaviour
{
    [SerializeField] protected RectTransform outside = null;

    private bool isChangedValidate_ = false;
    private bool isLock_ = false;
    private RectTransform selfRectTransform_ = null;

    /// <summary>
    /// レイアウトを更新する
    /// </summary>
    public void UpdateLayout()
    {
        if (isLock_) { return; }

        isLock_ = true;
        isChangedValidate_ = false;

        OnUpdateLayout();

        isLock_ = false;
    }

    /// <summary>
    /// RectTransform取得
    /// </summary>
    /// <returns></returns>
    public RectTransform GetRectTransform()
    {
        if (selfRectTransform_ == null) { selfRectTransform_ = this.GetComponent<RectTransform>(); }
        return selfRectTransform_;
    }

    protected override void Awake()
    {
        base.Awake();
        UpdateLayout();
    }

    // Update is called once per frame
    protected void Update()
    {
        // インスペクター更新チェック
        if (isChangedValidate_)
        {
            UpdateLayout();
        }
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
    /// レイアウト更新
    /// </summary>
    protected abstract void OnUpdateLayout();

    /// <summary>
    /// セーフエリア外取得(ボトム、レフト)
    /// </summary>
    /// <returns></returns>
    protected Vector2 GetOutsideOffsetMin()
    {
        var resolition = Screen.currentResolution;
        var area = Screen.safeArea;
        float scale = 1.0f;
        CanvasScaler scaler = GetParentCanvasScaler(this.transform);
        if (scaler != null && scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) { scale = scaler.referenceResolution.y / resolition.height; }

        Vector2 offsetMin = Vector2.zero;
        offsetMin.y = area.yMin * scale;
        offsetMin.x = area.xMin * scale;

        return offsetMin;
    }

    /// <summary>
    /// 左右セーフエリア外取得(トップ、ライト)
    /// </summary>
    /// <returns></returns>
    protected Vector2 GetOutsideOffsetMax()
    {
        var resolition = Screen.currentResolution;
        var area = Screen.safeArea;
        bool isSafeArea = !(area.xMin == 0.0f && area.xMax == area.width && area.yMin == 0.0f && area.yMax == area.height);
        if (!isSafeArea) { return Vector2.zero; }

        float scale = 1.0f;
        CanvasScaler scaler = GetParentCanvasScaler(this.transform);
        if (scaler != null && scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) { scale = scaler.referenceResolution.y / resolition.height; }

        Vector2 offsetMax = Vector2.zero;
        offsetMax.y = (area.yMax - resolition.height) * scale;
        offsetMax.x = (area.xMax - resolition.width) * scale;

        return offsetMax;
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
