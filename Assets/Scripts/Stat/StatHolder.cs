using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.stat
{
	public class StatHolder : MonoBehaviour
	{
		[SerializeField]
		private List<Stat> Stats;

		private Dictionary<string, Stat> stats = new Dictionary<string, Stat>();

		private void Awake()
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
				realStat = stat.CloneTo(this);
			}
			stats.Add(realStat.StatName, realStat);
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

		public float? GetStatValue(string statName, bool createIfNull = false, float defaultVal = 0.0f)
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
