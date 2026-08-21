using FFSC_PicoGK.EngineFFSC.Tests;
using Tests.vscode;

int result1 = TestRunner.RunAll();
int result2 = Test_PipelineCore.RunAll();
int result3 = Test_AllNodes.RunAll();

int totalFailed = result1 + result2 + result3;
Environment.Exit(totalFailed == 0 ? 0 : 1);
