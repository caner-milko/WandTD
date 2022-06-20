using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.stat
{
	[System.Serializable]
	public class StatHolder
	{
		[SerializeField]
		private List<Stat> Stats = new List<Stat>();

		private Dictionary<string, Stat> stats = new Dictionary<string, Stat>();

		public void setup()
		{
			foreach (Stat stat in Stats)
			{
				AddStat(stat);
			}
		}

		public Stat AddStat(Stat stat, bool clone = false)
		{
			if (stats.TryGetValue(stat.StatName, out Stat result))
			{
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
			Stat stat = new Stat(statName, this, defaultVal);
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
