using UnityEngine;

public class XeBanHangRong : MonoBehaviour
{
    GameManager XeBanHang;

    private void Awake()
    {
        XeBanHang = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        XeBanHang.NutTuongTac.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        XeBanHang.NutTuongTac.SetActive(false);
    }
}
