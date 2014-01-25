//-----------------------------------------------------------------------
// <copyright file="SygicLogFile.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
/*
 * Created by SharpDevelop.
 * User: paul
 * Date: 8-8-2012
 * Time: 19:26
 *
 * SygicLogFile contains all the functionality
 * to parse and validate the data associate with a specific travelbook log file.
 *
 * It generates a GPX file, and optionally a log file.
 */
namespace Sygiclog
{
using System;
using System.Globalization; // CultureInfo
using System.IO;   // File Path StreamWriter //BinaryReader //File //FileMode
using System.Text; // StringBuilder
using System.Xml;  // XmlTextWriter

/// <summary>
/// ParseLogFile parses the SYGIC binary log file and generates a GPX file,
/// according to the settings.
/// </summary>
public class SygicLogFile
{
    /// <summary>
    /// the settings to use when parsing
    /// </summary>
    private LogParserSettings mySettings;
    
    /// <summary>
    /// The filename "root"
    /// </summary>
    private string fileNameWithoutExtension;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SygicLogFile" /> class.
    /// The constructor demands the settings.
    /// </summary>
    /// <param name="settings">the processing settings</param>
    public SygicLogFile(LogParserSettings settings)
    {
        this.mySettings = settings;
    }
    
    /// <summary>
    /// Convert the input (four)bytes to a float 
    /// </summary>
    /// <param name="input">the bytes that represent the float</param>
    /// <returns>to containing float</returns>
    public static float ByteArrayToFloat(byte[] input)
    {
        return BitConverter.ToSingle(input, 0);
    }
    
    /// <summary>
    /// The main parse function doing the processing of the binary file.
    /// </summary>
    /// <returns>if parsing was successful</returns>
    public bool Parse()
    {
        bool result = true;
        int logfileVersion = -1;
        int trackPoints = 0;  // number of trackpoints parsed
        int point_count = -1; // number of points in the file (version >= 5)
        DateTime logStartTime = DateTime.MinValue;
        XmlTextWriter xmlWriter;
        DateTime now = DateTime.Now;
        
        // create FileNameWithoutExtension
        this.fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.mySettings.InputFileName);
        
