context.WriteInfo("It's a meat locker! And it is filled with Cosmic Meatballs!")
local setup = [[
    #####    ###E 
    #####   3#####
    ###MM###1#####E
    ###MM###2#####E
            4#####
             ###E
]]
context.StartCombat(setup, "03-win-meat-locker.lua", "return-to-altar.lua")