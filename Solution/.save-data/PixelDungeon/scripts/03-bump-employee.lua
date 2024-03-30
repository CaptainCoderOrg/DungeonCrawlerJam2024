local chance = math.random(1, 4)
if chance == 1 then
    local dialogue = Dialogue([[You accidentally bump shoulders with a disgruntled employee! 'HEY! Watch where yer goin!' The employee raises their fists ready to fight.]])
    dialogue.AddOption(RunScriptDialogueOption("Fight Employee", "03-fight-employee.lua"))
    dialogue.AddOption(CloseDialogueOption("Apologize"))
    context.ShowDialogue(dialogue)
    context.RunScript("spin2.lua")
elseif chance == 2 then
    context.WriteInfo("You narrowly avoid a disgruntled employee")
    context.RunScript("spin2.lua")
end