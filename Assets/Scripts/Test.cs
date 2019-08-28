using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Test : MonoBehaviour
{
    public GameObject sample;
    private HorizontalScrollSnap _horizontalScrollSnap;

    private void Start()
    {
        _horizontalScrollSnap = GetComponent<HorizontalScrollSnap>();

        for (int i = 0; i < 10; i++)
        {
            var gameObject = Instantiate(sample);
            gameObject.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
            _horizontalScrollSnap.AddChild(gameObject);
        }
    }
    
    private void OnRectTransformDimensionsChange()
    {
        Debug.Log("CURRENT RESOLUTION : " + Screen.width + "x" + Screen.height);
        
        //if (_horizontalScrollSnap)
            //_horizontalScrollSnap.UpdateLayout();
    }
}
