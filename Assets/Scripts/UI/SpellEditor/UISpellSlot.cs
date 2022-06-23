using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using wtd.spell;
namespace wtd.ui.spell
{
	[RequireComponent(typeof(RectTransform))]
	public class UISpellSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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

		public void setup(SpellSlot spellSlot)
		{
			this.Slot = spellSlot;
			Container.ContainerEdited.AddListener(RefreshImage);
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
			SpellEditorManager.instance.OnExitSpellSlot();
		}

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
