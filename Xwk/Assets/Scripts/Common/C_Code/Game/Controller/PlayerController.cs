using Assets.Scripts.C_Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    None,
    Idle,
    Welcome,
    Hello,
    AutoPlay,
    Touch
}

public class PlayerController : C_MonoSingleton<PlayerController>
{
    private MovementState m_CurMovementState = MovementState.None;
    public MovementState CurMovementState
    {
        get { return m_CurMovementState; }
        set
        {
            if (m_CurMovementState != value)
            {
                m_CurMovementState = value;

                if (XBLPlayerCharacter != null)
                    XBLPlayerCharacter.Play((int)value);
            }
        }
    }

    public GameObject XBLPlayer = null;
    public Character XBLPlayerCharacter = null;

    private float m_fMaxIdleTime = 12.0f;
    private float m_fCurIdleTime = 0;

    private bool m_bAutoPlayEnabled = true;
    public bool AutoPlayEnabled
    {
        get { return m_bAutoPlayEnabled; }
        set
        {
            if (m_bAutoPlayEnabled != value)
            {
                m_bAutoPlayEnabled = value;
                if (m_bAutoPlayEnabled)
                {
                    m_fCurIdleTime = m_fMaxIdleTime;
                    CurMovementState = MovementState.Idle;
                }
                else
                {
                    CurMovementState = MovementState.Idle;
                }
            }
        }
    }

    void Update()
    {
        if (XBLPlayer != null && XBLPlayer.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) && AutoPlayEnabled && !XBLPlayerCharacter.Running && CurMovementState == MovementState.Idle)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        CurMovementState = MovementState.Touch;

                        C_MonoSingleton<GameHelper>.GetInstance().SendDataStatistics(EnumDataStatistics.Chick, "dianji_xiaobanlong");
                    }
                }
            }

            if (!XBLPlayerCharacter.Running && CurMovementState != MovementState.Idle)
            {
                CurMovementState = MovementState.Idle;
                m_fCurIdleTime = m_fMaxIdleTime;
            }

            if (AutoPlayEnabled && CurMovementState == MovementState.Idle)
            {
                if (m_fCurIdleTime <= 0)
                    CurMovementState = MovementState.AutoPlay;
                else
                    m_fCurIdleTime -= Time.deltaTime;
            }
        }
    }

    private bool firstLogin = true;
    public void CreatePlayer()
    {
        if (XBLPlayer == null)
        {
            XBLPlayer = EntityMgr.CreateCharacter(50001);
            XBLPlayer.transform.SetParent(this.transform);

            if (C_UIMgr.c_AspectRatio < 1.4f)
                XBLPlayer.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            Vector3 pos = XBLPlayer.transform.position;

            if (C_UIMgr.c_AspectRatio > 2.0f)
                pos.x = Camera.main.ScreenToWorldPoint(new Vector3(250, 0, 0)).x;
            else
                pos.x = Camera.main.ScreenToWorldPoint(new Vector3(160, 0, 0)).x;

            XBLPlayer.transform.position = pos;

            XBLPlayerCharacter = XBLPlayer.GetComponent<Character>();

            m_fCurIdleTime = m_fMaxIdleTime;

            m_CurMovementState = MovementState.None;
        }

        if (firstLogin)
        {
            firstLogin = false;
            CurMovementState = MovementState.Hello;
        }
        else
        {
            CurMovementState = MovementState.Idle;
        }
    }

    public void DestroyPlayer()
    {
        m_CurMovementState = MovementState.None;

        if (XBLPlayer != null)
        {
            Destroy(XBLPlayer);
            XBLPlayer = null;
            XBLPlayerCharacter = null;
        }
    }

    public void PlayAudio(string name)
    {
        if (XBLPlayerCharacter != null)
            XBLPlayerCharacter.PlayAudio(name);
    }

    public void PlayAudioBabyNameBefore(string name)
    {
        if (XBLPlayerCharacter != null)
            XBLPlayerCharacter.PlayAudioBabyNameBefore(name);
    }
}
