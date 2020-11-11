using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Camera标记类,防止相机多次注册事件
/// </summary>
class MeshCameraSign:MonoBehaviour
{

}
class MeshButtonContainer
{
    Camera cam;
    public MeshButtonContainer(Camera cam)
    {
        this.cam = cam;
    }
    public void OnUpdate()
    {
        if (cam == null)
            return;

        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, 2000, ~(1 << 12)))
                {
                    GameObject obj = rayHit.collider.gameObject;
                    if (obj.CompareTag("meshButton") | obj.CompareTag("meshButton_Begin"))
                    {
                        MeshButton button = obj.GetComponent<MeshButton>();
                        if (button != null)
                        {
                            button.MeshHit();
                            button.MeshHit(obj);
                        }
                    }
                }
            }
        }
        if (Input.touchCount >= 1)
        {
            Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit rayHit;
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out rayHit))
                {
                    if (Physics.Raycast(ray, out rayHit, 2000, ~(1 << 12)))
                    {
                        GameObject obj = rayHit.collider.gameObject;
                        if (obj.CompareTag("meshButton_Begin"))
                        {
                            MeshButton button = obj.GetComponent<MeshButton>();
                            if (button != null)
                            {
                                button.MeshHit();
                                button.MeshHit(obj);
                            }
                        }
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast(ray, out rayHit, 2000, ~(1 << 12)))
                {
                    GameObject obj = rayHit.collider.gameObject;
                    if (obj.CompareTag("meshButton"))
                    {
                        MeshButton button = obj.GetComponent<MeshButton>();
                        if (button != null)
                        {
                            button.MeshHit();
                            button.MeshHit(obj);
                        }
                    }
                }
            }
        }
    
}
}
public class MeshButtonManager : MonoSingleton<MeshButtonManager>
{
    List<MeshButtonContainer> MeshButtonContainerList = new List<MeshButtonContainer>();
   
    /// <summary>
    /// 初始化相机
    /// </summary>
    /// <param name="cam"></param>
    public void SetMeshCamera(Camera cam)
    {
        if (cam.transform.GetComponent<MeshCameraSign>() == null)
        {
            MeshButtonContainer container = new MeshButtonContainer(cam);
            cam.transform.gameObject.AddComponent<MeshCameraSign>();
            MeshButtonContainerList.Add(container);
        }
    }
    public void SetMeshButtonInteractable(MeshButton button, bool flag)
    {
        button.Interactable = flag;
    }
    public void SetMeshButtonListInteractable(List<MeshButton> buttonList, bool flag)
    {
        for (int i = 0; i < buttonList.Count; i++)
            buttonList[i].Interactable = flag;
    }
    void Update()
    {
        for (int i = 0; i < MeshButtonContainerList.Count; i++)
        {
            MeshButtonContainerList[i].OnUpdate();
        }
    }
}
