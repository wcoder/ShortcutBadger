using System.Collections.Generic;
using Android.Content;
using Xamarin.ShortcutBadger.Infrastructure;
using Uri = Android.Net.Uri;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * Shortcut Badger support for Nova Launcher.
	 * TeslaUnread must be installed.
	 * User: Gernot Pansy
	 * Date: 2014/11/03
	 * Time: 7:15
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/NovaHomeBadger.java
	 */
	internal class NovaHomeBadger : IShortcutBadger
	{
		private const string ContentUri = "content://com.teslacoilsw.notifier/unread_count";
		private const string Count = "count";
		private const string Tag = "tag";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			var contentValues = new ContentValues();
			contentValues.Put(Tag, $"{componentName.PackageName}/{componentName.ClassName}");
			contentValues.Put(Count, badgeCount);
			context.ContentResolver.Insert(Uri.Parse(ContentUri), contentValues);
		}

		public IEnumerable<string> SupportLaunchers => new[] { "com.teslacoilsw.launcher" };

		#endregion
	}
}