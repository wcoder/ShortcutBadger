using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.App;
using Xamarin.ShortcutBadger.Implementations;
using Xamarin.ShortcutBadger.Infrastructure;

namespace Xamarin.ShortcutBadger
{
	/**
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/ShortcutBadger.java
	 */
	public static class ShortcutBadger
	{
		private const string LogTag = "ShortcutBadger";
		private const int SupportedCheckAttempts = 3;

		private static bool _sIsBadgeCounterSupported;
		private static readonly object SCounterSupportedLock = new object();

		private static IShortcutBadger _sShortcutBadger;
		private static ComponentName _sComponentName;

		private static readonly IEnumerable<Type> Badgers = new List<Type>
		{
			typeof(AdwHomeBadger),
			typeof(ApexHomeBadger),
			typeof(AsusHomeLauncher),
			typeof(DefaultBadger),
			typeof(EverythingMeHomeBadger),
			typeof(HuaweiHomeBadger),
			typeof(LgHomeBadger),
			typeof(NewHtcHomeBadger),
			typeof(NovaHomeBadger),
			typeof(OPPOHomeBader),
			typeof(SamsungHomeBadger),
			typeof(SonyHomeBadger),
			typeof(VivoHomeBadger),
			typeof(XiaomiHomeBadger),
			typeof(ZTEHomeBadger),
			typeof(ZukHomeBadger)
		};


		/// <summary>
		/// Tries to update the notification count
		/// </summary>
		/// <param name="context">context</param>
		/// <param name="badgeCount">Desired badge count</param>
		/// <returns>
		/// true in case of success, false otherwise
		/// </returns>
		public static bool ApplyCount(Context context, int badgeCount)
		{
			try
			{
				ApplyCountOrThrow(context, badgeCount);
				return true;
			}
			catch (ShortcutBadgeException e)
			{
				if (Log.IsLoggable(LogTag, LogPriority.Debug))
				{
					Log.Debug(LogTag, "Unable to execute badge", e);
				}
				return false;
			}
		}


		/// <summary>
		/// Tries to update the notification count
		/// </summary>
		/// <exception cref="ShortcutBadgeException"></exception>
		/// <param name="context">context</param>
		/// <param name="badgeCount">badge count</param>
		public static void ApplyCountOrThrow(Context context, int badgeCount)
		{
			if (_sShortcutBadger == null)
			{
				var launcherReady = InitBadger(context);

				if (!launcherReady)
					throw new ShortcutBadgeException("No default launcher available");
			}

			try
			{
				_sShortcutBadger.ExecuteBadge(context, _sComponentName, badgeCount);
			}
			catch (Exception e)
			{
				throw new ShortcutBadgeException($"Unable to execute badge {e.Message}");
			}
		}

		/// <summary>
		/// Tries to remove the notification count
		/// </summary>
		/// <param name="context">context</param>
		/// <returns>
		/// true in case of success, false otherwise
		/// </returns>
		public static bool RemoveCount(Context context)
		{
			return ApplyCount(context, 0);
		}

		/// <summary>
		/// Tries to remove the notification count
		/// </summary>
		/// <exception cref="ShortcutBadgeException"></exception>
		/// <param name="context"></param>
		public static void RemoveCountOrThrow(Context context)
		{
			ApplyCountOrThrow(context, 0);
		}

