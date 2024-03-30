if not context.GetVariable("has-green-key") then
    context.WriteInfo("The door is locked")
    context.SetPlayerPosition(11, 14);
elseif not context.GetVariable("first-green-exit") then
    context.WriteInfo("The green key opens the door")
    context.SetVariable("first-green-exit", true)
end

