using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace mergepdf
{
    class Program
    {
        static void Main(string[] args)
        {
            var last = args.Count();
            MergePdf(File.OpenWrite(args[last-1]), args.Take(last-1));
        }

        public static void MergePdf(Stream outputPdfStream, IEnumerable<string> pdfFilePaths)
        {
            using (var document = new Document())
            using (var pdfCopy = new PdfCopy(document, outputPdfStream))
            {
                pdfCopy.CloseStream = false;
                try
                {
                    document.Open();
                    foreach (var pdfFilePath in pdfFilePaths)
                    {
                        using (var pdfReader = new iTextSharp.text.pdf.PdfReader(pdfFilePath))
                        {
                            pdfCopy.AddDocument(pdfReader);
                            pdfReader.Close();
                        }
                    }
                }
                finally
                {
                    document?.Close();
                }
            }
        }
    }
}
