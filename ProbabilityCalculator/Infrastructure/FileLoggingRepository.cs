namespace ProbabilityCalculator.API.Infrastructure;

using System;
using System.IO;
using System.IO.Abstractions;

public class FileLoggingRepository : ILoggingRepository
{
    private readonly IFileSystem _fileSystem;
    private const string Filename = "calculations.txt";

    public FileLoggingRepository(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public async Task Log(string operation, decimal a, decimal b, decimal result)
    {
        await using StreamWriter writer = new(_fileSystem.FileStream.New(Filename, FileMode.Append, FileAccess.Write));
        await writer.WriteLineAsync($"{DateTime.Now}: Operation: {operation}, ProbabilityA: {a}, ProbabilityB: {b}, Result: {result}");
    }
}