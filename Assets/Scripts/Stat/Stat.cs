using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace wtd.stat
{
	[Serializable]
	public class Stat
	{
		public string StatName;

		public readonly StatHolder holder;
		public float Value => GetValue();
		[field: SerializeField]
		public float defaultVal { get; private set; } = 0.0f;

		[field: SerializeField]
		public List<StatEffector> effectors { get; private set; } = new List<StatEffector>();

		[SerializeField, ReadOnly]
		private float lastVal = 0.0f;

		private bool calculated = false;

		public Stat(string statName, StatHolder holder, float defaultVal)
		{
			StatName = statName;
			this.holder = holder;
			this.defaultVal = defaultVal;
			calculated = false;
		}

		public float GetValue()
		{
			if (calculated)
				return lastVal;
			float calc = defaultVal;
			float percAdd = 1.0f;
			bool lastPercAdd = false;
			foreach (StatEffector effector in effectors)
			{
				if (lastPercAdd && effector.type != StatEffector.StatEffectorType.PercentAdd)
				{
					calc *= percAdd;
					percAdd = 1.0f;
					lastPercAdd = false;
				}
				switch (effector.type)
				{
					case StatEffector.StatEffectorType.Add:
						calc += effector.value;
						break;
					case StatEffector.StatEffectorType.PercentAdd:
						percAdd += effector.value;
						lastPercAdd = true;
						break;
					case StatEffector.StatEffectorType.PercentMul:
						calc *= 1f + effector.value;
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
			this.defaultVal = val;
		}

		public void AddEffector(params StatEffector[] toAdd)
		{
			foreach (StatEffector effector in toAdd)
			{
				effector.SetEffecting(this);
			}
			effectors.AddRange(toAdd);
			effectors.Sort();
			calculated = false;
		}

		public void RemoveEffector(StatEffector effector)
		{
			effectors.Remove(effector);
			calculated = false;
		}

		public void RemoveEffectorsFromSource(object source)
		{
			for (int i = effectors.Count - 1; i >= 0; i--)
			{
				if (effectors[i].source == source)
					effectors.RemoveAt(i);
			}
			calculated = false;
		}

		public Stat CloneToHolder(StatHolder holder)
		{
			Stat newStat = new Stat(StatName, this.holder, this.defaultVal);
			newStat.AddEffector(this.effectors.ToArray());
			return newStat;
		}
	}
}
