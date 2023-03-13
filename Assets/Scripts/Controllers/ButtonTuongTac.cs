using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTuongTac : MonoBehaviour
{

    GameManager ButtonTT;
    private GameObject obj;

    private void Awake()
    {
        ButtonTT = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void HangRong()
    {
        ButtonTT.TabDoAnVat.SetActive(true);
    }

    public void VeSo()
    {
        ButtonTT.BatTabVeSo();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Untagged") || collider.gameObject.CompareTag("CoGiao"))
        {
            return;
        }
        else
        {
            ButtonTT.NutTuongTac.SetActive(true);
            obj = collider.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        ButtonTT.NutTuongTac.SetActive(false);
    }

    public void TuongTac()
    {
        if (obj.CompareTag("HangRong"))
        {
            HangRong();
        }
        else if (obj.CompareTag("VeSo"))
        {
            VeSo();
        }
    }
}