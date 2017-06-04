using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.ShortcutBadger.Infrastructure;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * Created by NingSo on 2016/10/14.上午10:09
	 *
	 * @author: NingSo
	 * Email: ningso.ping@gmail.com
	 * <p>
	 * OPPO R9 not supported
	 * Version number 6 applies only to chat-type apps
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/impl/OPPOHomeBader.java
	 */
	internal class OPPOHomeBader : IShortcutBadger
	{
		
		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> SupportLaunchers { get; }
	}
}