local dialogue = Dialogue([[You are standing in a small closet filled with skeletons]])
dialogue.AddOption(RunScriptDialogueOption("Search Closet", "fight-skeletons.lua"))
dialogue.AddOption(RunScriptDialogueOption("Leave", "push-back.lua"))
context.ShowDialogue(dialogue)