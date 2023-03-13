using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI inputTien;
    public int soTien;
    public TextMeshProUGUI inputVang;
    public int soVang;

    public TextMeshProUGUI tenNPC; // phan nay se hien thi ten cua cac NPC trong game

    public TextMeshProUGUI tenNguoiChoi; // phan nay se hien thi ten cua nguoi choi

    [SerializeField] private PlayerController playerctrl; // lay script tu manager
    //[SerializeField] private Mission_01 mission01; // lay script tu mission01
    public TextMeshProUGUI ThongBaoPopup;
    public string _ThongBaoPopup;

    public TextMeshProUGUI tenTrongThongTin;

    [Header("Phan Bat/Tat UI")]
    public GameObject BangChat;
    public GameObject UI_Chinh; // Bao gom Joystick va tien te
    public GameObject ThongTinPlayer;
    public GameObject ChinhAmLuong;
    public GameObject BangTB_VeSo;
    public GameObject DoiAvt;
    public GameObject CuaHang; // phan nay bat/tat tab cua hang cua game
    public GameObject TabTinNhan;
    public GameObject TabDoAnVat; //Nut de hien len tab ban do an vat
    public GameObject NutTuongTac;
    public GameObject BangTBChinh; // nay la khung chat chu nhat o phia duoi man hinh
    public GameObject MuiTenHD;

    [Header("Phan thong so trong game")]
    private int delayTime = 2;

    [Header("UI Thong bao nhiem vu")]
    public GameObject TabTB_NV01;


    IEnumerator VaoGame()
    {
        yield return new WaitForSeconds(delayTime);
        TabTB_NV01.SetActive(true);
        UI_Chinh.SetActive(false);
    }

    public void TatNVDauGame()
    {
        TabTB_NV01.SetActive(false);
        UI_Chinh.SetActive(true);
    }

    void Start()
    {
        inputTien.text = soTien.ToString();
        inputVang.text = soVang.ToString();
        tenNguoiChoi.text = playerctrl.TenNguoiChoi.ToString();
        tenTrongThongTin.text = playerctrl.TenNguoiChoi.ToString();
        StartCoroutine(VaoGame());
    }

    private void Update()
    {
        inputTien.text = soTien.ToString();
    }


    public void Bat_tabDoAnVat()
    {
        TabDoAnVat.SetActive(true);
    }


    public void Tat_tabDoAnVat()
    {
        TabDoAnVat.SetActive(false);
    }


    public void TatTabTN()
    {
        TabTinNhan.SetActive(false);
    }
    public void BatTabTN()
    {
        TabTinNhan.SetActive(true);
    }

    // Bat/Tat tab thong tin cua nguoi choi
    public void OnThongTin()
    {
        ThongTinPlayer.SetActive(true);
    }

    public void OffThongTin()
    {
        ThongTinPlayer.SetActive(false);
    }


    // Bat/Tat tab chinh am luong cua game
    public void BatSoundPanel()
    {
        ChinhAmLuong.SetActive(true);
    }

    public void TatSoundPanel()
    {
        ChinhAmLuong.SetActive(false);
    }

    public void BatTabVeSo()
    {
        BangTB_VeSo.SetActive(true);
    }
    public void TatTabVeSo()
    {
        BangTB_VeSo.SetActive(false);
    }

    public void BatDoiAvt()
    {
        DoiAvt.SetActive(true);
    }

    public void TatDoiAvt()
    {
        DoiAvt.SetActive(false);
    }

    public void BatTabCuaHang()
    {
        CuaHang.SetActive(true);
    }

    public void TatTabCuaHang()
    {
        CuaHang.SetActive(false);
    }
}
