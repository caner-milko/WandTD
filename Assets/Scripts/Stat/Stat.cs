using System;
using System.Collections.Generic;
using UnityEngine;
namespace wtd.stat
{
	[Serializable]
	public class Stat
	{
		public string StatName;

		public readonly StatHolder holder;
		public float Value => GetValue();
		[field: SerializeField]
		public float DefaultVal { get; private set; } = 0.0f;

		[field: SerializeField]
		public List<StatEffector> Effectors { get; private set; } = new List<StatEffector>();

		[SerializeField, ReadOnly]
		private float lastVal = 0.0f;

		private bool calculated = false;

		public Stat(string statName, float defaultVal, params StatEffector[] effectors) : this(statName, null, defaultVal)
		{
			AddEffector(effectors);
		}
		public Stat(string statName, StatHolder holder, float defaultVal)
		{
			StatName = statName;
			this.holder = holder;
			this.DefaultVal = defaultVal;
			calculated = false;
		}

		public float GetValue()
		{
			if (calculated)
				return lastVal;
			float calc = DefaultVal;
			float percAdd = 1.0f;
			bool lastPercAdd = false;
			foreach (StatEffector effector in Effectors)
			{
				if (lastPercAdd && effector.Type != StatEffector.StatEffectorType.PercentAdd)
				{
					calc *= percAdd;
					percAdd = 1.0f;
					lastPercAdd = false;
				}
				switch (effector.Type)
				{
					case StatEffector.StatEffectorType.Add:
						calc += effector.Value;
						break;
					case StatEffector.StatEffectorType.PercentAdd:
						percAdd += effector.Value;
						lastPercAdd = true;
						break;
					case StatEffector.StatEffectorType.PercentMul:
						calc *= 1f + effector.Value;
						break;
				}
			}
			calc *= percAdd;
			lastVal = calc;
			calculated = true;
			return calc;
		}

		public void SetDefault(float val)
		{
			this.DefaultVal = val;
		}

		public void AddEffector(params StatEffector[] toAdd)
		{
			Effectors.AddRange(toAdd);
			Effectors.Sort();
			calculated = false;
		}

		public void RemoveEffector(StatEffector effector)
		{
			Effectors.Remove(effector);
			calculated = false;
		}

		public void RemoveEffectorsFromSource(object source)
		{
			for (int i = Effectors.Count - 1; i >= 0; i--)
			{
				if (Effectors[i].source == source)
					Effectors.RemoveAt(i);
			}
			calculated = false;
		}

		public Stat CloneToHolder(StatHolder holder)
		{
			Stat newStat = new(StatName, holder, this.DefaultVal);
			newStat.AddEffector(this.Effectors.ToArray());
			return newStat;
		}
	}
}
