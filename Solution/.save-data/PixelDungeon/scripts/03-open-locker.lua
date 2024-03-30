context.WriteInfo("It's a meat locker! And it is filled with Cosmic Meatballs!")
local setup = [[
    #####    ###S 
    #####   3#####
    ###MM###1#####S
    ###MM###2#####S
            4#####
             ###S
]]
context.StartCombat(setup, "03-win-meat-locker.lua", "return-to-altar.lua")