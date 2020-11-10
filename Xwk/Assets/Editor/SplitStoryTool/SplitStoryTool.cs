using Slate;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerPrefsXBL
{

    [System.Serializable]
    class SerializedData
    {
        public bool autoKey = true;
        public bool magnetSnapping = true;
        public float trackListLeftMargin = 280f;
        public bool saveKey = true;
    }

    public static bool autoKey
    {

        get { return data.autoKey; }
        set { if (data.autoKey != value) { data.autoKey = value; Save(); } }
    }

    private static SerializedData _data;
    private static SerializedData data
    {
        get
        {
            if (_data == null)
            {
                _data = JsonUtility.FromJson<SerializedData>(EditorPrefs.GetString("xbl.editor"));
                if (_data == null)
                {
                    _data = new SerializedData();
                }
            }
            return _data;
        }
    }

    static void Save()
    {
        EditorPrefs.SetString("xbl.editor", JsonUtility.ToJson(data));
    }
}
public class SplitStoryTool : EditorWindow
{
    private float screenWidth
    {
        get { return Screen.width; }
    }

    private float screenHeight
    {
        get { return Screen.height; }
    }
    private const float TOOLBAR_HEIGHT = 18;  
    private const float TOP_MARGIN = 40;  

    Vector2 _ScrollPosition;
    Rect SavePathRect { get; set; }
    List<string> _SceneSavePathList = new List<string>();

    private string _DestionSceneName;
    private Object _DestionScene;
    private bool _IsOpen = false;
    float secs = 3.0f;
    float startVal = 0;
    float  progress = 0;
    private string _DestiionSceneName;
    private string _PlayCutsceneFilePath;
    private Scene _ActiveDestionScene;


    [MenuItem("工具/拼接剧情", false, 100)]
    static void DoSplitStoryTool()
    {
        EditorWindow.GetWindow<SplitStoryTool>("拼接剧情");
    }

