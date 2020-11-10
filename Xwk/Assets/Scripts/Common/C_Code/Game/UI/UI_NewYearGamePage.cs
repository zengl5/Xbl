using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XWK.Common.UI_Reward;
using YB.YM.Game;
using DG.Tweening;
public class UI_NewYearGamePage :  C_BaseUI{

    public Action PauseAction = null;
    private C_Event _BloodEvent;
    private C_Event _SkillUIEvent;
    [SerializeField]
    public RectTransform _BloodBar;
    private YMGameWoundedType _WoundedType;
    private float totalTime =   5 * 60f;
    private int totalHurtBlood = 0;
    private RectTransform _SkillRtf;
    private Sequence _SkillSequence;
    private float width = 663f;
    private Image _BloodImg;
    private bool _SkillFlag = true;
    private Transform _TimeClock;
    private Transform _Digiht;
    private Transform _Tens;
    private Transform _Hundred;
    private bool _GameOver = false;
    protected override void onOpenUI(params object[] uiObjParams)
    {
        _GameOver = false; 
        _TimeClock = transform.Find("Canvas/Title/Time/clockui");
        _Digiht =  _TimeClock.Find("ones");
        _Tens =  _TimeClock.Find("tens");
        _Hundred =  _TimeClock.Find("hundred");
        _SkillFlag = true;
        _BloodImg = _BloodBar.GetComponent<Image>();
        _BloodImg.color = new Color(2 / 255f, 97 / 255f, 255 / 255f);

        _SkillRtf = transform.Find("Canvas/Skill").GetComponent<RectTransform>();
        if (uiObjParams.Length >= 1)
        {
            PauseAction = (Action)uiObjParams[0];
        }
        UICanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        if (_BloodEvent != null)
        {
            _BloodEvent.UnregisterEvent();
            _BloodEvent = null;
        }
        _SkillUIEvent = new C_Event();
        _SkillUIEvent.RegisterEvent(C_EnumEventChannel.Global, "SkillUIEvent", (object[] result) => {
            if ((int)result[0]==1)
            {
                CloseSkillUI();
            }
            else
            {
                ResumeSkillUI();
            }
        });
        _BloodEvent = new C_Event();
        _BloodEvent.RegisterEvent(C_EnumEventChannel.Global, "YMGameBloodEvent", (object[] result) => {
            float progress = 0f;
            if (result.Length > 1)
            {
                progress =  ((int)result[1]) / 3688f;
                _BloodBar.sizeDelta   =new Vector2(_BloodBar.sizeDelta.x - width * progress, _BloodBar.sizeDelta.y);
                _WoundedType = (YMGameWoundedType)result[0];
                totalHurtBlood += (int)result[1];
            }
            else
            {
                progress = ((int)result[0]) / 3688f;
                _BloodBar.sizeDelta = new Vector2(_BloodBar.sizeDelta.x-width * progress, _BloodBar.sizeDelta.y);
                totalHurtBlood += ((int)result[0]);
            }
            if (totalHurtBlood > 50)
            {
                int star = totalHurtBlood / 50-1;
                totalHurtBlood = 0;
                if (_WoundedType == YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_MARROON)
                {
                    star -= 5;
                }
                if (star <= 0)
                {
                    star = 0;
                }
                RewardUIManager.Instance.RegisterOfflineBonus(1,new Vector2(Screen.width/2, Screen.height/ 2),ModuleType.SpriteWindow,(b)=> {
                    RewardUI.Instance.UpdateScore(star);
                });
                RewardUIManager.Instance.SetSuccess();
            }
            if (_BloodBar.sizeDelta.x <= 20f)
            {
                _GameOver = true;
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_SUCCESS);
            }
            progress = _BloodBar.sizeDelta.x / width;
            if (progress >0.8)
            {
                _BloodImg.color = new Color(2f/255f,97f/255f,255/255f);
            }
            else if (progress > 0.6)
            {
                
                _BloodImg.color = new Color(32f / 255f, 194/255f, 0);
            }
            else if (progress > 0.4)
            {
                
                _BloodImg.color = new Color(255f / 255f, 96/255f, 0);
            }
            else
            {
              
                _BloodImg.color = new Color(244f / 255f, 0, 0);
            }
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_MONSTER_LEVELDOWN,(int)(progress * 100));

        });
        totalTime =   5 * 60f;
        if (!IsInvoking())
        {
            Invoke("UpdateTime", 1);
        }
    }
    void CloseSkillUI()
    { 
        _SkillFlag = false;
        if (_SkillSequence!=null)
        {
            _SkillSequence.Kill();
        }
        _SkillSequence = DOTween.Sequence();
        if (_SkillRtf!=null)
        {
            _SkillSequence.Append(_SkillRtf.DOAnchorPosY(525f + 25f, 0.2f)).Append(_SkillRtf.DOAnchorPosY(525f -250f, 0.1f));
        }
    }
    void ResumeSkillUI()
    {
        _SkillFlag = true;

        if (_SkillSequence != null)
        {
            _SkillSequence.Kill();
        }
        _SkillSequence = DOTween.Sequence();
        if (_SkillRtf != null)
        {
            _SkillSequence.Append(_SkillRtf.DOAnchorPosY(525f , 0.2f));
        }
    }
    public void Pause()
    {
        if (PauseAction != null)
        {
            AppInfoData.ResumehNewYearGameTime();

            PauseAction();
            PauseAction = null;
        }
    }

    public void SerRenderMode(RenderMode renderMode)
    {
        UICanvas.renderMode = renderMode;
    }
    public void UpdateTime()
    {
        if (_GameOver)
        {
            return;
        }
        totalTime--;
        if (totalTime >= 0)
        {
            UpdateTime(totalTime);
            if (!IsInvoking())
            {
                Invoke("UpdateTime", 1);
            }
        }
        else
        {
            _GameOver = true;

            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_FAIL);

            CancelInvoke("UpdateTime");
        }
    }
    private void UpdateTime(float time)
    {
        float digit = time % 10;
        float tens = (time / 10)%10;
        float hundreds = (time  /100) % 10;
        HideNumber(_Digiht);
        HideNumber(_Tens);
        HideNumber(_Hundred);
        SetNumberVisible(_Digiht,(int)digit);
        SetNumberVisible(_Tens, (int)tens);
        SetNumberVisible(_Hundred, (int)hundreds);
    }
    void SetNumberVisible(Transform node,int id)
    {
        int length = node.childCount;
        for (int i = 0; i < length; i++)
        {
            if(i== id)
            {
                node.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    void HideNumber(Transform node)
    {
        int length = node.childCount;
        for (int i=0;i < length;i++)
        {
            node.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void ClosePauseUI()
    {
        CloseUI();
    }
    protected override void onCloseUI()
    {
        CancelInvoke("UpdateTime");
        if (_SkillSequence != null)
        {
            _SkillSequence.Kill();
        }
        if (_BloodEvent != null)
        {
            _BloodEvent.UnregisterEvent();
            _BloodEvent = null;
        }
    }
    public void EnterSkillState(int skillType)
    {
        if (!_SkillFlag)
        {
            return; 
        }
      //  CloseSkillUI();

        _SkillFlag = false;

        if (skillType == 1)
        {
            SkillIconState state =  transform.Find("Canvas/Skill/game_snow").GetComponent<SkillIconState>();
            if (state.isFreeze) {
                _SkillFlag = true;
                return;
            }
            state.FreezeSkill();
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_MONSTER_WOUND_SNOW);
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "skill_snow");

        }
        else if(skillType == 2)
        {
            SkillIconState state = transform.Find("Canvas/Skill/game_jgb").GetComponent<SkillIconState>();
            if (state.isFreeze)
            {
                _SkillFlag = true;
                return;
            }
            state.FreezeSkill();
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_MONSTER_WOUND_JGB);
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "skill_jgb");

        }
        else if (skillType == 3)
        {
            SkillIconState state = transform.Find("Canvas/Skill/game_fss").GetComponent<SkillIconState>();
            if (state.isFreeze)
            {
                _SkillFlag = true;
                return;
            }
            state.FreezeSkill();
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_MONSTER_WOUND_FSS);
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "skill_fss");

        }
        else if (skillType == 4)
        {
            SkillIconState state = transform.Find("Canvas/Skill/game_marron").GetComponent<SkillIconState>();
            if (state.isFreeze)
            {
                _SkillFlag = true;
                return;
            }
            state.FreezeSkill();
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_MONSTER_WOUND_MARROON);
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "skill_marron");

        }
    }
}
