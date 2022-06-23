using UnityEngine;

namespace wtd.spell
{
	public abstract class MulticastPassiveSpell : PassiveSpell
	{
		[field: SerializeField]
		public int MulticastCount { get; private set; } = 0;
		public override void AddToGroup(SpellGroupBuilder group)
		{
			group.IncreaseRemCastCount(MulticastCount);
		}
	}
}