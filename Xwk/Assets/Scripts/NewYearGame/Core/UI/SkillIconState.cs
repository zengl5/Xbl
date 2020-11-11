using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace YB.YM.Game
{
    public class SkillIconState : MonoBehaviour
    {
        private float totalTime = 10f;
        private float currentTime = 0f;
        private bool freeze = false;
        private Image coverUI;
        // Use this for initialization
        void Start()
        {
            coverUI = transform.Find("cd").GetComponent<Image>();
            coverUI.fillAmount = 0;
            freeze = false;
            if (transform.name.Equals("game_snow"))
            {
                totalTime = 6;
            }
            else if (transform.name.Equals("game_jgb"))
            {
                totalTime = 8;
            }
            else if (transform.name.Equals("game_fss"))
            {
                totalTime = 8;
            }
            else if (transform.name.Equals("game_marron"))
            {
                totalTime = 15;
            }
            totalTime *=3;
        }

        // Update is called once per frame
        void Update()
        {
            if (freeze)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= totalTime)
                {
                    coverUI.fillAmount = 0;
                    currentTime = 0;
                    isFreeze = false;
                }
                else
                {
                    coverUI.fillAmount =1- currentTime / totalTime;
                }
            }
        }
        public void FreezeSkill()
        {
            if (freeze)
            {
                return;
            }
            currentTime = 0;
            coverUI.fillAmount = 1;
            isFreeze = true;
        }
        public void ReleaseSkill()
        {
            coverUI.fillAmount = 0;
            isFreeze = false;
        }
        public bool isFreeze
        {
            set
            {
                freeze = value;
            }
            get
            {
                return freeze;
            }
        }
    }
}

