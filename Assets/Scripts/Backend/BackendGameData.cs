using System.Collections.Generic;
using System.Text;
using UnityEngine;

// �ڳ� SDK namespace �߰�
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

        Debug.Log("�����͸� �ʱ�ȭ�մϴ�.");
        userData.mClickLevel = 1;
        userData.mGoldsPerSecLevel = 1;
        userData.mGolds = 0f;
        userData.mGoldsPerSec = 0.3f;

        Debug.Log("�ڳ� ������Ʈ ��Ͽ� �ش� �����͵��� �߰��մϴ�.");
        Param param = new Param();
        param.Add("ClickLevel", userData.mClickLevel);
        param.Add("AutoLevel", userData.mGoldsPerSecLevel);
        param.Add("Golds", userData.mGolds);
        param.Add("GoldsPerSec", userData.mGoldsPerSec);

        Debug.Log("�������� ������ ������ ��û�մϴ�.");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log("�������� ������ ���Կ� �����߽��ϴ�. : " + bro);

            //������ ���������� �������Դϴ�.  
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("�������� ������ ���Կ� �����߽��ϴ�. : " + bro);
        }
    }

    public void GameDataGet()
    {
        Debug.Log("���� ���� ��ȸ �Լ��� ȣ���մϴ�.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json���� ���ϵ� �����͸� �޾ƿɴϴ�.  

            // �޾ƿ� �������� ������ 0�̶�� �����Ͱ� �������� �ʴ� ���Դϴ�.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //�ҷ��� ���������� �������Դϴ�.  

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
            Debug.LogError("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
        }
    }

    public void ClickLevelUp()
    {
        Debug.Log("������ 1 ������ŵ�ϴ�.");
        userData.mClickLevel += 1;
    }
    
    public void AutoGoldLevelUp()
    {
        Debug.Log("������ 1 ������ŵ�ϴ�.");
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


    // �������� �����ϱ�
    public void GameDataUpdate()
    {
        if (userData == null)
        {
            Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Insert Ȥ�� Get�� ���� �����͸� �������ּ���.");
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
            Debug.Log("�� ���� �ֽ� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}�� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("�������� ������ ������ �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("�������� ������ ������ �����߽��ϴ�. : " + bro);
        }
    }
}