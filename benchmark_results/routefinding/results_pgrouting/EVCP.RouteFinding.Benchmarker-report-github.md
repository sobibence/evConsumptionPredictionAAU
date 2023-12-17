```

BenchmarkDotNet v0.13.11, Ubuntu 20.04.6 LTS (Focal Fossa)
AMD EPYC Processor (with IBPB), 16 CPU, 16 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 DEBUG
  Job-AQPBSL : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

IterationCount=5  LaunchCount=2  WarmupCount=1  

```
| Method           | StartNodeIDs | EndNodeIDs | Mean    | Error    | StdDev   | Min     | Max     | Median  | Allocated |
|----------------- |------------- |----------- |--------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **FullRouteFinding** | **352924816**    | **279727728**  | **1.304 s** | **0.0521 s** | **0.0345 s** | **1.263 s** | **1.375 s** | **1.300 s** | **393.83 KB** |
| **FullRouteFinding** | **352924816**    | **338760520**  | **1.294 s** | **0.0426 s** | **0.0223 s** | **1.256 s** | **1.316 s** | **1.304 s** | **474.94 KB** |
| **FullRouteFinding** | **352924816**    | **828449218**  | **1.283 s** | **0.0685 s** | **0.0453 s** | **1.241 s** | **1.377 s** | **1.263 s** | **280.81 KB** |
| **FullRouteFinding** | **352924816**    | **1709787516** | **1.340 s** | **0.0835 s** | **0.0497 s** | **1.272 s** | **1.422 s** | **1.321 s** | **755.81 KB** |
| **FullRouteFinding** | **352924816**    | **5269743916** | **1.374 s** | **0.0846 s** | **0.0560 s** | **1.285 s** | **1.442 s** | **1.366 s** | **876.95 KB** |
