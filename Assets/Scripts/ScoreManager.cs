using UnityEngine;
using UnityEngine.UI;

/**
 * ScoreManager takes care of the scoring system. If it finds a UI Text element named ScoreGUI,
 * it updates the ScoreGUI text, but it works fine without ScoreGUI.
 */
public class ScoreManager {

	static int blockScore = 50;
	static int[] chainScoreMap = {
		0, 0, 0, 300, 700, 1300, 2100, 3100, 4600, 6100, // chain 3 ~ 9
		7600, 9600, 11600, 13600, 15600, 18100, 20600, 23100, 25600, 28100 // chain 10 ~ 19
	};

	Text scoreText;
	int score = 0;

	public ScoreManager() {
		GameObject canvas = GameObject.Find ("Canvas");
		if (canvas != null) {
			Transform scoreGUI = canvas.transform.Find ("ScoreGUI");
			if (scoreGUI != null) {
				scoreText = scoreGUI.GetComponent<Text> ();
				SyncScoreGUI ();
			}
		}
	}

	public void AddScore (int point) {
		score = score + point;
		SyncScoreGUI ();
	}

	public int GetScore () {
		return score;
	}

	void SyncScoreGUI() {
		if (scoreText != null) {
			scoreText.text = score.ToString ();
		}
	}

	public static int CalculateScore(int chain, int combo, bool fever) {
		int chainScore = CalculateChainScore (chain);
		if (fever) {
			return (int) ((blockScore * chain + chainScore) * 3);
		} else {
			float comboBonus = CalculateComboBonus (combo);
			return (int) ((blockScore * chain + chainScore) * (1 + comboBonus));
		}
	}

	static int CalculateChainScore(int chain) {
		if (chain <= 19) {
			return chainScoreMap [chain];
		} else if (chain <= 29) {
			return chainScoreMap [19] + 3000 * (chain - 19);
		} else {
			return chainScoreMap [19] + 3000 * 10 + 3500 * (chain - 29);
		}
	}

	static float CalculateComboBonus(int combo) {
		if (combo == 1) {
			return 0;
		} else {
			combo = Mathf.Min (combo, 49);
			return (combo + 9) * 0.01f;
		}
	}
}
