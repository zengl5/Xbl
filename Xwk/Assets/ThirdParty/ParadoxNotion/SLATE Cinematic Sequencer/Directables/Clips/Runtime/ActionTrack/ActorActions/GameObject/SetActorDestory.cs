using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips
{
    [Category("GameObject")]
    [Description("设置角色销毁")]
    public class SetActorDestory : ActorActionClip
    {
        
        [SerializeField]
        [HideInInspector]
        private float _length;
        public override float length
        {
            get { return _length; }
            set { _length = value; }
        }
        [SerializeField]
        private float _WaitTime = 0f;
        protected override void OnEnter()
        {
            GameObject.Destroy(actor,_WaitTime);
        }
    }

}
