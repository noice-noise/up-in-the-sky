using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera wideCamera;
    public Cinemachine.CinemachineVirtualCamera focusedCamera;

    public Transform mainTarget;
    public Transform[] targets;
    public int count;
    public Transform followTarget;

    private Animator animator;
    public bool wideViewCamera = true;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void Start() 
    {

    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            wideViewCamera = !wideViewCamera;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchTarget(followTarget);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchTarget(mainTarget);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            count += 1;
            SwitchTarget(targets[count]);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            count -= 1;
            SwitchTarget(targets[count]);
        }
    }

    private void SwitchTarget(Transform followTarget)
    {
            wideCamera.m_Follow = followTarget;
            wideCamera.m_LookAt = followTarget;
            focusedCamera.m_Follow = followTarget;
            focusedCamera.m_LookAt = followTarget;
    }

    private void LateUpdate() {
        HandleCameraSwitches();
    }

    private void HandleCameraSwitches()
    {
        if (wideViewCamera)
        {
            animator.Play("WideViewCamera");

        }
        else
        {
            animator.Play("FocusedViewCamera");
        }
    }

}
