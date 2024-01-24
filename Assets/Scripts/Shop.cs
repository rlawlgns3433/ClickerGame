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
    public long itemUseClickLevel;
    public Item item;
    // Start is called before the first frame update

    private void Awake()
    {
        item = Resources.Load<Item>("doubleClickItem");
    }

    private void Update()
    {
        //if(!item.isUsing && item.remainTime <= 0)
        //{
        //    item.remainTime = 10;
        //    StopCoroutine("UseItemCoroutine");
        //}
    }

    public void UseItem()
    {
        Debug.Log("지속시간 : " + item.durationTime);
        originalClickLevel = PlayerData.Instance.mClickLevel;
        itemUseClickLevel = originalClickLevel * 2;
        item.isUsing = true;
        StartCoroutine("UseItemCoroutine");
    }

    IEnumerator UseItemCoroutine()
    {
        while(true)
        {
            if(item.remainTime < 0)
            {
                PlayerData.Instance.mClickLevel = originalClickLevel;
                item.remainTime = item.durationTime;
                item.isUsing = false;
                yield break;
            }
            Debug.Log("코루틴 시작 x2");
            PlayerData.Instance.mClickLevel = itemUseClickLevel;
            item.isUsing = true;
            item.remainTime -= 1;
            Debug.Log(item.remainTime);
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }
}
