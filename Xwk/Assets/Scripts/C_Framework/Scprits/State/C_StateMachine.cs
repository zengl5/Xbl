using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_StateMachine
    {
        private Dictionary<string, C_IState> m_RegistedState = new Dictionary<string, C_IState>();

        private Stack<C_IState> m_StateStack = new Stack<C_IState>();

        public C_IState TarState = null;

        public int Count { get { return m_StateStack.Count; } }

        public void RegisterState(string name, C_IState state)
        {
            if (string.IsNullOrEmpty(name))
            {
                C_DebugHelper.LogError("C_StateMachine RegisterState name is null or empty!");
                return;
            }

            if (state == null)
            {
                C_DebugHelper.LogError("C_StateMachine RegisterState state is null!");
                return;
            }

            if (m_RegistedState.ContainsKey(name))
                C_DebugHelper.LogError("C_StateMachine RegisterState name = " + name + " is covered!");

            m_RegistedState[name] = state;
        }

        public void UnregisterState(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (m_RegistedState.ContainsKey(name))
                m_RegistedState.Remove(name);
        }

        public C_IState GetState(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (m_RegistedState.ContainsKey(name))
                return m_RegistedState[name];

            return null;
        }

        public string GetStateName(C_IState state)
        {
            if (state == null)
                return "";

            Dictionary<string, C_IState>.Enumerator enumerator = m_RegistedState.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, C_IState> current = enumerator.Current;
                if (current.Value == state)
                    return current.Key;
            }

            return "";
        }

        public void Push(string name)
        {
            Push(GetState(name));
        }

        public void Push(C_IState state)
        {
            if (state == null)
                return;

            if (m_StateStack.Count > 0)
                m_StateStack.Peek().OnStateOverride();

            m_StateStack.Push(state);

            state.OnStateEnter();
        }

        public C_IState PopState()
        {
            if (m_StateStack.Count > 0)
            {
                C_IState state = m_StateStack.Pop();
                state.OnStateLeave();

                if (m_StateStack.Count > 0)
                    m_StateStack.Peek().OnStateResume();

                return state;
            }

            return null;
        }


        public C_IState ChangeState(string name)
        {
            return ChangeState(GetState(name));
        }

        public C_IState ChangeState(C_IState state)
        {
            if (state == null)
                return null;

            TarState = state;
            C_IState state2 = null;
            if (m_StateStack.Count > 0)
            {
                state2 = m_StateStack.Pop();
                state2.OnStateLeave();
            }

            m_StateStack.Push(state);
            state.OnStateEnter();

            return state2;
        }

        public C_IState TopState()
        {
            if (m_StateStack.Count > 0)
                return m_StateStack.Peek();

            return null;
        }

        public string TopStateName()
        {
            if (m_StateStack.Count > 0)
            {
                C_IState state = m_StateStack.Peek();

                if (state != null)
                    return GetStateName(state);
            }

            return "";
        }

        public void Clear()
        {
            while (m_StateStack.Count > 0)
                m_StateStack.Pop().OnStateLeave();
        }
    }
}