using UnityEngine;
using wtd.spell;
namespace wtd.ui.spell
{
	public class UIHoldingSpellSlot : MonoBehaviour, ISpellCaster
	{
		private SpellContainer container;
		public SpellSlot HoldingSpell => container.AtSlot(0);

		[field: SerializeField]
		public UISpellSlot Slot { get; private set; }

		private void Awake()
		{
			container = new SpellContainer(this, 1);
			Slot.setup(HoldingSpell);
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
