using System;
using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

public class NotificationTest : MonoBehaviour
{

    float sleepUntil = 0;
    public static string Text = "";
    public static string Text1 = "";


    void OnGUI()
    {
        //Color is supported only in Android >= 5.0
        GUI.enabled = sleepUntil < Time.time;
        GUI.Label(new Rect(500, 250, 400, 200), Text);
        GUI.Label(new Rect(500, 550, 400, 200), Text1);
        if (GUILayout.Button("5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.Instance.CreateNotification("Title", "message", TimeSpan.FromSeconds(5));
            sleepUntil = Time.time + 1;
        }

        if (GUILayout.Button("30 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.Instance.CreateNotification("Title1", "messag1111", TimeSpan.FromSeconds(30));
            sleepUntil = Time.time + 1;
        }

        if (GUILayout.Button("Cancel last", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.Instance.CancelFirst();
            sleepUntil = Time.time + 1;
        }

        GUI.enabled = true;

        if (GUILayout.Button("ShowInfo", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.Instance.ShowInfo();
            sleepUntil = Time.time + 1;
        }

        if (GUILayout.Button("STOP ALL", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.Instance.CancelAllNotifications();
            sleepUntil = Time.time + 1;
        }
    }
}
