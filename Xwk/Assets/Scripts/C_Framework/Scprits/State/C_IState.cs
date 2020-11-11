using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public interface C_IState
    {
        string Name { get; set; }

        void OnStateEnter();

        void OnStateLeave();

        void OnStateOverride();

        void OnStateResume();
    }
}
