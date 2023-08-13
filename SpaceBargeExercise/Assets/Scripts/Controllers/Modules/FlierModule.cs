using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlierModule : MonoBehaviour
{
    [Header("Module Info")]
    [SerializeField] private string title;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite moduleIcon;
}
