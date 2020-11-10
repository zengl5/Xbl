using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAimMgr : MonoBehaviour
{
    private Animator _Animator;

	// Use this for initialization
	void Start () {
     //   GetAnimator();
        _Animator = GetComponent<Animator>();
	}
    
	// Update is called once per frame
	void Update () {
		
	}
    public void DropDown()
    {
       // GetAnimator();
        _Animator.SetTrigger("DropDown");
    }
    public void Move()
    {
       // GetAnimator();
        _Animator.SetTrigger("Show");
    }
    public void MoveOver()
    {
        if (AnimOverHander != null)
        {
            AnimOverHander();
        }
    }
    public void TangXiaoYaGameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_Game01");
    }
    public delegate void OnDelegate();
    public OnDelegate AnimOverHander;
    //public OnDelegate NextScene;
}

