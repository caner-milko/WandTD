using UnityEngine;
using UnityEngine.EventSystems;
using wtd.wand;
namespace wtd.ui.spell
{
	public class UIWandSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{

		public RectTransform Rect => (RectTransform)transform;

		[field: SerializeField]
		public UIWand HoldingWand { get; private set; }

		public WandSlot Slot { get; private set; }

		public WandContainer Container => Slot.Container;

		public Wand RealWand => Slot.Holding;

		public void Setup(WandSlot slot)
		{
			this.Slot = slot;
			if (!slot.IsEmpty)
			{
				ChangeHolding(WandEditorManager.instance.PutUIWandToSlot(slot.Holding, Rect));
			}
			Container.ContainerEdited.AddListener(RefreshHolding);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			WandEditorManager.instance.OnEnterWandSlot(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == null || eventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(transform))
				return;
			WandEditorManager.instance.OnExitWandSlot();
		}

		public void ChangeHolding(UIWand newHolding)
		{
			HoldingWand = newHolding;
		}

		public void RefreshHolding()
		{
			if ((!Slot.IsEmpty && HoldingWand == null) || RealWand != HoldingWand.RealWand)
			{
				if (Slot.IsEmpty)
					HoldingWand = null;
				else
					HoldingWand = WandEditorManager.instance.PutUIWandToSlot(RealWand, Rect);
			}
		}

	}
}
