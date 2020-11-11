using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YB.XWK.MainScene;
/// <summary>
/// 1、图标加倒计时，图标的倒计时跟内部宝箱的倒计时保持一致
/// 2、宝箱处于可领取显示红点
/// </summary>
public class MainCityUpSpiritAdMgr : MonoBehaviour {
    private GameObject _State;
    private C_Event m_GameEvent = new C_Event();

    // Use this for initialization
    void Start () {
        _State = transform.Find("ui_public_effect_bxtb").gameObject;
        _State.gameObject.SetActive(false);
        m_GameEvent.RegisterEvent(C_EnumEventChannel.Global, "MainGameEvent", (object[] result) =>
        {
            if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME
            || (GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME_RESUME_FALSE)
            {
                 if (ChestData.FetchChestState() == 1)//时间到未获取，则显示红点
                {
                    _State.gameObject.SetActive(true);
                }
                else
                {
                    _State.gameObject.SetActive(false);
                }
            }
        });
    }

    private void OnDestroy()
    {
        if (m_GameEvent!=null)
        {
            m_GameEvent.UnregisterEvent();
        }
    }
}
