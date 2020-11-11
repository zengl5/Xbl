using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using XWK.Common.UI_Reward;
using YB.XWK.MainScene;

public class coinIcon : MonoBehaviour, IPointerEnterHandler
{
    Vector3 TargetPos;
    public Action<string> HitCoinAction;
    public Action HitCoinAction2;
    SpLingqi lingqi;
    static bool HavePlayAuido = false;
    Vector3 InitLocalPostion=Vector3.zero;
    public void InitCoinState(SpLingqi lq)
    {
        InitLocalPostion = this.transform.localPosition;
        lingqi = lq;
        AddMoveTween();
    }
    public Vector3 GetInitLocalPostion()
    {
        //Debug.LogError(InitLocalPostion);
         return InitLocalPostion;
    }
    /// <summary>
    /// 收集灵气方法
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //AddDestoryEffect();//爆炸特效
        if (!HavePlayAuido)
        {
            HavePlayAuido = true;
            PlayAudio();
        }
        if(lingqi!=null)
        lingqi.RemoveLingqi(this.gameObject);
        Destroy(this.gameObject);
        if (HitCoinAction != null)
            HitCoinAction(lingqi.Name);
        if (HitCoinAction2 != null)
            HitCoinAction2();
        RewardUIManager.Instance.RegisterOfflineBonus(SpriteLingqiMgr.Instance.GetLqCam().WorldToScreenPoint(transform.position),null,lingqi.spData.Name);
        RewardUIManager.Instance.SetSuccess();
        Spwindow.PlaySmallClipSound("public_xwkyx_086");
        AddReport();
        AddDestoryEffect();
    }
    void AddReport()//用户点击收集 上报数据?
    {
        if (lingqi.Name != "")
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.offlinerevenue, "Recycle:" + lingqi.Name);
    }
    void AddMoveTween()
    {
        TargetPos = new Vector3(transform.localPosition.x, transform.localPosition.y+10, transform.localPosition.z);
        Tween tw = transform.DOLocalMove(TargetPos, UnityEngine.Random.Range(0.65F,1F));
        tw.SetLoops(1000, LoopType.Yoyo);
        tw.SetEase(Ease.Linear);
    }

    void AddDestoryEffect()
    {
        GameObject zha = ABResMgr.Instance.LoadResource<GameObject>(SpWindowPath.Instance.ui_effect_xln_zha, ABCommonConfig.EfBundleType, true);
        zha.transform.parent = this.transform.parent;
        zha.transform.localPosition = this.transform.localPosition;
        zha.transform.localScale = 1f * Vector3.one;
        foreach (Transform tran in zha.GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = LayerMask.NameToLayer("SpUI");
        }
        //Destroy(zha, 2);
    }

    //播放首次收集精灵
    void PlayAudio()
    {
        float rdIndex = UnityEngine.Random.Range(0f, 1f);

        if(rdIndex>=0.66f)
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_17");
        }
        else if(rdIndex>=0.33f)
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_18");
        }
        else
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_19");
        }
    }

}
