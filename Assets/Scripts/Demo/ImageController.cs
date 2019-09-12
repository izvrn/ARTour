using System.Collections;
using DG.Tweening;
using UnityEngine;
using Wikitude;

public class ImageController : MonoBehaviour
{
    [SerializeField] private GameObject imageGutterCanvasGameObject;
    [SerializeField] private GameObject closeButtonGameObject;
    
    [SerializeField] private VirtualObjectTouchController objectController;

    private float _scaleDuration = 0.8f;
    private Vector3 _localScale;

    private Transform _child1, _child2;

    private WikitudeCamera _wikitudeCamera;
    
    private void Start()
    {
        objectController.objectTouched.AddListener(OnVirtualButtonClicked);

        _child1 = imageGutterCanvasGameObject.transform.GetChild(0);
        _child2 = imageGutterCanvasGameObject.transform.GetChild(1);
        
        _localScale = _child1.localScale;

        _wikitudeCamera = FindObjectOfType<WikitudeCamera>();
    }
    
    private void OnVirtualButtonClicked()
    {
        closeButtonGameObject.SetActive(true);
        imageGutterCanvasGameObject.SetActive(true);
        objectController.gameObject.SetActive(false);

        ScaleOut();
    }

    private void ScaleOut()
    {
        _child1.localScale = Vector3.zero;
        _child1.DOScale(_localScale, _scaleDuration);
        
        _child2.localScale = Vector3.zero;
        _child2.DOScale(_localScale, _scaleDuration);
    }

    private void ScaleIn()
    {
        _child1.DOScale(Vector3.zero, _scaleDuration);
        _child2.DOScale(Vector3.zero, _scaleDuration);
    }

    public void OnCloseButtonPressed()
    {
        StartCoroutine(Close());
    }

    private IEnumerator Close()
    {
        ScaleIn();
        yield return new WaitForSeconds(_scaleDuration);
        
        closeButtonGameObject.SetActive(false);
        imageGutterCanvasGameObject.SetActive(false);
        objectController.gameObject.SetActive(true);
    }
}
