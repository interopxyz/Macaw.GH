using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Mp = Aviary.Macaw.Procedural;

namespace Aviary.Macaw.GH.Procedural
{
    public class Cellular2Base : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Cellular2Base class.
        /// </summary>
        public Cellular2Base()
          : base("Cellular 2 Base", "Cellular 2", "Description", "Aviary 1", "Image")
        {
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
            pManager.AddIntegerParameter("Interpolation", "I", "---", GH_ParamAccess.item, 0);
            pManager[5].Optional = true;
            pManager.AddIntegerParameter("Output", "O", "---", GH_ParamAccess.item, 0);
            pManager[6].Optional = true;
            pManager.AddNumberParameter("Jitter", "J", "---", GH_ParamAccess.item, 0.5);
            pManager[7].Optional = true;
            pManager.AddNumberParameter("Frequency", "F", "---", GH_ParamAccess.item, 0.25);
            pManager[8].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[4];
            paramA.AddNamedValue("Distance", 0);
            paramA.AddNamedValue("Addition", 1);
            paramA.AddNamedValue("Subtraction", 2);
            paramA.AddNamedValue("Multiplication", 3);
            paramA.AddNamedValue("Division", 4);

            Param_Integer paramB = (Param_Integer)pManager[5];
            foreach (Mp.Noise.InterpolationModes value in Enum.GetValues(typeof(Mp.Noise.InterpolationModes)))
            {
                paramB.AddNamedValue(value.ToString(), (int)value);
            }

            Param_Integer paramC = (Param_Integer)pManager[6];
            paramC.AddNamedValue("Euclidean", 0);
            paramC.AddNamedValue("Manhattan", 1);
            paramC.AddNamedValue("Natural", 2);
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

            int interp = 0;
            DA.GetData(5, ref interp);

            int output = 0;
            DA.GetData(6, ref output);

            double jitter = 0.5;
            DA.GetData(7, ref jitter);

            double frequency = 0.25;
            DA.GetData(8, ref frequency);


            Mp.Noise noise = new Mp.Noise(seed, width, height, depth);
            noise.InterpolationMode = (Mp.Noise.InterpolationModes)interp;
            noise.CellularMode = (Mp.Noise.CellularModes)mode;
            noise.CellularOutput = (Mp.Noise.CellularOutputs)(output+3);
            noise.Jitter = jitter;
            noise.Frequency = frequency;

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("064939a1-0b83-4b0c-b7a4-17c1e644f086"); }
        }
    }
}