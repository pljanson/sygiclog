//-----------------------------------------------------------------------
// <copyright file="SygicTravelbookLog.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// previously Program.cs
// </copyright>
//-----------------------------------------------------------------------
/*
 * Program of Sygiclog reads a sygic log file and creates a gpx file from it.
 *
 * The Program part handles the commandline options and calls the library for
 * each file to be processed (by SygicLogFile.cs)
 *
 * Copyright (C) 2014, Paul Janson
 *
 * Version 1.5.0.beta
 *
 * History
 * 1.5.0beta updated for version 5 based on Lars's information; added version 2..5 unittests & test inputs
 * 1.4.0 added for all *.log files in this directory, refactor, TIMEZONE!
 * 1.3.2 start refactoring for unit tests
 * 1.3.1 fixed title string
 * 1.3 fixed negative longitude and stopping criteria
 * 1.2 added error handling
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
/* Thanks to Jiří Meluzín from "Sygic Aura Log Extractor" for the first code
 * http://auralogextractor.appspot.com/
 * And thanks to Marian Holler (Product manager) from sygic for posting information on their forum.
 */
/* Thanks to Lars Tiede for his version 5 code, that I borrowed.
 * http://fleckenzwerg2000.blogspot.no/2013/03/merging-sygic-travel-book-log-files-and.html
 */

namespace Sygiclog
{
using System;
using System.Globalization; // StringComparison
using System.IO; // File Path

/// <summary>
/// The container for the min and the arguments parsing
/// </summary>
public static class SygicTravelbookLog
{
    /// <summary>
    /// The version string
    /// </summary>
    public const string SygiclogVersionString = "Sygiclog v1.5.0beta";

    /// <summary>
    /// The command line manual
    /// </summary>
    private static void DoUsage()
    {
        // The MANUAL:
        Console.WriteLine(SygiclogVersionString);
        Console.WriteLine("The sygic travelbook log file 2 gpx convertor.");
        Console.WriteLine("Usage: sygiclog.exe [log_file|all] [xml] [validate] [gpxext] [wait] [log]");
        Console.WriteLine("\t [log_file] the log file");
        Console.WriteLine("\t [all]      for all log files in this directory");
        Console.WriteLine("\t [tzcSHH:MM]   timezone correction. S Sign, H Hours, M Minutes ");
        Console.WriteLine("\t [xml]      the gpx file will get the .gpx.xml extension");
        Console.WriteLine("\t [validate] validate the output file");
        Console.WriteLine("\t [gpxext]   use GPX extensions for additional data");
        Console.WriteLine("\t [wait]     wait for use input before closing console");
        Console.WriteLine("\t [log]      create a txt logfile");

        Console.WriteLine("Please provide a inputfile or \"all\" as the first argument.");
        Console.WriteLine();
        Console.WriteLine("Example:");
        Console.WriteLine(@"sygiclog.exe .\travelbook.log");
        Console.WriteLine();
        Console.WriteLine("TZC Time Zone Correction:"); // parse the TZC
        Console.WriteLine("CET = UTC+1:00 So the TZC is -1:00! So tzc-1");
        Console.WriteLine("CET+DST = UTC+2:00 So the TZC is -2:00! So tzc-2");
    }
    
