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
        } // LogParserSettings
        
        /// <summary>
        /// Testing the properties of the Main <see cref="SygicTravelbookLog" /> class.
        /// </summary>
        [Test]
        public static void SygicTravelbookLog()
        {
            Assert.AreEqual("Sygiclog v1.5.0rc", Sygiclog.SygicTravelbookLog.SygiclogVersionString, " SygicTravelbookLog version sTitle");
            
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
                    
                    Assert.AreEqual("28-5-2012 11:48:12", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time
                    Assert.AreEqual("[CV-502  Cullera]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription
                    Assert.AreEqual("[Avenida de la Gola del Puchol  Valencia]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription
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
                    
                    Assert.AreEqual("30-5-2012 9:05:14", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time
                    Assert.AreEqual("[Estación de Tamarit 28/53  Moncofa]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription
                    Assert.AreEqual("[Avenida Mallorca 134/101  Nules]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription
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
                    
                    Assert.AreEqual("15-7-2011 7:53:30", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time
                    Assert.AreEqual("[Tongelresestraat 423/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription
                    Assert.AreEqual("[Grensstraat -/1  Veurne]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription
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
                    
                    Assert.AreEqual("20-5-2012 11:39:17", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time
                    Assert.AreEqual("[Terno d'Isola]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription
                    Assert.AreEqual("[Tangenziale Est  Vimercate]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription
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
                    
                    Assert.AreEqual("17-7-2012 13:57:55", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time
                    Assert.AreEqual("[E20  Malmö]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription
                    Assert.AreEqual("[E20  Malmö]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription
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
                    
                    Assert.AreEqual("13-4-2012 7:47:38", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time
                    Assert.AreEqual("[Collse Hoefdijk  Nuenen, Gerwen en Nederwetten]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription
                    Assert.AreEqual("[N270  Helmond]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription
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
                    
                    Assert.AreEqual("1-8-2012 8:31:58", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 17-7-2012 13:57:55
                    Assert.AreEqual("[Sønderjyske Motorvej  Vojens]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]    [E20  Malmö]
                    Assert.AreEqual("[Sønderjyske Motorvej  Vojens]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[49|31]    [E20  Malmö]
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
                    
                    Assert.AreEqual("30-7-2012 14:19:48", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 17-7-2012 13:57:55
                    Assert.AreEqual("[Växjö]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]    [E20  Malmö]
                    Assert.AreEqual("[23  Grimslöv]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[49|31]    [E20  Malmö]
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
                    
                    Assert.AreEqual("29-7-2013 9:16:49", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 17-7-2012 13:57:55
                    Assert.AreEqual("[Tongelresestraat 423/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]    [E20  Malmö]
                    Assert.AreEqual("[Middenweg 235/224  Venlo]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[49|31]    [E20  Malmö]
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
                    
                    Assert.AreEqual("31-12-2013 16:43:56", validatelogfile.CorrectedStartTime, "Corrected Start Tim == "); // Corrected Start Time = 17-7-2012 13:57:55
                    Assert.AreEqual("[Kawoepersteeg 21/24  Ermelo]", validatelogfile.StartLogDescription, "StartLogDescription == "); // startLogDescription p[27|1B]    [E20  Malmö]
                    Assert.AreEqual("[Looweg 15/10  Ermelo]", validatelogfile.EndLogDescription, "EndLogDescription == "); // endLogDescription p[49|31]    [E20  Malmö]
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
                    
                    if (this.passed)
                    {
                        string readline = m_reader.ReadLine();
                        while (readline != null)
                        {
                            if (readline.StartsWith("version =", StringComparison.Ordinal))
                            {
                                string sversion = readline.Substring(10);
                                this.logFileVersion = Convert.ToInt16(sversion, CultureInfo.InvariantCulture.NumberFormat);
                            }
                            else if (readline.StartsWith("wrote trkpt : ", StringComparison.Ordinal))
                            {
                                string strkpt = readline.Substring(14);
                                this.trkpts = Convert.ToInt16(strkpt, CultureInfo.InvariantCulture.NumberFormat);
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
        } // class ValidateLogFile
        
        // --- end inner helepr class -----
    }
}
