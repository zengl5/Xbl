using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
public class HuluCard : MonoBehaviour, IRubbish
{
    #region#卡片初始化
    Vector3 loopMoveTargetPos;
    DjData djdata;
    bool Click = false;
    Transform BgEffectParent = null;
    GameObject RecycleEffect = null;
    GameObject StarEffect = null;
    HlGame hlgame;
    bool isChooseRight = false;
    Action ac;
    public DjData GetDjdata()
    {
        return djdata;
    }
    public bool ClickMesh()
    {
        return Click;
    }
    public void RecycleRubbish()
    {
        StopAllCoroutines();
        if (RecycleEffect)
            Destroy(RecycleEffect);
        if (StarEffect)
            Destroy(StarEffect);
    }

    /// <summary>
    /// 道具循环上下移动
    /// </summary>
    void LoopMove()
    {
        Tween tw;
        loopMoveTargetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.125f);
        tw = transform.DOLocalMove(loopMoveTargetPos, 1f);
        tw.SetLoops(1000, LoopType.Yoyo);
        tw.SetEase(Ease.Linear);
    }
    public void InitDjdata(DjData card, HlGame sp)
    {
        isChooseRight = false;
        transform.localPosition = Vector3.zero;
        hlgame = sp;
        this.djdata = card;
        Click = false;
    }
    public void MoveDaoju(Action ac)
    {
        Vector3 targetScale = transform.localScale;
        this.ac = ac;
        Invoke("SetMeshButtonInteractable", 1);
        switch (djdata.Id)
        {
            case 0:
                Invoke("TogroundSound", 0.5f);
                break;
            case 1:
                Invoke("TogroundSound", 0.6f);
                break;
            case 2:
                Invoke("TogroundSound", 0.7f);
                break;
        }
    }
    //落地声音
    void TogroundSound()
    {
        hlgame.PlayToGroundEffect(djdata);
        LoopMove();
        HuluManager.Instance.PlayEffectAudio("public_xwkyx_018");
    }
    //落地以后打开按钮点击状态
    void SetMeshButtonInteractable()
    {
        if (ac != null)
            ac();
    }
    #endregion
    //玩家点击卡片
    public void OnclickDaojuEvent()
    {
        Click = true;
        if (this.djdata.IsRight)
        {
            isChooseRight = true;
            Invoke("AddRecycleEffect", 3);
            Invoke("OpenStarEffect", 3);
            Invoke("StartFly", 3.5f);
            hlgame.SetChooseDJResult(true);
        }
        else
        {
            isChooseRight = false;
            hlgame.SetChooseDJResult(false);
        }
    }
    #region#选择卡片结束逻辑处理
    //汇聚特效
    void AddRecycleEffect()
    {
        if (BgEffectParent == null)
        {
            BgEffectParent = GameObject.Find("HuluUICanvas").transform;
        }
        if (RecycleEffect == null)
        {
            RecycleEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_kphj, ABCommonConfig.EfBundleType, true);
            RecycleEffect.transform.parent = BgEffectParent;
            RecycleEffect.transform.localPosition = Vector3.zero;
            RecycleEffect.transform.localEulerAngles = Vector3.zero;
            RecycleEffect.transform.localScale = Vector3.one;
        }
        else
        {
            RecycleEffect.SetActive(false);
            RecycleEffect.SetActive(true);
        }
    }
    void OpenStarEffect()
    {
        if (StarEffect == null)
        {
            BgEffectParent = GameObject.Find("HuluUICanvas").transform;
        }
        if (StarEffect == null)
        {
            StarEffect = ABResMgr.Instance.LoadResource<GameObject>(SpfilePath.Instance.ui_public_effect_xxtw, ABCommonConfig.EfBundleType, true);
            StarEffect.transform.parent = BgEffectParent;
            StarEffect.transform.localPosition = Vector3.zero;
            StarEffect.transform.localEulerAngles = Vector3.zero;
            StarEffect.transform.localScale = 1f * Vector3.one;

        }
        else
        {
            StarEffect.transform.localPosition = Vector3.zero;
            StarEffect.transform.localEulerAngles = Vector3.zero;
            StarEffect.transform.localScale = 1f * Vector3.one;
            StarEffect.SetActive(false);
            StarEffect.SetActive(true);
        }
     }
    void StartFly()
    {
        Vector3[] posList = new Vector3[] { new Vector3(890, 364, 0), new Vector3(890, 265, 0), new Vector3(890, 163, 0), new Vector3(890, 163, 0), new Vector3(850, 16, 0) };
        if (HuluManager.Instance.GetLightId() <= posList.Length - 1 && HuluManager.Instance.GetLightId() >= 0)
            StarEffect.transform.DOLocalMove(posList[HuluManager.Instance.GetLightId()], 0.5F).OnComplete(HuluManager.Instance.LightStar);
    }
    #endregion
}

