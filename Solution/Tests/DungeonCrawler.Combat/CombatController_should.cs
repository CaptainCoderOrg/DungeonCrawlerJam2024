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
        Position start = new (x, y);
        Position end = new (targetX, targetY);
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

        MoveAction action = new (start, end);

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
        Position start = new (x, y);
        Position end = new (targetX, targetY);
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

        MoveAction action = new (start, end);

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
        CombatMap underTest = new ()
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

    // TODO: Move action is not valid if there is no player in the starting position

    // public void apply_move_action()
    // {

    // }

    // public void throw_exception_on_invalid_move_action()
    // {

    // }
}