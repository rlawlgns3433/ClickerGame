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
    private Text Login_Text_ID;

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
    [Tooltip("회원가입 아이디 입력")]
    [SerializeField]
    private Text Register_Text_ID;

    [Tooltip("회원가입 비밀번호 입력")]
    [SerializeField]
    private Text Register_Text_PW;

    [Tooltip("회원가입 비밀번호 확인 입력")]
    [SerializeField]
    private Text Register_Text_PW_Confirm;

    [Tooltip("회원가입 이메일 입력")]
    [SerializeField]
    private Text Register_Text_Email;

    [Tooltip("회원가입 버튼")]
    [SerializeField]
    private Button Register_Button_Register;
    #endregion

    private void Update()
    {
        QuitButton();
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
        BackendLogin.Instance.CustomLogin(Login_Text_ID.text, Login_InputField_PW.text);
        Login_Text_ID.text = "";
        Register_Text_PW.text = "";
    }

    public void Register()
    {
        BackendLogin.Instance.CustomSignUp(Register_Text_ID.text, Register_Text_PW.text, Register_Text_Email.text);
        Register_Text_ID.text = "";
        Register_Text_PW.text = "";
        Register_Text_PW_Confirm.text = "";
        Register_Text_Email.text = "";
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