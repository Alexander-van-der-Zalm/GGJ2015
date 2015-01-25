using UnityEngine;
using System.Collections;

public class WinningConditionManager : MonoBehaviour {

	bool gameFinished = false;
	bool player1Won = false;
	bool player2Won = false;

	public BlockManager blockManager;
	public UnitManager unitManager;

	// Use this for initialization
	void Start () {
		blockManager = GetComponent<BlockManager> ();
		unitManager = GetComponent<UnitManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameFinished) {
			gameIsOver();
		}
	}

	[RPC]
	public void checkWinningConditions() {

		// check if all blocks are taken
		int n = 0;

		for (int i = 0; i < blockManager.Blocks.Count; i++) {
			if (blockManager.Blocks[i].isOwned) {
				n++;
			}
		}
		if (n == blockManager.Blocks.Count) {
			gameFinished = true;
		}

		for (int i = 0; i < unitManager.Units.Count; i++) {
			if (unitManager.Units[i].team == 0 || unitManager.Units[i].team == 2) {
				gameFinished = true;
			}
		}
	}

	void gameIsOver() {
		Debug.Log ("Game Over");
	}
}
