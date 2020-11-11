using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.C_Framework;
using UnityEngine.EventSystems;
using System;

public class Effect_Breathe : MonoBehaviour
{
    public bool Enabled = true;
    public float Scale = 1.5f;
    public float Speed = 1;
    private bool m_bForward = true;

    private Vector3 localScale = Vector3.one;

    private Vector3 originalScale = Vector3.one;
    private Vector3 changeScale = Vector3.one;
    void Start()
    {
        originalScale = this.transform.localScale;
        changeScale = this.transform.localScale * Scale;
    }

    void Update()
    {
        if (Enabled)
        {
            localScale = this.transform.localScale;
            if (m_bForward)
            {
                localScale.x += 0.01f * Speed * (changeScale.x - originalScale.x);
                localScale.y += 0.01f * Speed * (changeScale.y - originalScale.y);
                localScale.z += 0.01f * Speed * (changeScale.z - originalScale.z);
                this.transform.localScale = localScale;

                if (this.transform.localScale.x > changeScale.x)
                    m_bForward = false;
            }
            else
            {
                localScale.x -= 0.01f * Speed * (changeScale.x - originalScale.x);
                localScale.y -= 0.01f * Speed * (changeScale.y - originalScale.y);
                localScale.z -= 0.01f * Speed * (changeScale.z - originalScale.z);
                this.transform.localScale = localScale;

                if (this.transform.localScale.x < originalScale.x)
                    m_bForward = true;
            }
        }
    }
}
