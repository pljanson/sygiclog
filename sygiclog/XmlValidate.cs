/*
 * Created by SharpDevelop.
 * User: plj
 * Date: 30-6-2012
 * Time: 15:02
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml;
using System.Xml.Schema; //validating

namespace sygiclog
{
/// <summary>
/// Description of XmlValidate.
/// </summary>
public class XmlValidate
{
	int ErrorsCount = 0;

	public XmlValidate()
	{
		//empty constructor for now
	} //xmlValidate constructor

	public void validate(string xmlFile, string xsdFile, string xsdextFile)
	{
		Console.WriteLine("\n[validate]: xml:[" + xmlFile + "] with xsd:[" + xsdFile + "] with xsdext:[" + xsdextFile + "]");

		XmlReaderSettings settings = new XmlReaderSettings();
		settings.Schemas.Add(null, xsdFile);

		if(xsdextFile != "")
		{
			settings.Schemas.Add(null, xsdextFile);
		}

		settings.ValidationType = ValidationType.Schema;

		try
		{
			XmlReader reader = XmlReader.Create(xmlFile, settings);
			XmlDocument Odoc = new XmlDocument();
			Odoc.Load(reader);
			ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

			Odoc.Validate(eventHandler);
		}
		catch(XmlSchemaValidationException e)
		{
			Console.WriteLine("            Error: " + e.Message + "line:" + e.LineNumber + " pos:" + e.LinePosition);
			ErrorsCount++;
		}

		//Final feedback
		if(ErrorsCount == 0)
		{
			Console.WriteLine("            Validation completed successfully.");
		}
		else if(ErrorsCount == 1)
		{
			Console.WriteLine("            Validation Failed: there was 1 error found");
		}
		else
		{
			Console.WriteLine("            Validation Failed: there were " + ErrorsCount + " errors found");
		}
	} // method validate

	public void ValidationEventHandler(object sender, ValidationEventArgs args)
	{
		Console.WriteLine("ValidationEventHandler Error: " + args.Message);
		ErrorsCount++;
	} //method ValidationEventHandler
} //class xmlValidate

} //namespace sygiclog