    /// <summary>
    /// The main function running the command line argument parsing and starting the individual log file parsing. 
    /// </summary>
    /// <param name="args">the command line arguments</param>
    private static void Main(string[] args)
    {
        int filesParsed = 0;
        int filesGood = 0;
        int filesError = 0;
        DateTime now = DateTime.Now;

        LogParserSettings settings = new LogParserSettings();
        settings.AppTitle = SygiclogVersionString;

        // parse input arguments
        if (args.Length == 0)
        {
            DoUsage();
            settings.WaitConsole = true;
        }
        else
        {
            // the parsing of the settings
            // first must always be the file
            settings.InputFileName = args[0];

            if (args[0] == "all")
            {
                settings.All = true;
            }

            // fill checking
            if ((!File.Exists(settings.InputFileName)) && (settings.InputFileName != "all"))
            {
                // error
                Console.WriteLine("ERROR SygicTravelbookLog inputfile [" + settings.InputFileName + "] does NOT EXIST\n"); // Write comments
                DoUsage();
                settings.WaitConsole = true;
            }
            else
            {
                // rest
                Console.WriteLine(SygiclogVersionString + ", log 2 gpx");
                Console.WriteLine("SygicTravelbookLog inputfile " + settings.InputFileName + " created at:" + now.ToString()); // Write comments

                // collect options and log the options on the console
                if (args.Length > 1)
                {
                    Console.Write("Options: ");
                }

                for (int argIdx = 0; argIdx < args.Length; argIdx++)
                {
                    if (args[argIdx] == "all")
                    {
                        Console.Write("[all]  ");
                    }

                    if (args[argIdx].StartsWith("tzc", StringComparison.Ordinal))
                    {
                        Console.Write("[tzcSHH:MM]");
                        settings.Tzc = args[argIdx];

                        // parse the TZC
                        // CET = UTC+1:00 So the TZC is -1:00!
                        // CET+DST = UTC+2:00 So the TZC is -2:00!
                        try
                        {
                            string timeString = settings.Tzc.Remove(0, 3);
                            string[] split = timeString.Split(new char[] { ':' });
                            settings.TzcHours = Convert.ToInt32(split[0], CultureInfo.InvariantCulture);

                            if (split.Length >= 2)
                            {
                                int sign = 1;
                                int minutes = Convert.ToInt32(split[1], CultureInfo.InvariantCulture);

                                // carry over the sign
                                if (settings.TzcHours < 0)
                                {
                                    sign = -1;
                                }

                                settings.TzcMinutes = minutes * sign;
                            }
                        }                        
                        catch (Exception)
                        {
                            Console.WriteLine("\nIncorrect formatting of tzcSHH:MM [" + settings.Tzc + "]");
                            
                            Console.WriteLine("\nTZC Time Zone Correction:"); // parse the TZC
                            Console.WriteLine("CET = UTC+1:00 So the TZC is -1:00! So tzc-1");
                            Console.WriteLine("CET+DST = UTC+2:00 So the TZC is -2:00! So tzc-2\n");
                            
                            // Console.WriteLine(exception);
                        }
                    }

                    if (args[argIdx] == "xml")
                    {
                        Console.Write("[xml]");
                        settings.XmlExtension = ".gpx.xml";
                    }

                    if (args[argIdx] == "wait")
                    {
                        Console.Write("[wait]");
                        settings.WaitConsole = true;
                    }

                    if (args[argIdx] == "validate")
                    {
                        Console.Write("[validate]");
                        settings.Validate = true;
                    }

                    if (args[argIdx] == "gpxext")
                    {
                        Console.Write("[gpxext]");
                        settings.GpxExt = true;
                    }

                    if (args[argIdx] == "log")
                    {
                        Console.Write("[log]");
                        settings.TxtLog = true;
                    }
                } // for args

                if (args.Length > 1)
                {
                    Console.WriteLine();
                }

                if (settings.All)
                {
                    // do for all files
                    bool parseThisFile = false;

                    foreach (string str in Directory.EnumerateFiles(Directory.GetCurrentDirectory()))
                    {
                        if (Path.GetExtension(str) == ".log")
                        {
                            parseThisFile = true;
                            settings.InputFileName = str;
                        }
                        else
                        {
                            parseThisFile = false;
                        }

                        if (!parseThisFile)
                        {
                            continue;
                        }

                        // else
                        filesParsed++;
                        SygicLogFile sygicLogFile = new SygicLogFile(settings);
                        bool succes = sygicLogFile.Parse();

                        if (succes)
                        {
                            filesGood++;
                        }
                        else
                        {
                            filesError++;
                        }

                        sygicLogFile.ValidateFile();
                    }
                }
                else
                {
                    // do for one file
                    SygicLogFile sygicLogFile = new SygicLogFile(settings);
                    sygicLogFile.Parse();
                    sygicLogFile.ValidateFile();
                }
            } // else Do it
        }

        // the end, wait for user input on console (so it doesn't close on debug ;^)
        if (settings.WaitConsole)
        {
            if (settings.All)
            {
                Console.WriteLine("number of logs parsed = " + filesParsed +
                                  ",\t" + filesGood + " suceeded, \t"
                                  + filesError + " Failed.");
            }

            Console.WriteLine("Hit a key to close");
            Console.ReadLine();
        }
    } // Main
} // class SygicTravelbookLog
} // namespace

// extra info for now
// http://support.microsoft.com/kb/318504  HOW TO: Validate XML Fragments Against an XML Schema in Visual C#.NET
// http://www.liquid-technologies.com/xmldatabinding/xml-schema-to-cs.aspx  XML databinding

// Once you have your XSD file generated from your XML file, open the Visual Studio Command Prompt and type:
// xsd <YourFileName>.xsd  /c /l:cs
//
// This will generate a C# class in the same folder with the same name as your XSD file.
//
// Type xsd /? for more information about this tool or check the MSDN page.

// http://xsd2code.codeplex.com/releases/22222/download/156232  xsd2code (CCT)

/*
// Load the Schema Into Memory. The Error handler is also presented here.
StringReader sr = new StringReader(File.ReadAllText("schemafile.xsd"));
XmlSchema sch = XmlSchema.Read(sr,SchemaErrorsHandler);

// Create the Reader settings.
XmlReaderSettings settings = new XmlReaderSettings();
settings.Schemas.Add(sch);

// Create an XmlReader specifying the settings.
StringReader xmlData = new StringReader(File.ReadAllText("xmlfile.xml"));
XmlReader xr = XmlReader.Create(xmlData,settings);

// Use the Native .NET Serializer (probably u cud substitute the Xsd2Code serializer here.
XmlSerializer xs = new XmlSerializer(typeof(SerializableClass));
var data = xs.Deserialize(xr);
*/