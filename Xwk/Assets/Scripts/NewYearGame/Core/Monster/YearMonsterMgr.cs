using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XWK.Common.UI_Reward;

namespace YB.YM.Game {
    public enum YMGameEvent
    {
        YMG_EVENT_IDLESTATE = 0,
        YMG_EVENT_CLICKSTATE = 1,
        YMG_EVENT_ATTACKSTATE=2,
        YMG_EVENT_HENSHINSTATE=3,
        YMG_EVENT_UNMATCHEDSTATE= 4,
        YMG_EVENT_HELLOSTATE = 5,
    }
    public enum YMGameMonsterLevel {
        YMG_MONSTER_LEVEL_1= 0,
        YMG_MONSTER_LEVEL_2= 1,
        YMG_MONSTER_LEVEL_3= 2,
        YMG_MONSTER_LEVEL_4 =3,
        YMG_MONSTER_LEVEL_5 =4,

    }
    public enum YMGameWoundedType {

        YMG_MONSTER_WOUNDED_TYPE_SIXCLICK,
        YMG_MONSTER_WOUNDED_TYPE_FSS,//分身术
        YMG_MONSTER_WOUNDED_TYPE_SNOW,//分身术
        YMG_MONSTER_WOUNDED_TYPE_JGB,//分身术
        YMG_MONSTER_WOUNDED_TYPE_MARROON,//分身术
        
    }
    

    public class RoleMgrBase
    {
        protected bool _PauseGame;
        protected MonsterBaseState _RoleState;
        private Camera _GameCamera;
        protected string _ActorTag;
        protected bool _enterHenShiStateFlag = false;
        protected bool _GameOver = false;
        public bool EnterHenShiStateFlag
        {
            get
            {
                return _enterHenShiStateFlag;
            }
            set
            {
                _enterHenShiStateFlag = value;
            }
        }

