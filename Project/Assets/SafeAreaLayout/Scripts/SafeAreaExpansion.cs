using UnityEngine;

/// <summary>
/// �Z�[�t�G���A���g������
/// </summary>
public class SafeAreaExpansion : SafeAreaBehaviour
{
    [SerializeField] private bool isTop = false;
    [SerializeField] private bool isBottom = false;
    [SerializeField] private bool isLeft = false;
    [SerializeField] private bool isRight = false;

    /// <summary>
    /// �m�b�W���X�V����
    /// </summary>
    protected override void UpdateLayout()
    {
        if (!isTop && !isBottom && !isLeft && !isRight) { return; }

        // �A���J�[�ݒ�
        RectTransform selfRectTransform = GetRectTransform();
        selfRectTransform.pivot = new Vector2(0.5f, 0.5f);
        selfRectTransform.anchorMin = Vector2.zero;
        selfRectTransform.anchorMax = Vector2.one;

        // �I�t�Z�b�g�ݒ�
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
