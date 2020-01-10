using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializingScript : MonoBehaviour
{

    [SerializeField] private Text _statusText;

    private bool _initialized = false;

    [SerializeField] bool _fakeInitialization = true;

    private Queue<string> _visTextQueue = new Queue<string>(
        new List<string>()
            {
                "Initializing",
                "Initializing.",
                "Initializing..",
                "Initializing..."
            });

    IEnumerator FakeInit()
    {
        yield return new WaitForSeconds(4f);
        _initialized = true;
        StopCoroutine(Initializing());
        StartCoroutine(OnInitialized(InitializingResult.success));
    }

    IEnumerator RealInit()
    {
        yield return new WaitForSeconds(4f);
        StopCoroutine(Initializing());
        _initialized = SetGPS();
        StartCoroutine(OnInitialized(_initialized ? InitializingResult.success : InitializingResult.fail));
        yield break;
    }

    IEnumerator Initializing()
    {
        StartCoroutine(_fakeInitialization ? FakeInit() : RealInit());
        
            while (!_initialized)
            {
            if (_initialized) { StartCoroutine(OnInitialized(InitializingResult.success)); yield break; }
                var txt = _visTextQueue.Dequeue();
                _statusText.text = txt;
                _visTextQueue.Enqueue(txt);
                yield return new WaitForSeconds(.1f);               
            }
        
    }


    //Я не помню сколько там может быть вариантов подключения, поэтому на всякий случай, можно просто заменить на bool
    public enum InitializingResult
    {
        success,
        fail
    }


    //Заменить на твой инстанс
    private bool SetGPS()
    {
        Input.location.Start();
        Input.compass.enabled = true;
        return (Input.location.status == LocationServiceStatus.Running && Input.compass.enabled);
    }



    public delegate void InitializationSuccessHandler();
    public static event InitializationSuccessHandler OnInitializationSuccess;




    public IEnumerator OnInitialized (InitializingResult res)
    {

        if (res == InitializingResult.success) OnInitializationSuccess();

        var color = _statusText.color;
        StopCoroutine(Initializing());
        _statusText.text = (res == InitializingResult.success) ?
            "Successful !" : "Failed";
        _statusText.color = (res == InitializingResult.success) ?
            Color.green : Color.red;
        yield return new WaitForSeconds(2f);
        
        if (res == InitializingResult.fail)
        {
            _statusText.color = color;
            //StartCoroutine(Initializing());
            yield break;
        }
        _statusText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Initializing());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
