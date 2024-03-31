context.HealParty()
local dialogue = Dialogue([[In the following combat, the skelton is too strong to beat in one hit. A move and attack combo won't work. However, if you select attack twice, during your turn you can spend energy to gain bonus movement. This is done by selecting <b>exert</b>. To win without taking damage, select 2 attack actions. Then, use exert to gain a movement point. Finally, move 1 space and defeat the skeleton with 2 attacks.]])
dialogue.AddOption(RunScriptDialogueOption("Fight a Skeleton", "fight-a-skeleton-3.lua"))
dialogue.AddOption(CloseDialogueOption("Leave"))
context.ShowDialogue(dialogue)