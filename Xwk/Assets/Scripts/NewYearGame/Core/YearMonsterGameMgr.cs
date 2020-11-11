using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XWK.Common.UI_Reward;

namespace YB.YM.Game
{
    public enum YMGameEvet
    {
        YMG_EVENT_SKILL_SNOW=0,
        YMG_EVENT_MONSTER_LEVELDOWN= 1,//变身
        YMG_EVENT_MONSTER_WOUND_FSS = 2,//分身术攻击
        YMG_EVENT_MONSTER_WOUND_SNOW = 3,//大雪球攻击
        YMG_EVENT_MONSTER_WOUND_JGB = 4,//金箍棒攻击
        YMG_EVENT_MONSTER_WOUND_ASR = 5,//语音识别
        YMG_EVENT_MONSTER_WOUND_MARROON = 6,//鞭炮攻击
        YMG_EVENT_GAME_SUCCESS = 7,//胜利
        YMG_EVENT_GAME_FAIL = 8,//失败
        YMG_EVENT_GAME_SUCCESS_0VER = 9,//胜利
        YMG_EVENT_SHOW_XWK_HELLO=10,//小悟空打招呼
        YMG_EVENT_SHOW_FAIL = 11,// 
        YMG_EVENT_GAME_SUCCESS_UI = 12,//收集碎片
        YMG_EVENT_GAME_FAIL_QUIT = 13,//失败退出
        YMG_EVENT_GAME_START = 14,//游戏开始
        YMG_EVENT_GAME_XWK_SUCCESS_START = 15,//小悟空开始做成功动作
        YMG_EVENT_GAME_XWK_FAIL_START = 16,//小悟空开始做失败动作

    }

