//-----------------------------------------------------------------------
// <copyright file="Test1.cs" company="PLJ">
// Copyright (C) 2015, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
/*
 * Created by SharpDevelop.
 * User: Paul Janson
 * Date: 29-6-2012 / 31-12-2013 / 2-10-2014
 */
        // Usage : Add your test.
        // Assert.AreEqual( int expected, int actual, string message );
        // Assert.AreEqual( float expected, float actual, float tolerance, string message );
        // Assert.Fail("This test fails.");
        // Assert.Ignore( string message );

        // Assert.IsTrue( bool condition, string message );
        // Assert.IsFalse( bool condition, string message );
        // Assert.IsNull( object anObject, string message );
        // Assert.IsNotNull( object anObject, string message );
        
        // Assert.Throws( Type expectedExceptionType, TestSnippet code, string message );      
         
       
namespace TestSygiclog
{    
    using NUnit.Framework;
    using Sygiclog;   
  
     /// <summary>
    /// Containing the first set of tests
    /// </summary>
    [TestFixture]
    public class Support
    {
 		/// <summary>
        /// Validate the LogParserSettings
        /// </summary>
        [Test]
        public static void LogParserSettings()
        {
            // check the initialized settings
            LogParserSettings settings = new LogParserSettings();

            Assert.AreEqual(string.Empty, settings.AppTitle, " settings.AppTitle = \"\" | string.empty ");
            Assert.AreEqual(string.Empty, settings.InputFileName, " settings.InputFileName = \"\" | string.empty ");
            Assert.AreEqual(".gpx", settings.XmlExtension, " settings.XMLExtention = \".gpx\" ");
            Assert.AreEqual(false, settings.All, " settings.All = false");
            Assert.AreEqual(string.Empty, settings.Tzc, " settings.Tzc =  = \"\" | string.empty ");
            Assert.AreEqual(0, settings.TzcHours, " settings.TzcHours = 0");
            Assert.AreEqual(0, settings.TzcMinutes, " settings.TzcMinutes = 0");
            Assert.AreEqual(false, settings.Validate, " settings.Validate = false");
            Assert.AreEqual(false, settings.GpxExt, " settings.GpxExt = false");
            Assert.AreEqual(false, settings.WaitConsole, " settings.WaitConsole = false");
            Assert.AreEqual(false, settings.TxtLog, " settings.TxtLog = false");
            
            // settings All
            settings.All = true;
            Assert.AreEqual(true, settings.All, " settings.All = true");
            
            // settings Title
            settings.AppTitle = "testTitle";
            Assert.AreEqual("testTitle", settings.AppTitle, " settings.AppTitle = testTitle");
            
            // settings ToComment
            string testString = settings.ToComment();
            Assert.AreEqual("\nm_sAppTitle=[testTitle]\nm_sInputFileName=[]\nm_sXMLExtention=[.gpx]\nm_bAll=[True]\nm_sTzc=[] 0 : 0\nm_bValidate=[False]\nm_bGpxExt=[False]\nm_bWaitConsole=[False]\nm_bTxtlog=[False]\nm_sLogStartTime=[]\n", testString, " settings.ToComment()");
            //Assert.AreEqual("\nm_sAppTitle=[testTitle]\nm_sInputFileName=[]\nm_sXMLExtention=[.gpx]\nm_bAll=[True]\nm_bValidate=[False]\nm_bGpxExt=[False]\nm_bWaitConsole=[False]\nm_bTxtlog=[False]\nm_sLogStartTime=[]\n", testString, " settings.ToComment()");
        } // LogParserSettings
        
        /// <summary>
        /// Testing the properties of the Main <see cref="SygicTravelbookLog" /> class.
        /// </summary>
        [Test]
        public static void SygicTravelbookLog()
        {
            Assert.AreEqual("Sygiclog v1.5.0", Sygiclog.SygicTravelbookLog.SygiclogVersionString, " SygicTravelbookLog version sTitle");
            
            // TODO doUsage()
        }
        
