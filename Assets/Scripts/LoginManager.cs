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
    private InputField Login_InputField_ID;

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
    [Tooltip("ȸ������ �г�")]
    [SerializeField]
    private GameObject RegisterPanel;

    [Tooltip("�г��� �Է�")]
    [SerializeField]
    private InputField Register_InputField_NickName;

    [Tooltip("ȸ������ ���̵� �Է�")]
    [SerializeField]
    private InputField Register_InputField_ID;

    [Tooltip("ȸ������ ��й�ȣ �Է�")]
    [SerializeField]
    private InputField Register_InputField_PW;

    [Tooltip("ȸ������ ��й�ȣ Ȯ�� �Է�")]
    [SerializeField]
    private InputField Register_InputField_PW_Confirm;

    [Tooltip("ȸ������ �̸��� �Է�")]
    [SerializeField]
    private InputField Register_InputField_Email;

    [Tooltip("ȸ������ ��ư")]
    [SerializeField]
    private Button Register_Button_Register;

    [Tooltip("��й�ȣ�� �ٸ� �� �ؽ�Ʈ")]
    [SerializeField]
    private Text Register_Text_PW_Incorrect;
    #endregion

    private void Update()
    {
        QuitButton();
        if (Register_InputField_PW.text.Length < 4)
        {
            Register_Text_PW_Incorrect.text = "��й�ȣ�� 4�ڸ� �̻� �Է��Ͻÿ�.";
            Register_Text_PW_Incorrect.color = new Color(0f, 0, 0, 255);
        }
        else if (Register_InputField_PW.text != Register_InputField_PW_Confirm.text)
        {
            Register_Text_PW_Incorrect.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            Register_Text_PW_Incorrect.color = new Color(255f, 0, 0,255);
        }
        else if(Register_InputField_PW.text == Register_InputField_PW_Confirm.text &&
            Register_InputField_PW.text.Length >= 4)
        {
            Register_Text_PW_Incorrect.text = "��й�ȣ�� ��ġ�մϴ�.";
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
            Debug.LogError("ȸ�����Կ� �����߽��ϴ�.");
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