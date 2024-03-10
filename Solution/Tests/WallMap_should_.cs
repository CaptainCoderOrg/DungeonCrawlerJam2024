namespace Tests;

using CaptainCoder.Dungeoneering.Model;
using CaptainCoder.Dungeoneering.Model.IO;

using Shouldly;

public class WallMap_should_
{
    [Fact]
    public void not_update_count_when_setting_matching_shared_wall()
    {
        WallMap underTest = new();
        underTest.SetWall(new Position(5, 5), Facing.North, WallType.Solid);
        underTest.SetWall(new Position(5, 4), Facing.South, WallType.Solid);

        underTest.Count.ShouldBe(1);
        underTest[new Position(5, 5), Facing.North].ShouldBe(WallType.Solid);
        underTest[new Position(5, 4), Facing.South].ShouldBe(WallType.Solid);
    }

    public static IEnumerable<object[]> SaveAsJsonAndRestorFromJsonData()
    {
        WallMap empty = new();
        WallMap oneWall = new();
        oneWall.SetWall(new Position(2, 5), Facing.North, WallType.Solid);

        WallMap threeWall = new();
        threeWall.SetWall(new Position(3, 3), Facing.South, WallType.Door);
        threeWall.SetWall(new Position(3, 3), Facing.East, WallType.Solid);
        threeWall.SetWall(new Position(3, 3), Facing.West, WallType.SecretDoor);
        return [
            [empty],
            [oneWall],
            [threeWall]
        ];
    }

    [Theory]
    [MemberData(nameof(SaveAsJsonAndRestorFromJsonData))]
    public void save_as_json_and_restore_from_json(WallMap underTest)
    {
        string json = underTest.ToJson();
        WallMap actual = JsonExtensions.LoadModel<WallMap>(json);
        underTest.Map.Count.ShouldBe(actual.Map.Count);
        underTest.Map.Keys.ShouldBeSubsetOf(actual.Map.Keys);
        foreach (var key in underTest.Map.Keys)
        {
            underTest.Map[key].ShouldBe(actual.Map[key]);
        }
    }
}