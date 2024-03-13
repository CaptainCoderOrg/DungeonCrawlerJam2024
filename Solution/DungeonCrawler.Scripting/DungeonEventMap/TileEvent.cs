using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;

namespace CaptainCoder.Dungeoneering.DungeonMap;
public record TileEvent(EventTrigger Trigger, EventScript OnTrigger);