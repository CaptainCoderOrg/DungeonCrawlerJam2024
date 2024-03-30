--context.ChangeDungeon("Ikea", 11, 11, West)

context.RunScript("begin-quest.lua")
context.AddPartyMember(1)
context.SetVariable("has-green-key", true)
context.SetVariable("defeated-green-room.lua", true)