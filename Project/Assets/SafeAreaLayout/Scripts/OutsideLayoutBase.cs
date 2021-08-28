using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// �Z�[�t�G���A�O���C�A�E�g
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
    /// ���C�A�E�g���X�V����
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
    /// RectTransform�擾
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
        // �C���X�y�N�^�[�X�V�`�F�b�N
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
    /// �C���X�y�N�^�[�ύX���m
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        isChangedValidate_ = true;
    }
#endif

    /// <summary>
    /// ���C�A�E�g�X�V
    /// </summary>
    protected abstract void OnUpdateLayout();
}
