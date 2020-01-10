using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailController : MonoBehaviour
{

    private cakeslice.Outline _outliner;

    private bool _enabled;

    public void SetLightningEnabled()
    {
        
        if (_enabled)
        {
           
            StopAllCoroutines();
            _enabled = false;
            _outliner.enabled = false;
            
            return;
        }
        else
        {
            _enabled = true;
            StartCoroutine(Glancing());
        }
        
    }

    private IEnumerator Glancing()
    {
        while (enabled)
        {
            _outliner.enabled = !_outliner.enabled;
            yield return new WaitForSeconds(.5f);
        }
    }

    void OnEnable()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GameObject.FindGameObjectWithTag("UIButton").GetComponent<UIManager>().AddButtonOnCanvas(gameObject.name, this.gameObject);
            _outliner = gameObject.AddComponent<cakeslice.Outline>();
            _outliner.color = 1;
            _outliner.enabled = false;
            //SetLightningEnabled();
        }
    }

    IEnumerator Lol()
    {
        yield return new WaitForSeconds(.1f);
        _outliner.enabled = false;
    }

    void Start()
    {
        //_outliner.enabled = false;

    }
}
