using BenchmarkDotNet.Running;
using EVCP.DataConsumer.Benchmarking;

var summary = BenchmarkRunner.Run<ConsumerBenchmarks>();

Console.WriteLine("Listening for messages. Hit <return> to quit.");
Console.ReadLine();