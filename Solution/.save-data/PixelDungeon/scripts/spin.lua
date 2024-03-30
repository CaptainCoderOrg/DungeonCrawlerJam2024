function newDir()
    local dir = math.random(0, 3)
    if (dir == context.PlayerView.Facing) then
        return newDir()
    end
    return dir
end
context.WriteInfo("The stench makes you spin")
context.SetPlayerFacing(newDir())