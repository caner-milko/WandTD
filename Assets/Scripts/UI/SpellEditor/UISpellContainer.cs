using System.Collections.Generic;
using UnityEngine;
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

		public SpellContainer Container { get; private set; }

		public List<UISpellSlot> Slots { get; private set; } = new List<UISpellSlot>();

		private void Start()
		{
			if (wand != null && wand.GetSpellContainer() != null)
				Setup(wand.GetSpellContainer());
		}

		public void Setup(SpellContainer container)
		{
			this.Container = container;
			//TODO instantiate slots
			for (int i = 0; i < container.Capacity; i++)
			{
				UISpellSlot newSlot = GameObject.Instantiate<UISpellSlot>(slotPrefab, spellSlotsParent);
				newSlot.gameObject.name = "Slot " + i;
				newSlot.setup(container.AtSlot(i));
				newSlot.Rect.position += new Vector3(200 * i, 0, 0);
				Slots.Add(newSlot);
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
			SpellEditorManager.instance.OnExitContainer();
		}
	}
}
