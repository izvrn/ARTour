using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject _buttonPrefab;

    [SerializeField] GameObject _UIOrigin;



    public void AddButtonOnCanvas(string name, GameObject reference)
    {
        var tmp = Instantiate(_buttonPrefab, _UIOrigin.transform);
        tmp.GetComponent<Button>().name = name;
        tmp.transform.GetChild(0).GetComponent<Text>().text = name;
        tmp.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction (() => reference.GetComponent<DetailController>().SetLightningEnabled()));



        //tmp.AddComponent<EventTrigger>();
        //EventTrigger t = tmp.GetComponent<EventTrigger>();
        //var e = new EventTrigger.Entry();
        //e.eventID = EventTriggerType.PointerClick;
        //e.callback.AddListener((PointerEventData) => reference.GetComponent<DetailController>().SetLightningEnabled());
        //t.triggers.Add(e);
        //Debug.Log(tmp.GetComponent<Button>().name);
        //tmp.GetComponent<Button>().onClick.AddListener(() => reference.GetComponent<DetailController>().SetLightningEnabled());
        //Instantiate(tmp, _UIOrigin.transform);
    }
}
