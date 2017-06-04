using System.Linq;
using Android.Content;

namespace Xamarin.ShortcutBadger.Utils
{
	/**
	 * Created by mahijazi on 17/05/16.
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/util/BroadcastHelper.java
	 */
	public class BroadcastHelper
	{
		public static bool CanResolveBroadcast(Context context, Intent intent)
		{
			var packageManager = context.PackageManager;
			var receivers = packageManager.QueryBroadcastReceivers(intent, 0);
			return receivers.Any();
		}
	}
}