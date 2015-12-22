using Android.Content;
using Java.Lang;
using Xamarin.ShortcutBadger.Infrastructure;

namespace Xamarin.ShortcutBadger.Implementations
{

	/**
	 * @author Leo Lin
	 * Deprecated, Samesung devices will use DefaultBadger
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/SamsungHomeBadger.java
	 */
	[Deprecated]
	internal class SamsungHomeBadger : BaseShortcutBadger
	{
		public SamsungHomeBadger(Context context)
			: base(context)
		{
		}
	}
}