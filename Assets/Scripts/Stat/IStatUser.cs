using System;
using System.Reflection;
namespace wtd.stat
{
	public interface IStatUser
	{
		public StatHolder GetStatHolder();

		public void SetupStats()
		{
			BindingFlags bindingFlags = BindingFlags.Public |
							BindingFlags.NonPublic |
							BindingFlags.Instance;

			StatHolder holder = GetStatHolder();

			foreach (var curStatField in GetType().GetFields(bindingFlags))
			{
				if (curStatField.GetCustomAttributes(typeof(AutoCopyStat), true).Length == 0) continue;

				AutoCopyStat attr = curStatField.GetCustomAttribute<AutoCopyStat>(true);

				if (curStatField.FieldType == typeof(Stat))
				{
					Stat curStat = (Stat)curStatField.GetValue(this);
					if (attr.HasName)
						curStat.StatName = attr.StatName;
					curStatField.SetValue(this, holder.AddStat(curStat, true));
				}
				if (curStatField.FieldType == typeof(float) || curStatField.FieldType == typeof(int))
				{
					if (!attr.HasName)
						continue;
					Stat curStat = new(attr.StatName, (float)curStatField.GetValue(this));
					holder.AddStat(curStat, true, true);
				}
			}
		}
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class AutoCopyStat : Attribute
	{
		private readonly string statName = null;

		public AutoCopyStat(string statName = null)
		{
			this.statName = statName;
		}

		public string StatName => statName;

		public bool HasName => statName != null;
	}
}
