using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Aviary.Macaw.GH
{
    public class AviaryMacawGHInfo : GH_AssemblyInfo
  {
    public override string Name
    {
        get
        {
            return "AviaryMacawGH";
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
            return new Guid("3437bc8b-87aa-4d62-bc8f-068422688a0d");
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
