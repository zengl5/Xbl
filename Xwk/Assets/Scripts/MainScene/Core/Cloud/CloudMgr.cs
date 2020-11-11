using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YB.XWK.MainScene {
    public class CloudMgr : MonoBehaviour, IActor
    {
        private GameObject _Cloud;
        private Animator _Animator;
        private Camera _ActorCamera;
        private CloudConfig _CloudConfig;
        private ActorState _ActorState;
        private bool _Pause;
        private IActor _TargetActor;

        public Camera m_Camera
        {
            get
            {
                return _ActorCamera;
            }

            set
            {
                _ActorCamera = value;
            }
        }
        public Transform getActor()
        {
            if (_Cloud == null)
            {
                _Cloud = GameObject.FindGameObjectWithTag("MainCloud");
            }
            return _Cloud.transform;
        }

        public Animator getAnimator()
        {
            return _Animator ?? getActor().GetComponent<Animator>();
        }

        public ActorStateInfo getInfo(string stateName)
        {
            if (_CloudConfig == null)
            {
                _CloudConfig = new CloudConfig();
            }
            return _CloudConfig.FecthData(stateName);
        }
        
        public void EnterIdleState()
        {
            bool flag = false;
            if (_ActorState == null)
            {
                flag = true;
            }
            //原地飘动
            if (_ActorState!=null && _ActorState.GetType()!= typeof(CloudIdleState))
            {
                flag = true;
            }
            if (flag)
            {
                ClearState();
                _ActorState = new CloudIdleState(this);
            }
        }

        public void EnterTouchState()
        {
            ClearState();
            _ActorState = new CloudTouchState(this);
        }
        public void EnterRainState()
        {
            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_main_event, LocalData.m_jindouyun_click);
            if (_ActorState != null && _ActorState.GetType() != typeof(CloudRainState))
            {
                ClearState();
                _ActorState = new CloudRainState(this);
            }
        }
        public void EnterRainState2()
        {
            if (_ActorState ==null)
            {
                return;
            }
            if (_ActorState!=null && _ActorState.GetType() != typeof(CloudRainState))
            {
                return;
            }
            CloudRainState cloudRainState = (CloudRainState)_ActorState;
            cloudRainState.DoTwiceTouch();
        }
        public void EnterSayHelloState()
        {
            bool flag = false;
            if (_ActorState == null)
            {
                flag = true;
            }
            //原地飘动
            if (_ActorState != null && _ActorState.GetType() != typeof(CloudWalkAroundState))
            {
                flag = true;
            }
            if (flag)
            {
                ClearState();
                _ActorState = new CloudHelloState(this);
            }
        }
  
        public void EnterWalkAoundState()
        {
            bool flag = false;
            if (_ActorState == null)
            {
                flag = true;
            }
            //原地飘动
            if (_ActorState != null && _ActorState.GetType() != typeof(CloudWalkAroundState))
            {
                flag = true;
            }
            if (flag)
            {
                ClearState();
                _ActorState = new CloudWalkAroundState(this);
            }
        }
        public void PlayAudio(string audioType, string audio, Action callback, bool loop)
        {
            //throw new NotImplementedException();
        }
        public void SetCoudActive(bool active)
        {
            _Pause = !active;
        }
        public void OnUpdate()
        {
            if (_Pause)
            {
                return;
            }
            if (_ActorState != null)
            {
                _ActorState.OnUpdate();
            }
            if (_ActorState!=null && !_ActorState.HasInteractiveState)
            {
                return;
            }
            
            RaycastHit hit;
            Ray ray ;
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
                ray = _ActorCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
                {
                    GameObject obj = hit.collider.gameObject;
                    if (obj != null
                        && obj.name.Equals("Point001"))
                    {
                        //if (_ActorState != null && _ActorState.GetType() == typeof(CloudRainState))
                        //{
                        //    EnterRainState2();
                        //}
                        //else
                        {
                            C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_click_cloud);

                            int data = UnityEngine.Random.Range(0, 10);
                            if (data < 5)
                            {
                                EnterTouchState();
                            }
                            else
                            {
                                EnterRainState();
                            }
                        }
                        
                    }
                }
            }

#else
           if (Application.isMobilePlatform && Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    ray = _ActorCamera.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out hit, 1000) && hit.collider != null)
                    {
                        GameObject obj = hit.collider.gameObject;
                        if (obj != null
                            && obj.name.Equals("Point001"))
                        {
                            //if (_ActorState != null && _ActorState.GetType() == typeof(CloudRainState))
                            //{
                            //    EnterRainState2();
                            //}
                            //else
                            {
                                 C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, LocalData.m_click_cloud);

                                int data = UnityEngine.Random.Range(0, 10);
                                if (data < 5)
                                {
                                    EnterTouchState();
                                }
                                else
                                {
                                    EnterRainState();
                                }
                            }
                        }
                    }
                }
            }   
#endif

        }

        public void OnInit(Camera camera, IActor target)
        {
            _TargetActor = target;
            _ActorCamera = camera; 
            if (_CloudConfig==null)
            {
                _CloudConfig = new CloudConfig();
            }
            _CloudConfig.Load();
            _Animator = getAnimator();

            _Pause = false;
        }
        private void ClearState()
        {
            if (_ActorState != null)
            {
                _ActorState.Stop();
                _ActorState = null;
            }
        }
        public void Stop()
        {
            _Pause = true;
            ClearState();
            GameObject Cloud = getActor().gameObject;
            //销毁自己
            DestroyObject(Cloud);
            Cloud = null;
        }
         
        List<QuestionConfigData> IActor.getActorAnimancerInfo()
        {
            return null;

        }

        public ActorAnimancerResConfig getActorResConfig(string stateName)
        {
            return null;
        }
        public void EnterFlashState()
        {
            Transform target = _TargetActor.getActor();
            float zoneX = target.position.x;
            float actorX = _Cloud.transform.position.x;
            if (actorX < zoneX + 4f && zoneX - 4f < actorX)
            {
                _TargetActor.EnterNextState(ActorStateEnum.ACTOR_STATE_ENUM_FLASH);
            }
        }
        public void EnterNextState(ActorStateEnum actorStateEnum)
        {
            switch (actorStateEnum) {
                case ActorStateEnum.ACTOR_STATE_ENUM_FLASH:
                    {
                        EnterFlashState();
                    }
                    break;
                case ActorStateEnum.ACTOR_STATE_ENUM_IDLE:
                    {
                        EnterIdleState();
                    }
                    break;
                case ActorStateEnum.ACTOR_STATE_ENUM_WALKAROUND:
                    {
                        EnterWalkAoundState();
                    }
                    break;
            }

        }
    }

}

