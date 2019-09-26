using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Construct
{
    public class BuildBitmap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BuildBitmap class.
        /// </summary>
        public BuildBitmap()
          : base("Build Bitmap", "Build", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Width", "W", "The horizontal pixel resolution", GH_ParamAccess.item, 100);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("Height", "H", "The vertical pixel resolution", GH_ParamAccess.item, 100);
            pManager[1].Optional = true;
            pManager.AddColourParameter("Colors", "C", "The pixels raster colors", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "The resulting Bitmap", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int width = 100;
            DA.GetData(0, ref width);

            int height = 100;
            DA.GetData(1, ref height);

            List<Color> colors = new List<Color>();
            if (!DA.GetDataList(2, colors)) return;

            Bitmap bitmap = new Bitmap(width, height);
            
            int k = 0;
            int c = colors.Count;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    k = (i * width + j) % c;
                    bitmap.SetPixel(j, i, colors[k]);
                }
            }

            DA.SetData(0, bitmap);
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
                return Properties.Resources.BuildBitmap_A;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dd47564d-3433-4595-b878-76e0218c74d1"); }
        }
    }
}