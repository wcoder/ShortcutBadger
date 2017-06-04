using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Xamarin.ShortcutBadger.Infrastructure;
using Uri = Android.Net.Uri;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * @author Leo Lin
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/SonyHomeBadger.java
	 */
	internal class SonyHomeBadger : IShortcutBadger
	{
		private const string IntentAction = "com.sonyericsson.home.action.UPDATE_BADGE";
		private const string IntentExtraPackageName = "com.sonyericsson.home.intent.extra.badge.PACKAGE_NAME";
		private const string IntentExtraActivityName = "com.sonyericsson.home.intent.extra.badge.ACTIVITY_NAME";
		private const string IntentExtraMessage = "com.sonyericsson.home.intent.extra.badge.MESSAGE";
		private const string IntentExtraShowMessage = "com.sonyericsson.home.intent.extra.badge.SHOW_MESSAGE";

		private const string ProviderContentUri = "content://com.sonymobile.home.resourceprovider/badge";
		private const string ProviderColumnsBadgeCount = "badge_count";
		private const string ProviderColumnsPackageName = "package_name";
		private const string ProviderColumnsActivityName = "activity_name";
		private const string SonyHomeProviderName = "com.sonymobile.home.resourceprovider";

		private readonly Uri _badgeContentUri = Uri.Parse(ProviderContentUri);
		private AsyncQueryHandler _mQueryHandler;

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			if (SonyBadgeContentProviderExists(context))
			{
				ExecuteBadgeByContentProvider(context, componentName, badgeCount);
			}
			else
			{
				ExecuteBadgeByBroadcast(context, componentName, badgeCount);
			}
		}

		public IEnumerable<string> SupportLaunchers => new[]
		{
			"com.sonyericsson.home",
			"com.sonymobile.home"
		};

		#endregion

		private void ExecuteBadgeByBroadcast(Context context, ComponentName componentName, int badgeCount)
		{
			var intent = new Intent(IntentAction);
			intent.PutExtra(IntentExtraPackageName, componentName.PackageName);
			intent.PutExtra(IntentExtraActivityName, componentName.ClassName);
			intent.PutExtra(IntentExtraMessage, badgeCount.ToString());
			intent.PutExtra(IntentExtraShowMessage, badgeCount > 0);
			context.SendBroadcast(intent);
		}

		private void ExecuteBadgeByContentProvider(Context context, ComponentName componentName, int badgeCount)
		{
			if (badgeCount < 0)
			{
				return;
			}

			var contentValues = CreateContentValues(badgeCount, componentName);
			if (Looper.MyLooper() == Looper.MainLooper)
			{
				// We're in the main thread. Let's ensure the badge update happens in a background
				// thread by using an AsyncQueryHandler and an async update.
				if (_mQueryHandler == null)
				{
					_mQueryHandler = new SonyAsyncQueryHandler(context.ApplicationContext.ContentResolver);
				}
				InsertBadgeAsync(contentValues);
			}
			else
			{
				// Already in a background thread. Let's update the badge synchronously. Otherwise,
				// if we use the AsyncQueryHandler, this thread may already be dead by the time the
				// async execution finishes, which will lead to an IllegalStateException.
				InsertBadgeSync(context, contentValues);
			}
		}

		/// <summary>
		/// Asynchronously inserts the badge counter.
		/// </summary>
		/// <param name="contentValues">values containing the badge count, package and activity names</param>
		private void InsertBadgeAsync(ContentValues contentValues)
		{
			_mQueryHandler.StartInsert(0, null, _badgeContentUri, contentValues);
		}

		/// <summary>
		/// Synchronously inserts the badge counter.
		/// </summary>
		/// <param name="context">Caller context</param>
		/// <param name="contentValues">Content values containing the badge count, package and activity names</param>
		private void InsertBadgeSync(Context context, ContentValues contentValues)
		{
			context.ApplicationContext.ContentResolver.Insert(_badgeContentUri, contentValues);
		}

		/// <summary>
		/// Creates a ContentValues object to be used in the badge counter update. The package and
		/// activity names must correspond to an activity that holds an intent filter with action
		/// "android.intent.action.MAIN" and category android.intent.category.LAUNCHER" in the manifest.
		/// Also, it is not allowed to publish badges on behalf of another client, so the package and
		/// activity names must belong to the process from which the insert is made.
		/// To be able to insert badges, the app must have the PROVIDER_INSERT_BADGE
		/// permission in the manifest file.In case these conditions are not
		/// fulfilled, or any content values are missing, there will be an unhandled
		/// exception on the background thread.
		/// </summary>
		/// <param name="badgeCount">the badge count</param>
		/// <param name="componentName">the component name from which package and class name will be extracted</param>
		private ContentValues CreateContentValues(int badgeCount, ComponentName componentName)
		{
			var contentValues = new ContentValues();
			contentValues.Put(ProviderColumnsBadgeCount, badgeCount);
			contentValues.Put(ProviderColumnsPackageName, componentName.PackageName);
			contentValues.Put(ProviderColumnsActivityName, componentName.ClassName);
			return contentValues;
		}

		/// <summary>
		/// Check if the latest Sony badge content provider exists .
		/// </summary>
		/// <param name="context">the context to use</param>
		/// <returns>true if Sony badge content provider exists, otherwise false.</returns>
		private static bool SonyBadgeContentProviderExists(Context context)
		{
			bool exists = false;
			var info = context.PackageManager.ResolveContentProvider(SonyHomeProviderName, 0);
			if (info != null)
			{
				exists = true;
			}
			return exists;
		}

		class SonyAsyncQueryHandler : AsyncQueryHandler
		{
			public SonyAsyncQueryHandler(ContentResolver cr)
				: base(cr)
			{
			}
		}
	}
}