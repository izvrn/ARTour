using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfoButtonScript : MonoBehaviour
{
    [SerializeField] private Text informationText;
    [SerializeField] private string information;
    
    [SerializeField] private float delay = .015f;
    
    private int _scale = 1;
    private Vector3 _scaleVec;
    
    private void Start()
    {
        _scaleVec = transform.localScale;
    }

    private void OnEnable()
    {
        StartCoroutine(TextFilling());
    }

    private void OnDisable()
    {
        informationText.text = String.Empty;
    }
    
    private IEnumerator TextFilling()
    {
        var index = 0;
        while (index < information.Length - 1 && gameObject.activeSelf)
        {
            informationText.text += information[index++];
            yield return new WaitForSeconds(delay);
        }
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale, 
            new Vector3(_scaleVec.x, _scaleVec.y * _scale, _scaleVec.z), 
            Time.deltaTime * 2.5f);
    }
}
