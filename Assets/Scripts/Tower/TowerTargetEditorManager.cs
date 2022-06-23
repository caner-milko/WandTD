using System.Collections.Generic;
using UnityEngine;

namespace wtd.tower.editor
{
	public class TowerTargetEditorManager : MonoBehaviour
	{
		public static TowerTargetEditorManager instance;


		[field: SerializeField]
		public CanvasRenderer TowerTargetEditorIndicator { get; private set; }
		[field: SerializeField]
		public List<TowerTarget> TargetPrefabs { get; private set; } = new List<TowerTarget>();

		public TowerTargetEditor TTargetEditor { get; private set; }

		public bool EditingTarget { get; private set; }

		private void Awake()
		{
			instance = this;
		}

		public void ChangeTowerTarget(Tower tower, TowerTarget newTarget)
		{
			if (newTarget is not EditableTowerTarget)
			{
				if (tower.TargetAdapter != newTarget)
					tower.TargetAdapter.ChangeTarget(newTarget);
				return;
			}
			EditableTowerTarget editableTarget = (EditableTowerTarget)newTarget;
			TTargetEditor = editableTarget.GetTTargetEditor();
			ActivateTargetEditor();
		}

		public void EditTowerTarget(Tower tower)
		{
			ChangeTowerTarget(tower, tower.TargetAdapter.Target);
		}

		private void ActivateTargetEditor()
		{
			TowerTargetEditorIndicator.gameObject.SetActive(true);
			EditingTarget = true;
			UpdateTTargetEditor();
		}

		private void EndTargetEditor()
		{
			TowerTargetEditorIndicator.gameObject.SetActive(false);
			TTargetEditor = null;
			EditingTarget = false;
		}

		private void CancelTargetEditor()
		{
			TTargetEditor.CancelEditor(CameraManager.Instance.ScreenMousePos);
			EndTargetEditor();
		}

		private void AcceptTargetEditor()
		{
			EndTargetEditor();
		}

		private void UpdateTTargetEditor()
		{
			if (TTargetEditor == null)
			{
				return;
			}
			((RectTransform)TowerTargetEditorIndicator.transform).anchoredPosition = CameraManager.Instance.ScreenMousePos;
			//change to raycast
			bool check = false;
			if (Physics.Raycast(CameraManager.Instance.MouseRay, out RaycastHit hit))
			{
				check = TTargetEditor.CheckTarget(hit.point);
			}

			if (check)
			{
				TowerTargetEditorIndicator.GetMaterial(0).color = Color.green;
			}
			else
			{
				TowerTargetEditorIndicator.GetMaterial(0).color = Color.red;
			}
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				if (check)
				{
					AcceptTargetEditor();
				}
				else
				{
					//TODO play sound/flash indicator
				}
			}
			else
			{
				CancelTargetEditor();
			}
		}

		private void Update()
		{
			UpdateTTargetEditor();
		}
	}
}
