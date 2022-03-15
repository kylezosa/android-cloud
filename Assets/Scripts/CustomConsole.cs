using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomConsole : Singleton<CustomConsole>
{
    [SerializeField] private RectTransform m_container;
    [SerializeField] private TextMeshProUGUI m_templateConsoleLine;
    private List<TextMeshProUGUI> m_logs = new List<TextMeshProUGUI>();
    public static void Log(string message)
    {
        // TextMeshProUGUI newLine = new GameObject("Console Message", typeof(RectTransform), typeof(TextMeshProUGUI))?.GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI newLine = Instantiate<TextMeshProUGUI>(Instance.m_templateConsoleLine, Instance.m_container);

        Instance.m_logs.Add(newLine);

        newLine.gameObject.SetActive(true);
        newLine.gameObject.name = string.Format("Console Message {0}", Instance.m_logs.Count);
        newLine.text = message;
    }
}
