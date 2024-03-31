local chance = math.random(1, 3)
if chance == 1 then
    local dialogue = Dialogue([[You accidentally bump shoulders with a disgruntled employee! 'HEY! Watch where yer goin!' The employee raises their fists ready to fight.]])
    dialogue.AddOption(RunScriptDialogueOption("Fight Employee", "03-fight-employee.lua"))
    dialogue.AddOption(RunScriptDialogueOption("Apologize", "03-apologize-employee.lua"))
    context.ShowDialogue(dialogue)
    context.RunScript("spin2.lua")
elseif chance == 2 then
    context.WriteInfo("You narrowly avoid a disgruntled employee")
    context.RunScript("spin2.lua")
end