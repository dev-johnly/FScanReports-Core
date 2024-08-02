using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace FScan.Reports.Infrastructure.Helpers
{
    public class PDFUtil
    {
        private IHostingEnvironment _env;

        public PDFUtil(IHostingEnvironment env)
        {
            this._env = env;
        }

        public MemoryStream CreatePdf(PdfPTable content, string filename)
        {
            MemoryStream workStream = new MemoryStream();
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            doc.Add(content);
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            System.IO.File.WriteAllBytes(filename, byteInfo);
            return workStream;
        }

        public MemoryStream CreatePdf(PdfPTable content, string filename, bool isShort, bool isLandScape)
        {
        

            try
            {
                MemoryStream workStream = new MemoryStream();
                Document doc = new Document();
                doc.SetMargins(0f, 0f, 20, 25);

                // Determine the page size based on isShort flag
                Rectangle pageSize = isShort ? PageSize.LETTER : PageSize.LEGAL;


                // Set the page size to portrait (no rotation needed)
                doc.AddTitle("Attendance Report");
                doc.SetPageSize(pageSize);
                PdfWriter writer = PdfWriter.GetInstance(doc, workStream);
                writer.CloseStream = false;
                doc.Open();

                var imagePath = Path.Combine(_env.ContentRootPath, "images", "LogoReport.png");

                Console.WriteLine($"Image path: {imagePath}");  // Log the image path

                if (File.Exists(imagePath))
                {
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
                    jpg.ScaleToFit(200f, 120f);
                    jpg.SpacingBefore = 1f;
                    jpg.SpacingAfter = 1f;
                    jpg.Alignment = Element.ALIGN_CENTER;

                    doc.Add(jpg);
                }
                else
                {
                    throw new FileNotFoundException("Image file not found", imagePath);
                }

                doc.Add(content);
                doc.Close();

                byte[] byteInfo = workStream.ToArray();
                File.WriteAllBytes(filename, byteInfo);

                return workStream;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }




        public PdfPTable AddContent(PdfPTable tableLayout, float[] headerSize, string reportTitle, int colSpan = 12)
        {
            tableLayout.SetWidths(headerSize);
            tableLayout.WidthPercentage = 90;
            tableLayout.HeaderRows = 1;
            tableLayout.AddCell(new PdfPCell(new Phrase(reportTitle, new Font(Font.FontFamily.HELVETICA, 12, 1, BaseColor.BLACK)))
            {
                Colspan = colSpan,
                Border = 0,
                PaddingTop = 0,
                HorizontalAlignment = Element.ALIGN_CENTER,
                MinimumHeight = 20,
            });
            return tableLayout;
        }

        public PdfPTable AddContentToCenter(PdfPTable tableLayout, float[] headerSize, string reportTitle, int colSpan)
        {
            tableLayout.SetWidths(headerSize);
            tableLayout.WidthPercentage = 50;
            tableLayout.HeaderRows = 1;
            tableLayout.AddCell(new PdfPCell(new Phrase(reportTitle, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
            {
                Colspan = colSpan,
                Border = 0,
                PaddingTop = 0,
                HorizontalAlignment = Element.ALIGN_CENTER,
                MinimumHeight = 5,
            });
            return tableLayout;
        }

        public PdfPTable AddNameHeader(PdfPTable tableLayout, float[] headerSize, string reportTitle)
        {
            tableLayout.SetWidths(headerSize);
            tableLayout.WidthPercentage = 50;
            tableLayout.HeaderRows = 1;
            tableLayout.AddCell(new PdfPCell(new Phrase(reportTitle, new Font(Font.FontFamily.HELVETICA, 9, 1, BaseColor.BLACK)))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT,
                MinimumHeight = 5,
            });
            return tableLayout;
        }

        public PdfPTable AddFooterCells(PdfPTable tableLayout, float[] headerSize, string reportTitle)
        {
            tableLayout.SetWidths(headerSize);
            tableLayout.WidthPercentage = 90;
            tableLayout.HeaderRows = 1;
            tableLayout.AddCell(new PdfPCell(new Phrase(reportTitle, new Font(Font.FontFamily.HELVETICA, 8, 0, BaseColor.BLACK)))
            {
                Colspan = 20,
                Border = 0,
                Padding = 5,
                PaddingBottom = 0,
                PaddingRight = 15,
                HorizontalAlignment = Element.ALIGN_CENTER,
                MinimumHeight = 5,
            });
            return tableLayout;
        }

        public PdfPTable AddCellHeader(PdfPTable tableLayout, string headerName, int colSpan)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(headerName, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                Colspan = colSpan
            });
            return tableLayout;
        }

        public PdfPTable AddCellToBody(PdfPTable table, float[] headerSize, string text, int colSpan)
        {
            table.AddCell(new PdfPCell(new Phrase(text, new Font(Font.FontFamily.HELVETICA, 8, 0, BaseColor.BLACK)))
            {
                Colspan = colSpan,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingBottom = 5,
                PaddingTop = 5,
                BackgroundColor = BaseColor.WHITE
            });
            return table;
        }

        public PdfPTable AddCellToBodyLeft(PdfPTable table, string text)
        {
            table.AddCell(new PdfPCell(new Phrase(text, new Font(Font.FontFamily.HELVETICA, 8, 0, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 5,
                PaddingTop = 5,
                BackgroundColor = BaseColor.WHITE
            });
            return table;
        }

        public PdfPTable AddCellToBodySubHeader(PdfPTable table, string text)
        {
            table.AddCell(new PdfPCell(new Phrase(text, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
            });
            return table;
        }
    }
}
