using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_GameStateCtrl : C_Singleton<C_GameStateCtrl>
    {
        private C_StateMachine m_GameState = new C_StateMachine();

        //public bool isBattleState
        //{
        //    get { return m_GameState.TopState() is BattleState; }
        //}

        //public bool isLoadingState
        //{
        //    get { return m_GameState.TopState() is LoadingState; }
        //}

        //public bool isLobbyState
        //{
        //    get { return m_GameState.TopState() is LobbyState; }
        //}

        //public bool isHeroChooseState
        //{
        //    get { return m_GameState.TopState() is HeroChooseState; }
        //}

        //public bool isLoginState
        //{
        //    get { return m_GameState.TopState() is LoginState; }
        //}

        public string CurrentStateName
        {
            get
            {
                C_IState curState = GetCurrentState();
                return (curState == null) ? "unkown state" : curState.Name;
            }
        }

        public void RegisterState(string name, C_IState state)
        {
            m_GameState.RegisterState(name, state);
        }

        public void Uninitialize()
        {
            m_GameState.Clear();
            m_GameState = null;
        }

        public void GotoState(string name)
        {
            C_DebugHelper.LogFormat("C_GameStateCtrl Goto State {0}", name);
            m_GameState.ChangeState(name);
        }

        public C_IState GetCurrentState()
        {
            return m_GameState.TopState();
        }
    }
}