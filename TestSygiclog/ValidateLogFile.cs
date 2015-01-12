//-----------------------------------------------------------------------
// <copyright file="SygicTravelbookLog.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Globalization; // Culture
using System.IO; // File Path

namespace TestSygiclog
{
    /// <summary>
    /// Reads the log file provides the "features" for unit test validation
    /// </summary>
    internal class ValidateLogFile
    {
        /// <summary>
        /// indicate if the validation passed
        /// </summary>
        private readonly bool passed;

        /// <summary>
        /// contains a possible error string
        /// </summary>
        private readonly string error;

        /// <summary>
        /// contains the log file version
        /// </summary>
        private readonly int logFileVersion;

        /// <summary>
        /// Contains the number of found track points
        /// </summary>
        private readonly int trkpts;

        /// <summary>
        /// Contains the corrected start time string
        /// </summary>
        private readonly string corStartTimeString;

        /// <summary>
        /// Contains the start place description
        /// </summary>
        private readonly string startLogDescription;

        /// <summary>
        /// Contains the end place description
        /// </summary>
        private readonly string endLogDescription;

        /// <summary>
        /// Contains the start time Description string
        /// </summary>
        private readonly string startTimeDescription;

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
        private readonly long tp1Latitude;
        private readonly long tp1Longitude;
        private readonly string stp1Time;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateLogFile" /> class.
        /// The constructor opens the txt log file and loops it to find all features in one go.
        /// If the file could not be opened _enabled is false and _error contains the error message.
        /// </summary>
        /// <param name="fullLogFilename">The full text log file name with extension</param>
        public ValidateLogFile(string fullLogFilename)
        {
            TextReader logReader;
            string logfilename = Path.GetFileNameWithoutExtension(fullLogFilename);

            try
            {
                logReader = new StreamReader(logfilename + ".txt");
                this.passed = true;
                this.error = string.Empty;

                bool bHeader = true;
                bool bNowTP1 = false;

                if (this.passed)
                {
                    string readline = logReader.ReadLine();
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
                                this.startTimeDescription = scutoff.Substring(idx);

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
                                string scutout = scutoff.Substring(0, idx);
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
                        readline = logReader.ReadLine();
                    }
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine("logfilename not found:" + e.Message);
                logReader = null;
                this.passed = false;
                this.error = e.Message;
            }
        } // ValidateLogFile constructor reader

        /// <summary>
        /// Gets a value indicating whether the validation passed.
        /// </summary>
        public bool Passed
        {
            get { return this.passed; }
        }

        /// <summary>
        /// Gets the error text in case the validation didn't pass.
        /// </summary>
        public string Error
        {
            get { return this.error; }
        }

        /// <summary>
        /// Gets the found log file version number
        /// </summary>
        public int LogFileVersion
        {
            get { return this.logFileVersion; }
        }

        /// <summary>
        /// Gets the found number of track points
        /// </summary>
        public int Trkpts
        {
            get { return this.trkpts; }
        }

        /// <summary>
        /// Gets the found correct time string
        /// </summary>
        public string CorrectedStartTime
        {
            get { return this.corStartTimeString; }
        }

        /// <summary>
        /// Gets the found location start description
        /// </summary>
        public string StartLogDescription
        {
            get { return this.startLogDescription; }
        }

        /// <summary>
        /// Gets the found location end description
        /// </summary>
        public string EndLogDescription
        {
            get { return this.endLogDescription; }
        }

        /// <summary>
        /// Gets the found startimeDescription
        /// </summary>
        public string StartTimeDescription
        {
            get { return this.startTimeDescription; }
        }

        //----Track Point 1 data-----------------------------------------
        /// <summary>
        /// Gets the found startimeDescription
        /// </summary>
        public long TP1Latitude
        {
            get { return this.tp1Latitude; }
        }

        /// <summary>
        /// Gets the found startimeDescription
        /// </summary>
        public long TP1Longitude
        {
            get { return this.tp1Longitude; }
        }

        /// <summary>
        /// Gets the found startimeDescription
        /// </summary>
        public string TP1Time
        {
            get { return this.stp1Time; }
        }

    } // class ValidateLogFile
}