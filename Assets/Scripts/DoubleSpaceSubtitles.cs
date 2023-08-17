using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DoubleSpaceSubtitles : MonoBehaviour
{
    void OnConversationLine(Subtitle subtitle)
    {
        subtitle.formattedText.text += "\n";
    }
}