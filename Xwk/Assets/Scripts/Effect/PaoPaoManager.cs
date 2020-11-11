using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XWK.Effect.Moudle{
    public class PaoPaoManager : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Invoke("ShowPaoPao",Random.Range(2,5));
        }
        void ShowPaoPao()
        {
            int childCount = transform.childCount;
            for(int i =0;i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (Random.Range(0,5) == 1)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
            CancelInvoke("ShowPaoPao");
        }
    }
}
