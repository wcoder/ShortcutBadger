using System.Collections.Generic;
using Android.Content;
using Xamarin.ShortcutBadger.Infrastructure;
using Xamarin.ShortcutBadger.Utils;

namespace Xamarin.ShortcutBadger.Implementations
{
	/**
	 * @author Gernot Pansy
	 */
	internal class AdwHomeBadger : IShortcutBadger
	{
		private const string IntentUpdateCounter = "org.adw.launcher.counter.SEND";
		private const string PackageName = "PNAME";
		private const string ClassName = "CNAME";
		private const string Count = "Count";

		#region IShortcutBadger implementation

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			var intent = new Intent(IntentUpdateCounter);
			intent.PutExtra(PackageName, componentName.PackageName);
			intent.PutExtra(ClassName, componentName.ClassName);
			intent.PutExtra(Count, badgeCount);
			
			if (BroadcastHelper.CanResolveBroadcast(context, intent))
			{
				context.SendBroadcast(intent);
			}
			else
			{
				throw new ShortcutBadgeException("unable to resolve intent: " + intent);
			}
		}

		public IEnumerable<string> SupportLaunchers => new []
		{
			"org.adw.launcher",
			"org.adwfreak.launcher"
		};

		#endregion
	}
}