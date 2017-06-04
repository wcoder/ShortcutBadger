using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Xamarin.ShortcutBadger.Infrastructure;
using Uri = Android.Net.Uri;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * @author Jason Ling
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/HuaweiHomeBadger.java
	 */
	internal class HuaweiHomeBadger : IShortcutBadger
	{
		private const string BundlePackageName = "package";
		private const string BundleClassName = "class";
		private const string BundleBadgeCount = "badgenumber";
		private const string ContentUrl = "content://com.huawei.android.launcher.settings/badge/";
		private const string ChangeBadge = "change_badge";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			Bundle localBundle = new Bundle();
			localBundle.PutString(BundlePackageName, context.PackageName);
			localBundle.PutString(BundleClassName, componentName.ClassName);
			localBundle.PutInt(BundleBadgeCount, badgeCount);
			context.ContentResolver.Call(Uri.Parse(ContentUrl), ChangeBadge, null, localBundle);
		}

		public IEnumerable<string> SupportLaunchers => new[] { "com.huawei.android.launcher" };

		#endregion
	}
}