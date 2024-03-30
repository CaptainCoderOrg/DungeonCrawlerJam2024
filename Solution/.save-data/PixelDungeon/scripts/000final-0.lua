if not context.GetVariable("final-0") then
    local dialogue = Dialogue([[A bloodshot meatball gazes at a group of angry customers. The gaze is keeping them from attacking.]])
    dialogue.AddOption(RunScriptDialogueOption("Fight!", "000-bloodshot0.lua"))
    dialogue.AddOption(CloseDialogueOption("Leave"))
    context.ShowDialogue(dialogue)
else
    context.WriteInfo("The bloodshot meatball appears to be reforming... you better hurry.")
end