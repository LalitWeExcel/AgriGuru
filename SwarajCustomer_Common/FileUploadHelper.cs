using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace SwarajCustomer_Common
{
    public static class FileUploadHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ValidateFile(HttpContext httpContext, IList<string> allowedFileExtensions, int maxContentLength, string maxSizeText)
        {
            string message = "";
            try
            {
                var httpRequest = httpContext.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var postedFile = httpRequest.Files[0];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!allowedFileExtensions.Contains(extension))
                            message = "Please upload file of valid extension (" + string.Join(", ", allowedFileExtensions) + ")";
                        else if (postedFile.ContentLength > maxContentLength)
                            message = "File size is limited to " + maxSizeText;
                    }
                }
                else
                    message = "Please upload a file.";
            }
            catch (Exception ex)
            {
                message = "Error while upload a file.";
                log.Error("Error in ValidateFile", ex);
            }
            return message;
        }


        public static string ValidateVedioFile(HttpContext httpContext, IList<string> allowedFileExtensions, int maxContentLength, string maxSizeText,int index)
        {
            string message = "";
            try
            {
                var httpRequest = httpContext.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var postedFile = httpRequest.Files[index];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!allowedFileExtensions.Contains(extension))
                            message = "Please upload file of valid extension (" + string.Join(", ", allowedFileExtensions) + ")";
                        else if (postedFile.ContentLength > maxContentLength)
                            message = "File size is limited to " + maxSizeText;
                    }
                }
                else
                    message = "Please upload a file.";
            }
            catch (Exception ex)
            {
                message = "Error while upload a file.";
                log.Error("Error in ValidateFile", ex);
            }
            return message;
        }

        public static string ValidateMultiFiles(HttpContext httpContext, IList<string> allowedFileExtensions, int maxContentLength, string maxSizeText)
        {
            string message = "";
            try
            {
                var httpRequest = httpContext.Request;
                if (httpRequest.Files.Count > 0)
                {
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        var postedFile = httpRequest.Files[i];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!allowedFileExtensions.Contains(extension))
                            {
                                message = "Please upload files of valid extension (" + string.Join(", ", allowedFileExtensions) + ")";
                                break;
                            }
                            else if (postedFile.ContentLength > maxContentLength)
                            {
                                message = "File size is limited to " + maxSizeText;
                                break;
                            }
                        }
                    }
                }
                else
                    message = "Please upload a file.";
            }
            catch (Exception ex)
            {
                message = "Error while upload a file.";
                log.Error("Error in ValidateFile", ex);
            }
            return message;
        }
        public static bool UploadFile(HttpContext httpContext, string path, string filename, int is_full_path = 0)
        {
            bool uploaded = false;
            try
            {
                var httpRequest = httpContext.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var postedFile = httpRequest.Files[0];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string folder_path = is_full_path == 0 ? httpContext.Server.MapPath("~/" + path) : path;
                        Directory.CreateDirectory(folder_path);
                        string filePath = folder_path + "/" + filename;
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        postedFile.SaveAs(filePath);
                        uploaded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                uploaded = false;
                log.Error("Error in UploadFile", ex);
            }
            return uploaded;
        }
        public static bool UploadMultiFiles(HttpContext httpContext, string path, List<string> filenames, int is_full_path = 0)
        {
            bool uploaded = false;
            try
            {
                var httpRequest = httpContext.Request;
                if (httpRequest.Files.Count > 0)
                {
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        var postedFile = httpRequest.Files[i];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            string folder_path = is_full_path == 0 ? httpContext.Server.MapPath("~/" + path) : path;
                            Directory.CreateDirectory(folder_path);
                            string filePath = folder_path + "/" + filenames[i];
                            if (File.Exists(filePath))
                                File.Delete(filePath);
                            postedFile.SaveAs(filePath);
                            uploaded = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                uploaded = false;
                log.Error("Error in UploadFile", ex);
            }
            return uploaded;
        }
        public static string SaveHTMLtoPDF(string html, string pdf_save_path, string pdf_filename, string pdf_get_path)
        {
            string pdf_url = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(html) && !string.IsNullOrEmpty(pdf_save_path) && !string.IsNullOrEmpty(pdf_filename))
                {
                    pdf_save_path = HttpContext.Current.Server.MapPath(pdf_save_path);
                    Directory.CreateDirectory(pdf_save_path);
                    string filePath = pdf_save_path + pdf_filename;
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    File.WriteAllBytes(filePath, OpenHtmlToPdf.Pdf.From(html).Content());
                    pdf_url = pdf_get_path + pdf_filename;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in SaveHTMLtoPDF", ex);
            }
            return pdf_url;
        }
    }
}
