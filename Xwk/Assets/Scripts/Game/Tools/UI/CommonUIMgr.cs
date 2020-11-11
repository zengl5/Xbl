using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
class UiShow
{
    Color alphaZero = new Vector4(1, 1, 1, 0);
    Color colorNormal = 4 * Vector4.one;
    Color colorZhihui = new Vector4(204 / 255f, 197 / 255f, 182 / 255f, 1);
    //204,197,182,255
    GameObject btn = null;
    Action ac;
    RawImage rawImage = null;
    Image image = null;
    public UiShow(GameObject btn, Action ac)
    {
        this.btn = btn;
        this.ac = ac;
    }
    void InitBtnData()
    {
        if (btn == null)
            return;
        if (!btn.transform.gameObject.activeInHierarchy)
            btn.transform.gameObject.SetActive(true);
        //MaskableGraphic
        if (btn.transform.GetComponent<RawImage>() != null)
        {
            rawImage = btn.transform.GetComponent<RawImage>();
            rawImage.color = alphaZero;
        }
        else if (btn.transform.GetComponent<Image>() != null)
        {
            image = btn.transform.GetComponent<Image>();
            image.color = alphaZero;
        }
    }
    public void Open()
    {
        InitBtnData();
        if (btn == null)
            return;
        btn.transform.localScale = 0.6f * Vector3.one;
        Sequence seq = DOTween.Sequence();

        if (rawImage != null)
        {
            rawImage.color = new Vector4(1, 1, 1, 0.6f);
            seq.Append(btn.transform.DOScale(1.05f * Vector3.one, 0.17f));
            seq.Insert(0, rawImage.DOColor(colorNormal, 0.17f));

            seq.Append(btn.transform.DOScale(0.96f * Vector3.one, 0.17f));

            seq.Append(btn.transform.DOScale(Vector3.one, 0.16f).OnComplete
                (
                delegate
                {
                    if (ac != null)
                        ac();
                })
                );//关闭
        }
        else
        {
            if (image == null)
            {
                Debug.LogError("UI关闭Error,检查UI关闭参数【参数是否是Image？】");
            }
            image.color = new Vector4(1, 1, 1, 0.6f);
            seq.Append(btn.transform.DOScale(1.05f * Vector3.one, 0.17f));
            seq.Insert(0, image.DOColor(colorNormal, 0.17f));

            seq.Append(btn.transform.DOScale(0.96f * Vector3.one, 0.17f));

            seq.Append(btn.transform.DOScale(Vector3.one, 0.16f).OnComplete
                (
                delegate
                {
                    if (ac != null)
                        ac();
                })
                );//关闭
        }
    }
    public void Close()
    {
        InitBtnData();
        if (btn == null)
            return;

        btn.transform.localScale = Vector3.one;
        Sequence seq = DOTween.Sequence();

        if (rawImage != null)
        {
            rawImage.color = colorNormal;
            seq.Append(btn.transform.DOScale(0.8f * Vector3.one, 0.17f));
            seq.Insert(0, rawImage.DOColor(colorNormal, 0.17f));

            seq.Append(btn.transform.DOScale(Vector3.one, 0.125f));
            seq.Insert(0.17F, rawImage.DOColor(colorNormal, 0.125f));//不置灰

            seq.Append(rawImage.DOColor(alphaZero, 0.164f).OnComplete
                (
                delegate
                {
                    if (ac != null)
                        ac();
                })
                );//关闭
        } 
        else
        {
            if(image==null)
            {
                Debug.LogError("UI关闭Error,检查UI关闭参数【参数是否是Image？】");
            }
            else
            {
                image.color = colorNormal;
                seq.Append(btn.transform.DOScale(0.8f * Vector3.one, 0.17f));
                seq.Insert(0, image.DOColor(colorNormal, 0.8f));

                seq.Append(btn.transform.DOScale(Vector3.one, 0.12f));
                seq.Insert(0.17f, image.DOColor(colorNormal, 0.12f));//不置灰

                seq.Append(image.DOColor(alphaZero, 0.16f).OnComplete
                    (
                    delegate
                    {
                        if (ac != null)
                            ac();
                    })
                    );//关闭;//关闭
            }
        }
             
    }
}
class FcUiShow
{
    Color alphaZero = new Vector4(1, 1, 1, 0);
    Color colorNormal = 4 * Vector4.one;
    Color colorZhihui = new Vector4(204 / 255f, 197 / 255f, 182 / 255f, 1);
    //204,197,182,255
    GameObject btn = null;
    Action ac;
    RawImage rawImage = null;
    Image image = null;
    public FcUiShow(GameObject btn, Action ac)
    {
        this.btn = btn;
        this.ac = ac;
    }
    void InitBtnData()
    {
        if (btn == null)
            return;
        if (!btn.transform.gameObject.activeInHierarchy)
            btn.transform.gameObject.SetActive(true);
        //MaskableGraphic
        if (btn.transform.GetComponent<RawImage>() != null)
        {
            rawImage = btn.transform.GetComponent<RawImage>();
            rawImage.color = alphaZero;
        }
        else if (btn.transform.GetComponent<Image>() != null)
        {
            image = btn.transform.GetComponent<Image>();
            image.color = alphaZero;
        }
    }
    public void Click()
    {
        InitBtnData();
        Sequence seq = DOTween.Sequence();
        if (rawImage != null)
        {
            rawImage.color = colorNormal;
            btn.transform.localScale = Vector3.one;

            seq.Append(btn.transform.DOScale(0.8f * Vector3.one, 0.17f));
            seq.Insert(0, rawImage.DOColor(colorNormal, 0.17f));

            seq.Insert(0.17f, rawImage.DOColor(colorNormal, 0.12f));

            seq.Append(btn.transform.DOScale(Vector3.one, 0.12f).OnComplete
                (
                delegate
                {
                    if (ac != null)
                        ac();
                })
                );//关闭
        }
        else
        {
            if (image == null)
            {
                Debug.LogError("UI关闭Error,检查UI关闭参数【参数是否是Image？】");
            }
            image.color = colorNormal;
            btn.transform.localScale = Vector3.one;

            seq.Append(btn.transform.DOScale(0.8f * Vector3.one, 0.17f));
            seq.Insert(0, image.DOColor(colorNormal, 0.17f));

            seq.Insert(0.17f, image.DOColor(colorNormal, 0.12f));

            seq.Append(btn.transform.DOScale(Vector3.one, 0.12f).OnComplete
                (
                delegate
                {
                    if (ac != null)
                        ac();
                })
                );//关闭        }
        }
    }
}
public class CommonUIMgr : MonoSingleton<CommonUIMgr> {


    #region##普通按钮
    /// <summary>
    /// 按钮出现
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="ac"></param>
    public void ShowUI(GameObject btn,Action ac=null)
    {
        UiShow ui = new UiShow(btn,ac);
        ui.Open();
    }
    /// <summary>
    /// 按钮消失【即点击后需要消失】
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="ac"></param>
    public void CloseUI(GameObject btn, Action ac = null)
    {
        UiShow ui = new UiShow(btn, ac);
        ui.Close();
    }
    #endregion

    /// <summary>
    /// 图标点击效果
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="ac"></param>
    public void ClickFunctionButton(GameObject btn,Action ac=null)
    {
        FcUiShow fc= new FcUiShow(btn,ac);
        fc.Click();
    }

    //普通点击


    //长按
   
}
