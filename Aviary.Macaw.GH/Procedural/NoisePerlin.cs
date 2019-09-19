using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Mp = Aviary.Macaw.Procedural;

namespace Aviary.Macaw.GH
{
    public class PerlinNoise : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PerturbNoise class.
        /// </summary>
        public PerlinNoise()
          : base("Perlin Noise", "Perlin", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Width", "W", "---", GH_ParamAccess.item, 100);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("Height", "H", "---", GH_ParamAccess.item, 100);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Depth", "D", "---", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Octaves", "O", "---", GH_ParamAccess.item, 1);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Frequency", "F", "---", GH_ParamAccess.item, 1.0);
            pManager[4].Optional = true;
            pManager.AddNumberParameter("Lacunarity", "L", "---", GH_ParamAccess.item, 1.0);
            pManager[5].Optional = true;
            pManager.AddNumberParameter("Gain", "G", "---", GH_ParamAccess.item, 1.0);
            pManager[6].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "---", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int width = 100;
            int height = 100;
            int depth= 1;
            DA.GetData(0, ref width);
            DA.GetData(1, ref height);
            DA.GetData(2, ref depth);

            int octaves = 1;
            DA.GetData(3, ref octaves);

            double frequency = 1.0;
            DA.GetData(4, ref frequency);

            double lacunarity = 1.0;
            DA.GetData(5, ref lacunarity);

            double gain = 1.0;
            DA.GetData(6, ref gain);

            Mp.Noise noise = new Mp.Noise(1, width, height,depth);
            noise.Frequency = frequency;
            noise.Gain = gain;
            noise.Lacunarity = lacunarity;
            noise.Octaves = octaves;

            DA.SetData(0,noise.GetPerlin());
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
            get { return new Guid("996fb7ce-44e9-49df-827b-4e4d6d09c57a"); }
        }
    }
}