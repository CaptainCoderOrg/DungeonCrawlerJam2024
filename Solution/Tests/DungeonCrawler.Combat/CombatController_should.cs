using CaptainCoder.DungeonCrawler.Combat;

using Shouldly;

namespace Tests;
public class CombatController_should
{
    public string TestMap =
    """
     ##    ## 0
    ##########1
     ##    ## 2
    0123456789
    """;
    [Theory]
    [InlineData(0, 1, 4, 1, 1)]
    [InlineData(7, 0, 3, 7, 1)]
    [InlineData(7, 0, 3, 6, 1)]
    [InlineData(7, 0, 3, 5, 1)]
    public void validate_move_action(int x, int y, int movementPoints, int targetX, int targetY)
    {
        // Arrange
        Position start = new(x, y);
        Position end = new(targetX, targetY);
        CombatMap map = new()
        {
            Tiles = CombatMapExtensions.ParseTiles(TestMap),
            PlayerCharacters = new()
            {
                { new Position(x, y), new PlayerCharacter() { MovementPoints = movementPoints } },
                // This PlayerCharacter can be moved through, but you cannot end your move in this space
                { new Position(4, 1), new PlayerCharacter(new CharacterCard("Obstacle")) },
            }
        };

        MoveAction action = new(start, end);

        // Act
        bool actual = map.IsValidMoveAction(action);

        // Assert
        actual.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0, 1, 4, 4, 1)] // Cannot end on other players
    [InlineData(0, 1, 4, 3, 0)] // Cannot move into wall space
    [InlineData(0, 1, 4, 0, 0)] // Cannot move into wall space
    [InlineData(0, 1, 4, 0, 2)] // Cannot move into wall space
    [InlineData(3, 1, 1, 5, 1)] // Cannot move into other player
    [InlineData(3, 1, 1, 5, 2)] // Moving through player counts as movement
    [InlineData(3, 1, 1, 3, 0)] // Cannot move into wall space
    [InlineData(3, 1, 1, 3, 1)] // Cannot move into wall space
    [InlineData(3, 1, 1, 1, 0)] // Too far
    [InlineData(3, 1, 1, 1, 1)] // Too far
    [InlineData(3, 1, 1, 1, 2)] // Too far
    public void invalidate_move_action(int x, int y, int movementPoints, int targetX, int targetY)
    {
        // """
        //  ##    ## 0
        // ####*#####1
        //  ##    ## 2
        // 0123456789
        // """;

        // Arrange
        Position start = new(x, y);
        Position end = new(targetX, targetY);
        CombatMap map = new()
        {
            Tiles = CombatMapExtensions.ParseTiles(TestMap),
            PlayerCharacters = new()
            {
                { new Position(x, y), new PlayerCharacter() { MovementPoints = movementPoints } },
                // This PlayerCharacter can be moved through, but you cannot end your move in this space
                { new Position(4, 1), new PlayerCharacter(new CharacterCard("Obstacle")) },
            }
        };

        MoveAction action = new(start, end);

        // Act
        bool actual = map.IsValidMoveAction(action);

        // Assert
        actual.ShouldBeFalse();
    }

    public static IEnumerable<object[]> FindValidMovesData => [
        [
            1,
            """
            ####
            ####
            ####
            ####
            """,
            """
            #**D
            #*A*
            #B**
            ##C#
            """,
        ],
        [
            4,
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             #*    ***
            ##**B*A****
            ##**C******
             #*    *D*
            """,
        ],

        [
            3,
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ***
            ###*B*A***#
            ###*C*****#
             ##    *D*
            """,
        ],
        [
            2,
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    **#
            ####B*A**##
            ####C****##
             ##    *D#
            """,
        ],

    ];
    // Each letter represents a PlayerCharacter
    // Each * represents where the PlayerCharacter marked as A can move
    [Theory]
    [MemberData(nameof(FindValidMovesData))]
    public void find_valid_moves(int movementPoints, string map, string expectedMap)
    {
        HashSet<Position> tiles = CombatMapExtensions.ParseTiles(map);
        Dictionary<char, HashSet<Position>> setupMap = CombatMapExtensions.ParseCharPositions(expectedMap);
        Position start = setupMap['A'].First();
        Position char2 = setupMap['B'].First();
        Position char3 = setupMap['C'].First();
        Position char4 = setupMap['D'].First();
        HashSet<Position> expectedValidMoves = setupMap['*'];
        CombatMap underTest = new()
        {
            Tiles = tiles,
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { start, new PlayerCharacter(){ MovementPoints = movementPoints } },
                { char2, new PlayerCharacter(){ } },
                { char3, new PlayerCharacter(){ } },
                { char4, new PlayerCharacter(){ } }
            },
        };

        HashSet<Position> actual = underTest.FindValidMoves(start, movementPoints);

        actual.Count.ShouldBe(expectedValidMoves.Count);
        actual.ShouldBeSubsetOf(expectedValidMoves);
    }

    public static IEnumerable<object[]> PlayerCannotMoveThroughEnemiesData => [
        [
            1,
            """
            ####
            ####
            ####
            ####
            """,
            """
            #**D
            #*A*
            #B**
            ##C#
            """,
        ],
        [
            4,
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ***
            ####B*A****
            ####C******
             ##    *D*
            """,
        ],

        [
            3,
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ***
            ####B*A***#
            ####C*****#
             ##    *D*
            """,
        ],
        [
            2,
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             #*    ###
            ##**B**####
            ##*CAD*####
             ##    ###
            """,
        ],

    ];
    // The letter 'A' represents the PlayerCharacter moving
    // Each * represents where the PlayerCharacter marked as A can move
    // B, C, and D represents enemies
    [Theory]
    [MemberData(nameof(PlayerCannotMoveThroughEnemiesData))]
    public void prevent_player_from_moving_through_enemy(int movementPoints, string map, string expectedMap)
    {
        HashSet<Position> tiles = CombatMapExtensions.ParseTiles(map);
        Dictionary<char, HashSet<Position>> setupMap = CombatMapExtensions.ParseCharPositions(expectedMap);
        Position start = setupMap['A'].First();
        Position enemyB = setupMap['B'].First();
        Position enemyC = setupMap['C'].First();
        Position enemyD = setupMap['D'].First();
        HashSet<Position> expectedValidMoves = setupMap['*'];
        CombatMap underTest = new()
        {
            Tiles = tiles,
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { start, new PlayerCharacter(){ MovementPoints = movementPoints } },
            },
            Enemies = new Dictionary<Position, Enemy>()
            {
                { enemyB, new Enemy() },
                { enemyC, new Enemy() },
                { enemyD, new Enemy() },
            }
        };

        HashSet<Position> actual = underTest.FindValidMoves(start, movementPoints);

        actual.Count.ShouldBe(expectedValidMoves.Count);
        actual.ShouldBeSubsetOf(expectedValidMoves);
    }

    // A - Player that is moving
    // T - Target position to move to
    // C, B - PlayerCharacters
    // 1, 2 - Enemies
    public static IEnumerable<object[]> ApplyMoveActionData => [
        [
            """
            ####
            ####
            ####
            ####
            """,
            """
            A###
            #T##
            ##CB
            ##12
            """,
            4,
            3
        ],
        [
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             BC    ###
            1#####T####
            2##########
             ##    ##A
            """,
            4,
            1
        ],

        [ // Enemies block movement, so the player must go AROUND the enemies
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ###
            B#####T####
            C######21##
             ##    ##A
            """,
            4,
            0
        ],
        [ // PlayerCharacters do not block movement
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ###
            1#####T####
            2######BC##
             ##    ##A
            """,
            4,
            1
        ],

    ];

    [Theory]
    [MemberData(nameof(ApplyMoveActionData))]
    public void apply_move_action(string map, string setup, int movementPoints, int expectedMovementPoints)
    {
        // Arrange
        HashSet<Position> tiles = CombatMapExtensions.ParseTiles(map);
        Dictionary<char, HashSet<Position>> setupMap = CombatMapExtensions.ParseCharPositions(setup);
        Position start = setupMap['A'].First();
        Position target = setupMap['T'].First();
        Position pcB = setupMap['B'].First();
        Position pcC = setupMap['C'].First();
        Position enemy1 = setupMap['1'].First();
        Position enemy2 = setupMap['2'].First();
        CombatMap underTest = new()
        {
            Tiles = tiles,
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { start, new PlayerCharacter(){ MovementPoints = movementPoints } },
                { pcB, new PlayerCharacter() },
                { pcC, new PlayerCharacter() },
            },
            Enemies = new Dictionary<Position, Enemy>()
            {
                { enemy1, new Enemy() },
                { enemy2, new Enemy() },
            }
        };

        MoveAction moveAction = new(start, target);

        // Act
        underTest.ApplyMoveAction(moveAction);

        underTest.PlayerCharacters.ShouldNotContainKey(start);
        underTest.PlayerCharacters[target].ShouldBe(new PlayerCharacter() { MovementPoints = expectedMovementPoints });
    }

    // A is the PlayerCharacter moving
    // T is the target position
    // 1, 2 - Enemies
    // B, C - PlayerCharacters
    public static IEnumerable<object[]> FindShortestPathData => [
        [
            """
            ####
            ####
            ####
            ####
            """,
            """
            A##B
            ###C
            ##T1
            ###2
            """,
            (Position[])[new Position(1, 1), new Position(2, 2)]
        ],
        [ // PlayerCharacters do not block movement
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ### 
            1######T###
            2######BC##
             ##    ##A 
            """,
            (Position[])[new Position(8, 2), new Position(7, 1)]
        ],
        [ // Must move around enemies
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ### 
            B######T###
            C#######1##
             ##    #2A 
            """,
            (Position[])[new Position(9, 2), new Position(8, 1), new Position(7, 1)]
        ],

    ];

    [Theory]
    [MemberData(nameof(FindShortestPathData))]
    public void find_shortest_path(string map, string setup, Position[] expectedPath)
    {
        // Arrange
        HashSet<Position> tiles = CombatMapExtensions.ParseTiles(map);
        Dictionary<char, HashSet<Position>> setupMap = CombatMapExtensions.ParseCharPositions(setup);
        Position start = setupMap['A'].First();
        Position target = setupMap['T'].First();
        Position pcB = setupMap['B'].First();
        Position pcC = setupMap['C'].First();
        Position enemy1 = setupMap['1'].First();
        Position enemy2 = setupMap['2'].First();
        CombatMap underTest = new()
        {
            Tiles = tiles,
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { start, new PlayerCharacter(){ MovementPoints = 4 } },
                { pcB, new PlayerCharacter() },
                { pcC, new PlayerCharacter() },
            },
            Enemies = new Dictionary<Position, Enemy>()
            {
                { enemy1, new Enemy() },
                { enemy2, new Enemy() },
            }
        };

        // Act
        Position[] actual = [.. underTest.FindShortestPath(start, target)];

        actual.SequenceEqual(expectedPath).ShouldBeTrue();
    }

    // A is the PlayerCharacter moving
    // T is the target position
    // 1, 2 - Enemies
    // B, C - PlayerCharacters
    public static IEnumerable<object[]> ThrowExceptionOnInvalidMoveActionData => [

        [ // PlayerCharacters do not block movement
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ### 
            1##T#######
            2######BC##
             ##    ##A 
            """
        ],
        [ // Must move around enemies
            """
             ##    ###
            ###########
            ###########
             ##    ###
            """,
            """
             ##    ### 
            B####T#####
            C#######1##
             ##    #2A 
            """
        ],

    ];

    [Theory]
    [MemberData(nameof(ThrowExceptionOnInvalidMoveActionData))]
    public void throw_exception_on_invalid_move_action(string map, string setup)
    {
        // Arrange
        HashSet<Position> tiles = CombatMapExtensions.ParseTiles(map);
        Dictionary<char, HashSet<Position>> setupMap = CombatMapExtensions.ParseCharPositions(setup);
        Position start = setupMap['A'].First();
        Position target = setupMap['T'].First();
        Position pcB = setupMap['B'].First();
        Position pcC = setupMap['C'].First();
        Position enemy1 = setupMap['1'].First();
        Position enemy2 = setupMap['2'].First();
        CombatMap underTest = new()
        {
            Tiles = tiles,
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { start, new PlayerCharacter(){ MovementPoints = 4 } },
                { pcB, new PlayerCharacter() },
                { pcC, new PlayerCharacter() },
            },
            Enemies = new Dictionary<Position, Enemy>()
            {
                { enemy1, new Enemy() },
                { enemy2, new Enemy() },
            }
        };

        // Act
        Should.Throw<ArgumentException>(() => underTest.ApplyMoveAction(new MoveAction(start, target)));

    }
}