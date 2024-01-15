using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "doubleClickItem", menuName = "doubleClickItemData", order = 1)]
public class Item : ScriptableObject
{
    public string name;
    public string description;
    public int price;
    public int durationTime;
    public float remainTime;
}
