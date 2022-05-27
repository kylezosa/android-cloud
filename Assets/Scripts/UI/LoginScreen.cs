using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScreen : UIScreen
{
    public override UIScreenType ScreenType { get => UIScreenType.Login; }
    private void Start()
    {
        Login();
    }
    public void Login()
    {
        CuConsole.Log(string.Format("Login called."), logArea: CuArea.LoginScreen);

        GooglePlayManager.Instance.Login((success) =>
        {
            if (success)
            {
                CuConsole.Log(string.Format("Login success."), logArea: CuArea.LoginScreen);

                GoToMainMenu();
            }
            else
            {
                CuConsole.Log(string.Format("Login failed."), logArea: CuArea.LoginScreen);
            }
        });
    }
}
