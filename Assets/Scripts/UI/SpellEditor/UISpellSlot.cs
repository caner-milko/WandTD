using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using wtd.spell;
namespace wtd.ui.spell
{
	[RequireComponent(typeof(RectTransform))]
	public class UISpellSlot : MonoBehaviour
	{
		public RectTransform Rect => (RectTransform)transform;

		[SerializeField]
		private Image spellRenderer;
		[field: SerializeField, ReadOnly]
		public SpellSlot Slot { get; private set; }
		public SpellContainer Container => Slot.belongsTo;
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

		public bool IsHovering { get; private set; } = false;

		public void Setup(SpellSlot spellSlot)
		{
			this.Slot = spellSlot;
			Container.ContainerEdited.AddListener(RefreshImage);
			RefreshImage();
		}


		private void LateUpdate()
		{
			List<RaycastResult> lastResults = WandEditorManager.instance.LastRaycastResults;
			foreach (RaycastResult res in lastResults)
			{
				if (res.gameObject == this.gameObject)
				{
					WandEditorManager.instance.OnEnterSpellSlot(this);
					IsHovering = true;
					return;
				}
			}
			WandEditorManager.instance.OnExitSpellSlot(this);
			IsHovering = false;
		}

		/*public void OnPointerEnter(PointerEventData eventData)
		{
			WandEditorManager.instance.OnEnterSpellSlot(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == null || !eventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(transform))
				WandEditorManager.instance.OnExitSpellSlot();
		}
		*/
		void RefreshImage()
		{
			if (Slot.IsEmpty)
			{
				spellRenderer.sprite = null;
				spellRenderer.color = Color.clear;
				return;
			}
			Spell.SpellImage spellImage = Slot.Spell.Spell.Image;
			spellRenderer.sprite = spellImage.sprite;
			spellRenderer.material = spellImage.material;
			spellRenderer.color = spellImage.color;
		}

	}
}
