using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.tower.editor
{
	public class TowerEditorManager : MonoBehaviour
	{
		public static TowerEditorManager instance { get; private set; }

		public Tower CurrentlyEditing { get; private set; }

		[field: SerializeField]
		public bool IsEditing { get; private set; } = false;

		private Tower LastClosest;

		[field: SerializeField]
		public Material HighlightMaterial { get; private set; }

		private Dictionary<MeshRenderer, Material[]> oldMaterials = new Dictionary<MeshRenderer, Material[]>();

		private void Awake()
		{
			instance = this;
		}

		// Start is called before the first frame update
		void Start()
		{

		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Mouse1))
			{
				Tower tw = TowerManager.instance.GetTowerFromMouse();
			}
			bool editInput = Input.GetKeyUp(KeyCode.E);
			if (!IsEditing)
			{
				Tower closest = GetClosestEditableToPlayer();
				ShowIndicator(closest);
				if (closest != null && editInput)
				{
					StartEditing(closest);
				}
			}

			else
			{
				if (!IsEditableByPlayer(CurrentlyEditing) || editInput)
				{
					StopEditing();
				}
			}
		}

		public void StartEditing(Tower tower)
		{
			CurrentlyEditing = tower;
			IsEditing = true;
			ResetLastIndicator();
			LastClosest = null;
		}

		public void StopEditing()
		{
			IsEditing = false;
		}

		public Tower GetClosestEditableToPlayer()
		{
			PlayerTest player = GameManager.instance.Player;
			Tower closestToPlayer = TowerManager.instance.GetClosestTower(player.GetPosition());
			if (IsEditableByPlayer(closestToPlayer)) return closestToPlayer;
			return null;
		}

		public bool IsEditableByPlayer(Tower tower)
		{
			PlayerTest player = GameManager.instance.Player;
			return TowerManager.instance.IsCloseToTower(player.GetPosition(), player.EditableDistance, tower);
		}

		private void ShowIndicator(Tower newClosest)
		{
			if (newClosest == LastClosest)
				return;
			ResetLastIndicator();
			LastClosest = newClosest;
			oldMaterials = new Dictionary<MeshRenderer, Material[]>();
			if (newClosest != null)
			{
				MeshRenderer[] renderers = newClosest.GetComponentsInChildren<MeshRenderer>(true);
				foreach (MeshRenderer renderer in renderers)
				{
					oldMaterials.Add(renderer, renderer.materials);
					Material[] materials = renderer.materials;
					for (int i = 0; i < renderer.materials.Length; i++)
					{
						materials[i] = HighlightMaterial;
					}
					renderer.materials = materials;
				}
			}
		}

		private void ResetLastIndicator()
		{
			if (LastClosest != null)
			{
				foreach (KeyValuePair<MeshRenderer, Material[]> pair in oldMaterials)
				{
					pair.Key.materials = pair.Value;
				}
			}
		}


	}
}
