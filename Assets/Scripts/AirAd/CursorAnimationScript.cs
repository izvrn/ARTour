﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CursorAnimationScript : MonoBehaviour
{

    private Vector3 _startPos;
    private Vector3 _targetPos;

    private Image _content;

    private Color _targetColor;
    private Color _startColor;

    void Awake()
    {
        _content = gameObject.GetComponent<Image>();
        _startPos = gameObject.GetComponent<Image>().rectTransform.localPosition;
        _targetPos = _startPos + new Vector3(0, 150, 0);
        
        var color = _content.color;
        _startColor = color;
        _targetColor = new Color(color.r, color.g, color.b, 0.1f);
    }

    private void OnEnable()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        while (gameObject.activeSelf)
        {
            _content.color = _startColor;
            _content.rectTransform.localPosition = _startPos;
            yield return new WaitForSeconds(1f);
            _targetPos = _startPos;
            yield return new WaitForSeconds(.1f);
            _targetPos = _startPos + new Vector3(0, 150, 0);
        }
    }
    
    private void Update()
    {
        _content.rectTransform.localPosition = Vector3.Lerp(_content.rectTransform.localPosition, _targetPos, Time.deltaTime * 1.4f);
        _content.color = Color.Lerp(_content.color, _targetColor, Time.deltaTime * 1.5f);
    }
}
