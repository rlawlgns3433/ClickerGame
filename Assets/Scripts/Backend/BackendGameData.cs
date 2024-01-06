using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class PlayerData
{
    private static PlayerData _instance = null;

    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerData();
            }

            return _instance;
        }
    }

    public float mGolds = 0;

    public float mGoldsPerSec = 0.3f;

    public long mUpgradeClickCost = 0;

    public long mUpgradeGoldsPerSecCost = 0;

    public long mClickLevel = 1;

    public long mGoldsPerSecLevel = 1;
}


public class BackendGameData
{
    private static BackendGameData _instance = null;

    public static BackendGameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendGameData();
            }

            return _instance;
        }
    }

    public static PlayerData userData;

    private string gameDataRowInDate = string.Empty;

    public void GameDataInsert()
    {
        if (userData == null)
        {
            userData = new PlayerData();
        }

        Debug.Log("데이터를 초기화합니다.");
        userData.mClickLevel = 1;
        userData.mGoldsPerSecLevel = 1;
        userData.mGolds = 0f;
        userData.mGoldsPerSec = 0.3f;

        Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
        Param param = new Param();
        param.Add("ClickLevel", userData.mClickLevel);
        param.Add("AutoLevel", userData.mGoldsPerSecLevel);
        param.Add("Golds", userData.mGolds);
        param.Add("GoldsPerSec", userData.mGoldsPerSec);

        Debug.Log("게임정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임정보의 고유값입니다.  
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }

    public void GameDataGet()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임정보의 고유값입니다.  

                userData = new PlayerData();

                userData.mClickLevel = int.Parse(gameDataJson[0]["ClickLevel"].ToString());
                userData.mGoldsPerSecLevel = int.Parse(gameDataJson[0]["AutoLevel"].ToString());
                userData.mGolds = float.Parse(gameDataJson[0]["Golds"].ToString());
                userData.mGoldsPerSec = float.Parse(gameDataJson[0]["GoldsPerSec"].ToString());

                Debug.Log(userData.mClickLevel);
                Debug.Log(userData.mGoldsPerSecLevel);
                Debug.Log(userData.mGolds);
                Debug.Log(gameDataRowInDate);

                PlayerData.Instance.mClickLevel = userData.mClickLevel;
                PlayerData.Instance.mGoldsPerSecLevel = userData.mGoldsPerSecLevel;
                PlayerData.Instance.mGolds = userData.mGolds;
                PlayerData.Instance.mGoldsPerSec = userData.mGoldsPerSec;
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }
    }

    public void ClickLevelUp()
    {
        Debug.Log("레벨을 1 증가시킵니다.");
        userData.mClickLevel += 1;
    }
    
    public void AutoGoldLevelUp()
    {
        Debug.Log("레벨을 1 증가시킵니다.");
        userData.mGoldsPerSecLevel += 1;
    }

    public void UpdateGold(float _Golds)
    {
        userData.mGolds = _Golds;
    }

    public void UpdateGoldsPerSec(float _GoldsPerSec)
    {
        userData.mGoldsPerSec = _GoldsPerSec;
    }


    // 게임정보 수정하기
    public void GameDataUpdate()
    {
        if (userData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param();
        param.Add("ClickLevel", userData.mClickLevel);
        param.Add("AutoLevel", userData.mGoldsPerSecLevel);
        param.Add("Golds", userData.mGolds);
        param.Add("GoldsPerSec", userData.mGoldsPerSec);

        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }
}