using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate {
    [Name("CollectRes")]
    [Attachable(typeof(ActorGroup))]
    [Description("收集加载资源")]
    public class CollectLoadResToolSlate : ActionTrack
    {
        public string ScriptEnd = "";
        protected override void OnEnter()
        {
           // GetAffectResPath();

        }
        public  string GetAffectResPath()
        {
            string res = "";
            if (actor!=null)
            {
                MonoBehaviour[] monos = actor.GetComponents<MonoBehaviour>();
                for (int i = 0; i < monos.Length; i++)
                {
                    if (monos[i].GetType().ToString().EndsWith(ScriptEnd))
                    {
                        res += monos[i].GetType().GetMethod("GetAffectResPath", System.Reflection.BindingFlags.Public).
                            Invoke(actor, new System.Object[] { });
                    }
                }
                return res;
            }
            
            if (CutsceneSequencePlayer._CurrentCutScene!=null)
            {
                C_DebugHelper.LogError(CutsceneSequencePlayer._CurrentCutScene.name + "CollectRes Component is null or actor is null");
            }
            else
            {
                C_DebugHelper.LogError("CollectRes Component is null or actor is null");
            }
            return "";
        }
    }
}


