using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class NotKeyableDemo : MonoBehaviour
{
    public int AnimParameter;
    
    //UnityEngine.Timeline.NotKeyable
    [UnityEngine.Animations.NotKeyable]
    public int AnimParameterNoKey;
}