using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YB.XWK.MainScene
{
    public class HomePageGameBarMgr : MonoBehaviour
    {
        private Transform _State;
        void Awake()
        {
           
        }
        // Use this for initialization
        void Start()
        {
            _State = transform.Find("state");
            if (WizardData.isLevelUp() && DailyBounsData.UnCollectHomePageAllGameBouns())
            {
                _State.gameObject.SetActive(true);
            }
            else
            {
                _State.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
