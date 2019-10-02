using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Si = System.Drawing.Imaging;

namespace Aviary.Macaw.GH.Output
{
    public class ExportBitmap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExportBitmap class.
        /// </summary>
        public ExportBitmap()
          : base("Export Bitmap", "Bmp Out", "Save the image to a bitmap", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.septenary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Drawing", "D", "An Aviary drawing object", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "P", "Set the filepath", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Name", "N", "Set the filename (no extension)", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Extention", "E", "Select an Extension (png=0, jpeg=1, bmp=2, tiff=3, gif=4)", GH_ParamAccess.item, 0);
            pManager[3].Optional = true;
            pManager.AddBooleanParameter("Save", "S", "Will save the file when true (recommend using a button)", GH_ParamAccess.item, false);
            pManager[4].Optional = true;

            Param_Integer param = (Param_Integer)Params.Input[3];
            param.AddNamedValue("Png", 0);
            param.AddNamedValue("Jpeg", 1);
            param.AddNamedValue("Bmp", 2);
            param.AddNamedValue("Tiff", 3);
            param.AddNamedValue("Gif", 4);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Filepath", "F", "The resulting filepath", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            Bitmap bitmap = new Bitmap(100, 100);
            if (!DA.GetData(0, ref goo)) return;
            if (!goo.TryGetBitmap(ref bitmap)) return;
            
            string path = "C:\\Users\\Public\\Documents\\";
            string name = DateTime.UtcNow.ToString("yyyy-dd-M_HH-mm-ss"); ;
            int extension = 0;
            string format = ".png";
            bool save = false;

            bool hasPath = DA.GetData(1, ref path);
            bool hasName = DA.GetData(2, ref name);
            if (!DA.GetData(3, ref extension)) return;
            if (!DA.GetData(4, ref save)) return;

            Si.ImageFormat encoding = Si.ImageFormat.Png;

            switch (extension)
            {
                case 1:
                    encoding = Si.ImageFormat.Jpeg;
                    format = ".jpeg";
                    break;
                case 2:
                    encoding = Si.ImageFormat.Bmp;
                    format = ".bmp";
                    break;
                case 3:
                    encoding = Si.ImageFormat.Tiff;
                    format = ".tiff";
                    break;
                case 4:
                    encoding = Si.ImageFormat.Gif;
                    format = ".gif";
                    break;
            }

            Bitmap bmp = (Bitmap)bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Si.PixelFormat.Format32bppArgb);

            if (!hasPath) { if (this.OnPingDocument().FilePath != null) { path = Path.GetDirectoryName(this.OnPingDocument().FilePath) + "\\"; } } else { path += "//"; }

            string filepath = path + name + format;

            if (save)
            {
                bmp.Save(filepath,encoding);
                bmp.Dispose();
            }

            DA.SetData(0, filepath);


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
                return Properties.Resources.SaveImage;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7cc40761-7cb4-4cff-bc87-d295d78101b0"); }
        }
    }
}