using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using BackEnd;

public class GameDirector : MonoBehaviour
{
    [Header("PlayerAnimation")]
    [SerializeField]
    private PlayerAnimation playerAnimator;

    [Tooltip("현재 보유중인 골드")]
    private Text Text_Gold;

    [Tooltip("현재 레벨")]
    private Text Text_Level;

    [Tooltip("라벨 클릭")]
    private Text Text_Label_ClickUpgrade;

    [Tooltip("라벨 초당 획득 골드")]
    private Text Text_Label_GoldsPerSecUpgrade;

    [Tooltip("클릭 업그레이드 비용")]
    private Text Text_ClickeUpgradeGold;

    [Tooltip("초당 획득 골드 업그레이드 비용")]
    private Text Text_GoldsPerSecUpgradeUpgradeGold;

    [Tooltip("터치 가능 영역")]
    private RectTransform Panel_PetTouchable;

    [Tooltip("닉네임")]
    private Text Text_NickName;

    private bool isPaused = false;

    private void Start()
    {
        playerAnimator = GameObject.Find("MyPet").GetComponent<PlayerAnimation>();
        Text_Gold = GameObject.Find("Text_Gold").GetComponent<Text>();
        Text_Level = GameObject.Find("Text_Level").GetComponent<Text>();
        Text_NickName = GameObject.Find("Text_NickName").GetComponent<Text>();
        Text_Label_ClickUpgrade = GameObject.Find("Label_ClickUpgrade").GetComponent<Text>();
        Text_Label_GoldsPerSecUpgrade = GameObject.Find("Label_GoldsPerSecUpgrade2").GetComponent<Text>();
        Text_ClickeUpgradeGold = GameObject.Find("Text_UpgradeGold").GetComponent<Text>();
        Text_GoldsPerSecUpgradeUpgradeGold = GameObject.Find("Text_GoldsPerSecUpgradeGold").GetComponent<Text>();
        Panel_PetTouchable = GameObject.Find("PetTouchable").GetComponent<RectTransform>();
        BackendGameData.Instance.GameDataGet(); // 데이터 삽입 함수

        // [추가] 서버에 불러온 데이터가 존재하지 않을 경우, 데이터를 새로 생성하여 삽입
        if (BackendGameData.userData == null)
        {
            BackendGameData.Instance.GameDataInsert();
        }

        StartCoroutine(IDisplayInfo());
        StartCoroutine(IEarnGoldsPerSec());

        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        string nickname = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        Text_NickName.text = nickname;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 범위 내에 터치 시 점수 획득
            if (touchPos.x >= -2.8f && touchPos.x <= 2.8f && touchPos.y >= -0.6f && touchPos.y <= 4.2f)
            {
                PlayerData.Instance.mGolds += PlayerData.Instance.mClickLevel;
            }
        }

