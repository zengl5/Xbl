using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene {

    public abstract class AnimAncerAimState : AnimStateBase
    {
        public AnimancerComponent m_AnimancerComponent;
        //所有动画数据
        public ActorAnimancerResConfig m_ActorAnimancerResConfig;
        //单个动画的资源
        public ActorAnimancerData m_InfoData;

        public AnimAncerAimState m_CurentAncerAnimState;

    }

}