        try
        {
            // create a XML file (manually ;^) )
            xmlWriter = new XmlTextWriter(".\\" + this.fileNameWithoutExtension + this.mySettings.XmlExtension, null);
            xmlWriter.WriteStartDocument(); // Opens the document
            xmlWriter.WriteComment("SygicTravelbookLog inputfile " + this.mySettings.InputFileName + " created at:" + now.ToString());
                    
            // create with object from the xsd:
            // gpxType mygpx = new gpxType();
            LogFile logfile = new LogFile(this.mySettings.TxtLog, this.fileNameWithoutExtension + ".txt");

            try
            {
                logfile.Logline(this.mySettings.ToComment());
                
                try
                {
                    logStartTime = DateTime.ParseExact(this.fileNameWithoutExtension, "yyMMdd_HHmmss", new CultureInfo("en-US"));
                    
                    logStartTime = logStartTime.AddHours(this.mySettings.TzcHours);
                    logStartTime = logStartTime.AddMinutes(this.mySettings.TzcMinutes);
                    logfile.Logline("Corrected Start Time = " + logStartTime);
                }
                catch (FormatException)
                {
                    // it stays the MinValue
                    // Console.WriteLine("no time in filename: " + exception);
                    logfile.Logline("No filename based time");
                }
                
                using (BinaryReader reader = new BinaryReader(File.Open(this.mySettings.InputFileName, FileMode.Open)))
                {
                    // def xml strings
                    string latitudeString;
                    string longitudeString;
                    string elevationString;
                    string timeString;
                    string speedString;
                    string courseString;
                    
                    // long length = reader.BaseStream.Length;
                    int position = 0;

                    /*----header----------------------------------------
                     #define ATODWORD(a,b,c,d) ((a<<24) | (b<<16) | (c<<8) | d)
                     if ( version != ATODWORD('T','R','L','F') )
                     {
                       if ( version == ATODWORD('T','R','F','2') )
                         m_nVersion = 2;
                       else if ( version == ATODWORD('T','R','F','3') )
                         m_nVersion = 3;
                       else if ( version == ATODWORD('T','R','F','4') )
                         m_nVersion = 4;
                       else
                         return;
                      }
                    -----------------------------------------------------
                    DWORD   version                               4
                    INT     favorite                              4
                    BYTE    log type                              1
                    DWORD   log start time  version 3 or higher   4
                    DWORD   log duration                          4
                    DWORD   log length                            4
                    DWORD   last mark distance                    4
                    String  start log point description           x  stored strings as number of WCHAR followd by string
                    String  end log point description             x
                    String  star time   version 4 or higher       x
                    */

                    //-------------------------------------------------
                    // read header
                    //-------------------------------------------------
                    
                    // DWORD---version----------------------------------4
                    // int version = -1;
                    // 2 F R T -> version 2
                    byte byte01 = reader.ReadByte();
                    byte byte02 = reader.ReadByte();
                    byte byte03 = reader.ReadByte();
                    byte byte04 = reader.ReadByte();

                    // log4Bytes(position, byte01, byte02, byte03, byte04);
                    logfile.Log4Bytes("version STR = ", position, byte01, byte02, byte03, byte04);
                    position += 4;
                    
                    if ((byte03 == 0x52) && (byte04 == 0x54))
                    {
                        // R T
                        // byte 3&4 OK: version 1-4                        
                        if ((byte02 == 0x4c) && (byte01 == 0x46))
                        {
                            // 'L','F'
                            logfileVersion = 1; // FLRT
                        }
                        else if (byte02 == 0x46)
                        {
                            // 'F'
                            // version 2 - 4
                            if (byte01 == 0x32)
                            {
                                // '2'   // 2FRT
                                logfileVersion = 2;
                            }
                            else if (byte01 == 0x33)
                            {
                                // '3' // 3FRT
                                logfileVersion = 3;
                            }
                            else if (byte01 == 0x34)
                            {
                                // '4' // 4FRT
                                logfileVersion = 4;
                            }
                            else if (byte01 == 0x35)
                            {
                                // '5' // 5FRT
                                logfileVersion = 5;
                            }
                            else
                            {
                                // not a valid version
                                string versionstring = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", (char)byte01, (char)byte02, (char)byte03, (char)byte04);
                                logfile.Logline("unknown version str =\t " + versionstring); 
                            }
                        }
                        else
                        {
                            // not a valid version
                            string versionstring = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", (char)byte01, (char)byte02, (char)byte03, (char)byte04);
                            logfile.Logline("unknown version str =\t " + versionstring);
                        }
                    }
                    else
                    {
                        // not a valid version
                        string versionstring = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", (char)byte01, (char)byte02, (char)byte03, (char)byte04);
                        logfile.Logline("unknown version str =\t " + versionstring);
                    }

                    logfile.Logline("version =\t " + logfileVersion);

                    // INT----favorite-----------------------------4
                    int favorite = reader.ReadInt32();
                    logfile.Log32("favorite = ", position, favorite);
                    position += 4;

                    // BYTE----log type-----------------------------1
                    byte logType = reader.ReadByte();
                    logfile.LogByte("logType = ", position, logType);
                    position += 1;

                    // if (logfileVersion >= 3)
                    if (logfileVersion >= 2)
                    {
                        // DWORD----log start time (version 3 or higher)-----------4
                        int readStartTime = reader.ReadInt32();
    
                        logfile.Log32("logStartTime = ", position, readStartTime);
                        position += 4;
                    }

                    // DWORD----log duration---------------------------------4
                    int logDuration = reader.ReadInt32();

                    logfile.Log32("logDuration = ", position, logDuration);
                    position += 4;

                    // DWORD----log length-----------------------------------4
                    int logLength = reader.ReadInt32();

                    logfile.Log32("logLength = ", position, logLength);
                    position += 4;

                    // DWORD----last mark distance---------------------------4
                    // for testing skip when version 4
                    if (logfileVersion < 4)
                    {
                        int lastMarkDistance = reader.ReadInt32();

                        logfile.Log32("lastMarkDistance = ", position, lastMarkDistance);
                        position += 4;
                    }
                    
                    // String----start log point description-----------------x
                    this.ReadWideString(logfile, reader, ref position, "startLogDescription");
                    
                    // String----end log point description--------x
                    this.ReadWideString(logfile, reader, ref position, "endLogDescription");
                    
                    // String----star time (version 4 or higher)------------------x
                    if (logfileVersion >= 4)
                    {
                        this.mySettings.LogStartTime  = this.ReadWideString(logfile, reader, ref position, "startimeDescription YYMMDD_HHMMSS_OOO");
                        
                        // set the starttime by this value (in case logfiles have other names):
                        logStartTime = DateTime.ParseExact(this.mySettings.LogStartTime.Substring(0, 13), "yyMMdd_HHmmss", new CultureInfo("en-US"));
                    }
                                        
                    if ((logfileVersion >= 2) && (logfileVersion <= 4))
                    {
                        // What would be contained as 4 bytes in this locatoin?                                                
                        reader.ReadByte(); // byte1
                        reader.ReadByte(); // byte2
                        reader.ReadByte(); // byte3
                        reader.ReadByte(); // byte4
                        position += 4;
                    }
                    
                    if (logfileVersion == 5)
                    {
                        // String----programmed destination description-----------------x
                        this.ReadWideString(logfile, reader, ref position, "DestinationDescription");
                                                
                        // long end_lon: I think this is the longitude of the place where the log ends.
                        // long end_lat: I think this is the latitude of the place where the log ends.
                        // long point_count: number of points in the log.
                        int end_lon = reader.ReadInt32();
                        logfile.Log32("end_lon = ", position, end_lon);
                        double longitude = ((double)end_lon) / 100000.0;
                        logfile.Logline("end_lon   = " + longitude);
                        position += 4;
                    
                        int end_lat = reader.ReadInt32();
                        logfile.Log32("end_lat = ", position, end_lat);
                        double latitude = ((double)end_lat) / 100000.0;
                        logfile.Logline("end_lat   = " + latitude);
                        position += 4;
                    
                        point_count = reader.ReadInt32();
                        logfile.Log32("point_count = ", position, point_count);
                        position += 4;
                    }
                        
                    logfile.Logline("\nData starts at " + position + " : " + position.ToString("X", CultureInfo.InvariantCulture) + "\n");

                    //-------------------------------------------------
                    //-------------------------------------------------
                    bool needToStop = false;
                    
                    if (logStartTime == DateTime.MinValue)
                    {
                        needToStop = true;
                        Console.WriteLine("No valid start time for the following file :( ");
                    }
                    
                    DateTime trkptTime = logStartTime;
                    
                    //-------------------------------------------------
                    // write the rest of the xml stuff including the settings
                    //-------------------------------------------------
                    // log the settings
                    xmlWriter.WriteComment(this.mySettings.ToComment());
                    
                    // create <gpx version= creator= >
                    xmlWriter.WriteStartElement("gpx", string.Empty);
                    xmlWriter.WriteAttributeString("version", "1.1");
                    xmlWriter.WriteAttributeString("creator", "SygicTravelbookLog");
                    xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    xmlWriter.WriteAttributeString("xmlns", "http://www.topografix.com/GPX/1/1");
        
                    if (this.mySettings.GpxExt)
                    {
                        xmlWriter.WriteAttributeString("xmlns:gpxext", "http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1");
                        xmlWriter.WriteAttributeString("xsi:schemaLocation", "http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1 http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1/GpxExtPlj.xsd http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
                    }
                    else
                    {
                        xmlWriter.WriteAttributeString("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
                    }
                    
                    // <trk> <trkseg>
                    xmlWriter.WriteStartElement("trk", string.Empty);
                    xmlWriter.WriteElementString("name", this.mySettings.InputFileName);
                    xmlWriter.WriteElementString("cmt", "created by SygicTravelbookLog");
        
                    xmlWriter.WriteStartElement("trkseg", string.Empty);
                    
                    // =======================================================================
                    // read data blocks
                    //-------------------------------------------------
                    logfile.Logline(">>>>  longitude\taltitude\televation\ttime\tspeed\n");

                    int correctTimeOfset = 0;
                    
                    // start positions
                    while (reader.BaseStream.Length > reader.BaseStream.Position)
                    {
                        /*
                        DWORD   point count------------------------------------------
                          LONG  longitude                                           4
                          LONG  latitude                                            4
                          LONG  altitude                                            4
                          DWORD time                                                4
                          FLOAT speed                                               4
                          BYTE  signal quality                                      1
                          BYTE  speeding        version 3 or higher                 1
                          BYTE  gsm signal quality      version 1 or higher         1
                          BYTE  internet signal quality     version 1 or higher     1
                          BYTE  battery status      version 1 or higher             1
                        DWORD   mark count-------------------------------------------
                          LONG  longitude                                           4
                          LONG  latitude                                            4
                          DWORD time                                                2
                          BYTE  signal quality                                      1
                          BYTE  type                                                1
                          DWORD lap time                                            2
                        */

                        // LONG----longitude-------------------------------------------4
                        int readLongitude = reader.ReadInt32();

                        logfile.Log32("\tlongitude = ", position, readLongitude);
                        position += 4;

                        double longitude = ((double)readLongitude) / 100000.0;

                        if ((longitude > 360.0f) || (longitude < -180.0f))
                        {
                            // wrong long found
                            logfile.Logline("\tBREAK Long  = " + readLongitude);
                            Console.WriteLine("\tBREAK Long  = " + readLongitude + " = " + longitude);
                            break;
                        }

                        // old stop criterium (version <= 4)
                        double stopLong = Math.Abs(longitude);
                        if (stopLong * 100 < 1)   
                        {
                            // (longitude < 0.01) //(longitude != 0)
                            needToStop = true;
                            logfile.Logline("\tEND BREAK Long<10 : " + readLongitude);
                        }
                        
                        if (trackPoints == point_count)
                        {
                            needToStop = true;
                            logfile.Logline("\tEND BREAK last point reached : " + point_count);
                        }
                        
                        if (!needToStop)
                        {
                            // LONG----latitude----------------------------------------4
                            int readLatitude = reader.ReadInt32();
                            logfile.Log32("\tlatitude = ", position, readLatitude);
                            position += 4;

                            double latitude = ((double)readLatitude) / 100000.0;

                            if ((latitude > 180.0f) || (latitude < -180.0f))
                            {
                                // wrong lat found
                                logfile.Logline("\tBREAK Lat  = " + readLatitude);
                                
                                // Console.WriteLine("\tBREAK Lat  = " + latitude);
                                break;
                            }

                            // LONG----elevation---------------------------------------4
                            int elevation = reader.ReadInt32();
                            logfile.Log32("\televation = ", position, elevation);
                            position += 4;

                            // DWORD----time-------------------------------------------4
                            int pointStepTime = reader.ReadInt32();
                            logfile.Log32("\tpointStepTime = ", position, pointStepTime);
                            position += 4;

                            if (correctTimeOfset == 0)
                            {
                                correctTimeOfset = pointStepTime;
                            }
                            
                            int timeDelta = pointStepTime - correctTimeOfset;
                            
                            // int num10 = (num9 / 0x3e8) / 0xe10;
                            // int num11 = ((num9 / 0x3e8) / 60) % 60;
                            // int num12 = ((num9 / 0x3e8) / 1) % 60;
                            // int num13 = num9 % 0x3e8;
                            DateTime time2 = trkptTime.AddMilliseconds((double)timeDelta);
                            
                            // TZC
                            // DateTime timeTZC = time2.AddHours(m_settings.TzcHours);
                            // timeTZC = timeTZC.AddMinutes(m_settings.TzcMinutes);
                            // log.log("\tTime = " + time2 + " TZC= " + timeTZC);
                            logfile.Logline("\tTime = " + time2);
                            
                            //----speed---------------------------------------------4
                            byte[] speedArray = reader.ReadBytes(4);
                            float speed = ByteArrayToFloat(speedArray);

                            logfile.Log4Bytes("\tspeed = " + speed, position, speedArray[0], speedArray[1], speedArray[2], speedArray[3], false);
                            position += 4;

                            // BYTE----signal quality--------------------------------1
                            byte signalQuality = reader.ReadByte();

                            logfile.LogByte("\tsignalQuality = ", position, signalQuality);
                            position++;

                            // BYTE----speeding----(version 3 or higher)-------------1
                            if (logfileVersion >= 3)
                            {
                                byte speeding = reader.ReadByte();

                                logfile.LogByte("\tspeeding = ", position, speeding);
                                position++;
                            }

                            // BYTE----gsm signal quality----------------------------1
                            byte gsmSignalQuality = reader.ReadByte();
                            logfile.LogByte("\tgsmSignalQuality = ", position, gsmSignalQuality);
                            position++;

                            // BYTE----internet signal quality-----------------------1
                            byte internetSignalQuality = reader.ReadByte();
                            logfile.LogByte("\tinternetSignalQuality = ", position, internetSignalQuality);
                            position++;

                            // BYTE----battery status--------------------------------1
                            byte batteryStatus = reader.ReadByte();

                            logfile.LogByte("\tbatteryStatus = ", position, batteryStatus);
                            position++;

                            // LOG----trackpoint=====================================
                            ++trackPoints;

                            logfile.Logline(
                                   ">>>>  " + (((double)readLongitude) / 100000.0)
                                    + "\t " + (((double)readLatitude) / 100000.0)
                                    + "\t " + elevation
                                    + "\t " + time2
                                    + "\t " + speed);

                            // write the GPX XML
                            // <trkpt lat="42.405660" lon="-71.098280">
                            // <ele>9.750610</ele>
                            // <time>2002-03-07T20:58:08Z</time>
                            // </trkpt>
                            longitudeString = (((double)readLongitude) / 100000.0).ToString(CultureInfo.InvariantCulture);
                            latitudeString = (((double)readLatitude) / 100000.0).ToString(CultureInfo.InvariantCulture);
                            elevationString = elevation.ToString(CultureInfo.InvariantCulture);
                            
                            timeString = time2.ToString("s", CultureInfo.InvariantCulture); // "s" means SortableDateTimePattern
                            // sTime = timeTZC.ToString("s"); // "s" means SortableDateTimePattern
                            // (based on ISO 8601), which is exactly what
                            // Xml Schema DateTime format is based on.
                            xmlWriter.WriteStartElement("trkpt", string.Empty);
                            xmlWriter.WriteAttributeString("lat", latitudeString);
                            xmlWriter.WriteAttributeString("lon", longitudeString);
                            xmlWriter.WriteElementString("ele", elevationString);
                            xmlWriter.WriteElementString("time", timeString);

                            // Add the optional data
                            if (this.mySettings.GpxExt)
                            {
                                speedString = speed.ToString(CultureInfo.InvariantCulture);
                                courseString = "0.0";
                                /*
                                <extensions>
                                    <gpxext:TrackPointExtension>
                                        <gpxext:Speed>5.55</gpxext:Speed>
                                        <gpxext:Course>30.0</gpxext:Course>
                                    </gpxext:TrackPointExtension>
                                </extensions>
                                 */
                                xmlWriter.WriteStartElement("extensions", string.Empty);

                                xmlWriter.WriteStartElement("gpxext:TrackPointExtension", string.Empty);
                                xmlWriter.WriteElementString("gpxext:Speed", speedString);
                                xmlWriter.WriteElementString("gpxext:Course", courseString);
                                xmlWriter.WriteEndElement(); // gpxext:TrackPointExtension

                                xmlWriter.WriteEndElement(); // extensions
                            }

                            xmlWriter.WriteEndElement(); // trkpt
                        }
                        else
                        {
                            // needToStop
                            logfile.Logline("\tEND");
                            break;
                        }
                    } // while
                } // using
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                result = false;
            }

            xmlWriter.WriteEndElement();   // trkseg
            xmlWriter.WriteEndElement();   // trk
            xmlWriter.WriteEndElement();   // gpx
            xmlWriter.WriteEndDocument();  // Ends the document.
            xmlWriter.Close();             // close xml writer

            logfile.Logline("wrote trkpt : " + trackPoints);
            logfile.Logline("The End!\n");

            // close log
            logfile.Close();
        }
        catch (Exception exception)
        {
            result = false;
            Console.WriteLine("create XmlTextWriter" + exception);
        }

        return result;
    } // Parse
    
    /// <summary>
    /// Validate the generated GPX file according to the xml schemas. 
    /// This method can also be called externally.
    /// </summary>
    public void ValidateFile()
    {
        if (this.mySettings.Validate)
        {
            bool doIt = true; // only validate if all files are available

            string xsdFilePath = "./gpx.xsd";
            string xsdExtFilePath = "./GpxExtPlj.xsd";

            if (!File.Exists(xsdFilePath))
            {
                Console.WriteLine();
                Console.WriteLine("validateGpx gpx.xsd does not exist in executable directory.");
                doIt = false;
            }

            if (this.mySettings.GpxExt)
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
                // only validate if all files are available
                XmlValidate xmlvalidate = new XmlValidate();
                string xmlFilePath = ".\\" + this.fileNameWithoutExtension + this.mySettings.XmlExtension;
                xmlvalidate.Validate(xmlFilePath, xsdFilePath, xsdExtFilePath);
            }
        }
    } // ValidateFile
        
    /// <summary>
    /// Helper function to read the two byte integer, and log it to the log file 
    /// </summary>
    /// <param name="log">the log file to log to</param>
    /// <param name="reader">the reader to read from</param>
    /// <param name="position">the position within the input file</param>
    /// <param name="logString">additional log string</param>
    /// <returns>the constructed string</returns>
    private string ReadWideString(LogFile log, BinaryReader reader, ref int position, string logString)
    {
        int length = reader.ReadInt16();
        position += 2;
    
        byte[] stringArray = reader.ReadBytes(length * 2); // UTF16 wchar string
        
        var mystring = Encoding.Unicode.GetString(stringArray);
        log.Logline(logString + " p[" + position + "|" + position.ToString("X") + "]\t[" + mystring + "]");
        
        position += length * 2;
        
        return mystring;
    }   
} // public class ParseLogFile
} // namespace Sygiclog
