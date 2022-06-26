using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using wtd.wand;
namespace wtd.ui.spell
{
	[RequireComponent(typeof(RectTransform))]
	public class UIWandSlot : MonoBehaviour
	{

		public RectTransform Rect => (RectTransform)transform;

		[field: SerializeField]
		public UIWand HoldingWand { get; private set; }

		public WandSlot Slot { get; private set; }

		public WandContainer Container => Slot.Container;

		public Wand RealWand => Slot.Holding;

		public bool IsHovering { get; private set; } = false;


		public void Setup(WandSlot slot)
		{
			this.Slot = slot;
			if (!slot.IsEmpty)
			{
				ChangeHolding(WandEditorManager.instance.PutUIWandToSlot(slot.Holding, Rect));
			}
			Container.ContainerEdited.AddListener(RefreshHolding);
		}
		private void LateUpdate()
		{
			List<RaycastResult> lastResults = WandEditorManager.instance.LastRaycastResults;
			foreach (RaycastResult res in lastResults)
			{
				if (res.gameObject == this.gameObject)
				{
					IsHovering = true;
					WandEditorManager.instance.OnEnterWandSlot(this);
					return;
				}
			}
			WandEditorManager.instance.OnExitWandSlot(this);
			IsHovering = false;
		}

		public void ChangeHolding(UIWand newHolding)
		{
			HoldingWand = newHolding;
		}

		public void RefreshHolding()
		{
			if (HoldingWand == null && Slot.IsEmpty)
				return;

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
