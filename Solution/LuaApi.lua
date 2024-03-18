---@meta

North = 0
East = 1
South = 2
West = 3

None = 0
Solid = 1
Door = 2
Secret = 3

---@class context
context = {}

---@class PlayerView
context.PlayerView = {}

context.PlayerView.Position = {}
context.PlayerView.Position.X = 0
context.PlayerView.Position.Y = 0
context.PlayerView.Facing = North

--- Sets the player's position
function context.SetPlayerPosition(x, y) end

--- Sets the player's position and facing.
function context.SetPlayerView(x, y, facing) end

--- Sets the player's facing. Use: North, East, South, or West
function context.SetPlayerFacing(facing) end

--- Retrieves the type of wall the player is currently facing: Solid, Door,
--  Secret, or None
function context.GetWall() end

--- Retrieves the type of wall at the specified location and facing.
function context.GetWallAt(x, y, facing) end

--- Sets the wall the player is currently facing. Use: Solid, Door, Secret, or None
function context.SetWall() end

--- Sets the wall at the specified position and facing to the specified type.
function context.SetWallAt(x, y, facing, type) end

--- Writes a debug message to the console
function context.Debug(message) end

--- Writes information to the player.
function context.WriteInfo(message) end

--- Sets a global variable that can be used between scripts
function context.SetVariable(name, value) end

--- Retrieves a global variable or nil if no value was previously set
function context.GetVariable(name) end

--- Teleports the player to a new dungeon and sets their position and facing
function context.ChangeDungeon(name, x, y, facing) end