using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Item")]
public class Item_SO : ScriptableObject
{
    [Header("Cac loai item co the thu thap")]

    public string ten;
    public string moTa;
    public string loai;
    public int soLuong;

    public Sprite icon;
}
