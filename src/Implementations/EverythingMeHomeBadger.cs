using System.Collections.Generic;
using Android.Content;
using Xamarin.ShortcutBadger.Infrastructure;
using Uri = Android.Net.Uri;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * @author Radko Roman
	 * @since  13.04.17.
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/EverythingMeHomeBadger.java
	 */
	internal class EverythingMeHomeBadger : IShortcutBadger
	{
		private const string ContentUrl = "content://me.everything.badger/apps";
		private const string ColumnPackageName = "package_name";
		private const string ColumnActivityName = "activity_name";
		private const string ColumnCount = "count";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			var contentValues = new ContentValues();
			contentValues.Put(ColumnPackageName, componentName.PackageName);
			contentValues.Put(ColumnActivityName, componentName.ClassName);
			contentValues.Put(ColumnCount, badgeCount);
			context.ContentResolver.Insert(Uri.Parse(ContentUrl), contentValues);
		}

		public IEnumerable<string> SupportLaunchers => new[] {"me.everything.launcher"};

		#endregion
	}
}