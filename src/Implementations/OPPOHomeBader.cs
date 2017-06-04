using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Java.IO;
using Java.Lang;
using Java.Lang.Reflect;
using Xamarin.ShortcutBadger.Infrastructure;
using Xamarin.ShortcutBadger.Utils;
using Exception = System.Exception;
using Uri = Android.Net.Uri;
using Object = Java.Lang.Object;

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
		private const string ProviderContentUri = "content://com.android.badge/badge";
		private const string IntentAction = "com.oppo.unsettledevent";
		private const string IntentExtraPackagename = "pakeageName";
		private const string IntentExtraBadgeCount = "number";
		private const string IntentExtraBadgeUpgradenumber = "upgradeNumber";
		private const string IntentExtraBadgeupgradeCount = "app_badge_count";
		private int _romversion = -1;

		public void ExecuteBadge(Context context, ComponentName componentName, int badgeCount)
		{
			if (badgeCount == 0)
			{
				badgeCount = -1;
			}
			var intent = new Intent(IntentAction);
			intent.PutExtra(IntentExtraPackagename, componentName.PackageName);
			intent.PutExtra(IntentExtraBadgeCount, badgeCount);
			intent.PutExtra(IntentExtraBadgeUpgradenumber, badgeCount);
			if (BroadcastHelper.CanResolveBroadcast(context, intent))
			{
				context.SendBroadcast(intent);
			}
			else
			{
				int version = SupportVersion;
				if (version == 6)
				{
					try
					{
						var extras = new Bundle();
						extras.PutInt(IntentExtraBadgeupgradeCount, badgeCount);
						context.ContentResolver.Call(Uri.Parse(ProviderContentUri), "setAppBadgeCount", null, extras);
					}
					catch (Throwable)
					{
						throw new ShortcutBadgeException("unable to resolve intent: " + intent);
					}
				}
			}
		}

		public IEnumerable<string> SupportLaunchers => new[] {"com.oppo.launcher"};


		private int SupportVersion
		{
			get
			{
				int i = _romversion;
				if (i >= 0)
				{
					return _romversion;
				}
				try
				{
					i = ((Integer) ExecuteClassLoad(GetClass("com.color.os.ColorBuild"), "getColorOSVERSION", null, null)).IntValue();
				}
				catch (Exception)
				{
					i = 0;
				}
				if (i == 0)
				{
					try
					{
						var str = GetSystemProperty("ro.build.version.opporom");
						if (str.StartsWith("V1.4"))
						{
							return 3;
						}
						if (str.StartsWith("V2.0"))
						{
							return 4;
						}
						if (str.StartsWith("V2.1"))
						{
							return 5;
						}
					}
					catch (Exception)
					{
						// ignored
					}
				}
				_romversion = i;
				return _romversion;
			}
		}

		private Object ExecuteClassLoad(Class cls, string str, Class[] clsArr, Object[] objArr)
		{
			Object obj = null;
			if (!(cls == null || CheckObjExists(str)))
			{
				var method = GetMethod(cls, str, clsArr);
				if (method != null)
				{
					method.Accessible = true;
					try
					{
						obj = method.Invoke(null, objArr);
					}
					catch (IllegalAccessException e)
					{
						e.PrintStackTrace();
					}
					catch (InvocationTargetException e)
					{
						e.PrintStackTrace();
					}
				}
			}
			return obj;
		}

		private Method GetMethod(Class cls, string str, Class[] clsArr)
		{
			if (cls == null || CheckObjExists(str))
			{
				return null;
			}
			try
			{
				cls.GetMethods();
				cls.GetDeclaredMethods();
				return cls.GetDeclaredMethod(str, clsArr);
			}
			catch (Exception)
			{
				try
				{
					return cls.GetMethod(str, clsArr);
				}
				catch (Exception)
				{
					return cls.Superclass != null
						? GetMethod(cls.Superclass, str, clsArr)
						: null;
				}
			}
		}

		private Class GetClass(string str)
		{
			Class cls = null;
			try
			{
				cls = Class.ForName(str);
			}
			catch (ClassNotFoundException)
			{
			}
			return cls;
		}

		private bool CheckObjExists(Object obj)
		{
			return obj == null || obj.ToString().Equals("") || obj.ToString().Trim().Equals("null");
		}

		private string GetSystemProperty(string propName)
		{
			string line;
			BufferedReader input = null;
			try
			{
				var p = Runtime.GetRuntime().Exec("getprop " + propName);
				input = new BufferedReader(new InputStreamReader(p.InputStream), 1024);
				line = input.ReadLine();
				input.Close();
			}
			catch (IOException)
			{
				return null;
			}
			finally
			{
				CloseHelper.CloseQuietly(input);
			}
			return line;
		}
	}
}