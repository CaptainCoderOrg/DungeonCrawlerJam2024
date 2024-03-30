local success = context.GetVariable("final-0") and
  context.GetVariable("final-1") and
  context.GetVariable("final-2") and
  context.GetVariable("final-3")
if success then
    context.WriteInfo("The meat has stopped moving. It appears Eye Key Uh has lost control of them.")
elseif not context.GetVariable("final-2") then
    local dialogue = Dialogue([[A hoard of cosmic meatballs are sleeping in two beds here.]])
    dialogue.AddOption(RunScriptDialogueOption("Fight!", "000-bloodshot2.lua"))
    dialogue.AddOption(CloseDialogueOption("Leave"))
    context.ShowDialogue(dialogue)
else
    context.WriteInfo("The meatballs appears to be reforming... you better hurry.")
end