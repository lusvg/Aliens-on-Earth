using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using iTextSharp.text;        //reference added for generating PDF File 
using iTextSharp.text.pdf;   //reference added for generating PDF File 
using System.IO;

using System.ComponentModel.Composition;        //namespaces contain MEF types contains all the parts available and performs composition.
                                                //(That is, the matching up of imports to exports.)

using System.ComponentModel.Composition.Hosting; //This will allow us to use the Import and Export statements directly. 
                                                 //It will also allow us to call the “ComposeParts” method on our container.

namespace Solution_AlienonEarth
{
    class Program
    {
        public SqlConnection Con;

        //This is our “port”. We are asking our hand to find us a plug that fits into a string variable.

        [Import]
        string message;

        static void Main(string[] args)
        {
            try
            {
                //We are instantiating this program and calling
                //it so we can properly use MEF

                Program p = new Program();
                p.major();
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Following Exception Found:" + e.Message);
            }
        }




        public void major()
        {
            int i;
            char ch;
            ch = 'y';
           
            while (ch == 'y') 
            {

                Compose();//This is our helper method that runs everything
                
                Console.WriteLine("Press 1: Register New Aliens");
                Console.WriteLine("Press 2: Export Aliens Detail in PDF Format");
                Console.WriteLine("Press 3: Export Aliens Detail in Text Format");
                Console.WriteLine("Press 4: Exit");

                Console.WriteLine("Enter Your Choice:");
                i = int.Parse(Console.ReadLine());
                
               
                switch (i)
                {
                    case 1:
                        Register();
                        Console.WriteLine(message); //Displaying the message by importing Foobar Plug-in
                        break;
                    case 2:
                        Pdf();
                        Console.WriteLine(message);
                        break;
                    case 3:
                        Text();
                        Console.WriteLine(message);
                        break;
                    case 4:
                        Console.WriteLine(message);
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please enter a correct choice");
                        break;
                }
                Console.WriteLine("Do you want to continue (y/n) :");
                ch = char.Parse(Console.ReadLine());
                //Console.ReadKey();
            }

        }

        // Open Connection
        public void OpenConnection()
        {
            Con = new SqlConnection(Solution_AlienonEarth.Properties.Settings.Default.Setting);
            Con.Open();
        }

       
        private void Compose()
        {
            //We are loading the currently-executing assembly
            AssemblyCatalog catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(catalog);

            //Here we are hooking up the "plugs" to the "ports".  This is one of the options to hook everything up. 
            container.SatisfyImportsOnce(this);
        }

        public void Register()
        {
            try
            {
                string CodeName, BloodColor, HomePlanet;
                int Antenna, NoLegs;
                Console.WriteLine("Enter Alian's Detail");
                Console.WriteLine("Code Name");
                CodeName = Console.ReadLine();
                Console.WriteLine("Blood Color");
                BloodColor = Console.ReadLine();
                Console.WriteLine("No of Antenna");
                Antenna = int.Parse(Console.ReadLine());
                Console.WriteLine("No of Legs");
                NoLegs = int.Parse(Console.ReadLine());
                Console.WriteLine("Home Planet");
                HomePlanet = Console.ReadLine();
                OpenConnection();  // Open the Connection
                string SqlQry1 = "INSERT INTO Alien_Details ([Code_Name],[Blood_Color],[Antenna],[No_Legs],[Home_Planet]) Values ('" + CodeName + "','" + BloodColor + "','" + Antenna + "','" + NoLegs + "','" + HomePlanet + "')";
                SqlCommand cmd = new SqlCommand(SqlQry1, Con);
                cmd.ExecuteNonQuery();
                Con.Close();
                Console.WriteLine("Details Inserted Successfully!!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Following Exception Found:" + e.Message);
            }
        }

