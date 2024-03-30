if not context.GetVariable("green-room-desc") then
    context.WriteInfo("This is an employee locker room... but where are the employees? And why aren't there any lockers?")
    context.SetVariable("green-room-desc", true)
end