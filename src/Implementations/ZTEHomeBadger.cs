using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Xamarin.ShortcutBadger.Infrastructure;
using Uri = Android.Net.Uri;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/ZTEHomeBadger.java
	 */
	internal class ZTEHomeBadger : IShortcutBadger
	{
		private const string BundleBadgeCount = "app_badge_count";
		private const string BundleComponentName = "app_badge_component_name";
		private const string ContentUrl = "content://com.android.launcher3.cornermark.unreadbadge";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			var extra = new Bundle();
			extra.PutInt(BundleBadgeCount, badgeCount);
			extra.PutString(BundleComponentName, componentName.FlattenToString());

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
			{
				context.ContentResolver.Call(Uri.Parse(ContentUrl), "setAppUnreadCount", null, extra);
			}
		}

		public IEnumerable<string> SupportLaunchers => new string[] {};

		#endregion
	}
}
 