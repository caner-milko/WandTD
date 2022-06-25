using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using wtd.spell;
namespace wtd.wand
{
	public class WandContainer : MonoBehaviour, ISpellTarget
	{
		[SerializeField]
		private List<Wand> startingWands = new List<Wand>();

		[SerializeField, Range(1, 8)]
		private int capacity = 1;

		public List<WandSlot> WandSlots { get; private set; } = new List<WandSlot>();
		public Wand First => AtSlot(0).Holding;

		public UnityEvent ContainerEdited;

		private void Awake()
		{
			CreateSlots();
			ContainerEdited = new UnityEvent();
		}

		private void CreateSlots()
		{
			for (int i = 0; i < capacity; i++)
			{
				WandSlot slot = new WandSlot(i, this);
				WandSlots.Add(slot);
				if (startingWands.Count > i)
				{
					slot.ChangeHolding(startingWands[i]);
					startingWands[i].ChangeOwner(this);
				}
			}

		}

		public void ChangeSlot(int slot, Wand newWand)
		{
			WandSlots[slot].ChangeHolding(newWand);
			if (newWand != null)
				newWand.ChangeOwner(this);
			ContainerEdited.Invoke();
		}

		public void SwapSlots(WandSlot toSwap, WandSlot swapWith)
		{
			Wand before = toSwap.Holding;
			toSwap.Container.ChangeSlot(toSwap.Slot, swapWith.Holding);
			swapWith.Container.ChangeSlot(swapWith.Slot, before);
		}

		public WandSlot AtSlot(int slot)
		{
			return WandSlots[slot];
		}



		public Vector3 GetPosition()
		{
			throw new System.NotImplementedException();
		}

		public string GetTargetType()
		{
			throw new System.NotImplementedException();
		}
	}
}
