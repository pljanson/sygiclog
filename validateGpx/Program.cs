using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Text;

namespace validateGpx
{
    class Program
    {
        //static bool bWaitConsole = false;

        static void Main(string[] args)
        {
            string xsdFilePath = "./gpx.xsd";
            string xsdExtFilePath = "./GpxExtPlj.xsd";

            bool bGpxExt = false;
            bool bWaitConsole = false; 
                
            if (args.Length == 0)
            {
                Console.WriteLine("Bad argument.");
                Console.WriteLine("validateGpx.exe gpxfile [gpxext] [wait]");
                Console.WriteLine("Please provide an inputfile as the first argument and optionally true to halt the console");
                Console.WriteLine("");
                Console.WriteLine("Example:");
                Console.WriteLine(@"validateGpx.exe 110715_075330.gpx.xml [true]");
                bWaitConsole = true;
            }
            else
            {
                //parse arguments:
                if (args.Length > 1)
                {
                    Console.Write("Options: ");
                }
                for (int nArgs = 1; nArgs < args.Length; nArgs++)
                {
                    if (args[nArgs] == "gpxext")
                    {
                        Console.Write("[gpxext]");
                        bGpxExt = true;
                    }
                    if (args[nArgs] == "wait")
                    {
                        Console.Write("[wait]");
                        bWaitConsole = true;
                    }
                }
                if (args.Length > 1)
                {
                    Console.WriteLine();
                }

                //check schema files available
                bool bDoIt = true;

                if (!File.Exists(xsdFilePath))
                {
                    Console.WriteLine("validateGpx.exe");
                    Console.WriteLine("gpx.xsd does not exist in executable directory.");
                    bDoIt = false;
                }
                if (bGpxExt) 
                {
                    if (!File.Exists(xsdExtFilePath)) 
                    {
                        Console.WriteLine();
                        Console.WriteLine("validateGpx GpxExtPlj.xsd does not exist in executable directory.");
                        bDoIt = false;
                    }
                }
                else
                {
                    xsdExtFilePath = "";
                }

                //do it!
                if (bDoIt)
                {
                    xmlValidate xmlvalidate = new xmlValidate();
                    string xmlFilePath = args[0];
                    xmlvalidate.validate(xmlFilePath, xsdFilePath, xsdExtFilePath);
                }
            }
            
            if (bWaitConsole)
            {
                Console.WriteLine("Hit a key to close");
                Console.ReadLine();
            }
        }
    }

    class xmlValidate
    {
        int ErrorsCount = 0;

        public xmlValidate()
        {
            //empty constructor for now
        }

        public void validate(string xmlFile, string xsdFile, string xsdextFile)
        {
            Console.WriteLine("Validating xml:[" + xmlFile + "] with xsd:[" + xsdFile + "] xsdext:[" + xsdextFile + "]");
            
            XmlReaderSettings settings = new XmlReaderSettings();
            try
            {
                settings.Schemas.Add(null, xsdFile);
                if (xsdextFile != "")
                {
                    settings.Schemas.Add(null, xsdextFile);
                }
                settings.ValidationType = ValidationType.Schema;

                try
                {
                    XmlReader reader = XmlReader.Create(xmlFile, settings);
                    XmlDocument Odoc = new XmlDocument();
                    Odoc.Load(reader);
                    ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                    Odoc.Validate(eventHandler);
                }
                catch (XmlSchemaValidationException e)
                {
                    Console.WriteLine("Error: " + e.Message + "line:" + e.LineNumber + " pos:" + e.LinePosition);
                    ErrorsCount++;
                }
            }
            catch (XmlSchemaException e)
            {
                Console.WriteLine("Error: " + e.Message + "line:" + e.LineNumber + " pos:" + e.LinePosition);
                ErrorsCount++;
            }

            
            
            //Final feedback
            Console.WriteLine();
            if (ErrorsCount == 0)
            {
                Console.WriteLine("Validation completed successfully.");
            }
            else if (ErrorsCount == 1)
            {
                Console.WriteLine("Validation Failed: there was 1 error found");
            }
            else
            {
                Console.WriteLine("Validation Failed: there were " + ErrorsCount + " errors found");
            }
        }
        public void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            Console.WriteLine("ValidationEventHandler Error: " + args.Message);
            ErrorsCount++;
	    }
    }
}