        public RoleMgrBase(Camera gameCamra)
        {
            _GameCamera = gameCamra;
        }
        public virtual void OnInit()
        {
            _PauseGame = false;
        }
        protected virtual void InitRole()
        {
            
        }
        public virtual GameObject getActor()
        {
            return null;
        }
        public virtual  void OnUpdate()
        {
            if (_PauseGame)
            {
                return;
            }
            if (_RoleState != null)
            {
                _RoleState.OnUpdate();
            }
            if (_RoleState != null && !_RoleState.m_AllowClick)
            {
                return;
            }
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                      //  TouchBeginHandle(touch.position);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        TouchHandle(touch.position);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    TouchHandle(Input.mousePosition);
                }
            }

        }
        void TouchBeginHandle(Vector2 startTouchpos)
        {
            TouchHandle(startTouchpos);
        }
        void TouchHandle(Vector2 startTouchpos)
        {
            if (_GameCamera == null)
            {
                return;
            }
            RaycastHit hit;
            Ray ray;
            ray = _GameCamera.ScreenPointToRay(startTouchpos);
            if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
            {
                GameObject obj = hit.collider.gameObject;
                if (obj != null)
                {
                    string tag = obj.tag; 
                    TouchEvent(obj, _GameCamera, _GameCamera.ScreenToWorldPoint(startTouchpos));
                    TouchEvent(obj);
                }
            }
        }
        public virtual void TouchEvent(GameObject obj)
        {
            if (_RoleState != null)
            {
                _RoleState.TouchEvent(obj);
            }
        }
        public virtual void TouchEvent(GameObject obj,Camera camera,Vector3 pos)
        {
            if (_RoleState != null)
            {
                _RoleState.TouchEvent(obj, camera,pos);
            }
        }
        public virtual void CleanState()
        {
            if (_RoleState != null)
            {
                _RoleState.CleanState();
                _RoleState = null;
            }
        }
        public virtual void EnterIdle()
        {
           
        }
        public virtual void EnterClickState()
        {
           
        }
        public virtual void EnterAttackState()
        {

        }
        public virtual void EnterHenShiState()
        {

        }
        public virtual void EnterSuccessState()
        {

        }
        public virtual void EnterFailState()
        {

        }
       
        public virtual void EnterUnMatchedState(YMGameWoundedType type)
        {

        }
        public virtual void Stop()
        {

        }
        public virtual bool UpdateMainTexture(int id)
        {
            return false;
        }
        public virtual void RewardClickCoin(Camera camera,Vector3 pos,System.Action success, System.Action fail)
        {
            RewardUIManager.Instance.RegisterOfflineBonus(1, camera.WorldToScreenPoint(pos), ModuleType.SpriteWindow, (b)=> {
                if (b)
                {
                    if (success!=null)
                    {
                        success();
                    }
                }
                else
                {
                    if (fail != null)
                    {
                        fail();
                    }
                }
            });
            RewardUIManager.Instance.SetSuccess();
        }

             
        public virtual void EnterNextState(YMGameEvent gameEvent, YMGameWoundedType type = YMGameWoundedType.YMG_MONSTER_WOUNDED_TYPE_SIXCLICK )
        {
            switch (gameEvent)
            {
                case YMGameEvent.YMG_EVENT_IDLESTATE:
                    {
                        EnterIdle();
                    }
                    break;
                case YMGameEvent.YMG_EVENT_ATTACKSTATE:
                    {
                        EnterAttackState();
                    }
                    break;
                case YMGameEvent.YMG_EVENT_CLICKSTATE:
                    {
                        EnterClickState();
                    }
                    break;
                case YMGameEvent.YMG_EVENT_HENSHINSTATE:
                    {
                        EnterHenShiState();
                    }
                    break;
                case YMGameEvent.YMG_EVENT_UNMATCHEDSTATE:
                    {
                        EnterUnMatchedState(type); 
                    }
                    break;
            }
        }
    }
    public class YearMonsterMgr: RoleMgrBase
    {
        protected GameObject _Monster;
        protected GameObject _Hand;
        protected bool _ShowHand = false;
        protected YMGameMonsterLevel _CurrentLevel;
        protected float touchEventTime = 0;
        public YearMonsterMgr(Camera gameCamra):base(gameCamra)
        {
            touchEventTime = 0;

            EnterHenShiStateFlag = false;

            _ShowHand = true;

            FetchMonsterLevel(YMLocalData.m_MonsterLevel);

            _GameOver = false;

            _ActorTag = "Player";
            OnInit();
        }
        public override GameObject getActor()
        {
            if (_Monster == null)
            {
                InitRole();
            }
            return _Monster;
        }
        protected override void InitRole()
        {
            if (_Monster == null)
            {
                _Monster = GameObject.FindGameObjectWithTag(_ActorTag);
                if (_Monster == null)
                {
                    _Monster = GameResMgr.Instance.LoadResource<GameObject>("public/mesh/jl_00014/public_model_jl_00014#mesh", true);
                    _Monster.transform.position = new Vector3(0f, 0f, 2.73f);
                    _Monster.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    _Monster.gameObject.SetActive(true);
                    _Monster.gameObject.tag = _ActorTag;
                    CapsuleCollider capsuleCollider = _Monster.GetAddComponent<CapsuleCollider>();
                    capsuleCollider.center = new Vector3(0f, 515f, 0f);
                    capsuleCollider.radius = 320f;
                    capsuleCollider.height = 1098f;
                    capsuleCollider.direction = 1;
                    Rigidbody rigidbody = _Monster.GetAddComponent<Rigidbody>();
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                    rigidbody.mass = 100f;
                }
            }
           
            if (_Hand == null)
            {
                _Hand = GameResMgr.Instance.LoadResource<GameObject>("public/hero_effect/prefab/public_effect_shoudianji", true);
            }
            _Hand.transform.position = new Vector3(0, 2.3f, 14f);
            _Hand.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0f));
            _Hand.gameObject.SetActive(false);

            UpdateMainTexture(0);
        }
        public override void OnInit()
        {
            InitRole();
//#if !UNITY_EDITOR
//             _MonsterState = new MonsterSayHelloState();
//#else
//            _RoleState = new MonsterIdleState(this);
//#endif
             _RoleState = new MonsterSayHelloState(this);

            _RoleState.OnEnter();
        }
        public override void EnterIdle()
        {
            if (_ShowHand)
            {
                _ShowHand = false;
                _Hand.gameObject.SetActive(true);
            }

            CleanState();
            _RoleState = new MonsterIdleState(this);
            _RoleState.OnEnter();
        }
        public override void EnterClickState()
        {
            if (_GameOver) return;
            
            if (_RoleState!=null && typeof(MonsterClickState) == _RoleState.GetType())
            {
                ((MonsterClickState)_RoleState).MultipleClick();
            }
            else
            {
                CleanState();
                _RoleState = new MonsterClickState(this);
                _RoleState.OnEnter();
            }
        }
        public override void EnterAttackState()
        {
            touchEventTime = 0;
            if (_GameOver) return;

            CleanState();
            _RoleState = new MonsterAttackState(this, _CurrentLevel);
            _RoleState.OnEnter();

        }
        public override void TouchEvent(GameObject obj,Camera camera, Vector3 pos)
        {
            if (obj == null)
            {
                return;
            }
            if (obj.tag.Equals(_ActorTag))//点击怪兽
            {
                if (_GameOver) return;

                HideHand();
                if ((_RoleState != null && typeof(MonsterClickState) == _RoleState.GetType())
                    || (_RoleState != null && typeof(MonsterIdleState) == _RoleState.GetType()))
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        AudioManager.Instance.PlayEffectAutoClose("newyeargame/sound/game/xwk_hd_ns_14_3.ogg");
                    }
                    else
                    {
                        AudioManager.Instance.PlayEffectAutoClose("newyeargame/sound/game/xwk_hd_ns_14_4.ogg");
                    }

                    touchEventTime++;
                    if (touchEventTime > 12)
                    {
                        touchEventTime = 0;
                        //切换到受伤状态
                        EnterNextState(YMGameEvent.YMG_EVENT_UNMATCHEDSTATE);
                    }
                    else
                    {
                        EnterClickState();
                    }
                    Assets.Scripts.C_Framework.C_EventHandler.SendEvent(C_EnumEventChannel.Global, "YMGameBloodEvent", 9);
                }
            }

            base.TouchEvent(obj, camera,pos);
        }
        public void DoMonsterLevelDown(float progress)
        {
            YMGameMonsterLevel Level = FetchMonsterLevel(progress);
            if (Level == _CurrentLevel)
            {
                return;
            }
            _CurrentLevel = Level;
            if (_RoleState != null 
                && (_RoleState.GetType() == typeof(MonsterWoundedState)
                || _RoleState.GetType() == typeof(MonsterHenShinState)))
            {
                EnterHenShiStateFlag = true;
            }
            else
            {
                EnterHenShiState();
            }
        }
        public YMGameMonsterLevel FetchMonsterLevel(float progress)
        {
            YMGameMonsterLevel level;
            //如果不是可以变身状态，先等待结束 
            if (progress > 80 && progress <= 100)
            {
                level = YMGameMonsterLevel.YMG_MONSTER_LEVEL_1;
            }
            else if (progress > 61 && progress <= 80)
            {
                level = YMGameMonsterLevel.YMG_MONSTER_LEVEL_2;
            }
            else if (progress > 40 && progress <= 60)
            {
                level = YMGameMonsterLevel.YMG_MONSTER_LEVEL_3;
            }
            else //if (progress > 20 && progress <= 40)
            {
                level = YMGameMonsterLevel.YMG_MONSTER_LEVEL_4;
            }
            //else //if (progress > 0 && progress <= 20)
            //{
            //    _CurrentLevel = YMGameMonsterLevel.YMG_MONSTER_LEVEL_3;
            //}

            return level;
        }
        public override void EnterHenShiState()
        {
            touchEventTime = 0;
            CleanState();
            _RoleState = new MonsterHenShinState(this, (int)_CurrentLevel);
            _RoleState.OnEnter();
        }
        public override void EnterSuccessState()
        {
            HideHand();

            touchEventTime = 0;

            _GameOver = true;
            if (_RoleState != null && typeof(MonsterSuccessState) != _RoleState.GetType())
            {
                CleanState();
                _RoleState = new MonsterSuccessState(this);
                _RoleState.OnEnter();
            }
        }
        public override void EnterFailState()
        {
            HideHand();
            touchEventTime = 0;

            _GameOver = true;
            if (_RoleState != null && typeof(MonsterFailState) != _RoleState.GetType())
            {
                CleanState();
                _RoleState = new MonsterFailState(this);
                _RoleState.OnEnter();
            }
        }
        public override void EnterUnMatchedState(YMGameWoundedType yMGameWoundedType)
        {
            touchEventTime = 0;

            if (_RoleState != null && typeof(MonsterWoundedState) != _RoleState.GetType())
            {
                CleanState();
                _RoleState = new MonsterWoundedState(this, yMGameWoundedType);
                _RoleState.OnEnter();
            }
        }
        public override bool UpdateMainTexture(int id)
        {
             string[] monsterMatTex =
            {
                "public/mesh/jl_00014/upTex/jl_00014_tx_c_1_l1.png",
                "public/mesh/jl_00014/upTex/jl_00014_tx_c_1_l2.png",
                "public/mesh/jl_00014/upTex/jl_00014_tx_c_1_l3.png",
                "public/mesh/jl_00014/upTex/jl_00014_tx_c_1_l4.png"
            };
           if (id >= monsterMatTex.Length) 
            {
                return false;
            }
            Texture _OrignalTexture = GameResMgr.Instance.LoadResource<Texture>(monsterMatTex[id]);
            Material _Material = _Monster.transform.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
            _Material.SetTexture("_MainTex", _OrignalTexture);
            return true;
        }
        public override void Stop()
        {
            CleanState();
            if (_Monster!=null)
            {
                GameObject.DestroyObject(_Monster);
                _Monster = null;
            }
        }
        public void HideHand()
        {
            if (_Hand !=null)
            {
                _Hand.gameObject.SetActive(false);
            }
        }
    }
}


