if not context.GetVariable("green-room-desc2") then
    context.WriteInfo("A few disgruntled employees wander in this area... better watch your step")
    context.SetVariable("green-room-desc2", true)
end