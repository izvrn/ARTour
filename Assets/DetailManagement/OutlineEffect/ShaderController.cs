using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public Shader Shader;

    private Shader _defaultShader;

    void OnMouseEnter()
    {
        GetComponent<MeshRenderer>().material.shader = Shader;
    }

    void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.shader = _defaultShader;
    }

    // Start is called before the first frame update
    void Start()
    {
        _defaultShader = GetComponent<MeshRenderer>().material.shader;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
