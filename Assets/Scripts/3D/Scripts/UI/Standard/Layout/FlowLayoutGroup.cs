using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlowLayoutGroup : LayoutGroup {

    [SerializeField]
    protected float m_HorizontalSpacing = 0;
    public float horizontalSpacing { get { return m_HorizontalSpacing; } set { SetProperty(ref m_HorizontalSpacing, value); } }

    [SerializeField]
    protected float m_VerticalSpacing = 0;
    public float verticalSpacing { get { return m_VerticalSpacing; } set { SetProperty(ref m_VerticalSpacing, value); } }

    [SerializeField]
    protected float m_Width = 0;
    public float width { get { return m_Width; } set { SetProperty(ref m_Width, value); } }

    [SerializeField]
    protected bool m_ChildForceExpandWidth = true;
    public bool childForceExpandWidth { get { return m_ChildForceExpandWidth; } set { SetProperty(ref m_ChildForceExpandWidth, value); } }

    float totalWidth = 0;
    float totalHeight = 0;

    public override void CalculateLayoutInputHorizontal() {
        base.CalculateLayoutInputHorizontal();
        SetChildrenAlongAxis(1, false);
        SetLayoutInputForAxis(totalWidth, totalWidth, totalWidth, 0);
    }

    public override void CalculateLayoutInputVertical() {
        SetChildrenAlongAxis(1, false);
        SetLayoutInputForAxis(totalHeight, totalHeight, totalHeight, 1);
    }

    public override void SetLayoutHorizontal() {
        SetChildrenAlongAxis(0, true);
    }

    public override void SetLayoutVertical() {
        SetChildrenAlongAxis(1, true);
    }

    protected void SetChildrenAlongAxis(int axis, bool set) {
        totalWidth = 0;
        float containerWidth = width - padding.right;

        //float startOffset = padding.top;
        float posV = padding.top;
        float posH = padding.left;
        float maxHeight = 0;
        //bool reachedMaxWidth = false;

        for (int i = 0; i < rectChildren.Count; i++) {
            float maxWidth = 0;
            RectTransform child = rectChildren[i];
            float childWidth = LayoutUtility.GetPreferredSize(child, 0);
            float childHeight = LayoutUtility.GetPreferredSize(child, 1);

            if (posH + childWidth >= containerWidth) {
                posH = padding.left;
                posV += maxHeight + verticalSpacing;
                maxHeight = 0;
                maxWidth = 0;
            }

            if (childHeight > maxHeight) {
                maxHeight = childHeight;
            }

            if (set) {
                if (axis == 0) {
                    SetChildAlongAxis(child, 0, posH, childWidth);
                } else {
                    //Debug.Log(startOffset + posV);
                    SetChildAlongAxis(child, 1, posV, childHeight);
                }
            }

            posH += childWidth + horizontalSpacing;
            maxWidth = posH - horizontalSpacing + padding.left;
            if (maxWidth > totalWidth) {
                totalWidth = maxWidth;
            }
        }

        //Debug.Log("posV: " + posV + "; maxH: " + maxHeight + "; pad: " + padding.bottom);
        totalHeight = posV + maxHeight + padding.bottom;
    }

}
