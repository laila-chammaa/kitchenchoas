using System;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    const string OPEN_CLOSE = "OpenClose";

    [SerializeField]
    ContainerCounter containerCounter;

    Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounterOnOnPlayerGrabbedObject;
    }

    void ContainerCounterOnOnPlayerGrabbedObject(object sender, EventArgs e)
    {
        m_Animator.SetTrigger(OPEN_CLOSE);
    }
}
