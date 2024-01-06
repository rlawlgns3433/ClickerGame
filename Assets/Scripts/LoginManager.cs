using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BackEnd;

public class LoginManager : MonoBehaviour
{
    #region LoginPanel
    [Tooltip("�α��� ���̵� �Է�")]
    [SerializeField]
    private Text Login_Text_ID;

    [Tooltip("�α��� ��й�ȣ �Է�")]
    [SerializeField]
    private InputField Login_InputField_PW;

    [Tooltip("�α��� ȸ������ ��ư")]
    [SerializeField]
    private Button Login_Button_Register;

    [Tooltip("�α��� ��ư")]
    [SerializeField]
    private Button Login_Button_Login;
    #endregion


    #region RegisterPanel
    [Tooltip("ȸ������ ���̵� �Է�")]
    [SerializeField]
    private Text Register_Text_ID;

    [Tooltip("ȸ������ ��й�ȣ �Է�")]
    [SerializeField]
    private Text Register_Text_PW;

    [Tooltip("ȸ������ ��й�ȣ Ȯ�� �Է�")]
    [SerializeField]
    private Text Register_Text_PW_Confirm;

    [Tooltip("ȸ������ �̸��� �Է�")]
    [SerializeField]
    private Text Register_Text_Email;

    [Tooltip("ȸ������ ��ư")]
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