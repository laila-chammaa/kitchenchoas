using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    const string CUT = "Cut";

    [SerializeField]
    CuttingCounter cuttingCounter;

    Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    void Start()
    {
        cuttingCounter.OnPlayerCutObject += CuttingCounterOnOnPlayerCutObject;
    }

    void CuttingCounterOnOnPlayerCutObject(object sender, EventArgs e)
    {
        m_Animator.SetTrigger(CUT);
    }
}
