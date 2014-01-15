/*
 * Created by SharpDevelop.
 * User: paul
 * Date: 29-6-2012
 * Time: 15:16
 */
using System;
using System.IO; //StreamWriter

namespace sygiclog
{
/// <summary>
/// Description of LogFile.
/// </summary>
public class LogFile
{
	private bool m_bEnabled;
	private string m_sLogFile;
	private TextWriter m_writer;

	public bool Enabled
	{
		get
		{
			return m_bEnabled;
		}
		//set {m_bEnabled = ValueType;}
	}

	public LogFile(bool bEnable, string sLogFile)
	{
		m_bEnabled = bEnable;
		m_sLogFile = sLogFile;
		
		//open the logfile
		if(m_bEnabled)
		{
			m_writer = new StreamWriter(sLogFile);
		}
	}
	~LogFile()
	{
		//close the logfile
		if(m_bEnabled)
		{
			//close the stream
			m_writer.Close();
		}

	}
	public void log(string sLogString)
	{
		//do log this line
		if(m_bEnabled)
		{
			m_writer.WriteLine(sLogString);
		}
	}

	public static int bytes2int32(int b1, int b2, int b3, int b4)
	{
		return b4 * 256 * 256 * 256 + b3 * 256 * 256 + b2 * 256 + b1;
	}

	public void logByte(string sLogString, int position, int byte1)
	{
		//do log this line
		if(m_bEnabled)
		{
			string str = String.Format("\t{0}\tp[{1}|{1:x4}]:{2:X2}", byte1, position, byte1);

			m_writer.WriteLine(sLogString + str);
		}
	}

	public void log4Bytes(string sLogString, int position, int byte1, int byte2, int byte3, int byte4, bool bChar = true)
	{
		//do log this line
		if(m_bEnabled)
		{
			int int32 = bytes2int32(byte1, byte2, byte3, byte4);
			string str;
			if (bChar)
			{
				str = String.Format("\t{0}\tp[{1}|{1:x4}]:{2:X2}-{3:X2}-{4:X2}-{5:X2} | {6}{7}{8}{9}", int32, position, byte1, byte2, byte3, byte4, 
			                           (char)byte1,(char)byte2, (char)byte3, (char)byte4 );
			}
			else
			{
				str = String.Format("\t{0}\tp[{1}|{1:x4}]:{2:X2}-{3:X2}-{4:X2}-{5:X2}", int32, position, byte1, byte2, byte3, byte4 );
			}
			m_writer.WriteLine(sLogString + str);
		}
	}
	public void log32(string sLogString, int position, int int32)
	{
		//do log this line
		if(m_bEnabled)
		{
			byte[] bytes = BitConverter.GetBytes(int32);
			string str = String.Format("\t{0}\tp[{1}|{1:x4}]:{2:X2}-{3:X2}-{4:X2}-{5:X2}", int32, position, bytes[0], bytes[1], bytes[2],  bytes[3]);

			m_writer.WriteLine(sLogString + str);
		}
	}

	public void close()
	{
		if(m_bEnabled)
		{
			//close the stream
			m_bEnabled = false;
			m_writer.Close();
		}
	}
}
}
