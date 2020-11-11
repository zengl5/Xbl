using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
 
    public static class Utility
    {
        public static string m_MoudlePath;
        public static string m_ScenemName;
        public static string m_PlantThemeUIPath = "MainScene/PlantThemeUI/";
        public static string m_PlantThemeDataPath = "/ResourceOut/MainScene/PlantThemeData.txt";
        public static string m_PngExtension = ".png";
        public static string GetResDirPath()
        {
            return m_MoudlePath + "/" + m_ScenemName + "/";
        }
        public static string GetPlaformName()
        {
            string filePath = string.Empty;
#if UNITY_EDITOR
            filePath = Application.dataPath;
#elif UNITY_IPHONE
	  //string filepath = Application.dataPath +"/Raw"+"/my.xml";
    filePath= Application.dataPath;
#elif UNITY_ANDROID
	  //string filepath = "jar:file://" + Application.dataPath + "!/assets/"+"/my.xml;
       //string path = Application.dataPath + Utility.m_PlantThemeUIPath + imgName + Utility.m_PngExtension;
      // string path = Application.persistentDataPath + Utility.m_PlantThemeUIPath + imgName + Utility.m_PngExtension;
        filePath = Application.persistentDataPath;
#endif
            return filePath;
        }
        public static string GetDataPath()
        {
            string filePath = string.Empty;
#if UNITY_EDITOR
            filePath = Application.dataPath + "/";
#elif UNITY_IPHONE
    filePath= Application.dataPath+ "/";
#elif UNITY_ANDROID
        filePath = Application.persistentDataPath+ "/";
#endif
            return filePath;
        }
        public static string GetStreamingAssets()
        {
            string filePath = string.Empty;
#if UNITY_ANDROID && !UNITY_EDITOR
        filePath ="jar:file://" + Application.dataPath + "!/assets/" ;  
#elif UNITY_IPHONE && !UNITY_EDITOR
        filePath =Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            filePath = "file://" + Application.dataPath + "/StreamingAssets" + "/";
#else
       filePath = string.Empty;  
#endif
            return filePath;
        }
        public enum Direction
        {
            _Right,
            _Left,
        }

        public static T GetComponent<T>(this Component component, bool create) where T : Component
        {
            T local = component.GetComponent(typeof(T)) as T;
            if (create && (local == null))
            {
                local = component.gameObject.AddComponent(typeof(T)) as T;
            }
            return local;
        }

        public static T GetComponent<T>(this GameObject go, bool create) where T : Component
        {
            T component = go.GetComponent(typeof(T)) as T;
            if (create && (component == null))
            {
                component = go.AddComponent(typeof(T)) as T;
            }
            return component;
        }
        public static Vector3 StringToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');
            if (sArray == null)
            {
                C_DebugHelper.LogError("sVector = " + sVector + "数组没有用,隔开");
                return Vector3.zero;
            }
            if (sArray != null && sArray.Length != 3)
            {
                C_DebugHelper.LogError("sVector = " + sVector + "数组数据长度不够3");
                return Vector3.zero;
            }

            // store as a Vector3
            Vector3 result = new Vector3(float.Parse(sArray[0].ToString()), float.Parse(sArray[1].ToString()), float.Parse(sArray[2].ToString()));
            return result;
        }

        public static GUIStyle GetTitleFontStyle()
        {

            GUIStyle bb = new GUIStyle();
            bb.normal.background = null; //这是设置背景填充的  
            bb.normal.textColor = new Color(1, 0, 0);   //设置字体颜色的  
            bb.fontSize = 16; //当然，这是字体颜色  
            return bb;


        }
        /// <summary>查找子物体（递归查找）</summary>
        /// <param name="trans">父物体</param>
        /// <param name="goName">子物体的名称</param>
        /// <returns>返回找到子物体的Transform组件</returns>
        public static Transform FindChild(Transform trans, string goName)
        {
            Transform child = trans.Find(goName);
            if (child != null)
                return child;
            Transform go = null;
            for (int i = 0; i < trans.childCount; i++)
            {
                child = trans.GetChild(i);
                go = FindChild(child, goName);
                if (go != null)
                    return go;
            }
            return null;
        }
        public static string CutCloneNameTail(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "";

            }
            else
            {
                if (name.Contains("(Clone)"))
                {
                    name = name.Substring(0, name.IndexOf("(Clone)"));
                }
            }
            return name;
        }
        public static bool SetTransformLayer(Transform trans, int layer)
        {
            if (trans == null)
            {
                return false;
            }
            foreach (Transform tran in trans.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = layer;//更改物体的Layer层  
            }
            return true;
        }
        public static void SetVisible(this Transform transform, bool visible)
        {
            transform.gameObject.SetActive(visible);
        }
        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            while (copyList.Count > 0)
            {
                //随机将a中序号为index的元素作为b中的第一个元素放入b中
                int index = Random.Range(0, copyList.Count - 1);
                //检测是否重复，保险起见
                if (!outputList.Contains(copyList[index]))
                {
                    //若b中还没有此元素，添加到b中
                    outputList.Add(copyList[index]);
                    //成功添加后，将此元素从a中移除，避免重复取值
                    copyList.Remove(copyList[index]);
                }
            }
            return outputList;
        }

        public static void SetRectTransformSize(this RectTransform trans, Vector2 newSize)
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }
        public static Transform getChildViaName(this Transform obj, string name)
        {
            Transform[] grandFa = obj.transform.GetComponentsInChildren<Transform>();
            foreach (Transform child in grandFa)
            {
                if (child != null && child.name.Equals(name))
                {
                    return child;
                }
            }
            //Transform[] grandFa = obj.transform.GetComponentsInChildren<Transform>();
            //for (int i = 0; i < grandFa.Length; i++)
            //{
            //    if (grandFa[i] != null && grandFa[i].name.Equals(name))
            //    {
            //        return grandFa[i];
            //    }
            //}

            return null;
        }
        public static void SetMainScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                C_DebugHelper.LogError("mainScenName is null");
                return;
            }
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (sceneName.Equals(SceneManager.GetSceneAt(i).name))
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(i));
                    break;
                }
            }
        }
        public static Vector3 GetCurrentPlatformClickPosition(Camera camera)
        {
            Vector3 clickPosition = Vector3.zero;

            if (Application.isMobilePlatform)
            {//current platform is mobile
                if (Input.touchCount != 0)
                {
                    Touch touch = Input.GetTouch(0);
                    clickPosition = touch.position;
                }
            }
            else
            {//others
                clickPosition = Input.mousePosition;
            }
            if (camera != null)
            {
                clickPosition = camera.ScreenToWorldPoint(clickPosition);//get click position in the world space
                clickPosition.z = 0;
            }

            return clickPosition;
        }
        public static Bounds getTextBounds(GameObject p)
        {
            if (p == null)
            {
                return new Bounds();
            }
            Bounds tempBounds = new Bounds();
            Renderer[] renderers = p.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer each = renderers[i];

                if (i == 0)
                {
                    tempBounds = each.bounds;
                }
                else
                {
                    tempBounds.Encapsulate(each.bounds);
                }

            }
            return tempBounds;
        }

        public static T GetAddComponent<T>(this GameObject go) where T : Component
        {
            return GetAddComponent(go, typeof(T)) as T;
        }

        public static T GetAddComponent<T>(this Component comp) where T : Component
        {
            return GetAddComponent(comp.gameObject, typeof(T)) as T;
        }

        public static Component GetAddComponent(this GameObject go, System.Type type)
        {
            var result = go.GetComponent(type);
            if (result == null)
            {
                result = go.AddComponent(type);
            }
            return result;


        }

        public static Camera FetchCamera(string name)
        {
            Camera cam = null;
            Camera[] cameraArray = GameObject.FindObjectsOfType<Camera>();
            if (cameraArray == null)
            {
                return null;
            }
            int index = 0;
            for (int i = 0; i < cameraArray.Length; i++)
            {
                if (cameraArray[i] != null && !string.IsNullOrEmpty(cameraArray[i].name) && cameraArray[i].name.ToLower().Contains(name.ToLower()))
                {
                    index = i;
                    break;
                }
            }
            cam = cameraArray[index];//TODO 总会返回一个相机，待修改
            return cam;
        }
        public static Slate.Cutscene FindCurrentCutscene()
        {
            //Slate.Cutscene cutscene = null;
            //Slate.CutsceneSequencePlayer cutsceneSequencePlayer = FindCutsceneSequencePlayer();
            //if (cutsceneSequencePlayer != null)
            //{
            //    cutscene = cutsceneSequencePlayer._CurrentCutScene;
            //}

            return Slate.CutsceneSequencePlayer._CurrentCutScene;
        }
        public static void getCutsceneState()
        {
            if (Utility.FindCurrentCutscene().isPaused)
            {
                C_DebugHelper.Log("current cus state: Utility.FindCurrentCutscene().isPaused");
            }
            else
            {
                C_DebugHelper.Log("current cus state: Utility.FindCurrentCutscene().play");
            }
        }

        public static Slate.CutsceneSequencePlayer FindCutsceneSequencePlayer()
        {
            GameObject player = GameObject.Find("CutsceneSequencePlayer");
            if (player == null)
            {
                C_DebugHelper.Log("CutsceneSequencePlayer is null..");
                return null;

            }
            Slate.CutsceneSequencePlayer cutsceneSequencePlayer = player.GetComponent<Slate.CutsceneSequencePlayer>();
            if (cutsceneSequencePlayer == null)
            {
                C_DebugHelper.Log("CutsceneSequencePlayer component is null..");
                return null;
            }
            return cutsceneSequencePlayer;
        }

        public static void SetMeshrenderInfo(Transform g, bool on = true)
        {
            if (g == null)
            {
                return;
            }
            SkinnedMeshRenderer meshRenderer = g.GetComponent<SkinnedMeshRenderer>();
            if (meshRenderer != null)
            {
                if (on)
                {
                    meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                else
                {
                    meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                meshRenderer.receiveShadows = on;
            }
            for (int i = 0; i < g.transform.childCount; i++)
            {
                Transform child = g.transform.GetChild(i);
                SetMeshrenderInfo(child, on);
            }
        }

        public static void SetRenderVisible(this Transform transform, bool visible)
        {
            if (transform == null)
            {
                return;
            }
            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
            int length = renderers.Length;
            for (int i = 0; i < length; i++)
            {
                if (renderers[i] != null)
                {
                    renderers[i].enabled = visible;
                }
            }
        }

  //  private int[] weight = new int[4] { 50， 25， 15， 10 };

    //返回值数组下标，和权重比一一对应即可。
    public static int GetRandPersonalityType(int[] array, int _total)
    {
        int rand = Random.Range(1, _total + 1);
        int tmp = 0;

        for (int i = 0; i < array.Length; i++)
        {
            tmp += array[i];
            if (rand < tmp)
            {
                return i;
            }
        }
        return 0;
    }
    public static void RandListData<T>(List<T> dataList)
    {
        if (dataList==null)
        {
            return;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            T temp = dataList[i];
            int randomIndex = Random.Range(0, dataList.Count);
            dataList[i] = dataList[randomIndex];
            dataList[randomIndex] = temp;
        }
    }
    public static void DisableAnylitics()
    {
        return;
        UnityEngine.Analytics.Analytics.enabled = false;
        UnityEngine.Analytics.Analytics.deviceStatsEnabled = false;
      //  UnityEngine.Analytics.Analytics.initializeOnStartup = false;
        UnityEngine.Analytics.Analytics.limitUserTracking = false;
        UnityEngine.Analytics.PerformanceReporting.enabled = false;
    }
    public static byte[] CaptureScreenShot(Camera shotCamera, Rect rect, string filename)
    {
        RenderTexture renderTexture = new RenderTexture((int)rect.width, (int)rect.height, 0);
        shotCamera.targetTexture = renderTexture;
        shotCamera.Render();
        RenderTexture.active = renderTexture;
        // 创建一个纹理
        Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        // 读取内容到纹理图片中
        texture.ReadPixels(rect, 0, 0);
        // 保存前面对纹理的修改
        texture.Apply();
        shotCamera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(renderTexture);

        // 编码纹理为PNG格式
        byte[] bytes = texture.EncodeToPNG();
        // 销毁没用的图片纹理
        GameObject.Destroy(texture);
        return bytes;
    }
 
    public static IEnumerator SaveScreenShot(Camera shotCamera, Rect rect, string filename,int saveType,bool closeCamera =false)
    {

        yield return new WaitForEndOfFrame();
     //   shotCamera.gameObject.SetActive(true);
      //  byte[] bytes =  CaptureScreenShot(shotCamera, rect, filename);
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            try
            {
                string base64String = System.Convert.ToBase64String(bytes);
                C_DebugHelper.Log("获取当前图片base64长度为---" + base64String.Length);
                C_DebugHelper.Log("获取base64String---" + base64String);

                if (saveType == 1)
                {
                    GameHelper.Instance.AddToDCIM(ref base64String);
                }else if(saveType == 2)
                {
                    GameHelper.Instance.ShareImgBase64Wechat(base64String,1);//朋友圈
                }
                else
                {
                    GameHelper.Instance.ShareImgBase64Wechat(base64String, 0);//好友
                }
             
            }
            catch (System.Exception e)
            {
                C_DebugHelper.LogError("ImgToBase64String 转换失败:" + e.Message);
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            string base64String = System.Convert.ToBase64String(bytes);
            C_DebugHelper.Log("获取base64String---" + base64String);

            string path = string.Concat(Application.dataPath, "/../capture/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(string.Concat(path,"/", filename), bytes);
        }
    }
   
}
 
