using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Mp = Aviary.Macaw.Procedural;

namespace Aviary.Macaw.GH.Procedural
{
    public class Fractal : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Fractal class.
        /// </summary>
        public Fractal()
          : base("Fractal", "Apply a fractal modifier to an Aviary Cellular or Noise system" + Environment.NewLine + "Built on Auburns' FastNoise " + Environment.NewLine + "https://github.com/Auburns/FastNoise", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Noise", "N", "A Noise object", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("Mode", "M", "The noise fractal mode", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Octaves", "O", "---", GH_ParamAccess.item, 5);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Lacunity", "L", "---", GH_ParamAccess.item, 2);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Gain", "G", "---", GH_ParamAccess.item, 0.5);
            pManager[4].Optional = true;
            
            Param_Integer paramA = (Param_Integer)pManager[1];
            paramA.AddNamedValue("FBM", 0);
            paramA.AddNamedValue("Billow", 1);
            paramA.AddNamedValue("Rigid", 2);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "---", GH_ParamAccess.item);
            pManager.AddGenericParameter("Noise", "N", "---", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mp.Noise noise = new Mp.Noise();
            if (!DA.GetData(0, ref noise));
            noise = new Mp.Noise(noise);

            int mode = 0;
            DA.GetData(1, ref mode);

            int octaves = 5;
            DA.GetData(2, ref octaves);

            double lacunity = 2.0;
            DA.GetData(3, ref lacunity);

            double gain = 0.5;
            DA.GetData(4, ref gain);

            noise.IsFractal = true;
            noise.FractalMode = (Mp.Noise.FractalModes)mode;
            noise.Octaves = octaves;
            noise.Lacunarity = lacunity;
            noise.Gain = gain;

            DA.SetData(0, new Image(noise.GetCurrent()));

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
                return Properties.Resources.Noise_Fractal;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7175f8d0-dab6-4858-9af1-4c2549acaf8e"); }
        }
    }
}