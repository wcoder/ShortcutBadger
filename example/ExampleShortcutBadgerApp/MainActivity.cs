using XamarinShortcutBadger;

namespace ExampleShortcutBadgerApp;

[Activity(Label = "ExampleShortcutBadgerApp", MainLauncher = true, Icon = "@drawable/icon")]
public class MainActivity : Activity
{
    protected override void OnCreate(Bundle? bundle)
    {
        base.OnCreate(bundle);

        SetContentView(Resource.Layout.Main);

        var editText = FindViewById<EditText>(Resource.Id.badgeValue);
        var button = FindViewById<Button>(Resource.Id.updateButton);

        button!.Click += delegate
        {
            var count = int.Parse(editText!.Text!);
            var isSupported = ShortcutBadger.IsBadgeCounterSupported(this);

            if (isSupported)
            {
                ShortcutBadger.ApplyCount(this, count);

                Toast.MakeText(this, "Success!", ToastLength.Short);
            }
            else
            {
                Toast.MakeText(this, "Not supported!", ToastLength.Short);
            }
				


            // For Xiaomi:
            // From https://github.com/leolin310148/ShortcutBadger/blob/master/SampleApp/src/main/java/me/leolin/shortcutbadger/example/BadgeIntentService.java
            /*
            var notificationId = 1;
            var mNotificationManager = (NotificationManager)GetSystemService(NotificationService);
            var builder = new Notification.Builder(ApplicationContext)
                .SetContentTitle("")
                .SetContentText("")
                .SetSmallIcon(Resource.Drawable.Icon);
            Notification notification = builder.Build();
            ShortcutBadger.ApplyNotification(ApplicationContext, notification, count);
            mNotificationManager.Notify(notificationId, notification);
            */
        };
    }
}

