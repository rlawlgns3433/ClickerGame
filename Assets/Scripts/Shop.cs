using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private static Shop _instance = null;

    public static Shop Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject shopObject = new GameObject("Shop");
                _instance = shopObject.AddComponent<Shop>();
                DontDestroyOnLoad(shopObject);
            }

            return _instance;
        }
    }

    public long originalClickLevel;
    public Item item;
    // Start is called before the first frame update

    private void Awake()
    {
        item = Resources.Load<Item>("doubleClickItem");
    }

    public void UseItem()
    {
        Debug.Log("지속시간 : " + item.durationTime);
        originalClickLevel = PlayerData.Instance.mClickLevel;
        StartCoroutine(UseItemCoroutine(item));
    }

    IEnumerator UseItemCoroutine(Item _item)
    {
        
        while(true)
        {
            if(item.remainTime < 0)
            {
                PlayerData.Instance.mClickLevel = originalClickLevel;
                StopCoroutine(UseItemCoroutine(_item));
            }
            Debug.Log("코루틴 시작 x2");

            PlayerData.Instance.mClickLevel = PlayerData.Instance.mClickLevel * 2;
            _item.remainTime -= Time.deltaTime;
            Debug.Log(_item.remainTime);
        }
    }
}
