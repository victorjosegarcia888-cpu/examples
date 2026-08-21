//
// SPDX-License-Identifier: CC0-1.0
//
// This example code file is released to the public under Creative Commons CC0.
// See https://creativecommons.org/publicdomain/zero/1.0/legalcode
//
// To the extent possible under law, the author has waived all copyright and
// related or neighboring rights to this example code file.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using PicoGK;
using PipelineCore;
using FFSC_PicoGK.Pipeline;

Library.Go(0.5f, () =>
{
    string pipelinePath = "Pipeline/pipeline.json";

    var registry = new TaskRegistry();
    PipelineBuilder.RegisterAllTasks(registry);

    Graph graph = PipelineBuilder.BuildPipeline(pipelinePath, registry);

    var scheduler = new Scheduler(registry);
    Voxels? engine = scheduler.ExecuteAndGetResult<Voxels>(graph, "VisualizarMotor");

    if (engine != null)
        Library.oViewer().Add(engine);
});
