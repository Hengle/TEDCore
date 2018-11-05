#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace TEDCore.Notification
{
    public class NotificationManager : MonoSingleton<NotificationManager>
    {
        private void Awake()
        {
            Clear();
        }


        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                Clear();
            }
        }


        private void Clear()
        {
            #if UNITY_IOS
            UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
            #endif
        }


        public void Schedule(int seconds, string description)
        {
            #if UNITY_IOS
            UnityEngine.iOS.LocalNotification notification = new UnityEngine.iOS.LocalNotification();
            notification.fireDate = DateTime.Now.AddSeconds(seconds);
            notification.alertBody = description;

            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notification);
            #endif
        }
    }
}