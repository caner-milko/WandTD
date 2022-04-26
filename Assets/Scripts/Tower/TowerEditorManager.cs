using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.tower.editor
{
	public class TowerEditorManager : MonoBehaviour
	{
		public static TowerEditorManager instance { get; private set; }

		public Tower CurrentlyEditing { get; private set; }

		public bool IsEditing { get; private set; } = false;

		private void Awake()
		{
			instance = this;
		}

		// Start is called before the first frame update
		void Start()
		{

		}

		public void StartEditing(Tower tower)
		{
			CurrentlyEditing = tower;
			IsEditing = true;

		}
	}
}
