using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XBL.Core;
using XWK.Common.RedBomb;
using XWK.Common.UI_Reward;


namespace YB.XWK.MainScene
{
    public enum GameEventEnum
    {
        GAME_EVENT_ENUM_RESUME_GAME = 1,
        GAME_EVENT_ENUM_ENTER_WIZARD = 2,
        GAME_EVENT_ENUM_PAUSE_GAME = 3,
        GAME_EVENT_ENUM_PAUSE_GAME_FALSE = 4,
        GAME_EVENT_ENUM_CLOSE_PRIVACY_POLICY = 5,
        GAME_EVENT_ENUM_RESUME_GAME_RESUME_FALSE = 6,
        GAME_EVENT_ENUM_UNREGISTEREVENT = 7,
        GAME_EVENT_ENUM_ENTER_GAME_BYJL = 8,
        GAME_EVENT_ENUM_ENTER_GAME_BBHL = 9,
        GAME_EVENT_ENUM_ENTER_GAME_JGB = 10,
        GAME_EVENT_ENUM_ENTER_GAME_FSS = 11,
        GAME_EVENT_ENUM_ENTER_COLLECT_SPIRIT = 13,
        GAME_EVENT_ENUM_ENTER_FREEZE_ROLE = 14,
        GAME_EVENT_ENUM_ENTER_RELEASE_ROLE=15,
        GAME_EVENT_ENUM_ENTER_HIDE_HAND=16,
        GAME_EVENT_ENUM_ENTER_NEWYEAR_GAME=17,
        GAME_EVENT_ENUM_ENTER_RB_GAME = 18,
        GAME_EVENT_ENUM_ENTER_NEWYEAR_PAGE_BACK = 19,

    }
    public  enum ActorStateEnum {
        ACTOR_STATE_ENUM_NULL,
        ACTOR_STATE_ENUM_HELLO,
        ACTOR_STATE_ENUM_GC,
        ACTOR_STATE_ENUM_SP,
        ACTOR_STATE_ENUM_IDLE,
        ACTOR_STATE_ENUM_MOVECAMERA,
        ACTOR_STATE_ENUM_FLASH,
        ACTOR_STATE_ENUM_SHOW,
        ACTOR_STATE_ENUM_WALKAROUND,
        ACTOR_STATE_ENUM_AI_TALK,
        ACTOR_STATE_ENUM_HELLO_OVER,
        ACTOR_STATE_ENUM_SPRING_AUDIO,
    }

    public interface IActor
    {
        Transform getActor();

        Animator getAnimator();

        void EnterNextState(ActorStateEnum actorStateEnum);

        void Stop();

        Camera m_Camera { set; get; }

        ActorStateInfo getInfo(string stateName);

        List<QuestionConfigData> getActorAnimancerInfo();

        ActorAnimancerResConfig getActorResConfig(string stateName);

    }
    public class ActorMgr : MonoBehaviour, IActor
    {
        private Animator _ActorAnimator;
        private ActorStateBase _ActorState;
        private ActorAnimancerConfig _ActorConfig;
        private ActorAiData _ActorAiData;
        private Camera _ActorCamera;
        private string _ActorTag = "Actor"; 
        private string _ActorTailTag = "XWK_Tail";
        private string _ActorCameraTag = "ActorCamera";
        private GameObject _ActorXwk;
        private Transform _ActorXwkTramsform;
        private C_Event _GameEvent = new C_Event();
        private bool _Pause = true;
        private CloudMgr _CloudMgr;
        private RedpacketMgr _RedpacketMgr;
        private MaroonMgr _MaroonMgr;
        private GameObject _WoodMaroon;
        private enum GameState
        {
            gamestate_null,
            gamestate_start,
            gamestate_idle,
        }
        private GameState _GameState;
        public Camera m_Camera
        {
            get
            {
                if (_ActorCamera== null)
                {
                    InitCamera();
                }
                return _ActorCamera;
            }

            set
            {
                _ActorCamera = value;
            }
        }
        public Transform getActor()
        {
            if (_ActorXwk == null || _ActorXwkTramsform==null)
            {
                _ActorXwk = GameObject.FindGameObjectWithTag("Actor");
                if (_ActorXwk == null)
                {
                    InitXwk();
                }
                _ActorXwkTramsform = _ActorXwk.transform;
            }
            return _ActorXwkTramsform;
        }
        void Start()
        {
            InitSpirit();
            LoadActor(); 
        }
 
