using UnityEngine;

namespace wtd.ui.spell
{
	public class SpellEditorManager : MonoBehaviour
	{
		public static SpellEditorManager instance;

		[SerializeField]
		private UIHoldingSpellSlot holdingSlot;

		[ReadOnly]
		public UISpellSlot pickedSlot = null;
		[ReadOnly]
		public UISpellContainer pickedContainer = null;
		[ReadOnly]
		public UISpellContainer hoveringContainer = null;
		[ReadOnly]
		public UISpellSlot hoveringSlot = null;


		public bool IsContainerValid => hoveringContainer != null;
		public bool IsSlotValid => hoveringSlot != null;

		public bool DidPickContainer => pickedContainer != null;
		public bool DidPickSlot => pickedSlot != null;


		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			holdingSlot.gameObject.SetActive(false);
		}

		private void Update()
		{
			holdingSlot.transform.position = Input.mousePosition;
			//	EventSystem.current.RaycastAll()
		}



		public void OnEnterContainer(UISpellContainer container)
		{
			if (container == pickedContainer)
				return;
			this.hoveringContainer = container;
		}

		public void OnExitContainer()
		{
			this.hoveringContainer = null;
		}

		public void OnEnterSpellSlot(UISpellSlot slot)
		{
			this.hoveringSlot = slot;
		}

		public void OnExitSpellSlot()
		{
			this.hoveringSlot = null;
		}

		public void PickSpellSlot()
		{
			if (hoveringSlot.Slot.IsEmpty)
				return;
			holdingSlot.gameObject.SetActive(true);
			pickedSlot = hoveringSlot;
			pickedSlot.Container.SwapSpells(pickedSlot.Slot, holdingSlot.HoldingSpell);
		}

		public void PickSpellContainer()
		{
			pickedContainer = hoveringContainer;
		}

		public void ReleasePicked()
		{
			pickedContainer = null;
			pickedSlot = null;
			holdingSlot.gameObject.SetActive(false);
		}

		public void SwapSpells()
		{
			hoveringSlot.Container.SwapSpells(hoveringSlot.Slot, holdingSlot.HoldingSpell);
			ReleasePicked();
			if (!holdingSlot.HoldingSpell.IsEmpty)
			{
				holdingSlot.gameObject.SetActive(true);
				pickedSlot = hoveringSlot;
			}
		}

		public void Click(bool leftClick)
		{
			if (!leftClick)
			{
				ReleasePicked();
			}
			else
			{
				if (DidPickContainer)
				{
					//swap to spell container slot
				}
				else if (DidPickSlot)
				{
					if (IsSlotValid)
					{
						SwapSpells();
					}
				}
				//pick if can
				else
				{
					if (IsSlotValid)
					{
						PickSpellSlot();
					}
					else if (IsContainerValid)
					{
						PickSpellContainer();
					}
				}
			}
		}

	}
}
