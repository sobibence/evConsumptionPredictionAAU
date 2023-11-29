using BenchmarkDotNet.Running;
using EVCP.DataConsumer.Benchmarking;

//var summary = BenchmarkRunner.Run<Demo>();
var summary = BenchmarkRunner.Run<BenchmarkingConsumer>();

Console.WriteLine("Listening for messages. Hit <return> to quit.");
Console.ReadLine();