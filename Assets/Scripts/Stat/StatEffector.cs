using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace wtd.stat
{
	[Serializable]
	public class StatEffector : IComparable<StatEffector>
	{
		[Serializable]
		public enum StatEffectorType
		{
			Add = 3, PercentAdd = 2, PercentMul = 1
		}
		[field: NonSerialized]
		public Stat effecting { get; private set; }
		public readonly object source;
		[field: ReadOnly, SerializeField]
		public float value { get; private set; }
		[field: ReadOnly, SerializeField]
		public StatEffectorType type { get; private set; }
		[field: ReadOnly, SerializeField]
		public int order { get; private set; }
		public StatHolder holder => effecting.holder;

		public StatEffector(float value, object Source, StatEffectorType type) : this(value, Source, type, (int)type)
		{
		}

		public StatEffector(float value, object Source, StatEffectorType type, int order)
		{
			this.value = value;
			this.source = Source;
			this.type = type;
			this.order = order;
		}

		public void SetEffecting(Stat effecting)
		{
			if (this.effecting != null)
			{
				throw new UnityException("Cannot set effecting of an effector again.");
			}
			this.effecting = effecting;
		}

		public int CompareTo(StatEffector other)
		{
			if (other == null)
				return 1;
			return this.order - other.order;
		}
	}
}
