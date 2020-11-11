//#define  _run 

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Text;
using Assets.Scripts.C_Framework;
using QFramework;


using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.SceneManagement;

namespace Slate
{

	///Plays a series of cutscenes in order.
	public class CutsceneSequencePlayer : MonoBehaviour {
        public bool playOnStart = true;
		public List<Cutscene> cutscenes;
		public UnityEvent onFinish;
        public string fileName;
		protected int currentIndex;
        protected bool isPlaying;


        public bool C_DebugHelperMode = true;
        protected List<string> FileName =new List<string>();

        [Range(0,100)]
        public float TimeScale = 1f;
        public static Cutscene _CurrentCutScene;
        public int _InitPlayedCutscene = 5;//初始加载的镜头个数
        protected int _CurrentNeedLoadIndex = 0;//每次产生镜头的开始加载的编号
        protected int _LoadOnceCutsceneSUM = 1;//每次新增加载的镜头个数
        protected int _LoadAllCutsceneSUM = 0;//加载的镜头总个数

        protected GameObject _CutsceneNode;
        protected string _ModuleName;
        protected List<GameObject> _DirtyCutsceneList = new List<GameObject>();
        public bool StaticPlayQueMode = false;
        public static string CurrentPlayerName
        {
            get
            {
                if (_CurrentCutScene == null)
                {
                    return string.Empty;
                }
                return _CurrentCutScene.name;
            }

        }
        protected static string StoryName;
        protected StringBuilder _StringBuilder = new StringBuilder();
        protected string _CurrentMoudle;
        protected static bool _JumpFlag = false;
        void Start(){
            _JumpFlag = false;



            //埋点
            if (!string.IsNullOrEmpty(_CurrentMoudle))
            {


            }
            DoStart();
        }
        protected  string  getCurrentMoudleName()
        {
            string sceneName = this.gameObject.scene.name;
             
            return sceneName.Split('_')[0];
        }
        protected virtual void DoStart()
        {
          
            OpenStoryUI();

            _CutsceneNode = GameObject.FindGameObjectWithTag("CutScene");
            _CurrentCutScene = null;
            Time.timeScale = TimeScale;
#if UNITY_EDITOR
              C_DebugHelperMode = false;
            //QConsole.Instance.SetShowLogin(true);
            if (!C_DebugHelperMode)
            {
                C_DebugHelperMode = false;
                Time.timeScale = 1;
                StaticPlayQueMode = false;
                playOnStart = true;
            }
#endif
#if !UNITY_EDITOR || _run
        C_DebugHelperMode = false;
        Time.timeScale = 1;
        StaticPlayQueMode = false;
        playOnStart = true;
#endif

            DoPlay();
        }

        protected virtual void DoPlay()
        {
            if (playOnStart)
            {
                if (!C_DebugHelperMode)
                {
                    cutscenes.Clear();
                    if (StaticPlayQueMode)
                    {
                        InitCutScene();
                    }
                    else
                    {
                        InitCutsceneDynamic();
                    }
                }

                Play();
            }
        } 

