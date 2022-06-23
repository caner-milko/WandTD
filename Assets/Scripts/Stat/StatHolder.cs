using System.Collections.Generic;
using UnityEngine;

namespace wtd.stat
{
	[System.Serializable]
	public class StatHolder
	{
		[SerializeField]
		private List<Stat> Stats = new();

		private readonly Dictionary<string, Stat> stats = new();

		public void Setup()
		{
			foreach (Stat stat in Stats)
			{
				AddStat(stat);
			}
		}

		public Stat AddStat(Stat stat, bool clone = false, bool addEffectorsIfExists = true)
		{
			if (stats.TryGetValue(stat.StatName, out Stat result))
			{
				if (addEffectorsIfExists)
				{
					result.AddEffector(stat.Effectors.ToArray());
				}
				return result;
			}
			Stat realStat = stat;
			if (clone)
			{
				realStat = stat.CloneToHolder(this);
			}
			stats.Add(realStat.StatName, realStat);
			//to display in inspector
			if (!Stats.Contains(realStat))
				Stats.Add(realStat);
			return realStat;
		}

		public Stat GetStat(string statName, bool createIfNull = false, float defaultVal = 0.0f)
		{
			if (stats.TryGetValue(statName, out Stat result))
			{
				return result;
			}
			if (!createIfNull)
				return null;
			Stat stat = new(statName, this, defaultVal);
			AddStat(stat);
			return stat;
		}

		public float GetStatValue(string statName, bool createIfNull = false, float defaultVal = 0.0f)
		{
			Stat stat = GetStat(statName, createIfNull, defaultVal);
			if (stat == null)
				return defaultVal;
			return stat.Value;
		}

		public void RemoveEffectorsFromSource(object source)
		{
			foreach (Stat stat in stats.Values)
			{
				stat.RemoveEffectorsFromSource(source);
			}
		}

	}
}
