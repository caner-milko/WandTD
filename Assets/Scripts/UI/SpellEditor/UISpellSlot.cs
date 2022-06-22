using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.spell;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace wtd.ui.spell
{
	[RequireComponent(typeof(RectTransform))]
	public class UISpellSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		public RectTransform rect => (RectTransform)transform;

		[SerializeField]
		private Image spellRenderer;
		[field: SerializeField, ReadOnly]
		public SpellSlot spellSlot { get; private set; }
		public SpellContainer container => spellSlot.belongsTo;
		public bool DisplayImage
		{
			get
			{
				return spellRenderer.enabled;
			}
			set
			{
				spellRenderer.enabled = value;
			}
		}

		public void setup(SpellSlot spellSlot)
		{
			this.spellSlot = spellSlot;
			container.ContainerEdited.AddListener(RefreshImage);
			RefreshImage();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			SpellEditorManager.instance.Click(eventData.button == PointerEventData.InputButton.Left);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			SpellEditorManager.instance.OnEnterSpellSlot(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			SpellEditorManager.instance.OnExitSpellSlot(this);
		}

		void RefreshImage()
		{
			if (spellSlot.IsEmpty)
			{
				spellRenderer.sprite = null;
				spellRenderer.color = Color.clear;
				return;
			}
			Spell.SpellImage spellImage = spellSlot.Spell.spell.image;
			spellRenderer.sprite = spellImage.sprite;
			spellRenderer.material = spellImage.material;
			spellRenderer.color = spellImage.color;
		}

	}
}