        protected virtual void OpenStoryUI()
        {
            //暂停按钮
        }
        public void Update()
        {
#if UNITY_EDITOR
            Time.timeScale = TimeScale;
#endif
            if (_DirtyCutsceneList.Count > 0)
            {
                for (int i = 0; i < _DirtyCutsceneList.Count; i++)
                {
                    DestroyImmediate(_DirtyCutsceneList[i]);
                    _DirtyCutsceneList[i] = null;
                }
                _DirtyCutsceneList.Clear();
                Resources.UnloadUnusedAssets();
            }
            
        }
        public static bool isPause()
        {
            if (_CurrentCutScene != null)
            {
              return  _CurrentCutScene.isPaused;
            }
            else
            {
                C_DebugHelper.Log("_CurrentCutScene is null ,pause fail..");
            }
            return false;
        }
		 public static void PauseCurrentCutscene()
        {
            if (_CurrentCutScene!=null)
            {
                _CurrentCutScene.Pause();
            }
            else
            {
                C_DebugHelper.Log("_CurrentCutScene is null ,pause fail..");
            }
        }
        public static void ResumeCurrentCutscene()
        {
            if (_CurrentCutScene != null)
            {
                _CurrentCutScene.Resume();
            }
            else
            {
                C_DebugHelper.Log("_CurrentCutScene is null ,resume fail..");
            }
        }
        public static void StopCurrentCutscene()
        {
            if (_CurrentCutScene != null)
            {
                _CurrentCutScene.Stop();
                _CurrentCutScene = null;
            }
            else
            {
                C_DebugHelper.Log("_CurrentCutScene is null ,resume Stop..");
            }
        }
        
        
        public static void BackToMainScene()
        {
            _JumpFlag = true;


            StopCurrentCutscene();
            GameObject cutscenes = GameObject.FindGameObjectWithTag("CutScene");
            
            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("wk_scene_01", "Main", () => { Resources.UnloadUnusedAssets();});

//#if TEST_MAIN_MOUDLE
//            YBSceneLoadingMgr.Instance.LoadMulitSceneAsync("Main", "",()=>{ Resources.UnloadUnusedAssets(); });
//#else
//            YBSceneLoadingMgr.Instance.LoadScene("SpriteWindow");

//#endif
            DestroyObject(AudioManager.Instance);
            
        }
        public void InitFile()
        {
            string[] moudleName = fileName.Split('_');
            if (moudleName.Length <= 0)
            {
                C_DebugHelper.LogError("读取的配置文件没有_符号进行命名模块，需要检查");
                return;
            }
            StoryName = _ModuleName = moudleName[0];
            StringBuilder filePath = new StringBuilder();
            filePath.Append("SlatePlayer/").Append(_ModuleName).Append("/").Append(fileName);
            ReadFile(filePath.ToString());

        }

        public void InitCutsceneDynamic()
        {
            InitFile();
          //  _CurrentNeedLoadIndex = CutscenePrefabs.getCurCusceneIndex(_ModuleName);
            if (_CurrentNeedLoadIndex >= FileName.Count)
            {
                _CurrentNeedLoadIndex = 0;
            }
            _LoadAllCutsceneSUM = FileName.Count - _CurrentNeedLoadIndex;
           // GenerateCutsceneAsync(_InitPlayedCutscene);
            GenerateCutsceneAsync(_LoadAllCutsceneSUM);
        }
      
        void GenerateCutsceneAsync(int loadSum)
        {
            int sum = loadSum;
            //动态创建5个镜头
            for (int i = _CurrentNeedLoadIndex; i < _CurrentNeedLoadIndex + sum; i++)
            {
                //获取名字，创建cutscene，添加go对象为父节点
                if (i >= FileName.Count)
                {
                    continue;
                }
                bool loadRes = false;
                GameObject temp = null;
                if (_CutsceneNode != null)
                {
                    C_DebugHelper.Log("FileName[i]:"+ FileName[i]);
                    Transform tran = _CutsceneNode.transform.Find(FileName[i]);
                    if (tran != null)
                    {
                        temp = tran.gameObject;
                    }
                    if (temp == null)
                    {
                        loadRes = true;
                    }
                }
                else
                {
                    loadRes = true;
                }

                if (loadRes)
                {
                    _StringBuilder.Length = 0;
                    _StringBuilder.Append("story/").Append(_ModuleName).Append("/").Append("cutscene/").Append(FileName[i]);
                    C_DebugHelper.Log("_StringBuilder.ToString():" + _StringBuilder.ToString());
                    temp = C_Singleton<GameResMgr>.GetInstance().LoadResource<GameObject>(_StringBuilder.ToString(), true, false);
                    if (temp == null)
                    {
                        continue;
                    }
                }
                temp.transform.SetParent(_CutsceneNode.transform);
                Cutscene cutscene = temp.GetComponent<Cutscene>();
                if (cutscene == null)
                {
                    continue;
                }
                cutscenes.Add(cutscene);
            }
            if (sum > 0)
                _CurrentNeedLoadIndex += sum;
        }
      