        /// <summary>
        /// Testing the log file class
        /// </summary>
        [Test]
        public static void LogFile()
        {
            // check the initialized settings
            bool enabled = false;
            string logFileName = "noFile";
            
            LogFile logfile = new LogFile(enabled, logFileName);
            Assert.AreEqual(false, logfile.Enabled, "LogFile enabled False");
            
            // test logfile exist
            enabled = true;
            logFileName = "test1.logfile";
            LogFile logfile2 = new LogFile(enabled, logFileName);
            Assert.AreEqual(true, logfile2.Enabled, "LogFile enabled True");
            
            // test logfile exist is true
            
            // static int bytes2int32(int b1, int b2, int b3, int b4)
            Assert.AreEqual(0,        Sygiclog.LogFile.Bytes2Int32(0, 0, 0, 0), "bytes2int32(0,0,0,0)");
            Assert.AreEqual(1,        Sygiclog.LogFile.Bytes2Int32(1, 0, 0, 0), "bytes2int32(1,0,0,0)");
            Assert.AreEqual(256,      Sygiclog.LogFile.Bytes2Int32(0, 1, 0, 0), "bytes2int32(0,1,0,0)");
            Assert.AreEqual(65536,    Sygiclog.LogFile.Bytes2Int32(0, 0, 1, 0), "bytes2int32(0,0,1,0)");
            Assert.AreEqual(16777216, Sygiclog.LogFile.Bytes2Int32(0, 0, 0, 1), "bytes2int32(0,0,0,1)");
            Assert.AreEqual(16843009, Sygiclog.LogFile.Bytes2Int32(1, 1, 1, 1), "bytes2int32(1,1,1,1)");
            
            //LogByte
            string logMessage = "message:";
            int position = 1234;
            byte value = 192;  
            string result = logfile2.LogByte(logMessage, position, value);
            Assert.AreEqual("message:\t192\tp[1234|04d2]:C0", result, "logByte(..)");
                        
             //Log4Byte true
            position = 4321;
            byte byte1 = 65;
            byte byte2 = 67;
            byte byte3 = 69;
            byte byte4 = 71;
            //string logMessage, int position, byte byte1, byte byte2, byte byte3, byte byte4, bool isChar = true
            string result2 = logfile2.Log4Bytes(logMessage, position, byte1, byte2, byte3, byte4, true);
            Assert.AreEqual("message:\t1195721537\tp[4321|10e1]:41-43-45-47 | ACEG", result2, "log4Bytes(.., true)");

			string result3 = logfile2.Log4Bytes(logMessage, position, byte1, byte2, byte3, byte4, false);
            Assert.AreEqual("message:\t1195721537\tp[4321|10e1]:41-43-45-47", result3, "log4Bytes(.., false)");
	
            //Log32
            //(string logMessage, int position, int value32)
            logMessage = "message:";
            position = 1234;
            int value32 = 192192;  
            string result4 = logfile2.Log32(logMessage, position, value32);
            Assert.AreEqual("message:\t192192\tp[1234|04d2]:C0-EE-02-00", result4, "log32(..)");
                    
            logfile.Close();
            logfile2.Close();
        } // LogFile    	
    	
    }
    
