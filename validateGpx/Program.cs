//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
namespace ValidateGpx
{
    using System;
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    
    /// <summary>
    /// The main program for validating XML files to schemas
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main for the command line program
        /// </summary>
        /// <param name="args">the command line arguments</param>
        public static void Main(string[] args)
        {
            string xsdFilePath = "./gpx.xsd";
            string xsdExtFilePath = "./GpxExtPlj.xsd";

            bool useGpxExt = false;
            bool waitConsole = false;
            
            if (args.Length == 0)
            {
                Console.WriteLine("Bad argument.");
                Console.WriteLine("validateGpx.exe gpxfile [gpxext] [wait]");
                Console.WriteLine("Please provide an inputfile as the first argument and optionally true to halt the console");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Example:");
                Console.WriteLine(@"validateGpx.exe 110715_075330.gpx.xml [true]");
                waitConsole = true;
            }
            else
            {
                // parse arguments:
                if (args.Length > 1)
                {
                    Console.Write("Options: ");
                }
                
                for (int argsIdx = 1; argsIdx < args.Length; argsIdx++)
                {
                    if (args[argsIdx] == "gpxext")
                    {
                        Console.Write("[gpxext]");
                        useGpxExt = true;
                    }
                    else if (args[argsIdx] == "wait")
                    {
                        Console.Write("[wait]");
                        waitConsole = true;
                    }
                }
                
                if (args.Length > 1)
                {
                    Console.WriteLine();
                }

                // check schema files available
                bool doIt = true;

                if (!File.Exists(xsdFilePath))
                {
                    Console.WriteLine("validateGpx.exe");
                    Console.WriteLine("gpx.xsd does not exist in executable directory.");
                    doIt = false;
                }
                
                if (useGpxExt)
                {
                    if (!File.Exists(xsdExtFilePath))
                    {
                        Console.WriteLine();
                        Console.WriteLine("validateGpx GpxExtPlj.xsd does not exist in executable directory.");
                        doIt = false;
                    }
                }
                else
                {
                    xsdExtFilePath = string.Empty;
                }
                
                if (doIt)
                {
                    XmlValidate xmlvalidate = new XmlValidate();
                    string xmlFilePath = args[0];
                    xmlvalidate.Validate(xmlFilePath, xsdFilePath, xsdExtFilePath);
                }
            }
            
            if (waitConsole)
            {
                Console.WriteLine("Hit a key to close");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Helper class for doing validations (can be reused for any XML and schema set)
        /// </summary>
        private class XmlValidate
        {
            /// <summary>
            /// the number of found errors
            /// </summary>
            private int errorsCount = 0;

            /// <summary>
            /// Initializes a new instance of the <see cref="XmlValidate" /> class.
            /// The empty constructor
            /// </summary>
            public XmlValidate()
            {
                // empty constructor for now
            }

            /// <summary>
            /// Gets the error count value
            /// </summary>
            public int ErrorCount
            {
                get
                {
                    return this.errorsCount;
                }
            }
            
            /// <summary>
            /// validate the file with schemas
            /// </summary>
            /// <param name="xmlFile">the file to validate</param>
            /// <param name="xsdFile">the main schema file</param>
            /// <param name="xsdextFile">an optional additional schema file</param>
            public void Validate(string xmlFile, string xsdFile, string xsdextFile)
            {
                Console.WriteLine("Validating xml:[" + xmlFile + "] with xsd:[" + xsdFile + "] xsdext:[" + xsdextFile + "]");
                
                XmlReaderSettings settings = new XmlReaderSettings();
                try
                {
                    settings.Schemas.Add(null, xsdFile);
                    
                    // if (xsdextFile != string.Empty)
                    if (!string.IsNullOrEmpty(xsdextFile))
                    {
                        settings.Schemas.Add(null, xsdextFile);
                    }
                    
                    settings.ValidationType = ValidationType.Schema;

                    try
                    {
                        XmlReader reader = XmlReader.Create(xmlFile, settings);
                        XmlDocument rootdoc = new XmlDocument();
                        rootdoc.Load(reader);
                        ValidationEventHandler eventHandler = new ValidationEventHandler(this.ValidationEventHandler);

                        rootdoc.Validate(eventHandler);
                    }
                    catch (XmlSchemaValidationException e)
                    {
                        Console.WriteLine("Error: " + e.Message + "line:" + e.LineNumber + " pos:" + e.LinePosition);
                        this.errorsCount++;
                    }
                }
                catch (XmlSchemaException e)
                {
                    Console.WriteLine("Error: " + e.Message + "line:" + e.LineNumber + " pos:" + e.LinePosition);
                    this.errorsCount++;
                }

                // Final feedback
                Console.WriteLine();
                if (this.errorsCount == 0)
                {
                    Console.WriteLine("Validation completed successfully.");
                }
                else if (this.errorsCount == 1)
                {
                    Console.WriteLine("Validation Failed: there was 1 error found");
                }
                else
                {
                    Console.WriteLine("Validation Failed: there were " + this.errorsCount + " errors found");
                }
            }
            
            /// <summary>
            /// The needed  or used event handler
            /// </summary>
            /// <param name="sender">the sender</param>
            /// <param name="args">the arguments</param>
            public void ValidationEventHandler(object sender, ValidationEventArgs args)
            {
                Console.WriteLine("ValidationEventHandler Error: " + args.Message);
                this.errorsCount++;
            }
        } // class XmlValidate 
    } // public class Program
}
