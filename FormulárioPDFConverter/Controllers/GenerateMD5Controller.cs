using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace FormulárioPDFConverter.Controllers
{
    public class GenerateMD5Controller : Controller
    {
        // GET: GenerateMD5/uploadFile
        public ActionResult uploadFile(string ID_Empresa)
        {
            if (string.IsNullOrEmpty(ID_Empresa))
            {
                return new HttpStatusCodeResult(400, "ID_Empresa is required");
            }

            var palavraChave = "ANFIR306";
            var combinacao = ID_Empresa + palavraChave;

            var hash = GenerateMD5(combinacao);

            ViewBag.MD5Hash = hash;

            return View("~/Views/Home/uploadFile.cshtml", hash);
        }

        private string GenerateMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

}