		/// <summary>
		/// Whether this platform launcher supports shortcut badges. Doing this check causes the side
		/// effect of resetting the counter if it's supported, so this method should be followed by
		/// a call that actually sets the counter to the desired value, if the counter is supported.
		/// </summary>
		public static bool IsBadgeCounterSupported(Context context)
		{
			// Checking outside synchronized block to avoid synchronization
			// in the common case (flag already set), and improve perf.
			if (!_sIsBadgeCounterSupported)
			{
				lock (SCounterSupportedLock)
				{
					// Checking again inside synch block to avoid setting the flag twice.
					if (!_sIsBadgeCounterSupported)
					{
						string lastErrorMessage = null;
						for (int i = 0; i < SupportedCheckAttempts; i++)
						{
							try
							{
								Log.Info(LogTag, $"Checking if platform supports badge counters, attempt {i + 1}/{SupportedCheckAttempts}.");
								if (InitBadger(context))
								{
									_sShortcutBadger.ExecuteBadge(context, _sComponentName, 0);
									_sIsBadgeCounterSupported = true;
									Log.Info(LogTag, "Badge counter is supported in this platform.");
									break;
								}
								else
								{
									lastErrorMessage = "Failed to initialize the badge counter.";
								}
							}
							catch (Exception e)
							{
								// Keep retrying as long as we can. No need to dump the stack trace here
								// because this error will be the norm, not exception, for unsupported
								// platforms. So we just save the last error message to display later.
								lastErrorMessage = e.Message;
							}
						}

						if (!_sIsBadgeCounterSupported)
						{
							Log.Warn(LogTag, $"Badge counter seems not supported for this platform: {lastErrorMessage}");
							_sIsBadgeCounterSupported = false;
						}
					}
				}
			}
			return _sIsBadgeCounterSupported;
		}

		/// <summary>
		/// </summary>
		/// <param name="context">context</param>
		/// <param name="notification"></param>
		/// <param name="badgeCount"></param>
		public static void ApplyNotification(Context context, Notification notification, int badgeCount)
		{
			if (Build.Manufacturer.Equals("Xiaomi", StringComparison.CurrentCultureIgnoreCase))
			{
				try
				{
					var field = notification.Class.GetDeclaredField("extraNotification");
					var extraNotification = field.Get(notification);
					var method = extraNotification.Class.GetDeclaredMethod("setMessageCount", Java.Lang.Integer.Type);
					method.Invoke(extraNotification, badgeCount);
				}
				catch (Exception e)
				{
					if (Log.IsLoggable(LogTag, LogPriority.Debug))
					{
						Log.Debug(LogTag, "Unable to execute badge", e);
					}
				}
			}
		}

		/// <summary>
		/// Initialize Badger if a launcher is availalble (eg. set as default on the device)
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		/// Returns true if a launcher is available, in this case, the Badger will be set and _sShortcutBadger will be non null.
		/// </returns>
		private static bool InitBadger(Context context)
		{
			Intent launchIntent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
			if (launchIntent == null)
			{
				Log.Error(LogTag, "Unable to find launch intent for package " + context.PackageName);
				return false;
			}

			_sComponentName = launchIntent.Component;

			var intent = new Intent(Intent.ActionMain);
			intent.AddCategory(Intent.CategoryHome);
			var resolveInfo = context.PackageManager.ResolveActivity(intent, PackageInfoFlags.MatchDefaultOnly);

			if (resolveInfo == null || resolveInfo.ActivityInfo.Name.ToLower().Contains("resolver"))
				return false;

			var currentHomePackage = resolveInfo.ActivityInfo.PackageName;

			foreach (var badger in Badgers)
			{
				IShortcutBadger shortcutBadger = null;
				try
				{
					shortcutBadger = (IShortcutBadger)Activator.CreateInstance(badger);
				}
				catch (Exception)
				{
					// ignored
				}

				if (shortcutBadger != null && shortcutBadger.SupportLaunchers.Contains(currentHomePackage))
				{
					_sShortcutBadger = shortcutBadger;
					break;
				}
			}

			if (_sShortcutBadger == null)
			{
				if (Build.Manufacturer.Equals("ZUK", StringComparison.CurrentCultureIgnoreCase))
					_sShortcutBadger = new ZukHomeBadger();
				else if (Build.Manufacturer.Equals("OPPO", StringComparison.CurrentCultureIgnoreCase))
					_sShortcutBadger = new OPPOHomeBader();
				else if (Build.Manufacturer.Equals("VIVO", StringComparison.CurrentCultureIgnoreCase))
					_sShortcutBadger = new VivoHomeBadger();
				else if (Build.Manufacturer.Equals("ZTE", StringComparison.CurrentCultureIgnoreCase))
					_sShortcutBadger = new ZTEHomeBadger();
				else
					_sShortcutBadger = new DefaultBadger();
			}

			return true;
		}
	}
}