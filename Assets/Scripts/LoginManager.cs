using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BackEnd;

public class LoginManager : MonoBehaviour
{
    #region LoginPanel
    [Tooltip("로그인 아이디 입력")]
    [SerializeField]
    private InputField Login_InputField_ID;

    [Tooltip("로그인 비밀번호 입력")]
    [SerializeField]
    private InputField Login_InputField_PW;

    [Tooltip("로그인 회원가입 버튼")]
    [SerializeField]
    private Button Login_Button_Register;

    [Tooltip("로그인 버튼")]
    [SerializeField]
    private Button Login_Button_Login;
    #endregion


    #region RegisterPanel
    [Tooltip("회원가입 패널")]
    [SerializeField]
    private GameObject RegisterPanel;

    [Tooltip("닉네임 입력")]
    [SerializeField]
    private InputField Register_InputField_NickName;

    [Tooltip("회원가입 아이디 입력")]
    [SerializeField]
    private InputField Register_InputField_ID;

    [Tooltip("회원가입 비밀번호 입력")]
    [SerializeField]
    private InputField Register_InputField_PW;

    [Tooltip("회원가입 비밀번호 확인 입력")]
    [SerializeField]
    private InputField Register_InputField_PW_Confirm;

    [Tooltip("회원가입 이메일 입력")]
    [SerializeField]
    private InputField Register_InputField_Email;

    [Tooltip("회원가입 버튼")]
    [SerializeField]
    private Button Register_Button_Register;

    [Tooltip("비밀번호가 다를 때 텍스트")]
    [SerializeField]
    private Text Register_Text_PW_Incorrect;
    #endregion

    private void Update()
    {
        QuitButton();
        if (Register_InputField_PW.text.Length < 4)
        {
            Register_Text_PW_Incorrect.text = "비밀번호를 4자리 이상 입력하시오.";
            Register_Text_PW_Incorrect.color = new Color(0f, 0, 0, 255);
        }
        else if (Register_InputField_PW.text != Register_InputField_PW_Confirm.text)
        {
            Register_Text_PW_Incorrect.text = "비밀번호가 일치하지 않습니다.";
            Register_Text_PW_Incorrect.color = new Color(255f, 0, 0,255);
        }
        else if(Register_InputField_PW.text == Register_InputField_PW_Confirm.text &&
            Register_InputField_PW.text.Length >= 4)
        {
            Register_Text_PW_Incorrect.text = "비밀번호가 일치합니다.";
            Register_Text_PW_Incorrect.color = new Color(0f, 255f, 0f, 255);
        }
    }

    private static LoginManager _instance = null;
    public static LoginManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LoginManager();
            }

            return _instance;
        }
    }

    public void Login()
    {
        BackendLogin.Instance.CustomLogin(Login_InputField_ID.text, Login_InputField_PW.text);
        Login_InputField_ID.text = "";
        Register_InputField_PW.text = "";
    }

    public void Register()
    {
        if (Register_InputField_PW.text != Register_InputField_PW_Confirm.text) return;
        if (Register_InputField_PW.text.Length < 4)
        {
            Debug.LogError("회원가입에 실패했습니다.");
            return;
        }

        BackendLogin.Instance.CustomSignUp(Register_InputField_ID.text, Register_InputField_PW_Confirm.text, Register_InputField_Email.text, Register_InputField_NickName.text);
        Register_InputField_ID.text = "";
        Register_InputField_PW.text = "";
        Register_InputField_PW_Confirm.text = "";
        Register_InputField_Email.text = "";

        RegisterPanel.SetActive(false);
    }

    public void QuitButton()
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

}