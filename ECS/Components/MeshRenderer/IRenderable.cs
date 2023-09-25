using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineLibrary.ECS.Components
{
    internal interface IRenderable
    {
        void Render(float dt);
    }
}
