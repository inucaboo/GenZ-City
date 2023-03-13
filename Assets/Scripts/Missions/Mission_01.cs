using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class Mission_01 : MonoBehaviour
{

    public GameObject ViTriNhanNhiemVu_01;

    //public GameObject ThongBaoNV_01;
    public bool CheckNhanNV = false;

    public TextMeshProUGUI textComponent;
    private string tenNPC = "Chủ quán";
    private string ND_NV_01 = "Chào INUCABOO, có 1 khách hàng nhờ chú chuyển món hàng này cho anh ta. Củm ơn cháu.";
    private float delay = 0.05f;

    [SerializeField] private GameManager _tenNPC;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(MissionBegin());
            StartCoroutine(ShowText());
        }
        else
        {
            Debug.Log("Loi");
        }

    }

    IEnumerator ShowText()
    {
        _tenNPC.tenNPC.text = tenNPC; // nay de gan cai ten len phan text
        textComponent.text = "";
        foreach (char letter in ND_NV_01.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator MissionBegin()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        _tenNPC.UI_Chinh.SetActive(false); // an cai nut tren man hinh
        //ThongBaoNV_01.SetActive(true);
        _tenNPC.BangTBChinh.SetActive(true);
        _tenNPC.MuiTenHD.SetActive(false); // den noi roi thi tat cai mui ten di
        AudioManager.Instance.PlaySFX("popup_chat"); // chay doan sound ting ting
        CheckNhanNV = true; // neu den cho nv roi thi phan nhiem vu se la true tuc la da nhan nhiem vu

        yield return new WaitForSeconds(4); // doi 3s sau do tat cai bang thong bao di
        //ThongBaoNV_01.SetActive(false);

    }

    public void NhanNhiemVu()
    {
        _tenNPC.BangTBChinh.SetActive(false);
        _tenNPC.UI_Chinh.SetActive(true);
    }
}
