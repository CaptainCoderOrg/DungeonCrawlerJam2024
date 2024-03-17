using System.IO.Abstractions.TestingHelpers;

Console.Clear();

string root = Path.Combine("project");
string dungeons = Path.Combine(root, "dungeons");
string scripts = Path.Combine(root, "scripts");

Console.WriteLine(root);

MockFileSystem fileSystem = new();
fileSystem.AddDirectory(dungeons);
fileSystem.AddDirectory(scripts);

MockFileData data = fileSystem.GetFile(dungeons);
MockFileData data2 = fileSystem.GetFile("banana");
Console.WriteLine(data.IsDirectory);
Console.WriteLine(data2 is null);