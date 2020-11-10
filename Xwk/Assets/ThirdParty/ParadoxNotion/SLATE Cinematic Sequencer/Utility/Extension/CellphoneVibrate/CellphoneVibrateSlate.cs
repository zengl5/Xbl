using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Slate.ActionClips {
    [Category("Utility")]
    [Description("摇一摇")]
    public class CellphoneVibrateSlate : DirectorActionClip
    {
        //记录上一次的重力感应的Y值
        private float old_y = 0;
        //记录当前的重力感应的Y值
        private float new_y;
        //当前手机晃动的距离
        private float currentDistance = 0;

        //手机晃动的有效距离
        public float distance = 2f;
        private bool StopFlag = false;
        protected override void OnEnter()
        {
            StopFlag = false;
            CutsceneSequencePlayer.PauseCurrentCutscene();
        }
        protected override void OnRootUpdated(float time, float previousTime)
        {
            if (StopFlag)
            {
                return;
            }
            new_y = Input.acceleration.y;
            currentDistance = new_y - old_y;
            old_y = new_y;

            if (currentDistance > distance)
            {
                StopFlag = true;
                Handheld.Vibrate(); //手机的震动效果
                CutsceneSequencePlayer.ResumeCurrentCutscene();
            }
        }
        
       
    }

}


