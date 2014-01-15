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
using System;
using System.Text; // StringBuilder
using System.Xml;  // XmlTextWriter
using System.Globalization; // CultureInfo
using System.IO;   // File Path StreamWriter //BinaryReader //File //FileMode

namespace sygiclog
{
/// <summary>
/// Description of ParseLogFile.
/// </summary>
public class SygicLogFile
{
	private LogParserSettings m_settings;
	
	private float ToFloat(byte[] input)
	{
		return BitConverter.ToSingle(input, 0);
	}
	
	private string readWideString(LogFile log, BinaryReader reader, ref int position, string logString)
	{
		int length = reader.ReadInt16();
		position += 2;
	
		byte[] stringArray = reader.ReadBytes(length * 2); //UTF16 wchar string
		
		var mystring = Encoding.Unicode.GetString(stringArray);
		log.log(logString + " p[" + position + "|" + position.ToString("X") + "]\t[" + mystring + "]");
		
		position += length * 2;
		
		return mystring;
	}
	
	public SygicLogFile(LogParserSettings settings)
	{
		m_settings = settings;
	}

	public bool Parse()
	{
		bool bReturn = true;
		int logfileVersion = -1;
		int nTrackPoints = 0; //number of trackpoints parsed
		int point_count = -1; //number of points in the file (version >= 5)
		DateTime logStartTime = DateTime.MinValue; 
		XmlTextWriter xmlWriter;
		DateTime now = DateTime.Now;
		
		try
		{
			//create a XML file (manually ;^) )
			xmlWriter = new XmlTextWriter(".\\" + m_settings.FileNameWithoutExtension + m_settings.XMLExtention, null);
			xmlWriter.WriteStartDocument(); // Opens the document
			xmlWriter.WriteComment("SygicTravelbookLog inputfile " + m_settings.InputFileName + " created at:" + now.ToString()); // Write comments
					
			//create with object from the xsd:
			//gpxType mygpx = new gpxType();

			LogFile logfile = new LogFile(m_settings.TxtLog, m_settings.FileNameWithoutExtension + ".txt");

			try
			{
				logfile.log(m_settings.ToComment());
				
				try
				{
					logStartTime = DateTime.ParseExact(m_settings.FileNameWithoutExtension, "yyMMdd_HHmmss", 
					                                   new CultureInfo("en-US"));
					
					logStartTime = logStartTime.AddHours(m_settings.TzcHours);
					logStartTime = logStartTime.AddMinutes(m_settings.TzcMinutes);
					logfile.log("Corrected Start Time = " + logStartTime );
				}
				catch(FormatException) // exception)
				{
					//it stays the MinValue
					//Console.WriteLine("no time in filename: " + exception);
					logfile.log("No filename based time");
				}
				
				using(BinaryReader reader = new BinaryReader(File.Open(m_settings.InputFileName, FileMode.Open)))
				{
					//def xml strings
					string sLat;
					string sLon;
					string sEle;
					string sTime;
					string sSpeed;
					string sCourse;

					byte byte1 = 0;
					byte byte2 = 0;

					long length = reader.BaseStream.Length;
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
					
					
					//DWORD---version----------------------------------4
					//int version = -1;
					//2 F R T -> version 2
					byte byte01 = reader.ReadByte();
					byte byte02 = reader.ReadByte();
					byte byte03 = reader.ReadByte();
					byte byte04 = reader.ReadByte();

					//log4Bytes(position, byte01, byte02, byte03, byte04);
					logfile.log4Bytes("version STR = ", position, byte01, byte02, byte03, byte04);
					position += 4;

					if((byte03 == 0x52) && (byte04 == 0x54))   // R T
					{
						//byte 3&4 OK: version 1-4

						if((byte02 == 0x4c) && (byte01 == 0x46))   //'L','F'
						{
							logfileVersion = 1; // FLRT
						}
						else if(byte02 == 0x46)     //'F'
						{
							//version 2 - 4
							if(byte01 == 0x32)   //'2'   // 2FRT
							{
								logfileVersion = 2;
							}
							else if(byte01 == 0x33)     //'3' // 3FRT
							{
								logfileVersion = 3;
							}
							else if(byte01 == 0x34)     //'4' // 4FRT
							{
								logfileVersion = 4;
							}
							else if(byte01 == 0x35)     //'5' // 5FRT
							{
								logfileVersion = 5;
							}
							else
							{
								//not a valid version
								string versionstring = String.Format("{0}{1}{2}{3}", (char)byte01,(char)byte02, (char)byte03, (char)byte04);
								logfile.log("unknown version str =\t " + versionstring);
							}
						}
						else
						{
							//not a valid version
							string versionstring = String.Format("{0}{1}{2}{3}", (char)byte01,(char)byte02, (char)byte03, (char)byte04);
							logfile.log("unknown version str =\t " + versionstring);
						}
					}
					else
					{
						//not a valid version
						string versionstring = String.Format("{0}{1}{2}{3}", (char)byte01,(char)byte02, (char)byte03, (char)byte04);
						logfile.log("unknown version str =\t " + versionstring);
					}

					logfile.log("version =\t " + logfileVersion);

					//INT----favorite-----------------------------4
					int favorite = reader.ReadInt32();
					logfile.log32("favorite = ", position, favorite);
					position += 4;

					//BYTE----log type-----------------------------1
					byte logType = reader.ReadByte();
					logfile.logByte("logType = ", position, logType);
					position += 1;

					//if (logfileVersion >= 3)
					if (logfileVersion >= 2)
					{
						//DWORD----log start time (version 3 or higher)-----------4
						int nLogStartTime = reader.ReadInt32();
	
						logfile.log32("logStartTime = ", position, nLogStartTime);
						position += 4;
					}

					//DWORD----log duration---------------------------------4
					int logDuration = reader.ReadInt32();

					logfile.log32("logDuration = ", position, logDuration);
					position += 4;

					//DWORD----log length-----------------------------------4
					int logLength = reader.ReadInt32();

					logfile.log32("logLength = ", position, logLength);
					position += 4;

					//DWORD----last mark distance---------------------------4
					//for testing skip when version 4
					if  (logfileVersion < 4)
					{
						int lastMarkDistance = reader.ReadInt32();

						logfile.log32("lastMarkDistance = ", position, lastMarkDistance);
						position += 4;
					}
					
					//String----start log point description-----------------x
					readWideString(logfile, reader, ref position, "startLogDescription");
					
					//String----end log point description--------x
					readWideString(logfile, reader, ref position, "endLogDescription");
					
					//String----star time (version 4 or higher)------------------x
					if(logfileVersion >= 4)
					{
						m_settings.LogStartTime  = readWideString(logfile, reader, ref position, "startimeDescription YYMMDD_HHMMSS_OOO");
						
						//set the starttime by this value (in case logfiles have other names):
						logStartTime = DateTime.ParseExact(m_settings.LogStartTime.Substring(0,13), "yyMMdd_HHmmss", new CultureInfo("en-US"));
					}
					
					//if(logfileVersion == 4)
					//if((logfileVersion >= 3) && (logfileVersion <= 4))
					if((logfileVersion >= 2) && (logfileVersion <= 4))
					{
						byte1 = reader.ReadByte();
						byte2 = reader.ReadByte();
						byte byte3 = reader.ReadByte();
						byte byte4 = reader.ReadByte();
						position += 4;
					}
					if(logfileVersion == 5)
					{
						//String----programmed destination description-----------------x
						readWideString(logfile, reader, ref position, "DestinationDescription");
												
						//long end_lon: I think this is the longitude of the place where the log ends.
    					//long end_lat: I think this is the latitude of the place where the log ends.
    					//long point_count: number of points in the log.
    					int end_lon = reader.ReadInt32();
						logfile.log32("end_lon = ", position, end_lon);
						double fLong = ((double)end_lon) / 100000.0;
						logfile.log("end_lon   = " + fLong);
						position += 4;
					
						int end_lat = reader.ReadInt32();
						logfile.log32("end_lat = ", position, end_lat);
						double fLat = ((double)end_lat) / 100000.0;
						logfile.log("end_lat   = " + fLat);
						position += 4;
					
						point_count = reader.ReadInt32();
						logfile.log32("point_count = ", position, point_count);
						position += 4;
					}
						
					logfile.log("\nData starts at " + position + " : " +   position.ToString("X") + "\n");

					//-------------------------------------------------
					//-------------------------------------------------
					bool bStop = false;
					
					if (logStartTime == DateTime.MinValue)
					{
						bStop = true;
						Console.WriteLine("No valid start time for the following file :( ");
					}
					
					DateTime trkptTime = logStartTime;
					
					//-------------------------------------------------
					//write the rest of the xml stuff including the settings
					//-------------------------------------------------
					//log the settings
					xmlWriter.WriteComment(m_settings.ToComment());
					
					//create <gpx version= creator= >
					xmlWriter.WriteStartElement("gpx", "");
					xmlWriter.WriteAttributeString("version", "1.1");
					xmlWriter.WriteAttributeString("creator", "SygicTravelbookLog");
					xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
					xmlWriter.WriteAttributeString("xmlns", "http://www.topografix.com/GPX/1/1");
		
					if(m_settings.GpxExt)
					{
						xmlWriter.WriteAttributeString("xmlns:gpxext", "http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1");
						xmlWriter.WriteAttributeString("xsi:schemaLocation", "http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1 http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1/GpxExtPlj.xsd http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
					}
					else
					{
						xmlWriter.WriteAttributeString("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
					}
					
					// <trk> <trkseg>
					xmlWriter.WriteStartElement("trk", "");
					xmlWriter.WriteElementString("name", m_settings.InputFileName);
					xmlWriter.WriteElementString("cmt", "created by SygicTravelbookLog");
		
					xmlWriter.WriteStartElement("trkseg", "");
					
					//=======================================================================
					// read data blocks
					//-------------------------------------------------
					logfile.log(">>>>  longitude\taltitude\televation\ttime\tspeed\n");

					int correctTimeOfset = 0;
					
					//start positions
					while(reader.BaseStream.Length > reader.BaseStream.Position)
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

						//LONG----longitude-------------------------------------------4
						int longitude = reader.ReadInt32();

						logfile.log32("\tlongitude = ", position, longitude);
						position += 4;

						double fLong = ((double)longitude) / 100000.0;

						if((fLong > 360.0f) || (fLong < -180.0f))
						{
							//wrong long found
							logfile.log("\tBREAK Long  = " + longitude);
							Console.WriteLine("\tBREAK Long  = " + longitude + " = " + fLong);
							break;
						}

						// old stop criterium (version <= 4)
						double fStop = Math.Abs(fLong);
						if(fStop * 100 < 1)   //(longitude < 0.01) //(longitude != 0)
						{
							bStop = true;
							logfile.log("\tEND BREAK Long<10 : " + longitude);
						}
						if (nTrackPoints == point_count)
						{
							bStop = true;
							logfile.log("\tEND BREAK last point reached : " + point_count);
						}
						if(!bStop)
						{
							//LONG----latitude----------------------------------------4
							int latitude = reader.ReadInt32();
							logfile.log32("\tlatitude = ", position, latitude);
							position += 4;

							double fLat = ((double)latitude) / 100000.0;

							if((fLat > 180.0f) || (fLat < -180.0f))
							{
								//wrong lat found
								logfile.log("\tBREAK Lat  = " + latitude);
								//Console.WriteLine("\tBREAK Lat  = " + latitude);
								break;
							}

							//LONG----elevation---------------------------------------4
							int elevation = reader.ReadInt32();
							logfile.log32("\televation = ", position, elevation);
							position += 4;

							//DWORD----time-------------------------------------------4
							int pointStepTime = reader.ReadInt32();
							logfile.log32("\tpointStepTime = ", position, pointStepTime);
							position += 4;

							if(correctTimeOfset == 0)
							{
								correctTimeOfset = pointStepTime;								
							}
							
							int timeDelta = pointStepTime - correctTimeOfset;
							//int num10 = (num9 / 0x3e8) / 0xe10;
							//int num11 = ((num9 / 0x3e8) / 60) % 60;
							//int num12 = ((num9 / 0x3e8) / 1) % 60;
							//int num13 = num9 % 0x3e8;
							DateTime time2 = trkptTime.AddMilliseconds((double)timeDelta);
							//TZC							
							//DateTime timeTZC = time2.AddHours(m_settings.TzcHours);
							//timeTZC = timeTZC.AddMinutes(m_settings.TzcMinutes);
							//log.log("\tTime = " + time2 + " TZC= " + timeTZC);
							logfile.log("\tTime = " + time2 );
							
							//----speed---------------------------------------------4
							byte[] aSpeed = reader.ReadBytes(4);
							float speed = ToFloat(aSpeed);

							logfile.log4Bytes("\tspeed = " + speed , position, aSpeed[0], aSpeed[1], aSpeed[2], aSpeed[3], false);
							position += 4;

							//BYTE----signal quality--------------------------------1
							int signalQuality = reader.ReadByte();

							logfile.logByte("\tsignalQuality = ", position, signalQuality);
							position++;

							//BYTE----speeding----(version 3 or higher)-------------1
							if(logfileVersion >= 3)
							{
								int speeding = reader.ReadByte();

								logfile.logByte("\tspeeding = ", position, speeding);
								position++;
							}

							//BYTE----gsm signal quality----------------------------1
							int gsmSignalQuality = reader.ReadByte();
							logfile.logByte("\tgsmSignalQuality = ", position, gsmSignalQuality);
							position++;

							//BYTE----internet signal quality-----------------------1
							int internetSignalQuality = reader.ReadByte();
							logfile.logByte("\tinternetSignalQuality = ", position, internetSignalQuality);
							position++;

							//BYTE----battery status--------------------------------1
							int batteryStatus = reader.ReadByte();

							logfile.logByte("\tbatteryStatus = ", position, batteryStatus);
							position++;

							//LOG----trackpoint=====================================
							++nTrackPoints;

							logfile.log(">>>>  " + ((double)longitude) / 100000.0
									+ "\t " + ((double)latitude) / 100000.0
									+ "\t " + elevation
									+ "\t " + time2
									+ "\t " + speed);

							//write the GPX XML
							//<trkpt lat="42.405660" lon="-71.098280">
							//<ele>9.750610</ele>
							//<time>2002-03-07T20:58:08Z</time>
							//</trkpt>
							sLon = (((double)longitude) / 100000.0).ToString(CultureInfo.InvariantCulture);
							sLat = (((double)latitude) / 100000.0).ToString(CultureInfo.InvariantCulture);
							sEle = (elevation).ToString(CultureInfo.InvariantCulture);
							
							sTime = time2.ToString("s"); //"s" means SortableDateTimePattern
							//sTime = timeTZC.ToString("s"); //"s" means SortableDateTimePattern
							//(based on ISO 8601), which is exactly what
							//Xml Schema DateTime format is based on.
							xmlWriter.WriteStartElement("trkpt", "");
							xmlWriter.WriteAttributeString("lat", sLat);
							xmlWriter.WriteAttributeString("lon", sLon);
							xmlWriter.WriteElementString("ele", sEle);
							xmlWriter.WriteElementString("time", sTime);

							//Add the optional data
							if(m_settings.GpxExt)
							{
								sSpeed = speed.ToString(CultureInfo.InvariantCulture);
								sCourse = "0.0";
								/*
								<extensions>
								    <gpxext:TrackPointExtension>
								        <gpxext:Speed>5.55</gpxext:Speed>
								        <gpxext:Course>30.0</gpxext:Course>
								    </gpxext:TrackPointExtension>
								</extensions>
								 */
								xmlWriter.WriteStartElement("extensions", "");

								xmlWriter.WriteStartElement("gpxext:TrackPointExtension", "");
								xmlWriter.WriteElementString("gpxext:Speed", sSpeed);
								xmlWriter.WriteElementString("gpxext:Course", sCourse);
								xmlWriter.WriteEndElement(); //gpxext:TrackPointExtension

								xmlWriter.WriteEndElement(); //extensions
							}

							xmlWriter.WriteEndElement(); //trkpt
						}
						else //bStop
						{
							logfile.log("\tEND");
							break;
						}
					}//while
				}//using
			}
			catch(Exception exception)
			{
				Console.WriteLine(exception);
				bReturn = false;
			}

			xmlWriter.WriteEndElement();   // trkseg
			xmlWriter.WriteEndElement();   // trk
			xmlWriter.WriteEndElement();   // gpx
			xmlWriter.WriteEndDocument();  // Ends the document.
			xmlWriter.Close();             // close xml writer

			logfile.log("wrote trkpt : " + nTrackPoints);
			logfile.log("The End!\n");

			//close log
			logfile.close();
		}
		catch(Exception exception)
		{
			bReturn = false;
			Console.WriteLine("create XmlTextWriter" + exception);
		}

		return bReturn;

	} //Parse
	
	public void validateFile()
	{	
		if(m_settings.Validate)
		{
			bool bDoIt = true; //only validate if all files are available

			string xsdFilePath = "./gpx.xsd";
			string xsdExtFilePath = "./GpxExtPlj.xsd";

			if(!File.Exists(xsdFilePath))
			{
				Console.WriteLine();
				Console.WriteLine("validateGpx gpx.xsd does not exist in executable directory.");
				bDoIt = false;
			}

			if(m_settings.GpxExt)
			{
				if(!File.Exists(xsdExtFilePath))
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

			if(bDoIt)   //only validate if all files are available
			{
				XmlValidate xmlvalidate = new XmlValidate();
				string xmlFilePath = ".\\" + m_settings.FileNameWithoutExtension + m_settings.XMLExtention;
				xmlvalidate.validate(xmlFilePath, xsdFilePath, xsdExtFilePath);
			}
		}
	}//ValidateFile
	
} //public class ParseLogFile
} // namespace sygiclog
