//-----------------------------------------------------------------------
// <copyright file="Test1.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
/*
 * Created by SharpDevelop.
 * User: paul
 * Date: 29-6-2012 / 31-12-2013
 * Time: 14:17
 */

namespace TestSygiclog
{    
    using System;
    using System.Globalization; // Culture
    using System.IO; // File Path
    using NUnit.Framework;
    using Sygiclog;   
  
    /// <summary>
    /// Containing the first set of tests
    /// </summary>
    [TestFixture]
    public class Test1
    {
        // Usage : Add your test.
        // Assert.AreEqual( int expected, int actual, string message );
        // Assert.AreEqual( float expected, float actual, float tolerance, string message );
        // Assert.Fail("This test fails.");
        // Assert.Ignore( string message );

        // Assert.IsTrue( bool condition, string message );
        // Assert.IsFalse( bool condition, string message );
        // Assert.IsNull( object anObject, string message );
        // Assert.IsNotNull( object anObject, string message );
        
        //// Assert.Throws( Type expectedExceptionType, TestSnippet code, string message );
                        
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
            //Assert.AreEqual(string.Empty, settings.Tzc, " settings.Tzc =  = \"\" | string.empty ");
            //Assert.AreEqual(0, settings.TzcHours, " settings.TzcHours = 0");
            //Assert.AreEqual(0, settings.TzcMinutes, " settings.TzcMinutes = 0");
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
            //Assert.AreEqual("\nm_sAppTitle=[testTitle]\nm_sInputFileName=[]\nm_sXMLExtention=[.gpx]\nm_bAll=[True]\nm_sTzc=[] 0 : 0\nm_bValidate=[False]\nm_bGpxExt=[False]\nm_bWaitConsole=[False]\nm_bTxtlog=[False]\nm_sLogStartTime=[]\n", testString, " settings.ToComment()");
            Assert.AreEqual("\nm_sAppTitle=[testTitle]\nm_sInputFileName=[]\nm_sXMLExtention=[.gpx]\nm_bAll=[True]\nm_bValidate=[False]\nm_bGpxExt=[False]\nm_bWaitConsole=[False]\nm_bTxtlog=[False]\nm_sLogStartTime=[]\n", testString, " settings.ToComment()");
        } // LogParserSettings
        
        /// <summary>
        /// Testing the properties of the Main <see cref="SygicTravelbookLog" /> class.
        /// </summary>
        [Test]
        public static void SygicTravelbookLog()
        {
            Assert.AreEqual("Sygiclog v1.5.0rc2", Sygiclog.SygicTravelbookLog.SygiclogVersionString, " SygicTravelbookLog version sTitle");
            
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
            
            // test logfile exist ois false
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
            
            logfile.Close();
            logfile2.Close();
        } // LogFile
        
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

