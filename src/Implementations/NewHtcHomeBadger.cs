using System.Collections.Generic;
using Android.Content;
using Xamarin.ShortcutBadger.Infrastructure;
using Xamarin.ShortcutBadger.Utils;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * @author Leo Lin
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/NewHtcHomeBadger.java
	 */
	internal class NewHtcHomeBadger : IShortcutBadger
	{
		private const string IntentUpdateShortcut = "com.htc.launcher.action.UPDATE_SHORTCUT";
		private const string IntentSetNotification = "com.htc.launcher.action.SET_NOTIFICATION";
		private const string PackageName = "packagename";
		private const string Count = "count";
		private const string ExtraComponent = "com.htc.launcher.extra.COMPONENT";
		private const string ExtraCount = "com.htc.launcher.extra.Count";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			var intent1 = new Intent(IntentSetNotification);
			intent1.PutExtra(ExtraComponent, componentName.FlattenToShortString());
			intent1.PutExtra(ExtraCount, badgeCount);

			var intent = new Intent(IntentUpdateShortcut);
			intent.PutExtra(PackageName, componentName.PackageName);
			intent.PutExtra(Count, badgeCount);

			if (BroadcastHelper.CanResolveBroadcast(context, intent1)
				|| BroadcastHelper.CanResolveBroadcast(context, intent))
			{
				context.SendBroadcast(intent1);
				context.SendBroadcast(intent);
			}
			else
			{
				throw new ShortcutBadgeException("unable to resolve intent: " + intent);
			}
		}

		public IEnumerable<string> SupportLaunchers => new[] { "com.htc.launcher" };

		#endregion
	}
}