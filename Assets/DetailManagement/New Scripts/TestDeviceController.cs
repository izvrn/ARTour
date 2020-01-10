using cakeslice;
using System.Collections.Generic;
using UnityEngine;

public class TestDeviceController : MonoBehaviour
{
    [Header ("Materials Settings")]
    [SerializeField] private Material _transparent;

    [Header ("Important Details")]
    [SerializeField] private List<GameObject> _details;

    private Dictionary<GameObject, Material> detailMaterialPairs;

    private void OnEnable()
    {
        detailMaterialPairs = new Dictionary<GameObject, Material>();
        foreach (var detail in _details)
        {
            detailMaterialPairs.Add(detail, detail.GetComponent<MeshRenderer>().material);
        }
    }

    public void ResetVisualization()
    {
        if (gameObject.activeInHierarchy)
            foreach (var detail in _details)
            {
                detailMaterialPairs.TryGetValue(detail, out Material mat);
                detail.GetComponent<MeshRenderer>().material = mat;
                    Destroy(detail.GetComponent<Outline>());
            }
    }

    public void EnlightDetail (GameObject gameObject)
    {
        if (!gameObject.activeInHierarchy) return;
        
        foreach (var detail in _details)
        {
            var detComp = detail.GetComponent<MeshRenderer>();
            if (detail != gameObject)
            {
                detComp.material = _transparent;
                Destroy(detail.GetComponent<Outline>());
            }
            else
            {
                var ol = detail.AddComponent<Outline>();
                ol.color = 1;
                detailMaterialPairs.TryGetValue(detail, out Material mat);
                detComp.material = mat;
            }
                
        }
    }
}
