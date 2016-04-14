using UnityEngine;
using System;
using System.Collections.Generic;

public class LocalNotificationManager
{
    private const string AndroidClassName = "net.agasper.unitynotification.UnityNotificationManager";

    private const string AndroidMainActivityClassName = "com.unity3d.player.UnityPlayerNativeActivity";

    private static LocalNotificationManager _Instance;

    public static LocalNotificationManager Instance
    {
        get
        {
            if (_Instance == null)
            {
#if UNITY_IOS
                NotificationServices.RegisterForNotifications(NotificationType.Alert | NotificationType.Badge);
#endif
                _Instance = new LocalNotificationManager();
            }

            return _Instance;
        }
    }

#if UNITY_IOS
    public void CreateIOSNotification(int id,string title, string message, TimeSpan delay, UnityEngine.iOS.CalendarUnit repeatInterval = UnityEngine.iOS.CalendarUnit.Era)
    {
        var notif = new UnityEngine.iOS.LocalNotification();
        notif.userInfo = new Dictionary<string, int>();
        notif.userInfo["id"] = id;
        notif.alertAction = title;
        notif.fireDate = DateTime.Now.AddSeconds(delay.TotalSeconds);
        notif.alertBody = message;
        notif.repeatInterval = repeatInterval;
        notif.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
        UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
    }

    public void CancelIOSNotification(int id)
    {
        foreach (var notification in UnityEngine.iOS.NotificationServices.localNotifications)
        {
            if ((int)notification.userInfo["id"] == id)
            {
                UnityEngine.iOS.NotificationServices.CancelLocalNotification(notification);
            }
        }
    }

    public void CancelAllIOSNotifications()
    {
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
    }
#endif

    #if UNITY_ANDROID

    public void CreateAndroidNotification(int id, string title, string message, TimeSpan delay)
    {
        AndroidJavaClass pluginClass = new AndroidJavaClass(AndroidClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetNotification", id, delay.TotalSeconds*1000L, title, message, message, 1, 1, 1,
                0, AndroidMainActivityClassName);
        }
    }

    public void CreateAndroidRepeatingNotification(int id, string title, string message, TimeSpan delay,
        TimeSpan timeout)
    {
        AndroidJavaClass pluginClass = new AndroidJavaClass(AndroidClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetRepeatingNotification", id, delay.TotalSeconds*1000L, title, message, message,
                timeout.TotalSeconds * 1000L, 1, 1, 1, 0, AndroidMainActivityClassName);
        }
    }

    public void CancelAndroidNotification(int id)
    {
        AndroidJavaClass pluginClass = new AndroidJavaClass(AndroidClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("CancelNotification", id);
        }
    }



    public void CancelAllAndroidNotifications()
    {
        AndroidJavaClass pluginClass = new AndroidJavaClass(AndroidClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("CancelAll");
        }
    }
#endif

    /*
    public enum NotificationExecuteMode
    {
        Inexact = 0,
        Exact = 1,
        ExactAndAllowWhileIdle = 2
    }


    private static string fullClassName = "net.agasper.unitynotification.UnityNotificationManager";
    private static string mainActivityClassName = "com.unity3d.player.UnityPlayerNativeActivity";


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
        notif.repeatInterval = CalendarUnit.Minute;
        NotificationServices.ScheduleLocalNotification(notif);
        NotificationTest.Text = "SendRepeatingNotification";
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
        NotificationServices.CancelAllLocalNotifications();
        //NotificationServices.CancelLocalNotification(NotificationServices.GetLocalNotification(0));
        NotificationTest.Text = "CancelNotification";
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
    }*/
}