                    Assert.AreEqual("28-5-2012 11:48:12", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 28-5-2012 11:48:12
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
                    
                    Assert.AreEqual("30-5-2012 9:05:14", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 30-5-2012 9:05:14
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

                    Assert.AreEqual("15-7-2011 7:53:30", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 15-7-2011 7:53:30
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

                    Assert.AreEqual("20-5-2012 11:39:17", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 20-5-2012 11:39:17
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

                    Assert.AreEqual("17-7-2012 13:57:55", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 17-7-2012 13:57:55
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

                    Assert.AreEqual("13-4-2012 7:47:38", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 13-4-2012 7:47:38
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

                    Assert.AreEqual("1-8-2012 8:31:58", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 1-8-2012 8:31:58
                    Assert.AreEqual("[Sønderjyske Motorvej  Vojens]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Sønderjyske Motorvej  Vojens]
                    Assert.AreEqual("[Sønderjyske Motorvej  Vojens]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[81|51]	[Sønderjyske Motorvej  Vojens]

                    Assert.AreEqual("[120801_063158_120]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //startimeDescription YYMMDD_HHMMSS_OOO p[139|8B]	[120801_063158_120]
                    Assert.AreEqual("1-8-2012 8:31:58", validatelogfile.TP1Time, "TP1 Time = "); // Time = 1-8-2012 8:31:58

                    Assert.AreEqual(939019, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	939019	p[177|00b1]:0B-54-0E-00
                    Assert.AreEqual(5523712, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5523712	p[181|00b5]:00-49-54-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
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

                    Assert.AreEqual("30-7-2012 14:19:48", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 30-7-2012 14:19:48
                    Assert.AreEqual("[Växjö]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Växjö]
                    Assert.AreEqual("[23  Grimslöv]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[35|23]	[23  Grimslöv]

                    Assert.AreEqual("[120730_121948_120]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //startimeDescription YYMMDD_HHMMSS_OOO p[61|3D]	[120730_121948_120]
                    Assert.AreEqual("30-7-2012 14:19:48", validatelogfile.TP1Time, "TP1 Time = "); // Time = 30-7-2012 14:19:48

                    Assert.AreEqual(1480059, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	1480059	p[99|0063]:7B-95-16-00
                    Assert.AreEqual(5688045, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5688045	p[103|0067]:ED-CA-56-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
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

                    Assert.AreEqual("29-7-2013 9:16:49", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 29-7-2013 9:16:49
                    Assert.AreEqual("[Tongelresestraat 423/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Tongelresestraat 423/-  Eindhoven]
                    Assert.AreEqual("[Middenweg 235/224  Venlo]", validatelogfile.EndLogDescription, "EndLogDescription == "); //endLogDescription p[91|5B]	[Middenweg 235/224  Venlo]

                    Assert.AreEqual("[130729_071649_120]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); //startimeDescription YYMMDD_HHMMSS_OOO p[141|8D]	[130729_071649_120]
                    Assert.AreEqual("29-7-2013 9:16:49", validatelogfile.TP1Time, "TP1 Time = "); // Time = 29-7-2013 9:16:49

                    Assert.AreEqual(551283, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	551283	p[241|00f1]:73-69-08-00
                    Assert.AreEqual(5144429, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5144429	p[245|00f5]:6D-7F-4E-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
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

                    Assert.AreEqual("31-12-2013 16:43:56", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 31-12-2013 16:43:56
                    Assert.AreEqual("[Kawoepersteeg 21/24  Ermelo]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Kawoepersteeg 21/24  Ermelo]
                    Assert.AreEqual("[Looweg 15/10  Ermelo]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[79|4F]	[Looweg 15/10  Ermelo]

                    Assert.AreEqual("[131231_154357_060]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[121|79]	[131231_154357_060]
                    Assert.AreEqual("31-12-2013 16:43:57", validatelogfile.TP1Time, "TP1 Time = "); // Time = 31-12-2013 16:43:57

                    Assert.AreEqual(559696, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	559696	p[169|00a9]:50-8A-08-00
                    Assert.AreEqual(5230596, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5230596	p[173|00ad]:04-D0-4F-00
                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }
            }
                       
            //- - - - -
            // 14.3.1 (2014-06-17) Galaxy Note 3; rondje wasven
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
                    
                    Assert.AreEqual("16-6-2014 17:50:53", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time =
                    Assert.AreEqual("[Hofstraat 125/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]
                    Assert.AreEqual("[Tongelresestraat 425/-  Eindhoven]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[49|31]
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
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

                    Assert.AreEqual("22-9-2014 7:11:38", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 22-9-2014 7:11:38
                    Assert.AreEqual("[Insulindelaan  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Insulindelaan  Eindhoven]
                    Assert.AreEqual("[Nieuwleusenerdijk  Zwolle]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[73|49]	[Nieuwleusenerdijk  Zwolle]
             
                   //startimeDescription YYMMDD_HHMMSS_OOO p[125|7D]	[140922_051142_120]
                    Assert.AreEqual("[140922_051142_120]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[125|7D]	[140922_051142_120]
                    Assert.AreEqual("22-9-2014 7:11:42", validatelogfile.TP1Time, "TP1 Time = "); // Time = 22-9-2014 7:11:42

                    Assert.AreEqual(550383, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	550383	p[191|00bf]:EF-65-08-00
                    Assert.AreEqual(5144251, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	5144251	p[195|00c3]:BB-7E-4E-00                
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
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

                    Assert.AreEqual("13-7-2014 11:39:41", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 13-9-2014 11:39:41
                    Assert.AreEqual("[Route 88 1407/1406  Lakewood]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[23|17]	[Insulindelaan  Eindhoven]
                    Assert.AreEqual("[Route 9 532/533  Ocean]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[73|49]	[Nieuwleusenerdijk  Zwolle]

                    Assert.AreEqual("[140713_120341_-24]", validatelogfile.StartTimeDescriptionString, "Start Time Description == "); // startimeDescription YYMMDD_HHMMSS_OOO p[127|7F]	[140713_120341_-24]
                    Assert.AreEqual("13-7-2014 11:39:41", validatelogfile.TP1Time, "TP1 Time = "); // Time = 13-7-2014 11:39:41
                    
                    Assert.AreEqual(-7415324, validatelogfile.TP1Longitude, "TP1 longitude = ");//longitude = 	-7415324	p[219|00db]:E4-D9-8E-FF
                    Assert.AreEqual(4008155, validatelogfile.TP1Latitude, "TP1 latitude = ");//latitude = 	4008155	p[223|00df]:DB-28-3D-00
                }
                else
                {
                    Assert.AreEqual(string.Empty, validatelogfile.Error, "validate error : ");
                }
            }


        } // SygicLogFileV5
        
        // --- start inner helper class -----------------
        
        /// <summary>
        /// Reads the log file provides the "features" for unit test validation
        /// </summary>
        private class ValidateLogFile
        {
            /// <summary>
            /// indicate if the validation passed
            /// </summary>
            private bool passed;
            
            /// <summary>
            /// contains a possible error string
            /// </summary>
            private string error;
            
            /// <summary>
            /// contains the log file version
            /// </summary>
            private int logFileVersion;
            
            /// <summary>
            /// Contains the number of found track points
            /// </summary>
            private int trkpts;
            
            /// <summary>
            /// Contains the corrected start time string
            /// </summary>
            private string corStartTimeString;
            
            /// <summary>
            /// Contains the start place description
            /// </summary>
            private string startLogDescription;
            
            /// <summary>
            /// Contains the end place description
            /// </summary>
            private string endLogDescription;

            /// <summary>
            /// Contains the start time Description string
            /// </summary>
            private string startTimeDescriptionString;

            /* Get first track point data:               
            >>>>  longitude	altitude	elevation	time	speed
            longitude = 	550383	p[191|00bf]:EF-65-08-00
            latitude = 	5144251	p[195|00c3]:BB-7E-4E-00
            elevation = 	67	p[199|00c7]:43-00-00-00
            pointStepTime = 	0	p[203|00cb]:00-00-00-00
            Time = 22-9-2014 5:11:42
            speed = 57.2	1113902285	p[207|00cf]:CD-CC-64-42
            signalQuality = 	3	p[211|00d3]:03
            speeding = 	0	p[212|00d4]:00
            gsmSignalQuality = 	255	p[213|00d5]:FF
            internetSignalQuality = 	0	p[214|00d6]:00
            batteryStatus = 	255	p[215|00d7]:FF                                         
             */
            private long tp1Latitude;
            private long tp1Longitude;
            private string stp1Time;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValidateLogFile" /> class.
            /// The constructor opens the txt log file and loops it to find all features in one go.
            /// If the file could not be opened _enabled is false and _error contains the error message.
            /// </summary>
            /// <param name="fullLogFilename">The full text log file name with extension</param>
            public ValidateLogFile(string fullLogFilename)
            {
                TextReader m_reader;                    
                string logfilename = Path.GetFileNameWithoutExtension(fullLogFilename);
                
                try
                {
                    m_reader = new StreamReader(logfilename + ".txt");
                    this.passed = true;
                    this.error = string.Empty;

                    bool bHeader = true;
                    bool bNowTP1 = false;

                    if (this.passed)
                    {
                        string readline = m_reader.ReadLine();
                        while (readline != null)
                        {
                            if (bHeader)
                            {
                                if (readline.StartsWith("version =", StringComparison.Ordinal))
                                {
                                    string sversion = readline.Substring(10);
                                    this.logFileVersion = Convert.ToInt16(sversion,
                                        CultureInfo.InvariantCulture.NumberFormat);
                                }                               
                                else if (readline.StartsWith("Corrected Start Time = ", StringComparison.Ordinal))
                                {
                                    // Corrected Start Time = 17-7-2012 13:57:55
                                    this.corStartTimeString = readline.Substring(23);
                                }
                                else if (readline.StartsWith("startLogDescription p[", StringComparison.Ordinal))
                                {
                                    // startLogDescription p[27|1B]    [E20  Malmö]
                                    string scutoff = readline.Substring(23);
                                    int idx = scutoff.IndexOf('[');
                                    this.startLogDescription = scutoff.Substring(idx);
                                }
                                else if (readline.StartsWith("endLogDescription p[", StringComparison.Ordinal))
                                {
                                    // endLogDescription p[49|31]    [E20  Malmö]
                                    string scutoff = readline.Substring(21);
                                    int idx = scutoff.IndexOf('[');
                                    this.endLogDescription = scutoff.Substring(idx);
                                }
                                    //startTimeDescriptionString
                                else if (readline.StartsWith("startimeDescription YYMMDD_HHMMSS_OOO p[",
                                    StringComparison.Ordinal))
                                {
                                    // startimeDescription YYMMDD_HHMMSS_OOO p[125|7D]	[140922_051142_120]
                                    string scutoff = readline.Substring(40);
                                    int idx = scutoff.IndexOf('[');
                                    this.startTimeDescriptionString = scutoff.Substring(idx);

                                }
                                else if (readline.StartsWith(">>>>  longitude	altitude", StringComparison.Ordinal))
                                {                                    
                                    //Start of first track point found, next line for track point
                                    bHeader = false;
                                    bNowTP1 = true;
                                }
                            }
                            else if (bNowTP1)
                            {
                                if (readline.StartsWith("\tTime = ", StringComparison.Ordinal)) // Time = 22-9-2014 5:11:42
                                {
                                    this.stp1Time = readline.Substring(8);
                                }
                                else if (readline.StartsWith("\tlongitude =", StringComparison.Ordinal))
                                {
                                    //longitude = 	550383	p[191|00bf]:EF-65-08-00   
                                    string scutoff = readline.Substring(13);
                                    int idx = scutoff.IndexOf('p');
                                    string scutout = scutoff.Substring(0,idx);
                                    this.tp1Longitude = Convert.ToInt32(scutout,
                                        CultureInfo.InvariantCulture.NumberFormat);
                                }
                                else if (readline.StartsWith("\tlatitude =", StringComparison.Ordinal))
                                {
                                    //latitude = 	5144251	p[195|00c3]:BB-7E-4E-00
                                    string scutoff = readline.Substring(11);
                                    int idx = scutoff.IndexOf('p');
                                    string scutout = scutoff.Substring(0, idx);
                                    this.tp1Latitude = Convert.ToInt32(scutout,
                                        CultureInfo.InvariantCulture.NumberFormat);
                                }
                                else if (readline.StartsWith(">>>>", StringComparison.Ordinal))
                                {
                                    //next track point found!
                                    bNowTP1 = false;
                                }
                            }
                            else if (readline.StartsWith("wrote trkpt : ", StringComparison.Ordinal))
                            {
                                string strkpt = readline.Substring(14);
                                this.trkpts = Convert.ToInt16(strkpt, CultureInfo.InvariantCulture.NumberFormat);
                            }

                            // read next line
                            readline = m_reader.ReadLine();
                        }
                    }
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Console.WriteLine("logfilename not found:" + e.Message);
                    m_reader = null;
                    this.passed = false;
                    this.error = e.Message;
                }
            } // ValidateLogFile constructor reader
            
            /// <summary>
            /// Gets a value indicating whether the validation passed.
            /// </summary>
            public bool Passed
            {
                get
                {
                    return this.passed;
                }
            }
            
            /// <summary>
            /// Gets the error text incase the validation didn't pass.
            /// </summary>
            public string Error
            {
                get
                {
                    return this.error;
                }
            }
            
            /// <summary>
            /// Gets the found log file version number
            /// </summary>
            public int LogFileVersion
            {
                get
                {
                    return this.logFileVersion;
                }
            }
            
            /// <summary>
            /// Gets the found number of track points
            /// </summary>
            public int Trkpts
            {
                get
                {
                    return this.trkpts;
                }
            }
            
            /// <summary>
            /// Gets the found correct time string
            /// </summary>
            public string CorrectedStartTime
            {
                get
                {
                    return this.corStartTimeString;
                }
            }
            
            /// <summary>
            /// Gets the found location start description
            /// </summary>
            public string StartLogDescription
            {
                get
                {
                    return this.startLogDescription;
                }
            }
            
            /// <summary>
            /// Gets the found location end description
            /// </summary>
            public string EndLogDescription
            {
                get
                {
                    return this.endLogDescription;
                }
            }

            /// <summary>
            /// Gets the found startimeDescription
            /// </summary>
            public string StartTimeDescriptionString
            {
                get
                {
                    return this.startTimeDescriptionString;
                }
            }

            //--TP1----------
            /// <summary>
            /// Gets the found startimeDescription
            /// </summary>
            public long TP1Latitude
            {
                get
                {
                    return this.tp1Latitude;
                }
            }
            /// <summary>
            /// Gets the found startimeDescription
            /// </summary>
            public long TP1Longitude
            {
                get
                {
                    return this.tp1Longitude;
                }
            }
            /// <summary>
            /// Gets the found startimeDescription
            /// </summary>
            public string TP1Time
            {
                get
                {
                    return this.stp1Time;
                }
            }
            
        } // class ValidateLogFile
        
        // --- end inner helper class -----
    }
}
