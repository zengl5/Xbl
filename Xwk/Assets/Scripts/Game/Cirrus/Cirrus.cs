using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;

/// <summary>
/// 筋斗云
/// </summary>
public class Cirrus : BaseEvent {

    struct CirrusPos
    {
        public float Min;
        public float Max;
        public CirrusPos(float x,float y)
        {
            Min = x;
            Max = y;
        }
    }
    public GameObject cube;
    GameObject obj;
    int count = 1;
    CirrusPos screenWidth = new CirrusPos(-12.5f, 12.5f);
    CirrusPos screenHeight = new CirrusPos(0.25f,1.25f);
    public static Cirrus Instance;
    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
       // WindowSliderControl.Instance.AddEvent(StopSelfState);//注册滑动屏幕事件
        base.AddEvent(delegate { Debug.LogError("点击筋斗云，其它动作打断....");});
 	}
	
	// Update is called once per frame
	void Update () {
        UpdateClick();
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    obj = Instantiate(cube);
        //    obj.name = "cube";
        //    C_PoolMgr.Instance.Despawn(obj);
        //    obj.gameObject.SetActive(false);
        //}
        //if(Input.GetKeyDown(KeyCode.F1))
        //{
        //    GameObject obj=C_PoolMgr.Instance.Spawn("cube") as GameObject;
        //    obj.name = "zeng";
        //    obj.SetActive(true);
        //}
	}
    void UpdateClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 定义射线
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 2000))  // 参数1 射线，参数2 碰撞信息， 参数3 碰撞层
            {
                //打印射线碰撞到的对象需要挂载Collider盒子
                if (rayHit.collider.gameObject.name.Equals(this.transform.gameObject.name))
                {
                    //播放爆炸特效
                    PlayEffect();
                    //随机位置
                    ReInitializaPos();
                    NotifyStopState();
                }
            }
        }
        if (Input.touchCount >= 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit))
            {
                //播放爆炸特效
                PlayEffect();
                //随机位置
                ReInitializaPos();
                NotifyStopState();
            }
        }
    }
    
    void NotifyStopState()
    {
        //通知悟空打断状态
        base.DealEvent();
    }
    public void StopSelfState()
    {
        //切换待机状态，停止语音动画播放【1：点击小悟空2：滑屏】
        Debug.LogError("点击悟空，打断筋斗云状态");
    }
    void PlayEffect()
    {
        //爆炸

        //播放语音
        AudioManager.Instance.PlayerSound("爆炸");
    }
    void ReInitializaPos()//Initialization
    {
        //初始化位置
        float X = Random.Range(0, screenWidth.Max);
        if (Random.Range(0,1f) >= 0.5f)
            X = -X;      
        float Y = Random.Range(screenHeight.Min, screenHeight.Max);
        transform.position = new Vector3(X, Y, transform.position.z);
    }

}
