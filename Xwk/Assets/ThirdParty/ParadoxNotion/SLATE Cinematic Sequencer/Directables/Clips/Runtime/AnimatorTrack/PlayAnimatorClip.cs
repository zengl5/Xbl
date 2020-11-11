#if UNITY_5_4_OR_NEWER
//#define _Res  
#define _As
using UnityEngine;
using System.Collections;
using Assets.Scripts.C_Framework;

namespace Slate.ActionClips
{

    [Name("Animation Clip")]
    [Attachable(typeof(AnimatorTrack))]
    public class PlayAnimatorClip : ActorActionClip, ICrossBlendable, ISubClipContainable
    {
        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0f;
        [Header("填入相应动画片段的集数（比如iuv）,如果是公共动画片段则填入public")]
        [Required]
        public string _AssetBundleType;
        [Header("填入相应动画片段名字")]
        [Required]
        public string _ClipName;
        [HideInInspector]
        public AnimationClip animationClip;
        public float clipOffset;
        [Range(0.1f, 2)]
        public float playbackSpeed = 1f;
        [AnimatableParameter]
        public Vector2 steerLocalRotation;

        private Vector3 wasRotation;

        float ISubClipContainable.subClipOffset
        {
            get { return clipOffset; }
            set { clipOffset = value; }
        }

        public override string info
        {
            get {
                LoadAnimator();
                return animationClip != null ? animationClip.name : base.info;
            }
        }

        public override bool isValid
        {
            get {
                LoadAnimator();
                return base.isValid && animationClip != null && !animationClip.legacy;
            }
        }

        public override float length
        {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn
        {
            get { return _blendIn; }
            set { _blendIn = value; }
        }

        public override float blendOut
        {
            get { return _blendOut; }
            set { _blendOut = value; }
        }

        private AnimatorTrack track { get { return (AnimatorTrack)parent; } }
        private Animator animator { get { return track.animator; } }

        protected override void OnEnter()
        {
            if (animationClip == null)
            {
                LoadAnimator();
            }
            wasRotation = (Vector2)animator.transform.GetLocalEulerAngles();
            track.EnableClip(this);
        }

        protected override void OnReverseEnter() { track.EnableClip(this); }

        protected override void OnUpdate(float time, float previousTime)
        {

            if (track.useRootMotion && steerLocalRotation != default(Vector2))
            {
                var rot = wasRotation + (Vector3)steerLocalRotation;
                animator.transform.SetLocalEulerAngles(rot);
            }

            track.UpdateClip(this, (time - clipOffset) * playbackSpeed, (previousTime - clipOffset) * playbackSpeed, GetClipWeight(time));
        }

        protected override void OnExit() { track.DisableClip(this); }
        protected override void OnReverse()
        {
            animator.transform.SetLocalEulerAngles(wasRotation);
            track.DisableClip(this);
        }


        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect)
        {
            if (string.IsNullOrEmpty(_AssetBundleType) || string.IsNullOrEmpty(_ClipName) || (animationClip!=null && !animationClip.name.Equals(_ClipName)))
            {
                animationClip = null;
            }
            if (animationClip == null && !string.IsNullOrEmpty(_AssetBundleType) && !string.IsNullOrEmpty(_ClipName))
            {
                LoadAnimator();
            }
            if (animationClip != null)
            {
                EditorTools.DrawLoopedLines(rect, animationClip.length / playbackSpeed, this.length, clipOffset);
            }
        }
        public override string GetAffectResPath()
        {
            if (string.IsNullOrEmpty(_AssetBundleType) || string.IsNullOrEmpty(_ClipName))
            {
                return "";

            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            return stringBuilder.Append(_AssetBundleType).Append(_ClipName).ToString();
        }

#endif
        protected virtual void LoadAnimator()
        {
#if _Res
            //if (root == null || parent == null ||(parent!=null && parent.actor == null))
            //{
            //    return;
            //}
            if (animationClip == null && !string.IsNullOrEmpty(_AssetBundleType) && !string.IsNullOrEmpty(_ClipName)){
                System.Text.StringBuilder path = new System.Text.StringBuilder();
                path.Append("PackagingResources/").Append(_AssetBundleType).Append("/").Append("Anim/").Append(_ClipName);
                animationClip = Resources.Load<AnimationClip>(path.ToString()) as AnimationClip;
            }
            
            return;
#elif _As
            if (animationClip == null && !string.IsNullOrEmpty(_AssetBundleType) && !string.IsNullOrEmpty(_ClipName))
            {
                //animationClip = C_Singleton<GameResMgr>.GetInstance().LoadResource<AnimationClip>(_ClipName + ".FBX", _AssetBundleType, "anim") as AnimationClip;
                animationClip = C_Singleton<GameResMgr>.GetInstance().LoadResource<AnimationClip>(_ClipName + ".FBX", _AssetBundleType,"",string.Concat(_AssetBundleType,"/")) as AnimationClip;
                //if (animationClip != null)
                //{
                //    C_DebugHelper.Log(animationClip.name);
                //}
                //else
                //{
                //    //  track.DisableClip(this);
                //}
            }
#endif
        }
    }
}

#endif