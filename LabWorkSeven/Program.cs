using ClassLibrary;
using System;
using System.Reflection;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        //get assembly by name
        Assembly assembly = Assembly.LoadFrom("ClassLibrary1");
        //checking the file
        if (assembly != null) Console.WriteLine("FileFound");
        // Make xmlElem with general Name
        XElement xmlClassesAll = new XElement("ClassesDiagram");

        // Get all classes, enums
        Type[] types = assembly.GetTypes();
        //check all elems
        foreach (var type in types)
        {
            //get attributes, then taking first of them and converting
            var commentAttribute = (CommentAttribute)type.GetCustomAttributes(typeof(CommentAttribute)).FirstOrDefault();
            //making comment
            string comment = "";
            if (commentAttribute != null && commentAttribute.Comment != null)  comment = commentAttribute.Comment;

          
            if (comment == string.Empty && type.IsEnum)
            {
                XElement xmlEnum = new XElement("Enum",
                    new XAttribute("Name", type.Name));

                foreach (var enumValue in Enum.GetNames(type))
                {
                    XElement xmlNames = new XElement("Names",
                        new XAttribute("Name", enumValue)
                    );

                    xmlEnum.Add(xmlNames);
                    
                }
                xmlClassesAll.Add(xmlEnum);
            } else
            {
                XElement xmlClass = new XElement("Class",
              new XAttribute("Name", type.Name),
              new XAttribute("Comment", comment)
          );
                foreach (var property in type.GetProperties())
                {
                    XElement xmlProperty = new XElement("Property",
                        new XAttribute("Name", property.Name)
                    );

                    xmlClass.Add(xmlProperty);
                }
                // Добавляем методы класса в XML
                foreach (var method in type.GetMethods())
                {
                    XElement xmlMethod = new XElement("Method",
                        new XAttribute("Name", method.Name)
                    );

                    xmlClass.Add(xmlMethod);
                    
                }
                xmlClassesAll.Add(xmlClass);
            }
                

          
        }

        XDocument xmlDoc = new XDocument(xmlClassesAll);
        xmlDoc.Save("output.xml");

        Console.WriteLine("XML created!");
    }
}
