//-----------------------------------------------------------------------
// <copyright file="LogParserSettings.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
/*
 * Created by SharpDevelop.
 * User: paul
 * Date: 29-6-2012
 * Time: 14:27
 */

namespace Sygiclog
{
    using System;
    using System.Globalization; // CultureInfo.InvariantCulture
    
/// <summary>
/// This class contains the settings that are passed to each <see cref="SygicLogFile" /> class to process that file,
/// with these settings.
/// </summary>
public class LogParserSettings
{
    /// <summary>
    /// The title for logging???
    /// </summary>
    private string appTitle;
    
    /// <summary>
    /// The file to process
    /// </summary>
    private string inputFileName;
    
    /// <summary>
    /// Which extension to use for the GPX files (probably XML or GPX)
    /// </summary>
    private string xmlExtension;
    
    /// <summary>
    /// If we need to do all files (in this directory)
    /// </summary>
    private bool doAll;
    
    /// <summary>
    /// The time zone correction string.
    /// </summary>
    private string tzcName;
    
    /// <summary>
    /// The time zone correction, hours part
    /// </summary>
    private int tzcHours;
    
    /// <summary>
    /// The time zone correction, minutes part
    /// </summary>
    private int tzcMinutes;
    
    /// <summary>
    /// Do we need to validate the generated GPX (xml) file?
    /// </summary>
    private bool doValidate;
    
    /// <summary>
    /// Use the GPX extension
    /// </summary>
    private bool useGpxExt;
    
    /// <summary>
    /// Wait on user input, for use within a script  to validate the output before finishing.
    /// </summary>
    private bool waitConsole;
    
    /// <summary>
    /// Create a text log file describing the parsing, for debugging.
    /// </summary>
    private bool createTxtlog;
    
    /// <summary>
    /// The log start time
    /// </summary>
    private string logStartTimeString;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogParserSettings" /> class.
    /// A default constructor.
    /// </summary>
    public LogParserSettings()
    {
        this.appTitle = string.Empty;
        this.inputFileName = string.Empty;
        this.XmlExtension = ".gpx";
        this.doAll = false;
        this.tzcName = string.Empty;
        this.tzcHours = 0;
        this.tzcMinutes = 0;
        this.doValidate = false;
        this.useGpxExt = false;
        this.waitConsole = false;
        this.createTxtlog = false;
        this.logStartTimeString = string.Empty;
    }
    
    /// <summary>
    /// Gets or sets a value indicating whether All option is to be used.
    /// </summary>
    public bool All
    {
        get
        {
            return this.doAll;
        }
        
        set
        {
            this.doAll = value;
        }
    }
    
    /// <summary>
    /// Gets or sets a value indicating whether the time zone string
    /// </summary>
    public string Tzc
    {
        get
        {
            return this.tzcName;
        }
        
        set
        {
            this.tzcName = value;
        }
    }
    
    /// <summary>
    /// Gets or sets a value indicating whether time zone hour correction
    /// </summary>
    public int TzcHours
    {
        get
        {
            return this.tzcHours;
        }
        
        set
        {
            this.tzcHours = value;
        }
    }
    
    /// <summary>
    /// Gets or sets a value indicating whether time zone minute correction
    /// </summary>
    public int TzcMinutes
    {
        get
        {
            return this.tzcMinutes;
        }
        
        set
        {
            this.tzcMinutes = value;
        }
    }
    
    /// <summary>
    /// Gets or sets a value indicating whether the AppTitle string
    /// </summary>
    public string AppTitle
    {
        get
        {
            return this.appTitle;
        }
        
        set
        {
            this.appTitle = value;
        }
    }
    
    /// <summary>
    /// Gets or sets a value indicating whether the input file name
    /// </summary>
    public string InputFileName
    {
        get
        {
            return this.inputFileName;
        }
        
        set
        {
            this.inputFileName = value;
        }
    }
    
    /*
    /// <summary>
    /// Gets or sets a value indicating whether the file name root (without extension)
    /// </summary>
    public string FileNameWithoutExtension
    {
        get
        {
            return this.fileNameWithoutExtension;
        }
        
        set
        {
            this.fileNameWithoutExtension = value;
        }
    }
    */
   
    /// <summary>
    /// Gets or sets a value indicating whether the xml extension string
    /// </summary>
    public string XmlExtension
    {
        get
        {
            return this.xmlExtension;
        }
        
        set
        {
            this.xmlExtension = value;
        }
    }
    
    /// <summary>
    /// Gets or sets a value indicating whether to validate the GPX output for correct xml formatting
    /// </summary>
    public bool Validate
    {
        get
        {
            return this.doValidate;
        }
        
        set
        {
            this.doValidate = value;
        }
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the GPX extension option 
    /// </summary>
    public bool GpxExt
    {
        get
        {
            return this.useGpxExt;
        }
        
        set
        {
            this.useGpxExt = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the wait on console option
    /// </summary>
    public bool WaitConsole
    {
        get
        {
            return this.waitConsole;
        }
        
        set
        {
            this.waitConsole = value;
        }
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the log file option 
    /// </summary>
    public bool TxtLog
    {
        get
        {
            return this.createTxtlog;
        }
        
        set
        {
            this.createTxtlog = value;
        }
    }
    
    /// <summary>
    ///  Gets or sets a value indicating whether the log file start time string
    /// </summary>
    public string LogStartTime
    {
        get
        {
            return this.logStartTimeString;
        }
        
        set
        {
            this.logStartTimeString = value;
        }
    }

    /// <summary>
    /// Create a string for logging all settings in the log file
    /// </summary>
    /// <returns>the comment string</returns>
    public string ToComment()
    {
        string comment = "\n" +
            "m_sAppTitle=["         + this.appTitle + "]\n" +
            "m_sInputFileName=["     + this.inputFileName + "]\n" +
            "m_sXMLExtention=["     + this.XmlExtension + "]\n" +
            "m_bAll=["                 + this.doAll.ToString() + "]\n" +
            "m_sTzc=["                 + this.tzcName + "] " + this.tzcHours.ToString(CultureInfo.InvariantCulture) + " : " + this.tzcMinutes.ToString(CultureInfo.InvariantCulture) + "\n" +
            "m_bValidate=["         + this.doValidate.ToString() + "]\n" +
            "m_bGpxExt=["             + this.useGpxExt.ToString() + "]\n" +
            "m_bWaitConsole=["         + this.waitConsole.ToString() + "]\n" +
            "m_bTxtlog=["             + this.createTxtlog.ToString() + "]\n" +
            "m_sLogStartTime=["     + this.logStartTimeString + "]\n";
        return comment;
    }
} // class LogParserSettings
} // namespace sygiclog
