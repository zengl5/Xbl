using UnityEngine;
using System.Collections;

namespace Slate.ActionClips{

	[Category("Transform")]
	[Description("Set the parent of the actor gameobject temporarily, or permanently if length is zero")]
	public class SetTransformParent : ActorActionClip {

		[SerializeField] [HideInInspector]
		private float _length;

		public Transform newParent;
		public bool resetPosition = false;
		public bool resetRotation = false;
		public bool resetScale = false;

		private Transform originalParent;
		private Vector3 originalPos;
		private Quaternion originalRot;
		private Vector3 originalScale;
       [Tooltip("添加在父节点中子节点位置")]
        public Vector3 newlocalPos = Vector3.zero;
        public Vector3 newlocalRot= Vector3.zero;
        public Vector3 newlocalScale= Vector3.one;

        public string mParentName;
        private bool temporary;

		public override string info{
			get {return string.Format("Set Parent\n{0}", newParent != null? newParent.name : "none");}
		}

		public override float length{
			get {return _length;}
			set {_length = value;}
		}

		protected override void OnEnter(){ temporary = length > 0; Do(); }
		protected override void OnReverseEnter(){ if (temporary){ Do(); } }
		protected override void OnExit(){ if (temporary) { UnDo(); } }
		protected override void OnReverse(){ UnDo(); }

		void Do(){
            if (!string.IsNullOrEmpty(mParentName) && newParent == null)
            {
                if(Utility.FindCurrentCutscene() !=null && Utility.FindCurrentCutscene().CutscenePath!=null)
                    newParent = Utility.FindCurrentCutscene().CutscenePath.transform.Find(mParentName);
            }
			originalParent = actor.transform.parent;
			originalPos = actor.transform.localPosition;
			originalRot = actor.transform.localRotation;
			originalScale = actor.transform.localScale;

			actor.transform.SetParent(newParent, true);
            if (resetPosition) { actor.transform.localPosition = newlocalPos; } 
			if (resetRotation){ actor.transform.localEulerAngles = newlocalRot; }  
            if (resetScale){ actor.transform.localScale = newlocalScale; } 
        }

		void UnDo(){
			actor.transform.SetParent(originalParent, true);
			actor.transform.localPosition = originalPos;
			actor.transform.localRotation = originalRot;
			actor.transform.localScale = originalScale;
		}
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect)
        {
            if (newParent!=null)
            {
                mParentName = newParent.name;
            }
            if (newParent == null && !string.IsNullOrEmpty(mParentName))
            {
                if (Utility.FindCurrentCutscene() == null)
                {
                   // Debug.LogError("当前没有镜头，先拖个镜头到cutsceneplayer");
                    return;
                }
                if (Utility.FindCurrentCutscene().CutscenePath != null)
                    newParent = Utility.FindCurrentCutscene().CutscenePath.transform.Find(mParentName);
            }
        }
#endif
    }
}