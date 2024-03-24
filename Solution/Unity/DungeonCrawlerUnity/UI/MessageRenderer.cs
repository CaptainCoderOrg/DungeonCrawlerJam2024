using CaptainCoder.Dungeoneering.Unity;
using CaptainCoder.Unity;

using TMPro;

using UnityEngine;

namespace CaptainCoder.Dungeoneering.Game.Unity;

public class MessageRenderer : MonoBehaviour
{
    [field: SerializeField]
    public Transform TextOutput { get; private set; } = default!;
    [field: SerializeField]
    public TextMeshProUGUI TextTemplate { get; private set; } = default!;

    public void Awake()
    {
        TextOutput.DestroyAllChildren(Destroy);
    }

    public void Start()
    {
        CrawlingModeController.CrawlerMode.OnMessageAdded += AddMessage;
        CrawlingModeController.OnCrawlerModeChange += crawler => crawler.OnMessageAdded += AddMessage;
    }

    public void AddMessage(Message message)
    {
        TextMeshProUGUI newText = Instantiate(TextTemplate, TextOutput);
        newText.text = message.Text;
    }

}