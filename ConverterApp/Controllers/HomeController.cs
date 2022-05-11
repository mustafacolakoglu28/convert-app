using System;
using System.Collections.Generic;
using System.Diagnostics;
using Syncfusion.Drawing;

using Microsoft.AspNetCore.Mvc;
using ConverterApp.Models;
using System.IO;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;

using Microsoft.AspNetCore.Http;

namespace ConverterApp.Controllers
{
    public class HomeController : Controller

    {
       

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ConvertToPDF(IList<IFormFile> files)
        {
            //Creating the new PDF document
            PdfDocument document = new PdfDocument();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    var file = System.IO.File.Create(filePath);
                    formFile.CopyTo(file);

                    //Loading the image
                    PdfImage image = PdfImage.FromStream(file);
                    //Adding new page
                    PdfPage page = page = document.Pages.Add();

                    //Drawing image to the PDF page
                    page.Graphics.DrawImage(image, new PointF(0, 0));
                    file.Dispose();
                }
            }
            

            //saving the PDF to the 
            var pathPDF = Path.GetTempFileName();
            var stream = System.IO.File.Create(pathPDF);
            document.Save(stream);
 

            //Set the position as '0'.
            stream.Position = 0;

            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application / pdf");

            fileStreamResult.FileDownloadName = "ImageToPDF.pdf";

            //deleting the temp file
            var deleteFile = new FileInfo(pathPDF);
            deleteFile.Delete();

            return fileStreamResult;
        }

       
        
        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
