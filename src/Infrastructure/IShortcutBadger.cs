using System.Collections.Generic;
using Android.Content;

namespace Xamarin.ShortcutBadger.Infrastructure
{
	// Original:
	// https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/Badger.java
	public interface IShortcutBadger
	{
		/// <summary>
		/// Called to let {ShortcutBadger} knows which launchers are supported by this badger. It should return a
		/// </summary>
		/// <returns>List containing supported launchers package names</returns>
		IEnumerable<string> SupportLaunchers { get; }

		/// <summary>
		/// Called when user attempts to update notification count
		/// </summary>
		/// <param name="context">context</param>
		/// <param name="componentName">containing package and class name of calling application's launcher activity</param>
		/// <param name="badgeCount">notification count</param>
		/// <exception cref="ShortcutBadgeException"></exception>
		void ExecuteBadge(Context context, ComponentName componentName, int badgeCount);
	}
}