        public void InitSpirit()
        {

        }
        private void InitCamera()
        {
            if(_ActorCamera==null)
                 _ActorCamera = GameObject.FindGameObjectWithTag(_ActorCameraTag).GetComponent<Camera>();
        }
       
        // Use this for initialization
        public void LoadActor()
        {
            InitCamera();
            LocalData.m_SpiritGameMode = false; 

            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeStart, LocalData.m_main_time, LocalData.m_main_time);

            AudioManager.Instance.PlayBgMusic("public/sound_effect/public_xwkbgm_001.ogg",true);
            _Pause = false;
            InitEvent();
            InitXwk();

            GameObject tail = Utility.FindChild(_ActorXwkTramsform, "Bone001 Tail06").gameObject;
            BoxCollider tailBX = tail.AddComponent<BoxCollider>();
            tailBX.size = new Vector3(31.6f, 13.45f, 23.2f);
            tailBX.center = new Vector3(-24.79f,0.6f,0f);
            tail.tag = _ActorTailTag;

            CapsuleCollider capsuleCollider = _ActorXwk.GetAddComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0f, 72f, 0f);
            capsuleCollider.radius = 32.2f;
            capsuleCollider.height = 144f;
            capsuleCollider.direction = 1;
            capsuleCollider.isTrigger = true;

            Rigidbody rigidbody =  _ActorXwk.GetAddComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            rigidbody.mass = 100f;

            _ActorAnimator = _ActorXwk.GetComponent<Animator>();
            LoadConfig();

            WindowSliderControl.Instance.InitCharacter(this, _ActorCamera);

            _Pause = false;

            InitCloud();
                
