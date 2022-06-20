using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	public abstract class SpellTrigger : MonoBehaviour
	{
		public CastedSpell casted { get; private set; }

		public virtual void Setup(CastedSpell casted)
		{
			this.casted = casted;
		}
	}
}
