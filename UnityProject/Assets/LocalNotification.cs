using UnityEngine;
using System;

using NotificationServices = UnityEngine.iOS.NotificationServices;
using LocalNotification = UnityEngine.iOS.LocalNotification;


public class LocalNotificationManager
{
    private static LocalNotificationManager _Instance;

    public static LocalNotificationManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new LocalNotificationManager();
            }

            return _Instance;
        }
    }

    public enum NotificationExecuteMode
    {
        Inexact = 0,
        Exact = 1,
        ExactAndAllowWhileIdle = 2
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static string fullClassName = "net.agasper.unitynotification.UnityNotificationManager";
    private static string mainActivityClassName = "com.unity3d.player.UnityPlayerNativeActivity";
#endif

    public static void SendNotification(int id, TimeSpan delay, string title, string message)
    {
        SendNotification(id, (int)delay.TotalSeconds, title, message, Color.white);
    }
    
    public static void SendNotification(int id, long delay, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", NotificationExecuteMode executeMode = NotificationExecuteMode.Inexact)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetNotification", id, delay * 1000L, title, message, message, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, (int)executeMode, mainActivityClassName);
        }
#elif UNITY_IOS && !UNITY_EDITOR
        var notif = new LocalNotification();
        notif.alertAction = title;
        notif.fireDate = DateTime.Now.AddSeconds(delay);
        notif.alertBody = message;
        NotificationServices.ScheduleLocalNotification(notif);
#endif
    }

    public static void SendRepeatingNotification(int id, long delay, long timeout, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetRepeatingNotification", id, delay * 1000L, title, message, message, timeout * 1000, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, mainActivityClassName);
        }
#elif UNITY_IOS && !UNITY_EDITOR
        var notif = new LocalNotification();
        notif.alertAction = title;
        notif.fireDate = DateTime.Now.AddSeconds(delay);
        notif.alertBody = message;
        //notif.repeatInterval = 
        NotificationServices.ScheduleLocalNotification(notif);
#endif
    }

    public static void CancelNotification(int id)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null) {
            pluginClass.CallStatic("CancelNotification", id);
        }
#elif UNITY_IOS && !UNITY_EDITOR

        NotificationServices.CancelLocalNotification(NotificationServices.GetLocalNotification(0));
#endif

    }

    public static void CancelAllNotifications()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null)
            pluginClass.CallStatic("CancelAll");
#elif UNITY_IOS && !UNITY_EDITOR
        NotificationServices.CancelAllLocalNotifications();
#endif
    }
}