    void OnGUI()
    {
        Rect rectVaule = new Rect(0,0,0,0);
        _ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition, false, true, GUILayout.MinHeight(200), GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        //第一步,创建拼接的场景
        FirstStep();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        //第二步
        SecondSetp();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        //第三步
        ThirthSetp();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        //第四步
        CreatePlayerCutscene();
        EditorUtility.SetDirty(this);
        EditorGUILayout.EndScrollView();
    }
    public void FirstStep()
    {
        EditorGUILayout.LabelField("第一步：创建拼接的场景");
        EditorGUILayout.BeginHorizontal("box");

        //SavePathRect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true));
       // _DestionScene =(Scene)EditorGUILayout.ObjectField(_DestionScene, typeof(Scene)) ;
        _DestionScene = (Object)EditorGUILayout.ObjectField( _DestionScene, typeof(Object),true);
        if (_DestionScene == null)
        {
            EditorGUILayout.HelpBox("先创建一个空的场景用来拼接剧情", MessageType.Warning);
        }
        if (_DestionScene != null )
        {
            _DestiionSceneName = AssetDatabase.GetAssetPath(_DestionScene);
        }
        //UtilityTools.DropToTextFiled(SavePathRect, ref _DestionSceneName);
        if (GUILayout.Button("创建或者打开"))
        {
            if(_DestionScene == null)
            {
                return;
            }
            InitProgress();
            _DestiionSceneName = AssetDatabase.GetAssetPath(_DestionScene);
            _ActiveDestionScene  = EditorSceneManager.OpenScene(_DestiionSceneName, OpenSceneMode.Additive);
            if (_ActiveDestionScene == null)
            {
                Debug.Log("创建");
            }
            //_IsOpen = true;

           // EditorSceneManager.sceneOpening -= OpenSceneOver;
           // EditorSceneManager.sceneOpening += OpenSceneOver;
        }
        EditorGUILayout.EndHorizontal();
    }
    public void CreatePlayerCutscene()
    {
        EditorGUILayout.LabelField("第四步：创建剧情运行配置文本");

        EditorGUILayout.BeginHorizontal("box");

     //   SavePathRect = ;
        //将上面的框作为文本输入框  
        _PlayCutsceneFilePath = EditorGUI.TextField(EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true)), _PlayCutsceneFilePath);
        UtilityTools.DropToTextFiled(SavePathRect, ref _PlayCutsceneFilePath);
        FileTools.GetFileNameWithoutExtionAndFullPath(_PlayCutsceneFilePath, ref _PlayCutsceneFilePath);
        if (GUILayout.Button("复制运行文本"))
        {
            if (_ActiveDestionScene == null )
            {
                _ActiveDestionScene = EditorSceneManager.OpenScene(_DestiionSceneName, OpenSceneMode.Additive);
            }
            
            EditorSceneManager.SetActiveScene(_ActiveDestionScene);
            GameObject player =GameObject.Find("CutsceneSequencePlayer");
            if (player == null)
            {
                player = new GameObject("CutsceneSequencePlayer");
            }
            CutsceneSequencePlayer playercutscene = player.GetComponent<CutsceneSequencePlayer>();
            if (playercutscene == null)
            {
                playercutscene = player.AddComponent<CutsceneSequencePlayer>();
            }
            playercutscene.fileName = _PlayCutsceneFilePath;
        }
        EditorGUILayout.EndHorizontal();
    }
    public void InitProgress()
    {
        startVal = (float) EditorApplication.timeSinceStartup;
        progress = 0;
    }
    public void ShowOpenScene()
    {
        if (_IsOpen)
        {
            if (progress < secs)

                EditorUtility.DisplayProgressBar(

                    "正在打开场景",

                    "进度",

                    progress / secs);

            else
                EditorUtility.ClearProgressBar();

            progress = (float)EditorApplication.timeSinceStartup - startVal;
            return;
        }
    }
    //public void OpenSceneOver(string path, OpenSceneMode mode)
    //{
    //    _IsOpen = false;
       
    //    EditorUtility.ClearProgressBar();
    //}
    public void SecondSetp()
    {
      //  GUI.Label(new Rect(0f, 0f, screenWidth, 30), "第二步：添加所有美术配置的场景");
        EditorGUILayout.LabelField("第二步：添加所有美术配置的场景");

        //将上面的框作为文本输入框  
        if (_SceneSavePathList != null && _SceneSavePathList.Count > 0)
        {
            //获得一个长300的框  
            for (int i = 0; i < _SceneSavePathList.Count; i++)
            {
                SetSceneSavePath(i);
            }
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("增加"))
        {
            _SceneSavePathList.Add("");
        }
        if (GUILayout.Button("删除"))
        {
            if ((_SceneSavePathList != null && _SceneSavePathList.Count <= 0) || _SceneSavePathList == null)
            {
                return;
            }
            _SceneSavePathList.Remove(_SceneSavePathList[_SceneSavePathList.Count - 1]);
        }
        EditorGUILayout.EndHorizontal();
    }
    public void ThirthSetp()
    {
        EditorGUILayout.LabelField("第三步：关闭美术配置的场景");
        if (GUILayout.Button("关闭美术拼接的所有场景"))
        {
            for (int i = 0; i < _SceneSavePathList.Count; i++)
            {
                for (int j = 0; j < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(j);
                    if (scene.name.Equals(_SceneSavePathList[i]))
                    {
                        EditorSceneManager.CloseScene(scene, true);
                    }
                }
            }
        }

    }
    public void SetSceneSavePath(int i)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.textArea);
        EditorGUILayout.LabelField("场景配置文件的路径");
        //获得一个长300的框  
        SavePathRect = EditorGUILayout.GetControlRect(true,GUILayout.ExpandWidth(true));
        //将上面的框作为文本输入框  
        string vaule = EditorGUI.TextField(SavePathRect, _SceneSavePathList[i]).ToString();
        UtilityTools.DropToTextFiled(SavePathRect, ref vaule);
        _SceneSavePathList[i] = vaule;
        if (GUILayout.Button("拼接"))
        {
            Debug.Log(_SceneSavePathList[i]);
            //打开对应场景，进行拼接
            SplitJointCutscene.ReplaceArt("CutSceneArt", "CutScene", _SceneSavePathList[i], _DestiionSceneName, null, "CutScene");
            SplitJointCutscene.ReplaceArt("PathArt", "Path", _SceneSavePathList[i], _DestiionSceneName, null, "CutScenePath");
        }
        EditorGUILayout.EndHorizontal();
    }
    void show(Rect topLeftRect,string word)
    {
        var autoKeyRect = new Rect(topLeftRect.xMin + 10, topLeftRect.yMin + 4, 32, 32);
        AddCursorRect(autoKeyRect, MouseCursor.Link);
        GUI.backgroundColor = PlayerPrefsXBL.autoKey ? new Color(0, 0, 0, 0.3f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
        GUI.color = PlayerPrefsXBL.autoKey ? new Color(1, 0.4f, 0.4f) : Color.white;
        if (GUI.Button(autoKeyRect, word))
        {
            PlayerPrefsXBL.autoKey = !PlayerPrefsXBL.autoKey;
            ShowNotification(new GUIContent(string.Format("AutoKey {0}", PlayerPrefsXBL.autoKey ? "Enabled" : "Disabled")));
        }
    }

    void AddCursorRect(Rect rect, MouseCursor type)
    {
        EditorGUIUtility.AddCursorRect(rect, type);
    }

}
