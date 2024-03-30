if not context.GetVariable("green-room-desc2") then
    context.WriteInfo("Disgruntled employees wander about muttering profanties. You better be careful!")
    context.SetVariable("green-room-desc2", true)
end