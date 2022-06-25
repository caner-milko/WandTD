using System.Collections.Generic;
using UnityEngine;
using wtd.spell;
namespace wtd.ui.spell
{
	[RequireComponent(typeof(RectTransform))]
	public class UISpellContainer : MonoBehaviour
	{


		[SerializeField]
		private RectTransform spellSlotsParent;

		[SerializeField]
		private UISpellSlot slotPrefab;

		public SpellContainer Container { get; private set; }

		public List<UISpellSlot> Slots { get; private set; } = new List<UISpellSlot>();

		public void Setup(SpellContainer container)
		{
			this.Container = container;
			//TODO instantiate slots
			for (int i = 0; i < container.Capacity; i++)
			{
				UISpellSlot newSlot = GameObject.Instantiate<UISpellSlot>(slotPrefab, spellSlotsParent);
				newSlot.gameObject.name = "Spell Slot " + i;
				newSlot.Setup(container.AtSlot(i));
				newSlot.Rect.position += new Vector3(newSlot.Rect.rect.width * (float)i * 1.1f, 0, 0);
				Slots.Add(newSlot);
			}
		}
	}
}
