local dialogue = Dialogue([[You are standing in a small closet filled with skeletons]])
dialogue.AddOption(RunScriptDialogueOption("Fight Skeletons", "fight-skeletons.lua"))
dialogue.AddOption(CloseDialogueOption("Leave"))
context.ShowDialogue(dialogue)