local chance = math.random(1, 4)
if chance == 1 then
    local dialogue = Dialogue([['Sorry is for suckers!', the employee attacks you!]])
    dialogue.AddOption(RunScriptDialogueOption("Fight Employee", "03-fight-employee.lua"))
    context.ShowDialogue(dialogue)
end