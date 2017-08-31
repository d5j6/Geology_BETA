using TMPro;
using System.Collections.Generic;
using HoloToolkit.Unity;

public class HoloConsole : Singleton<HoloConsole> {

    List<string> lines = new List<string>();
    int maxCount = 10;
    public TextMeshPro text;

    public void Log(string message = "")
    {
        lines.Add(message);
        if (lines.Count > maxCount)
        {
            lines.RemoveAt(lines.Count - 1);
        }
        text.text = "";
        for (int i = 0; i < lines.Count; i++)
        {
            text.text += lines[i] + "\r\n";
        }
    }
}
