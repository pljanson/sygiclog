//-----------------------------------------------------------------------
// <copyright file="LogFile.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
/*
 * Created by SharpDevelop.
 * User: paul
 * Date: 29-6-2012 / 24-01-2014
 * Time: 15:16
 */

namespace Sygiclog
{
    using System;
    using System.Globalization; 
    using System.IO; // StreamWriter
    
/// <summary>
/// Description of LogFile.
/// </summary>
public class LogFile
{
    /// <summary>
    /// Enabled / disabled logging
    /// </summary>
    private bool enabled;
    
    /// <summary>
    /// the stream writer to write to
    /// </summary>
    private TextWriter writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogFile" /> class.
    /// Constructor, create a log file (en/dis-abled) and in the mentioned file.
    /// </summary>
    /// <param name="enable">if logging is enabled</param>
    /// <param name="logFileName">the file to log to</param>
    public LogFile(bool enable, string logFileName)
    {
        this.enabled = enable;
        
        // open the logfile
        if (this.enabled)
        {
            this.writer = new StreamWriter(logFileName);
        }
    }
    
    /// <summary>
    /// Finalizes an instance of the <see cref="LogFile" /> class.
    /// The destructor closes the log file if enabled.
    /// </summary>
    ~LogFile()
    {
        // close the logfile
        if (this.enabled)
        {
            // close the stream
            this.writer.Close();
        }
    }
        
    /// <summary>
    /// Gets a value indicating whether logging is enabled
    /// </summary>
    public bool Enabled
    {
        get
        {
            return this.enabled;
        }
        
        // set {m_bEnabled = ValueType;}
    }

    /// <summary>
    /// Convert four bytes to one integer
    /// </summary>
    /// <param name="byte1">least significant byte</param>
    /// <param name="byte2">2de byte</param>
    /// <param name="byte3">3rd byte</param>
    /// <param name="byte4">most significant byte</param>
    /// <returns>the calculated 32bits integer</returns>
    public static int Bytes2Int32(byte byte1, byte byte2, byte byte3, byte byte4)
    {
        return (byte4 * 256 * 256 * 256) + (byte3 * 256 * 256) + (byte2 * 256) + byte1;
    }
    
    /// <summary>
    /// Log this string (if enabled)
    /// </summary>
    /// <param name="logMessage">the string to be logged</param>
    public void Logline(string logMessage)
    {
        // do log this line
        if (this.enabled)
        {
            this.writer.WriteLine(logMessage);
        }
    }

    /// <summary>
    /// Log the data byte, its position in the input file and the bytes hex value
    /// </summary>
    /// <param name="logMessage">the log string / description</param>
    /// <param name="position">to position in the source file of this byte</param>
    /// <param name="value">the value of the byte</param>
    public void LogByte(string logMessage, int position, byte value)
    {
        // do log this line
        if (this.enabled)
        {
            string str = string.Format(CultureInfo.InvariantCulture, "\t{0}\tp[{1}|{1:x4}]:{2:X2}", value, position, value);

            this.writer.WriteLine(logMessage + str);
        }
    }

    /// <summary>
    /// log four bytes, its position in the input file and the bytes hex value
    /// </summary>
    /// <param name="logMessage">the log string, description</param>
    /// <param name="position">to position in the source file of this byte</param>
    /// <param name="byte1">least significant byte</param>
    /// <param name="byte2">byte 2</param>
    /// <param name="byte3">byte 3</param>
    /// <param name="byte4">most significant byte</param>
    /// <param name="isChar">log as characters, else log as Hex values</param>
    public void Log4Bytes(string logMessage, int position, byte byte1, byte byte2, byte byte3, byte byte4, bool isChar = true)
    {
        // do log this line
        if (this.enabled)
        {
            int int32 = Bytes2Int32(byte1, byte2, byte3, byte4);
            string str;
            if (isChar)
            {
                str = string.Format(
                    CultureInfo.InvariantCulture,
                          "\t{0}\tp[{1}|{1:x4}]:{2:X2}-{3:X2}-{4:X2}-{5:X2} | {6}{7}{8}{9}",
                          int32, 
                          position, 
                          byte1, 
                          byte2, 
                          byte3, 
                          byte4, 
                          (char)byte1, 
                          (char)byte2, 
                          (char)byte3, 
                          (char)byte4);
            }
            else
            {
                str = string.Format(CultureInfo.InvariantCulture, "\t{0}\tp[{1}|{1:x4}]:{2:X2}-{3:X2}-{4:X2}-{5:X2}", int32, position, byte1, byte2, byte3, byte4);
            }
            
            this.writer.WriteLine(logMessage + str);
        }
    }
    
    /// <summary>
    /// loa a 32 bits value
    /// </summary>
    /// <param name="logMessage">the log string / description</param>
    /// <param name="position">to position in the source file of this byte</param>
    /// <param name="value32">the 32bits value to log</param>
    public void Log32(string logMessage, int position, int value32)
    {
        // do log this line
        if (this.enabled)
        {
            byte[] bytes = BitConverter.GetBytes(value32);
            string str = string.Format(CultureInfo.InvariantCulture, "\t{0}\tp[{1}|{1:x4}]:{2:X2}-{3:X2}-{4:X2}-{5:X2}", value32, position, bytes[0], bytes[1], bytes[2],  bytes[3]);
            this.writer.WriteLine(logMessage + str);
        }
    }

    /// <summary>
    /// Close the log file, if enabled 
    /// </summary>
    public void Close()
    {
        if (this.enabled)
        {
            // close the stream
            this.enabled = false;
            this.writer.Close();
        }
    }
}
}
