using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class StoryBtn : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler,IPointerDownHandler 
{
    public Vector3 LocalEnterPos;
    public Vector3 LocalExitPos;


    public void OnPointerDown(PointerEventData eventData)
    {
        SpritBtn.Instance.SetStarMode(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SpritBtn.Instance.SetStarMode(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SpritBtn.Instance.SetStarMode(true);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        SpritBtn.Instance.SetStarMode(true);
    }
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
