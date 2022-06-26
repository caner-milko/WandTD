using System.Collections.Generic;
using UnityEngine;
using wtd.wand;
namespace wtd.ui.spell
{
	public class UIWandContainer : MonoBehaviour
	{
		public List<UIWandSlot> WandSlots { get; private set; } = new();

		[SerializeField]
		private UIWandSlot wandSlotPrefab;

		[SerializeField]
		private RectTransform wandSlotsParent;

		[field: SerializeField]
		public WandContainer Container { get; private set; }

		private void Start()
		{
			if (Container != null)
			{
				Setup(Container);
			}
		}

		public void Setup(WandContainer container)
		{
			this.Container = container;
			for (int i = 0; i < container.WandSlots.Count; i++)
			{
				UIWandSlot newSlot = GameObject.Instantiate<UIWandSlot>(wandSlotPrefab, wandSlotsParent);
				newSlot.gameObject.name = "Wand Slot " + i;
				newSlot.Setup(container.AtSlot(i));
				newSlot.Rect.position += new Vector3(0, newSlot.Rect.rect.height * (float)i * -1.05f, 0);
				WandSlots.Add(newSlot);
			}
		}

	}
}
