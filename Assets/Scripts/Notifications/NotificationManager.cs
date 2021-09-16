using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Notifications.Android;

using TMPro;

public class NotificationManager : MonoBehaviour
{
    public UnityEvent OnDataNotifPressed;
    //Minutes
    [SerializeField] int intervalTime = 10;
    [SerializeField] Slider intervalSlider;
    [SerializeField] TextMeshProUGUI intervalText;
    public void SetIntervalTime()
    {
        intervalTime = (int)intervalSlider.value;
        intervalText.text = $"{intervalTime} minutes";
    }

    public void SendNotification()
    {
        string notif_title = "Simple Notification";
        string notif_message = "This is a simple notification";
        DateTime fireTime = DateTime.Now.AddSeconds(10);

        AndroidNotification notif = new AndroidNotification(notif_title, notif_message, fireTime);

        notif.SmallIcon = "icon_small";
        notif.LargeIcon = "icon_large";

        AndroidNotificationCenter.SendNotification(notif, "default");
    }

    public void SendRepeatingNotification()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        string notif_title = "Repeating Notification";
        string notif_message = "This is a repeating notification";
        DateTime fireTime = DateTime.Now.AddSeconds(5);
        TimeSpan interval = new TimeSpan(0, intervalTime, 0);

        AndroidNotification notif = new AndroidNotification(notif_title, notif_message, fireTime, interval);

        notif.SmallIcon = "icon_small";
        notif.LargeIcon = "icon_large";

        AndroidNotificationCenter.SendNotification(notif, "repeat");
    }

    public void SendDataNotification()
    {
        string notif_title = "Get 10G!";
        string notif_message = "Play now to get 10G!";
        DateTime fireTime = DateTime.Now.AddSeconds(10);

        AndroidNotification notif = new AndroidNotification(notif_title, notif_message, fireTime);

        notif.SmallIcon = "icon_small";
        notif.LargeIcon = "icon_large";

        //Gets this data when notification is pressed, can also add reward data
        notif.IntentData = "data";

        AndroidNotificationCenter.SendNotification(notif, "default");
    }

    private void CheckIntentData()
    {
        //Gets the last notification intent
        AndroidNotificationIntentData data = AndroidNotificationCenter.GetLastNotificationIntent();

        if(data != null)
        {
            OnDataNotifPressed.Invoke();
        }
    }

    private void SetUpDefaultChannel()
    {
        //Unique ID of the channel
        //Must have another one with the same name ingame
        string channel_id = "default";

        //Name of the channel
        //Will show up in the settings
        string channel_title = "Default Channel";

        //Description of this channel
        string channel_description = "Default Channel for this game";

        //Importance of this channel
        Importance importance = Importance.Default;

        AndroidNotificationChannel defaultChannel = new AndroidNotificationChannel(channel_id, channel_title, channel_description, importance);

        AndroidNotificationCenter.RegisterNotificationChannel(defaultChannel);
    }

    private void SetUpRepeatingChannel()
    {
        //Unique ID of the channel
        //Must have another one with the same name ingame
        string channel_id = "repeat";

        //Name of the channel
        //Will show up in the settings
        string channel_title = "Repeat Channel";

        //Description of this channel
        string channel_description = "Repeat Channel for this game";

        //Importance of this channel
        Importance importance = Importance.Default;

        AndroidNotificationChannel repeatingChannel = new AndroidNotificationChannel(channel_id, channel_title, channel_description, importance);

        AndroidNotificationCenter.RegisterNotificationChannel(repeatingChannel);
    }

    public void CancelNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }

    private void Awake()
    {
        SetUpDefaultChannel();
        SetUpRepeatingChannel();

        AndroidNotificationCenter.CancelAllNotifications();
    }

    private void Start()
    {
        CheckIntentData();
    }
}
