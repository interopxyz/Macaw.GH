using System;
using System.Collections.Generic;
using Sd = System.Drawing;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;


namespace Aviary.Macaw.GH
{
    public class ImageViewer : GH_Component
    {
        public Sd.Image img = null;
        string message = "Nothing here";

        /// <summary>
        /// Initializes a new instance of the BitmapViewer class.
        /// </summary>
        public ImageViewer()
          : base("Image Viewer", "Image", "Display an Aviary Image or Bitmap in the canvas", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.septenary; }
        }

        public override void CreateAttributes()
        {
            img = Properties.Resources.ImageViewer_Background;
            m_attributes = new Attributes_Custom(this);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "An Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            Sd.Bitmap bitmap = new Sd.Bitmap(100, 100);

            if (DA.GetData(0, ref goo))
            {
                if (!goo.CastTo<Sd.Bitmap>(out bitmap))
                {
                    Image image = new Image();
                    if (goo.CastTo<Image>(out image))
                    {
                        bitmap = image.GetFilteredBitmap();
                    }

                    else
                    {
                        bitmap = Properties.Resources.ImageViewer_Background;
                    }
                }
            }
            else
            {
                bitmap = Properties.Resources.ImageViewer_Background;
            }

            img = (Sd.Bitmap)bitmap.Clone();
            message = bitmap.PixelFormat.ToString();
            UpdateMessage();
        }

        private void UpdateMessage()
        {
            Message = message;
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
                return Properties.Resources.ExportBitmap24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8b1a4d1c-08d2-492e-b84c-b085c3aa4b0d"); }
        }
    }

    public class Attributes_Custom : GH_ComponentAttributes
    {
        public Attributes_Custom(GH_Component owner) : base(owner) { }

        private Sd.Rectangle ButtonBounds { get; set; }
        protected override void Layout()
        {
            base.Layout();
            ImageViewer comp = Owner as ImageViewer;

            int width = comp.img.Width;
            int height = comp.img.Height;
            Sd.Rectangle rec0 = GH_Convert.ToRectangle(Bounds);

            int cWidth = rec0.Width;
            int cHeight = rec0.Height;

            rec0.Width = width;
            rec0.Height += height;

            Sd.Rectangle rec1 = rec0;
            rec1.Y = rec1.Bottom - height;
            rec1.Height = height;
            rec1.Width = width;

            Bounds = rec0;
            ButtonBounds = rec1;

        }

        protected override void Render(GH_Canvas canvas, Sd.Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);
            ImageViewer comp = Owner as ImageViewer;

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule capsule = GH_Capsule.CreateCapsule(ButtonBounds, GH_Palette.Normal, 0, 0);
                capsule.Render(graphics, Selected, Owner.Locked, true);
                capsule.AddOutputGrip(this.OutputGrip.Y);
                capsule.Dispose();
                capsule = null;

                Sd.StringFormat format = new Sd.StringFormat();
                format.Alignment = Sd.StringAlignment.Center;
                format.LineAlignment = Sd.StringAlignment.Center;

                Sd.RectangleF textRectangle = ButtonBounds;

                graphics.DrawImage(comp.img, Bounds.X + 2, m_innerBounds.Y - (ButtonBounds.Height - Bounds.Height), comp.img.Width - 4, comp.img.Height - 2);

                format.Dispose();
            }
        }
    }
}