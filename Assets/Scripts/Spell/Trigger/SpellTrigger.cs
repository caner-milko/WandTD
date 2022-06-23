using UnityEngine;

namespace wtd.spell
{
	public abstract class SpellTrigger : MonoBehaviour
	{
		public CastedSpell Casted { get; private set; }

		public virtual void Setup(CastedSpell casted)
		{
			this.Casted = casted;
		}
	}
}
