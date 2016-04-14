using UnityEngine;
using System.Collections;
using UnityEngine.iOS;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using LocalNotification = UnityEngine.iOS.LocalNotification;
public class NotificationTest : MonoBehaviour {

    float sleepUntil = 0;
    public static string Text = "";

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
            LocalNotificationManager.SendNotification(1, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
            sleepUntil = Time.time + 5;
        }

        if (GUILayout.Button("5 SECONDS BIG ICON", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.SendNotification(1, 5, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
            sleepUntil = Time.time + 5;
        }

        if (GUILayout.Button("EVERY 5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.SendRepeatingNotification(1, 5, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
            sleepUntil = Time.time + 99999;
        }

        if (GUILayout.Button("10 SECONDS EXACT", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.SendNotification(1, 10, "Title", "Long exact message text", new Color32(0xff, 0x44, 0x44, 255), executeMode: LocalNotificationManager.NotificationExecuteMode.ExactAndAllowWhileIdle);
            sleepUntil = Time.time + 10;
        }

        GUI.enabled = true;

        if (GUILayout.Button("STOP", GUILayout.Height(Screen.height * 0.2f)))
        {
            LocalNotificationManager.CancelNotification(1);
            sleepUntil = 0;
        }
        
	}
}
