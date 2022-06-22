using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using wtd.spell;
namespace wtd.ui.spell
{
	[RequireComponent(typeof(RectTransform))]
	public class UISpellContainer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{


		[SerializeField]
		private RectTransform spellSlotsParent;

		[SerializeField]
		private UISpellSlot slotPrefab;

		[SerializeField]
		private wtd.wand.Wand wand;

		public SpellContainer container { get; private set; }

		public List<UISpellSlot> slots { get; private set; } = new List<UISpellSlot>();

		private void Start()
		{
			if (wand != null && wand.GetSpellContainer() != null)
				setup(wand.GetSpellContainer());
		}

		public void setup(SpellContainer container)
		{
			this.container = container;
			//TODO instantiate slots
			for (int i = 0; i < container.Capacity; i++)
			{
				UISpellSlot newSlot = GameObject.Instantiate<UISpellSlot>(slotPrefab, spellSlotsParent);
				newSlot.gameObject.name = "Slot " + i;
				newSlot.setup(container.AtSlot(i));
				newSlot.rect.position += new Vector3(200 * i, 0, 0);
				slots.Add(newSlot);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			SpellEditorManager.instance.Click(eventData.button == PointerEventData.InputButton.Left);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			SpellEditorManager.instance.OnEnterContainer(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			SpellEditorManager.instance.OnExitContainer(this);
		}
	}
}
