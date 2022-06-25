using UnityEngine;
using wtd.wand;
namespace wtd.ui.spell
{
	public class UIHoldingWandSlot : MonoBehaviour
	{

		[field: SerializeField]
		public WandContainer Container { get; private set; }

		public WandSlot Slot => Container.AtSlot(0);

		[field: SerializeField]
		public UIWandSlot UISlot { get; private set; }

		private void Start()
		{
			UISlot.Setup(Container.AtSlot(0));
		}

	}
}
