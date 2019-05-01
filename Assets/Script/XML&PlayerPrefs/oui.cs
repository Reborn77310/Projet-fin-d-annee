using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oui : MonoBehaviour {
    private string _nameInput = "";
    private string _scoreInput = "0";
 
    private void OnGUI() {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
 
        // Display high scores!
        for (int i = 0; i < test.EntryCount; ++i) {
            var entry = test.GetEntry(i);
            GUILayout.Label("Name: " + entry.name + ", Score: " + entry.score);
        }
 
        // Interface for reporting test scores.
        GUILayout.Space(10);
 
        _nameInput = GUILayout.TextField(_nameInput);
        _scoreInput = GUILayout.TextField(_scoreInput);
 
        if (GUILayout.Button("Record")) {
            int score;
            int.TryParse(_scoreInput, out score);
 
            test.Record(_nameInput, score);
 
            // Reset for next input.
            _nameInput = "";
            _scoreInput = "0";
        }
 
        GUILayout.EndArea();
    }

    public void recordd()
    {
        test.Record(PlayerPrefs.GetString("PlayerName"),230);
    }
}