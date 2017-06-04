using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Java.Lang;
using Java.Lang.Reflect;
using Xamarin.ShortcutBadger.Infrastructure;
using Xamarin.ShortcutBadger.Utils;
using Exception = Java.Lang.Exception;
using Object = Java.Lang.Object;
using String = Java.Lang.String;
using Android.App;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * @author leolin
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/XiaomiHomeBadger.java
	 */
	[Obsolete]
	internal class XiaomiHomeBadger : IShortcutBadger
	{
		private const string IntentAction = "android.intent.action.APPLICATION_MESSAGE_UPDATE";
		private const string ExtraUpdateAppComponentName = "android.intent.extra.update_application_component_name";
		private const string ExtraUpdateAppMsgText = "android.intent.extra.update_application_message_text";

		private ResolveInfo _resolveInfo;


		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			try
			{
				var miuiNotificationClass = Class.ForName("android.app.MiuiNotification");
				var miuiNotification = miuiNotificationClass.NewInstance();
				var field = miuiNotificationClass.GetDeclaredField("messageCount");
				field.Accessible = true;

				try
				{
					field.Set(miuiNotification, String.ValueOf(badgeCount == 0 ? "" : badgeCount.ToString()));
				}
				catch (Exception)
				{
					field.Set(miuiNotification, badgeCount);
				}
			}
			catch (Exception)
			{
				var localIntent = new Intent(IntentAction);
				localIntent.PutExtra(ExtraUpdateAppComponentName, $"{componentName.PackageName}/{componentName.ClassName}");
				localIntent.PutExtra(ExtraUpdateAppMsgText, String.ValueOf(badgeCount == 0 ? "" : badgeCount.ToString()));

				if (BroadcastHelper.CanResolveBroadcast(context, localIntent))
				{
					context.SendBroadcast(localIntent);
				}
			}

			if (Build.Manufacturer.Equals("Xiaomi", StringComparison.CurrentCultureIgnoreCase))
			{
				TryNewMiuiBadge(context, badgeCount);
			}
		}

		public IEnumerable<string> SupportLaunchers => new[]
		{
			"com.miui.miuilite",
			"com.miui.home",
			"com.miui.miuihome",
			"com.miui.miuihome2",
			"com.miui.mihome",
			"com.miui.mihome2",
			"com.i.miui.launcher"
		};

		#endregion

		private void TryNewMiuiBadge(Context context, int badgeCount)
		{
			if (_resolveInfo == null)
			{
				var intent = new Intent(Intent.ActionMain);
				intent.AddCategory(Intent.CategoryHome);
				_resolveInfo = context.PackageManager.ResolveActivity(intent, PackageInfoFlags.MatchDefaultOnly);
			}

			if (_resolveInfo != null)
			{
				NotificationManager mNotificationManager =
					(NotificationManager) context.GetSystemService(Context.NotificationService);
				Notification.Builder builder = new Notification.Builder(context)
					.SetContentTitle("")
					.SetContentText("")
					.SetSmallIcon(_resolveInfo.IconResource);
				Notification notification = builder.Build();
				try
				{
					Field field = notification.Class.GetDeclaredField("extraNotification");
					Object extraNotification = field.Get(notification);
					Method method = extraNotification.Class.GetDeclaredMethod("setMessageCount");
					method.Invoke(extraNotification, badgeCount);
					mNotificationManager.Notify(0, notification);
				}
				catch (Exception e)
				{
					throw new ShortcutBadgeException($"not able to set badge {e.Message}");
				}
			}
		}
	}
}