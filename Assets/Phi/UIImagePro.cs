using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.Serialization;
//TODO don't set anchors when using setsize - no need!
//TODO - fix rotation when using filled method.
[AddComponentMenu("UI/ImagePro", 10)]
public class UIImagePro : MaskableGraphic, ICanvasRaycastFilter, ISerializationCallbackReceiver, ILayoutElement
{
    private float m_EventAlphaThreshold = 1f;
    [Range(0f, 1f), SerializeField]
    private float m_FillAmount = 1f;
    [SerializeField]
    private bool m_FillCenter = true;
    [SerializeField]
    private bool m_FillClockwise = true;
    [SerializeField]
    private FillMethod m_FillMethod = FillMethod.Radial360;
    [SerializeField]
    private int m_FillOrigin;
    [NonSerialized]
    private Sprite m_OverrideSprite;
    [SerializeField]
    private bool m_PreserveAspect;
    [SerializeField]
    [Tooltip("When ticked the rect transform will be driven by the sprite's physical size times this size allowing you to change sprites and the value be updated. When not ticked you can edit this value to update the rect transform which will retain it's size even if the sprite's size changes.")]
    private bool m_SetSize = false;
    [SerializeField]
    private Vector2 m_Scale = Vector2.one;
    [SerializeField]
    private ImageTransform m_ImageTransform = ImageTransform.None;
    [SerializeField]
    private bool m_RoundTiling = true;
    [SerializeField]
    private bool m_ButterflyX = false;
    [SerializeField]
    private bool m_ButterflyY = false;
    [SerializeField]
    private float m_ScalePixelSize = 1;
    [Tooltip("When set the size will be relative to the center of the sliced sprite instead of the size of the whole sprite")]
    [SerializeField]
    private bool m_SizeByCenter = true;
#if BETA20
    [RenamedSerializedData("m_Frame"), SerializeField]
#else
    [FormerlySerializedAs("m_Frame"), SerializeField]
#endif
    private Sprite m_Sprite;
    [SerializeField]
    private Type m_Type;
    private static readonly Vector2[] s_Uv = new Vector2[4];
    private static readonly Vector2[] s_UVScratch = new Vector2[4];
    private static readonly Vector2[] s_VertScratch = new Vector2[4];
    private static readonly Vector2[] s_Xy = new Vector2[4];
    protected DrivenRectTransformTracker m_Tracker;

    private void AddQuad(List<UIVertex> vbo, UIVertex v, Vector2 posMin, Vector2 posMax, Vector2 uvMin, Vector2 uvMax)
    {
        if (imageTransform >= ImageTransform.Rotate90CW)
        {
            v.position = new Vector3(posMin.x, posMin.y, 0f);
            v.uv0 = new Vector2(uvMin.y, uvMin.x);
            vbo.Add(v);
            v.position = new Vector3(posMin.x, posMax.y, 0f);
            v.uv0 = new Vector2(uvMax.y, uvMin.x);
            vbo.Add(v);
            v.position = new Vector3(posMax.x, posMax.y, 0f);
            v.uv0 = new Vector2(uvMax.y, uvMax.x);
            vbo.Add(v);
            v.position = new Vector3(posMax.x, posMin.y, 0f);
            v.uv0 = new Vector2(uvMin.y, uvMax.x);
            vbo.Add(v);
        }
        else
        {
            v.position = new Vector3(posMin.x, posMin.y, 0f);
            v.uv0 = new Vector2(uvMin.x, uvMin.y);
            vbo.Add(v);
            v.position = new Vector3(posMin.x, posMax.y, 0f);
            v.uv0 = new Vector2(uvMin.x, uvMax.y);
            vbo.Add(v);
            v.position = new Vector3(posMax.x, posMax.y, 0f);
            v.uv0 = new Vector2(uvMax.x, uvMax.y);
            vbo.Add(v);
            v.position = new Vector3(posMax.x, posMin.y, 0f);
            v.uv0 = new Vector2(uvMax.x, uvMin.y);
            vbo.Add(v);
        }
    }
    // Do not call SetSize here as it causes the size to be recorded in animation playback when enabling / disabling object
 /*   protected override void OnEnable()
    {
        base.OnEnable();
        SetSize();
    }*/
    protected override void Awake()
    {
        base.Awake();
        SetSize();
    } 


    public virtual void CalculateLayoutInputHorizontal()
    {
    }

    public virtual void CalculateLayoutInputVertical()
    {
    }

