local dialogue = Dialogue([[This room is lined with cosmic wardrobes. The employees use them as lockers.]])
local search = Dialogue([[The wardrobes spring to life! Prepare for battle.]])
search.AddOption(RunScriptDialogueOption("Fight!", "03-fight-lockers.lua"))
dialogue.AddOption(ContinueDialogueOption("Search Wardrobes", search))
dialogue.AddOption(CloseDialogueOption("Leave"))
context.ShowDialogue(dialogue)