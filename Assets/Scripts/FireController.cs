using System;
using UnityEngine;

public class FireController : MonoBehaviour
{
    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void KeepFireAlive()
    {
        m_animator.SetTrigger("KeepAlive");
        Debug.Log("KeepAlive Trigger set!");
    }
}