    private void GenerateFilledSprite(List<UIVertex> vbo, bool preserveAspect)
    {
        if (this.m_FillAmount >= 0.001f)
        {
            Vector4 drawingDimensions = this.GetDrawingDimensions(preserveAspect);
            Vector4 vector2 = (this.overrideSprite == null) ? Vector4.zero : TransformUV(DataUtility.GetOuterUV(this.overrideSprite));
            UIVertex simpleVert = UIVertex.simpleVert;
            simpleVert.color = base.color;
            float x = vector2.x;
            float y = vector2.y;
            float z = vector2.z;
            float w = vector2.w;
            if ((this.m_FillMethod == FillMethod.Horizontal) || (this.m_FillMethod == FillMethod.Vertical))
            {
                if (this.fillMethod == FillMethod.Horizontal)
                {
                    float num5 = (z - x) * this.m_FillAmount;
                    if (this.m_FillOrigin == 1)
                    {
                        drawingDimensions.x = drawingDimensions.z - ((drawingDimensions.z - drawingDimensions.x) * this.m_FillAmount);
                        x = z - num5;
                    }
                    else
                    {
                        drawingDimensions.z = drawingDimensions.x + ((drawingDimensions.z - drawingDimensions.x) * this.m_FillAmount);
                        z = x + num5;
                    }
                }
                else if (this.fillMethod == FillMethod.Vertical)
                {
                    float num6 = (w - y) * this.m_FillAmount;
                    if (this.m_FillOrigin == 1)
                    {
                        drawingDimensions.y = drawingDimensions.w - ((drawingDimensions.w - drawingDimensions.y) * this.m_FillAmount);
                        y = w - num6;
                    }
                    else
                    {
                        drawingDimensions.w = drawingDimensions.y + ((drawingDimensions.w - drawingDimensions.y) * this.m_FillAmount);
                        w = y + num6;
                    }
                }
            }
            s_Xy[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
            s_Xy[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
            s_Xy[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
            s_Xy[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
            s_Uv[0] = new Vector2(x, y);
            s_Uv[1] = new Vector2(x, w);
            s_Uv[2] = new Vector2(z, w);
            s_Uv[3] = new Vector2(z, y);
            if (this.m_FillAmount < 1f)
            {
                if (this.fillMethod == FillMethod.Radial90)
                {
                    if (RadialCut(s_Xy, s_Uv, this.m_FillAmount, this.m_FillClockwise, this.m_FillOrigin))
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            simpleVert.position = s_Xy[j];
                            simpleVert.uv0 = s_Uv[j];
                            vbo.Add(simpleVert);
                        }
                    }
                    return;
                }
                if (this.fillMethod == FillMethod.Radial180)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        float num9;
                        float num10;
                        float num11;
                        float num12;
                        int num13 = (this.m_FillOrigin <= 1) ? 0 : 1;
                        if ((this.m_FillOrigin == 0) || (this.m_FillOrigin == 2))
                        {
                            num11 = 0f;
                            num12 = 1f;
                            if (k == num13)
                            {
                                num9 = 0f;
                                num10 = 0.5f;
                            }
                            else
                            {
                                num9 = 0.5f;
                                num10 = 1f;
                            }
                        }
                        else
                        {
                            num9 = 0f;
                            num10 = 1f;
                            if (k == num13)
                            {
                                num11 = 0.5f;
                                num12 = 1f;
                            }
                            else
                            {
                                num11 = 0f;
                                num12 = 0.5f;
                            }
                        }
                        s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num9);
                        s_Xy[1].x = s_Xy[0].x;
                        s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num10);
                        s_Xy[3].x = s_Xy[2].x;
                        s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num11);
                        s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num12);
                        s_Xy[2].y = s_Xy[1].y;
                        s_Xy[3].y = s_Xy[0].y;
                        s_Uv[0].x = Mathf.Lerp(x, z, num9);
                        s_Uv[1].x = s_Uv[0].x;
                        s_Uv[2].x = Mathf.Lerp(x, z, num10);
                        s_Uv[3].x = s_Uv[2].x;
                        s_Uv[0].y = Mathf.Lerp(y, w, num11);
                        s_Uv[1].y = Mathf.Lerp(y, w, num12);
                        s_Uv[2].y = s_Uv[1].y;
                        s_Uv[3].y = s_Uv[0].y;
                        float num14 = !this.m_FillClockwise ? ((this.m_FillAmount * 2f) - (1 - k)) : ((this.fillAmount * 2f) - k);
                        if (RadialCut(s_Xy, s_Uv, Mathf.Clamp01(num14), this.m_FillClockwise, ((k + this.m_FillOrigin) + 3) % 4))
                        {
                            for (int m = 0; m < 4; m++)
                            {
                                simpleVert.position = s_Xy[m];
                                simpleVert.uv0 = s_Uv[m];
                                vbo.Add(simpleVert);
                            }
                        }
                    }
                    return;
                }
                if (this.fillMethod == FillMethod.Radial360)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        float num17;
                        float num18;
                        float num19;
                        float num20;
                        if (n < 2)
                        {
                            num17 = 0f;
                            num18 = 0.5f;
                        }
                        else
                        {
                            num17 = 0.5f;
                            num18 = 1f;
                        }
                        switch (n)
                        {
                            case 0:
                            case 3:
                                num19 = 0f;
                                num20 = 0.5f;
                                break;

                            default:
                                num19 = 0.5f;
                                num20 = 1f;
                                break;
                        }
                        s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num17);
                        s_Xy[1].x = s_Xy[0].x;
                        s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num18);
                        s_Xy[3].x = s_Xy[2].x;
                        s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num19);
                        s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num20);
                        s_Xy[2].y = s_Xy[1].y;
                        s_Xy[3].y = s_Xy[0].y;
                        s_Uv[0].x = Mathf.Lerp(x, z, num17);
                        s_Uv[1].x = s_Uv[0].x;
                        s_Uv[2].x = Mathf.Lerp(x, z, num18);
                        s_Uv[3].x = s_Uv[2].x;
                        s_Uv[0].y = Mathf.Lerp(y, w, num19);
                        s_Uv[1].y = Mathf.Lerp(y, w, num20);
                        s_Uv[2].y = s_Uv[1].y;
                        s_Uv[3].y = s_Uv[0].y;
                        float num21 = !this.m_FillClockwise ? ((this.m_FillAmount * 4f) - (3 - ((n + this.m_FillOrigin) % 4))) : ((this.m_FillAmount * 4f) - ((n + this.m_FillOrigin) % 4));
                        if (RadialCut(s_Xy, s_Uv, Mathf.Clamp01(num21), this.m_FillClockwise, (n + 2) % 4))
                        {
                            for (int num22 = 0; num22 < 4; num22++)
                            {
                                simpleVert.position = s_Xy[num22];
                                simpleVert.uv0 = s_Uv[num22];
                                vbo.Add(simpleVert);
                            }
                        }
                    }
                    return;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                simpleVert.position = s_Xy[i];
                simpleVert.uv0 = s_Uv[i];
                vbo.Add(simpleVert);
            }
        }
    }
    private void GenerateSimpleSprite(List<UIVertex> vbo, bool preserveAspect)
    {
        UIVertex simpleVert = UIVertex.simpleVert;
        simpleVert.color = base.color;
        Vector4 drawingDimensions = this.GetDrawingDimensions(preserveAspect);
        Vector4 outerUVs = (this.overrideSprite == null) ? Vector4.zero : TransformUV(DataUtility.GetOuterUV(this.overrideSprite));

            simpleVert.position = new Vector3(drawingDimensions.x, drawingDimensions.y);
            simpleVert.uv0 = new Vector2(outerUVs.x, outerUVs.y);
            vbo.Add(simpleVert);
            simpleVert.position = new Vector3(drawingDimensions.x, drawingDimensions.w);
            simpleVert.uv0 = new Vector2(outerUVs.x, outerUVs.w);
            vbo.Add(simpleVert);
            simpleVert.position = new Vector3(drawingDimensions.z, drawingDimensions.w);
            simpleVert.uv0 = new Vector2(outerUVs.z, outerUVs.w);
            vbo.Add(simpleVert);
            simpleVert.position = new Vector3(drawingDimensions.z, drawingDimensions.y);
            simpleVert.uv0 = new Vector2(outerUVs.z, outerUVs.y);
            vbo.Add(simpleVert);
        }

    private void GenerateSlicedSprite(List<UIVertex> vbo, bool preserveAspect)
    {
        if (!this.hasBorder)
        {
            this.GenerateSimpleSprite(vbo, preserveAspect);
        }
        else
        {
            Vector4 outerUV;
            Vector4 innerUV;
            Vector4 padding;
            Vector4 adjustedBorders;
            if (this.overrideSprite != null)
            {
                outerUV = TransformUV(DataUtility.GetOuterUV(this.overrideSprite));
                innerUV = TransformUV(DataUtility.GetInnerUV(this.overrideSprite));
                padding = TransformUV(DataUtility.GetPadding(this.overrideSprite));
                adjustedBorders = TransformUV(this.overrideSprite.border);
            }
            else
            {
                outerUV = Vector4.zero;
                innerUV = Vector4.zero;
                padding = Vector4.zero;
                adjustedBorders = Vector4.zero;
            }
            Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
            if (preserveAspect)
            {
                Vector4 drawingDimensions = this.GetDrawingDimensions(preserveAspect);
                pixelAdjustedRect.xMin = drawingDimensions.x;
                pixelAdjustedRect.yMin = drawingDimensions.y;
                pixelAdjustedRect.xMax = drawingDimensions.z;
                pixelAdjustedRect.yMax = drawingDimensions.w;
            }
            adjustedBorders = this.GetAdjustedBorders((Vector4) (adjustedBorders / this.pixelsPerUnit), pixelAdjustedRect);
            s_VertScratch[0] = new Vector2(padding.x, padding.y);
            s_VertScratch[3] = new Vector2(pixelAdjustedRect.width - padding.z, pixelAdjustedRect.height - padding.w);
            s_VertScratch[1].x = adjustedBorders.x;
            s_VertScratch[1].y = adjustedBorders.y;
            s_VertScratch[2].x = pixelAdjustedRect.width - adjustedBorders.z;
            s_VertScratch[2].y = pixelAdjustedRect.height - adjustedBorders.w;
            for (int i = 0; i < 4; i++)
            {
                s_VertScratch[i].x += pixelAdjustedRect.x;
                s_VertScratch[i].y += pixelAdjustedRect.y;
            }
                s_UVScratch[0] = new Vector2(outerUV.x, outerUV.y);
                s_UVScratch[1] = new Vector2(innerUV.x, innerUV.y);
                s_UVScratch[2] = new Vector2(innerUV.z, innerUV.w);
                s_UVScratch[3] = new Vector2(outerUV.z, outerUV.w);
            UIVertex simpleVert = UIVertex.simpleVert;
            simpleVert.color = base.color;
            for (int j = 0; j < 3; j++)
            {
                int index = j + 1;
                for (int k = 0; k < 3; k++)
                {
                    if ((this.m_FillCenter || (j != 1)) || (k != 1))
                    {
                        int num5 = k + 1;
                        this.AddQuad(vbo, simpleVert, new Vector2(s_VertScratch[j].x, s_VertScratch[k].y), new Vector2(s_VertScratch[index].x, s_VertScratch[num5].y), new Vector2(s_UVScratch[j].x, s_UVScratch[k].y), new Vector2(s_UVScratch[index].x, s_UVScratch[num5].y));
                    }
                }
            }
        }
    }
    private void GenerateTiledSprite(List<UIVertex> vbo, bool preserveAspect)
    {
        Vector4 outerUV;
        Vector4 innerUV;
        Vector4 adjustedBorders;
        Vector2 size;
        if (this.overrideSprite != null)
        {
            outerUV = TransformUV(DataUtility.GetOuterUV(this.overrideSprite));
            innerUV = TransformUV(DataUtility.GetInnerUV(this.overrideSprite));
            adjustedBorders = TransformUV(this.overrideSprite.border);
            size = this.overrideSprite.rect.size;
        }
        else
        {
            outerUV = Vector4.zero;
            innerUV = Vector4.zero;
            adjustedBorders = Vector4.zero;
            size = (Vector2)(Vector2.one * 100f);
        }
        Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
        if (preserveAspect)
        {
            Vector4 drawingDimensions = this.GetDrawingDimensions(preserveAspect);
            pixelAdjustedRect.xMin = drawingDimensions.x;
            pixelAdjustedRect.yMin = drawingDimensions.y;
            pixelAdjustedRect.xMax = drawingDimensions.z;
            pixelAdjustedRect.yMax = drawingDimensions.w;
        }
        float num = ((size.x - adjustedBorders.x) - adjustedBorders.z) / this.pixelsPerUnit;
        float num2 = ((size.y - adjustedBorders.y) - adjustedBorders.w) / this.pixelsPerUnit;
        adjustedBorders = this.GetAdjustedBorders((Vector4)(adjustedBorders / this.pixelsPerUnit), pixelAdjustedRect);
        Vector2 uvMin = new Vector2(innerUV.x, innerUV.y);
        Vector2 uvMaxOrig = new Vector2(innerUV.z, innerUV.w);
        UIVertex simpleVert = UIVertex.simpleVert;
        simpleVert.color = base.color;
        float x = adjustedBorders.x;
        float centerWidth = pixelAdjustedRect.width - adjustedBorders.z;
        float y = adjustedBorders.y;
        float centerHeight = pixelAdjustedRect.height - adjustedBorders.w;
        if (((centerWidth - x) > (num * 100f)) || ((centerHeight - y) > (num2 * 100f)))
        {
            num = (centerWidth - x) / 100f;
            num2 = (centerHeight - y) / 100f;
        }
        if (m_RoundTiling)
        {
            float t = Mathf.Round((centerHeight - y) / num2);
            if (t < 1) t = 1;
            num2 = (centerHeight - y) / t;
            t = Mathf.Round((centerWidth - x) / num);
            if (t < 1) t = 1;
            num = (centerWidth - x) / t;
        }
        Vector2 uvMax = uvMaxOrig;
        Vector2 uvMinOrig = uvMin;
        if (this.m_FillCenter)
        {
            for (float i = y; i < centerHeight; i += num2)
            {
                float num8 = i + num2;
                if (num8 > centerHeight)
                {
                    uvMax.y = uvMin.y + (((uvMax.y - uvMin.y) * (centerHeight - i)) / (num8 - i));
                    num8 = centerHeight;
                }
                for (float j = x; j < centerWidth; j += num)
                {
                    float num10 = j + num;
                    if (num10 > centerWidth)
                    {
                        uvMax.x = uvMin.x + (((uvMax.x - uvMin.x) * (centerWidth - j)) / (num10 - j));
                        num10 = centerWidth;
                    }
                    this.AddQuad(vbo, simpleVert, new Vector2(j, i) + pixelAdjustedRect.position, new Vector2(num10, num8) + pixelAdjustedRect.position, uvMin, uvMax);
                    if (m_ButterflyX)
                    {
                        float temp = uvMax.x;
                        uvMax.x = uvMin.x;
                        uvMin.x = temp;
                    }
                }
                uvMax.x = uvMaxOrig.x;
                uvMin.x = uvMinOrig.x;
                if (m_ButterflyY)
                {
                    float temp = uvMax.y;
                    uvMax.y = uvMin.y;
                    uvMin.y = temp;
                }
            }
            uvMax.y = uvMaxOrig.y;
            uvMin.y = uvMinOrig.y;
        }
        if (this.hasBorder)
        {
            uvMax = uvMaxOrig;
            for (float k = y; k < centerHeight; k += num2)
            {
                float num12 = k + num2;
                if (num12 > centerHeight)
                {
                    uvMax.y = uvMin.y + (((uvMax.y - uvMin.y) * (centerHeight - k)) / (num12 - k));
                    num12 = centerHeight;
                }
                this.AddQuad(vbo, simpleVert, new Vector2(0f, k) + pixelAdjustedRect.position, new Vector2(x, num12) + pixelAdjustedRect.position, new Vector2(outerUV.x, uvMin.y), new Vector2(uvMin.x, uvMax.y));
                this.AddQuad(vbo, simpleVert, new Vector2(centerWidth, k) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, num12) + pixelAdjustedRect.position, new Vector2(uvMaxOrig.x, uvMin.y), new Vector2(outerUV.z, uvMax.y));
                if (m_ButterflyY)
                {
                    float temp = uvMax.y;
                    uvMax.y = uvMin.y;
                    uvMin.y = temp;
                }
            }
            uvMax.y = uvMaxOrig.y;
            uvMin.y = uvMinOrig.y;
            uvMax = uvMaxOrig;
            for (float m = x; m < centerWidth; m += num)
            {
                float num14 = m + num;
                if (num14 > centerWidth)
                {
                    uvMax.x = uvMin.x + (((uvMax.x - uvMin.x) * (centerWidth - m)) / (num14 - m));
                    num14 = centerWidth;
                }
                this.AddQuad(vbo, simpleVert, new Vector2(m, 0f) + pixelAdjustedRect.position, new Vector2(num14, y) + pixelAdjustedRect.position, new Vector2(uvMin.x, outerUV.y), new Vector2(uvMax.x, uvMin.y));
                this.AddQuad(vbo, simpleVert, new Vector2(m, centerHeight) + pixelAdjustedRect.position, new Vector2(num14, pixelAdjustedRect.height) + pixelAdjustedRect.position, new Vector2(uvMin.x, uvMaxOrig.y), new Vector2(uvMax.x, outerUV.w));

                if (m_ButterflyX)
                {
                    float temp = uvMax.x;
                    uvMax.x = uvMin.x;
                    uvMin.x = temp;
                }
            }
            uvMax.x = uvMaxOrig.x;
            uvMin.x = uvMinOrig.x;
            this.AddQuad(vbo, simpleVert, new Vector2(0f, 0f) + pixelAdjustedRect.position, new Vector2(x, y) + pixelAdjustedRect.position, new Vector2(outerUV.x, outerUV.y), new Vector2(uvMin.x, uvMin.y));
            this.AddQuad(vbo, simpleVert, new Vector2(centerWidth, 0f) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y) + pixelAdjustedRect.position, new Vector2(uvMaxOrig.x, outerUV.y), new Vector2(outerUV.z, uvMin.y));
            this.AddQuad(vbo, simpleVert, new Vector2(0f, centerHeight) + pixelAdjustedRect.position, new Vector2(x, pixelAdjustedRect.height) + pixelAdjustedRect.position, new Vector2(outerUV.x, uvMaxOrig.y), new Vector2(uvMin.x, outerUV.w));
            this.AddQuad(vbo, simpleVert, new Vector2(centerWidth, centerHeight) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, pixelAdjustedRect.height) + pixelAdjustedRect.position, new Vector2(uvMaxOrig.x, uvMaxOrig.y), new Vector2(outerUV.z, outerUV.w));
        }
    }

    private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
    {
        float size = border.x + border.z;
        if (rect.size.x < size && size != 0)
        {            
            float scale = rect.size.x / size;
            border.x *= scale;
            border.z *= scale;
        }

        size = border.y + border.w;
        if (rect.size.y < size && size != 0)
        {            
            float scale = rect.size.y / size;
            border.y *= scale;
            border.w *= scale;
        }
        return border;
    }

    private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
    {
        // Padding is number of texels left,top,right,bottom that were trimmed off the sprite to pack it into the texture.
        Vector4 padding = (this.overrideSprite != null) ? TransformUV(DataUtility.GetPadding(this.overrideSprite)) : Vector4.zero;
        // Size is the original sprite size (may be bigger than the texture data)
        Vector2 size = (this.overrideSprite != null) ? new Vector2(this.overrideSprite.rect.width, this.overrideSprite.rect.height) : Vector2.zero;
        Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
        int num = Mathf.RoundToInt(size.x);
        int num2 = Mathf.RoundToInt(size.y);
        if (imageTransform >= ImageTransform.Rotate90CW)
        {
            int t = num;
            num = num2;
            num2 = t;
        }
        Vector4 normalizedPadding = new Vector4(padding.x / ((float)num), padding.y / ((float)num2), (num - padding.z) / ((float)num), (num2 - padding.w) / ((float)num2));
        if (shouldPreserveAspect && (size.sqrMagnitude > 0f))
        {
            float spriteAspect;
            
            if (imageTransform >= ImageTransform.Rotate90CW)
            {
                spriteAspect = size.y / size.x;
            }
            else
            {
                spriteAspect = size.x / size.y;
            }
            float renderAspect = pixelAdjustedRect.width / pixelAdjustedRect.height;
            if (spriteAspect > renderAspect)
            {
                float height = pixelAdjustedRect.height;
                pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / spriteAspect);
                pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * base.rectTransform.pivot.y;
            }
            else
            {
                float width = pixelAdjustedRect.width;
                pixelAdjustedRect.width = pixelAdjustedRect.height * spriteAspect;
                pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * base.rectTransform.pivot.x;
            }
        }
        return new Vector4(pixelAdjustedRect.x + (pixelAdjustedRect.width * normalizedPadding.x), pixelAdjustedRect.y + (pixelAdjustedRect.height * normalizedPadding.y), pixelAdjustedRect.x + (pixelAdjustedRect.width * normalizedPadding.z), pixelAdjustedRect.y + (pixelAdjustedRect.height * normalizedPadding.w));
    }

    public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 vector;
        if (this.m_EventAlphaThreshold >= 1f)
        {
            return true;
        }
        Sprite overrideSprite = this.overrideSprite;
        if (overrideSprite == null)
        {
            return true;
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out vector);
        Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
        vector.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
        vector.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
        vector = this.MapCoordinate(vector, pixelAdjustedRect);
        Rect textureRect = overrideSprite.textureRect;
        Vector2 vector2 = new Vector2(vector.x / textureRect.width, vector.y / textureRect.height);
        float u = Mathf.Lerp(textureRect.x, textureRect.xMax, vector2.x) / ((float)overrideSprite.texture.width);
        float v = Mathf.Lerp(textureRect.y, textureRect.yMax, vector2.y) / ((float)overrideSprite.texture.height);
        try
        {
            return (overrideSprite.texture.GetPixelBilinear(u, v).a >= this.m_EventAlphaThreshold);
        }
        catch (UnityException exception)
        {
            Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + exception.Message + " Also make sure to disable sprite packing for this sprite.", this);
            return true;
        }
    }

    private Vector2 MapCoordinate(Vector2 local, Rect rect)
        {
            Rect rect2 = this.sprite.rect;
            if ((this.type == Type.Simple) || (this.type == Type.Filled))
            {
                return new Vector2((local.x * rect2.width) / rect.width, (local.y * rect2.height) / rect.height);
            }
            Vector4 border = this.sprite.border;
            Vector4 adjustedBorders = this.GetAdjustedBorders((Vector4) (border / this.pixelsPerUnit), rect);
            for (int i = 0; i < 2; i++)
            {
                if (local[i] > adjustedBorders[i])
                {
                    if ((rect.size[i] - local[i]) <= adjustedBorders[i + 2])
                    {
                        // At max border
                        local[i] = rect2.size[i] - (rect.size[i] - local[i]);
                    }
                    else if (this.type == Type.Sliced)
                    {
                        float t = Mathf.InverseLerp(adjustedBorders[i], rect.size[i] - adjustedBorders[i + 2], local[i]);
                        local[i] = Mathf.Lerp(border[i], rect2.size[i] - border[i + 2], t);
                    }
                    else
                    {
                        float a = local[i] - adjustedBorders[i];
                        a = Mathf.Repeat(a, (rect2.size[i] - border[i]) - border[i + 2]);    // Note I assume -ve numbers just remain and then get added back to boarder beloow to become positive.
                        local[i] = a + border[i];
                    }
                }
            }
            return local;
        }

    public virtual void OnAfterDeserialize()
    {
        if (this.m_FillOrigin < 0)
        {
            this.m_FillOrigin = 0;
    }
        else if ((this.m_FillMethod == FillMethod.Horizontal) && (this.m_FillOrigin > 1))
        {
            this.m_FillOrigin = 0;
        }
        else if ((this.m_FillMethod == FillMethod.Vertical) && (this.m_FillOrigin > 1))
        {
            this.m_FillOrigin = 0;
        }
        else if (this.m_FillOrigin > 3)
        {
            this.m_FillOrigin = 0;
        }
        this.m_FillAmount = Mathf.Clamp(this.m_FillAmount, 0f, 1f);
    }

    public virtual void OnBeforeSerialize()
    {
    }
    protected override void OnFillVBO(List<UIVertex> vbo)
    {
        
/*            if (name.StartsWith("BlackLeft"))
            {
                Debug.Log(name+" FillVBO pixelsPerUnit=" + pixelsPerUnit + " size=" + base.rectTransform.sizeDelta);
            }*/
/*        if (m_SetSize)
        {
            m_Tracker.Clear();
            m_Tracker.Add(this, transform as RectTransform, DrivenTransformProperties.SizeDelta);
        }*/
        if (this.overrideSprite == null)
        {
            base.OnFillVBO(vbo);
        }
        else
        {
            switch (this.type)
            {
                case Type.Simple:
                    this.GenerateSimpleSprite(vbo, this.m_PreserveAspect);
                    break;

                case Type.Sliced:
                    this.GenerateSlicedSprite(vbo, this.m_PreserveAspect);
                    break;

                case Type.Tiled:
                    this.GenerateTiledSprite(vbo, this.m_PreserveAspect);
                    break;

                case Type.Filled:
                    this.GenerateFilledSprite(vbo, this.m_PreserveAspect);
                    break;
            }
        }
    }

    private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
    {
        if (fill < 0.001f)
        {
            return false;
        }
        if ((corner & 1) == 1)
        {
            invert = !invert;
        }
        if (invert || (fill <= 0.999f))
        {
            float f = Mathf.Clamp01(fill);
            if (invert)
            {
                f = 1f - f;
            }
            f *= 1.570796f;
            float cos = Mathf.Cos(f);
            float sin = Mathf.Sin(f);
            RadialCut(xy, cos, sin, invert, corner);
            RadialCut(uv, cos, sin, invert, corner);
        }
        return true;
    }


    private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
    {
        int index = corner;
        int num2 = (corner + 1) % 4;
        int num3 = (corner + 2) % 4;
        int num4 = (corner + 3) % 4;
        if ((corner & 1) == 1)
        {
            if (sin > cos)
            {
                cos /= sin;
                sin = 1f;
                if (invert)
                {
                    xy[num2].x = Mathf.Lerp(xy[index].x, xy[num3].x, cos);
                    xy[num3].x = xy[num2].x;
                }
            }
            else if (cos > sin)
            {
                sin /= cos;
                cos = 1f;
                if (!invert)
                {
                    xy[num3].y = Mathf.Lerp(xy[index].y, xy[num3].y, sin);
                    xy[num4].y = xy[num3].y;
                }
            }
            else
            {
                cos = 1f;
                sin = 1f;
            }
            if (!invert)
            {
                xy[num4].x = Mathf.Lerp(xy[index].x, xy[num3].x, cos);
            }
            else
            {
                xy[num2].y = Mathf.Lerp(xy[index].y, xy[num3].y, sin);
            }
        }
        else
        {
            if (cos > sin)
            {
                sin /= cos;
                cos = 1f;
                if (!invert)
                {
                    xy[num2].y = Mathf.Lerp(xy[index].y, xy[num3].y, sin);
                    xy[num3].y = xy[num2].y;
                }
            }
            else if (sin > cos)
            {
                cos /= sin;
                sin = 1f;
                if (invert)
                {
                    xy[num3].x = Mathf.Lerp(xy[index].x, xy[num3].x, cos);
                    xy[num4].x = xy[num3].x;
                }
            }
            else
            {
                cos = 1f;
                sin = 1f;
            }
            if (invert)
            {
                xy[num4].y = Mathf.Lerp(xy[index].y, xy[num3].y, sin);
            }
            else
            {
                xy[num2].x = Mathf.Lerp(xy[index].x, xy[num3].x, cos);
            }
        }
    }
    public override void SetNativeSize()
    {
        DoSetNativeSize(true);
    }

    void DoSetNativeSize(bool callSetDirtyIfNeeded = false)
    {
        bool changed = false;
        if (this.overrideSprite != null)
        {
            Vector2 size;
            size.x = CalcWidth(m_Scale.x);//Mathf.Round(this.overrideSprite.rect.width) / pixelsPerUnit;
            size.y = CalcHeight(m_Scale.y);//Mathf.Round(this.overrideSprite.rect.height) / pixelsPerUnit;
            /*if (setSize)
            {
                size.x *= m_Scale.x;
                size.y *= m_Scale.y;
            }*/
            if (base.rectTransform.anchorMax != base.rectTransform.anchorMin)
            {
                base.rectTransform.anchorMax = base.rectTransform.anchorMin;
                changed = true;
            }
            if (size != base.rectTransform.sizeDelta)
            {
/*                if (name.StartsWith("BlackLeft"))
				{
                    Debug.Log(name+": pixelsPerUnit=" + pixelsPerUnit + " size=" + size + " was=" + base.rectTransform.sizeDelta);
				}*/
                base.rectTransform.sizeDelta = size;
                changed = true;
            }
            if (callSetDirtyIfNeeded && changed)
            {
                this.SetAllDirty();
            }
            /*else if (name.StartsWith("BlackLeft"))
            {
                Debug.Log(name+": NoChange pixelsPerUnit=" + pixelsPerUnit + " size=" + base.rectTransform.sizeDelta);
            }*/
        }
    }
    // Returns the size of the sprite relative to the texture
    // if the texture is tilled then the value is a multiple of the center size
    public Vector2 GetCurrentRelativeSize()
    {
        if (this.overrideSprite != null)
        {
            Vector2 sizeDelta = rectTransform.rect.size;// rectTransform.sizeDelta;
            Vector2 size;
            size.x = Mathf.Round(this.overrideSprite.rect.width);
            size.y = Mathf.Round(this.overrideSprite.rect.height);
            if (IsRelativeToCenter)
            {
                Vector2 borders;
                borders.x = overrideSprite.border.x + overrideSprite.border.z;
                if (borders.x >= size.x)
                {
                    // Disable if there is no center
                    borders.x = 0;
                }
                borders.y = overrideSprite.border.y + overrideSprite.border.w;
                if (borders.y >= size.y)
                {
                    // Disable if there is no center
                    borders.y = 0;
                }
                sizeDelta -= borders / pixelsPerUnit;
                size -= borders;
            }
            size = size / pixelsPerUnit;
            size.x = sizeDelta.x / size.x;
            size.y = sizeDelta.y / size.y;
            return size;
        }
        return Vector2.one;
    }

    private bool IsRelativeToCenter
    {
        get
        {
            return m_SizeByCenter && hasBorder && (m_Type == Type.Tiled || m_Type == Type.Tiled);
        }
    }


    public void SetRelativeWidth(float size)
    {
        if (this.overrideSprite != null)
        {
            size = CalcWidth(size);
            if (size != base.rectTransform.rect.width)
            {
                base.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
                this.SetAllDirty();
            }
        }
    }

    public float CalcWidth(float size)
    {
        float width = Mathf.Round(this.overrideSprite.rect.width);
        if (IsRelativeToCenter)
        {
            float borders = overrideSprite.border.x + overrideSprite.border.z;
            if (borders < width)
            {
                size = size * (width - borders) + borders;
            }
            else
            {
                size *= width;
            }
        }
        else
        {
            size *= width;
        }
        size = size / pixelsPerUnit;
        return size;
    }

    public void SetRelativeHeight(float size)
    {
        if (this.overrideSprite != null)
        {
            size = CalcHeight(size);
            if (size != base.rectTransform.rect.height)
            {
                base.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
                this.SetAllDirty();
            }
        }
    }

    public float CalcHeight(float size)
    {
        float height = Mathf.Round(this.overrideSprite.rect.height);
        if (IsRelativeToCenter)
        {
            float borders = overrideSprite.border.y + overrideSprite.border.w;
            if (borders < height)
            {
                size = size * (height - borders) + borders;
            }
            else
            {
                size *= height;
            }
        }
        else
        {
            size *= height;
        }
        size = size / pixelsPerUnit;
        return size;
    }

    public float eventAlphaThreshold
    {
        get
        {
            return this.m_EventAlphaThreshold;
        }
        set
        {
            this.m_EventAlphaThreshold = value;
        }
        }

    public float fillAmount
    {
        get
        {
            return this.m_FillAmount;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<float>(ref this.m_FillAmount, Mathf.Clamp01(value)))
            {
                SetVerticesDirty();
        }
    }
    }

    public bool fillCenter
    {
        get
        {
            return this.m_FillCenter;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<bool>(ref this.m_FillCenter, value))
            {
                SetVerticesDirty();
        }
    }
    }

    public bool fillClockwise
    {
        get
        {
            return this.m_FillClockwise;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<bool>(ref this.m_FillClockwise, value))
            {
                SetVerticesDirty();
        }
    }
    }

    public FillMethod fillMethod
    {
        get
        {
            return this.m_FillMethod;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<FillMethod>(ref this.m_FillMethod, value))
            {
                SetVerticesDirty();
            this.m_FillOrigin = 0;
        }
    }
    }


    public ImageTransform imageTransform
    {
        get
        {
            return this.m_ImageTransform;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<ImageTransform>(ref this.m_ImageTransform, value))
            {
                SetVerticesDirty();
        }
    }
    }

    public int fillOrigin
    {
        get
        {
            return this.m_FillOrigin;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<int>(ref this.m_FillOrigin, value))
            {
                SetVerticesDirty();
        }
    }
    }

    public virtual float flexibleHeight
    {
        get
        {
            return -1f;
        }
    }

    public virtual float flexibleWidth
    {
        get
        {
            return -1f;
        }
    }

    public bool roundTiling
    {
        get
        {
            return m_RoundTiling;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<bool>(ref this.m_RoundTiling, value))
            {
                SetVerticesDirty();
            }
        }
    }

    public bool butterflyX
    {
        get
        {
            return m_ButterflyX;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<bool>(ref this.m_ButterflyX, value))
            {
                SetVerticesDirty();
            }
        }
    }

    public bool butterflyY
    {
        get
        {
            return m_ButterflyY;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<bool>(ref this.m_ButterflyY, value))
            {
                SetVerticesDirty();
            }
        }
    }

    public float scalePixelSize
    {
        get
        {
            return m_ScalePixelSize;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<float>(ref this.m_ScalePixelSize, value))
            {
                SetVerticesDirty();
            }
        }
    }


    public bool sizeByCenter
    {
        get
        {
            return m_SizeByCenter;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<bool>(ref this.m_SizeByCenter, value))
            {
                SetVerticesDirty();
            }
        }
    }
    

    public bool hasBorder
    {
        get
        {
            return ((this.overrideSprite != null) && (this.overrideSprite.border.sqrMagnitude > 0f));
        }
    }

    public virtual int layoutPriority
    {
        get
        {
            return 0;
        }
    }

    public override Texture mainTexture
    {
        get
        {
            return ((this.overrideSprite != null) ? this.overrideSprite.texture : Graphic.s_WhiteTexture);
        }
    }

    public virtual float minHeight
    {
        get
        {
            return 0f;
        }
    }
    
    public virtual float minWidth
    {
        get
        {
            return 0f;
        }
    }

    public Sprite overrideSprite
    {
        get
        {
            return ((this.m_OverrideSprite != null) ? this.m_OverrideSprite : this.sprite);
        }
        set
        {
            if (UtilsSetProperty.SetClass<Sprite>(ref this.m_OverrideSprite, value))
            {
                SetSize();
                SetAllDirty();
            }
        }
    }

    public virtual float preferredHeight
    {
        get
        {
            if (this.overrideSprite == null)
            {
                return 0f;
            }

            if (setSize)
            {
                return CalcHeight(1);// Mathf.Round(this.overrideSprite.rect.height) * m_Scale.y / pixelsPerUnit;
            }
            if ((this.type != Type.Sliced) && (this.type != Type.Tiled))
            {
                return this.overrideSprite.rect.size.y;
            }
            return DataUtility.GetMinSize(this.overrideSprite).y;
        }
    }

    public virtual float preferredWidth
    {
        get
        {
            if (this.overrideSprite == null)
            {
                return 0f;
            }
            if (setSize)
            {
                return CalcWidth(1);// Mathf.Round(this.overrideSprite.rect.width) * m_Scale.x / pixelsPerUnit;
            }
            if ((this.type != Type.Sliced) && (this.type != Type.Tiled))
            {
                return this.overrideSprite.rect.size.x;
            }
            return DataUtility.GetMinSize(this.overrideSprite).x;
        }
    }

    public bool preserveAspect
    {
        get
        {
            return this.m_PreserveAspect;
        }
        set
        {
            if(UtilsSetProperty.SetStruct<bool>(ref this.m_PreserveAspect, value))
            {
                SetVerticesDirty();
        }
    }
    }

    public bool setSize
    {
        get
        {
            return this.m_SetSize;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<bool>(ref this.m_SetSize, value))
            {
                SetAllDirty();
                SetSize();
        }
    }
    }
    
    public Vector2 scale
    {
        get
        {
            return this.m_Scale;
        }
        set
        {
            if (UtilsSetProperty.SetStruct<Vector2>(ref this.m_Scale, value))
            {
                SetAllDirty();
                SetSize();
        }
    }
    }


    public Sprite sprite
    {
        get
        {
            return this.m_Sprite;
        }
        set
        {
            if(UtilsSetProperty.SetClass<Sprite>(ref this.m_Sprite, value))
            {

                SetSize();
                SetAllDirty();
            }
        }
    }

    public Type type
    {
        get
        {
            return this.m_Type;
        }
        set
        {
            if(UtilsSetProperty.SetStruct<Type>(ref this.m_Type, value))
            {
                SetVerticesDirty();
        }
    }
    }
    
    public enum FillMethod
    {
        Horizontal,
        Vertical,
        Radial90,
        Radial180,
        Radial360
    }

    public enum Origin180
    {
        Bottom,
        Left,
        Top,
        Right
    }

    public enum Origin360
    {
        Bottom,
        Right,
        Top,
        Left
    }

    public enum Origin90
    {
        BottomLeft,
        TopLeft,
        TopRight,
        BottomRight
    }

    public enum OriginHorizontal
    {
        Left,
        Right
    }

    public enum OriginVertical
    {
        Bottom,
        Top
    }

    public enum Type
    {
        Simple,
        Sliced,
        Tiled,
        Filled
    }

    public enum ImageTransform
    {
        None,
        VerticalFlip,
        HorizontalFlip,
        Rotate180,
        Rotate90CW,
        Rotate90CCW
    }

    public float pixelsPerUnit
    {
        get
        {
            float num = 100f;
            if (this.sprite != null)
            {
                num = this.sprite.pixelsPerUnit;
            }
            float num2 = 100f;
            if (base.canvas != null)
            {
                num2 = base.canvas.referencePixelsPerUnit;
            }
            if (m_ScalePixelSize == 0)
            {
                return 100000;
            }
            return (num / num2) / m_ScalePixelSize;
        }
    }

    /*
    public override void Rebuild(CanvasUpdate update)
    {
        Debug.Log("Rebuild " + update);
        if (update == CanvasUpdate.Prelayout)
        {
            if (m_SetSize)
            {
                DoSetNativeSize();
            }
        }
        base.Rebuild(update);
    }*/


    Vector4 TransformUV(Vector4 uv)
    {
        switch (m_ImageTransform)
        {
            case ImageTransform.HorizontalFlip:
                {
                    float t = uv.z;
                    uv.z = uv.x;
                    uv.x = t;
                    break;
                }
            case ImageTransform.VerticalFlip:
                {
                    float t = uv.w;
                    uv.w = uv.y;
                    uv.y = t;
                    break;
                }

            case ImageTransform.Rotate180:
                {
                    float t = uv.w;
                    uv.w = uv.y;
                    uv.y = t;
                    t = uv.z;
                    uv.z = uv.x;
                    uv.x = t;
                    break;
                }

            case ImageTransform.Rotate90CCW:
                {
                    Vector4 prev = uv;
                    uv.x = prev.w;
                    uv.y = prev.x;
                    uv.z = prev.y;
                    uv.w = prev.z;
                    break;
                }
            case ImageTransform.Rotate90CW:
                {
                    Vector4 prev = uv;
                    uv.x = prev.y;
                    uv.y = prev.z;
                    uv.z = prev.w;
                    uv.w = prev.x;
                    break;
                }
        }
        return uv;
    }
    #if UNITY_EDITOR
    protected override void OnValidate()
    {
		// Comment out the line below and the project cross compiles on iOS.
        base.OnValidate();
		if (Application.isPlaying) return;	// Simulate onDevice functionality
        SetSize();
    }
    #endif

    protected override void OnTransformParentChanged()
    {
        SetSize();
        base.OnTransformParentChanged();
    }
            
    void SetSize()
    {
        m_Tracker.Clear();
        if (m_SetSize)
        {
            m_Tracker.Add(this, transform as RectTransform, DrivenTransformProperties.SizeDelta);
            SetNativeSize();
        }
    }

    protected override void OnDisable()
    {
        this.m_Tracker.Clear();
        base.OnDisable();
    }

    protected override void OnDidApplyAnimationProperties()
    {
        base.OnDidApplyAnimationProperties();
        SetSize();
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/UI/Image Pro", false, 5000)]
    static void Create()
    {
        if (UnityEditor.Selection.activeGameObject == null) return;
        RectTransform rectT = UnityEditor.Selection.activeGameObject.transform as RectTransform;
        if (rectT != null)
        {
            GameObject go = new GameObject("ImagePro");
            go.AddComponent<UIImagePro>();
            go.transform.parent = rectT;
            RectTransform rt = go.transform as RectTransform;
            rt.localPosition = Vector3.zero;
            rt.sizeDelta = new Vector2(100, 100);
        }
    }
#endif
}


