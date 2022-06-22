using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.spell;
namespace wtd.ui.spell
{
	public class UIHoldingSpellSlot : MonoBehaviour, ISpellCaster
	{
		private SpellContainer container;
		public SpellSlot holdingSpell => container.AtSlot(0);

		[field: SerializeField]
		public UISpellSlot slot { get; private set; }

		private void Awake()
		{
			container = new SpellContainer(this, 1);
			slot.setup(holdingSpell);
		}

		public string CasterType()
		{
			throw new System.NotImplementedException();
		}

		public Vector3 GetPosition()
		{
			throw new System.NotImplementedException();
		}

		public SpellContainer GetSpellContainer()
		{
			return container;
		}

		public CasterSpell NextSpell()
		{
			throw new System.NotImplementedException();
		}
	}
}
