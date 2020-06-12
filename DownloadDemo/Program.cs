using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DownloadFile();
        }
        static void DownloadFile()
        {
            string username = "xxx";
            string password = "xxx";
            string domain="xxx";
            string repository = "http://xxx:8080/tfs/DefaultCollection";

            // Step 1: Obtain server via authenticated connection to project collection.

            NetworkCredential nc = new NetworkCredential(username, password);
            Uri uri = new Uri(repository);

            //TfsClientCredentials tfsCredential = new TfsClientCredentials(new BasicAuthCredential(nc));
            //tfsCredential.AllowInteractive = false;

            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(uri, nc);
            tpc.Authenticate();

            VersionControlServer vcs = tpc.GetService<VersionControlServer>();

            // Step 2: Obtain item to be downloaded.

            string projectName = "ScrumStarain2019TFVC";
            string fileName = "TestCaseProject/ClassLibrary1/Class1.cs";

            string itemPath = Path.Combine(projectName, fileName);

            Item file = vcs.GetItems(String.Format(@"$/{0}", itemPath), RecursionType.None).Items.First<Item>();

            // Step 3: Download item.

            string destinationFolder = @"D:\DemoFile";

            using (Stream input = file.DownloadFile())
            {
                using (FileStream fout = File.Create(Path.Combine(destinationFolder, "Class1.cs")))
                {
                    input.CopyTo(fout);
                }
            }
        }
    }
}