            if (LocalData.m_FirstEnterApp)
            {
                LocalData.m_BackToMain = true;

                FirstEnterMain();
                LocalData.m_FirstEnterApp = false;
            }
            else
            {
                //打开二级界面
                if (!LocalData.m_BackToMain)
                {
                    EnterWizard();
                }
                else
                {
                    StartActor();
                }
            }
            LocalData.m_BackToMain = false;
        }
        private void InitXwk()
        {
            if (_ActorXwk == null)
            {
                _ActorXwk = GameResMgr.Instance.LoadResource<GameObject>("public/mesh/wukong/public_model_wukong#mesh.prefab", true);
                _ActorXwk.SetActive(true);
                _ActorXwkTramsform = _ActorXwk.transform;
                _ActorXwkTramsform.localPosition = new Vector3(0f, 0f, 0f);
                _ActorXwkTramsform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                _ActorXwkTramsform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
            _ActorXwkTramsform = _ActorXwk.transform;
            _ActorXwk.tag = _ActorTag;
        }
        //后续改成只有一个事件对象
        private void InitEvent()
        { 
            _GameEvent.RegisterEvent(C_EnumEventChannel.Global, "MainGameEvent", (object[] result) => {
                if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME )
                {
                  //  RedBombManager.Instance.StartPlay();
                    ResumeMainState(true);
                }
                else  if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_WIZARD)
                {
                    EnterWizard();
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME)
                {
                    PauseMainState();
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE)
                {
                    PauseMainState(false);
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_CLOSE_PRIVACY_POLICY)
                {
                    ClosePrivacyPolicy((string)result[1]);
                }
                else if((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME_RESUME_FALSE)//6
                {
                    ResumeMainState(false);
                }
                else if((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_UNREGISTEREVENT)//7
                {
                    _GameEvent.UnregisterEvent();
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_GAME_BYJL)//8
                {
                    EnterGame(0);
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_GAME_BBHL)//.9
                {
                    EnterGame(1);
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_GAME_JGB)//10
                {
                    EnterGame(2);
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_GAME_FSS)//11
                {
                    EnterGame(3);
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_FREEZE_ROLE)//14
                {
                    EnterRoleFreeze();
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_RELEASE_ROLE)//15
                {
                    EnterIdleState();
                }
                else if ((GameEventEnum)result[0] == GameEventEnum.GAME_EVENT_ENUM_ENTER_NEWYEAR_GAME)//15
                {
                    EnterGame(4);
                }
            });
        }
        private void ResumeMainState(bool closeAllUi)
        {
            //m_Camera.gameObject.SetActive(true);
            m_Camera.enabled = true;
            getActor().gameObject.SetActive(true);
            if (_CloudMgr != null)
            {
                _CloudMgr.getActor().gameObject.SetActive(true);
            }
            if (_RedpacketMgr!=null)
            {
                _RedpacketMgr.Resume();
            }
            if (_MaroonMgr != null)
            {
                _MaroonMgr.Resume();
            }
            //恢复进入待机状态
            _Pause = false;
            EnterIdleState();
            if (_CloudMgr != null)
                _CloudMgr.EnterIdleState();
            WindowSliderControl.Instance.ReleaseCamera();

            if (closeAllUi)
            {
                C_MonoSingleton<C_UIMgr>.GetInstance().MandatoryCloseUIAll();
            }
            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_MainCityUp");
        }
        private void PauseMainState(bool hideCamera = true)
        {
            _Pause = true;
            Exit();
            WindowSliderControl.Instance.DFrozenCamera();
            if (hideCamera)
            {
                m_Camera.enabled = false;
             //   m_Camera.gameObject.SetActive(false);
            }
            getActor().gameObject.SetActive(false);
            if(_CloudMgr!=null)
            {
                _CloudMgr.getActor().gameObject.SetActive(false);
            }
            if (_RedpacketMgr != null)
            {
                _RedpacketMgr.Pause();
            }
            if (_MaroonMgr != null)
            {
                _MaroonMgr.Pause();
            }
            
        }
        private void ClosePrivacyPolicy(string result)
        {
            string main_playerprefs = "playerprefs_main_policy";


            PlayerPrefs.SetString(main_playerprefs, main_playerprefs);
            PlayerPrefs.Save();
            EnterIdleState();
            UpdateCloudPos();
            if (_CloudMgr != null)
                _CloudMgr.EnterIdleState();
            if (_RedpacketMgr != null)
                _RedpacketMgr.EnterIdle();
            if (_MaroonMgr != null)
                _MaroonMgr.EnterIdle();
            if ("1".Equals(result))
            {
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "yinsixieyi", "yes");
            }
            else
            {
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "yinsixieyi", "no");
            }

            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_MainCityUp");

        }
        private void FirstEnterMain()
        {
            GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "first_enter_main");

            //开启主界面隐私协议
            string main_playerprefs = "playerprefs_main_policy";
            string mainPolicy = PlayerPrefs.GetString(main_playerprefs, string.Empty);
#if UNITY_EDITOR
            mainPolicy = "";
