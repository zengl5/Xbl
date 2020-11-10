using UnityEngine;
using UnityEditor;

/// <summary>
/// 在Editor Mode中预览动画
/// </summary>
public class AnimationPreview : EditorWindow
{
    //含有动画的GameObject 
    GameObject animationHolder; 
    //指定播放某一动画
    string specifiedAnimation;
    //是否循环播放
    bool loop;

    //支持播放六个动画序列，可自行添加
    enum AnimationIndex
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six
    }

    private AnimationIndex animationIndex = AnimationIndex.One;
    private Animation ani;
    private string animationName;


    [MenuItem("Custom/Preview Animation")]
    static void AddWindow()
    {
        Rect wr = new Rect(0, 0, 900, 900);
        AnimationPreview window = (AnimationPreview)EditorWindow.GetWindowWithRect(typeof(AnimationPreview), wr, true, "Animation Player");
        window.Show();
    }

    void OnGUI()
    {
        animationHolder = EditorGUILayout.ObjectField(new GUIContent("Select object", ""), animationHolder, typeof(GameObject), true) as GameObject;
        specifiedAnimation = EditorGUILayout.TextField(new GUIContent("Specified animation", ""), specifiedAnimation);
        animationName = EditorGUILayout.TextArea(animationName, GUILayout.Height(80));
        loop = EditorGUILayout.Toggle(new GUIContent("Loop", ""), loop);

        if (GUILayout.Button("Play Animation"))
		{
            PlayAnimation();
		}

        if (GUILayout.Button("Play Animation Order"))
        {
            PlayOrder();
        }
    }

    double previewStartTime;
    AnimationClip clip;
    double timeElapsed;
   

    void PlayAnimation()
    {
        //if (animationHolder == null)
        animationHolder = Selection.activeGameObject;

        ani = animationHolder.GetComponent<Animation>();
        if (!string.IsNullOrEmpty(specifiedAnimation))
            clip = ani.GetClip(specifiedAnimation);
        else
            clip = ani.GetClip(ani.clip.name);

        previewStartTime = EditorApplication.timeSinceStartup;
        EditorApplication.update = null;
        EditorApplication.update += DoPlay;

    }

    AnimationClip[] animationList;
    //播放动画序列
    void PlayOrder()
    {
        //if (animationHolder == null)
        animationHolder = Selection.activeGameObject;

        ani = animationHolder.GetComponent<Animation>();
        
        string[] names = animationName.Trim().Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        if (names.Length == 0) return;

        animationList = new AnimationClip[names.Length];
        int i = 0;
        foreach (string name in names)
        {
            animationList[i] = ani.GetClip(name);
            i++;
        }

        clip = animationList[0];
        previewStartTime = EditorApplication.timeSinceStartup;
        EditorApplication.update = null;
        EditorApplication.update += DoPlayOrder;

        animationIndex = AnimationIndex.One;
    }

    

    void DoPlay()
    {
        timeElapsed = EditorApplication.timeSinceStartup - previewStartTime;
        
        clip.SampleAnimation(ani.gameObject, (float)timeElapsed);

        if (loop)
        {
            if ((float)timeElapsed >= clip.length)
            {
                previewStartTime = EditorApplication.timeSinceStartup;
            }
        }
    }

    void DoPlayOrder()
    {
        timeElapsed = EditorApplication.timeSinceStartup - previewStartTime;
        
        switch (animationList.Length)
        {
            case 2:
                if (timeElapsed < animationList[0].length)
                {
                    animationIndex = AnimationIndex.One;
                }
                else if (timeElapsed < animationList[0].length + animationList[1].length)
                {
                    animationIndex = AnimationIndex.Two;
                }

                break;
            case 3:
                if (timeElapsed < animationList[0].length)
                    animationIndex = AnimationIndex.One;
                else if (timeElapsed < animationList[0].length + animationList[1].length)
                    animationIndex = AnimationIndex.Two;
                else
                    animationIndex = AnimationIndex.Three;
                break;
            case 4:
                if (timeElapsed < animationList[0].length)
                    animationIndex = AnimationIndex.One;
                else if (timeElapsed < animationList[0].length + animationList[1].length)
                    animationIndex = AnimationIndex.Two;
                else if (timeElapsed < animationList[0].length + animationList[1].length + animationList[2].length)
                    animationIndex = AnimationIndex.Three;
                else
                    animationIndex = AnimationIndex.Four;
                break;
            case 5:
                break;
            case 6:
                break;
            default:
                break;
        }

        switch (animationIndex)
        {
            case AnimationIndex.One:
                animationList[0].SampleAnimation(ani.gameObject, (float)timeElapsed);
                break;
            case AnimationIndex.Two:
                animationList[1].SampleAnimation(ani.gameObject, (float)timeElapsed - animationList[0].length);
                break;
            case AnimationIndex.Three:
                animationList[1].SampleAnimation(ani.gameObject, (float)timeElapsed - animationList[1].length - animationList[0].length);
                break;
            case AnimationIndex.Four:
                animationList[1].SampleAnimation(ani.gameObject, (float)timeElapsed - animationList[1].length - animationList[0].length - animationList[2].length);
                break;
            case AnimationIndex.Five:
                break;
            case AnimationIndex.Six:
                break;
            default :
                break;
        }
    }


    void OnDestroy()
    {
        if (ani)
        {
            EditorApplication.update = null;
            ani.clip.SampleAnimation(ani.gameObject, 0);
        }
            
    }

}