#if false
namespace UnityEngine.UI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
    public abstract class LayoutGroup : UIBehaviour, ILayoutElement, ILayoutController, ILayoutGroup
    {
        [RenamedSerializedData("m_Alignment"), SerializeField]
        protected TextAnchor m_ChildAlignment;
        [SerializeField]
        protected RectOffset m_Padding = new RectOffset();
        [NonSerialized]
        private RectTransform m_Rect;
        [NonSerialized]
        private List<RectTransform> m_RectChildren = new List<RectTransform>();
        private Vector2 m_TotalFlexibleSize = Vector2.zero;
        private Vector2 m_TotalMinSize = Vector2.zero;
        private Vector2 m_TotalPreferredSize = Vector2.zero;
        protected DrivenRectTransformTracker m_Tracker;

        protected LayoutGroup()
        {
            if (this.m_Padding == null)
            {
                this.m_Padding = new RectOffset();
            }
        }

        public virtual void CalculateLayoutInputHorizontal()
        {
            this.m_RectChildren.Clear();
            for (int i = 0; i < this.rectTransform.childCount; i++)
            {
                RectTransform child = this.rectTransform.GetChild(i) as RectTransform;
                ILayoutIgnorer component = child.GetComponent(typeof(ILayoutIgnorer)) as ILayoutIgnorer;
                if (((child != null) && child.gameObject.activeInHierarchy) && ((component == null) || !component.ignoreLayout))
                {
                    this.m_RectChildren.Add(child);
                }
            }
            this.m_Tracker.Clear();
        }

        public abstract void CalculateLayoutInputVertical();
        protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
        {
            float num = requiredSpaceWithoutPadding + ((axis != 0) ? ((float) this.padding.vertical) : ((float) this.padding.horizontal));
            float num2 = this.rectTransform.rect.size[axis];
            float num3 = num2 - num;
            float num4 = 0f;
            if (axis == 0)
            {
                num4 = ((float) (this.childAlignment % TextAnchor.MiddleLeft)) * 0.5f;
            }
            else
            {
                num4 = ((float) (this.childAlignment / TextAnchor.MiddleLeft)) * 0.5f;
            }
            return (((axis != 0) ? ((float) this.padding.top) : ((float) this.padding.left)) + (num3 * num4));
        }

        protected float GetTotalFlexibleSize(int axis)
        {
            return this.m_TotalFlexibleSize[axis];
        }

        protected float GetTotalMinSize(int axis)
        {
            return this.m_TotalMinSize[axis];
        }

        protected float GetTotalPreferredSize(int axis)
        {
            return this.m_TotalPreferredSize[axis];
        }

        protected override void OnDidApplyAnimationProperties()
        {
            this.SetDirty();
        }

        protected override void OnDisable()
        {
            this.m_Tracker.Clear();
        }

        protected override void OnEnable()
        {
            this.SetDirty();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (this.isRootLayoutGroup)
            {
                this.SetDirty();
            }
        }

        protected virtual void OnTransformChildrenChanged()
        {
            this.SetDirty();
        }

        protected override void OnValidate()
        {
            this.SetDirty();
        }

        protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
        {
            if (rect != null)
            {
                this.m_Tracker.Add(this, rect, DrivenTransformProperties.SizeDelta | DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition);
                rect.SetInsetAndSizeFromParentEdge((axis != 0) ? RectTransform.Edge.Top : RectTransform.Edge.Left, pos, size);
            }
        }

        protected void SetDirty()
        {
            if (this.IsActive())
            {
                LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
            }
        }

        public abstract void SetLayoutHorizontal();
        protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
        {
            this.m_TotalMinSize[axis] = totalMin;
            this.m_TotalPreferredSize[axis] = totalPreferred;
            this.m_TotalFlexibleSize[axis] = totalFlexible;
        }

        public abstract void SetLayoutVertical();
        protected void SetProperty<T>(ref T currentValue, T newValue)
        {
            if (((((T) currentValue) != null) || (newValue != null)) && ((((T) currentValue) == null) || !currentValue.Equals(newValue)))
            {
                currentValue = newValue;
                this.SetDirty();
            }
        }

        public TextAnchor childAlignment
        {
            get
            {
                return this.m_ChildAlignment;
            }
            set
            {
                this.SetProperty<TextAnchor>(ref this.m_ChildAlignment, value);
            }
        }

        public float flexibleHeight
        {
            get
            {
                return this.GetTotalFlexibleSize(1);
            }
        }

        public float flexibleWidth
        {
            get
            {
                return this.GetTotalFlexibleSize(0);
            }
        }

        private bool isRootLayoutGroup
        {
            get
            {
                return ((base.transform.parent == null) || (base.transform.parent.GetComponent(typeof(ILayoutGroup)) == null));
            }
        }

        public int layoutPriority
        {
            get
            {
                return 0;
            }
        }

        public float minHeight
        {
            get
            {
                return this.GetTotalMinSize(1);
            }
        }

        public float minWidth
        {
            get
            {
                return this.GetTotalMinSize(0);
            }
        }

        public RectOffset padding
        {
            get
            {
                return this.m_Padding;
            }
            set
            {
                this.SetProperty<RectOffset>(ref this.m_Padding, value);
            }
        }

        public float preferredHeight
        {
            get
            {
                return this.GetTotalPreferredSize(1);
            }
        }

        public float preferredWidth
        {
            get
            {
                return this.GetTotalPreferredSize(0);
            }
        }

        protected List<RectTransform> rectChildren
        {
            get
            {
                return this.m_RectChildren;
            }
        }

        protected RectTransform rectTransform
        {
            get
            {
                if (this.m_Rect == null)
                {
                    this.m_Rect = base.GetComponent<RectTransform>();
                }
                return this.m_Rect;
            }
        }
    }
}

#endif
