using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	[CreateAssetMenu(fileName = "Multicast Spell Data", menuName = "Spells/Multicast Passive Spell")]
	public class MulticastPassiveSpellData : PassiveSpellData
	{
		[SerializeField]
		private int multicastCount = 0;
		public override void addToGroup(SpellGroupBuilder group)
		{
			group.increaseRemCastCount(multicastCount);
		}
	}
}