#endif
            if (string.IsNullOrEmpty(mainPolicy) && GameConfig.AutoTest==0)
           {
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, "yinsixieyi", "start");

                _CloudMgr.gameObject.SetActive(false);

                EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_HELLO_OVER);
                GameHelper.Instance.OpenvPrivacyPolicy();

                //出现展示ui逻辑
                C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", 1);
            }
            else
            {
                EnterSayHelloState(); 
            }
            LocalData.m_FirstEnterMain = false;
        }
        private void StartActor()
        {
            EnterIdleState();
            _CloudMgr.EnterIdleState();
            _RedpacketMgr.EnterIdle();
            if (_MaroonMgr != null)
                _MaroonMgr.EnterIdle();
            C_MonoSingleton<C_UIMgr>.GetInstance().OpenUI("UI_MainCityUp");
        }
        private void InitCloud()
        {
            if (_CloudMgr == null)
            {
                GameObject cloud = GameResMgr.Instance.LoadResource<GameObject>("public/mesh/jdy02/public_model_jdy02#mesh.prefab", true);
                cloud.SetActive(true);
                cloud.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                cloud.transform.tag = "MainCloud";
                BoxCollider collider =  cloud.transform.Find("Point002/Point001").GetAddComponent<BoxCollider>();
                collider.size = new Vector3(100f,100f,100f);
                collider.isTrigger = true;
                _CloudMgr = cloud.GetAddComponent<CloudMgr>();
                if (!LocalData.m_FirstEnterMain)
                {
                    UpdateCloudPos();
                }
            }
            _CloudMgr.OnInit(_ActorCamera, this);

            if (_RedpacketMgr == null)
            {
                _RedpacketMgr = new RedpacketMgr(_ActorCamera,this);
            }
            if (_MaroonMgr ==null)
            {
                _MaroonMgr = new MaroonMgr(_ActorCamera);
            }
            if (_WoodMaroon == null)
            {
                _WoodMaroon = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_wk_s_16.prefab", true);
                _WoodMaroon.transform.position = Vector3.zero;
            }
        }
        private void UpdateCloudPos()
        {
            if(_CloudMgr == null)
            {
                return;
            }
            _CloudMgr.gameObject.SetActive(true);
            _CloudMgr.transform.localPosition = new Vector3(-1f, 2.30f, -2f);
            _CloudMgr.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        /// <summary>
        /// 加载悟空状态的配置文本
        /// </summary>
        private void LoadConfig()
        {
            _ActorConfig = new ActorAnimancerConfig();
            _ActorConfig.Load();
            _ActorAiData = new ActorAiData();
            _ActorAiData.Load();
        }
        // Update is called once per frame
        void Update()
        { 
            if (Time.frameCount % 30 == 0)
            {
                System.GC.Collect();
            }
            if (_Pause)
            {
                if (_ActorState!=null)
                {
                    _ActorState.Stop();
                    _ActorState = null;
                }
                return;
            }
            //规避角色下陷
            if (_ActorXwkTramsform != null && _ActorXwkTramsform.position.y < -0.01f)
            {
                _ActorXwkTramsform.position = new Vector3(_ActorXwkTramsform.position.x, 0, _ActorXwkTramsform.position.z);
            }
            if (_ActorState!=null )
            {
                _ActorState.OnUpdate();
            }
            if (_CloudMgr!=null)
            {
                _CloudMgr.OnUpdate();
            }
            if (_RedpacketMgr != null)
            {
                _RedpacketMgr.OnUpdate();
            }
            if (_MaroonMgr != null)
            {
                _MaroonMgr.OnUpdate();
            }
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                      
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (_Pause) return;
                        if (WindowSliderControl.Instance.OutView(_ActorXwk.transform))
                        {
                            MoveCameraState();
                        }
                        if (_ActorState != null && !_ActorState.HasInteractiveState)
                        {
                            return;
                        }
                        TouchHandle(touch.position);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (_Pause) return;

                    if (WindowSliderControl.Instance.OutView(_ActorXwk.transform))
                    {
                        MoveCameraState();
                    }
                    if (_ActorState != null && !_ActorState.HasInteractiveState)
                    {
                        return;
                    }
                    TouchHandle(Input.mousePosition);
                }
                
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    EnterAiTalk();
                    LocalData.aiQuesid++;
                    if (LocalData.aiQuesid > 50)
                    {
                        LocalData.aiQuesid = 0;
                    }
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    EnterAiTalk();
                    LocalData.aiQuesid--;
                    if (LocalData.aiQuesid <= 0)
                    {
                        LocalData.aiQuesid = 0;
                    }
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ChestData.FetchChestData();
                }
                //if (Input.GetKeyDown(KeyCode.D))
                //{
                //    ChestData.SynchroData();
                //}
                //if (Input.GetKeyDown(KeyCode.S))
                //{
                //    ChestData.ReceiveChest();
                //}
            } 
        }
        void TouchHandle(Vector2 startTouchpos)
        {
            if (_ActorCamera==null)
            {
                return;
            }
            RaycastHit hit;
            Ray ray;
            ray = _ActorCamera.ScreenPointToRay(startTouchpos);
            if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
            {
                GameObject obj = hit.collider.gameObject;
                if (obj != null)
                {
                    string tag = obj.tag;
                    if (tag.Equals(_ActorTag))//点击身体
                    {
                        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_click_xwk);
                        EnterTouchBodyState();
                    }
                    else if (tag.Equals(_ActorTailTag))
                    {
                        EnterTouchTailState();
                    }
                }
            }
        }
        //动画控制器配置文件使用。
        public ActorStateInfo getInfo(string stateName)
        {
            //if (_ActorConfig == null)
            //{
            //    LoadConfig();
            //}
            //return  _ActorConfig.FecthData(stateName);
            return null;
        }

        public Animator getAnimator()
        {
            return _ActorAnimator ?? _ActorXwk.GetComponent<Animator>();
        }
        private void ClearState()
        {
            if (_CloudMgr != null)
            {
                _CloudMgr.SetCoudActive(true);
            }
            if (_ActorState != null)
            {
                _ActorState.Stop();
                _ActorState = null;
            }
        }
        public void EnterRoleFreeze()
        {
            ClearState();
            if (_CloudMgr != null)
            {
                _CloudMgr.SetCoudActive(false);
            }
            _ActorState = new ActorFreezeState(this);
        }
        public void EnterAiTalk()
        {
            ClearState();
            if (_CloudMgr != null)
            {
                _CloudMgr.SetCoudActive(false);
            }
            _ActorState = new ActorAiTalkState(this);
        }
        public void EnterIdleState()
        {
            ClearState();
            _ActorState = new IdleActorState(this);
        }
        public void EnterTouchBodyState()
        {
            int reslut = UnityEngine.Random.Range(0, 100);
            if (reslut <= 25)
            {
                EnterAiTalk();
            }
            else if (reslut > 25 && reslut <= 45)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    EnterSpState();
                }
                else
                {
                    EnterGcState();
                }
            }
            else
            {
                ClearState();
                _ActorState = new TouchBodyState(this);
            }
        }
        public void EnterTouchHeadState()
        {
            if (_ActorState==null ||(_ActorState!=null && _ActorState.GetType() != typeof(TouchHeadState)))
            {
                ClearState();
                _ActorState = new TouchHeadState(this);
            }
        }
        public void EnterTouchTailState()
        {
            if (_ActorState==null ||(_ActorState!=null && _ActorState.GetType() != typeof(TouchTailState)))
            {
                ClearState();
                _ActorState = new TouchTailState(this);
            }
        }

        public void EnterSayHelloState()
        {
            if (_ActorState==null ||(_ActorState!=null && _ActorState.GetType() != typeof(SayActorHelloState)))
            {
                //创建角色
                ClearState();
                 _ActorState = new SayActorHelloState(this);
                  _CloudMgr.EnterSayHelloState();
                WindowSliderControl.Instance.DFrozenCamera();
            }
        }
        public void MoveCameraState()
        {
            if (_ActorState==null ||(_ActorState!=null && _ActorState.GetType() != typeof(MoveCameraState)))
            {
                ClearState();
            }
            _ActorState = new MoveCameraState(this);
        }

        public void EnterGcState()
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_fenshenshu, "main_show");
            ClearState();
            _ActorState = new GoldenCudgelState(this);
        }

        public void EnterSpState()
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_game_jingubang, "main_show");

            ClearState();
             _ActorState = new SeparationState(this);
        }

        public void EnterWalkAoundState()
        {
            if (_ActorState == null || (_ActorState != null && _ActorState.GetType() != typeof(WalkAroundState)))
            {
                ClearState();
                _ActorState = new WalkAroundState(this);
            }
        }
        public void EnterAskState()
        {
            ClearState();
            _ActorState = new AskActorState(this);
        }
        void Exit()
        {
            ClearState();

            AudioManager.Instance.StopAllEffect();
            AudioManager.Instance.StopPlayerSound();
        }
        void IActor.Stop()
        {
            QuitActorManager();
        }
        public void QuitActorManager()
        {
            if (_CloudMgr!=null)
            {
                _CloudMgr.Stop();
            }
            if (_RedpacketMgr!=null)
            {
                _RedpacketMgr.Stop();
            }
            if (_MaroonMgr != null)
            {
                _MaroonMgr.Stop();
            }
            if (_WoodMaroon != null)
            {
                GameObject.DestroyObject(_WoodMaroon.gameObject);
                _WoodMaroon = null;
            }
            _Pause = true;

            ClearState();
            //不滑动镜头
            WindowSliderControl.Instance.DFrozenCamera();
            AudioManager.Instance.StopBgMusic();
            C_MonoSingleton<C_UIMgr>.GetInstance().DestoryAllUI();
            
            _GameEvent.UnregisterEvent();

            AudioManager.Instance.StopAllEffect();
            AudioManager.Instance.StopPlayerSound();

            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.TimeEnd, LocalData.m_main_time, LocalData.m_main_time);
             
            Resources.UnloadUnusedAssets();

        }
        public void DestoryCamera()
        {
            if (m_Camera != null)
            {
                GameObject.DestroyObject(m_Camera.gameObject);
                m_Camera = null;
            }
        }
        public void DestoryAuido()
        {
           // GameObject audio = AudioManager.Instance.gameObject;
            GameObject.DestroyObject(AudioManager.Instance);
           // audio = null;
        }
        public void EnterWizard()
        {
            if (_Pause) return;

            //进入暂停状态
            PauseMainState();
            if (_CloudMgr != null)
            {
                _CloudMgr.SetCoudActive(false);
            }
            C_MonoSingleton<C_UIMgr>.GetInstance().DestoryAllUI();
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_main_enter_wizard);

            C_UIMgr.Instance.OpenUI("UI_SpriteWindow");
        }
        public void EnterGame(int type)
        {
            //if (_Pause) return;
            //_Pause = true;
            LocalData.m_SpiritGameMode = true;
            LocalData.m_BackToMain = true;
            QuitActorManager();

            if (_ActorXwk != null)
                GameObject.Destroy(_ActorXwk.gameObject);

            switch (type)
            {
                case 0: EnterByjlGame(); break;
                case 1: EnterBbhlGame(); break;
                case 2: EnterJgbGame(); break;
                case 3: EnterSpGame(); break;
                case 4: EnterNYGame(); break;
            }
            DestoryAuido();
            DestoryCamera();
        }
        public void EnterNYGame()
        {
          //  YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Main", "Spirit", () => { Utility.SetMainScene("Spirit"); });
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "NewYearGame");
        }
        public void EnterByjlGame()
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_fashu_game_byjl");

            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_byjl);
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Main", "Spirit", () => { Utility.SetMainScene("Spirit"); });
        }
        public void EnterBbhlGame()
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_fashu_game_bbhl");

            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_bbhl);

            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Main", "BaibianHulu", () => { Utility.SetMainScene("BaibianHulu"); });
        }
        public void EnterSpGame()
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_fashu_game_fenshenshu");

            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_fss);

            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Main", "Separation", () => { Utility.SetMainScene("Separation"); });
        }
        public void EnterJgbGame()
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_fashu_game_jgb");

            DailyBounsData.SetDailBounsDataState(DailyBounsName.DailyBouns_Game_Ggb);
            YBSceneLoadingMgr.Instance.UnloadSceneAndLoadNewSceneAsync("Main", "Goldhoopbar", () => { Utility.SetMainScene("Goldhoopbar"); });
        }
        public void EnterWizard_LoadScene()
        {
            if (_Pause) return;
            

            QuitActorManager();

            if (_ActorXwk != null)
                GameObject.Destroy(_ActorXwk.gameObject);

            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "game_wizard_main_show_ui");
            DestoryAuido();
            DestoryCamera();
            RewardUIManager.GetInstance().ClearRewardUI();
            YBSceneLoadingMgr.Instance.LoadScene("SpriteWindow");

           // C_UIMgr.Instance.OpenUI("UI_SpriteWindow");

        }
        public void AutoMoveCamera()
        {
            if (_ActorState == null || (_ActorState != null && _ActorState.GetType() != typeof(AutoMoveCameraState)))
            {
                ClearState();
                _ActorState = new AutoMoveCameraState(this);
            }
        }
        public void AutoMoveCameraBBHL()
        {
            if (_ActorState == null || (_ActorState != null && _ActorState.GetType() != typeof(AutoMoveCameraStateBBHL)))
            {
                ClearState();
                _ActorState = new AutoMoveCameraStateBBHL(this);
            }
        }

        public void EnterFlashState()
        {
            if (_ActorState == null || (_ActorState != null && _ActorState.GetType() != typeof(ActorFlashState)))
            {
                ClearState();
                _ActorState = new ActorFlashState(this);
            }
        }

        public void EnterShowState()
        {
            ClearState();
            _ActorState = new ActorShowState(this);
        }

        public List<QuestionConfigData> getActorAnimancerInfo()
        {
            if (_ActorAiData==null)
            {
                _ActorAiData = new ActorAiData();
                _ActorAiData.Load();
            }
            return _ActorAiData.FetchQuestionData();
        }

        public ActorAnimancerResConfig getActorResConfig(string stateName)
        {
            if (_ActorConfig==null)
            {
                _ActorConfig = new ActorAnimancerConfig();
            }
            return _ActorConfig.FecthData(stateName);
        }

        public void EnterNextState(ActorStateEnum actorStateEnum)
        {
            if (_Pause)
            {
                return;
            }
            switch (actorStateEnum) {
                case ActorStateEnum.ACTOR_STATE_ENUM_NULL: {
                    } break;
                case ActorStateEnum.ACTOR_STATE_ENUM_HELLO: {
                        EnterSayHelloState();
                    } break;
                case ActorStateEnum.ACTOR_STATE_ENUM_HELLO_OVER:
                    {
                        if (_CloudMgr != null)
                        {
                            _CloudMgr.gameObject.SetActive(true);
                            _CloudMgr.SetCoudActive(true);
                            _CloudMgr.EnterIdleState();
                        }
                        UpdateCloudPos();
                        EnterIdleState();
                        if (_RedpacketMgr!=null)
                        {
                            _RedpacketMgr.EnterIdle();
                        }
                        if (_MaroonMgr != null)
                            _MaroonMgr.EnterIdle();
                    }
                    break;
                case ActorStateEnum.ACTOR_STATE_ENUM_GC: {
                        EnterGcState();
                    } break;
                case ActorStateEnum.ACTOR_STATE_ENUM_SP: {
                        EnterSpState();
                    } break;
                case ActorStateEnum.ACTOR_STATE_ENUM_IDLE: {
                        EnterIdleState();
                    } break;
                case ActorStateEnum.ACTOR_STATE_ENUM_MOVECAMERA: {
                        MoveCameraState();
                    } break;
                case ActorStateEnum.ACTOR_STATE_ENUM_SHOW: {
                        EnterShowState();
                    } break;
                case ActorStateEnum.ACTOR_STATE_ENUM_WALKAROUND: {
                        EnterWalkAoundState();
                    } break;
                case ActorStateEnum.ACTOR_STATE_ENUM_FLASH:
                    {
                        EnterFlashState();
                    }
                    break;
                case ActorStateEnum.ACTOR_STATE_ENUM_AI_TALK:
                    {
                        EnterAiTalk();
                    }
                    break;
                case ActorStateEnum.ACTOR_STATE_ENUM_SPRING_AUDIO:
                    {
                        EnterAiTalk();
                    }
                    break;
                default:break;
            }
            
        }
    }
}

