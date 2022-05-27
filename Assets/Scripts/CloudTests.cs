using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using System.Linq;

public class CloudTests : Singleton<CloudTests>
{
    [SerializeField] private List<UIScreen> m_uiScreensList = new List<UIScreen>();

    private UIScreen m_currentScreen = null;

    private Dictionary<UIScreenType, UIScreen> m_uiScreensDict = new Dictionary<UIScreenType, UIScreen>();

    protected override void Awake()
    {
        base.Awake();

        if (m_uiScreensList.Count < 1)
        {
            m_uiScreensList = GetComponentsInChildren<UIScreen>(true).ToList();
        }

        foreach (UIScreen screen in m_uiScreensList)
        {
            m_uiScreensDict.Add(screen.ScreenType, screen);
        }
    }

    private void Tick()
    {
        if (!Social.localUser.authenticated && m_currentScreen?.ScreenType != UIScreenType.Login)
        {
            SetScreen(UIScreenType.Login);
        }
        else if (Social.localUser.authenticated && m_currentScreen?.ScreenType == UIScreenType.Login)
        {
            SetScreen(UIScreenType.MainMenu);
        }
    }

    public void SetScreen(UIScreenType p_screenType)
    {
        CuConsole.Log(string.Format("Set Screen {0} called.", p_screenType), logArea: CuArea.CloudTests);

        foreach (KeyValuePair<UIScreenType, UIScreen> entry in m_uiScreensDict)
        {
            if (entry.Key == p_screenType)
            {
                entry.Value.Show();
            }
            else if (entry.Value.IsVisible)
            {
                entry.Value.Hide();
            }
        }
    }
}
