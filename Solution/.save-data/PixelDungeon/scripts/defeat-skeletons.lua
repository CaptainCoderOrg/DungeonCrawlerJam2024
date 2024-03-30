local adjectives = {
    "squishy",
    "rubbery",
    "giggly",
    "wobbly",
    "squeaky",
    "bouncy",
    "fluffy",
    "sparkly",
    "slippery",
    "ticklish"
}

-- Function to randomly select an adjective
function selectRandomAdjective(adjectives)
    -- #adjectives gives the size of the table
    local index = math.random(1, #adjectives)
    return adjectives[index]
end

-- Select a random adjective and print it
local randomAdjective = selectRandomAdjective(adjectives)