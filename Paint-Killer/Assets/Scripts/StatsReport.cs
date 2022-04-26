using UnityEngine;
using System.Collections;

public class StatsReport : MonoBehaviour
{
	float deltaTime = 0.0f;
	float msec = 0, fps = 0;
	string text, text2;
	Rect rect, rect2;
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}


	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		rect = new Rect(0, 0, w, h * 2 / 100);
		rect2 = new Rect(0, h*8/200, w, h * 2/200);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 50;
		style.normal.textColor = Color.magenta;//new Color(0.0f, 1.5f, 0.5f, 1.0f);
		msec = deltaTime * 1000.0f;
		fps = 1.0f / deltaTime;
		text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		text2 = string.Format("res {0:0} x {1:0}", w, h);
		GUI.Label(rect, text, style);
		GUI.Label(rect2, text2, style);
	}
}
