using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AddAniamtiroController  {
    /****
     * 点击物体，为物体添加动画控制器，并生成prefab。
     * 注意添加的动画是按照拖进去的顺序依次播放，所以要注意拖入的顺序
     * 
     * 2018-4-10 黄志龙
     */
    private static string _LoadAnimatorCtrlPath = "";
    private static AnimatorController _CurrentAnimCtrl;
    //添加菜单
    [MenuItem(@"工具/配置对象的动画编辑器AniamtorController")]
    public static void CreatAnimatorController()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
        for (int i = 0; i < transforms.Length; i++)
        {
            AnimatorController animatorController = null;
            string path = Application.dataPath + "/FBX/" + transforms[i].name + ".controller";
            if (transforms[i].GetComponent<Animator>() == null)
            {
                transforms[i].gameObject.AddComponent<Animator>();
            }
            
            string directory = "Assets/FBX/AnimatorController/";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string fileName = directory + transforms[i].name + "_AnimatorController.controller";
            if (FileTools.IsFileExited(fileName))
            {
                _LoadAnimatorCtrlPath = Application.dataPath + "/FBX/AnimatorController/" + transforms[i].name + "_AnimatorController.controller";
                MessageBoxEditor.ShowSaveFileBoxComplex("创建动画控制器", fileName + "存在,是否覆盖", "不覆盖，直接使用","新建", "连线",()=>{
                    animatorController = (AnimatorController)transforms[i].GetComponent<Animator>().runtimeAnimatorController;
                    transforms[i].GetComponent<Animator>().runtimeAnimatorController = animatorController;

                }, () =>
                {
                    DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/FBX/AnimatorController/");
                    FileInfo[] fileInfo = dir.GetFiles();
                    string filePath = directory + transforms[i].name;
                    int num = 0;
                    string fileMark = fileName.Substring(fileName.LastIndexOf("/") + 1);
                    int index = fileMark.LastIndexOf("_AnimatorController") + 1;
                    string fileSubMark = fileMark.Substring(0, index);
                   
                    for (int k = 0; k < fileInfo.Length; k++)
                    {
                        if (fileInfo[k].Name.Contains(".meta") && fileInfo[k].Name.Contains(fileSubMark))
                        {
                            num++;
                        }
                    }
                    fileName = directory + transforms[i].name + "_" + num.ToString() + "_AnimatorController.controller";
                    animatorController = AnimatorController.CreateAnimatorControllerAtPath(fileName);

                    transforms[i].GetComponent<Animator>().runtimeAnimatorController = animatorController;
                }, () =>
                {
                    animatorController = (AnimatorController)transforms[i].GetComponent<Animator>().runtimeAnimatorController;
                    if (animatorController == null)
                    {
                        MessageBoxEditor.ShowErrorBox("创建出错", fileName + "没有动画控制器,需要先创建", "确认", () => { });
                    }
                });
            }
            else
            {
                animatorController = AnimatorController.CreateAnimatorControllerAtPath(fileName);
                transforms[i].GetComponent<Animator>().runtimeAnimatorController = animatorController;
            }
            if (animatorController == null)
            {
                return;
            }

            AnimatorControllerLayer acl = animatorController.layers[0];
            AnimatorStateMachine sm = acl.stateMachine;
            //创建一个空的状态，并且设置为默认
            //int index = Array.FindIndex(sm.states, e => e.state.name.Equals("Empty"));
            for (int id = 0; id < sm.states.Length; )
            {
                if (sm.states[id].state.name.Equals("Empty")
                    || sm.states[id].state.name.Contains("Empty"))
                {
                    sm.RemoveState(sm.states[id].state);
                    id--;
                }
                id++;
            }
            sm.defaultState = sm.AddState("Empty");
            //删除所有的参数和参数连线
            AnimatorControllerParameter[] animatorParameters = animatorController.parameters;
            if (animatorParameters!=null && animatorParameters.Length > 0)
            {
                for (int p = 0; p < animatorParameters.Length; p++)
                {
                    animatorController.RemoveParameter(animatorParameters[p]);
                }
            }
            animatorParameters = null;
            //新增所有的参数和参数连线
            int length  =  sm.states.Length;
            for (int j = 0; j < length; j++)
            {
                string parameterTmp = "";
                if (sm.states[j].state != null && sm.states[j].state.motion != null)
                {
                    parameterTmp = sm.states[j].state.motion.name;
                }
                //如果是第一个对象，则第一个对象与最后一个对象形成切换条件
                if (j == 0)
                {
                    //空状态到第一个状态
                    SetConditionAndParameter(animatorController, sm, parameterTmp, 0, sm.states.Length - 1);
                }
                else if (j < sm.states.Length - 1)//其余的对象，当前的切换条件设置在前一个对象
                {
                    SetConditionAndParameter(animatorController, sm, parameterTmp, j, j - 1);
                }
            }

        }
    }
    
    static void SetConditionAndParameter(AnimatorController animatorController, AnimatorStateMachine sm, string parameter, int currentIndex, int preIndex)
    {
        //为动画控制器添加参数，形成参数列表
        animatorController.AddParameter(parameter, AnimatorControllerParameterType.Trigger);
        //生成状态之间的触发器，即：连线
        AnimatorStateTransition trans;
        AnimatorStateTransition[] stateTransition = sm.states[preIndex].state.transitions;
        //删除原先的切换条件
        if (stateTransition != null && stateTransition.Length > 0)
        {
            int transitionLenght = stateTransition.Length;
            for (int k = 0; k < transitionLenght; k++)
            {
                sm.states[preIndex].state.RemoveTransition(stateTransition[k]);
            }
        }
        stateTransition = null;
        trans = sm.states[preIndex].state.AddTransition(sm.states[currentIndex].state, false);
        //为触发器添加触发条件
        trans.AddCondition(AnimatorConditionMode.If, 0, parameter);
    }
    #region
    [MenuItem("工具/自动创建一个父节点")]
    public static void CreateParentTool()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
        GameObject parent;
        if (transforms.Length <= 0)
        {
            return;
        }
        parent = new GameObject("ParentNode");
        if (transforms.Length == 1)
        {
            parent.name = transforms[0].name + "_ParentNode";
            transforms[0].parent = parent.transform;
        }
        else if (transforms.Length > 1)
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].parent = parent.transform;
            }
        }
    }
    #endregion
}
