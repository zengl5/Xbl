using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XWK.Common.UI_Reward;
/// <summary>
/// 1、获取宝箱，以及获取的精灵名字
/// </summary>
namespace YB.XWK.MainScene
{
    public class UI_ClockInMgr : C_BaseUI
    {
        private Transform _LeaveTime;
        private Sequence _StartSequence;
        private Image _BgImg;
        private Transform _Chest;
        private Transform RoleNode;
        private string saveFileName = "ui_mainclockin";
        private bool captureAction = false;
        private GameObject ChestLightEffect;
        private GameObject RoleLightEffect;
        private GameObject ChestAvaiableEffect;
        private GameObject ui_public_effect_jldsj_bx;//可领取宝箱的特效显示
        private GameObject ui_public_effect_jldsj_bxdk;//可领取宝箱的特效显示
        private GameObject ui_public_effect_jldsj_jljb;
        private Camera shotCamera;
        protected override void onOpenUI(params object[] uiObjParams)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Tips.Create("请先连接网络，再继续体验。");
                BackToMain();
                return;
            }
            if (!ChestData.isChestReceive())
            {
                BackToMain();
                return;
            }
            _LeaveTime = transform.Find("Canvas/MainLayer/chest");
            RoleNode = transform.Find("Canvas/MainLayer/role");
            captureAction = false;
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE);
            ChestData.FetchChestData(()=> {
                ShowRoleState();
                ShowShareUI();
                DoAction();
            });

            ChestLightEffect = transform.Find("Canvas/MainLayer/ui_public_effect_jldsj_jllq").gameObject;
            ChestLightEffect.transform.position = new Vector3(0,-117f,0);
            ChestLightEffect.gameObject.SetActive(false);
            RoleLightEffect = transform.Find("Canvas/MainLayer/ui_public_effect_jldsj_jsdl").gameObject;
            RoleLightEffect.gameObject.SetActive(false);
            ui_public_effect_jldsj_jljb = transform.Find("Canvas/ui_public_effect_jldsj_jljb").gameObject;
            ui_public_effect_jldsj_jljb.SetActive(false);

            RewardUIManager.Instance.ChangeCameraDepth(51);
        }
        protected override void onInit()
        {
            shotCamera = transform.Find("Camera").GetAddComponent<Camera>();
            shotCamera.clearFlags = CameraClearFlags.Depth;
            shotCamera.cullingMask = LayerMask.GetMask("TuJianLayerUI");
            shotCamera.orthographic = true;
            shotCamera.orthographicSize = 50f;
            shotCamera.depth = 51f;
            shotCamera.allowHDR = false;
            shotCamera.gameObject.SetActive(true);
             UICanvas.worldCamera = shotCamera;
        }
        void DoAction()
        {
            if (_StartSequence != null)
            {
                _StartSequence.Kill();
            }
            _StartSequence = DOTween.Sequence();
            _StartSequence.Append(m_MainLayer.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.17f))
               .Append(m_MainLayer.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.164f))
               .Append(m_MainLayer.DOScale(new Vector3(0.96f, 0.96f, 0.96f), 0.166f))
               .Append(m_MainLayer.DOScale(Vector3.one, 0.166f));
               

            _BgImg.DOKill();
            _BgImg = transform.Find("Canvas/shadow").GetComponent<Image>();
            _BgImg.DOFade(0, 0).OnComplete(() =>
            {
                _BgImg.DOFade(0.7f, 0.25f);
            });
        }
        void DoCloseAllUI()
        {
            if (_StartSequence != null)
            {
                _StartSequence.Kill(true);
            }
            _StartSequence = DOTween.Sequence();
            _StartSequence.Join(_BgImg.DOFade(0, 0.25f))
              .Join(m_MainLayer.DOScale(1.08f, 0.125f))
              .Append(m_MainLayer.DOScale(0f, 0.125f))
              .OnComplete(() =>
              {
                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME);
              });
        }
        void ShowRoleState()
        {
            ChestLightEffect.transform.SetParent(RoleNode.parent);
            //显示宝箱的图片以及状态
            _Chest = transform.Find("Canvas/MainLayer/chest");
            ChestAvaiableEffect = _Chest.transform.Find("ui_public_effect_jldsj_bxct").gameObject;
            ChestAvaiableEffect.gameObject.SetActive(false);
            ui_public_effect_jldsj_bx = _Chest.transform.Find("ui_public_effect_jldsj_bx").gameObject;
            ui_public_effect_jldsj_bx.gameObject.SetActive(false);
            if (ui_public_effect_jldsj_bxdk!=null)
            {
                ui_public_effect_jldsj_bxdk.gameObject.SetActive(false);
            }
            ui_public_effect_jldsj_bxdk = _Chest.transform.Find(ChestData.FetchChestEffect()).gameObject;
            ui_public_effect_jldsj_bxdk.gameObject.SetActive(false);

            GameObject hour = transform.Find("Canvas/MainLayer/chest/hour").gameObject;
            GameObject minute = transform.Find("Canvas/MainLayer/chest/minute").gameObject;
            GameObject time = transform.Find("Canvas/MainLayer/chest/time").gameObject;
            if (ChestData.isChestLock())
            {
                time.gameObject.SetActive(true);
                minute.gameObject.SetActive(true);
                hour.gameObject.SetActive(true);

                hour.GetComponent<Text>().text = ChestData.FetchLeaveTime().Hours.ToString().PadLeft(2, '0');
                minute.GetComponent<Text>().text = ChestData.FetchLeaveTime().Minutes.ToString().PadLeft(2, '0');
                _Chest.GetComponent<Image>().sprite = GameResMgr.Instance.LoadResource<Sprite>("c_framework/ui/package_ui_sprite/main_clockin/" + ChestData.FetchChestType(), false);

            }
            else if(ChestData.isChestReceive())
            {
                //隐藏剩余时间
                time.gameObject.SetActive(false);
                minute.gameObject.SetActive(false);
                hour.gameObject.SetActive(false);
                _Chest.GetComponent<Image>().sprite = GameResMgr.Instance.LoadResource<Sprite>("c_framework/ui/package_ui_sprite/main_clockin/" + ChestData.FetchChestOpenUI(), false);
            }
            else
            {

                //隐藏剩余时间
                time.gameObject.SetActive(false);
                minute.gameObject.SetActive(false);
                hour.gameObject.SetActive(false);
                _Chest.GetComponent<Image>().sprite = GameResMgr.Instance.LoadResource<Sprite>("c_framework/ui/package_ui_sprite/main_clockin/" + ChestData.FetchChestType(), false);
                ChestAvaiableEffect.gameObject.SetActive(true);
              //  WizardData.AddWizardItem(data); 
            }
            _Chest.gameObject.SetActive(true);
            UpdateRoleUI();
        }
        void UpdateRoleUI()
        {
            //根据所有获得的精灵状态设置当前显示精灵的状态
            int length = RoleNode.childCount;
            for (int i = 0; i < length; i++)
            {
                Transform role = RoleNode.GetChild(i);
                Image img = role.GetComponent<Image>();
                if (role == null || img==null)
                {
                    continue;
                }
                if (WizardData.IsWizardCollected(role.name))
                {
                    img.color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    img.color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
                }
            }
        }
        void ShowShareUI()
        {
            //根据当前是否有微信进行不同图标显示
            if (GameHelper.Instance.IsWxAvailable())
            {
                transform.Find("Canvas/MainLayer/wchy").gameObject.SetActive(true);
                transform.Find("Canvas/MainLayer/wc").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("Canvas/MainLayer/wchy").gameObject.SetActive(false);
                transform.Find("Canvas/MainLayer/wc").gameObject.SetActive(false);
            }
        }
        public void BackToMain()
        {
            if (ui_public_effect_jldsj_bxdk != null)
            {
                ui_public_effect_jldsj_bxdk.gameObject.SetActive(false);
            }
            RewardUIManager.Instance.RestoreCameraDepth();
            CancelInvoke("ShowNextChest");
            AudioManager.Instance.StopPlayerSound();
            StopAllCoroutines();
            DoCloseAllUI();
        }
        public void CaptureScreenShotToWeChat()
        {
            //if (captureAction)
            //{
            //    return;
            //}
            //captureAction = true;
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "UI_ClockIn_pyq_bc");

            StartCoroutine(Utility.SaveScreenShot(shotCamera, new Rect(0, 0, Screen.width, Screen.height), saveFileName, 3, true));

        }
        public void CaptureScreenShotToWeChatMoments()
        {
            //if (captureAction)
            //{
            //    return;
            //}
            //captureAction = true;
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "UI_ClockIn_wx_bc");

            StartCoroutine(Utility.SaveScreenShot(shotCamera, new Rect(0, 0, Screen.width, Screen.height), saveFileName, 2, true));

        }
        public void CaptureScreenShotToDCIM(int type)
        {
            //if (captureAction)
            //{
            //    return;
            //}
            //captureAction = true;
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "UI_ClockIn_bd_bc");

            StartCoroutine(Utility.SaveScreenShot(shotCamera, new Rect(0, 0, Screen.width, Screen.height), saveFileName, 1,true));
        }

        protected override void onUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(UICanvas.worldCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit!=null&&hit.collider != null)
                {
                    if (hit.collider.transform.parent != null)
                    {
                        TouchRole(hit.collider.gameObject.name);
                    }
                }
            }
        }
        public void TouchChest()
        {
            if (ChestData.isChestLock())
            {

                if (Random.Range(1,2)==1)
                {
                    AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_71.ogg");
                }
                else
                {
                    AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_70.ogg");
                }
                return;
            }
            if ( ChestData.isChestReceive())
            {
                C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "UI_ClockIn_djs");

                return;
            }
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "UI_ClockIn_bx_dk");

            //飞特效到精灵，结束的时候，切换到下一个宝箱
            ChestData.ReceiveChest((type,data)=> {
               if (type != 0)
               {
                   if (_Chest!=null)
                   {
                       _Chest.GetComponent<Image>().sprite = GameResMgr.Instance.LoadResource<Sprite>("c_framework/ui/package_ui_sprite/main_clockin/" + ChestData.FetchChestOpenUI(), false);
                        ui_public_effect_jldsj_bxdk.gameObject.SetActive(true);
                   }
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "UI_ClockIn_reward",data);

                }
                if (type == 1)
               {
                    AudioManager.Instance.PlayEffectAutoClose("main/sound/recommendspirit/public_xwkyx_105.ogg");
                    AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_72.ogg");
                    RewardUIManager.GetInstance().UpdateScore(int.Parse(data), ModuleType.HomePage);
                    ui_public_effect_jldsj_jljb.SetActive(true);
                    ui_public_effect_jldsj_jljb.transform.position = _Chest.position;
                    Invoke("ShowNextChest", 2f);
               }
              else if (type == 2)
                {
                    AudioManager.Instance.PlayEffectAutoClose("main/sound/recommendspirit/public_xwkyx_106.ogg");
                    AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_62.ogg");
                    ChestLightEffect.gameObject.SetActive(false);
                    ChestLightEffect.transform.position = _Chest.position;
                    RoleLightEffect.gameObject.SetActive(false);
                    ChestLightEffect.transform.SetParent(RoleNode);
                    Transform role = RoleNode.transform.Find(data);
                    if (role == null)
                    {
                        C_DebugHelper.LogError("role find spirit is null:"+ data);
                        return;
                    }
                    WizardData.AddWizardItem(data);
                    WizardData.CurrentLocationRecommend(data);
                    Vector3 destion = role.transform.position;
                    ChestLightEffect.gameObject.SetActive(true);

                    ChestLightEffect.transform.DOMove(ChestLightEffect.transform.position, 0.5f).OnComplete(()=> {
                        ChestLightEffect.transform.DOMove(destion, 0.5f).OnComplete(() =>
                        {
                            ChestLightEffect.transform.SetParent(RoleNode.parent);
                            ChestLightEffect.gameObject.SetActive(false);
                            //点亮角色身上的特效
                            RoleLightEffect.gameObject.SetActive(true);
                            RoleLightEffect.transform.SetParent(RoleNode);
                            RoleLightEffect.transform.position = new Vector3(destion.x, destion.y - 10f, destion.z);
                            RoleLightEffect.transform.SetParent(RoleNode.parent);
                            UpdateRoleUI();
                            ShowNextChest();
                        });
                    });
                }
            });
        }
      
        private void ShowNextChest()
        {
            ChestData.FetchChestData(() => {
                AudioManager.Instance.PlayEffectAutoClose("main/sound/recommendspirit/public_xwkyx_107.ogg");

                ShowRoleState();
                ui_public_effect_jldsj_bx.gameObject.SetActive(true);
            });
        }
        private void TouchRole(string name)
        {
            if (WizardData.IsWizardCollected(name))
            {
                return;
            }
            //后续配置到表格
            switch (name)
            {
                case "jl_lingwa":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_56");
                    }
                    break;
                case "jl_xcww":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_57");
                    }
                    break;
                case "jl_hxjl":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_61");
                    }
                    break;
                case "jl_xixilang":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_58");
                    }
                    break;
                case "jl_huohuolong":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_55");
                    }
                    break;
                case "jl_menmenmiao":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_53");
                    }
                    break;
                case "jl_niuxiaoxian":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_60");
                    }
                    break;
                case "jl_xiuxiuwoniu":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_59");
                    }
                    break;
                case "jl_jisuanji":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_54");
                    }
                    break;
                case "jl_dazuishou":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_52");
                    }
                    break;
                case "Not_Recommend_jl_00014":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_34");
                    }
                    break;
                case "Not_Recommend_jl_00015":
                    {
                        AudioManager.Instance.PlayerSound("main/sound/recommendspirit/xwk_jlej_38");
                    }
                    break;
            }
        }
    }
     
}
