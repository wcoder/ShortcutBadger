using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Xamarin.ShortcutBadger.Infrastructure;
using Uri = Android.Net.Uri;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * Created by wuxuejian on 2016/10/9.
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/ZukHomeBadger.java
	 */
	internal class ZukHomeBadger : IShortcutBadger
	{
		private const string ContentUrl = "content://com.android.badge/badge";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			var extra = new Bundle();
			extra.PutInt("app_badge_count", badgeCount);
			context.ContentResolver.Call(Uri.Parse(ContentUrl), "setAppBadgeCount", null, extra);
		}

		public IEnumerable<string> SupportLaunchers => new[] { "com.zui.launcher" };

		#endregion
	}
}