        public void InitCutScene()
        {
           
            InitFile();
            for (int i = 0; i < FileName.Count; i++)
            {
                Transform temp = _CutsceneNode.transform.Find(FileName[i]);
                if (temp == null)
                {
                    C_DebugHelper.LogError(FileName[i] + "not find");
                    return;
                }
                Cutscene cut = temp.GetComponent<Cutscene>();
                if (cut == null)
                {
                    C_DebugHelper.LogError(FileName[i] + "not find cutscene");
                    return;
                }
                cutscenes.Add(cut);
            }
        }
        public void InitCutScene(int start,int end)
        {
            InitCutScene();
            GameObject go = GameObject.FindGameObjectWithTag("CutScene");
            int count = go.transform.childCount;
            for (int i = 0; i < FileName.Count; i++)
            {
                C_DebugHelper.Log(FileName[i]);
                Transform temp = go.transform.Find(FileName[i]);
                if (temp == null)
                {
                    C_DebugHelper.LogError(FileName[i] + "not find");
                    return;
                }
                Cutscene cut = temp.GetComponent<Cutscene>();
                if (cut == null)
                {
                    C_DebugHelper.LogError(FileName[i] + "not find cutscene");
                    return;
                }
                if (i >= start && i <= end)
                {
                    cutscenes.Add(cut);
                }
            }
        }
        public void InitCutScene(string start, string end)
        {
            bool Record = false;
            InitCutScene();

            GameObject go = GameObject.FindGameObjectWithTag("CutScene");
            int count = go.transform.childCount;
            for (int i = 0; i < FileName.Count; i++)
            {
                Transform temp = go.transform.Find(FileName[i]);
                if (temp == null)
                {
                    C_DebugHelper.LogError(FileName[i] + "not find");
                    return;
                }
                Cutscene cut = temp.GetComponent<Cutscene>();
                if (cut == null)
                {
                    C_DebugHelper.LogError(FileName[i] + "not find cutscene");
                    return;
                }
                if (FileName[i].Equals( start ) )
                {
                    Record = true;
                }
                if (Record)
                {
                    cutscenes.Add(cut);
                }
                if (FileName[i].Equals(end))
                {
                    Record = false;
                }
            }
        }
        public static bool IsCurrentStory(string name)
        {
            return (!string.IsNullOrEmpty(StoryName) && StoryName.Equals(name));
        }

        public void PlayCutsceneViaName(string cutsceneName)
        {
            for (int i=0;i < cutscenes.Count;i++)
            {
                if (cutscenes[i].name.Equals(cutsceneName))
                {
                    isPlaying = true;
                    _CurrentCutScene.OnStop -= MoveNext;
                    _CurrentCutScene.Stop();
                    currentIndex = i;
                    break;
                }
            }
            if (isPlaying)
            {
                MoveNext();
            }
        }
      
        public void ReadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                C_DebugHelper.LogError("文件是空，请检查");
                return;

            }
            TextAsset binAsset = Resources.Load(fileName) as TextAsset;
            if (binAsset == null )
            {
                C_DebugHelper.LogError(fileName + "读取可视化播放文件路径不存在");
                return;
            }
            if (binAsset.text == null)
            {
                C_DebugHelper.LogError(fileName + "读取可视化播放文件内容为空");
                return;
            }
            string content = binAsset.text.Replace("\r\n",string.Empty);
            FileName.Clear();
            //读取每一行的内容  
            string[] Info = content.Split(',');
            if (Info.Length <= 0)
            {
                C_DebugHelper.LogError(fileName + "读取可视化播放文件内容不存在，");
                return;
            }
            try
            {
                for (int i = 0; i < Info.Length; i++)
                {
                    if (!string.IsNullOrEmpty(Info[i]))
                    {
                        Info[i].Trim();
                        FileName.Add(Info[i]);
                    }
                }
            }
            catch (System.Exception e)
            {
                C_DebugHelper.LogError(" 文件名" + fileName + "错误信息" + e.Message);
            }
        }
        public void OnGUI()
        {
            if (GameConfig.LogState != 0)
            {
                if (_CurrentCutScene != null)
                    GUI.Label(new Rect(20, 60, 350, 60), "当前镜头名字：" + _CurrentCutScene.gameObject.name, QConsole.ConsoleFontGUIStyle);
            }
        }

