using System.Collections.Generic;
using Android.Content;
using Xamarin.ShortcutBadger.Infrastructure;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * @author leolin
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/VivoHomeBadger.java
	 */
	internal class VivoHomeBadger : IShortcutBadger
	{
		private const string IntentAction = "launcher.action.CHANGE_APPLICATION_NOTIFICATION_NUM";
		private const string IntentPackageName = "packageName";
		private const string IntentClassName = "className";
		private const string IntentNotificationNum = "notificationNum";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			var intent = new Intent(IntentAction);
			intent.PutExtra(IntentPackageName, context.PackageName);
			intent.PutExtra(IntentClassName, componentName.ClassName);
			intent.PutExtra(IntentNotificationNum, badgeCount);
			context.SendBroadcast(intent);
		}

		public IEnumerable<string> SupportLaunchers => new[] {"com.vivo.launcher"};

		#endregion
	}
}