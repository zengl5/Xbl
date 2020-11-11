using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YB.XWK.MainScene;
using DG.Tweening;
public class Futureinfo : baseRoleInfo {
     
    Vector3 TargetPos;
    float Timer = 1;
    void Awake()
    {
        base.Awake();
        animLength = new[] { 2f, 1.1f};
        AddBoxCollider();           
        Invoke("RandomFuturePlayAnim", animLength[0]);
    }

    void AddBoxCollider()
    {
        BoxCollider box = this.gameObject.GetAddComponent<BoxCollider>();
        box.size = new Vector3(80, 100, 50);
        box.center = new Vector3(0, 70, 0);
        box.tag = "meshButton";
    }
    public override void OnClickRoleEvent()
    {
        // base.OnClickRoleEvent();
        float id = Random.Range(0f, 1f);
        //还没有解锁 播放语音
        if (id<=0.33f)
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_8");
        }
        else if (id <= 0.66f)
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_9");
        }
        else if (id <= 1f)
        {
            Spwindow.PlayCharacterAudio("xwk_jlej_10");
        }
    }
    public override void Refresh()
    {
        base.Refresh();
        CancelInvoke("RandomFuturePlayAnim");
        Invoke("RandomFuturePlayAnim", animLength[0]);       
    }
    void RandomFuturePlayAnim()
    {
        if (!this.gameObject.activeInHierarchy)
            return;
        float rd = Random.Range(0f, 1f);
        if (rd >=0.3f)
        {
            anim.Play("stand01#anim");
            Invoke("RandomFuturePlayAnim", animLength[0]);
        }
        else
        {
            anim.Play("stand02#anim");
            Invoke("BackPlay", animLength[1]);
        }
    }
    void BackPlay()
    {
        anim.Play("stand01#anim");
        Invoke("RandomFuturePlayAnim", animLength[0]);
    }
}