#if UNITY_EDITOR
        public string ReadLoadingMsg(string startName, string endName, string matchContent = "")
        {
            cutscenes.Clear();
            InitCutScene(startName, startName);
            return Search(matchContent);

        }
        public string ReadLoadingMsg(int startIndex, int endIndex, string matchContent = "")
        {
            cutscenes.Clear();
            InitCutScene(startIndex, endIndex);
            return Search(matchContent);

        }
        //根据配置文本创建对象，并且判断是否有对象存在
        public string NewReadLoadingMsg(string key,string matchContent ="")
        {
            fileName = key;
            return ReadLoadingMsg(matchContent);
        }
        public string ReadLoadingMsg(string matchContent ="")
        {
            _CutsceneNode = GameObject.FindGameObjectWithTag("CutScene");
            _CurrentCutScene = null;
            //初始化所有场景镜头
            cutscenes.Clear();
            InitFile();
            _LoadAllCutsceneSUM  = FileName.Count;
            _CurrentNeedLoadIndex = 0;
            GenerateCutsceneAsync(_LoadAllCutsceneSUM);
            return Search(matchContent);

        }
        private string Search(string matchContent = "")
        {
            List<string> ResList = new List<string>();
            if (!string.IsNullOrEmpty(matchContent))
            {
                string[] data = matchContent.Split('\n');
                for (int id =0;id<data.Length;id++)
                {
                    ResList.Add(data[id]);
                }
            }
            string content = "";
            string mark = "PackagingResources";
            for (int i = 0; i < cutscenes.Count; i++)//所有剧情
            {
                Cutscene cutscene = cutscenes[i];//单个剧情
                if (cutscene != null)
                {
                    for (int ii = 0; ii < cutscene.groups.Count; ii++)//单个剧情的所有轨道
                    {
                        string[] fileName = cutscene.name.Split('_');
                        if (fileName.Length < 1)
                        {
                            C_DebugHelper.LogError(string.Format("'{0}'名字不规范 要求类似iuv_cam001 ", cutscene.name));
                            continue;
                        }
                        if (cutscene.name.Contains("(Clone)"))
                        {
                            int end = name.LastIndexOf("(Clone)");
                            cutscene.name = cutscene.name.Substring(0, (end == -1 ? cutscene.name.Length : end));
                        }
                        if (cutscene.name.Contains("(clone)"))
                        {
                            int end = name.LastIndexOf("(clone)");
                            cutscene.name = cutscene.name.Substring(0, (end == -1 ? cutscene.name.Length : end));
                        }
                        string dirpath = fileName[0] + "/prefabs/" + cutscene.name + "/" + cutscene.name;
                        ContainContent(dirpath, ResList);
                        
                        //收集cutscene
                        string cutscenepath = fileName[0] + "/cutscene/" + cutscene.name ;
                        ContainContent( cutscenepath, ResList);
                        
                        //初始化镜头的所有对象
                        cutscene.LoadAffectedResEditor();
                        //对象prefab
                        var actor = cutscene.groups[ii].actor;
                        if (actor == null)
                        {
                            C_DebugHelper.LogWarning("对象：" + dirpath + " actor is null");
                        }
                        if (actor != null && PrefabUtility.GetPrefabType(actor) == PrefabType.Prefab)
                        {
                            string tempActorName = AssetDatabase.GetAssetPath(actor);
                            tempActorName = tempActorName.Substring(tempActorName.IndexOf(mark) + mark.Length + 1);
                            string temp = C_String.DeleteExpandedName(tempActorName).ToLower();
                             ContainContent(temp, ResList);
                            
                        }
                        //所有的轨道上资源对象
                        List<CutsceneTrack> cutsceneTrack = cutscene.groups[ii].tracks;
                        if (cutsceneTrack != null)
                        {
                            for (int iii = 0; iii < cutsceneTrack.Count; iii++)
                            {
                                string path = cutsceneTrack[iii].GetAffectResPath();
                                string pathTemp = C_String.DeleteExpandedName(path).ToLower();
                                 ContainContent(pathTemp, ResList);
                                
                                List<ActionClip> _actionClips = cutsceneTrack[iii].actionClips;
                                for (int j = 0; j < _actionClips.Count; j++)
                                {
                                    string clipPath = _actionClips[j].GetAffectResPath();
                                    string clipPathTemp = C_String.DeleteExpandedName(clipPath).ToLower();
                                    ContainContent(clipPathTemp, ResList);
                                }
                            }
                        }

                    }
                }
            }
            //C_DebugHelper.Log("资源：+" + content);
            content = "";
            //清理重复的名字资源
            for (int i =0;i < ResList.Count;i++)
            {
                content += ResList[i] + "\n";
            }
            return content;
        }
        //查重
        public void ContainContent(string content, List<string> resDataList)
        {
            if (resDataList == null) return;

            bool contain = false;
            if (!string.IsNullOrEmpty(content))
            {
                //当前获取的资源,判断当前的资源是否有多个,|隔开
                string[] data = content.Split('|');
                if (data != null && data.Length > 0)
                {
                    contain = true;
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (string.IsNullOrEmpty(data[i]))
                        {
                            continue;
                        }
                        if (!resDataList.Contains(data[i]) && !string.IsNullOrEmpty(content))
                        {
                            //  result += content + "\n";
                            resDataList.Add(data[i]);
                        }

                    }
                }
            }
            if (!contain)
            {
                if (!resDataList.Contains(content) && !string.IsNullOrEmpty(content))
                {
                    //  result += content + "\n";
                    resDataList.Add(content);
                }
            }
        }
