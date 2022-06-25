using System;
namespace wtd.wand
{
	[Serializable]
	public class WandSlot
	{
		public Wand Holding { get; private set; }

		public WandContainer Container { get; private set; }

		public bool IsEmpty => Holding == null;

		public int Slot { get; private set; }

		public WandSlot(int slot, WandContainer holder)
		{
			this.Slot = slot;
			this.Container = holder;
		}

		public void ChangeHolding(Wand holding)
		{
			this.Holding = holding;
		}
	}
}
