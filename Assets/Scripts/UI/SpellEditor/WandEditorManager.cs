using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

		private bool isEditing = false;

		public bool IsEditing
		{
			get { return isEditing; }
			set { isEditing = value; }
		}

		[field: SerializeField]
		public UnityEvent StartEditing { get; private set; } = new UnityEvent();
		[field: SerializeField]
		public UnityEvent StopEditing { get; private set; } = new UnityEvent();

		public bool IsWandValid => hoveringWand != null;
		public bool IsSlotValid => hoveringSlot != null;

		public bool DidPickWand => pickedWand != null;
		public bool DidPickSlot => pickedSlot != null;


		public List<RaycastResult> LastRaycastResults { get; private set; } = new();

		private void Awake()
		{
			instance = this;
			StartEditing.AddListener(StartEdit);
			StopEditing.AddListener(StopEdit);
			holdingSlot.gameObject.SetActive(false);
			holdingWand.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (IsEditing)
				{
					StopEditing.Invoke();
				}
				else
					StartEditing.Invoke();
			}
			if (!IsEditing)
				return;
			holdingsParent.position = Input.mousePosition;

			DetectHovers();

			if (Input.GetMouseButtonDown(0))
			{
				Click(true);
			}
			else if (Input.GetMouseButtonDown(1))
			{
				Click(false);
			}
		}

		private void StartEdit()
		{
			IsEditing = true;
		}
		private void StopEdit()
		{
			IsEditing = false;
			ReleasePicked();
		}

		private void DetectHovers()
		{
			PointerEventData = new PointerEventData(EventSystem);
			PointerEventData.position = Input.mousePosition;

			LastRaycastResults = new List<RaycastResult>();

			Raycaster.Raycast(PointerEventData, LastRaycastResults);
		}

		public bool OnEnterWandSlot(UIWandSlot container)
		{
			if (hoveringWand == container)
				return false;
			hoveringWand = container;
			return true;
		}

		public bool OnExitWandSlot(UIWandSlot container)
		{
			if (hoveringWand != container)
				return false;
			this.hoveringWand = null;
			return true;
		}

		public bool OnEnterSpellSlot(UISpellSlot slot)
		{
			if (hoveringWand == slot)
				return false;
			this.hoveringSlot = slot;
			return true;
		}

		public bool OnExitSpellSlot(UISpellSlot slot)
		{
			if (hoveringSlot != slot)
				return false;
			this.hoveringSlot = null;
			return true;
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
			if (pickedWand != null)
			{
				pickedWand.Container.SwapSlots(pickedWand.Slot, holdingWand.Slot);
				pickedWand = null;
			}
			if (pickedSlot != null)
			{
				pickedSlot.Container.SwapSpells(pickedSlot.Slot, holdingSlot.HoldingSpell);
				pickedSlot = null;
			}
			holdingSlot.gameObject.SetActive(false);
			holdingWand.gameObject.SetActive(false);
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

		public UIWand PutUIWandToSlot(Wand wand, RectTransform newSlot)
		{
			if (Wands.TryGetValue(wand, out UIWand uiwand))
			{
				uiwand.transform.SetParent(newSlot.transform, false);
				return uiwand;
			}
			UIWand newUIWand = GameObject.Instantiate<UIWand>(UIWandPrefab, newSlot.transform);
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
