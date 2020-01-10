using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
    private Material _pMat;

    private Vector2 _targetTiling = new Vector2(1f, 1f);

    // Start is called before the first frame update
    void Awake()
    {
        _pMat = GetComponent<MeshRenderer>().material;
        _pMat.mainTextureScale = new Vector2(50, 0);
    }



    // Update is called once per frame
    void Update()
    {
        _pMat.mainTextureScale = Vector2.Lerp(_pMat.mainTextureScale, _targetTiling, 1.5f * Time.deltaTime);
    }
}