    /// <summary>
    /// Containing the first set of tests
    /// </summary>
    [TestFixture]
    public class Test1
    {
        /// <summary>
        /// Testing version 2 binary log files
        /// </summary>
        [Test]
        public static void SygicLogFileV2()
        {
            //--------------------------------
            // version 2
            // 120528_114812.log  //iPhone
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\120528_114812.log";
                settings.TxtLog = true;
                
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse V2");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(2, validatelogfile.LogFileVersion, "version == 2");
                    Assert.AreEqual(761, validatelogfile.Trkpts, "trkpt == 761");

                    Assert.AreEqual("28-5-2012 11:48:12", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 28-5-2012 11:48:12
                    Assert.AreEqual("[CV-502  Cullera]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]	[CV-502  Cullera]
                    Assert.AreEqual("[Avenida de la Gola del Puchol  Valencia]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[59|3B]	[Avenida de la Gola del Puchol  Valencia]

                    //Assert.AreEqual("[]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //NO startimeDescription
                    Assert.AreEqual("28-5-2012 11:48:12", validatelogfile.TP1Time, "TP1 Time = "); // Time = 28-5-2012 11:48:12

                    Assert.AreEqual(-24582, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	-24582	p[141|008d]:FA-9F-FF-FF
                    Assert.AreEqual(3920774, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	3920774	p[145|0091]:86-D3-3B-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\120528_114812.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(761, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2012-05-28T11:48:12", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(-0.24582, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(39.20774, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
            
            // - - - - - - - - - - - - - - - -
            // 120530_090514.log  //iPhone
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\120530_090514.log";
                settings.TxtLog = true;
                
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(2, validatelogfile.LogFileVersion, "version == 2");
                    Assert.AreEqual(662, validatelogfile.Trkpts, "trkpt == 662");
                    
                    Assert.AreEqual("30-5-2012 9:05:14", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 30-5-2012 9:05:14
                    Assert.AreEqual("[Estación de Tamarit 28/53  Moncofa]", validatelogfile.StartLogDescription, "StartLogDescription == "); //startLogDescription p[27|1B]	[Estación de Tamarit 28/53  Moncofa]
                    Assert.AreEqual("[Avenida Mallorca 134/101  Nules]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[97|61]	[Avenida Mallorca 134/101  Nules]

                    //Assert.AreEqual("[]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //NO startimeDescription
                    Assert.AreEqual("30-5-2012 9:05:14", validatelogfile.TP1Time, "TP1 Time = "); // Time = 30-5-2012 9:05:14

                    Assert.AreEqual(-13911, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	-13911	p[163|00a3]:A9-C9-FF-FF
                    Assert.AreEqual(3978890, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	3978890	p[167|00a7]:8A-B6-3C-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\120530_090514.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(662, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2012-05-30T09:05:14", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(-0.13911, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(39.78890, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
            
            // - - - - - - - - - - - - - - - -
            // 110715_075330.log
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\110715_075330.log";
                settings.TxtLog = true;
                
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(2, validatelogfile.LogFileVersion, "version == 2");
                    Assert.AreEqual(8967, validatelogfile.Trkpts, "trkpt == 8967");

                    Assert.AreEqual("15-7-2011 7:53:30", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 15-7-2011 7:53:30
                    Assert.AreEqual("[Tongelresestraat 423/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]	[Tongelresestraat 423/-  Eindhoven]
                    Assert.AreEqual("[Grensstraat -/1  Veurne]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[95|5F]	[Grensstraat -/1  Veurne]
                
                    //Assert.AreEqual("[]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //NO startimeDescription
                    Assert.AreEqual("15-7-2011 7:53:30", validatelogfile.TP1Time, "TP1 Time = "); // Time = 15-7-2011 7:53:30

                    Assert.AreEqual(551317, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	551317	p[145|0091]:95-69-08-00
                    Assert.AreEqual(5144420, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5144420	p[149|0095]:64-7F-4E-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\110715_075330.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(8967, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2011-07-15T07:53:30", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(5.51317, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(51.44420, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }

            }
        } // SygicLogFileV2
        
        /// <summary>
        /// Testing V3 type binary input files
        /// </summary>
        [Test]
        public static void SygicLogFileV3()
        {
            //---------------------------------------------------------------------
            // version 3
            
            //--------------------------------
            // version 3 file
            // 120520_113917.log
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\120520_113917.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(3, validatelogfile.LogFileVersion, "version == 3");
                    Assert.AreEqual(1592, validatelogfile.Trkpts, "trkpt == 1592");

                    Assert.AreEqual("20-5-2012 11:39:17", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 20-5-2012 11:39:17
                    Assert.AreEqual("[Terno d'Isola]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]	[Terno d'Isola]
                    Assert.AreEqual("[Tangenziale Est  Vimercate]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[55|37]	[Tangenziale Est  Vimercate]

                    //Assert.AreEqual("[]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //NO startimeDescription
                    Assert.AreEqual("20-5-2012 11:39:17", validatelogfile.TP1Time, "TP1 Time = "); // Time = 20-5-2012 11:39:17

                    Assert.AreEqual(952659, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	952659	p[111|006f]:53-89-0E-00
                    Assert.AreEqual(4568400, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	4568400	p[115|0073]:50-B5-45-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\120520_113917.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(1592, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2012-05-20T11:39:17", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(9.52659, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(45.68400, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
            
            // - - - - - - - - - - - - - - - -
            // 120717_135755.log (E20 Malmo Sweden) (android 11.2.6)
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\120717_135755.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(3, validatelogfile.LogFileVersion, "version == 3");
                    Assert.AreEqual(58, validatelogfile.Trkpts, "trkpt == 58");

                    Assert.AreEqual("17-7-2012 13:57:55", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 17-7-2012 13:57:55
                    Assert.AreEqual("[E20  Malmö]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]	[E20  Malmö]
                    Assert.AreEqual("[E20  Malmö]", validatelogfile.EndLogDescription, "EndLogDescription == "); // startLogDescription p[27|1B]	[E20  Malmö]
                
                    //Assert.AreEqual("[]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //NO startimeDescription
                    Assert.AreEqual("17-7-2012 13:57:55", validatelogfile.TP1Time, "TP1 Time = "); // Time = 17-7-2012 13:57:55

                    Assert.AreEqual(1296869, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	1296869	p[73|0049]:E5-C9-13-00
                    Assert.AreEqual(5554679, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5554679	p[77|004d]:F7-C1-54-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\120717_135755.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(58, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2012-07-17T13:57:55", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(12.96869, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(55.54679, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
            
            // - - - - - - - - - - - - - - - -
            // 120413_074738.log
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\120413_074738.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(3, validatelogfile.LogFileVersion, "version == 3");
                    Assert.AreEqual(286, validatelogfile.Trkpts, "trkpt == 286");

                    Assert.AreEqual("13-4-2012 7:47:38", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 13-4-2012 7:47:38
                    Assert.AreEqual("[Collse Hoefdijk  Nuenen, Gerwen en Nederwetten]", validatelogfile.StartLogDescription, "StartLogDescription == "); //startLogDescription p[27|1B]	[Collse Hoefdijk  Nuenen, Gerwen en Nederwetten]
                    Assert.AreEqual("[N270  Helmond]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[121|79]	[N270  Helmond]

                    //Assert.AreEqual("[]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //NO startimeDescription
                    Assert.AreEqual("13-4-2012 7:47:38", validatelogfile.TP1Time, "TP1 Time = "); // Time = 13-4-2012 7:47:38

                    Assert.AreEqual(555930, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	555930	p[151|0097]:9A-7B-08-00
                    Assert.AreEqual(5146032, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5146032	p[155|009b]:B0-85-4E-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\120413_074738.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(286, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2012-04-13T07:47:38", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(5.55930, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(51.46032, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
        } // SygicLogFileV3
        
        /// <summary>
        /// Testing the version4 binary log files
        /// </summary>
        [Test]
        public static void SygicLogFileV4()
        {
            //---------------------------------------------------------------------
            // version 4 file (android, sygic 12.1.0)
            // 120801_083158.log (Denmark)
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\120801_083158.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(4, validatelogfile.LogFileVersion, "version == 4");
                    Assert.AreEqual(50, validatelogfile.Trkpts, "trkpt == 50");

                    Assert.AreEqual("1-8-2012 8:31:58", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 1-8-2012 8:31:58
                    Assert.AreEqual("[Sønderjyske Motorvej  Vojens]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Sønderjyske Motorvej  Vojens]
                    Assert.AreEqual("[Sønderjyske Motorvej  Vojens]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[81|51]	[Sønderjyske Motorvej  Vojens]

                    Assert.AreEqual("[120801_063158_120]", validatelogfile.StartTimeDescription, "Start Time Description == "); //startimeDescription YYMMDD_HHMMSS_OOO p[139|8B]	[120801_063158_120]
                    Assert.AreEqual("1-8-2012 8:31:58", validatelogfile.TP1Time, "TP1 Time = "); // Time = 1-8-2012 8:31:58

                    Assert.AreEqual(939019, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	939019	p[177|00b1]:0B-54-0E-00
                    Assert.AreEqual(5523712, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5523712	p[181|00b5]:00-49-54-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\120801_083158.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(50, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2012-08-01T08:31:58", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(9.39019, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(55.23712, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
            
            // - - - - - - - - - - - - - - - -
            // 120730_141948.log
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\120730_141948.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(4, validatelogfile.LogFileVersion, "version == 4");
                    Assert.AreEqual(1795, validatelogfile.Trkpts, "trkpt == 1795");

                    Assert.AreEqual("30-7-2012 14:19:48", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 30-7-2012 14:19:48
                    Assert.AreEqual("[Växjö]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Växjö]
                    Assert.AreEqual("[23  Grimslöv]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[35|23]	[23  Grimslöv]

                    Assert.AreEqual("[120730_121948_120]", validatelogfile.StartTimeDescription, "Start Time Description == "); //startimeDescription YYMMDD_HHMMSS_OOO p[61|3D]	[120730_121948_120]
                    Assert.AreEqual("30-7-2012 14:19:48", validatelogfile.TP1Time, "TP1 Time = "); // Time = 30-7-2012 14:19:48

                    Assert.AreEqual(1480059, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	1480059	p[99|0063]:7B-95-16-00
                    Assert.AreEqual(5688045, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5688045	p[103|0067]:ED-CA-56-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\120730_141948.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(1795, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2012-07-30T14:19:48", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(14.80059, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(56.88045, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
        } // SygicLogFileV4
        
        /// <summary>
        /// Testing the version5 binary log files
        /// </summary>
        [Test]
        public static void SygicLogFileV5()
        {
            
            //---------------------------------------------------------------------
            // version 5 ... android / 13.xx
            // 130729_091649.log (Denmark)
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\130729_091649.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(797, validatelogfile.Trkpts, "trkpt == 797");

                    Assert.AreEqual("29-7-2013 9:16:49", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 29-7-2013 9:16:49
                    Assert.AreEqual("[Tongelresestraat 423/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Tongelresestraat 423/-  Eindhoven]
                    Assert.AreEqual("[Middenweg 235/224  Venlo]", validatelogfile.EndLogDescription, "EndLogDescription == "); //endLogDescription p[91|5B]	[Middenweg 235/224  Venlo]

                    Assert.AreEqual("[130729_071649_120]", validatelogfile.StartTimeDescription, "Start Time Description == "); //startimeDescription YYMMDD_HHMMSS_OOO p[141|8D]	[130729_071649_120]
                    Assert.AreEqual("29-7-2013 9:16:49", validatelogfile.TP1Time, "TP1 Time = "); // Time = 29-7-2013 9:16:49

                    Assert.AreEqual(551283, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	551283	p[241|00f1]:73-69-08-00
                    Assert.AreEqual(5144429, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5144429	p[245|00f5]:6D-7F-4E-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\130729_091649.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(797, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2013-07-29T09:16:49", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(5.51283, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(51.44429, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
            
            // - - - - - - - - - - - - - - - -
            // 131231_164356.log  sygic 13.4.1 ermelo looweg
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\131231_164356.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(806, validatelogfile.Trkpts, "trkpt == 806");

                    Assert.AreEqual("31-12-2013 16:43:57", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 31-12-2013 16:43:56
                    Assert.AreEqual("[Kawoepersteeg 21/24  Ermelo]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Kawoepersteeg 21/24  Ermelo]
                    Assert.AreEqual("[Looweg 15/10  Ermelo]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[79|4F]	[Looweg 15/10  Ermelo]

                    Assert.AreEqual("[131231_154357_060]", validatelogfile.StartTimeDescription, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[121|79]	[131231_154357_060]
                    Assert.AreEqual("31-12-2013 16:43:57", validatelogfile.TP1Time, "TP1 Time = "); // Time = 31-12-2013 16:43:57

                    Assert.AreEqual(559696, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	559696	p[169|00a9]:50-8A-08-00
                    Assert.AreEqual(5230596, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5230596	p[173|00ad]:04-D0-4F-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\131231_164356.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(806, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2013-12-31T16:43:57", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(5.59696, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(52.30596, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
                       
            //- - - - -
            // 14.3.1 (2014-06-16) Galaxy Note 3; rondje wasven
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\140616_175053.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(559, validatelogfile.Trkpts, "trkpt == 559");
                    
                    Assert.AreEqual("16-6-2014 17:50:53", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time =
                    Assert.AreEqual("[Hofstraat 125/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]
                    Assert.AreEqual("[Tongelresestraat 425/-  Eindhoven]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[49|31]

					Assert.AreEqual("[140616_155053_120]", validatelogfile.StartTimeDescription, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[121|79]	[140616_155053_120]
                    Assert.AreEqual("16-6-2014 17:50:53", validatelogfile.TP1Time, "TP1 Time = "); // Time = 2014-06-16 17:50:53

                    Assert.AreEqual(551067, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	550383	p[191|00bf]:EF-65-08-00
                    Assert.AreEqual(5144561, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5144251	p[195|00c3]:BB-7E-4E-00                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\140616_175053.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(559, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2014-06-16T17:50:53", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(5.51067, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(51.44561, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
            
            //140922_071138.log  sygic 14.3.3 Insulindelaan  Eindhoven - Nieuwleusenerdijk  Zwolle
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\140922_071138.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(5336, validatelogfile.Trkpts, "trkpt == 5336");

                    Assert.AreEqual("22-9-2014 7:11:42", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 22-9-2014 7:11:38
                    Assert.AreEqual("[Insulindelaan  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Insulindelaan  Eindhoven]
                    Assert.AreEqual("[Nieuwleusenerdijk  Zwolle]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[73|49]	[Nieuwleusenerdijk  Zwolle]
             
                   //startimeDescription YYMMDD_HHMMSS_OOO p[125|7D]	[140922_051142_120]
                    Assert.AreEqual("[140922_051142_120]", validatelogfile.StartTimeDescription, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[125|7D]	[140922_051142_120]
                    Assert.AreEqual("22-9-2014 7:11:42", validatelogfile.TP1Time, "TP1 Time = "); // Time = 22-9-2014 7:11:42

                    Assert.AreEqual(550383, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	550383	p[191|00bf]:EF-65-08-00
                    Assert.AreEqual(5144251, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5144251	p[195|00c3]:BB-7E-4E-00                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\140922_071138.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(5336, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2014-09-22T07:11:42", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(5.50383, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(51.44251, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
            
            //140713_113941.log  sygic 14.? Route 88 1407/1406  Lakewood - Route 9 532/533  Ocean</name><LookAt><longitude>-74.153236</longitude><latitude>40.081551</latitude></LookAt>
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\140713_113941.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(1545, validatelogfile.Trkpts, "trkpt == 1545");

                    Assert.AreEqual("13-7-2014 11:39:41", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 13-9-2014 11:39:41
                    Assert.AreEqual("[Route 88 1407/1406  Lakewood]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Insulindelaan  Eindhoven]
                    Assert.AreEqual("[Route 9 532/533  Ocean]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[73|49]	[Nieuwleusenerdijk  Zwolle]

                    Assert.AreEqual("[140713_120341_-24]", validatelogfile.StartTimeDescription, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[127|7F]	[140713_120341_-24]
                    Assert.AreEqual("13-7-2014 11:39:41", validatelogfile.TP1Time, "TP1 Time = "); // Time = 13-7-2014 11:39:41
                    
                    Assert.AreEqual(-7415324, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	-7415324	p[219|00db]:E4-D9-8E-FF
                    Assert.AreEqual(4008155, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	4008155	p[223|00df]:DB-28-3D-00
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\140713_113941.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(1545, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2014-07-13T11:39:41", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(-74.15324, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(40.08155, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }


            //141227_191942.log  sygic 14.7.4 Skavska Sweden
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\141227_191942.log";
                settings.TxtLog = true;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(623, validatelogfile.Trkpts, "trkpt == 623");

                    Assert.AreEqual("27-12-2014 19:19:42", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 27-12-2014 19:19:42
                    Assert.AreEqual("[52  Nyköping]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[52  Nyköping]
                    Assert.AreEqual("[E4  Nyköping]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[73|49]	[E4  Nyköping]

                    Assert.AreEqual("[141227_181942_060]", validatelogfile.StartTimeDescription, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[127|7F]	[140713_120341_-24]
                    Assert.AreEqual("27-12-2014 19:19:42", validatelogfile.TP1Time, "TP1 Time = "); // Time = 13-7-2014 11:39:41
                    
                    Assert.AreEqual(1693714, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 1693714
                    Assert.AreEqual(5877017, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5877017
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\141227_191942.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(623, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2014-12-27T19:19:42", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(16.93714, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(58.77017, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            } //14.7.4
            
        } // SygicLogFileV5
    }
     /// </summary>
    [TestFixture]
    public class TZC
    {
        /// <summary>
        /// Testing the tzc time code correction with positive values
        /// </summary>
        [Test]
        public static void tzcTestsPositive()
        {
            // a [tzc1:30] positive time code correction
            // 11:39:41 + 1:30 = 13:09:41
            //140713_113941.log  sygic 14.* Route 88 1407/1406  Lakewood - Route 9 532/533  Ocean</name><LookAt><longitude>-74.153236</longitude><latitude>40.081551</latitude></LookAt>
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\140713_113941.log";
                settings.TxtLog = true;
                settings.Tzc = "1:00";
                settings.TzcHours = 1;
                settings.TzcMinutes = 30;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(1545, validatelogfile.Trkpts, "trkpt == 1545");

                    Assert.AreEqual("13-7-2014 13:09:41", validatelogfile.CorrectedStartTime, "Corrected Start Time == ");
                        // Corrected Start Time = 13-9-2014 13:09:41
                    Assert.AreEqual("[Route 88 1407/1406  Lakewood]", validatelogfile.StartLogDescription,
                        "StartLogDescription == "); // startLogDescription p[23|17]	[Insulindelaan  Eindhoven]
                    Assert.AreEqual("[Route 9 532/533  Ocean]", validatelogfile.EndLogDescription,
                        "EndLogDescription == "); // endLogDescription p[73|49]	[Nieuwleusenerdijk  Zwolle]

                    Assert.AreEqual("[140713_120341_-24]", validatelogfile.StartTimeDescription,
                        "Start Time Description == ");
                        // startimeDescription YYMMDD_HHMMSS_OOO p[127|7F]	[140713_120341_-24]
                    Assert.AreEqual("13-7-2014 13:09:41", validatelogfile.TP1Time, "TP1 Time = ");
                        // Time = 13-7-2014 13:09:41

                    Assert.AreEqual(-7415324, validatelogfile.TP1Longitude, "TP1 longitude = ");
                        //longitude = 	-7415324	p[219|00db]:E4-D9-8E-FF
                    Assert.AreEqual(4008155, validatelogfile.TP1Latitude, "TP1 latitude = ");
                        //latitude = 	4008155	p[223|00df]:DB-28-3D-00
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\140713_113941.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(1545, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2014-07-13T13:09:41", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(-74.15324, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(40.08155, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
        }
    //}
    // / </summary>
    //[TestFixture]
    //public class TZCNegative
    //{
        /// <summary>
        /// Testing the tzc time code correction with negative values
        /// </summary>
        [Test]
        public static void tzcTestsNegative()
        {
            // a [tzc-2:45] negative timecode correction
            // 11:39:41  -2:45 = 08:54:41
            //140713_113941.log  sygic 14.* Route 88 1407/1406  Lakewood - Route 9 532/533  Ocean</name><LookAt><longitude>-74.153236</longitude><latitude>40.081551</latitude></LookAt>
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\140713_113941.log";
                settings.TxtLog = true;
                settings.Tzc = "-2:45";
                settings.TzcHours = -2;
                settings.TzcMinutes = -45;
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(1545, validatelogfile.Trkpts, "trkpt == 1545");

                    Assert.AreEqual("13-7-2014 8:54:41", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 13-9-2014 11:39:41
                    Assert.AreEqual("[Route 88 1407/1406  Lakewood]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Insulindelaan  Eindhoven]
                    Assert.AreEqual("[Route 9 532/533  Ocean]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[73|49]	[Nieuwleusenerdijk  Zwolle]

                    Assert.AreEqual("[140713_120341_-24]", validatelogfile.StartTimeDescription, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[127|7F]	[140713_120341_-24]
                    Assert.AreEqual("13-7-2014 8:54:41", validatelogfile.TP1Time, "TP1 Time = "); // Time = 13-7-2014 11:39:41

                    Assert.AreEqual(-7415324, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	-7415324	p[219|00db]:E4-D9-8E-FF
                    Assert.AreEqual(4008155, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	4008155	p[223|00df]:DB-28-3D-00
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\140713_113941.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(false, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    Assert.AreEqual(1545, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2014-07-13T08:54:41", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(-74.15324, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(40.08155, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");

                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
        }
    }
     /// </summary>
    [TestFixture]
    public class GPXextention
    {
        /// <summary>
        /// Testing the GPXext exporting with the speed
        /// </summary>
        [Test]
        public static void GPXext()
        {
            //140713_113941.log  sygic 14.* Route 88 1407/1406  Lakewood - Route 9 532/533  Ocean</name><LookAt><longitude>-74.153236</longitude><latitude>40.081551</latitude></LookAt>
            {
                LogParserSettings settings = new LogParserSettings();
                settings.InputFileName = "..\\..\\..\\TESTlogs\\140713_113941.log";
                settings.TxtLog = true;
                settings.GpxExt = true; //m_bGpxExt=[True]
                SygicLogFile sygicLogFile = new SygicLogFile(settings);
                Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
                ValidateLogFile validatelogfile = new ValidateLogFile(settings.InputFileName);
                if (validatelogfile.Passed)
                {
                    // validate in the log file
                    Assert.AreEqual(5, validatelogfile.LogFileVersion, "version == 5");
                    Assert.AreEqual(1545, validatelogfile.Trkpts, "trkpt == 1545");

                    Assert.AreEqual("13-7-2014 11:39:41", validatelogfile.CorrectedStartTime, "Corrected Start Time == "); // Corrected Start Time = 13-9-2014 11:39:41
                    Assert.AreEqual("[Route 88 1407/1406  Lakewood]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Insulindelaan  Eindhoven]
                    Assert.AreEqual("[Route 9 532/533  Ocean]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[73|49]	[Nieuwleusenerdijk  Zwolle]

                    Assert.AreEqual("[140713_120341_-24]", validatelogfile.StartTimeDescription, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[127|7F]	[140713_120341_-24]
                    Assert.AreEqual("13-7-2014 11:39:41", validatelogfile.TP1Time, "TP1 Time = "); // Time = 13-7-2014 11:39:41

                    Assert.AreEqual(-7415324, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	-7415324	p[219|00db]:E4-D9-8E-FF
                    Assert.AreEqual(4008155, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	4008155	p[223|00df]:DB-28-3D-00
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate log error : ");
                }

                ValidateGpxFile validateGpxFile = new ValidateGpxFile(settings.InputFileName, settings.XmlExtension);
                if (validateGpxFile.Passed)
                {
                    Assert.AreEqual("..\\..\\..\\TESTlogs\\140713_113941.log", validateGpxFile.GetTrkName(), "Gpx trackname");
                    Assert.AreEqual(true, validateGpxFile.IsGpxExt(), "Is GpxExt");

                    //<trkpt lat="40.08155" lon="-74.15324">
				    //<ele>-33</ele>
				    //<time>2014-07-13T11:39:41</time>

                    Assert.AreEqual(77, validateGpxFile.FirstTrackpointSpeed(), "trackpoint speed");                    
                    Assert.AreEqual(1545, validateGpxFile.Trackpoints(), "tracks");
                    Assert.AreEqual("2014-07-13T11:39:41", validateGpxFile.FirstTrackpointTime(), "trackpoint time");
                    Assert.AreEqual(-74.15324, validateGpxFile.FirstTrackpointLongitude(), 0.0001, "trackpoint longitude");
                    Assert.AreEqual(40.08155, validateGpxFile.FirstTrackpointLatitude(), 0.0001, "trackpoint latitude");
                    
                }
                else
                {
                    Assert.AreEqual(string.Empty, validateGpxFile.Error, "validate gpx error : ");
                }
            }
        }
    }
}
