using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.C_Framework;

namespace Slate{

	[Description("** This Track will be deprecated in the future. Please use Animator Track instead. **\n\nThe Mecanim Track works with an 'Animator' component attached on the actor and with it's assigned Controller by modifying the Controller's parameters.\n\nConsider working with the new Animator Track instead to playback animation clips directly without the need of a Controller, which is more intuitive for animations.")]
	[Icon("Animator Icon")]
	[Category("Legacy")]
	[Attachable(typeof(ActorGroup))]
	public class MecanimTrack : CutsceneTrack {

		private Animator animator;

		private AnimatorDispatcher _dispatcher;
		public AnimatorDispatcher dispatcher{
			get
			{
				if (actor == null) { return null; }
				if (_dispatcher == null || _dispatcher.gameObject != actor.gameObject){
					_dispatcher = actor.GetComponent<AnimatorDispatcher>();
					if (_dispatcher == null){
						_dispatcher = actor.gameObject.AddComponent<AnimatorDispatcher>();
					}
				}
				return _dispatcher;		
			}
		}
        [Header("填入相应动画片段的集数（比如iuv）,如果是公共动画片段则填入public")]
        [Required]
        public string AnimatorControllerType;
        [Header("填入相应动画动画控制器AnimatorController名字")]
        [Required]
        public string AnimatorControllerName;
        private RuntimeAnimatorController _RuntimeAnimatorController;


        protected override bool OnInitialize(){
            //animator = actor.GetComponent<Animator>();
            //if (animator == null){
            //    Debug.LogError("Mecanim Track requires that the actor has the Animator Component attached.", actor);
            //    return false;
            //}


            //if (animator.runtimeAnimatorController == null){
            //    Debug.LogWarning(string.Format("The Mecanim Track requires the target actor '{0}' to have an assigned Runtime Animator Controller", actor.name));
            //    return false;
            //}
            if(!LoadAnimatorController()){
                Debug.LogWarning(string.Format("The Mecanim Track in actor '{0}' init Runtime Animator Controller error", actor.name));
                return false;
            }
			return true;	
		}
        protected virtual bool LoadAnimatorController()
        {
            animator = actor.GetComponent<Animator>();
            if (animator== null)
            {
                animator = actor.AddComponent<Animator>();
            }
            //System.Text.StringBuilder path = new System.Text.StringBuilder();
            //path.Append(AnimatorControllerType).Append("/").Append("AnimatorController/").Append(AnimatorControllerName);
            //RuntimeAnimatorController ctrl = Resources.Load<RuntimeAnimatorController>(path.ToString()) as RuntimeAnimatorController;
            //if (ctrl != null)
            //    Debug.Log(ctrl.name);

            // animator.runtimeAnimatorController = C_Singleton<GameResMgr>.GetInstance().LoadResource<RuntimeAnimatorController>(AnimatorControllerName + ".controller", AnimatorControllerType, "AnimatorController") as RuntimeAnimatorController;
            _RuntimeAnimatorController = C_Singleton<GameResMgr>.GetInstance().LoadResource<RuntimeAnimatorController>(AnimatorControllerName + ".controller", AnimatorControllerType, "",string.Concat(AnimatorControllerType,"/")) as RuntimeAnimatorController;
            animator.runtimeAnimatorController = _RuntimeAnimatorController;
            animator.applyRootMotion = true;
          //  C_DebugHelper.Log(" animator.runtimeAnimatorController :" + animator.runtimeAnimatorController + "-- animator:" + animator.name+"   cutscene name :  "+root.context.name);

            //Debug.Log("animator.runtimeAnimatorController "+animator.runtimeAnimatorController .name);
            if (_RuntimeAnimatorController == null)
            {
                Debug.LogWarning(string.Format("The Mecanim Track requires the target actor '{0}' to have an assigned Runtime Animator Controller", actor.name));
                return false;
            }
            return true;
        }

        protected override void OnEnter()
        {
            animator = actor.GetComponent<Animator>();
            if (animator == null)
            {
                return;
            }
            animator.runtimeAnimatorController = _RuntimeAnimatorController;
          //  C_DebugHelper.Log(" animator.runtimeAnimatorController :" + animator.runtimeAnimatorController + "-- animator:" + animator.name);
#if UNITY_EDITOR
            MecanimEnter();
#endif

        }
#if UNITY_EDITOR

        private AnimatorCullingMode wasCullingMode;
		const int RECORDING_FRAMERATE = 10;

		protected  void MecanimEnter(){

			//animator = actor.GetComponent<Animator>();
			//if (animator == null){
			//	return;
			//}
   //         animator.runtimeAnimatorController = _RuntimeAnimatorController;

			if (Application.isPlaying || layerOrder != 0){ //only 0 MecanimTrack layer does the recording
				animator = null;
				return;
			}


			wasCullingMode = animator.cullingMode;
			animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

			var updateInterval = (1f/RECORDING_FRAMERATE);

			animator.recorderStartTime = this.startTime;
			animator.recorderStopTime = this.endTime + updateInterval;
			animator.StartRecording(0);


			var clips = new List<IDirectable>();
			foreach (var track in (parent as CutsceneGroup).tracks.OfType<MecanimTrack>().Where(t => t.isActive).Reverse() ){
				clips.AddRange( track.actions.OfType<ActionClips.MecanimBaseClip>().Where(a => a.isValid).Cast<IDirectable>() );
			}
			clips = clips.OrderBy(a => a.startTime).ToList();

			var lastTime = -1f;
			for (var i = startTime; i <= endTime + updateInterval; i += updateInterval){
				foreach (var clip in clips){

					if (i >= clip.startTime && lastTime < clip.startTime){
						clip.Enter();
						clip.Update(0,0);
					}

					if (i >= clip.startTime && i <= clip.endTime){
						clip.Update(i - clip.startTime, i - clip.startTime - updateInterval);
					}

					if (i > clip.endTime && lastTime <= clip.endTime){
						clip.Update(clip.endTime - clip.startTime, Mathf.Max(0, lastTime - clip.startTime) );
						clip.Exit();
					}
				}

				animator.Update(updateInterval);
				lastTime = i;
			}

			animator.StopRecording();
			animator.StartPlayback();
		}

		protected override void OnUpdate(float time, float previousTime){
			if (animator != null && time != endTime){
				animator.playbackTime = time;
				animator.Update(0);
			}
		}

        public override string GetAffectResPath()
        {
           // Debug.Log("mecanim 记录到加载列表");
            if (string.IsNullOrEmpty(AnimatorControllerType) || string.IsNullOrEmpty(AnimatorControllerName))
            {
                return "";
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            return stringBuilder.Append(AnimatorControllerType).Append("/AnimatorController/").Append(AnimatorControllerName).ToString();
        }
#endif


        protected override void OnReverse(){
		
			DestroyDispatcher();

			#if UNITY_EDITOR
			if (animator != null){
				animator.cullingMode = wasCullingMode;
				animator.StopPlayback();
				animator = null;
			}
			#endif
		}

		protected override void OnExit(){
            _RuntimeAnimatorController = null;

            DestroyDispatcher();			
		}

		void DestroyDispatcher(){
			var dispatcher = actor.GetComponent<AnimatorDispatcher>();
			if (dispatcher != null){
				DestroyImmediate(dispatcher);
			}
		}
	}
}