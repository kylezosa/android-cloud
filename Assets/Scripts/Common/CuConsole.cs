using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CuConsole : Singleton<CuConsole>
{
    [SerializeField] private Canvas m_consoleCanvas = null;
    [SerializeField] private RectTransform m_consoleContainer = null;
    [SerializeField] private RectTransform m_logContainer = null;
    [SerializeField] private TextMeshProUGUI m_templateConsoleLine = null;

    [SerializeField] private RectTransform m_minimizeButtonRect = null;
    [SerializeField] private TextMeshProUGUI m_minimizeButtonText = null;

    private bool m_bIsMinimized = true;

    private List<CuLog> m_logs = new List<CuLog>();

    public bool IsMinimized { get => m_bIsMinimized; private set => m_bIsMinimized = value; }

    protected override void Awake()
    {
        base.Awake();

        m_consoleCanvas = m_consoleCanvas != null ? m_consoleCanvas : GetComponent<Canvas>();

        if (m_consoleCanvas != null)
        {
            m_consoleCanvas.worldCamera = Camera.main;
        }

        SetViewState(true);
    }

    public void ToggleViewState()
    {
        SetViewState(!IsMinimized);
    }

    public bool SetViewState(bool p_bMinimize)
    {
        if (p_bMinimize != IsMinimized)
        {
            // Do change state stuff
            m_bIsMinimized = p_bMinimize;

            if (p_bMinimize)
            {
                m_consoleContainer.anchorMin = new Vector2(0, -1);
                m_consoleContainer.anchorMax = new Vector2(1, 0);

                m_minimizeButtonText.SetText("Show Console");
                m_minimizeButtonRect.anchorMin = new Vector2(0.5f, 0);
                m_minimizeButtonRect.anchorMax = new Vector2(0.5f, 0);
                m_minimizeButtonRect.pivot = new Vector2(0.5f, 0);
            }
            else
            {
                m_consoleContainer.anchorMin = new Vector2(0, 0);
                m_consoleContainer.anchorMax = new Vector2(1, 1);

                m_minimizeButtonText.SetText("Hide Console");
                m_minimizeButtonRect.anchorMin = new Vector2(0.5f, 1);
                m_minimizeButtonRect.anchorMax = new Vector2(0.5f, 1);
                m_minimizeButtonRect.pivot = new Vector2(0.5f, 1);
            }
        }

        return IsMinimized;
    }
    public static void Log(object message, CuLogType logType = CuLogType.Log, CuArea logArea = CuArea.General)
    {
        Debug.Log(String.Format("[{0}]: [{1}] {2}", logType.ToString(), logArea.ToString(), message.ToString()));

        TextMeshProUGUI newLine = Instantiate<TextMeshProUGUI>(Instance.m_templateConsoleLine, Instance.m_logContainer);
        CuLog newLog = new CuLog(
                message,
                p_logType: logType,
                p_logArea: logArea,
                p_uiElement: newLine.gameObject
            );

        Instance.m_logs.Add(newLog);

        newLine.gameObject.SetActive(true);
        newLine.gameObject.name = string.Format("Console Message {0}", Instance.m_logs.Count);
        newLine.text = String.Format("[{0}][{1}]: [{2}] {3}", newLog.timeStamp.ToString(), logType.ToString(), logArea.ToString(), message.ToString());
    }
}

public enum CuLogType
{
    Log,
    Warning,
    Error,
}
public enum CuArea
{
    General,
    CloudTests,
    GooglePlayManager,
    SaveManager,
    LoginScreen,
    MainMenuScreen,
    SaveScreen,
    LeaderboardsScreen,
    AchievementsScreen,
    UIScreen,
    IAPManager
}
public struct CuLog
{
    public DateTime? timeStamp;
    public CuLogType logType;
    public CuArea logArea;
    public object message;
    public GameObject uiElement;
    public CuLog(object p_message, CuLogType p_logType = CuLogType.Log, CuArea p_logArea = CuArea.General, GameObject p_uiElement = null, DateTime? p_timeStamp = null)
    {
        message = p_message;
        logType = p_logType;
        logArea = p_logArea;
        uiElement = p_uiElement;
        timeStamp = p_timeStamp != null ? p_timeStamp : DateTime.Now;
    }
}