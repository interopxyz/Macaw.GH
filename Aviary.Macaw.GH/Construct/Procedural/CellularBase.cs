using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Mp = Aviary.Macaw.Procedural;

namespace Aviary.Macaw.GH.Procedural
{
    public class CellularBase : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent3 class.
        /// </summary>
        public CellularBase()
          : base("Cellular", "Cellular", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Seed", "S", "The seed for the noise", GH_ParamAccess.item, 1);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("Width", "W", "---", GH_ParamAccess.item, 100);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Height", "H", "---", GH_ParamAccess.item, 100);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Depth", "D", "---", GH_ParamAccess.item, 1);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Mode", "M", "---", GH_ParamAccess.item, 0);
            pManager[4].Optional = true;
            pManager.AddIntegerParameter("Output", "O", "---", GH_ParamAccess.item, 0);
            pManager[5].Optional = true;
            pManager.AddNumberParameter("Jitter", "J", "---", GH_ParamAccess.item, 0.5);
            pManager[6].Optional = true;
            pManager.AddNumberParameter("Frequency", "F", "---", GH_ParamAccess.item, 0.25);
            pManager[7].Optional = true;
            pManager.AddIntervalParameter("Interval", "I", "---", GH_ParamAccess.item, new Interval(0,1));
            pManager[8].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[4];
            paramA.AddNamedValue("Euclidean", 0);
            paramA.AddNamedValue("Manhattan", 1);
            paramA.AddNamedValue("Natural", 2);

            Param_Integer paramB = (Param_Integer)pManager[5];
            paramB.AddNamedValue("Value", 0);
            paramB.AddNamedValue("Distance", 2);
            paramB.AddNamedValue("Distance2", 3);
            paramB.AddNamedValue("Dist2Add", 4);
            paramB.AddNamedValue("Dist2Sub", 5);
            paramB.AddNamedValue("Dist2Mul", 6);
            paramB.AddNamedValue("Dist2Div", 7);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "---", GH_ParamAccess.item);
            pManager.AddGenericParameter("Noise", "N", "---", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int seed = 1;
            int width = 100;
            int height = 100;
            int depth = 1;
            DA.GetData(0, ref seed);
            DA.GetData(1, ref width);
            DA.GetData(2, ref height);
            DA.GetData(3, ref depth);

            int mode = 0;
            DA.GetData(4, ref mode);
            
            int output = 0;
            DA.GetData(5, ref output);

            double jitter = 0.5;
            DA.GetData(6, ref jitter);

            double frequency = 0.25;
            DA.GetData(7, ref frequency);

            Interval interval = new Interval(0,1);
            DA.GetData(8, ref interval);
            
            Mp.Noise noise = new Mp.Noise(seed, width, height, depth);
            noise.CellularMode = (Mp.Noise.CellularModes)mode;
            noise.CellularOutput = (Mp.Noise.CellularOutputs)output;
            noise.Jitter = jitter;
            noise.Frequency = frequency;
            noise.Index0 = (int)interval.T0;
            noise.Index1 = (int)interval.T1;

            DA.SetData(0, noise.GetCellular());
            DA.SetData(1, new Mp.Noise(noise));
        }


        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.Noise_Cellular;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0a31d869-9164-4f41-a745-94ef8a68767a"); }
        }
    }
}