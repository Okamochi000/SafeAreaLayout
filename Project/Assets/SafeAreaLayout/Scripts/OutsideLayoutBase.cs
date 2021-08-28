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
}
