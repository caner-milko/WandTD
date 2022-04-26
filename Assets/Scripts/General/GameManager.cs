using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd
{

	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;


		[field: SerializeField]
		public PlayerTest Player { get; private set; }

		public enum GameState
		{
			PLAYING, MENU
		}

		public GameState State = GameState.PLAYING;

		private void Start()
		{
			instance = this;
		}

		private void OnValidate()
		{
			instance = this;
		}

	}
}
