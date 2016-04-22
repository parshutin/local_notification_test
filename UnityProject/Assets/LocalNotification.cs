using UnityEngine;
using System;
using System.Collections.Generic;

public class LocalNotificationManager
{
    private const string AndroidClassName = "net.agasper.unitynotification.UnityNotificationManager";

    private static string AndroidMainActivityClassName = "com.unity3d.player.UnityPlayerNativeActivity";

    private int _LastId = 0;
#if UNITY_ANDROID
    private List<int> _IdsList = new List<int>();
#endif
    private static LocalNotificationManager _Instance;

#if UNITY_IOS
        private Dictionary<int, UnityEngine.iOS.LocalNotification> _Notifications = new Dictionary<int, LocalNotification>();
#endif

    public static LocalNotificationManager Instance
    {
        get
        {
            if (_Instance == null)
            {
#if UNITY_IOS
                UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |
                                                                  UnityEngine.iOS.NotificationType.Badge |
                                                                  UnityEngine.iOS.NotificationType.Sound);
#endif
                _Instance = new LocalNotificationManager();
            }

            return _Instance;
        }
    }

    public void CreateNotification(string title, string message, TimeSpan delay)
    {
        _LastId++;
#if UNITY_IOS
            CreateIOSNotification(_LastId, title, message, delay);
#elif UNITY_ANDROID
        CreateAndroidNotification(_LastId, title, message, delay);
#endif
    }

    public void ShowInfo()
    {
#if UNITY_IOS
            NotificationTest.Text = UnityEngine.iOS.NotificationServices.scheduledLocalNotifications.Length.ToString();
        string str = "";
        foreach (var id in _Notifications.Keys)
        {
            str += id;
            str += " ";
        }

        NotificationTest.Text1 = str;
#elif UNITY_ANDROID
        NotificationTest.Text = _IdsList.Count.ToString();
        string str = "";
        foreach (var id in _IdsList)
        {
            str += id;
            str += " ";
        }

        NotificationTest.Text1 = str;
#endif
    }

    public void CancelFirst()
    {
#if UNITY_IOS
        foreach(var key in _Notifications.Keys)
        {
            CancelNotification(key);
            break;
        }
        
#elif UNITY_ANDROID
        CancelNotification(_IdsList[0]);
#endif

    }

    public void CancelAllNotifications()
    {
#if UNITY_IOS
            CancelAllIOSNotifications();
#elif UNITY_ANDROID
        CancelAllAndroidNotifications();
#endif
    }

    public void CancelNotification(int id)
    {
#if UNITY_IOS
            CancelIOSNotification(id);
#elif UNITY_ANDROID
        CancelAndroidNotification(id);
#endif
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
        _Notifications.Add(_LastId, notif);
    }

    public void CancelIOSNotification(int id)
    {
        UnityEngine.iOS.LocalNotification notification = null;
        if(_Notifications.TryGetValue(id, out notification))
        {
            UnityEngine.iOS.NotificationServices.CancelLocalNotification(notification);
            _Notifications.Remove(id);
        }
    }

    public void CancelAllIOSNotifications()
    {
        _Notifications.Clear();
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
    }
#endif

#if UNITY_ANDROID

    public enum NotificationExecuteMode
    {
        Inexact = 0,
        Exact = 1,
        ExactAndAllowWhileIdle = 2
    }

    public void CreateAndroidNotification(int id, string title, string message, TimeSpan delay)
    {
        _IdsList.Add(id);
        SendNotification(id, (int)delay.TotalSeconds, title, message, Color.red);
    }

    private void SendNotification(int id, long delay, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", NotificationExecuteMode executeMode = NotificationExecuteMode.Inexact)
    {
        AndroidJavaClass pluginClass = new AndroidJavaClass(AndroidClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetNotification", id, delay * 1000L, title, message, message, sound ? 1 : 0,
                vibrate ? 1 : 0, lights ? 1 : 0, "app_icon", "icon_notification",
                bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, (int)executeMode, AndroidMainActivityClassName);
        }
    }

    public void CreateAndroidRepeatingNotification(int id, string title, string message, TimeSpan delay, TimeSpan timeout)
    {
        SenRepeatingNotification(id, title, message, (int)delay.TotalSeconds, (int)timeout.TotalSeconds, Color.white);
    }

    private void SenRepeatingNotification(int id, string title, string message, long delay, long timeout, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
    {
        AndroidJavaClass pluginClass = new AndroidJavaClass(AndroidClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetRepeatingNotification", id, delay * 1000L, title, message, message, timeout * 1000,
                sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small",
                bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, AndroidMainActivityClassName);
        }
    }

    public void CancelAndroidNotification(int id)
    {
        _IdsList.Remove(id);
        AndroidJavaClass pluginClass = new AndroidJavaClass(AndroidClassName);
        if (pluginClass != null)
        {
            pluginClass.CallStatic("CancelNotification", id);
        }
    }

    public void CancelAllAndroidNotifications()
    {
        _IdsList.Clear();
        AndroidJavaObject pluginClass = new AndroidJavaObject(AndroidClassName);
        if (pluginClass != null)
            pluginClass.CallStatic("CancelAll");
    }
#endif
}
