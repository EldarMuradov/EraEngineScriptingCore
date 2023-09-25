using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLibrary.ECS.Components.Scripting;
using EngineLibrary.Math;

namespace EngineLibrary.ECS.Components
{
    public class Script : Component, IExecutable
    {
        internal string Name { get; private set; }

        public void Execute()
        {
        }

        public override void Start()
        {

        }

        public override void Update()
        {

        }

        internal override void Init(params object[] args)
        {
            Name = (string)args[0];
        }
    }
}