        QuitButton();
    }

    private void OnApplicationQuit()
    {
        BackendGameData.Instance.GameDataUpdate();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // 앱이 일시 중지될 때 실행할 로직
            isPaused = true;
            // 예를 들어 데이터 저장 등
            BackendGameData.Instance.GameDataUpdate();
        }
        else
        {
            // 앱이 다시 활성화될 때 실행할 로직
            if (isPaused)
            {
                // 앱이 강제 종료된 경우에만 이 부분 실행
                // 예를 들어 강제 종료 시 로그 기록 등
                BackendGameData.Instance.GameDataUpdate();
            }
            isPaused = false;
        }
    }
    public void UpgradeButtonClicked()
    {
        PlayerData.Instance.mUpgradeClickCost = PlayerData.Instance.mClickLevel * PlayerData.Instance.mClickLevel * PlayerData.Instance.mClickLevel;
        if (PlayerData.Instance.mGolds >= PlayerData.Instance.mUpgradeClickCost)
        {
            PlayerData.Instance.mClickLevel += 1;
            PlayerData.Instance.mGolds -= PlayerData.Instance.mUpgradeClickCost;
            BackendGameData.Instance.ClickLevelUp();
            playerAnimator.PlayUpgradeAnimation();
        }
        else
        {
            Debug.Log("업그레이드 할 수 없습니다.");
        }
    }

    public void UpgradeButtonGoldsPerSec()
    {
        PlayerData.Instance.mUpgradeGoldsPerSecCost = PlayerData.Instance.mGoldsPerSecLevel * PlayerData.Instance.mGoldsPerSecLevel * PlayerData.Instance.mGoldsPerSecLevel;
        if (PlayerData.Instance.mGolds >= PlayerData.Instance.mUpgradeGoldsPerSecCost)
        {
            PlayerData.Instance.mGoldsPerSecLevel += 1;
            if (PlayerData.Instance.mGoldsPerSec < 100) PlayerData.Instance.mGoldsPerSec *= 1.5f;
            else if (PlayerData.Instance.mGoldsPerSec < 1000) PlayerData.Instance.mGoldsPerSec *= 1.2f;
            else if (PlayerData.Instance.mGoldsPerSec < 10000) PlayerData.Instance.mGoldsPerSec *= 1.05f;
            else PlayerData.Instance.mGoldsPerSec *= 1.01f;
            PlayerData.Instance.mGolds -= PlayerData.Instance.mUpgradeGoldsPerSecCost;


            BackendGameData.Instance.AutoGoldLevelUp();
            playerAnimator.PlayUpgradeAnimation();
            BackendGameData.Instance.UpdateGoldsPerSec(PlayerData.Instance.mGoldsPerSec);
        }
        else
        {
            Debug.Log("업그레이드 할 수 없습니다.");
        }
    }

    public void UseDoubleClickItem()
    {
        if (PlayerData.Instance.mGolds >= Shop.Instance.item.price)
        {
            PlayerData.Instance.mGolds -= Shop.Instance.item.price;

            Shop.Instance.UseItem();

            Debug.Log("아이템을 사용합니다.");
        }
        else
        {
            Debug.Log("아이템을 사용할 수 없습니다.");
        }
    }

    public void LogoutButton()
    {
        BackendGameData.Instance.GameDataUpdate();
        Backend.BMember.Logout();
        SceneManager.LoadScene("LoginScene");
    }

    private float MyRound(float number, int decimalPlaces)
    {
        return Mathf.Floor(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);
    }

    private void QuitButton()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackendGameData.Instance.GameDataUpdate();
                Application.Quit();
            }
        }
    }

    IEnumerator IDisplayInfo()
    {
        while (true)
        {
            Text_Label_ClickUpgrade.text = "1 클릭 = " + (PlayerData.Instance.mClickLevel + 1).ToString() + " 골드";
            Text_Label_GoldsPerSecUpgrade.text = PlayerData.Instance.mGoldsPerSec.ToString() + " 골드";
            Text_Gold.text = "골드 : " + MyRound(PlayerData.Instance.mGolds, 3).ToString();
            Text_Level.text = "레벨 : " + ((PlayerData.Instance.mClickLevel + PlayerData.Instance.mGoldsPerSecLevel) / 2).ToString();
            Text_ClickeUpgradeGold.text = (PlayerData.Instance.mClickLevel * PlayerData.Instance.mClickLevel * PlayerData.Instance.mClickLevel).ToString() + "골드";
            Text_GoldsPerSecUpgradeUpgradeGold.text = (PlayerData.Instance.mGoldsPerSecLevel * PlayerData.Instance.mGoldsPerSecLevel * PlayerData.Instance.mGoldsPerSecLevel).ToString() + "골드";


            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator IEarnGoldsPerSec()
    {
        while (true)
        {
            PlayerData.Instance.mGolds += MyRound(PlayerData.Instance.mGoldsPerSec, 3);
            BackendGameData.Instance.UpdateGold(PlayerData.Instance.mGolds);
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }
}
