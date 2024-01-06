using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendManager : MonoBehaviour
{
    private static BackendManager _instance = null;

    public static BackendManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendManager();
            }

            return _instance;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }
    }
}