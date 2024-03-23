using CaptainCoder.Dungeoneering.Lua;
using CaptainCoder.Dungeoneering.Lua.Dialogue;
using CaptainCoder.Dungeoneering.Player.Unity;
using CaptainCoder.Unity;

using TMPro;

using UnityEngine;

namespace CaptainCoder.Dungeoneering.Unity;

public class DialogueController : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI Text { get; set; } = default!;

    [field: SerializeField]
    public Transform OptionsContainer { get; set; } = default!;
    [field: SerializeField]
    public DialogueOptionController OptionPrefab { get; set; } = default!;
    [field: SerializeField]
    public CrawlingModeController CrawlingController { get; set; } = default!;
    [field: SerializeField]
    public PlayerInputHandler PlayerInputHandler { get; set; } = default!;
    [field: SerializeField]
    public Color SelectedColor { get; set; } = Color.yellow;
    [field: SerializeField]
    public Color NormalColor { get; set; } = Color.black;
    private readonly List<DialogueOptionController> _allOptions = [];
    private int _selectedIx = 0;
    public void OnEnable()
    {
        PlayerInputHandler.OnDialogueAction?.AddListener(HandleUserInput);
        PlayerInputHandler.OnMovementAction?.RemoveListener(CrawlingController.HandleMovement);
    }
    public void OnDisable()
    {
        PlayerInputHandler.OnDialogueAction?.RemoveListener(HandleUserInput);
        PlayerInputHandler.OnMovementAction?.AddListener(CrawlingController.HandleMovement);
    }
    private void TestDialogue()
    {
        Dialogue dialogue = new(
            """
            This is a sample dialogue to test how it looks in the game view.

            I've added some extra spaces <b>bold</b> text and <i>italic</i> text.

            Additionally I've added <color=red>colors</color>!
            """
        );

        Dialogue more = new("I have nothing else to say.");

        dialogue.AddOption(new ContinueDialogueOption("Tell <b>Me</b> More", more));
        dialogue.AddOption(new RunScriptDialogueOption("Teleport <i>me</i>!", "teleport.lua"));
        dialogue.AddOption(new CloseDialogueOption("C<color=yellow>l</color>ose"));

        Show(dialogue);
    }

    public void Show(Dialogue dialogue)
    {
        Text.text = dialogue.Text;

        List<DialogueOption> options = [.. dialogue.Options];
        if (options.Count == 0)
        {
            options.Add(new CloseDialogueOption("Close"));
        }

        _allOptions.Clear();
        _selectedIx = 0;
        OptionsContainer.DestroyAllChildren(Destroy);
        foreach (DialogueOption option in options)
        {
            DialogueOptionController optionController = Instantiate(OptionPrefab, OptionsContainer);
            optionController.Text.text = option.Label;
            optionController.Option = option;
            _allOptions.Add(optionController);
        }

        Next(0); // Selects the first option
        gameObject.SetActive(true);
    }

    private void Next(int delta)
    {
        DialogueOptionController prev = _allOptions[_selectedIx];
        _selectedIx = (_selectedIx + delta) switch
        {
            int ix when ix >= _allOptions.Count => 0,
            int ix when ix < 0 => _allOptions.Count - 1,
            int ix => ix,
        };
        DialogueOptionController current = _allOptions[_selectedIx];
        prev.Text.color = NormalColor;
        current.Text.color = SelectedColor;
    }

    public void HandleUserInput(DialogueAction action)
    {
        Action result = action switch
        {
            DialogueAction.Next => () => Next(1),
            DialogueAction.Previous => () => Next(-1),
            DialogueAction.Select => ExecuteOption,
            DialogueAction.Exit => Close,
            _ => () => { }
        };
        result.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ExecuteOption()
    {
        DialogueOptionController controller = _allOptions[_selectedIx];
        Action action = controller.Option switch
        {
            CloseDialogueOption close => Close,
            ContinueDialogueOption option => () => Show(option.NextDialogue),
            RunScriptDialogueOption option => () => RunScriptAndClose(option.ScriptName),
            var unknown => throw new NotImplementedException($"Unknown option of type {unknown?.GetType()}: {unknown}."),
        };
        action.Invoke();
    }

    private void RunScriptAndClose(string scriptName)
    {
        Close();
        string script = CrawlingController.Manifest.Scripts[scriptName].Script;
        Interpreter.ExecLua(script, CrawlingController);
    }
}

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogueOptionController : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI Text { get; set; } = default!;
    public DialogueOption Option { get; set; } = default!;

    public void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }
}