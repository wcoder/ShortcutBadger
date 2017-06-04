using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.OS;
using Xamarin.ShortcutBadger.Infrastructure;
using Xamarin.ShortcutBadger.Utils;
using Uri = Android.Net.Uri;

namespace Xamarin.ShortcutBadger.Implementations
{

	/**
	 * @author Leo Lin
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/SamsungHomeBadger.java
	 */
	[Obsolete("Deprecated, Samesung devices will use DefaultBadger")]
	internal class SamsungHomeBadger : IShortcutBadger
	{
		private const string ContentUrl = "content://com.sec.badge/apps?notify=true";
		private string[] _contentProjection = {"_id", "class"};
		private DefaultBadger _defaultBadger;

		public SamsungHomeBadger()
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				_defaultBadger = new DefaultBadger();
			}
		}

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			if (_defaultBadger != null && _defaultBadger.IsSupported(context))
			{
				_defaultBadger.ExecuteBadge(context, componentName, badgeCount);
			}
			else
			{
				var mUri = Uri.Parse(ContentUrl);
				var contentResolver = context.ContentResolver;
				ICursor cursor = null;
				try
				{
					cursor = contentResolver.Query(mUri, _contentProjection, "package=?", new[] { componentName.PackageName }, null);
					if (cursor != null)
					{
						var entryActivityName = componentName.ClassName;
						var entryActivityExist = false;
						while (cursor.MoveToNext())
						{
							var id = cursor.GetInt(0);
							var contentValues = GetContentValues(componentName, badgeCount, false);
							contentResolver.Update(mUri, contentValues, "_id=?", new[] { id.ToString() });
							if (entryActivityName.Equals(cursor.GetString(cursor.GetColumnIndex("class"))))
							{
								entryActivityExist = true;
							}
						}

						if (!entryActivityExist)
						{
							var contentValues = GetContentValues(componentName, badgeCount, true);
							contentResolver.Insert(mUri, contentValues);
						}
					}
				}
				finally
				{
					CloseHelper.Close(cursor);
				}
			}
		}

		public IEnumerable<string> SupportLaunchers => new[]
		{
			"com.sec.android.app.launcher",
			"com.sec.android.app.twlauncher"
		};


		private ContentValues GetContentValues(ComponentName componentName, int badgeCount, bool isInsert)
		{
			var contentValues = new ContentValues();
			if (isInsert)
			{
				contentValues.Put("package", componentName.PackageName);
				contentValues.Put("class", componentName.ClassName);
			}
			contentValues.Put("badgecount", badgeCount);
			return contentValues;
		}
	}
}