using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QFXParser;


namespace TestQFXParser
{
    class Program
    {
        static void showElement(Element e)
        {
            System.Console.Out.WriteLine("<" + e.Name + ">" + e.Text);
            foreach(Element c in e.Children)
            {
                showElement(c);
            }
            System.Console.Out.WriteLine("</" + e.Name + ">");
        }

        static string getFilenameFromArgs(string[] args)
        {
            if (args.GetLength(0) < 1)
                return null;
            return args[0];
        }
        static void Main(string[] args)
        {
            string filename = getFilenameFromArgs(args);

            if (filename == null)
            {
                System.Console.Out.WriteLine("Usage:\r\n\tTestQFXParser <QFXFilePath>");
                
            }
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                Parser p = new Parser(fs, 4);
                Element root = p.parse();
                if(root != null)
                    showElement(root);
            }
            catch (ScannerException se)
            {
                string message = String.Format("%s line %i col %1", se.Message, se.Line, se.Col);
                System.Console.Out.WriteLine(message);
                
            }
            catch (Exception e)
            {
                string message = e.Message + "\r\n" + e.StackTrace;
                System.Console.Out.WriteLine(message);
                
            }
            System.Console.WriteLine("Press any key to continue.");
            System.Console.In.Read();
        }
    }
}