        public void Pdf()
        {
            try
            {
                string DirectoryPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                //need a PDF to create/modify based on our requirement
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string sourcePath = System.IO.Path.Combine(desktop, "Alien_Details.pdf");
                //Dataset will bring all the data for PDF
                DataTable DT = new DataTable();
                DT = fillData();
                using (Document doc = new Document(PageSize.A4, 42, 73, 85, 46))
                {
                    
                    //Create a new PDF from template
                    var existingFileStream = new FileStream(sourcePath, FileMode.Create);
                    PdfWriter writer = PdfWriter.GetInstance(doc, existingFileStream);
                    doc.Open();
                    //Create First Page a table with five columns          
                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    //Adjust the font size
                    iTextSharp.text.Font bodyFont = FontFactory.GetFont("Time New Roman", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    PdfPCell cell1, cell2, cell3, cell4, cell5;
                    Phrase test1 = new Phrase("Code Name");
                    Phrase test2 = new Phrase("Blood Color");
                    Phrase test3 = new Phrase("No of Antenna");
                    Phrase test4 = new Phrase("No of Legs");
                    Phrase test5 = new Phrase("Home Planete");
                    cell1 = new PdfPCell(test1);
                    cell2 = new PdfPCell(test2);
                    cell3 = new PdfPCell(test3);
                    cell4 = new PdfPCell(test4);
                    cell5 = new PdfPCell(test5);
                    table.AddCell(cell1);
                    table.AddCell(cell2);
                    table.AddCell(cell3);
                    table.AddCell(cell4);
                    table.AddCell(cell5);

                    foreach (DataRow dtRow in DT.Rows)
                    {
                        // On all tables' columns
                        foreach (DataColumn dc in DT.Columns)
                        {

                            Chunk testChunk = new Chunk(dtRow[dc].ToString(), bodyFont);
                            Phrase testPhrase = new Phrase(testChunk);
                            PdfPCell cell;
                            cell = new PdfPCell(testPhrase);
                            //cell.PaddingTop = 10;
                            //cell.PaddingBottom = 10;
                            //cell.PaddingLeft = 10;
                            //cell.PaddingRight = 10;
                            //cell.FixedHeight = 15f;
                            cell.Border = PdfPCell.BOX; //Adjust Border
                            table.AddCell(cell);
                        }

                    }

                    doc.Add(table);
                    doc.Close();
                    Console.WriteLine("Pdf file is saved on your desktop");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Following Exception Found:" + e.Message);
            }
        }

        private static DataTable fillData()
        {
            Program objProgram = new Program(); 
       
            DataTable datatable;
            using (objProgram.Con)
            {
                objProgram.OpenConnection();
                // Create new DataAdapter
                using (SqlDataAdapter adaptor = new SqlDataAdapter("SELECT Code_Name,Blood_Color,Antenna,No_Legs,Home_Planet FROM Alien_Details", objProgram.Con))
                {
                    // Use DataAdapter to fill DataTable
                    datatable = new DataTable();
                    adaptor.Fill(datatable);
                }
            }
            return datatable;
        }

        public void Text()
        {
            try
            {

                OpenConnection();
                string queryString = "SELECT Code_Name,Blood_Color,Antenna,No_Legs,Home_Planet FROM Alien_Details";
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, Con);
                DataSet details = new DataSet();
                adapter.Fill(details, "Alien_Details");
                DataTable dt = details.Tables[0];
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string sourcePath = System.IO.Path.Combine(desktop, "Alien_Details.txt");
                TextWriter sw = new StreamWriter(sourcePath);
                sw.Write("Code");
                sw.Write("\t");
                sw.Write("Blood");
                sw.Write("\t");
                sw.Write("Antenna");
                sw.Write("\t");
                sw.Write("Legs");
                sw.Write("\t");
                sw.Write("Planete");
                sw.Write("\t");
                sw.WriteLine("\n");
                foreach (DataRow dtRow in dt.Rows)
                {
                    // On all tables' columns
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sw.Write(dtRow[dc].ToString());
                        sw.Write("\t");
                    }
                    sw.WriteLine("\n");
                }

                sw.Close();     //Closeing TextWriter Object(sw)

                Console.WriteLine("Text file is saved on your desktop");
            }
            catch (Exception e)
            {
                Console.WriteLine("Following Exception Found:" + e.Message);
            }
        }
    }

    // Extensible Plug-in

    public class Foobar
    {
        //The MyMessage property is our “plug” that will be put into the string “port” we defined above. 
        [Export()]
        public string MyMessage
        {
            get { return "Thanks for using Foobar Plugin!!"; }
        }
    }

}
