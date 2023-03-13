using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public string TenNguoiChoi;

    Mission_01 mission_01;
    //GameManager ThuongNV1;
    [SerializeField] private GameManager NPC_cogiao;


    private string CoGiaoNoi = "Cam on chau da giao do an cho co nha.";
    private string YeuCauMuaBanh = "Nho chau di den tiem banh mua cho co 1 cai banh nha.";
    public TextMeshProUGUI textComponent;
    private float delay = 0.05f;

    private void Awake()
    {
        mission_01 = GameObject.Find("M_01_Trigger").GetComponent<Mission_01>();
        //ThuongNV1 = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider test)
    {
        if (test.gameObject.CompareTag("CoGiao"))
        {
            
            //Debug.Log(mission_01.CheckNhanNV);
            //ThuongNV1.BangTBChinh.SetActive(true);
            StartCoroutine(Text());
        }
        else
        {
            Debug.Log("Khong biet");
        }


    }


    IEnumerator Text()
    {
        NPC_cogiao.UI_Chinh.SetActive(false); // cai nay la phan thong bao khi nhiem vu bat dau
        NPC_cogiao.BangTBChinh.SetActive(true);
        NPC_cogiao.tenNPC.text = "Co Giao"; // nay de gan cai ten len phan text

        if (mission_01.CheckNhanNV == true)
        {

            textComponent.text = "";
            foreach (char letter in CoGiaoNoi.ToCharArray())
            {
                textComponent.text += letter;
                yield return new WaitForSeconds(delay);
            }

            // Cong tien thuong khi hoan thanh nhiem vu
            NPC_cogiao.soTien = NPC_cogiao.soTien + 100;
            NPC_cogiao.ThongBaoPopup.text = NPC_cogiao.soTien.ToString();
            yield return new WaitForSeconds(3f);
        }


        else
        {
            textComponent.text = "";
            foreach (char letter in YeuCauMuaBanh.ToCharArray())
            {
                textComponent.text += letter;
                yield return new WaitForSeconds(delay);
            }
        }

    }
}
