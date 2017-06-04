using System.Collections.Generic;
using Android.Content;
using Xamarin.ShortcutBadger.Infrastructure;
using Xamarin.ShortcutBadger.Utils;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * @author Gernot Pansy
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/ApexHomeBadger.java
	 */
	internal class ApexHomeBadger : IShortcutBadger
	{
		private const string IntentUpdateCounter = "com.anddoes.launcher.COUNTER_CHANGED";
		private const string PackageName = "package";
		private const string Count = "count";
		private const string Class = "class";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			var intent = new Intent(IntentUpdateCounter);
			intent.PutExtra(PackageName, componentName.PackageName);
			intent.PutExtra(Count, badgeCount);
			intent.PutExtra(Class, componentName.ClassName);

			if (BroadcastHelper.CanResolveBroadcast(context, intent))
			{
				context.SendBroadcast(intent);
			}
			else
			{
				throw new ShortcutBadgeException("unable to resolve intent: " + intent);
			}
		}

		public IEnumerable<string> SupportLaunchers => new[] { "com.anddoes.launcher" };

		#endregion
	}
}