    public class YearMonsterGameMgr : MonoBehaviour
    {
        private NewYearCameraMgr _NewYearCameraMgr;
        private YearMonsterMgr _YearMonsterMgr;
        private YearXwkMgr _YearXwkMgr;
        private C_Event _GameEvent;
        private Camera _GameCamera;
        // Use this for initialization
        void Start()
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "start_game");
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, "newyeargame", "game_time");

            AudioManager.Instance.PlayBgMusic("newyeargame/sound/game/public_xwkbgm_007.ogg",true);
            RewardUIManager.Instance.SetScoreVisible(false);
            if (_GameEvent != null)
            {
                _GameEvent.UnregisterEvent();
                _GameEvent = null;
            }
            _GameEvent = new C_Event();
            _GameEvent.RegisterEvent(C_EnumEventChannel.Global, "YMGameEvent", (object[] result) => {
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_MONSTER_LEVELDOWN)
                {
                    if (_YearMonsterMgr!=null)
                    {
                        _YearMonsterMgr.DoMonsterLevelDown((int)result[1]);
                    }
                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_MONSTER_WOUND_FSS)
                {
                    if (_YearMonsterMgr != null)
                    {
                        _YearMonsterMgr.EnterUnMatchedState(YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_FSS);
                    }
                    if (_YearXwkMgr!=null)
                    {
                        _YearXwkMgr.EnterUnMatchedState(YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_FSS);
                    }
                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_MONSTER_WOUND_SNOW)
                {
                    if (_YearMonsterMgr != null)
                    {
                        _YearMonsterMgr.EnterUnMatchedState(YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_SNOW);
                    }
                    if (_YearXwkMgr != null)
                    {
                        _YearXwkMgr.EnterUnMatchedState(YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_SNOW);
                    }
                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_MONSTER_WOUND_JGB)
                {
                    if (_YearMonsterMgr != null)
                    {
                        _YearMonsterMgr.EnterUnMatchedState(YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_JGB);
                    }
                    if (_YearXwkMgr != null)
                    {
                        _YearXwkMgr.EnterUnMatchedState(YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_JGB);
                    }
                } 
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_MONSTER_WOUND_MARROON)
                {
                    if (_YearMonsterMgr != null)
                    {
                        _YearMonsterMgr.EnterUnMatchedState(YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_MARROON);
                    }
                    if (_YearXwkMgr != null)
                    {
                        _YearXwkMgr.EnterUnMatchedState(YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_MARROON);
                    }
                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_GAME_SUCCESS)
                {
                    RewardUIManager.Instance.SetScoreVisible(false);

                    C_UIMgr.Instance.MandatoryCloseUIAll();

                    if (_YearMonsterMgr != null)
                    {
                        _YearMonsterMgr.EnterSuccessState();
                    }
                    if (_YearXwkMgr != null)
                    {
                        _YearXwkMgr.EnterSuccessState();
                    }
                    if (_NewYearCameraMgr != null)
                    {
                        _NewYearCameraMgr.EnterSuccessState();
                    }
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "success_times");

                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_GAME_SUCCESS_UI)
                {
                    RewardUIManager.Instance.SetScoreVisible(false);

                    C_UIMgr.Instance.CloseUI("UI_NewYearPage");
                    QuitGame();
                     C_UIMgr.Instance.OpenUI("UI_YearMonsterSuccess", null);
                     
                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_GAME_SUCCESS_0VER)
                {
                    AppInfoData.ResumehNewYearGameTime();

                    C_UIMgr.Instance.CloseUI("UI_YearMonsterSuccess");
                    QuitGame();
                    YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Main");
                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_GAME_FAIL)
                {
                    C_UIMgr.Instance.MandatoryCloseUIAll();

                    if (_YearMonsterMgr != null)
                    {
                        _YearMonsterMgr.EnterFailState();
                    }
                    if (_YearXwkMgr != null)
                    {
                        _YearXwkMgr.EnterFailState();
                    }
                    if (_NewYearCameraMgr != null)
                    {
                        _NewYearCameraMgr.EnterFailState();
                    }
                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_GAME_FAIL_QUIT)
                {
                    AppInfoData.ResumehNewYearGameTime();

                    QuitGame();
                    YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Main");
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "fail_times");

                }
                if ((YMGameEvet)result[0] == YMGameEvet.YMG_EVENT_GAME_START)
                {
                    RewardUIManager.Instance.SetScoreVisible(true);

                    //打开金币
                    RewardUIManager.Instance.ChangeModule(ModuleType.SpriteWindow, "");
                    AppInfoData.ResumehNewYearGameTime();
                    C_UIMgr.Instance.OpenUI("UI_NewYearPage", new System.Action(() => {
                        QuitGame();
                        YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Main");

                        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "newyeargame", "back_btn");

                    }));
                }
            });
            _GameCamera = transform.GetAddComponent<Camera>();
            _NewYearCameraMgr = new NewYearCameraMgr(_GameCamera);
            _YearMonsterMgr = new YearMonsterMgr(_GameCamera);
            _YearXwkMgr = new YearXwkMgr(_GameCamera);
//#if UNITY_EDITOR
//            _NewYearCameraMgr.EnterIdle();
//            _YearXwkMgr.EnterIdle();
//            _YearMonsterMgr.EnterIdle();
//#endif
        }
        void QuitGame()
        {
            // C_UIMgr.Instance.MandatoryCloseUIAll();
            AudioManager.Instance.StopBgMusic();
            C_UIMgr.Instance.CloseUI("UI_NewYearPage");
            _YearMonsterMgr.Stop();
            _YearXwkMgr.Stop();
            _NewYearCameraMgr.Stop();

            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, "newyeargame", "game_time");

        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_FAIL);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameEvent", YMGameEvet.YMG_EVENT_GAME_SUCCESS);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _NewYearCameraMgr.EnterIdle();
                _YearXwkMgr.EnterIdle();
                _YearMonsterMgr.EnterIdle();
            }

            if (_YearMonsterMgr!=null)
            {
                _YearMonsterMgr.OnUpdate();
            }
            if (_NewYearCameraMgr != null)
            {
                _NewYearCameraMgr.OnUpdate();
            }
            if (_YearXwkMgr != null)
            {
                _YearXwkMgr.OnUpdate();
            }
            
        }
    }
}


