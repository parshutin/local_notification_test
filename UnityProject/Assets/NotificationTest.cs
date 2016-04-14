using System;
using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

public class NotificationTest : MonoBehaviour {

    float sleepUntil = 0;
    public static string Text = "";

    private int count = 0;

    private void Start()
    {
        #if UNITY_IOS
        NotificationServices.RegisterForNotifications(NotificationType.Alert | NotificationType.Badge);
#endif
    }
	void OnGUI () {
        //Color is supported only in Android >= 5.0
        GUI.enabled = sleepUntil < Time.time;
        GUI.Label(new Rect(500, 250, 400, 200), Text);
        if (GUILayout.Button("5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
            #if UNITY_ANDROID
                LocalNotificationManager.Instance.CreateAndroidNotification(count, "Title", "Long message text", TimeSpan.FromSeconds(5));
            #elif UNITY_IOS
                LocalNotificationManager.Instance.CreateIOSNotification(count, "Title", "Long message text", TimeSpan.FromSeconds(5));
            #endif
            count++;
            sleepUntil = Time.time + 5;
        }

        if (GUILayout.Button("30 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
#if UNITY_ANDROID
                LocalNotificationManager.Instance.CreateAndroidNotification(count, "Title", "Long message text", TimeSpan.FromSeconds(30));
#elif UNITY_IOS
                LocalNotificationManager.Instance.CreateIOSNotification(count, "Title", "Long message text", TimeSpan.FromSeconds(30));
#endif
            sleepUntil = Time.time + 30;
            count++;
        }

        if (GUILayout.Button("EVERY 60 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
#if UNITY_ANDROID
                LocalNotificationManager.Instance.CreateAndroidRepeatingNotification(count, "Title", "Long message text", TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(60));
#elif UNITY_IOS
                LocalNotificationManager.Instance.CreateIOSNotification(count, "Title", "Long message text", TimeSpan.FromSeconds(5), UnityEngine.iOS.CalendarUnit.Minute);
#endif
            sleepUntil = Time.time + 99999;
        }

        GUI.enabled = true;

        if (GUILayout.Button("STOP", GUILayout.Height(Screen.height * 0.2f)))
        {
#if UNITY_ANDROID
            LocalNotificationManager.Instance.CancelAndroidNotification(count);
#elif UNITY_IOS
            LocalNotificationManager.Instance.CancelIOSNotification(count);
#endif
        }

        if (GUILayout.Button("STOP ALL", GUILayout.Height(Screen.height * 0.2f)))
        {
#if UNITY_ANDROID
            LocalNotificationManager.Instance.CancelAllAndroidNotifications();
#elif UNITY_IOS
            LocalNotificationManager.Instance.CancelAllIOSNotifications();
#endif
        }
	}
}
