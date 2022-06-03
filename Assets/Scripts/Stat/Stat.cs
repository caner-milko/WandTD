using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.stat
{
	public class Stat : MonoBehaviour
	{
		public string StatName;
		public StatHolder holder;
		[field: SerializeField]
		public List<StatEffector> effectors { get; private set; }
		public float Value => GetValue();
		[SerializeField]
		private float defaultVal = 0.0f;

		[SerializeField, ReadOnly]
		private float lastVal = 0.0f;

		private bool updated = true;

		public float GetValue()
		{
			if (!updated)
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
			updated = false;
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
			updated = true;
		}

		public void RemoveEffector(StatEffector effector)
		{
			effectors.Remove(effector);
			updated = true;
		}

		public void RemoveEffectorsFromSource(object source)
		{
			for (int i = effectors.Count - 1; i >= 0; i--)
			{
				if (effectors[i].source == source)
					effectors.RemoveAt(i);
			}
			updated = true;
		}
	}
}
