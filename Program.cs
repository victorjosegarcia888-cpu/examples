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

using EngineFFSC.EngineAssembly;
using EngineFFSC.Igniters;
using EngineFFSC.Preburners;
using EngineFFSC.Turbopumps;
using EngineFFSC.CombustionChamber;
using EngineFFSC.Materials;
using EngineFFSC.Geometry;
using PicoGK;

Library.Go(0.0005f, () =>
{
    FFSC_Engine engine = new FFSC_Engine
    {
        Scale = 1.0f,
        IncludeIgniter = true,
        IncludePreburners = true,
        IncludeTurbopump = true,
        IncludeChamber = true,
        IncludeNozzle = true,
        IncludeLattice = true,
        IncludeQuasicrystal = false,
        ExportPath = "FFSC_engine_full.stl"
    };

    engine.SetMaterial("Inconel_A286");

    engine.Assemble();

    engine.ExportSTL("FFSC_engine_full.stl");
});
