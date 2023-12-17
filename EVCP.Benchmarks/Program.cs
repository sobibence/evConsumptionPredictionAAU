using BenchmarkDotNet.Running;
using EVCP.Benchmarks;

Console.WriteLine("Welcome to Benchmarking!");

var summary = BenchmarkRunner.Run<ConsumerBenchmarks>();
