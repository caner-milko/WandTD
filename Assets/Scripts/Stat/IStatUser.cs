using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
namespace wtd.stat
{
	public interface IStatUser
	{
		public StatHolder GetStatHolder();
	}

	public class StatUtils
	{
		public static void SetupStats(IStatUser statUser)
		{
			BindingFlags bindingFlags = BindingFlags.Public |
							BindingFlags.NonPublic |
							BindingFlags.Instance;

			StatHolder holder = statUser.GetStatHolder();

			foreach (var curStatField in statUser.GetType().GetFields(bindingFlags))
			{
				if (curStatField.GetCustomAttributes(typeof(AutoCopyStat), true).Length == 0) continue;

				AutoCopyStat attr = curStatField.GetCustomAttribute<AutoCopyStat>(true);

				if (curStatField.FieldType == typeof(Stat))
				{
					Stat curStat = (Stat)curStatField.GetValue(statUser);
					if (attr.HasName)
						curStat.StatName = attr.StatName;
					curStatField.SetValue(statUser, holder.AddStat(curStat, true));
				}
				if (curStatField.FieldType == typeof(float) || curStatField.FieldType == typeof(int))
				{
					if (!attr.HasName)
						continue;
					Stat curStat = new Stat(attr.StatName, (float)curStatField.GetValue(statUser));
					holder.AddStat(curStat, true, true);
				}
			}

		}
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class AutoCopyStat : Attribute
	{
		private string statName = null;

		public AutoCopyStat(string statName = null)
		{
			this.statName = statName;
		}

		public string StatName => statName;

		public bool HasName => statName != null;
	}
}
