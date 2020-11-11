using UnityEngine;
using System.Collections;
using Assets.Scripts.C_Framework;
namespace Slate.ActionClips{

	[Name("Play Audio Clip")]
	[Description("The audio clip will be send to the AudioMixer selected in it's track if any. You can trim or loop the audio by scaling the clip and you can optionaly show subtitles as well.")]
	[Attachable(typeof(ActorAudioTrack), typeof(DirectorAudioTrack))]
	public class PlayAudio : ActionClip, ISubClipContainable {

		[SerializeField] [HideInInspector]
		private float _length = 1f;
		[SerializeField] [HideInInspector]
		private float _blendIn = 0f;
		[SerializeField] [HideInInspector]
		private float _blendOut = 0f;
        [Header("填入相应语音的集数（比如iuv）,如果是公共语音则填入public")]
        [Required]
        [SerializeField]
        private string _AudioType;
        [Header("填入对应语音的名字")]
        [Required]
        [SerializeField]
        private string _AuidoName;
        [HideInInspector]
		public AudioClip audioClip;
         
		[AnimatableParameter(0, 1)]
		public float volume = 1;
		[AnimatableParameter(-1, 1)]
		public float stereoPan = 0;

		public float clipOffset;
		[Multiline(5)]
		public string subtitlesText;
		public Color subtitlesColor = Color.white;

        [SerializeField]
        private bool _PlayLoop = false;

        protected bool _PlayOver = false;
		float ISubClipContainable.subClipOffset{
			get {return clipOffset;}
			set {clipOffset = value;}
		}

		public override float length{
			get { return _length;}
			set	{_length = value;}
		}

		public override float blendIn{
			get {return _blendIn;}
			set {_blendIn = value;}
		}

		public override float blendOut{
			get {return _blendOut;}
			set {_blendOut = value;}
		}

		public override bool isValid{
			get {
                LoadAudioRes();
                return audioClip != null;
            }
		}

		public override string info{
			get {
                LoadAudioRes();
                return isValid? (string.IsNullOrEmpty(subtitlesText)? audioClip.name : string.Format("<i>'{0}'</i>", subtitlesText) ): base.info;
            }
		}

		private AudioTrack track{
			get {return (AudioTrack)parent; }
		}

		protected AudioSource source{
			get {return track.source;}
		}
	
		protected override void OnEnter(){ Do(); }
		protected override void OnReverseEnter(){ Do(); }
		protected override void OnExit(){ Undo(); }
		protected override void OnReverse(){ Undo(); }

	protected  virtual void Do(){
            _PlayOver = false;
            audioClip = null;
        LoadAudioRes();
        if (audioClip == null)
        {
            return;
        }
        if (source != null)
        {
            source.clip = audioClip;
            source.loop = _PlayLoop;
            if (_PlayLoop && !source.isPlaying)
            {
                source.Play();
                source.loop = _PlayLoop;
            }
        }
    }
		protected override void OnUpdate(float time, float previousTime){
            if (_PlayOver)
            {
                return;
            }
            if (audioClip == null)
            {
                return;
            }
            if (source != null && !_PlayLoop)
            {
                float currentTimeLength = time - clipOffset;
                if (currentTimeLength > audioClip.length)
                {
                    _PlayOver = true;
                    return;
                }
                var weight = Easing.Ease(EaseType.QuadraticInOut, 0, 1, GetClipWeight(time));
				var totalVolume = weight * volume * track.weight;

				AudioSampler.Sample(source, audioClip, currentTimeLength, previousTime - clipOffset, totalVolume, track.ignoreTimeScale);
				source.panStereo = Mathf.Clamp01(stereoPan + track.stereoPan);
                source.loop = _PlayLoop;
				if (!string.IsNullOrEmpty(subtitlesText)){
					var lerpColor = subtitlesColor;
					lerpColor.a = weight;
					DirectorGUI.UpdateSubtitles(string.Format("{0}{1}", parent is ActorAudioTrack? (actor.name + ": ") : "", subtitlesText), lerpColor);
				}
			}
		}
        protected virtual void Undo()
        {
           // _PlayOver = false;
            if (source != null && !source.loop)
            {
				source.clip = null;
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnClipGUI(Rect rect){
            //if (root == null || parent == null || (parent != null && parent.actor == null))
            //{
            //    return;
            //}

            LoadAudioRes();
            EditorTools.DrawLoopedAudioTexture(rect, audioClip, length, clipOffset);
		}
        public override string GetAffectResPath()
        {
            //return base.GetAffectResPath();
            if (string.IsNullOrEmpty(_AuidoName) || string.IsNullOrEmpty(_AudioType))
            {
                audioClip = null;
                return "";
            }
            
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            return stringBuilder.Append(_AudioType).Append("/sound/").Append(_AuidoName).ToString();
        }
#endif
        public virtual void LoadAudioRes()
        {
            if (string.IsNullOrEmpty(_AuidoName) || string.IsNullOrEmpty(_AudioType))
            {
                audioClip = null;
                return;
            }
            if (audioClip == null || (audioClip != null && !audioClip.name.Equals(_AuidoName)))
                audioClip = C_Singleton<GameResMgr>.GetInstance().LoadResource<AudioClip>(string.Concat(_AuidoName ,".ogg"), _AudioType, "", string.Concat(_AudioType, "/"));
        }

	}
}