local chance = math.random(1, 4)
local map = ""
if chance == 1 then
    map = [[
        ##      ##
        ##      ##
        ##      ##
        #B########
        ######B###
        ##1###4###
        ###2#3####

    ]]
elseif chance == 2 then
    map = [[
        #B      ##
        ##      #B
        ##      ##
        #1########
        ######2###
        ##B#######
        4####B###3

    ]]
elseif chance == 3 then
    map = [[
        #B      ##
        ##      #B
        #1      ##
        #######2##
        ##########
        ##########
        4B###3####

    ]]
else
    map = [[
        ##      ##
        ##      ##
        ##      ##
        #####B####
        ##12######
        ##34######
        B####B####
    ]]
end

context.StartCombat(map, "03-defeat-employees.lua", "return-to-altar.lua")