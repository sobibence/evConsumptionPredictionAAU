```

BenchmarkDotNet v0.13.11, Ubuntu 20.04.6 LTS (Focal Fossa)
AMD EPYC Processor (with IBPB), 16 CPU, 16 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 DEBUG
  Job-XKLQTM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

IterationCount=5  LaunchCount=2  WarmupCount=1  

```
| Method           | StartNodeIDs | EndNodeIDs | Mean    | Error    | StdDev   | Min     | Max     | Median  | Allocated |
|----------------- |------------- |----------- |--------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **FullRouteFinding** | **352924816**    | **279727728**  | **1.295 s** | **0.0285 s** | **0.0189 s** | **1.266 s** | **1.320 s** | **1.297 s** | **393.87 KB** |
| **FullRouteFinding** | **352924816**    | **338760520**  | **1.301 s** | **0.0279 s** | **0.0184 s** | **1.271 s** | **1.326 s** | **1.297 s** | **474.36 KB** |
| **FullRouteFinding** | **352924816**    | **828449218**  | **1.277 s** | **0.0375 s** | **0.0223 s** | **1.258 s** | **1.314 s** | **1.265 s** | **280.91 KB** |
| **FullRouteFinding** | **352924816**    | **1709787516** | **1.344 s** | **0.0662 s** | **0.0438 s** | **1.289 s** | **1.406 s** | **1.333 s** | **788.34 KB** |
| **FullRouteFinding** | **352924816**    | **5269743916** |      **NA** |       **NA** |       **NA** |      **NA** |      **NA** |      **NA** |        **NA** |

Benchmarks with issues:
  Benchmarker.FullRouteFinding: Job-XKLQTM(IterationCount=5, LaunchCount=2, WarmupCount=1) [StartNodeIDs=352924816, EndNodeIDs=5269743916]
