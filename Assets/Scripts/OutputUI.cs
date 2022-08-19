using System.Linq;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class OutputUI : MonoBehaviour
{
    private static TMP_Text output;

    // Text that will not get cleared.
    private static string fixedText = "";

    // Text that can be cleared.
    private static string temporaryText = "";

    // Text that will be cleared after every frame.
    private static string dynamicText = "";

    // Text that can only be set once before getting cleared.
    private static string oneTimeText = "";

    void Awake()
    {
        var textUI = transform.GetChild(0);
        output = textUI.GetComponent<TMP_Text>();
    }

    void FixedUpdate()
    {
        // Update the UI to display the current non-null text values.
        output.text = string.Join(
            '\n',
            // New array every FixedUpdate lol, RIP garbage collector lmao.
            new string[] {
                temporaryText, fixedText, oneTimeText, dynamicText
            }
            .Where(line => !string.IsNullOrEmpty(line))
        );

        // Clear the 'dynamicText'.
        dynamicText = "";
    }

    public static void SetText(params string[] lines)
    {
        LineBreak(ref temporaryText);
        fixedText += string.Join('\n', lines);
    }

    public static void AppendText(params string[] lines)
    {
        LineBreak(ref temporaryText);
        temporaryText += string.Join('\n', lines);
    }

    public static void UpdateText(params string[] lines)
    {
        LineBreak(ref dynamicText);
        dynamicText += string.Join('\n', lines);
    }

    public static void SetOneTimeText(params string[] lines)
    {
        if (oneTimeText.Length == 0)
            oneTimeText = string.Join('\n', lines);
    }

    public static void ClearText()
    {
        temporaryText = "";
        oneTimeText = "";
    }

    private static void LineBreak(ref string text)
    {
        if (text.Length > 0 && !text.EndsWith('\n'))
            text += '\n';
    }
}
