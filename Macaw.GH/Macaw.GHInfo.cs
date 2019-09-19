using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Aviary.Macaw.GH
{
    public class MacawGHInfo : GH_AssemblyInfo
  {
    public override string Name
    {
        get
        {
            return "MacawGH";
        }
    }
    public override Bitmap Icon
    {
        get
        {
            //Return a 24x24 pixel bitmap to represent this GHA library.
            return null;
        }
    }
    public override string Description
    {
        get
        {
            //Return a short string describing the purpose of this GHA library.
            return "";
        }
    }
    public override Guid Id
    {
        get
        {
            return new Guid("250c9dbf-d7c7-4280-844e-7bfa8bbe7c7d");
        }
    }

    public override string AuthorName
    {
        get
        {
            //Return a string identifying you or your company.
            return "";
        }
    }
    public override string AuthorContact
    {
        get
        {
            //Return a string representing your preferred contact details.
            return "";
        }
    }
}
}
