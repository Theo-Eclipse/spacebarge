using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffEffect : MonoBehaviour
{
    [Header("Effect Info")]
    [SerializeField] private string title;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite effectIcon;

    [Tooltip("If equals to zero - means the effect will stay permanently.")]
    public float duration = 0;

    protected abstract void StartEffect();
    protected abstract void StopEffect();
}
