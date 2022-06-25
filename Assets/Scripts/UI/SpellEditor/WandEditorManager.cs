using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using wtd.wand;
namespace wtd.ui.spell
{
	public class WandEditorManager : MonoBehaviour
	{
		public static WandEditorManager instance;

		[field: SerializeField]
		public GraphicRaycaster Raycaster { get; private set; }
		public PointerEventData PointerEventData { get; private set; }
		[field: SerializeField]
		public EventSystem EventSystem { get; private set; }

		[SerializeField]
		private RectTransform holdingsParent;
		[SerializeField]
		private UIHoldingSpellSlot holdingSlot;
		[SerializeField]
		private UIHoldingWandSlot holdingWand;

		private Dictionary<Wand, UIWand> Wands = new();

		[SerializeField]
		private UIWand UIWandPrefab;

		[ReadOnly]
		public UISpellSlot pickedSlot = null;
		[ReadOnly]
		public UIWandSlot pickedWand = null;
		[ReadOnly]
		public UISpellSlot hoveringSlot = null;
		[ReadOnly]
		public UIWandSlot hoveringWand = null;


		public bool IsWandValid => hoveringWand != null;
		public bool IsSlotValid => hoveringSlot != null;

		public bool DidPickWand => pickedWand != null;
		public bool DidPickSlot => pickedSlot != null;


		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			holdingSlot.gameObject.SetActive(false);
			holdingWand.gameObject.SetActive(false);
		}

		private void Update()
		{
			holdingsParent.position = Input.mousePosition;


			if (Input.GetMouseButtonDown(0))
			{
				Click(true);
			}
			else if (Input.GetMouseButtonDown(1))
			{
				Click(false);
			}
		}

		private void DetectHovers()
		{
			PointerEventData = new PointerEventData(EventSystem);
			PointerEventData.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult>();

			Raycaster.Raycast(PointerEventData, results);

			bool foundHoveringWand = false;
			bool foundHoveringSpell = false;



			foreach (RaycastResult result in results)
			{
				if (!foundHoveringWand)
				{
					if (IsWandValid)
					{
						if (result.gameObject != hoveringWand.gameObject)
						{

						}
					}
				}
			}

			if (!foundHoveringSpell)
			{
				hoveringSlot = null;
			}

			if (!foundHoveringWand)
			{
				hoveringWand = null;
			}
		}

		public void OnEnterWandSlot(UIWandSlot container)
		{
			this.hoveringWand = container;
		}

		public void OnExitWandSlot()
		{
			this.hoveringWand = null;
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

		public void PickWand()
		{
			if (hoveringWand.Slot.IsEmpty)
				return;
			holdingWand.gameObject.SetActive(true);
			pickedWand = hoveringWand;
			holdingWand.Container.SwapSlots(pickedWand.Slot, holdingWand.Slot);
		}

		public void ReleasePicked()
		{
			pickedWand = null;
			if (pickedSlot != null)
			{
				pickedSlot.Container.SwapSpells(pickedSlot.Slot, holdingSlot.HoldingSpell);
				pickedSlot = null;
			}
			holdingSlot.gameObject.SetActive(false);
		}

		public void SwapSpells()
		{
			hoveringSlot.Container.SwapSpells(hoveringSlot.Slot, holdingSlot.HoldingSpell);
			pickedSlot = null;
			if (!holdingSlot.HoldingSpell.IsEmpty)
			{
				holdingSlot.gameObject.SetActive(true);
				pickedSlot = hoveringSlot;
			}
			else
			{
				holdingSlot.gameObject.SetActive(false);
			}
		}

		public void SwapWands()
		{
			hoveringWand.Container.SwapSlots(hoveringWand.Slot, holdingWand.Slot);
			if (!holdingWand.Slot.IsEmpty)
			{
				holdingWand.gameObject.SetActive(true);
				pickedWand = hoveringWand;
			}
			else
			{
				pickedWand = null;
				holdingWand.gameObject.SetActive(false);
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
				if (DidPickWand)
				{
					if (IsWandValid)
					{
						SwapWands();
					}
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
					else if (IsWandValid)
					{
						PickWand();
					}
				}
			}
		}

		public UIWand PutUIWandToSlot(Wand wand, RectTransform newParent)
		{
			if (Wands.TryGetValue(wand, out UIWand uiwand))
			{
				uiwand.transform.SetParent(newParent, false);
				return uiwand;
			}
			UIWand newUIWand = GameObject.Instantiate<UIWand>(UIWandPrefab, newParent);
			newUIWand.Setup(wand);
			Wands.Add(wand, newUIWand);
			return newUIWand;
		}

		public void DeleteWand(Wand wand)
		{
			if (Wands.TryGetValue(wand, out UIWand uiwand))
			{
				GameObject.Destroy(uiwand.gameObject);
			}
		}

	}
}
