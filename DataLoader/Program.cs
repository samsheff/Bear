using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;

namespace DataLoader
{
    class Program
    {
        public string DataDirectory = @"C:\\Companies Data\\";
        static void Main(string[] args)
        {
            Program program = new Program();

            program.WriteHeader();

            foreach (var file in program.GetListOfFiles(program.DataDirectory, @"*.xml"))
            {
                FileInfo info = new FileInfo(file);

                program.ParseDocument(XDocument.Load(info.FullName));
            }
        }

        public string[] GetListOfFiles(string directory, string pattern)
        {
            return System.IO.Directory.GetFiles(directory, pattern, System.IO.SearchOption.AllDirectories);
        }

        public void WriteHeader()
        {
            Console.WriteLine("id|company_name|company_short_name|inn|company_type|oblast_id|region_type|region_name|street_type|street_name|main_activity");
        }

        public void ParseDocument(XDocument document)
        {
            Console.OutputEncoding = Encoding.UTF8;
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Someone\test.csv", true))
            {

                var documents = document.Root.Elements("Документ").ToList();
                foreach (var docNode in documents)
                {
                    string docId = docNode.Attribute("ИдДок").Value;
                    try
                    {
                        string name = docNode.Element("ОргВклМСП").Attribute("НаимОрг").Value;
                        string shortName = docNode.Element("ОргВклМСП").Attribute("НаимОргСокр").Value;
                        string inn = docNode.Element("ОргВклМСП").Attribute("ИННЮЛ").Value;
                        string companyType = docNode.Attribute("ВидСубМСП").Value.ToString();
                        string oblastId = docNode.Element("СведМН").Attribute("КодРегион").Value.ToString();
                        string regionType = docNode.Element("СведМН").Element("Регион").Attribute("Тип").Value.ToString();
                        string regionName = docNode.Element("СведМН").Element("Регион").Attribute("Наим").Value.ToString();
                        string streetType = docNode.Element("СведМН").Element("НаселПункт").Attribute("Тип").Value.ToString();
                        string streetName = docNode.Element("СведМН").Element("НаселПункт").Attribute("Наим").Value.ToString();
                        string mainActivity = docNode.Element("СвОКВЭД").Element("СвОКВЭДОсн").Attribute("НаимОКВЭД").Value.ToString();

                        foreach (var element in docNode.Element("СвОКВЭД").Elements("СвОКВЭДДоп").Attributes("НаимОКВЭД"))
                        {
                            mainActivity += " " + element.Value.ToString();
                        }

                        string line = docId + "|" + name + "|" + shortName + "|" + inn + "|" + companyType + "|" + oblastId + 
                            "|" + regionType + "|" + regionName + "|" + streetType + "|" + streetName + "|" + mainActivity;

                        file.WriteLine(line);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
