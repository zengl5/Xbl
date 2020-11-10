using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine.SceneManagement;
using Assets.Scripts.C_Framework;

public class SplitJointCutscene : MonoBehaviour {
    private static SplitJointCutsceneModel model = new SplitJointCutsceneModel();
    private static Queue<Scene> _DirtyScene = new Queue<Scene>();
    //[MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Story")]
    //static void OpenSceneStory_Stroy()
    //{
    //    SplitJointCutsceneModel.OpenSceneStory_Story();
    //}
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy/iuv_Stroy_lm")]
    static void OpenSceneStory_Story_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story_lm");
    }
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy/iuv_Stroy_xh")]
    static void OpenSceneStory_Story_xh()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story_xh");
    }
     [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy/iuv_Stroy_yt")]
    static void OpenSceneStory_Story_yt()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story_yt");
    }
     #region 25-27镜头
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy25-27/iuv_Stroy_xh")]
    static void OpenSceneStory_Stroy25_27_xh()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story25_27_xh");
    }
     #endregion
    #region 31-39镜头
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy31-39/iuv_Stroy_lm")]
    static void OpenSceneStory_story31_39_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story31_39_lm");
    }
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy31-39/iuv_Stroy_xh")]
    static void OpenSceneStory_story31_39_xh()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story31_39_xh");

    }
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy31-39/iuv_Stroy_yt")]
    static void OpenSceneStory_story31_39_yt()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story31_39_yt");
    }
    #endregion

    #region 40镜头
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy40/iuv_Stroy_yt")]
    static void OpenSceneStory_story40_yt()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story40_yt");
    }
    #endregion
    #region 43镜头 iuv_Cam50
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy43/iuv_Stroy_lm")]
    static void OpenSceneStory_story43_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story43_lm");
    }
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy43/iuv_Stroy_xh")]
    static void OpenSceneStory_story43_xh()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story43_xh");
    }
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy43/iuv_Stroy_yt")]
    static void OpenSceneStory_story43_yt()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story43_yt");
    }
    #endregion

    

    #region 55镜头
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy55/iuv_Stroy_lm")]
    static void OpenSceneStory_story55_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story55_lm");
    }
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy55/iuv_Stroy_xh")]
    static void OpenSceneStory_story55_xh()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story55_xh");
    }
    [MenuItem("工具/自动拼接美术制作的多份剧本/iuv/iuv_Stroy55/iuv_Stroy_yt")]
    static void OpenSceneStory_story55_yt()
    {
        SplitJointCutsceneModel.OpenSceneStory("iuv_story55_yt");
    }
    #endregion

    #region aoe 1-22镜头
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story1-19/aoe_Stroy_xh")]
    static void OpenSceneAoe_story1_22()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story1-19_xh","aoe");
    }
    #endregion
    //aoe_story20-22_xh
    #region aoe_story20-22镜头  xh
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story20-22/aoe_Stroy_xh")]
    static void OpenSceneAoe_story74_83_xh()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story20-22_xh", "aoe");
    }
    #endregion
    #region aoe 25-28镜头
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story25-28/aoe_Stroy_xh")]
    static void OpenSceneAoe_story25_28()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story25-28_xh", "aoe");
    }
    #endregion
    #region aoe 29-40镜头
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story29-40/aoe_Stroy_lm")]
    static void OpenSceneAoe_story25_40()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story29-40_lm", "aoe");
    }
    #endregion
   
    //#region aoe 29-40镜头 xh
    //[MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story29-40/aoe_Stroy_xh")]
    //static void OpenSceneAoe_story25_40_xh()
    //{
    //    SplitJointCutsceneModel.OpenSceneStory("aoe_story25-40_xh", "aoe");
    //}
    //#endregion
    #region aoe 47-69镜头 lm
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story47-64/aoe_Stroy_lm")]
    static void OpenSceneAoe_story47_69_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story47-64_lm", "aoe");
    }
    #endregion
    #region aoe 47-64镜头 yt
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story47-64/aoe_Stroy_yt")]
    static void OpenSceneAoe_story47_69_yt()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story47-64_yt", "aoe");
    }
    #endregion
    #region aoe 65-69镜头 xh
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story65-69/aoe_Stroy_xh")]
    static void OpenSceneAoe_story65_69_xh()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story65-69_xh", "aoe");
    }
    #endregion
    #region aoe 65-69镜头 xh
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story65-69/aoe_Stroy_lm")]
    static void OpenSceneAoe_story65_69_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story65-69_lm", "aoe");
    }
    #endregion
    #region aoe 71-72镜头 xh
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story71-72/aoe_Stroy_xh")]
    static void OpenSceneAoe_story71_xh()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story71-72_xh", "aoe");
    }
    #endregion
    #region aoe 71-72镜头 xh
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story71-72/aoe_Stroy_lm")]
    static void OpenSceneAoe_story72_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story71-72_lm", "aoe");
    }
    #endregion
    #region aoe_story74-77镜头  lm 
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story74-76/aoe_Stroy_lm")]
    static void OpenSceneAoe_story74_77_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story74-76_lm", "aoe");
    }
    #endregion
    #region aoe_story78-83镜头  lm 
    [MenuItem("工具/自动拼接美术制作的多份剧本/aoe/aoe_story77-83/aoe_Stroy_lm")]
    static void OpenSceneAoe_story78_83_lm()
    {
        SplitJointCutsceneModel.OpenSceneStory("aoe_story77-83_lm", "aoe");
    }
    #endregion
   
    void OnGUI() { }

    public static void ReplaceArt(string src, string des, string srcSceneName, string desSceneName, List<string> replaceList, string tag)
    {
        //if (replaceList == null)
        //{
        //    return;
        //}
        Scene desScene = EditorSceneManager.OpenScene(desSceneName, OpenSceneMode.Additive);
        //打开拷贝的场景对象
        Scene srcScene = EditorSceneManager.OpenScene(srcSceneName, OpenSceneMode.Additive);//被拷贝的场景对象
        SceneManager.SetActiveScene(srcScene);
        //查到所有需要把被拷贝的被对象
        GameObject obj = GameObject.Find(src);//查找到cutscene
        if (obj == null)
        {
            Debug.LogErrorFormat("{0} is not exit ", src);
            return;
        }

        //移动不保存
        EditorSceneManager.MoveGameObjectToScene(obj, desScene);//整体移动到拼接场景
                                                                // EditorSceneManager.CloseScene(srcScene, true);
                                                                // EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        //替换存在的剧本对象和路径对象
        desScene = EditorSceneManager.OpenScene(desSceneName, OpenSceneMode.Additive);
        SceneManager.SetActiveScene(desScene);
        GameObject desObj = GameObject.Find(des);//找到path或者是cutscene
        if (desObj == null)
        {
            desObj = new GameObject(des);
            if (!string.IsNullOrEmpty(tag))
                desObj.gameObject.tag = tag;

            Debug.LogWarningFormat("{0} is not exit {1} object", desScene.name, desObj.gameObject.name);
        }

        GameObject source = GameObject.Find(src);//找到pathart或者是cutsceneart
        if (source == null)
        {
            Debug.LogErrorFormat("{0} is not exit {1} object", desScene.name, src);
            return;
        }
        List<Transform> _SourceList = new List<Transform>();

        //查找需要被替换场景的所有对象 
        for (int i = 0; i < source.transform.childCount; i++)
        {
            //获取需要被替换的对象名字
            Transform child = source.transform.GetChild(i);
            child.gameObject.SetActive(true);

            if ((replaceList != null && ListContainItem(child.name, replaceList)) || replaceList == null)
            {
                //判断当前的对象是在需要替换场景的子节点中，是则删除
                Transform temp = hasSameChild(desObj.transform, child.name);
                if (temp != null)
                {
                    DestroyImmediate(temp.gameObject);
                }
                //美术制作的对象保留
                _SourceList.Add(child);
            }
        }
        //拷贝source下的所有子对象到需要拼接场景的父节点
        //while (source.transform.childCount > 0)
        for (int i = 0; i < _SourceList.Count; i++)
        {
            Transform child = _SourceList[i];
            Transform temp = hasSameChild(desObj.transform, child.name);
            if (temp != null)
            {
                child.SetParent(desObj.transform);
            }
            else
            {
                if (child == null)
                {
                    continue;
                }
                child.SetParent(desObj.transform);
            }
        }

        DestroyImmediate(source.gameObject);

       //  EditorSceneManager.CloseScene(srcScene, false);
         _DirtyScene.Enqueue(srcScene);
    }
    static bool ListContainItem(string itemName, List<string> list)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogErrorFormat(" {0}itemname 的名字为空", itemName);
            return false;
        }
        if (list == null)
        {
            Debug.LogErrorFormat(" {0}队列为空", list);
            return false;
        }
        for (int i = 0; i < list.Count;i++ )
        {
            if (itemName.Equals(list[i]))
            {
                return true;
            }
        }
        return false;
    }
    static Transform hasSameChild(Transform parent, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogErrorFormat(" {0}传入的子节点名字有错误", name);
            return null;
        }
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogErrorFormat(" {0}对应的art子对象为空", parent);
            return null;
        }
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name.Equals(name))
            {
                return parent.transform.GetChild(i);
            }
        }
        return null;
    }
}