#endif

        public void Play(){
			
			if (isPlaying){
				C_DebugHelper.LogWarning("Sequence is already playing"+ gameObject);
				return;
			}

			if (cutscenes.Count == 0){
				C_DebugHelper.LogError("No Cutscenes provided"+ gameObject);
				return;
			}

			isPlaying = true;
			currentIndex = 0;
			MoveNext();
		}

		public void Stop(){
			if (isPlaying){
				isPlaying = false;
				cutscenes[currentIndex].Stop();
			}
		}
        public void Pause()
        {
            if (isPlaying)
            {
                _CurrentCutScene.OnStop -= MoveNext;
                _CurrentCutScene.Pause();
            }
        }
        public void StopAllCutscene()
        {
            if (isPlaying && _CurrentCutScene != null )
            {
                currentIndex = cutscenes.Count;
                isPlaying = false;
                _CurrentCutScene.OnStop -= MoveNext;
                _CurrentCutScene.Stop();
            }
        }
             
       protected virtual void DoClick()
        {
         
        }
	    protected virtual	void MoveNext(){
            if (_JumpFlag)//中途退出
            {
                isPlaying = false;
                onFinish.Invoke();
                return;
            }
            if (!string.IsNullOrEmpty(_CurrentMoudle)&& cutscenes != null && cutscenes.Count == 1 )
            {
            }
            if (!isPlaying || currentIndex >= cutscenes.Count)
            {
                
                isPlaying = false;
                onFinish.Invoke();
                return;
            }

            _CurrentCutScene = null;
           // DoClick();

            var cutscene = _CurrentCutScene = cutscenes[currentIndex];
            if (cutscene == null)
            {
                C_DebugHelper.LogError("Cutscene is null in Cutscene Sequencer"+gameObject);
                return;
            }
            if (!string.IsNullOrEmpty(_CurrentMoudle))
            {
            }
            cutscene.gameObject.SetActive(true);
            cutscene.isActive = false;
            cutscene.Play(() => {
                //埋点
              //  C_MonoSingleton<GameHelper>.GetInstance().StoryDataStatistics(Slate.CutsceneSequencePlayer.CurrentPlayerName);
                _CurrentCutScene = null;

                if (cutscenes != null && cutscenes.Count > 0)
                {
                    GameObject go = cutscenes[0].gameObject;
                    cutscenes.RemoveAt(0);
                    _DirtyCutsceneList.Add(go);
                    //StartCoroutine("ReleaseDirtyCutscene");
                }
                MoveNext();
                //GenerateCutsceneAsync(_LoadOnceCutsceneSUM);
            });

        }
        public IEnumerator ReleaseDirtyCutscene()
        {
            yield return null;
            if (_DirtyCutsceneList.Count > 0)
            {
                for (int i = 0; i < _DirtyCutsceneList.Count; i++)
                {
                    DestroyImmediate(_DirtyCutsceneList[i]);
                    _DirtyCutsceneList[i] = null;
                }
                _DirtyCutsceneList.Clear();
                Resources.UnloadUnusedAssets();
            }
        }

        public static GameObject Create(){
			var go = new GameObject("CutsceneSequencePlayer");
			go.AddComponent<CutsceneSequencePlayer>();
			return go;
		}
	}
}