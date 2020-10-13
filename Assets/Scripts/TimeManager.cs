//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Timer counts down 1 min. If it finds a UI Text element named TimeGUI, it updates
 * the TimeGUI text, but it works fine without TimeGUI.
 */
public class TimeManager : MonoBehaviour
{

    Text timeText;
    float time = 60;

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform timeGUI = canvas.transform.Find("TimeGUI");
            if (timeGUI != null)
            {
                timeText = timeGUI.GetComponent<Text>();
                SyncTimeGUI();
            }
        }
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            time = 0;
        }
        SyncTimeGUI();
    }

    public void AddTime(float deltaTime)
    {
        time += deltaTime;
    }

    void SyncTimeGUI()
    {
        if (timeText != null)
        {
            timeText.text = ((int)time).ToString();
        }
    }
}