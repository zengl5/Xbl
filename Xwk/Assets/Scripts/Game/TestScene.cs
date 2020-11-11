using Assets.Scripts.C_Framework;
using UnityEngine;
using XWK.Common.RedBomb;
using XWK.Common.UI_Reward;
using UnityEngine.UI;
public class TestScene : MonoBehaviour
{
    public Button btn;

    private void Start()
    {
        RedBombManager.Instance.StartPlay(0);
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            CommonUIMgr.Instance.ShowUI(btn.transform.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            CommonUIMgr.Instance.CloseUI(btn.transform.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            CommonUIMgr.Instance.ClickFunctionButton(btn.transform.gameObject);
        }
         


    }
}