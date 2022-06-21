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
		public readonly object source;
		[field: ReadOnly, SerializeField]
		public float value { get; private set; }
		[field: ReadOnly, SerializeField]
		public StatEffectorType type { get; private set; }
		[field: ReadOnly, SerializeField]
		public int order { get; private set; }

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

		public int CompareTo(StatEffector other)
		{
			if (other == null)
				return 1;
			return this.order - other.order;
		}
	}
}
