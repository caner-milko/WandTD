using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.stat
{
	public class StatHolder : MonoBehaviour
	{
		private Dictionary<string, Stat> stats = new Dictionary<string, Stat>();

		[SerializeField]
		private Transform StatsParent;

		private void Awake()
		{
			if (StatsParent == null || !StatsParent)
			{
				bool found = false;
				foreach (Transform tf in transform)
					if (tf.gameObject.name == "Stats")
					{
						StatsParent = tf;
						found = true;
					}
				if (!found)
				{
					StatsParent = new GameObject("Stats").transform;
					StatsParent.parent = transform;
				}
			}
			foreach (Stat stat in StatsParent.GetComponentsInChildren<Stat>())
			{
				AddStat(stat, false);
			}
		}

		public Stat AddStat(Stat stat, bool instantiate)
		{
			if (stats.TryGetValue(stat.name, out Stat result))
			{
				return result;
			}
			Stat realStat = stat;
			if (instantiate)
			{
				realStat = GameObject.Instantiate<Stat>(stat, transform);
			}
			realStat.holder = this;
			stats.Add(realStat.StatName, realStat);
			return realStat;
		}

		public Stat GetStat(string statName, bool createIfNull)
		{
			if (stats.TryGetValue(statName, out Stat result))
			{
				return result;
			}
			if (!createIfNull)
				return null;
			Stat stat = gameObject.AddComponent<Stat>();
			AddStat(stat, false);
			return stat;
		}

		public float? GetStatValue(string statName, bool createIfNull)
		{
			if (stats.TryGetValue(statName, out Stat result))
				return result.Value;
			if (!createIfNull)
				return null;
			Stat stat = gameObject.AddComponent<Stat>();
			AddStat(stat, false);
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
