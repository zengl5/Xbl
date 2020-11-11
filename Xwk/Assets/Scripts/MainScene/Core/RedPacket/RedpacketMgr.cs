using Assets.Scripts.C_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XWK.Common.RedBomb;
using YB.YM.Game;

namespace YB.XWK.MainScene
{
    public class RedpacketMgr : RoleMgrBase
    {

        private GameObject _RedPacket;
        private Animator _Animator;
        private Camera _ActorCamera;
        private bool _Pause;
        private IActor _TargetActor;

        public RedpacketMgr(Camera gameCamra, IActor target) : base(gameCamra)
        {
            _ActorTag = "Player";
            _TargetActor = target;
            _ActorCamera = gameCamra;
            OnInit();
        }

        public override GameObject getActor()
        {
            if (_RedPacket == null)
            {
                InitRole();
            }
            return _RedPacket.gameObject;
        }
 
        protected override void InitRole()
        {
            if (_RedPacket == null)
            {
                _RedPacket = GameResMgr.Instance.LoadResource<GameObject>("public/mesh/jl_00015/public_model_jl_00015#mesh", true);
                _RedPacket.transform.position = new Vector3(-17.05f, 0f, -1.04f);
                _RedPacket.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                _RedPacket.gameObject.SetActive(true);
                _RedPacket.gameObject.tag = _ActorTag;
                CapsuleCollider capsuleCollider = _RedPacket.GetAddComponent<CapsuleCollider>();
                capsuleCollider.center = new Vector3(0f, 67.4f, 0f);
                capsuleCollider.radius = 45.69f;
                capsuleCollider.height = 124.5f;
                capsuleCollider.direction = 1;
                capsuleCollider.isTrigger = true;
                Rigidbody rigidbody = _RedPacket.GetAddComponent<Rigidbody>();
                rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                rigidbody.mass = 100f;
            }
        }
        public override void OnInit()
        {
            InitRole();
           // EnterIdle();
        }
        public override void EnterIdle()
        {
            CleanState();
            _RoleState = new RpWalkAroundState(this);
            _RoleState.OnEnter();
        }
        public override void EnterClickState()
        {
            if(_RoleState!=null && typeof(RpClickState) != _RoleState.GetType())
            {
                CleanState();


                if (!WizardData.IsWizardCollected(WizardItemName.Wizard_hongbao))//第一次进入红包
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_redpacket_collect");

                    AudioManager.Instance.StopAllSounds();

                    WizardData.AddWizardItem(WizardItemName.Wizard_hongbao);
                    WizardData.CurrentLocationRecommend(WizardItemName.Wizard_hongbao);

                    C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE);
                    SpiritAdModel _SpiritAdModel = new SpiritAdModel();
                    _SpiritAdModel.Load();
                    C_UIMgr.Instance.OpenUI("UI_CollectSpiritAction", WizardItemName.Wizard_hongbao, _SpiritAdModel, new System.Action(() => {
                        EnterRedPacketGame();
                    }));
                }
                else
                {
                    C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "main_redpacket");

                    EnterRedPacketGame();

                    //出现收集效果
                    //   _RoleState = new RpClickState(this);
                    //   _RoleState.OnEnter();
                }
            }
        }
        void EnterRedPacketGame()
        {
            AudioManager.Instance.StopAllSounds();
            RedBombManager.Instance.StartPlay();
            C_UIMgr.Instance.MandatoryCloseUIAll();
            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", GameEventEnum.GAME_EVENT_ENUM_PAUSE_GAME_FALSE);
        }
        public override void TouchEvent(GameObject obj, Camera camera, Vector3 pos)
        {
            if (obj == null)
            {
                return;
            }
            if (obj.tag.Equals(_ActorTag)) 
            {
                EnterClickState();
            }
            base.TouchEvent(obj, camera, pos);
        }
       
        public override void Stop()
        {
            CleanState();
            _Pause = true;

            if (_RedPacket != null)
            {
                GameObject.DestroyObject(_RedPacket);
                _RedPacket = null;
            }
        }
        public void Pause()
        {
            CleanState();
            _Pause = true;
            if (_RedPacket != null)
            {
                _RedPacket.gameObject.SetActive(false);
            }
        }
        public void Resume()
        {
            if (_RedPacket == null)
            {
                OnInit();
            }
            _RedPacket.gameObject.SetActive(true);
            EnterIdle();
        }
    }
}
