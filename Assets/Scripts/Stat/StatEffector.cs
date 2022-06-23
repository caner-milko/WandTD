using System;
using UnityEngine;
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
		public float Value { get; private set; }
		[field: ReadOnly, SerializeField]
		public StatEffectorType Type { get; private set; }
		[field: ReadOnly, SerializeField]
		public int Order { get; private set; }

		public StatEffector(float value, object Source, StatEffectorType type) : this(value, Source, type, (int)type)
		{
		}

		public StatEffector(float value, object Source, StatEffectorType type, int order)
		{
			this.Value = value;
			this.source = Source;
			this.Type = type;
			this.Order = order;
		}

		public int CompareTo(StatEffector other)
		{
			if (other == null)
				return 1;
			return this.Order - other.Order;
		}
	}
}
