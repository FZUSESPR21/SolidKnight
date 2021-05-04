using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public void SetPosition(float x,float y)
    {
        gameObject.GetComponent<Transform>().localPosition = new Vector2(x, y);       
    }

    private void Awake()
    {
        
    }

    public void OnEnable()
    {
        SetPosition(-144, 44);
    }
}
