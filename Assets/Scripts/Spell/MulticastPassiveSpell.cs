using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	public abstract class MulticastPassiveSpell : PassiveSpell
	{
		[field: SerializeField]
		public int multicastCount { get; private set; } = 0;
		public override void addToGroup(SpellGroupBuilder group)
		{
			group.increaseRemCastCount(multicastCount);
		}
	}
}